using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;


namespace EpocData.MIF
{
    public class JmpTable : IDisposable
    {
        public long qtaImages; // Quantita' di immagini totali contenute
        public List<IImage> paintData;


        public JmpTable(BinaryReader br)
        {
            paintData = new List<IImage>();
            Hashtable entries = new Hashtable();
            qtaImages = br.ReadUInt32(); 

            int i = 0;
            while (i < qtaImages)
            {
                Int32 offset = br.ReadInt32();
                Int32 length = br.ReadInt32();
                if (offset > 0 && !entries.ContainsKey(offset))
                    entries.Add( offset, length );
                i++;
            }

            if (br.BaseStream.Position < br.BaseStream.Length)
            {
                UInt32 test = br.ReadUInt32();
                Debug.Assert( test == 0x34232343 );
            }

            foreach (Int32 offset in entries.Keys)
            {
//                Int32 length = (Int32)entries[offset];
                br.BaseStream.Seek( offset, SeekOrigin.Begin );
                paintData.Add( new PaintDataSection( br ) );
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (IImage pd in paintData)
            {
                pd.Dispose();
            }
            paintData.Clear();
            paintData = null;
        }

        #endregion
    }


    internal struct TIndexEntry
    {
        public Int32 offset;
        public Int32 length;

        public TIndexEntry(Int32 offs, Int32 len)
        {
            offset = offs;
            length = len;
        }
    }
}
