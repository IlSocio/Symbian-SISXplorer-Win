using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISString : SISField
    {
        public string aString;

        public SISString(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            byte[] chars = new byte[length];            
            chars = br.ReadBytes((int)length);
            aString = Encoding.Unicode.GetString(chars);
        }

        public override string ToString()
        {
            return aString;
        }
    }
}
