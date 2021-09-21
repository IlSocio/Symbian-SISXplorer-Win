using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISDataChecksum : SISField
    {
        public UInt16 checksum;

        public SISDataChecksum(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            checksum = br.ReadUInt16();
        }

        public override string ToString()
        {
            return checksum.ToString();
        }
    }

}
