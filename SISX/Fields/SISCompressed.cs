using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using zlib;

namespace SISX.Fields
{

    enum TCompressionAlgoritm
    {
        ECompressNone=0,
        ECompressDeflate
    };


    public class SISCompressed : SISField
    {
        public UInt32 algoritm;                 // TCompressionAlgoritm
        public UInt64 uncompressedDataSize; 
        public byte[] data;

        public SISCompressed(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {            
            algoritm = br.ReadUInt32();
            uncompressedDataSize = br.ReadUInt64();
            ulong totBytesToRead = (ulong)(length - sizeof( UInt32 ) - sizeof( UInt64 ));            
            data = ReadDataFromStream( totBytesToRead, br.BaseStream );
        }

        /// <summary>
        /// Legge i dati dallo stream e se e' il caso li decomprime...
        /// </summary>
        private byte[] ReadDataFromStream(ulong totBytesToRead, Stream strm_in)
        {   
            byte[] decompressedData = new byte[uncompressedDataSize];

            MemoryStream ms_out = new MemoryStream( decompressedData, true );
            System.IO.Stream strm_out = ms_out;
            if (algoritm != 0)
                strm_out = new zlib.ZOutputStream( ms_out );
            CopyStream( totBytesToRead, strm_in, strm_out );
            ms_out.Close();
            strm_out.Close();

            return decompressedData;
        }


        private void CopyStream(ulong totBytesToRead, System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[20000];
            int len;
            ulong leftToRead = totBytesToRead;
            ulong chunkSize = Math.Min( (ulong)leftToRead, (ulong)buffer.Length );

            while (leftToRead > 0)
            {
                len = input.Read( buffer, 0, (int) chunkSize );
                output.Write( buffer, 0, len );
                System.Diagnostics.Debug.Assert( len == (int)chunkSize );
                leftToRead -= chunkSize;
                chunkSize = Math.Min( leftToRead, chunkSize );
            }
/*            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }*/
            output.Flush();
        }


    }
}
