using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{

    public enum TInstallType
    {
        EInstApplication,       // SA
        EInstAugmentation,      // SP
        EInstPartialUpgrade,    // PU
        EInstPreInstalledApp,   // PA
        EInstPreInstalledPatch  // PP
    };


   public enum TInstallFlags
    {
        EInstFlagShutdownApps = 1
    };


    public class SISInfo : SISField
    {
        public SISUid uid;
        public SISString vendorName;
        public SISArray names;          // SISString
        public SISArray vendorNames;    // SISString
        public SISVersion version;
        public SISDateTime creationTime;
        public byte installType;        // TInstallType
        public byte installFlags;       // TInstallFlags


        public SISInfo(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            uid = SISField.Factory(br) as SISUid;
            vendorName = SISField.Factory(br) as SISString;
            names = SISField.Factory(br) as SISArray;
            vendorNames = SISField.Factory(br) as SISArray;
            version = SISField.Factory(br) as SISVersion;
            creationTime = SISField.Factory(br) as SISDateTime;
            installType = br.ReadByte();
            installFlags = br.ReadByte();
        }
    }
}
