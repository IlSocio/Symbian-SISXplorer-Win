using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISLogo : SISField
    {
        public SISFileDescription logo;

        public SISLogo(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            logo = SISField.Factory(br) as SISFileDescription;
        }
    }
}
