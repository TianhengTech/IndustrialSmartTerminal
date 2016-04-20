using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snap7;
using System.Runtime.InteropServices;

namespace BaseClass.Communication
{
    class Plc_Snap7:ProfinetCommunicationBase
    {

        S7Client Client=new S7Client();
        
        ~Plc_Snap7()
        {
            Client.Disconnect();
            
        }

        public override int ConnectTo(string Address, int Rack, int Slot)
        {
            return Client.ConnectTo(Address, Rack, Slot);                       
        }
        public override int ConnectTo_200(string Address, ushort LocalTSAP, ushort RemoteTSAP)
        {
            Client.SetConnectionParams(Address, LocalTSAP, RemoteTSAP);
            return Connect();
        }
        public override int PlcStop()
        {
            return Client.PlcStop();          
        }
        public override bool IsConneted()
        {
            return Client.Connected();
        }
        public override int Read(int Area, int start, int Amount, int WordLen, byte[] buffer)
        {           
            return Client.ReadArea(Area, 0, start, Amount, WordLen, buffer);          
        }
        public override int ReadDB(int Area, int DBnumber, int start, int Amount, int WordLen, byte[] buffer)
        {
            return Client.ReadArea(Area, DBnumber, start, Amount, WordLen, buffer);
        }
        public override int Write(int Area, int start, int Amount, int WordLen, byte[] buffer)
        {
            return Client.WriteArea(Area, 0,start, Amount, WordLen, buffer);
        }
        public override int WriteDB(int Area, int DBnumber, int start, int Amount, int WordLen, byte[] buffer)
        {
            return Client.WriteArea(Area, DBnumber, start, Amount, WordLen, buffer);
        }
        public override int PlcHotStart()
        {
 	        return Client.PlcHotStart();
        }
        public override int PlcColdStart()
        {
            return Client.PlcColdStart();
        }
        public override string PLCstatus()
        {
            int status=0;
            Client.PlcGetStatus(ref status);
            if(status ==4)
            {
                return "PLC Stop";
            }
            else if(status == 8)
            {
                return "PLC Run";
            }
            else
            {
                return "Status Unknown";
            }
        }
        public override DateTime GetPlctime()
        {
            DateTime time = Convert.ToDateTime("0000-00-00");
            int i=Client.GetPlcDateTime(ref time);
            if(i==0) 
            {
                  return time;
            }
            else
            {
                throw new Exception("Unable to get DateTime,Error code" + i.ToString());          
            }
          
        }
        public override int SetPlctime(DateTime time)
        {
            return Client.SetPlcDateTime(time);
        }
        public override int ReadMultiVars(S7Client.S7DataItem[] itemlist, int itemcount)
        {
            return Client.ReadMultiVars(itemlist,itemcount);
        }
        public override int WriteMultiVars(S7Client.S7DataItem[] itemlist, int itemcount)
        {
            return Client.WriteMultiVars(itemlist,itemcount);
        }
        public override int Connect()
        {
            return Client.Connect();
        }
        public override int Disconnect()
        {
            return Client.Disconnect();
        }

      



        #region [Constants, private vars and TypeDefs]
        private const int MsgTextLen = 1024;
        // Error codes
        public static readonly uint errNegotiatingPDU = 0x00100000;
        public static readonly uint errCliInvalidParams = 0x00200000;
        public static readonly uint errCliJobPending = 0x00300000;
        public static readonly uint errCliTooManyItems = 0x00400000;
        public static readonly uint errCliInvalidWordLen = 0x00500000;
        public static readonly uint errCliPartialDataWritten = 0x00600000;
        public static readonly uint errCliSizeOverPDU = 0x00700000;
        public static readonly uint errCliInvalidPlcAnswer = 0x00800000;
        public static readonly uint errCliAddressOutOfRange = 0x00900000;
        public static readonly uint errCliInvalidTransportSize = 0x00A00000;
        public static readonly uint errCliWriteDataSizeMismatch = 0x00B00000;
        public static readonly uint errCliItemNotAvailable = 0x00C00000;
        public static readonly uint errCliInvalidValue = 0x00D00000;
        public static readonly uint errCliCannotStartPLC = 0x00E00000;
        public static readonly uint errCliAlreadyRun = 0x00F00000;
        public static readonly uint errCliCannotStopPLC = 0x01000000;
        public static readonly uint errCliCannotCopyRamToRom = 0x01100000;
        public static readonly uint errCliCannotCompress = 0x01200000;
        public static readonly uint errCliAlreadyStop = 0x01300000;
        public static readonly uint errCliFunNotAvailable = 0x01400000;
        public static readonly uint errCliUploadSequenceFailed = 0x01500000;
        public static readonly uint errCliInvalidDataSizeRecvd = 0x01600000;
        public static readonly uint errCliInvalidBlockType = 0x01700000;
        public static readonly uint errCliInvalidBlockNumber = 0x01800000;
        public static readonly uint errCliInvalidBlockSize = 0x01900000;
        public static readonly uint errCliDownloadSequenceFailed = 0x01A00000;
        public static readonly uint errCliInsertRefused = 0x01B00000;
        public static readonly uint errCliDeleteRefused = 0x01C00000;
        public static readonly uint errCliNeedPassword = 0x01D00000;
        public static readonly uint errCliInvalidPassword = 0x01E00000;
        public static readonly uint errCliNoPasswordToSetOrClear = 0x01F00000;
        public static readonly uint errCliJobTimeout = 0x02000000;
        public static readonly uint errCliPartialDataRead = 0x02100000;
        public static readonly uint errCliBufferTooSmall = 0x02200000;
        public static readonly uint errCliFunctionRefused = 0x02300000;
        public static readonly uint errCliDestroying = 0x02400000;
        public static readonly uint errCliInvalidParamNumber = 0x02500000;
        public static readonly uint errCliCannotChangeParam = 0x02600000;

        // Area ID
        public static readonly byte S7AreaPE = 0x81;
        public static readonly byte S7AreaPA = 0x82;
        public static readonly byte S7AreaMK = 0x83;
        public static readonly byte S7AreaDB = 0x84;
        public static readonly byte S7AreaCT = 0x1C;
        public static readonly byte S7AreaTM = 0x1D;

        // Word Length
        public static readonly int S7WLBit = 0x01;
        public static readonly int S7WLByte = 0x02;
        public static readonly int S7WLWord = 0x04;
        public static readonly int S7WLDWord = 0x06;
        public static readonly int S7WLReal = 0x08;
        public static readonly int S7WLCounter = 0x1C;
        public static readonly int S7WLTimer = 0x1D;

        // Block type
        public static readonly byte Block_OB = 0x38;
        public static readonly byte Block_DB = 0x41;
        public static readonly byte Block_SDB = 0x42;
        public static readonly byte Block_FC = 0x43;
        public static readonly byte Block_SFC = 0x44;
        public static readonly byte Block_FB = 0x45;
        public static readonly byte Block_SFB = 0x46;

        // Sub Block Type 
        public static readonly byte SubBlk_OB = 0x08;
        public static readonly byte SubBlk_DB = 0x0A;
        public static readonly byte SubBlk_SDB = 0x0B;
        public static readonly byte SubBlk_FC = 0x0C;
        public static readonly byte SubBlk_SFC = 0x0D;
        public static readonly byte SubBlk_FB = 0x0E;
        public static readonly byte SubBlk_SFB = 0x0F;

        // Block languages
        public static readonly byte BlockLangAWL = 0x01;
        public static readonly byte BlockLangKOP = 0x02;
        public static readonly byte BlockLangFUP = 0x03;
        public static readonly byte BlockLangSCL = 0x04;
        public static readonly byte BlockLangDB = 0x05;
        public static readonly byte BlockLangGRAPH = 0x06;

        // Max number of vars (multiread/write)
        public static readonly int MaxVars = 20;

        // Client Connection Type
        public static readonly UInt16 CONNTYPE_PG = 0x01;  // Connect to the PLC as a PG
        public static readonly UInt16 CONNTYPE_OP = 0x02;  // Connect to the PLC as an OP
        public static readonly UInt16 CONNTYPE_BASIC = 0x03;  // Basic connection 

        // Job
        private const int JobComplete = 0;
        private const int JobPending = 1;
        #endregion
        


      


    }
}
