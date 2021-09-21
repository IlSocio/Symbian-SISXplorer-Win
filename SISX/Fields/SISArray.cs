using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace SISX.Fields
{
    public class SISArray : SISField
    {
        public UInt32 type;
        public ArrayList fields;

        public SISArray(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            fields = new ArrayList();
            type = br.ReadUInt32();

            long oldPos = br.BaseStream.Position;
            while ((br.BaseStream.Position - oldPos) < (long)length - sizeof(UInt32))
            {
                SISField fld = SISField.Factory(br, type);
                fields.Add(fld);
            }
        }

        public override string ToString()
        {
            string s = "";
            foreach (Object obj in fields)
            {
                if (s != "")
                    s += "; ";
                s += obj.ToString();
            }
            return s;
        }
    }

}
