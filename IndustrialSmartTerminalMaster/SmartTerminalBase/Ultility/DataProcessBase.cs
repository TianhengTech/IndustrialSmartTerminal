using System.Collections.Generic;

namespace SmartTerminalBase.TerminalUltility
{
    internal class DataProcessBase
    {
        public virtual object HexToByte(string hex)
        {
            return true;
        }

        public virtual object ByteToSingle(byte[] bytes)
        {
            return true;
        }

        public virtual object ByteToString(byte[] bytes)
        {
            return true;
        }

        public virtual string DumpToJson(object obj)
        {
            return "";
        }

        public virtual t LoadFromJson<t>(string jsonstr)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<t>(jsonstr);
        }
        public virtual List<int> PlcDump(byte[] bytes, int start, int size, int Numofbyte)
        {
            var mList = new List<int>();
            if (bytes == null)
                return mList;
            for (var i = 0; i < size; i = i + Numofbyte)
            {
                var value = 0;
                var b = new byte[Numofbyte];
                for (var j = 0; j < Numofbyte; j++)
                {
                    value = value + (bytes[i + j] << (8*(Numofbyte - j - 1)));
                    b[Numofbyte - j - 1] = bytes[i + j];
                    mList.Add(value);
                }
            }
            return mList;
        }
    }
}