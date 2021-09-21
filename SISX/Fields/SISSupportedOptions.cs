using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISSupportedOptions : SISField
    {
        public SISArray options;  // SISSupportedOption

        public SISSupportedOptions(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            options = SISField.Factory(br) as SISArray;
        }

        public override string ToString()
        {
            return options.ToString();
        }
    }
}
