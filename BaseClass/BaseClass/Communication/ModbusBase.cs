using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClass.Communication
{
    class ModbusBase
    {
        String Connectiontype;
        public ModbusBase()
        {

        }

        protected object TCPConnect(string ip,int port)
        {
            return true;
        }
        protected object RTUConnect(string port,params object[] list)
        {
            return true;
        }
        protected object GetPortStatus(string port)
        {
            return true;
        }
        protected object Disconnect()
        {
            return true;
        }
        protected object IsConnected()
        {
            return true;
        }
        protected object ReadRegister(int slaveaddress,string registertype,int start,int amount,byte[] ReadData)
        {
            return true;
        }
        protected object WriteRegister(int slaveaddress, string registertype,int start,byte[] WriteData)
        {
            return true;
        }

    }
}
