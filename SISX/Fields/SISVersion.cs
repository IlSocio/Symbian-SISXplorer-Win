using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISVersion : SISField
    {
        public Int32 major;
        public Int32 minor;
        public Int32 build;


        public SISVersion(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            major = br.ReadInt32();
            minor = br.ReadInt32();
            build = br.ReadInt32();
        }

        public override string ToString()
        {
            return major + "," + minor + "," + build;
        }
    }
}
