using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISProperties : SISField
    {
        public SISArray properties; // SISProperty

        public SISProperties(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            properties = SISField.Factory(br) as SISArray;
        }
    }
}
