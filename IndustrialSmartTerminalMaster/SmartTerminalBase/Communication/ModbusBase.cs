using System.IO.Ports;
using Modbus.Device;

namespace SmartTerminalBase.Communication
{
    internal abstract class ModbusBase
    {
        public virtual int Retries { set; get; }

        protected object IsConnected
        {
            get { return true; }
        }

        public virtual int ReadTimeOut { set; get; }

        public virtual int WriteTimeOut { set; get; }

        protected virtual void ModbusRtuInit(string portName, int baudrate = 115200, Parity parity = Parity.None)
        {
        }

        protected virtual void TcpConnect(string ipAddress, int tcpPort)
        {
        }

        public virtual void TcpReconnect()
        {
        }

        public abstract IModbusMaster CreateRtuMaster(string portName, int baudrate = 115200, int dataBits = 8,
            StopBits stopbits = StopBits.One, Parity parity = Parity.None);

        public abstract IModbusMaster CreateTcpMaster(string ip, int tcpport);


        public object GetPortStatus(string port)
        {
            return true;
        }

        public virtual void Destory()
        {
        }

        public abstract ushort[] ReadRegister(byte slaveaddress, int registertype, ushort start, ushort amount);
        public abstract void WriteRegister(byte slaveaddress, int registertype, ushort start, ushort[] WriteData);
        public abstract bool[] ReadInOut(byte slaveaddress, int registertype, ushort start, ushort amount);
        public abstract void WriteCoils(byte slaveaddress, int type, ushort start, bool[] WriteData);
    }
}