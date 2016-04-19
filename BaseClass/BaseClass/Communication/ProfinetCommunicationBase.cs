using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClass.Communication
{
    class ProfinetCommunicationBase
    {
        public ProfinetCommunicationBase()
        {

        }
        protected object SetParameters(IntPtr Client, string Address,UInt16 LocalTSAP,UInt16 RemoteTSAP)
        {
            return true;
        }
        protected object Connect(string Address,int Rack,int Slot)
        {
            return true;
        }
        protected object Disconnect(object Client, string Address, UInt16 LocalTSAP, UInt16 RemoteTSAP)
        {
            return true;
        }
        protected object IsConneted(object Client)
        {
            return true;
        }
        protected object Destory(object Client)
        {
            return true;
        }
        protected object Read(string Area,int start,int Amount,int WordLen,byte[] buffer)
        {
            return true;
        }
        protected object Write(string Area, int start, int Amount, int WordLen, byte[] buffer)
        {
            return true;
        }
        protected object PLCstatus(object Client)
        {
            return true;
        }

    }
}
