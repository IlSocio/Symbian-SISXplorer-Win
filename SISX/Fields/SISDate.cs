using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISDate : SISField
    {
        public UInt16 year;
        public byte month;
        public byte day;


        public SISDate(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            year = br.ReadUInt16();
            month = br.ReadByte();
            day = br.ReadByte();
        }

        public override string ToString()
        {
            // Gestione di year, month, day in base a LocaleSettings
            DateTime time = new DateTime(year, month+1, day);
            return time.ToShortDateString();
        }
    }
}
