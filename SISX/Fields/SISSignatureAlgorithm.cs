using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{

    public class SISSignatureAlgorithm : SISField
    {
        public SISString algorithmIdentifier;


        public SISSignatureAlgorithm(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            algorithmIdentifier = (SISString)SISField.Factory(br);
        }

        public override string ToString()
        {
            return algorithmIdentifier.ToString();
        }
    }
}
