using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISSignature : SISField
    {
        public SISSignatureAlgorithm signatureAlgorithm;
        public SISBlob signatureData;

        public SISSignature(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            signatureAlgorithm = (SISSignatureAlgorithm)SISField.Factory(br);
            signatureData = (SISBlob)SISField.Factory(br);
        }
    }
}
