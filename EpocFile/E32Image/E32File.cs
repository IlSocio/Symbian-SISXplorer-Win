using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Drawing;


namespace EpocData.E32Image
{
    public enum TCpu
    {
        ECpuUnknown = 0, 
        ECpuX86 = 0x1000, 
        ECpuArmV4 = 0x2000, 
        ECpuArmV5 = 0x2001, 
        ECpuArmV6 = 0x2002, 
        ECpuMCore = 0x4000
    };


    public enum TProcessPriority
    {
        EPriorityLow = 150,
        EPriorityBackground = 250,
        EPriorityForeground = 350,
        EPriorityHigh = 450,
        EPriorityWindowServer = 650,
        EPriorityFileServer = 750,
        EPriorityRealTimeServer = 850,
        EPrioritySupervisor = 950
    };


/*    public enum TFlagMask // Vedere f32image.h
    {
      EImageImpFmt_PE = 0x10000000,
      EImageHdrFmt_V = 0x02000000, 
      EImageEpt_Eka2 = 0x00000020,
      EImageABI_EABI = 0x00000008, 
      EImageNoCallEntryPoint = 0x00000002
    }*/


    public class E32File : EpocFile
    {
        public UInt32 uidCrc;   // CRC
        public UInt32 iSignature;
        public UInt32 iHeaderCrc; // CRC-32 of entire header
        public UInt32 iModuleVersion; // Version number for this executable (used in link resolution)
        public UInt32 iCompressionType; // Type of compression used (UID or 0 for none)
        public UInt32 iToolsVersion; // Version of PETRAN/ELFTRAN which generated this file
        public UInt32 iTimeLo;
        public UInt32 iTimeHi;
        public UInt32 iFlags; // 0 = exe, 1 = dll, 2 = fixed address exe
        public UInt32 iCodeSize; // size of code, import address table, constant data and export dir
        public UInt32 iDataSize; // size of initialised data
        public UInt32 iHeapSizeMin;
        public UInt32 iHeapSizeMax;
        public UInt32 iStackSize;
        public UInt32 iBssSize;
        public UInt32 iEntryPoint; // offset into code of entry point
        public UInt32 iCodeBase; // where the code is linked for
        public UInt32 iDataBase; // where the data is linked for
        public UInt32 iDllRefTableCount; // filling this in enables E32ROM to leave space for it
        public UInt32 iExportDirOffset; // offset into the file of the export address table
        public UInt32 iExportDirCount;
        public UInt32 iTextSize; // size of just the text section, also doubles as the offset for the iat w.r.t. the code section
        public UInt32 iCodeOffset; // file offset to code section, also doubles as header size
        public UInt32 iDataOffset; // file offset to data section
        public UInt32 iImportOffset; // file offset to import section
        public UInt32 iCodeRelocOffset; // relocations for code and const
        public UInt32 iDataRelocOffset; // relocations for data
        public UInt16 iProcessPriority; // executables priority
        public UInt16 iCpuIdentifier; // 0x1000 = X86, 0x2000 = ARM
        public UInt32 iUncompressedSize;
        public UInt32 iSecureID;
        public UInt32 iVendorID;
        public UInt32 iCaps;
        public UInt32 iExceptionDescriptor;
        public UInt32 iSpare2;
        public UInt16 iExportDescSize;
        public byte iExportDescType;
        public byte iExportDesc;

        public E32File(BinaryReader br)
            : base(br)
        {
            uidCrc = br.ReadUInt32();
            iSignature = br.ReadUInt32();
            if (iSignature != 0x434f5045) throw new Exception();
            iHeaderCrc = br.ReadUInt32();
            iModuleVersion = br.ReadUInt32();
            iCompressionType = br.ReadUInt32();
            iToolsVersion = br.ReadUInt32();
            iTimeLo = br.ReadUInt32();
            iTimeHi = br.ReadUInt32();
            iFlags = br.ReadUInt32();
            iCodeSize = br.ReadUInt32();
            iDataSize = br.ReadUInt32();
            iHeapSizeMin = br.ReadUInt32();
            iHeapSizeMax = br.ReadUInt32();
            iStackSize = br.ReadUInt32();
            iBssSize = br.ReadUInt32();
            iEntryPoint = br.ReadUInt32();
            iCodeBase = br.ReadUInt32();
            iDataBase = br.ReadUInt32();
            iDllRefTableCount = br.ReadUInt32();
            iExportDirOffset = br.ReadUInt32();
            iExportDirCount = br.ReadUInt32();
            iTextSize = br.ReadUInt32();
            iCodeOffset = br.ReadUInt32();
            iDataOffset = br.ReadUInt32();
            iImportOffset = br.ReadUInt32();
            iCodeRelocOffset = br.ReadUInt32();
            iDataRelocOffset = br.ReadUInt32();
            iProcessPriority = br.ReadUInt16();
            iCpuIdentifier = br.ReadUInt16();
            iUncompressedSize = br.ReadUInt32();
            iSecureID = br.ReadUInt32();
            iVendorID = br.ReadUInt32();
            iCaps = br.ReadUInt32();
            iExceptionDescriptor = br.ReadUInt32();
            iSpare2 = br.ReadUInt32();
            iExportDescSize = br.ReadUInt16();
            iExportDescType = br.ReadByte();
            iExportDesc = br.ReadByte();
            // http://www.antonypranata.com/articles/e32fileformatv9.html
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
