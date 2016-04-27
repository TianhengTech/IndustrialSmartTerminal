using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using NHibernate;
using S7_PPI_Space;
using SmartTerminalBase.Communication;
using SmartTerminalBase.DataBase;
using SmartTerminalBase.FileEditor;
using SmartTerminalBase.TerminalUltility;
namespace SmartTerminalBase
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        //[STAThread]
        static void Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            MinimalSystem p1 = new MinimalSystem();
            p1.Run();
            Console.WriteLine();
            Console.ReadKey();
        }
    }
    #region minimalsystem
    class MinimalSystem
    {
        private S7_PPI S7200;
        private Thread ReadThread;
        private ThPlcSnap7 s71200;
        private ThModbusObject dam3038;
        private Thread DataProcessThread;
        public  ILog log;

        public void Run()
        {
            Init();
            ConnectToPlc();
            ConnectToSensor();
            ReadThread = new Thread(ReadCycle) { IsBackground = true };
            ReadThread.Start();
            DataProcessThread = new Thread(DataProcess) {IsBackground = true};
            DataProcessThread.Start();

        }
        private void Init()
        {              
            log=LogManager.GetLogger(typeof(MinimalSystem));  
            ThConfigFileManager manager = new ThConfigFileManager();
            TerminalCommon.PlcAddress = manager.ConvertToDictionary(manager.IniGetAllItems("./cfg.ini", "address"));
        }
        void ConnectToPlc()
        {
            s71200 = new ThPlcSnap7();
            s71200.ConnectTo("192.168.1.50", 0, 0);
            S7200 = new S7_PPI("COM4", 9600);
            S7200.Connect(0x02);
        }
        void ConnectToSensor()
        {
            dam3038 = new ThModbusObject();
            var master=dam3038.CreateRtuMaster(Properties.Settings.Default.ModbusPort);
            dam3038.ReadTimeOut= 1000;
        }
        void ReadCycle()
        {
           
            bool plcread = false;
            bool plcread_ppi = false;
            bool sensorread = false;
            byte[] buffer = new byte[256];           
            while (true)
            {
                var plcdata = new PlcDAQCommunicationObject();
                if (!s71200.IsConneted())
                {
                    s71200.Connect();
                    plcread = false;
                }
                else
                {
                    foreach (var item in TerminalCommon.PlcAddress)
                    {
                        string[] range = item.Value.Split('-');
                        int start = Convert.ToInt32(range[0]);
                        int end = Convert.ToInt32(range[1]);
                        int size = end - start + 1;
                        if (item.Key.Contains("DB"))
                        {
                            if (s71200.ReadDb(ThPlcSnap7.S7AreaDB, 1, start, size, ThPlcSnap7.S7WLByte, buffer) == 0)
                            {
                                BufferDump(plcdata, buffer, start, size, "DB");
                            }
                            string plctime = s71200.PlCstatus();
                            plcdata.plc_time["PLC_TIME"] = plctime;
                        } 
                     }
                    plcread = true;       
                }
                try
                {
                    double ppiresult = S7200.Read(S7200.AreaV, 4000, S7200.LenW)/10.0d;
                    plcdata.temp_value["EM231:"] = ppiresult.ToString("0.0")+"℃";
                    plcread_ppi = true;
                }
                catch
                {
                    plcread_ppi = false;
                }
                try
                {
                    ushort[] buf = dam3038.ReadRegister(1, ThModbusObject.InputRegister, 256, 16);
                    ushort[] buf2 = dam3038.ReadRegister(1, ThModbusObject.InputRegister, 399, 1);
                    for (int i = 0; i < 16; i = i + 2)
                    {
                        plcdata.temp_value["Sensor_" + (i / 2).ToString()] = (buf[i] / 65535.0d * 1300).ToString("0.00")+"℃";
                    }                  
                    plcdata.temp_value["Environment_temp"] = (buf2[0] / 10.0d - 273.15).ToString("0.00")+"℃";
                    sensorread = true;
                }
                catch (Exception ex)
                {
                    sensorread = false;
                    log.Error("ModbusError:"+ex);
                }
                if (plcread || sensorread||plcread_ppi)
                {
                    TerminalQueues.plcdataprocessqueue.Enqueue(plcdata);
                    TerminalQueues.datacenterprocessqueue.Enqueue(plcdata);
                }
               
                
                Thread.Sleep(1000);
            }
        }

        void DataCenterSave()
        {
            ISession session = FluentNHibernateHelper.GetSession();
            PlcDAQCommunicationObject plcdata = new PlcDAQCommunicationObject();
            while (true)
            {
                if (TerminalQueues.datacenterprocessqueue.TryDequeue(out plcdata))
                {
                    string jsonstring;
                    JsonConvert.SerializeObject(plcdata.plc_data);
                    historydata hi = new historydata
                    {
                        json_string =
                            JsonConvert.SerializeObject(plcdata.plc_data) +
                            JsonConvert.SerializeObject(plcdata.temp_value) +
                            JsonConvert.SerializeObject(plcdata.plc_time),
                        storetime = plcdata.daq_time
                    };
                    session.Save(hi);
                    session.Flush();
                }

            }

        }
        void DataProcess()
        {

            PlcDAQCommunicationObject plcdata = new PlcDAQCommunicationObject();
            while (true)
            {
                if(TerminalQueues.plcdataprocessqueue.TryDequeue(out plcdata))
                {
                    Console.WriteLine(@"Data:");
                    foreach (var item in plcdata.plc_data)
                    {
                        Console.WriteLine(item.Key+@"="+item.Value+" Processed");
                        
                    }
                   // Console.WriteLine("PLC_TIME:"+plcdata.plc_time["PLC_TIME"]);
                    foreach (var item in plcdata.temp_value)
                    {
                        Console.WriteLine(item.Key + @"=" + item.Value);
                    }
                    Console.WriteLine("Time:" + plcdata.daq_time);
                }
            }
        }

        static readonly char[] wtype = "BWD".ToCharArray();
        public static void BufferDump(PlcDAQCommunicationObject daq, byte[] buffer, int start, int size, string area)
        {
            if (buffer == null)
                return;
            int Wordlen = 1;
            string key;
            for (int i = 0; i < size; i = i + 1)
            {
                int value = 0;
                key = area + wtype[Wordlen - 1] + (start + i).ToString();
                byte[] bytes = new byte[Wordlen];
                Array.Copy(buffer, i, bytes, 0, Wordlen);
                Array.Reverse(bytes);
                for (int j = 0; j < Wordlen; j++)
                {
                    value = value + (bytes[j] << 8 * j);
                }
                daq.plc_data[key] = value;
            }
            return;
        }
    }

#endregion



}
