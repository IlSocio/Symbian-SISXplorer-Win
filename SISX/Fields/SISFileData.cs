using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISFileData : SISField
    {
        public SISCompressed fileData;  // RAW DATA di ciascun file

        public SISFileData(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            fileData = SISField.Factory(br) as SISCompressed;
        }
    }
}
