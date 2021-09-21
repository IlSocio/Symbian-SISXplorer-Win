using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISDataIndex : SISField
    {
        public UInt32 dataIndex;


        public SISDataIndex(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            dataIndex = br.ReadUInt32();
        }

        public override string ToString()
        {
            return dataIndex.ToString();
        }
    }
}
