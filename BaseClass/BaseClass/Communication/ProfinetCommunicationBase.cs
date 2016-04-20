using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClass.Communication
{
    public abstract class ProfinetCommunicationBase
    {
        public ProfinetCommunicationBase()
        {

        }
        public virtual object SetParameters(string Address,UInt16 LocalTSAP,UInt16 RemoteTSAP)
        {
            return true;
        }
        public virtual int ConnectTo(string Address,int Rack,int Slot)
        {
            return 0;
        }
        public virtual int ConnectTo_200(string Address, UInt16 LocalTSAP, UInt16 RemoteTSAP)
        {
            return 0;
        }
        public virtual int Connect()
        {
            return 0;
        }
        public virtual int Disconnect()
        {
            return 0;
        }
        public virtual bool IsConneted()
        {
            return true;
        }
        public virtual int PlcStop()
        {
            return 0;
        }
        public virtual int PlcHotStart()
        {
            return 0;
        }
        public virtual int PlcColdStart()
        {
            return 0;
        }
        public virtual object Destory(object Client)
        {
            Client = null;
            return true;
        }
        public virtual int Read(int Area,int start,int Amount,int WordLen,byte[] buffer)
        {
            return 0;
        }
        public virtual int ReadDB(int Area,int DBnumber, int start, int Amount, int WordLen, byte[] buffer)
        {
            return 0;
        }
        public virtual int Write(int Area, int start, int Amount, int WordLen, byte[] buffer)
        {
            return 0;
        }
        public virtual int WriteDB(int Area,int DBnmuber, int start, int Amount, int WordLen, byte[] buffer)
        {
            return 0;
        }
        public virtual string PLCstatus()
        {
            return "";
        }
        public virtual DateTime GetPlctime()
        {
            return DateTime.Today;
        }
        public virtual int SetPlctime(DateTime time)
        {
            return 0;
        }
        public virtual int ReadMultiVars(Snap7.S7Client.S7DataItem[] itemlist,int itemcount)
        {
            return 0;
        }
        public virtual int WriteMultiVars(Snap7.S7Client.S7DataItem[] itemlist,int itemcount)
        {
            return 0;
        }
    }
}
