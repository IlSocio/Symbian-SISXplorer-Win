using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;


namespace MBM
{
    public class MBMFile
    {
        public UInt32 uid1;
        public UInt32 uid2;
        public UInt32 uid3;
        public UInt32 uidCrc;
        public UInt32 trailerOffset;
        public JmpTable jmpTable;


        public MBMFile(BinaryReader br)
        {
            uid1 = br.ReadUInt32();     // 37 00 00 10
            uid2 = br.ReadUInt32();     // 42 00 00 10
            uid3 = br.ReadUInt32();     // 00 00 00 00
            uidCrc = br.ReadUInt32();   // 39 64 39 47
            trailerOffset = br.ReadUInt32();
            br.BaseStream.Seek( trailerOffset, SeekOrigin.Begin );
            jmpTable = new JmpTable( br );
//            hdr = new Header(br);
//            cnt = SISField.Factory(br) as SISContent;
        }
    }
}
