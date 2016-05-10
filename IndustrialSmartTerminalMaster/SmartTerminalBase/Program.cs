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
using FluentNHibernate.Automapping;
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
            //Application.Run(new TestForm());
            //ThWarnNotifier p = new ThWarnNotifier();
            //p.CheckNumberOfInfo(20);
            Core p1 = new Core();
            p1.Run();
            Console.ReadLine();
            p1.Stop();
        }
    }

    
}