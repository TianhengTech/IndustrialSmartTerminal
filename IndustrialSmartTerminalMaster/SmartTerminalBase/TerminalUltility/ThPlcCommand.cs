namespace SmartTerminalBase.TerminalUltility
{
    //定义PLC数据格式
    internal class PlcCommand
    {
        /*
        S7-200
        04-S 05-SM 06-AI 07-AQ 1E-C 81-I 82-Q 83-M 184-V 1F-T
        读写函数前三个参数为：存储区类型（如上），相应存储区开始地址，数据长度或数据
        CT为16位数据，其它的可以是8位到32位数据

        S7-1200
        81-I 82-Q 83-M 84-D
        读写函数前三个参数为：存储区类型（如上），相应存储区开始地址，数据长度或数据
        D区调用需要加为DB的那个块，如程序中DB3则为0x84+0x300
        S7S.Get_Cpu_State() 只适用于S7-1200
        */
        private string _area;
        private int _bitaddr;
        private string _type;
        public int addr;
        public int data;

        /// <summary>
        ///     Creat a PLC command
        /// </summary>
        /// <param name="input_area"></param>
        /// <param name="input_type"></param>
        /// <param name="input_data"></param>
        /// <param name="input_addr"></param>
        /// <param name="input_bitaddr"></param>
        public PlcCommand(string input_area, string input_type, int input_data, int input_addr, int input_bitaddr = 0)
        {
            area = input_area;
            type = input_type;
            data = input_data;
            addr = input_addr;
            bitaddr = input_bitaddr;
        }

        public string area
        {
            get { return _area; }
            set
            {
                if (value == TerminalCommon.S7200AreaS || value == TerminalCommon.S7200AreaSM ||
                    value == TerminalCommon.S7200AreaAI
                    || value == TerminalCommon.S7200AreaAQ || value == TerminalCommon.S7200AreaC ||
                    value == TerminalCommon.S7200AreaI
                    || value == TerminalCommon.S7200AreaQ || value == TerminalCommon.S7200AreaM ||
                    value == TerminalCommon.S7200AreaV
                    || value == TerminalCommon.S7200AreaT)
                {
                    _area = value;
                }
                else
                {
                    _area = "null";
                }
            }
        }

        public string type
        {
            get { return _type; }
            set
            {
                if (value == TerminalCommon.S7200DataByte || value == TerminalCommon.S7200DataBit ||
                    value == TerminalCommon.S7200DataWord)
                {
                    _type = value;
                }
            }
        }

        public int bitaddr
        {
            get { return _bitaddr; }
            set
            {
                if (value >= 0 && value <= 7)
                {
                    _bitaddr = value;
                }
            }
        }
    }
}