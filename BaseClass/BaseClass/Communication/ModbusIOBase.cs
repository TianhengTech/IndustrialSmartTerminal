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
        public abstract ushort[] ReadRegister(byte slaveaddress, int registertype, ushort start, ushort amount);
        public abstract void WriteRegister(byte slaveaddress, int registertype, ushort start, ushort[] WriteData);
        public abstract bool[] ReadInOut(byte slaveaddress, int registertype, ushort start, ushort amount);
        public abstract void WriteCoils(byte slaveaddress, int type, ushort start, bool[] WriteData);

        public virtual int ReadTimeOut
        {
            set;
            get;
        }
        public virtual int WriteOut
        {
            set;get;
        }
    }
}
