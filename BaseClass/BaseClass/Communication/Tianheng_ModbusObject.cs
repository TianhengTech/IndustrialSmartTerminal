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
        public static readonly int Input = 0x03;
        public static readonly int Coils = 0x04;
        #endregion

    }
}
