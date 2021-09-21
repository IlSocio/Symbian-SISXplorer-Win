using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISCertificateChain : SISField
    {
        public SISBlob certificateData;

        public SISCertificateChain(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            certificateData = (SISBlob)SISField.Factory(br);
        }
    }
}
