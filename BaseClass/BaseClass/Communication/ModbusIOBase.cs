using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClass.Communication
{
    abstract class ModbusIOBase
    {
        String Connectiontype;
        public ModbusIOBase()
        {

        }
        protected virtual void ModbusRtuInit(string portName, int baudrate = 115200, Parity parity = Parity.None)
        {
            return;
        }

        protected virtual void TcpConnect(string ipAddress,int tcpPort)
        {
            return;
        }
        public virtual void TcpReconnect()
        {
            return;
        }
        public abstract IModbusMaster CreateRtuMaster(string portName, int baudrate = 115200, int DataBits = 8, StopBits stopbits = StopBits.One, Parity parity = Parity.None);
        public abstract IModbusMaster CreateTcpMaster(string ip, int tcpport);
        public virtual int retries
        {
            set;
            get;
        }
        protected object GetPortStatus(string port)
        {
            return true;
        }
        public virtual void Destory()
        {
            return;
        }
        protected object IsConnected
        {
            get { return true; }
        }
        public abstract ushort[] ReadRegister(byte slaveaddress, string registertype, int start, int amount);
        public abstract ushort[] WriteRegister(byte slaveaddress, string registertype, int start, byte[] WriteData);
        public override bool[] ReadInOut(byte slaveaddress, int type, ushort start, ushort amount);
        public override bool[] WriteInOut(byte slaveaddress, int type, ushort start, ushort amount, bool[] WriteData);

        public virtual int SetReadTimeOut
        {
            set
            {
                
            }
        }
        public virtual int SetReadWriteOut
        {
            set
            {
                
            }
        }
    }
}
