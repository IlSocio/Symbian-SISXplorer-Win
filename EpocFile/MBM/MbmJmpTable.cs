using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;


namespace EpocData.MBM
{
    public class MbmJumpTable : IDisposable
    {
        public long qtaImages; // Quantita' di immagini totali contenute
        public List<IImage> paintData;


        public MbmJumpTable(BinaryReader br)
        {
            qtaImages = br.ReadUInt32();
            paintData = new List<IImage>();
            List<long> offsets = new List<long>();
            for (int i = 0; i < qtaImages; i++)
            {
                offsets.Add( br.ReadUInt32() );
            }

            foreach (long offset in offsets)
            {
                if (paintData.Count == 0) Debug.Assert( offset == 0x14 );
                // Verifica che il primo offset sia 0x14
                br.BaseStream.Seek( offset, SeekOrigin.Begin );
                paintData.Add( new MbmPaintDataSection( br ) );
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
/*            foreach (IImage pd in paintData)
            {
                pd.Dispose();
            }
            paintData.Clear();
            paintData = null;*/
        }

        #endregion
    }
}
