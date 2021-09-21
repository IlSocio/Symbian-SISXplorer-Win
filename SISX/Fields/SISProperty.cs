using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISProperty : SISField
    {
        public UInt32 key;
        public UInt32 value;


        public SISProperty(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            key = br.ReadUInt32();
            value = br.ReadUInt32();
        }        
    }
}
