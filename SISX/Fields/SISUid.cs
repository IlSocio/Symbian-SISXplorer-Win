using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISUid : SISField
    {
        public UInt32 uid;


        public SISUid(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            uid = br.ReadUInt32();
        }

        public override string ToString()
        {
            return "0x" + String.Format( "{0:X8}", uid );
        }
    }
}
