namespace SmartTerminalBase.Communication
{
    internal class SerialBase
    {
        public virtual bool IsOpen { get; set; }

        public virtual int ReadTimeOut { set; get; }

        public virtual int WriteTimeOut { set; get; }

        public virtual void OpenPort()
        {
        }

        public virtual void ClosePort()
        {
        }

        public virtual void WriteToPort(byte[] cmd, int offset, int count)
        {
        }

        public virtual void ReadPort(byte[] readbuffer, int offset, int count)
        {
        }

        public virtual void Destory()
        {
        }
    }
}