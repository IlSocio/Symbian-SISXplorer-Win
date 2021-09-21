using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISSignatureCertificateChain : SISField
    {
        public SISArray signatures; // SISSignature
        public SISCertificateChain certificateChain;

        public SISSignatureCertificateChain(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            signatures = (SISArray) SISField.Factory(br);
            certificateChain = (SISCertificateChain)SISField.Factory(br);
        }
    }
}
