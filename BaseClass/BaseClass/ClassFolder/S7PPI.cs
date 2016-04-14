using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

/*
S7-200
04-S 05-SM 06-AI 07-AQ 1E-C 81-I 82-Q 83-M 184-V 1F-T
读写函数前三个参数为：存储区类型（如上），相应存储区开始地址，数据长度或数据
CT为16位数据，其它的可以是8位到32位数据
*/

namespace S7_PPI_Space
{
    class S7_PPI
    {
        // Area ID
        public  int AreaI = 0x81;
        public  int AreaQ = 0x82;
        public  int AreaM = 0x83;
        public  int AreaV = 0x184;
        public  int AreaC = 0x1E;
        public  int AreaT = 0x1F;
        public  int AreaAI = 0x06;//只读

        public int LenB = 0x01;
        public int LenW = 0x02;
        public int LenD = 0x04;

        private bool Debug = false;
        private static byte[] result = new byte[1024];
        public SerialPort SPort=new SerialPort("COM1", 9600, Parity.Even, 8, StopBits.One);
        private int Daddr;
        public S7_PPI(String PortName, int BaudRate)
        {
            this.SPort.PortName = PortName;
            this.SPort.BaudRate = BaudRate;
            this.SPort.ReadTimeout = 2000;
        }
		//连接服务器
        public bool Creat_Com()
        {       
            try{
                this.SPort.Open();
                Thread.Sleep(200);
				if(true) Trace.WriteLine("端口打开成功！");
                return true;
            }
            catch{
                if (true) Trace.WriteLine("端口打开失败！");
                return false;
            }
        }
        private string HexStringToString(string str)
        {
            byte[] b = new byte[str.Length / 2];
            //逐个字符变为16进制字节数据
            for (int i = 0; i < str.Length/2; i++)
            {
                b[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }
            //按照指定编码将字节数组变为字符串
            return System.Text.Encoding.Default.GetString(b);
        }
        public String HexToStr(int argv,int Len){
            String Str=argv.ToString("X");
            if (Str.Length<Len){
                for(int i=Str.Length;i<Len;i++)
                    Str="0"+Str;
            }
            return Str;
        }
        public static byte[] Hex2Bytes(string hex){
            byte[] result = new byte[hex.Length / 2];
            for (int i = 0; i < result.Length; i++)
                result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return result;
        }
        public static string BytesToHex(byte[] bytes) {
            return BytesToHex(bytes, bytes.Length);
        }
        public static string BytesToHex(byte[] bytes,int Len)
        {
            StringBuilder result = new StringBuilder();
            if (bytes != null)
            {
                for (int i = 0; i < Len; i++)
                {
                    result.Append(bytes[i].ToString("X2"));
                }
            }
            return result.ToString();
        }
		//通过 Com 发送数据
        public string Send_Cmd(string Cmd)
        {
            if (Debug) Trace.WriteLine("服务器发送消息：" + BytesToHex(Hex2Bytes(Cmd)));
            string VerifyCmd  = "10"+HexToStr(Daddr,2)+"005C"+HexToStr((Daddr+0x5C)%0x100,2)+"16";
            int receiveLength=0;
            try{
                SPort.Write(Hex2Bytes(Cmd), 0,Hex2Bytes(Cmd).Length);
                if (SPort.ReadByte() == 0xe5)
                {
                    if (Debug) Trace.WriteLine("E5 OK");
                    SPort.Write(Hex2Bytes(VerifyCmd), 0, Hex2Bytes(VerifyCmd).Length);
                    //Thread.Sleep(100);
                    //receiveLength = SPort.Read(result, 0, result.Length);                  
                    if (SPort.ReadByte() == 0x68)
                    {
                        receiveLength = SPort.ReadByte()+4;
                        for (int i = 0; i < receiveLength; i++)
                        {
                            result[i] = (byte)SPort.ReadByte();
                        }
                    }
                }
                if (Debug) Trace.WriteLine("服务器接收消息：" + BytesToHex(result, receiveLength));
            }
            catch (TimeoutException e)
            {
                Trace.WriteLine("TimeoutException:" + e);
            }

            if (receiveLength != 0)
            {
                return BytesToHex(result, receiveLength);
            }
            return "0000";
		}

        public bool Connect(int Daddr){
            this.Daddr = Daddr;
           return Creat_Com();
        }
        //只适用于S7-200
        public bool Set_Cpu_State(string State)
        { 
            string Cmd="",Str;
        
            if(State=="Run")
                 Cmd= "68212168"+HexToStr(Daddr,2)+"006C3201000000000014000028000000000000FD000009505F50524F4752414D"+HexToStr((Daddr+0xA8)%0x100,2)+"16";
            else if(State=="Stop")
                 Cmd= "681D1D68"+HexToStr(Daddr,2)+"006C3201000000000010000029000000000009505F50524F4752414D"+HexToStr((Daddr+0xA8)%0x100,2)+"16";
            else
                Trace.WriteLine( "CmdError");
            Str = Send_Cmd(Cmd);
            return true;
        }
        public string Get_Cpu(){
            string GetCPUcmd = "681B1B68" + HexToStr(Daddr, 2) + "007C320100000000000E00000401120A10020014000003000000" + HexToStr((Daddr + 0x07) % 0x100, 2) + "16";
            string Str = Send_Cmd(GetCPUcmd);
            if (Str.Length - 44>0)
                Str=Str.Substring(Str.Length-44,40);
            return HexStringToString(Str);
        }
        public int Read_Bit(int type, int addr, int bitaddr)
        {
            if (bitaddr<=7)
                return (Read(type, addr,1) >> bitaddr) & 0x01;
            Trace.WriteLine("bitaddr ERROR");
            return 0;
        }
        public int Read(int type, int addr)
        {
            return Read(type, addr, 1);
        }
        public int Read(int type,int addr,int size){
            int Len;
            string readCmd,rStr;
			addr=addr*8;
            if (type==0x1E||type==0x1F){
                Len=type;
            }
            else{
                Len=((int)Math.Log(size,2)+1)*2;
            }
            readCmd="681B1B68"+HexToStr(Daddr,2)+"006C32010000AAAA000E00000401120A10";
            readCmd = readCmd + HexToStr(Len, 2) + "000100" + HexToStr(type, 4) + HexToStr(addr, 6) +
                HexToStr((0x33 + Daddr + Len + (type >> 8) % 0x100 + type % 0x100 + (addr >> 16) % 0x100 + (addr >> 8) % 0x100 + addr % 0x100) % 0x100, 2) + "16";
             // Trace.WriteLine(readCmd);
             rStr=Send_Cmd(readCmd);
            //Trace.WriteLine(rStr);
             if (rStr != "")
             {
                 if (rStr.Length - (size + 2) * 2 > 0)
                 {
                    if (type==0x1E || type==0x1F)
                        rStr=rStr.Substring(rStr.Length-8,4);
                    else
                        rStr=rStr.Substring(rStr.Length-(size+2)*2,2*size);
                    //Trace.WriteLine(rStr);
                     return Convert.ToInt32(rStr,16);
                 }
            }
            return 0;
        }
        public int[,] Reads(int[,] AreaPar)
        {
            int  i,j;
            int Sets = AreaPar.Length / 4;
            int[] rBytes = new int[200];
            int[,] AreaR=new int[100,100];
            int TLen = 15 + 12 * Sets;
            int Len = 0, type,addr,size, sizeLength;
            string readCmd, rStr;
            int checkout;
            readCmd = "68" + HexToStr(TLen, 2) + "0068" + HexToStr(Daddr, 2) + "006C32010000AAAA00" + HexToStr(Sets * 12 + 2, 2) + "000004" + HexToStr(Sets, 2);
            checkout = (Daddr + 0x6C + 0x32 + 0x01 + 0xAA * 2 + Sets * 12 + 2 + 0x04 + Sets) % 0x100;
            for(j=0;j<Sets;j++)
            {
                type = AreaPar[j,0];
                addr = AreaPar[j,1];
                size = AreaPar[j,2];
                sizeLength = AreaPar[j,3];

                addr = addr * 8;
                if (type == 0x1E || type == 0x1F)
                {
                    Len = type;
                }
                else
                {
                    Len = ((int)Math.Log(size, 2) + 1) * 2;
                }
                readCmd+= "120A10" + HexToStr(Len, 2) + "00" + HexToStr(sizeLength, 2) + "00" + HexToStr(type, 4) + HexToStr(addr, 6);
                checkout = (checkout + 0x12 + 0x0A + 0x10 + Len + sizeLength + (type >> 8) % 0x100 + type % 0x100 + (addr >> 16) % 0x100 + (addr >> 8) % 0x100 + addr % 0x100) % 0x100;
            }
            readCmd = readCmd + HexToStr(checkout, 2) + "16";
            //Trace.WriteLine(readCmd);
            rStr = Send_Cmd(readCmd);
#if true
            if (rStr != "" && rStr.Length>46)
            {
                rStr = rStr.Substring(46, rStr.Length-46);
                for (j = 0; j < Sets; j++)
                {
                    type = AreaPar[j, 0];
                    
                    sizeLength = AreaPar[j, 3];

                    if (type == 0x1E) size = 3;
                    else if (type == 0x1F) size = 5;
                    else size = AreaPar[j, 2];
                    if(Debug) Trace.WriteLine(rStr);
                    if (Debug)  Trace.Write("第" + j + "行：");
                    for (i = 0; i < sizeLength; i++)
                    {
                        AreaR[j, i] = Convert.ToInt32(rStr.Substring(2 * size * i, 2 * size), 16);
                        if (Debug) Trace.Write(AreaR[j, i] + " ");
                    }
                    if (j < Sets-1)
                    {
                        rStr = rStr.Substring(2 * size * sizeLength , rStr.Length - 2 * size * sizeLength );
                        if (rStr.Substring(0, 2) != "FF")
                            rStr = rStr.Substring(2, rStr.Length - 2);
                        rStr = rStr.Substring( 8, rStr.Length - 8);
                    }

                    if (Debug) Trace.WriteLine("");
                }

            }
#endif
            
            return AreaR;
        }

        public int[] Reads(int type, int addr, int size,int sizeLength)
        {
            int[] rBytes=new int[200];
            int Len;
            string readCmd, rStr;
            addr = addr * 8;
            if (type == 0x1E || type == 0x1F)
            {
                Len = type;
            }
            else
            {
                Len = ((int)Math.Log(size, 2) + 1) * 2;
            }
            readCmd = "681B1B68" + HexToStr(Daddr, 2) + "006C32010000AAAA000E00000401120A10";
            readCmd = readCmd + HexToStr(Len, 2) + "00" + HexToStr(sizeLength, 2) + "00" + HexToStr(type, 4) + HexToStr(addr, 6) +
                HexToStr((0x32 + Daddr + Len + sizeLength + (type >> 8) % 0x100 + type % 0x100 + (addr >> 16) % 0x100 + (addr >> 8) % 0x100 + addr % 0x100) % 0x100, 2) + "16";
            rStr = Send_Cmd(readCmd);
            if (rStr != "")
            {
                if (rStr.Length - (size + 2) * 2 > 0)
                {

                    for (int i = 0; i < sizeLength; i++)
                    {
                        if (type == 0x1E)
                            rBytes[i] = Convert.ToInt32(rStr.Substring(rStr.Length - 6 * (sizeLength - i)-4, 3 * size), 16);
                        else if(type == 0x1F)
                            rBytes[i] = Convert.ToInt32(rStr.Substring(rStr.Length - 10 * (sizeLength - i) - 4, 5 * size), 16);
                        else
                            rBytes[i] = Convert.ToInt32(rStr.Substring(rStr.Length - size * 2 * (sizeLength - i) - 4, 2 * size), 16); 
                        Trace.Write(rBytes[i]+" ");
                    }
                }
            }
            Trace.WriteLine("");
            return rBytes;
        }
        public int SizeOfInt(int argv){
            int len=1;
            while(argv/0x100!=0){
                argv=argv/0x100;
                len=len+1;
            }
            return len;
         }
        public bool Write_Bit(int type, int addr, int bitaddr, int data)
        {
            if (data >= 1) data = 1;
            else data = 0;
            return Write(type, addr, data,1,bitaddr);
        }
        public bool Write(int type, int addr, int data)
        {
            return Write(type, addr, data, 1, 8);
        }
        public bool Write( int type, int addr, int size,int data) { 
             return Write( type,  addr,  data, size, 8);
        }
        //size 只能为1，2，4，表示Byte，Word，Double Word
        public bool Write(int type, int addr, int data, int size, int bitaddr)
        {
            int Len;
            string WriteCmd,rStr;
			addr=addr*8;
            if (type==0x1E){
                Len=type;
				WriteCmd="68232368"+HexToStr(Daddr,2)+"007C320100009425000E00080501120A10";
				WriteCmd=WriteCmd+HexToStr(Len,2)+"000100"+HexToStr(type,4)+HexToStr(addr,6)+"0004001800"+HexToStr(data,4)+"00"+
					HexToStr((Daddr+0x1B0+Len+0x01+0x1C+(type>>8)%0x100+type%0x100+(addr>>16)%0x100+(addr>>8)%0x100+addr%0x100+(data>>8)%0x100+data%0x100)%0x100,2)+ "16";
            } 
            else if(type==0x1F){
                Len=type;
                WriteCmd="68252568"+HexToStr(Daddr,2)+"007C3201000079AC000E000A0501120A10";
				WriteCmd=WriteCmd+HexToStr(Len,2)+"000100"+HexToStr(type,4)+HexToStr(addr,6)+"00040028000000"+HexToStr(data,4)+"00"+
					HexToStr((Daddr+0x21E+Len+0x01+0x2C+(type>>8)%0x100+type%0x100+(addr>>16)%0x100+(addr>>8)%0x100+addr%0x100+(data>>8)%0x100+data%0x100)%0x100,2)+ "16";
            }
            else{
                WriteCmd = "68" + HexToStr(size + 0x1F, 2) + "0068" + HexToStr(Daddr, 2) + "006C32010000D1D1000E" + HexToStr(size+0x04, 4) + "0501120A10";
                if (bitaddr >= 8)
                {
                    Len = 2;
                    WriteCmd = WriteCmd + HexToStr(Len, 2)  + HexToStr(size, 4) + "00" + HexToStr(type, 4) + HexToStr(addr, 6) + "0004" + HexToStr(size*8, 4) + HexToStr(data, size*2) +
                        HexToStr((Daddr+0x24F + size+0x04+0x32  + Len +size+ (type >> 8) % 0x100 + type % 0x100 + (addr >> 16) % 0x100 + (addr >> 8) % 0x100 + addr % 0x100 +0x04+size*8+
                        (data >> 24) % 0x100+(data >> 16) % 0x100 + (data >> 8) % 0x100 + data % 0x100) % 0x100, 2) + "16";
                }
                else {
                    Len = 1;
                    addr = addr + bitaddr;
                    WriteCmd = WriteCmd + HexToStr(Len, 2) + "000100" + HexToStr(type, 4) + HexToStr(addr, 6) + "00030001" + HexToStr(data, 2) +
                        HexToStr((0x27E + Daddr + 0x01 + 0x0C + Len + (type >> 8) % 0x100 + type % 0x100 + (addr >> 16) % 0x100 + (addr >> 8) % 0x100 + addr % 0x100 + data) % 0x100, 2) + "16";
                }
            }
              //Trace.WriteLine(WriteCmd);
             rStr=Send_Cmd(WriteCmd);
            //Trace.WriteLine(rStr);
             if (rStr.Length - 6 > 0) {
                 if (rStr.Substring(rStr.Length - 6, 2) == "FF") return true;
                 else Trace.WriteLine("错误代码:" + rStr.Substring(rStr.Length - 6, 2));
             }
             return false;
        }    
    }
}
