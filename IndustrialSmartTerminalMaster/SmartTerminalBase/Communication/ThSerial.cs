using System.IO.Ports;

namespace BaseClass.Communication
{
    internal class ThSerial : SerialBase
    {
        private readonly int baudrate;
        private readonly int databits;
        private readonly Parity parity;
        private readonly string portname;

        private SerialPort serialport;
        private readonly StopBits stopbits;
        /// <summary>
        /// Initialize port Setting
        /// </summary>
        /// <param name="PortName"></param>
        /// <param name="BaudRate"></param>
        /// <param name="DataBits"></param>
        /// <param name="stopBits"></param>
        /// <param name="pArity"></param>
        public ThSerial(string PortName, int BaudRate, int DataBits = 8, StopBits stopBits = StopBits.One,
            Parity pArity = Parity.None)
        {
            databits = DataBits;
            portname = PortName;
            baudrate = BaudRate;
            stopbits = stopBits;
            parity = pArity;
        }
        /// <summary>
        /// Check if the port is open
        /// </summary>
        public override bool IsOpen
        {
            get
            {
                if (serialport != null)
                    return serialport.IsOpen;
                return false;
            }
        }
        /// <summary>
        /// Set read timeout in millisecond
        /// </summary>
        public override int ReadTimeOut
        {
            get { return serialport.ReadTimeout; }
            set
            {
                if (serialport != null)
                    serialport.ReadTimeout = value;
            }
        }
        /// <summary>
        /// Set write timeot in millisecond
        /// </summary>
        public override int WriteTimeOut
        {
            get { return serialport.WriteTimeout; }
            set { serialport.WriteTimeout = value; }
        }

        ~ThSerial()
        {
            if (serialport != null)
            {
                serialport.Close();
            }
        }
        /// <summary>
        /// Open the port
        /// </summary>
        public override void OpenPort()
        {
            serialport = new SerialPort();
            serialport.DataBits = databits;
            serialport.PortName = portname;
            serialport.BaudRate = baudrate;
            serialport.StopBits = stopbits;
            serialport.Parity = parity;
            serialport.Open();
        }
        /// <summary>
        /// Close the port
        /// </summary>
        public override void ClosePort()
        {
            serialport.Close();
        }
        /// <summary>
        /// Reads a number of bytes from input buffer and writes to readbuffer at specific offset
        /// </summary>
        /// <param name="readbuffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void ReadPort(byte[] readbuffer, int offset, int count)
        {
            serialport.Read(readbuffer, offset, count);
        }
        /// <summary>
        /// Write to port using data from buffer
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void WriteToPort(byte[] cmd, int offset, int count)
        {
            serialport.Write(cmd, offset, count);
        }

        public override void Destory()
        {
            serialport.Dispose();
        }
    }
}