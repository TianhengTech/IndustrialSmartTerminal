using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClass.Communication
{
    class SerialBase
    {
        bool _keepreading;
        SerialPort serialport;
        public SerialBase(params object[] parameters)
        {
        }
        protected object OpenPort()
        {
            return true;
        }
        protected object ClosePort()
        {
            return true;
        }
        protected object IsOpen
        {
            get
            {
                return serialport.IsOpen;
            }
        }
        protected int SetReadTimeOut
        {
            set
            {
                serialport.ReadTimeout = value;
            }
        }
        protected int SetReadWriteOut
        {
            set
            {
                serialport.WriteTimeout = value;
            }
        }

        protected object SendCmd(byte[] cmd)
        {
            return true;
        }
        protected object ReadPort(byte[] readbuffer)
        {
            return true;
        }
        

    }
}
