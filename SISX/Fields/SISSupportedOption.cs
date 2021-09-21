using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISSupportedOption : SISField
    {
        public SISArray names;  // SISString

        public SISSupportedOption(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            names = SISField.Factory(br) as SISArray;
        }

        public override string ToString()
        {
            return names.ToString();
        }
    }
}
