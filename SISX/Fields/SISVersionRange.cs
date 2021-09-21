using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISVersionRange : SISField
    {
        public SISVersion fromVersion;
        public SISVersion toVersion;

        public SISVersionRange(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            fromVersion = (SISVersion)SISField.Factory(br);
            if (br.PeekChar() == 4) // Potrebbe non essere presente
                toVersion = (SISVersion)SISField.Factory(br);
        }
    }
}
