using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Drawing;


namespace EpocData.MBM
{


    public class MBMArchive : EpocFile, IDisposable
    {
        public UInt32 trailerOffset;
        public MbmJumpTable jmpTable;

        public MBMArchive(BinaryReader br)
            : base( br )
        {
            Debug.Assert( uid2 == 0x10000042 );
            UInt32 crc = br.ReadUInt32();
            trailerOffset = br.ReadUInt32();
            br.BaseStream.Seek( trailerOffset, SeekOrigin.Begin );
            jmpTable = new MbmJumpTable( br );
        }


    // TODO: Cambia per restituire non un Bitmap ma una mia interfaccia IImage che avra' i metodi: 
    //          Bitmap GetBitmap();    
    //          void SaveTo("\\path\\filename.ext");    
    //          byte[] RawData();

        public IImage this[int index]
        {
            get     
            {
                return jmpTable.paintData[index];
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            jmpTable.Dispose();
            jmpTable = null;
        }

        #endregion

    }
}
