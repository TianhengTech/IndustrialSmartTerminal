using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Data;
using Modbus.Device;
using Modbus.Utility;
using System.IO.Ports;
using System.Net.Sockets;



namespace BaseClass.Communication
{
    class Tianheng_ModbusObject:ModbusIOBase
    {
        private SerialPort port;
        public TcpClient tcpClient;
        private IModbusMaster Master;
        string ipAddress;
        int tcpPort;

        /// <summary>
        /// Modbus TCP/RTU class
        /// </summary>
        public Tianheng_ModbusObject()
        {
        }
        /// <summary>
        /// Modbus RTU initialization
        /// </summary>
        /// <param name="slaveID"></param>
        /// <param name="portName"></param>
        /// <param name="baudrate"></param>
        /// <param name="parity"></param>
        protected override void ModbusRtuInit(string portName, int baudrate = 115200, Parity parity = Parity.None)
        {
            port = new SerialPort();
            port.BaudRate = baudrate;
            port.DataBits = 8;
            port.Parity = parity;
            port.StopBits = StopBits.One;
            port.PortName = portName;

        }
        /// <summary>
        /// Create&amp;Return a Rtu Master host;
        /// </summary>
        /// <returns></returns>
        public override IModbusMaster CreateRtuMaster(string portName, int baudrate = 115200,int DataBits=8,StopBits stopbits=StopBits.One, Parity parity = Parity.None)
        {
            port = new SerialPort();
            port.BaudRate = baudrate;
            port.DataBits = 8;
            port.Parity = parity;
            port.StopBits = StopBits.One;
            port.PortName = portName;
            port.Open();

            Master = ModbusSerialMaster.CreateRtu(port);
            return Master;
        }
        /// <summary>
        /// Start an Asyc Tcp Connect
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="tcpport"></param>
        protected override void TcpConnect(string ip, int tcpport)
        {
            tcpClient = new TcpClient();
            tcpClient.BeginConnect(ip, tcpport, null, null);
            return;
        }
        /// <summary>
        /// Try to reconnect 
        /// </summary>
        public override void TcpReconnect()
        {
            if (tcpClient != null && tcpClient.Connected == false)
            {
                tcpClient.BeginConnect(ipAddress, tcpPort, null, null);
                return;
            }
            else
                return;
        }
        /// <summary>
        /// Create&amp;Return Modbus TCP Master host.
        /// </summary>
        /// <returns></returns>
        public override IModbusMaster CreateTcpMaster(string ip,int tcpport)
        {
            ipAddress = ip;
            tcpPort = tcpport;
            TcpConnect(ip, tcpport);
            Master=ModbusIpMaster.CreateIp(tcpClient);
            return Master;
        }
        /// <summary>
        /// Read multiple registers
        /// </summary>
        /// <param name="slaveaddress"></param>
        /// <param name="registertype"></param>
        /// <param name="start"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public override ushort[] ReadRegister(byte slaveaddress, int registertype, ushort start, ushort amount)
        {
            switch(registertype)
            {
                case(0x01):
                    return Master.ReadHoldingRegisters(slaveaddress, start, amount);
                case(0x02):
                    return Master.ReadInputRegisters(slaveaddress, start, amount);
                default:
                    throw new TypeAccessException("Invalid type");
            }            
        }
        /// <summary>
        /// Read multiple coils/inputs
        /// </summary>
        /// <param name="slaveaddress"></param>
        /// <param name="type"></param>
        /// <param name="start"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public override bool[] ReadInOut(byte slaveaddress, int type, ushort start, ushort amount)
        {
            switch (type)
            {
                case (0x03):
                    return Master.ReadInputs(slaveaddress, start, amount);
                case (0x04):
                    return Master.ReadCoils(slaveaddress, start, amount);
                default:
                    throw new TypeAccessException("Invalid type");
            }
        }
        /// <summary>
        /// Write 1 to 123 contiguous registers
        /// </summary>
        /// <param name="slaveaddress"></param>
        /// <param name="registertype"></param>
        /// <param name="start"></param>
        /// <param name="WriteData"></param>
        public override void WriteRegister(byte slaveaddress, int registertype, ushort start, ushort[] WriteData)
        {
            switch (registertype)
            {
                case (0x01):
                    Master.WriteMultipleRegisters(slaveaddress, start, WriteData);
                    return;
                case (0x02):
                    Master.WriteMultipleRegisters(slaveaddress, start, WriteData);
                    return;
                default:
                    throw new TypeAccessException("Invalid type");
            }       
        }
        /// <summary>
        /// Write a sequence of coils
        /// </summary>
        /// <param name="slaveaddress"></param>
        /// <param name="type"></param>
        /// <param name="start"></param>
        /// <param name="WriteData"></param>
        public override void WriteCoils(byte slaveaddress, int type, ushort start, bool[] WriteData)
        {
            switch (type)
            {
                case (0x04):
                    Master.WriteMultipleCoils(slaveaddress, start, WriteData);
                    return;
                default:
                    throw new TypeAccessException("Invalid type");
            }
        }
        /// <summary>
        /// Read timeout in milliseconds
        /// </summary>
        public override int ReadTimeOut
        {
            get
            {
                return Master.Transport.ReadTimeout;
            }
            set
            {
                Master.Transport.ReadTimeout = value;
            }
        }
        /// <summary>
        /// Write timeout in milliseconds
        /// </summary>
        public override int WriteOut
        {
            get
            {
                return Master.Transport.WriteTimeout;
            }
            set
            {
                Master.Transport.WriteTimeout = value;
            }
        }
       /// <summary>
       /// Number of times to retry sending a message after encountering an faliure
       /// </summary>
        public override int retries
        {
            get
            {
                return Master.Transport.Retries;
            }
            set
            {
                Master.Transport.Retries = value;
            }
        }
        /// <summary>
        /// Dispose the object master
        /// </summary>
        public override void Destory()
        {
            Master.Dispose();
            return;
        }

        #region [Constants]
        public static readonly int HoldingRegister = 0x01;
        public static readonly int InputRegister = 0x02;
        public static readonly int Inputs = 0x03;
        public static readonly int Coils = 0x04;
        #endregion

    }
}
