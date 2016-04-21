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
        
        public SerialBase()
        {

        }
        public virtual void OpenPort()
        {
            return;
        }
        public virtual void ClosePort()
        {
            return;
        }
        public virtual bool IsOpen
        {
            get;
            set;
        }

        public virtual int ReadTimeOut
        {
            set;
            get;
        }
        public virtual int WriteTimeOut
        {
            set;
            get;
        }

        public virtual void WriteToPort(byte[] cmd,int offset, int count)
        {
            return;
        }
        public virtual void ReadPort(byte[] readbuffer,int offset,int count)
        {
            return;
        }

        public virtual void  Destory()
        {

        }
        

    }
}
