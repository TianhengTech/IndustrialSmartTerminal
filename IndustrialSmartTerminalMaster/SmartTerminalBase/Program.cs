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
using System.Diagnostics;
using Modbus.IO;

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
            Console.ReadLine();
            p1.Stop();
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
        public ILog log;
        private Thread DataCenterThread;
        private ISessionFactory sessionFactory;

        public void Run()
        {
            log = LogManager.GetLogger(typeof(MinimalSystem));
            Init();
            ConnectToPlc();
            ConnectToSensor();
            ReadThread = new Thread(ReadCycle) {IsBackground = true};
            ReadThread.Start();
            DataProcessThread = new Thread(DataProcess) {IsBackground = true};
            DataProcessThread.Start();
            DataCenterThread = new Thread(DataCenterSave) {IsBackground = true};
            DataCenterThread.Start();
        }

        private void Init()
        {
            sessionFactory = FluentNHibernateHelper.GetSessionFactory();
            Console.WriteLine("SessionFactory Got!");

            ThConfigFileManager manager = new ThConfigFileManager();
            TerminalCommon.PlcAddress = manager.ConvertToDictionary(manager.IniGetAllItems("./cfg.ini", "address"));
            Console.WriteLine("Initialize finished!");
        }

        void ConnectToPlc()
        {
            s71200 = new ThPlcSnap7();
            if (s71200.ConnectTo("192.168.1.50", 0, 0) == 0)
            {
                Console.WriteLine("S7-1200 Connected!");
            }
            S7200 = new S7_PPI(Properties.Settings.Default.PPIport, 9600);
            if (S7200.Connect(0x02))
            {
                Console.WriteLine("S7-200 Connected");
            }
        }

        void ConnectToSensor()
        {
            dam3038 = new ThModbusObject();
            var master = dam3038.CreateRtuMaster(Properties.Settings.Default.ModbusPort);
        }

        void ReadCycle()
        {
            bool plcread = false;
            bool plcread_ppi = false;
            bool sensorread = false;
            byte[] buffer = new byte[256];
            while (true)
            {
                Stopwatch sw = new Stopwatch();
                Console.Beep();
                sw.Start();
                var plcdata = new PlcDAQCommunicationObject();
                if (!s71200.IsConneted())
                {
                    s71200.Connect();
                    plcread = false;
                    Console.WriteLine("S7-1200 READ ERROR");
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
                    Console.WriteLine("S7-1200 READ");
                    plcread = true;
                }
                try
                {
                    double ppiresult = S7200.Read(S7200.AreaV, 4000, S7200.LenW)/10.0d;
                    if (ppiresult != 0)
                    {
                        plcdata.temp_value["EM231:"] = ppiresult.ToString("0.0") + "℃";
                        plcread_ppi = true;
                        Console.WriteLine("S7-200 READ");
                    }
                    else
                    {
                        plcread = false;
                        Console.WriteLine("S7-200 READ ERROR");
                    }
                }
                catch
                {
                    plcread_ppi = false;
                    Console.WriteLine("S7-200 READ ERROR");
                }
                try
                {
                    ushort[] buf = dam3038.ReadRegister(1, ThModbusObject.InputRegister, 256, 16);

                    ushort[] buf2 = dam3038.ReadRegister(1, ThModbusObject.InputRegister, 399, 1);
                    for (int i = 0; i < 16; i = i + 2)
                    {
                        plcdata.temp_value["Sensor_" + (i/2).ToString()] = (buf[i]/65535.0d*1300).ToString("0.00") + "℃";
                    }
                    plcdata.temp_value["Environment_temp"] = (buf2[0]/10.0d - 273.15).ToString("0.00") + "℃";
                    sensorread = true;
                    Console.WriteLine("DAM3038 READ");
                }
                catch (Exception ex)
                {
                    sensorread = false;
                    Console.WriteLine("DAM3038 READ ERROR");
                    log.Error("ModbusError:" + ex);
                }
                if (plcread || sensorread || plcread_ppi)
                {
                    Console.Beep(10000, 200);
                    TerminalQueues.plcdataprocessqueue.Enqueue(plcdata);
                    TerminalQueues.datacenterprocessqueue.Enqueue(plcdata);
                    sw.Stop();
                    Console.WriteLine("Enqueue " + sw.ElapsedMilliseconds + " ms");
                }
                if (sw.ElapsedMilliseconds >= 1000)
                    continue;
                else
                    Thread.Sleep((int) (1000 - sw.ElapsedMilliseconds));
            }
        }

        void DataCenterSave()
        {
            PlcDAQCommunicationObject plcdata = new PlcDAQCommunicationObject();
            using (var session = sessionFactory.OpenSession())
            {
                while (true)
                {
                    try
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
                            using (var trans = session.BeginTransaction())
                            {
                                session.Save(hi);
                                trans.Commit();
                            }
                            Console.WriteLine("Data Center Processed!");
                        }
                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Data Center Error:" + ex);
                    }
                }
            }
        }

        void DataProcess()
        {
            PlcDAQCommunicationObject plcdata = new PlcDAQCommunicationObject();
            while (true)
            {
                if (TerminalQueues.plcdataprocessqueue.TryDequeue(out plcdata))
                {
                    Console.WriteLine(@"Data:");
                    foreach (var item in plcdata.plc_data)
                    {
                        Console.WriteLine(item.Key + @"=" + item.Value + " Processed");
                    }
                    // Console.WriteLine("PLC_TIME:"+plcdata.plc_time["PLC_TIME"]);
                    foreach (var item in plcdata.temp_value)
                    {
                        Console.WriteLine(item.Key + @"=" + item.Value + "Processed");
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
                    value = value + (bytes[j] << 8*j);
                }
                daq.plc_data[key] = value;
            }
            return;
        }

        public void Stop()
        {
            if (ReadThread.IsAlive)
            {
                ReadThread.Abort();
            }
            if (DataCenterThread.IsAlive)
            {
                DataCenterThread.Abort();
            }
            if (DataProcessThread.IsAlive)
            {
                DataProcessThread.Abort();
            }
            if (!sessionFactory.IsClosed)
            {
                sessionFactory.Close();
            }
        }
    }

    #endregion
}