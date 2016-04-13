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



namespace BaseClass
{
    class ModbusObject
    {
        private SerialPort port;
        string ip;
        int tcpport;
        TcpClient tcpClient;

        /// <summary>
        /// Modbus TCP/RTU class
        /// </summary>
        public ModbusObject()
        {

        }
        /// <summary>
        /// Modbus RTU initialization
        /// </summary>
        /// <param name="slaveID"></param>
        /// <param name="portName"></param>
        /// <param name="baudrate"></param>
        /// <param name="parity"></param>
        public void modbusRTUinit(byte slaveID,string portName,int baudrate=115200,Parity parity=Parity.None)
        {
            port = new SerialPort();
            port.BaudRate = baudrate;
            port.DataBits = 8;
            port.Parity = parity;
            port.StopBits = StopBits.One;
            port.PortName = portName;
        }
        /// <summary>
        /// Modbus TCP initialization
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="tcpPort"></param>
        public void modbusTCPinit(string ipAddress,int tcpPort=502)
        {
            ip = ipAddress;
            tcpport = tcpPort;            
        }
        /// <summary>
        /// Create&Return a RTU master
        /// </summary>
        /// <returns></returns>
        public IModbusMaster CreateRtuMaster()
        {
            return ModbusSerialMaster.CreateRtu(port);
        }
        /// <summary>
        /// Create&Return Modbus TCP Master host.
        /// </summary>
        /// <returns></returns>
        public IModbusMaster CreateTCPMaster()
        {
            tcpClient = new TcpClient();
            tcpClient.BeginConnect(ip, tcpport, null, null);
            return ModbusIpMaster.CreateIp(tcpClient);

        }
    }
}
