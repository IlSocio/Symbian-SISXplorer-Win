
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SISX.Fields;
using System.Diagnostics;
using System.Collections;


namespace SISX
{
    public class Header
    {
        public UInt32 uid1;
        public UInt32 uid2;
        public UInt32 uid3;
        public UInt32 uidChecksum;

        public Header(BinaryReader br)
        {
            uid1 = br.ReadUInt32();
            uid2 = br.ReadUInt32();
            uid3 = br.ReadUInt32();
            uidChecksum = br.ReadUInt32();
        }
    }


    public class SISXFile
    {
        public Header hdr;
        public SISContent cnt;        

        public SISXFile(BinaryReader br)
        {
            hdr = new Header(br);
            cnt = SISField.Factory(br) as SISContent;
        }
    }
}
