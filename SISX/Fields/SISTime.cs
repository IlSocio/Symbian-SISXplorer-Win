using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISTime : SISField
    {
        public byte hours;
        public byte minutes;
        public byte seconds;


        public SISTime(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            hours = br.ReadByte();
            minutes = br.ReadByte();
            seconds = br.ReadByte();
        }

        public override string ToString()
        {
            DateTime time = new DateTime(2000, 1, 1, hours, minutes, seconds);
            //TimeSpan time = new TimeSpan( hours, minutes, seconds );
            return time.ToShortTimeString();
        }
    }
}
