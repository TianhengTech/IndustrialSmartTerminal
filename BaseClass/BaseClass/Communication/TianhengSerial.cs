using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClass.Communication
{
    class TianhengSerial:SerialBase
    {

        SerialPort serialport;
        int databits;
        string portname;
        int baudrate;
        StopBits stopbits;
        Parity parity;

        public TianhengSerial(string PortName, int BaudRate, int DataBits = 8, StopBits stopBits = StopBits.One, Parity pArity = Parity.None)
        {
            databits =DataBits;
            portname = PortName;
            baudrate = BaudRate;
            stopbits = stopBits;
            parity = pArity;
        }
        ~TianhengSerial()
        {
            if(serialport!=null)
            {
                serialport.Close();
            }
        }
        public override void OpenPort()
        {
            serialport = new SerialPort();
            serialport.DataBits = databits;
            serialport.PortName = portname;
            serialport.BaudRate = baudrate;
            serialport.StopBits = stopbits;
            serialport.Parity = parity;
            serialport.Open();
        }
        public override bool IsOpen
        {
            get
            {
                if(serialport!=null)
                return serialport.IsOpen;
                return false;
            }
        }
        public override int ReadTimeOut
        {
            get
            {                
                return serialport.ReadTimeout;
            }
            set
            {
                if(serialport!=null)
                serialport.ReadTimeout = value;
                return;
            }
        }
        public override int WriteTimeOut
        {
            get
            {
                return serialport.WriteTimeout;
            }
            set
            {
                serialport.WriteTimeout = value;
            }
        }
        public override void ClosePort()
        {
            serialport.Close();
        }
        public override void ReadPort(byte[] readbuffer,int offset,int count)
        {
            serialport.Read(readbuffer, offset, count);
        }
        public override void WriteToPort(byte[] cmd,int offset,int count)
        {
            serialport.Write(cmd,offset,count);
        }
        public override void Destory()
        {
            serialport.Dispose();
        }

    }
}
