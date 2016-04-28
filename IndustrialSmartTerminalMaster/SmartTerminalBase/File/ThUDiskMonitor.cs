using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SmartTerminalBase.FileEditor
{
    /// <summary>
    ///     Detect Flash Device
    ///     Copy the class Contents into your MainForm!
    /// </summary>
    public class UDiskMonitor : Form
    {
        public const int WmDevicechange = 0x219;
        public const int DbtDevicearrival = 0x8000;
        public const int DbtConfigchangecanceled = 0x0019;
        public const int DbtConfigchanged = 0x0018;
        public const int DbtCustomevent = 0x8006;
        public const int DbtDevicequeryremove = 0x8001;
        public const int DbtDevicequeryremovefailed = 0x8002;
        public const int DbtDeviceremovecomplete = 0x8004;
        public const int DbtDeviceremovepending = 0x8003;
        public const int DbtDevicetypespecific = 0x8005;
        public const int DbtDevnodesChanged = 0x0007;
        public const int DbtQuerychangeconfig = 0x0017;
        public const int DbtUserdefined = 0xFFFF;
        private bool _isInsert;
        private string _usbName;

        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == WmDevicechange)
                {
                    switch (m.WParam.ToInt32())
                    {
                        case WmDevicechange:
                            break;
                        case DbtDevicearrival: //U盘插入
                            var s = DriveInfo.GetDrives();
                            foreach (var drive in s)
                            {
                                if (drive.DriveType == DriveType.Removable)
                                {
                                    Trace.WriteLine(DateTime.Now + "--> U盘已插入，盘符为:" + drive.Name);
                                    _usbName = drive.Name;
                                    _isInsert = true;
                                    Thread.Sleep(1000);
                                    break;
                                }
                            }
                            break;
                        case DbtConfigchangecanceled:
                            break;
                        case DbtConfigchanged:
                            break;
                        case DbtCustomevent:
                            break;
                        case DbtDevicequeryremove:
                            break;
                        case DbtDevicequeryremovefailed:
                            break;
                        case DbtDeviceremovecomplete: //U盘卸载
                            Trace.WriteLine(DateTime.Now + "--> U盘已卸载！");
                            _isInsert = false;
                            break;
                        case DbtDeviceremovepending:
                            break;
                        case DbtDevicetypespecific:
                            break;
                        case DbtDevnodesChanged:
                            break;
                        case DbtQuerychangeconfig:
                            break;
                        case DbtUserdefined:
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            base.WndProc(ref m);
        }
    }
}