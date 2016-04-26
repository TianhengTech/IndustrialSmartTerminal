using System;
using Snap7;

namespace BaseClass.Communication
{
    internal class ThPlcSnap7 : ProfinetCommunicationBase
    {
        private readonly S7Client Client = new S7Client();
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

        // area ID
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
        public static readonly ushort CONNTYPE_PG = 0x01; // Connect to the PLC as a PG
        public static readonly ushort CONNTYPE_OP = 0x02; // Connect to the PLC as an OP
        public static readonly ushort CONNTYPE_BASIC = 0x03; // Basic connection 

        // Job
        private const int JobComplete = 0;
        private const int JobPending = 1;

        #endregion

        ~ThPlcSnap7()
        {
            Client.Disconnect();
        }
        /// <summary>
        /// Connect to PLC
        /// </summary>
        /// <param name="address"></param>
        /// <param name="rack"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public override int ConnectTo(string address, int rack, int slot)
        {
            return Client.ConnectTo(address, rack, slot);
        }
        /// <summary>
        /// Connect to S7-200+CP243 with TSAP setting
        /// </summary>
        /// <param name="address"></param>
        /// <param name="localTsap"></param>
        /// <param name="remoteTsap"></param>
        /// <returns></returns>
        public override int ConnectTo_200(string address, ushort localTsap, ushort remoteTsap)
        {
            Client.SetConnectionParams(address, localTsap, remoteTsap);
            return Connect();
        }
        /// <summary>
        /// Set PLC status to stop
        /// </summary>
        /// <returns></returns>
        public override int PlcStop()
        {
            return Client.PlcStop();
        }
        /// <summary>
        /// If the connection is established
        /// </summary>
        /// <returns></returns>
        public override bool IsConneted()
        {
            return Client.Connected();
        }
        /// <summary>
        /// Read non-DB area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="start"></param>
        /// <param name="amount"></param>
        /// <param name="wordLen"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public override int Read(int area, int start, int amount, int wordLen, byte[] buffer)
        {
            return Client.ReadArea(area, 0, start, amount, wordLen, buffer);
        }
        /// <summary>
        /// Read DB
        /// </summary>
        /// <param name="area"></param>
        /// <param name="dBnumber"></param>
        /// <param name="start"></param>
        /// <param name="amount"></param>
        /// <param name="wordLen"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public override int ReadDb(int area, int dBnumber, int start, int amount, int wordLen, byte[] buffer)
        {
            return Client.ReadArea(area, dBnumber, start, amount, wordLen, buffer);
        }
        /// <summary>
        /// Write non-DB area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="start"></param>
        /// <param name="amount"></param>
        /// <param name="wordLen"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public override int Write(int area, int start, int amount, int wordLen, byte[] buffer)
        {
            return Client.WriteArea(area, 0, start, amount, wordLen, buffer);
        }
        /// <summary>
        /// Write DB area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="dBnumber"></param>
        /// <param name="start"></param>
        /// <param name="amount"></param>
        /// <param name="wordLen"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public override int WriteDb(int area, int dBnumber, int start, int amount, int wordLen, byte[] buffer)
        {
            return Client.WriteArea(area, dBnumber, start, amount, wordLen, buffer);
        }
        /// <summary>
        /// Hot Start
        /// </summary>
        /// <returns></returns>
        public override int PlcHotStart()
        {
            return Client.PlcHotStart();
        }
        /// <summary>
        /// Cold Start
        /// </summary>
        /// <returns></returns>
        public override int PlcColdStart()
        {
            return Client.PlcColdStart();
        }
        /// <summary>
        /// Return PLC status
        /// </summary>
        /// <returns></returns>
        public override string PlCstatus()
        {
            var status = 0;
            Client.PlcGetStatus(ref status);
            if (status == 4)
            {
                return "PLC Stop";
            }
            if (status == 8)
            {
                return "PLC Run";
            }
            return "Status Unknown";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override DateTime GetPlctime()
        {
            var time = Convert.ToDateTime("0000-00-00");
            var i = Client.GetPlcDateTime(ref time);
            if (i == 0)
            {
                return time;
            }
            throw new Exception("Unable to get DateTime,Error code" + i);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public override int SetPlctime(DateTime time)
        {
            return Client.SetPlcDateTime(time);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemlist"></param>
        /// <param name="itemcount"></param>
        /// <returns></returns>
        public override int ReadMultiVars(S7Client.S7DataItem[] itemlist, int itemcount)
        {
            return Client.ReadMultiVars(itemlist, itemcount);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemlist"></param>
        /// <param name="itemcount"></param>
        /// <returns></returns>
        public override int WriteMultiVars(S7Client.S7DataItem[] itemlist, int itemcount)
        {
            return Client.WriteMultiVars(itemlist, itemcount);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int Connect()
        {
            return Client.Connect();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int Disconnect()
        {
            return Client.Disconnect();
        }

        
    }
}