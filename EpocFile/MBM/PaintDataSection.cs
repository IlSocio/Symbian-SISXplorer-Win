using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Utility;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using EpocData;



namespace EpocData.MBM
{

    // Fai derivare PaintDataSection da IImage

    public class PaintDataSection : IImage
    {
        private uint length;
        private uint headerLength; // Offset
        private uint xSize;
        private uint ySize;
        private uint xTwips;
        private uint yTwips;
        private uint bitsPerPixel;
        private bool imageIsColor;
        private uint paletteSize;    // Sempre Zero?
        private uint encoding;
        private Bitmap _bitmap;
        private byte[] comprData;
        private static ColorPalette Palette8Color;
        private static ColorPalette Palette8BW;

        static PaintDataSection()
        {
            Image bmp = Bitmap.FromFile( "col8bit.bmp" );
            Palette8Color = bmp.Palette;
            bmp.Dispose();
            bmp = Bitmap.FromFile( "bw8bit.bmp" );
            Palette8BW = bmp.Palette;
            bmp.Dispose();
        }


        // 1 RLE packet: [GGGGBBBB] [xxxxRRRR]
/*        private int GetR(byte gb, byte r)
        {
            r = (byte)(r & 0x0F);   // 5
            int g = (gb & 0xF0)>>4; // 5
            int b = (gb & 0x0F);    // 5
            r = (byte)((r << 4) | r);
            g = (g << 4) | g;
            b = (b << 4) | b;

            r = (byte)(r & 0x1F);
            g = g & 0x1F;
            b = b & 0x1F;

            // [RRRRRGGG GGBBBBBB]
/*            int ris = r << 11;
            ris = ris | (g << 6);
            ris = ris | b;

            // [RRRRRGGG GGGBBBBB]
            int ris = r << 10;
            ris = ris | (g << 5);
            ris = ris | b;
            return ris;
        }*/


        // 1 RLE packet: [GGGGBBBB] [nnnnRRRR]
        // 16bpp = i 5 bit per rappresentare il rosso e 5 bit per rappresentare il blu, ma 6 bit per rappresentare il verde
        private int RLE_Unpack12(byte[] dest)
        {
            //throw new Exception( "NON GESTITA" );
            int i = 0;
            int j = 0;
            byte repeat = 0;
            while (i < comprData.Length)
            {
                byte gb = comprData[i]; i++; // [GGGGBBBB]
                repeat = comprData[i]; i++; 
                byte r = (byte)(repeat & 0x0F) ;
                repeat &= 0xF0;
                repeat >>= 4;
                int g = gb >> 4;
                int b = (byte)(gb & 0x0F);
                r = (byte)((r << 4) | r);
                g = (g << 4) | g;
                b = (b << 4) | b;
                for (int c = 0; c < repeat + 1; c++)
                {
                    if (j <= dest.Length - 3)
                    {
                        dest[j] = (byte)r; j++;
                        dest[j] = (byte)g; j++;
                        dest[j] = (byte)b; j++;
                    }
                    else
                        Debug.WriteLine("Uncompress1: Data Skipped.");
                }
            }
            //            Debug.WriteLine( "Compressed:" + i + "/" + comprData.Length + "  Plain:" + j + "/" + data.Length );
            if (i != comprData.Length || j != dest.Length)
            {
                if (i == comprData.Length && j < dest.Length) return j;
            } 
            return j;
        }

        private int RLE_Unpack(byte[] data)
        {
            int bytesPerPixel = 0;

//            Debug.WriteLine( "encoding:" + encoding );
            switch (encoding)
            {
                case 0:
                    {
                        // Plain data
                        Debug.WriteLine("Plain Data");
                        Array.Copy( comprData, data, comprData.Length );
                        return comprData.Length;
                    }
                case 2:
                    {
                        // 12-bit RLE encoding
                        // encoding consists of words. The 4 most significant bits hold the number of repetitions minus 1 of the 12 least significant bits.
                        //Debug.Assert( false );                        
                        return RLE_Unpack12( data );
                    }
                case 1:
                    {
                        // 8-bit RLE encoding
                        // encoding consists of sequences of marker bytes with data bytes.
                        // A marker byte of 00 to 7F means that the next byte should be repeated that many times and once more. 
                        // A marker byte of 80 to FF means that (100-marker) bytes of data follow.
                        bytesPerPixel = 1;
                        break;
                    }
                case 3:
                    {
                        // 16-bit RLE encoding
                        // A marker byte of 00 to 7F means that the next pixel (2 bytes) should be repeated that many times and once more. 
                        // A marker byte of 80 to FF means that (100-marker) pixels (2 bytes each) of data follow.
                        bytesPerPixel = 2;
                        break;
                    }
                case 4:
                    {
                        // 24-bit RLE encoding
                        // A marker byte of 00 to 7F means that the next pixel (3 bytes) should be repeated that many times and once more. 
                        // A marker byte of 80 to FF means that (100-marker) pixels (3 bytes each) of data follow.
                        bytesPerPixel = 3;
                        break;
                    }
                default:
                    {
                        throw new Exception( "Unknown Encoding" );
                    }
            }

            int i = 0;
            int j = 0;
            byte marker = 0;
            while (i < comprData.Length)
            {
                marker = comprData[i]; i++;
                if (marker < 0x80)
                {   // Meno di 128 ripetizioni
                    for (int c = 0; c < marker + 1; c++)
                    {
                        if (j <= data.Length - bytesPerPixel)
                        {
                            Array.Copy( comprData, i, data, j, bytesPerPixel ); // Ripete un singolo pixel
                            j += bytesPerPixel;
                        }
                        else
                            Debug.WriteLine("Uncompress1: Data Skipped.");
                    }
                    i += bytesPerPixel;
                }
                else
                {   // Meno di 128 bytes da copiare
                    for (int c = 0; c < 0x100 - marker; c++)
                    {
                        for (int zz = 0; zz < bytesPerPixel; zz++) // copia un singolo pixel
                        {
                            if (j < data.Length && i < comprData.Length)
                                data[j] = comprData[i];
                            else
                                Debug.WriteLine("Uncompress2: Data Skipped.");
                            j++; i++;
                        }
                    }
                }
            }
//            Debug.WriteLine( "Compressed:" + i + "/" + comprData.Length + "  Plain:" + j + "/" + data.Length );
            if (i != comprData.Length || j != data.Length)
            {
                if (i == comprData.Length && j < data.Length) return j;
            }
            return j;
        }


        private Bitmap bitmap
        {
            get
            {
                if (_bitmap != null)
                    return _bitmap;

                /*if (encoding == 2)
                {
                    Debug.Assert( false );
                }*/

                // 20 pixels 1bit per ogni pixel => 20 bits per linea => non e' multiplo di 32 => devo aggiungere bits sino ad arrivare a 32...
                // 32 bits per linea = 4 byte per linea
/*                uint bitsPerLine = xSize * bitsPerPixel;
                uint stride = (uint)(bitsPerLine % 32); // quantita' di bits presenti per completare la linea (0=linea completa)
                if (stride > 0)
                {
                    stride = 32 - stride;                   // bits da aggiungere 
                    //stride = stride / bitsPerPixel;
                    bitsPerLine += stride;
                    // xSize = bitsPerLine / bitsPerPixel; // TODO: Forse e' da eliminare...
                    xSize += (stride / 8);
                    bitsPerLine = xSize * bitsPerPixel;
                }*/

/*                uint bitsPerLine = xSize * bitsPerPixel;
                int pixelsToAdd = (int)(xSize % 4);
                pixelsToAdd = (4 - pixelsToAdd) % 4;
                int bytesToAdd = (int)(pixelsToAdd * bitsPerPixel / 8);

                int rem = 0;*/
/* Conversioni..
 * 30x..x24  => 32x
 * 13x..x1   => 16x
 * 13x..x16  =>         28BytesPerLine
 * 
 * 
 */
                /*uint bitsPerLine = xSize * bitsPerPixel;
                int bitsToAdd = (int)(bitsPerLine % 32);
                bitsToAdd = (int)((32 - bitsPerLine) % 32);
                int bytesPerLine = (int)(bitsPerLine+bitsToAdd)/8;/**/

                /*int rem = (int) xSize % 4;
                rem = (int)(4 - rem) % 4;
                int bytesPerLine = (int)((xSize + rem) * bitsPerPixel / 8);*/


                // Debug.Assert(bytesPerLine % 4 == 0, "Errore");
                // E' un assert sbagliato... es: xSize=13  bitsPerPixel=16  rem=2
                // Debug.Assert((xSize+rem) % 4 == 0, "Errore");
                // E' un assert sbagliato... es: xSize=30  bitsPerPixel=16  rem=0
                
   

                PixelFormat fmt = PixelFormat.Format16bppRgb565;
                switch (bitsPerPixel)
                {
                    case 1:
                        {
                            fmt = PixelFormat.Format1bppIndexed;
                            // TODO: Formato non ancora gestito
                           // return new Bitmap((int)xSize, (int)ySize);
                            break;
                        }
                    case 4:
                        {
                            fmt = PixelFormat.Format4bppIndexed;
                            // TODO: Formato non ancora gestito
                            return new Bitmap((int)xSize, (int)ySize);
                        }
                    case 8:
                        {
                            //Debug.Assert( false );
                            //comprData = null;
                            fmt = PixelFormat.Format8bppIndexed;
                            break;
                        }
                    case 12:
                        {
                            // Debug.Assert(false);
                            // ???? Convertire data da 12 a 16 
                            fmt = PixelFormat.Format24bppRgb;
                            bitsPerPixel = 24;
                            break;
                            // TODO: Formato non ancora gestito
                            // return new Bitmap((int)xSize, (int)ySize);
                        }
                    case 16:
                        {
                            fmt = PixelFormat.Format16bppRgb565;
                            break;
                        }
                    case 24:
                        {
                            fmt = PixelFormat.Format24bppRgb;
                            //                            Debug.WriteLine( xSize );
                            break;
                        }
                    case 32:
                        {
                            Debug.Assert(false);
                            fmt = PixelFormat.Format32bppRgb;
                            break;
                        }
                    default:
                        throw new Exception("NON GESTITO");
                }

                int maxBytesPerLine = (int)((xSize + 4) * 24 / 8);

                byte[] data = new byte[maxBytesPerLine * ySize];
                int realLength = RLE_Unpack(data);
                int bytesPerLine = (int)(realLength / ySize);

                if (encoding == 2)
                {
                    // C'e' stata una conversione da 12bpp -> 24bpp bisogna aggiungere i bytes di stride...
                    int missingBPL = bytesPerLine % 4;
                    if (missingBPL > 0)
                    {
                        missingBPL = 4 - missingBPL;
                        byte[] data2 = new byte[maxBytesPerLine * ySize];
                        int i=0;
                        int j=0;
                        for (int y1 = 0; y1 < ySize; y1++)
                        {
                            Array.Copy(data, i, data2, j, bytesPerLine);
                            i += bytesPerLine;
                            j += bytesPerLine;
                            j += missingBPL;
                        }
                        data = data2;
                        realLength = j;
                        bytesPerLine = (int)(realLength / ySize);
                    }
                }
                Debug.Assert(bytesPerLine % 4 == 0, "Non e' mod 4");
                long pixelsPerLine = bytesPerLine * 8 / bitsPerPixel;

                int unusedPixels = (int)(pixelsPerLine - xSize);
//                unusedPixels = 0;

                _bitmap = new Bitmap((int)xSize + unusedPixels, (int)ySize, fmt);

                if (fmt == PixelFormat.Format8bppIndexed)
                {
                    if (this.imageIsColor)
                        _bitmap.Palette = PaintDataSection.Palette8Color;
                    else
                        _bitmap.Palette = PaintDataSection.Palette8BW;
                }

                //Lock all pixels
//                unusedPixels = 0;
                BitmapData bmpData = _bitmap.LockBits(
                           new Rectangle(0, 0, (int)xSize + unusedPixels, (int)ySize),
                           ImageLockMode.WriteOnly, fmt);

                unusedPixels = (int)(pixelsPerLine - xSize);
    //            Debug.WriteLine("Unused Pix:" + unusedPixels + "\t xSize:" + xSize + "\tStride:" + bmpData.Stride * 8 / bitsPerPixel);
                Debug.Assert(xSize + unusedPixels == (bmpData.Stride * 8 / bitsPerPixel), "ERRORE");

                Debug.Assert(bmpData.Stride % 4 == 0, "Non e' mod 4");
                Debug.Assert(bmpData.Stride == bytesPerLine);
                Debug.Assert(realLength <= (int)(bmpData.Stride * ySize));

                //                        Marshal.Copy( data, 0, bmpData.Scan0, (int)(bmpData.Stride*ySize) );
                // TODO: Cosa fare se e' 1bpp???
                //
                Marshal.Copy(data, 0, bmpData.Scan0, realLength);

                //Unlock the pixels
                _bitmap.UnlockBits(bmpData);

                // CROP IMAGE
                Rectangle rectangle = new Rectangle(0, 0, (int)xSize, (int)ySize);
                Bitmap cropped = _bitmap.Clone(rectangle, fmt);
                _bitmap.Dispose();
                _bitmap = cropped;
                return _bitmap;                
#region old

                /*int bytePerPixel = (int)(bitsPerPixel / 8);
                uint stride = xSize % 4;
                if (stride > 0)
                {
                    stride = (uint)4 - stride;                     // Calcola quanti bytes x ogni riga non sono usati...
                    xSize += stride;
                }

                PixelFormat fmt = PixelFormat.Format16bppRgb565;
                switch (bitsPerPixel)
                {
                    case 1:
                        {
                            fmt = PixelFormat.Format1bppIndexed;
                            // TODO: Formato non ancora gestito
                            return new Bitmap((int)xSize, (int)ySize);
                        }
                    case 4:
                        {
                            fmt = PixelFormat.Format4bppIndexed;
                            // TODO: Formato non ancora gestito
                            return new Bitmap((int)xSize, (int)ySize);
                        }
                    case 8:
                        {
                            //Debug.Assert( false );
                            //comprData = null;
                            fmt = PixelFormat.Format8bppIndexed;
                            break;
                        }
                    case 12:
                        {
                            Debug.Assert( false );
                            // ???? Convertire data da 12 a 16 
                            fmt = PixelFormat.Format16bppRgb565;
                            // TODO: Formato non ancora gestito
                            return new Bitmap((int)xSize, (int)ySize);
                        }
                    case 16:
                        {
                            xSize = (uint)(xSize - stride);
                            if (xSize % 2 > 0)
                            {
                                xSize++;
                            }
                            fmt = PixelFormat.Format16bppRgb565;
                            break;
                        }
                    case 24:
                        {
                            fmt = PixelFormat.Format24bppRgb;
//                            Debug.WriteLine( xSize );
                            break;
                        }
                    case 32:
                        {
                            Debug.Assert( false );
                            fmt = PixelFormat.Format32bppRgb;
                            break;
                        }
                    default:
                        throw new Exception( "NON GESTITO" );
                }

                _bitmap = new Bitmap( (int)xSize, (int)ySize, fmt );

                if (fmt == PixelFormat.Format8bppIndexed)
                {
                    if (this.imageIsColor)
                        _bitmap.Palette = PaintDataSection.Palette8Color;
                    else
                        _bitmap.Palette = PaintDataSection.Palette8BW;
                }

                //Lock all pixels
                BitmapData bmpData = _bitmap.LockBits(
                           new Rectangle( 0, 0, (int)xSize, (int)ySize ),
                           ImageLockMode.WriteOnly, fmt );

                byte[] data = RLE_Unpack( xSize * bytePerPixel * ySize );
                // stride = numero di pixel per ogni linea...

                Debug.Assert( (int)(bmpData.Stride * ySize) == data.Length );

                //                        Marshal.Copy( data, 0, bmpData.Scan0, (int)(bmpData.Stride*ySize) );
                Marshal.Copy( data, 0, bmpData.Scan0, data.Length );

                //Unlock the pixels
                _bitmap.UnlockBits( bmpData );
                return _bitmap;*/
#endregion
            }
        }



        public PaintDataSection(BinaryReader br)
        {
            length = br.ReadUInt32();
            headerLength = br.ReadUInt32(); // Sempre = 0x28 ???
            System.Diagnostics.Debug.Assert( headerLength == 0x28 );
            xSize = br.ReadUInt32();
            ySize = br.ReadUInt32();
            xTwips = br.ReadUInt32();
            xTwips = br.ReadUInt32();
            bitsPerPixel = br.ReadUInt32(); // 8 = 256 colori
            imageIsColor = (br.ReadUInt32() > 0);
            paletteSize = br.ReadUInt32();
            encoding = br.ReadUInt32();

            // 0 = NO
            // 1 = 8 Bit RLE    // Marker 1 byte / Pixel 1 byte
            // 2 = 12 Bit RLE   // Marker 4 bit  / Pixel 12 bit
            // 3 = 16 Bit RLE   // Marker 1 byte / Pixel 2 byte
            // 4 = 24 Bit RLE   // Marker 1 byte / Pixel 3 byte
            // La grandezza di una linea dopo la decodifica RLE e' sempre allineata ai long...

            long newPos = br.BaseStream.Seek( headerLength - 0x28, SeekOrigin.Current );
            comprData = br.ReadBytes( (int)(length - headerLength) );/**/
            //            Debug.Assert( comprData != null );
        }

        #region IDisposable Members

        public override void Dispose()
        {
            if (_bitmap != null)
                _bitmap.Dispose();
            _bitmap = null;
        }

        #endregion

        public override Object GetData()
        {
            return bitmap;
        }

        public override void SaveTo(string filename)
        {
            Bitmap bmp = GetData() as Bitmap;
            bmp.Save(filename + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
        }
    }
}
