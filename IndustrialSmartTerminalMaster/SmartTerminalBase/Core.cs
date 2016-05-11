using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using NHibernate;
using S7_PPI_Space;
using SmartTerminalBase.Communication;
using SmartTerminalBase.DataBase;
using SmartTerminalBase.FileEditor;
using SmartTerminalBase.TerminalUltility;

namespace SmartTerminalBase
{

    class Core
    {
        private S7_PPI S7200;
        private Thread ReadThread;
        private ThPlcSnap7 s71200;
        private ThModbusObject dam3038;
        private Thread DataProcessThread;
        public ILog log;
        private Thread DataCenterThread;
        private ISessionFactory sessionFactory;
        private int warninfolength=0;
        private int warnstart;
        private string warnaddr;
        private static byte[] DBbuffer = new byte[256];
        private static byte[] Mbuffer = new byte[256];
        private IntPtr DBptr;
        private IntPtr Mptr;


        /// <summary>
        /// 程序启动
        /// </summary>

        public void Run()
        {   
            log = LogManager.GetLogger(typeof(Core));
            Init();
            ConnectToPlc();
          //  ConnectToSensor();
            ReadThread = new Thread(ReadCycle) { IsBackground = true };
            ReadThread.Start();
            log.Info("ReadThread" + TerminalCommon.LogMsgs["ThreadStart"]);
            DataProcessThread = new Thread(DataProcess) { IsBackground = true };
            DataProcessThread.Start();
            log.Info("DataProcessThread" + TerminalCommon.LogMsgs["ThreadStart"]);
            DataCenterThread = new Thread(DataCenterSave) { IsBackground = true };
            DataCenterThread.Start();
            log.Info("DataCenterThread" + TerminalCommon.LogMsgs["ThreadStart"]);
        }
        /// <summary>
        /// 初始化，读配置文件
        /// </summary>
        unsafe private void Init()
        {
            ThConfigFileManager manager = new ThConfigFileManager();
            TerminalCommon.LogMsgs = manager.ConvertToDictionary(manager.IniGetAllItems(Properties.Settings.Default.msg_cfg_path, "loginfo"));
            TerminalCommon.ConsoleMsgs = manager.ConvertToDictionary(manager.IniGetAllItems(Properties.Settings.Default.msg_cfg_path, "console"));
            TerminalCommon.PlcAddress = manager.ConvertToDictionary(manager.IniGetAllItems(Properties.Settings.Default.addr_cfg_path, "address"));
            Console.WriteLine(TerminalCommon.ConsoleMsgs["InitialSuccess"]);

            GetSessionFactory:
            {
                 try
                {
                    sessionFactory = FluentNHibernateHelper.GetSessionFactory();
                }
                catch(FluentNHibernate.Cfg.FluentConfigurationException)
                {
                    log.Error(TerminalCommon.LogMsgs["FluentCfgError"]);
                    Console.WriteLine(TerminalCommon.ConsoleMsgs["FluentCfgError"]);
                }                
            }
            Console.WriteLine("SessionFactory Got!");

            TerminalCommon.WarnInfo = manager.ConvertToDictionary(manager.IniGetAllItems(Properties.Settings.Default.addr_cfg_path, "warninfo"));
            var warnset = manager.IniGetAllItems(Properties.Settings.Default.addr_cfg_path, "warninforage");//DB1B0=2
            warnaddr =warnset[0].Split('=')[0];//DB1B0
            warnstart = Convert.ToInt32(warnaddr.Substring(warnaddr.Length - 1, 1));// 警告开始地址 0
            warnaddr = warnaddr.Substring(0, warnaddr.Length - 1);//警报区域信息 DB1B0
            warninfolength = Convert.ToInt32(warnset[0].Split('=')[1]);//报警数据的字节数 2
            TerminalCommon.warnflagname = Properties.Settings.Default.warnflag;
            Console.WriteLine(TerminalCommon.ConsoleMsgs["WarnMessageGot"]);

            DBptr = Marshal.UnsafeAddrOfPinnedArrayElement(DBbuffer, 0);
            Mptr = Marshal.UnsafeAddrOfPinnedArrayElement(Mbuffer, 0);//内存虚拟地址
            log.Debug("DBptr: " + DBptr + " Mptr: " + Mptr);



        }
        void ConnectToPlc()
        {
            s71200 = new ThPlcSnap7();
            if (s71200.ConnectTo(Properties.Settings.Default.plc_ip_s7200, 0, 0) == 0)
            {
                Console.WriteLine("S7-1200 " + TerminalCommon.ConsoleMsgs["ConnectSuccess"]);
            }
            //S7200 = new S7_PPI(Properties.Settings.Default.PPIport, 9600);
            //if (S7200.Connect(0x02))
            //{
            //    Console.WriteLine("S7-200 " + TerminalCommon.ConsoleMsgs["ConnectSuccess"]);
            //}
        }
        /// <summary>
        /// 连接MODBUS
        /// </summary>
        void ConnectToSensor()
        {
            dam3038 = new ThModbusObject();
            var master = dam3038.CreateRtuMaster(Properties.Settings.Default.ModbusPort);
        }
        /// <summary>
        /// 读数据
        /// </summary>
        void ReadCycle()
        {
            bool plcread = false;
            bool plcread_ppi = false;
            bool sensorread = false;
            byte[] buffer = new byte[256];
            while (true)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var plcdata = new PlcDAQCommunicationObject();
                plcread = Read1200(plcdata,buffer);
                //plcread_ppi = Read200(plcdata);
                //sensorread = Read3038(plcdata);  
             
                if (plcread || sensorread || plcread_ppi)
                {
                    Console.Beep(10000, 200);
                    TerminalQueues.plcdataprocessqueue.Enqueue(plcdata);                   
                    sw.Stop();
                    Console.WriteLine(TerminalCommon.ConsoleMsgs["Enqueue"] +" "+ sw.ElapsedMilliseconds + " ms");
                }
                if (sw.ElapsedMilliseconds >= 1000)
                    continue;
                else
                    Thread.Sleep((int) (1000 - sw.ElapsedMilliseconds));
            }
        }
        /// <summary>
        /// 将数据存入数据库
        /// </summary>
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
                            Console.WriteLine(TerminalCommon.ConsoleMsgs["DataCenterProcessed"]);
                        }
                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        log.Error(TerminalCommon.LogMsgs["DatabaseError"], ex);
                    }
                }
            }
        }
        /// <summary>
        /// 数据处理，报警信息处理
        /// </summary>
        void DataProcess()
        {
            PlcDAQCommunicationObject plcdata = new PlcDAQCommunicationObject();
            while (true)
            {
                if (TerminalQueues.plcdataprocessqueue.TryDequeue(out plcdata))
                {
                    WarnInfoProcess(plcdata);
                    Console.WriteLine(@"Data:");
                    foreach (var item in plcdata.plc_data)
                    {
                        Console.WriteLine(item.Key + @"=" + item.Value + " Processed");
                    }
                    foreach (var item in plcdata.temp_value)
                    {
                        Console.WriteLine(item.Key + @"=" + item.Value + "Processed");
                    }
                    Console.WriteLine("Time:" + plcdata.daq_time);
                    TerminalQueues.datacenterprocessqueue.Enqueue(plcdata);
                }
               
            }
        }
        /// <summary>
        /// 获得报警信息，并将其放入相应队列 
        /// </summary>
        /// <param name="plcdata"></param>
        void WarnInfoProcess(PlcDAQCommunicationObject plcdata)
        {
            string warnflagbyte = TerminalCommon.warnflagname.Split('_')[0];
            string warnflagbit = TerminalCommon.warnflagname.Split('_')[1];
            int bitnum = 1 << Convert.ToInt16(warnflagbit);
            if ((plcdata.plc_data[warnflagbyte] & bitnum) == bitnum)
            {
                PLCWarningObject plcwarn = new PLCWarningObject();
                plcwarn.warn_time = plcdata.daq_time;
                var results = GetWarnInfo(plcdata.plc_data);
                plcwarn.warndata = results;
                TerminalQueues.warninfoqueue.Enqueue(plcwarn);
                TerminalQueues.warninfoqueue_local.Enqueue(plcwarn);
            }
        }
        /// <summary>
        /// 获取地址对应的报警信息，以列表形式返回
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public List<string> GetWarnInfo(Dictionary<string, int> info)
        {
            var results = new List<string>();
            var i = 0;
            for (var j = 0; j <= warninfolength; j++)
            {
                string key = warnaddr + (warnstart + j).ToString();
                if (info[key] > 0)
                {
                    for (i = 0; i <= 7; i++)
                    {
                        if ((info[key] & (1 << i)) == 1 << i)
                        {
                            results.Add(TerminalCommon.WarnInfo[key + "_" + i]);
                        }
                    }
                }
            }
            return results;
        }
        /// <summary>
        /// 将接收到的byte数据按地址名称存入字典
        /// </summary>
        /// <param name="daq"></param>
        /// <param name="buffer"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="area"></param>
        /// <param name="block"></param>
        public static void BufferDump(PlcDAQCommunicationObject daq, byte[] buffer, int start, int size, string area,int block=1)
        {
            if (buffer == null)
                return;
            string key;
            string blockstr = null;
            int Wordlen = 1;
            if (area == "DB")
            {
                blockstr = block.ToString();
            }
            for (int i = 0; i < size; i = i + 1)
            {
                int value = 0;

                key = area + blockstr+TerminalCommon.wtype[Wordlen - 1] + (start + i).ToString();
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
        /// <summary>
        /// 停止所有线程
        /// </summary>
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
        /// <summary>
        /// Profinet读1200对应地址
        /// </summary>
        /// <param name="plcdata"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        bool Read1200(PlcDAQCommunicationObject plcdata, byte[] buffer)
        {
            if (!s71200.IsConneted())
            {
                s71200.Connect();
                Console.WriteLine("S7-1200 READ ERROR");
                log.Error("S7-1200 is not connected");
                return false;
            }
            else
            {
                int i=0;
                int readtimes = TerminalCommon.PlcAddress.Count();
                int[] starts = new int[readtimes];
                int[] sizes=new int[readtimes];
                bool isDbAddr=false,isMaddr=false;
                Snap7.S7Client.S7DataItem[] DataItem = new Snap7.S7Client.S7DataItem[readtimes];
                foreach (var item in TerminalCommon.PlcAddress)
                {
                    string[] range = item.Value.Split('-');
                    int start = Convert.ToInt32(range[0]);
                    int end = Convert.ToInt32(range[1]);
                    int size = end - start + 1;
                    starts[i]=start;
                    sizes[i]=size;

                    if (item.Key.Contains("DB"))
                    {
                       
                        DataItem[i].Area = ThPlcSnap7.S7AreaDB;
                        DataItem[i].DBNumber = 1;
                        DataItem[i].WordLen = ThPlcSnap7.S7WLByte;
                        DataItem[i].Start = start;
                        DataItem[i].Amount = size;
                        DataItem[i].pData = DBptr;
                        isDbAddr=true;
                        //if (s71200.ReadDb(ThPlcSnap7.S7AreaDB, 1, start, size, ThPlcSnap7.S7WLByte, buffer) == 0)
                        //{
                        //    //BufferDump(plcdata, buffer, start, size, "DB");
                        //}
                    }
                    else if (item.Key.Contains("M"))
                    {
                        DataItem[i].Area = ThPlcSnap7.S7AreaMK;
                        DataItem[i].DBNumber = 0;
                        DataItem[i].WordLen = ThPlcSnap7.S7WLByte;
                        DataItem[i].Start = start;
                        DataItem[i].Amount = size;
                        DataItem[i].pData = Mptr;
                        isMaddr = true;
                        //if (s71200.Read(ThPlcSnap7.S7AreaMK, start, size, ThPlcSnap7.S7WLByte, buffer) == 0)
                        //{
                        //    BufferDump(plcdata, buffer, start, size, "M");
                        //}
                    }
                    i++;
                }
                string plctime = s71200.PlCstatus();
                plcdata.plc_time["PLC_TIME"] = plctime;
                if (s71200.ReadMultiVars(DataItem, readtimes) == 0)
                {
                    for (i = 0; i < readtimes; i++)
                    {
                        if(isMaddr)   
                        BufferDump(plcdata, Mbuffer, starts[i], sizes[i], "M");
                        if(isDbAddr)
                        BufferDump(plcdata, DBbuffer, starts[i], sizes[i], "DB");
                    }
                    Console.WriteLine("S7-1200 READ");
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        /// <summary>
        /// MODBUS读采卡对应数据
        /// </summary>
        /// <param name="plcdata"></param>
        /// <returns></returns>
        bool Read3038(PlcDAQCommunicationObject plcdata)
        {
            try
                {
                    double ppiresult = S7200.Read(S7200.AreaV, 4000, S7200.LenW)/10.0d;
                    if (ppiresult != 0)
                    {
                        plcdata.temp_value["EM231:"] = ppiresult.ToString("0.0") + "℃";
                        Console.WriteLine("S7-200 READ");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("S7-200 READ ERROR");
                        return false;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("S7-200 READ ERROR");
                    log.Error("DAM3038 error", ex);
                    return false;
                }
        }
        /// <summary>
        /// PPI读S7200对应数据
        /// </summary>
        /// <param name="plcdata"></param>
        /// <returns></returns>
        bool Read200(PlcDAQCommunicationObject plcdata)
        {
            try
            {
                ushort[] buf = dam3038.ReadRegister(1, ThModbusObject.InputRegister, 256, 16);

                ushort[] buf2 = dam3038.ReadRegister(1, ThModbusObject.InputRegister, 399, 1);
                for (int i = 0; i < 16; i = i + 2)
                {
                    plcdata.temp_value["Sensor_" + (i / 2).ToString()] = (buf[i] / 65535.0d * 1300).ToString("0.00") + "℃";
                }
                plcdata.temp_value["Environment_temp"] = (buf2[0] / 10.0d - 273.15).ToString("0.00") + "℃";
                Console.WriteLine("DAM3038 READ");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAM3038 READ ERROR");
                log.Error("ModbusError:" + ex);
                return false;
            }
        }
    }


}
