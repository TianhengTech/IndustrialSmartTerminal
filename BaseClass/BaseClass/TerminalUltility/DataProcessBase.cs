using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClass.TerminalUltility
{
    class DataProcessBase
    {
        public DataProcessBase()
        {

        }
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
        public virtual List<int> PlcDump(byte[] bytes, int start, int size, int Numofbyte)
        {
            List<int> mList = new List<int>();
            if (bytes == null)
                return mList;
            for (int i = 0; i < size; i = i + Numofbyte)
            {
                int value = 0;
                byte[] b = new byte[Numofbyte];
                for (int j = 0; j < Numofbyte; j++)
                {
                    value = value + (bytes[i + j] << (8 * (Numofbyte - j - 1)));
                    b[Numofbyte - j - 1] = bytes[i + j];
                    mList.Add(value);
                }
            }
            return mList;

        }
    }
}
