using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISDataUnit : SISField
    {
        public SISArray fileData;  // SISArray di SISFileData --- C'e' un SISFileData per ogni file

        public SISDataUnit(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            fileData = SISField.Factory(br) as SISArray;
        }
    }
}
