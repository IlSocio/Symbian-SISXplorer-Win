using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;


namespace EpocData.MBM
{
    public class JmpTable : IDisposable
    {
        public long length;
        public List<IImage> paintData;


        public JmpTable(BinaryReader br)
        {
            paintData = new List<IImage>();
            List<long> offsets = new List<long>();
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
}
