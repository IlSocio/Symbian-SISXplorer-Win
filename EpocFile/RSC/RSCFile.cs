using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Drawing;
using href.Utils;


namespace EpocData.RSC
{

    public class Chunk
    {
        public byte[] data;

        public Chunk(byte[] aData)
        {            
            data = aData;
        }
    }


    public class Resource
    {
        public UInt32 offset;
        public List<Chunk> chunks;
        public bool isCompressed;
        public bool notCompressed;


        public Resource(UInt32 aOffset, bool compressedFlag, byte[] aData)
        {
            chunks = new List<Chunk>();
            offset = aOffset;
            isCompressed = compressedFlag;
            notCompressed = false;

            if (isCompressed)
            {
                // Parsifica e suddivide aData in Chunks
                int i = 0;
                while (i < aData.Length)
                {
                    int lun = aData[i]; i++;
                    if (lun > 127)
                    {
                        lun &= 0x7F;
                        lun <<= 8;
                        lun += aData[i]; i++;
                    }
                    if (i == 1 && lun == 0)
                    {
                        notCompressed = true;
                        continue;
                    } 
                    Debug.Assert(lun + i <= aData.Length);
                    byte[] newData = new byte[lun];
                    Array.Copy(aData, i, newData, 0, lun);
                    Chunk run = new Chunk(newData);
                    chunks.Add(run);
                    i += lun;
                }
            }
            else
            {
                notCompressed = true;
                if (aData.Length > 0)
                {
                    Chunk run = new Chunk(aData);
                    chunks.Add(run);
                }
            }

            /* Per ogni "chunk" e' presente un intero (1 o 2 bytes) che indica la lunghezza del chunk (escluso questo intero)
             * la lunghezza e' espressa con un singolo byte se e' < 128 altrimenti occupa 2 bytes (little-endian) con il bit + significativo impostato ad 1 per indicare che occupa 2 byte
             * Solo la lunghezza del primo "chunk" puo' essere 0 il che indica che la risorsa non inizia con compressed unicode
             */
        }

        public int TotChunks
        {
            get
            {
                return chunks.Count;
            }
        }

        public Chunk GetChunk(int pos)
        {
            return chunks[pos];
/*            if (!isCompressed)
            {
                // TODO: Rimuovi 0xab
                // string s = Encoding.Unicode.GetString(data);
                return data;
            }
            // TODO: 
            return data;*/
        }

        public override string ToString()
        {
            string s = "Offset: " + offset + "\tCompressed:" + isCompressed;
            for (int i=0; i< TotChunks; i++)
                GetChunk(i);
            return s;
        }
    }


    public class RSCFile : EpocFile, IDisposable
    {
        public byte flags;
        public UInt32 uidCrc;
        public UInt16 maxResSize;
        public UInt16 indexOffset;
        public List<Resource> resources = new List<Resource>();
        public Encoding encoding;


        public RSCFile(BinaryReader br)
            : base(br)
        {
            Debug.Assert(uid1 == 0x101f4a6b);
            // Debug.Assert(uid2 == 0x00000000); 
            // uid3 = resource file offset i.e. the twenty-bit integer generated from the resource file’s name
            // These twenty bits are stored in the least significant twenty bits of the third UID; the most significant twelve bits are all zero.            
            uidCrc = br.ReadUInt32();
            flags = br.ReadByte();
            if (flags % 2 == 1)
            {
                UInt32 offset = uid3;
            }
            br.BaseStream.Seek(-2, SeekOrigin.End);
            indexOffset = br.ReadUInt16();

            // Legge gli offset delle varie risorse...
            List<UInt16> offsets = new List<UInt16>();
            offsets.Clear();
            br.BaseStream.Seek(indexOffset, SeekOrigin.Begin);
            while (br.BaseStream.Position < br.BaseStream.Length - 2)
            {
                offsets.Add(br.ReadUInt16());
            }

            // Legge i vari bit che descrivono se le risorse sono compresse o meno...
            int n = (int)Math.Ceiling((double)offsets.Count / 8);
            br.BaseStream.Seek(16 + 1 + 2, SeekOrigin.Begin);
            byte[] compressed = br.ReadBytes(n);

            for (int i = 0; i < offsets.Count; i++)
            {
                // Rileva l'offset
                UInt16 offset = offsets[i];

                // Rileva il flag Compressed
                int pos = i;
                int indx = pos / 8;
                pos = (pos % 8);
                byte mask = (byte) (1 << pos);
                bool compr = ((compressed[indx] & mask) > 0);

                // Rileva la lunghezza dei dati
                UInt16 offset1 = offsets[i];
                UInt16 offset2 = indexOffset;
                if (i < offsets.Count - 1)
                    offset2 = offsets[i + 1];
                UInt16 resLen = (UInt16) (offset2 - offset1);

                // Rileva i dati veri e propri
                br.BaseStream.Seek(offset, SeekOrigin.Begin);
                byte[] data = br.ReadBytes(resLen);
                if (data.Length > 0)
                {
                    Resource res = new Resource(offset, compr, data);
                    resources.Add(res);
                    Debug.WriteLine(res);
                }
            }

/*            Hashtable tbl = new Hashtable();

            // Rileva l'encoding...
            int ascii=0;
            int unicode=0;
            foreach (Resource res in resources)
            {
                foreach (Chunk chunk in res.chunks)
                {
                    Encoding enc = EncodingTools.DetectInputCodepage(chunk.data);
                    Debug.WriteLine(enc.ToString());
                    if (enc == Encoding.ASCII) ascii++;
                    if (enc == Encoding.Unicode) unicode++;
                }
            }

            if (ascii > unicode) encoding = Encoding.ASCII;
            else encoding = Encoding.Unicode;*/
        }


        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
