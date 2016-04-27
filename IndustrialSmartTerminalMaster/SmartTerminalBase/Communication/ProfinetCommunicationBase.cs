using System;
using Snap7;

namespace SmartTerminalBase.Communication
{
    internal class ProfinetCommunicationBase
    {
        protected ProfinetCommunicationBase()
        {
        }

        public virtual object SetParameters(string address, ushort localTsap, ushort remoteTsap)
        {
            return true;
        }

        public virtual int ConnectTo(string address, int rack, int slot)
        {
            return 0;
        }

        public virtual int ConnectTo_200(string address, ushort localTsap, ushort remoteTsap)
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

        public virtual object Destory(object client)
        {
            client = null;
            return true;
        }

        public virtual int Read(int area, int start, int amount, int wordLen, byte[] buffer)
        {
            return 0;
        }

        public virtual int ReadDb(int area, int dBnumber, int start, int amount, int wordLen, byte[] buffer)
        {
            return 0;
        }

        public virtual int Write(int area, int start, int amount, int wordLen, byte[] buffer)
        {
            return 0;
        }

        public virtual int WriteDb(int area, int dBnmuber, int start, int amount, int wordLen, byte[] buffer)
        {
            return 0;
        }

        public virtual string PlCstatus()
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

        public virtual int ReadMultiVars(S7Client.S7DataItem[] itemlist, int itemcount)
        {
            return 0;
        }

        public virtual int WriteMultiVars(S7Client.S7DataItem[] itemlist, int itemcount)
        {
            return 0;
        }
    }
}