using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISDateTime : SISField
    {
        public SISDate date;
        public SISTime time;

        public SISDateTime(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            date = SISField.Factory(br) as SISDate;
            time = SISField.Factory(br) as SISTime;
            string s= this.ToString();        
        }

        public override string ToString()
        {
            return date.ToString() + "  " + time.ToString();
        }
    }
}
