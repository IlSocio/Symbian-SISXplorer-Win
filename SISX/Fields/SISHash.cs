using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    enum TSISHashAlgorithm 
    {
        ESISHashAlgSHA1 = 1
    };

    public class SISHash : SISField
    {
        public UInt32 hashAlgoritm;
        public SISBlob hashData;


        public SISHash(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            hashAlgoritm = br.ReadUInt32();
            hashData = SISField.Factory(br) as SISBlob;
        }
    }
}
