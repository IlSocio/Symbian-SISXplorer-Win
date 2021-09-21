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
    public class PaintDataSection
    {
        public long length;
        public long headerLength; // Offset
        public long xSize;
        public long ySize;
        public long xTwips;
        public long yTwips;
        public long bitsPerPixel;
        public bool imageIsColor;
        public long paletteSize;
        public long encoding;
        public byte[] data;


        public PaintDataSection(BinaryReader br) 
        {
            length = br.ReadUInt32(); 
            headerLength = br.ReadUInt32(); // Sempre = 0x28 ???
            System.Diagnostics.Debug.Assert( headerLength == 0x28 );
            xSize = br.ReadUInt32();
            ySize = br.ReadUInt32();
            xTwips = br.ReadUInt32();
            xTwips = br.ReadUInt32();
            bitsPerPixel = br.ReadUInt32(); // 256 colori = 0x08000000            
            imageIsColor = (br.ReadUInt32() > 0);
            paletteSize = br.ReadUInt32();
            encoding = br.ReadUInt32();
            // 0x00000000 = NO
            // 0x10000000 = 8 Bit RLE
            // 0x20000000 = 12 Bit RLE
            // 0x30000000 = 16 Bit RLE
            // 0x40000000 = 24 Bit RLE
            // La grandezza di una linea e' sempre allineata ai long???

            br.BaseStream.Seek( headerLength - 0x28, SeekOrigin.Current );
            //            data = new byte[length-0x28];
            data = br.ReadBytes( (int)(length - 0x28) );
            
        }
    }
}
