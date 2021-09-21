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
    public class JmpTable
    {
        public long length;
        public List<PaintDataSection> paintData;
        private List<long> offsets;


        public JmpTable(BinaryReader br)
        {
            paintData = new List<PaintDataSection>();
            offsets = new List<long>();
            length = br.ReadUInt32();
            for (int i = 0; i < length; i++)
            {
                offsets.Add( br.ReadUInt32() );
            }

            foreach (long offset in offsets)
            {
                if (paintData.Count == 0) Debug.Assert( offset == 0x14 );
                // Verifica che il primo offset sia 0x14
                br.BaseStream.Seek( offset, SeekOrigin.Begin );
                paintData.Add( new PaintDataSection( br ) );
            }
        }
    }
}
