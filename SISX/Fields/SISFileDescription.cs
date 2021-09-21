using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public enum TSISFileOperation
    {
        EOpInstall  = 1,
        EOpRun      = 2,
        EOpText     = 4,
        EOpNull     = 8
    };


    // Valide per EOpInstall
    public enum TSISFileOperationOption
    {
        EInstVerifyOnRestore = 1 << 15    // Verify On Restore
    };


    // Valide per EOpRun
    public enum TInstFileRunOption
    {
        EInstFileRunOptionInstall       = 1 << 1,
        EInstFileRunOptionUninstall     = 1 << 2,
        EInstFileRunOptionByMimeType    = 1 << 3,
        EInstFileRunOptionWaitEnd       = 1 << 4,
        EInstFileRunOptionSendEnd       = 1 << 5
    };


    // Valide per EOpText
    public enum TInstTextOption
    {
        EInstFileTextOptionContinue     = 1 << 9,
        EInstFileTextOptionSkipIfNo     = 1 << 10,
        EInstFileTextOptionAbortIfNo    = 1 << 11,
        EInstFileTextOptionExitIfNo     = 1 << 12
    };


    public class SISFileDescription : SISField, IComparable
    {
        public SISString target;
        public SISString mimeType;
        public SISCapabilities capabilities;    // Opzionale
        public SISHash hash;
        public UInt32 operation;                // Vedi TSISOperation
        public UInt32 operationOptions;
        public UInt64 compressedLength;
        public UInt64 uncompressedLength;
        public UInt32 fileIndex;


        public SISFileDescription(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            target = SISField.Factory(br) as SISString;
            mimeType = SISField.Factory(br) as SISString;
            
            SISField fld = SISField.Factory(br);
            if (fld is SISCapabilities)
            {
                capabilities = fld as SISCapabilities;
                fld = SISField.Factory(br) as SISHash;
            }
            hash = fld as SISHash;
            operation = br.ReadUInt32();
            operationOptions = br.ReadUInt32();
            compressedLength = br.ReadUInt64();
            uncompressedLength = br.ReadUInt64();
            fileIndex = br.ReadUInt32();
        }

        #region IComparable Membri di

        public int CompareTo(object obj)
        {
            SISFileDescription fd = obj as SISFileDescription;
            if (fd == null) return 1;
            return target.aString.CompareTo(fd.target.aString);
        }

        #endregion
    }
}
