using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{

    public class SISBlob : SISField
    {
        public byte[] data;


        public SISBlob(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            data = br.ReadBytes((int)length);
        }
    }
}
