using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Drawing;


namespace EpocData.MIF
{
    class MIFFileEnumator : IEnumerator
    {
        private List<PaintDataSection> list;
        private int pos;

        public MIFFileEnumator(List<PaintDataSection> lista)
        {
            list = lista;
            pos = -1;
        }

        #region IEnumerator Members

        public object Current
        {
            get
            {
                if (pos < 0 || pos >= list.Count) return null;
                return list[pos].data;
            }
        }

        public bool MoveNext()
        {
            pos++;
            if (pos < 0 || pos >= list.Count) 
                return false;
            return true;
        }

        public void Reset()
        {
            pos = -1;
        }

        #endregion
    }

    public class MIFFile : ICollection, IDisposable
    {
        public UInt32 head1; // 42 23 23 34
        public UInt32 head2; // 02 00 00 00 
        public UInt32 head3; // 10 00 00 00 
        public JmpTable jmpTable;

        public MIFFile(BinaryReader br)
        {
//            br.BaseStream.Seek( 0, SeekOrigin.Begin );
            head1 = br.ReadUInt32(); Debug.Assert( head1 == 0x34232342 );
            head2 = br.ReadUInt32(); Debug.Assert( head2 == 0x00000002 );
            head3 = br.ReadUInt32(); Debug.Assert( head3 == 0x00000010 );
            jmpTable = new JmpTable( br );
            br.BaseStream.Seek( 0, SeekOrigin.Begin );
        }


        public IImage this[int index]
        {
            get     
            {
                return jmpTable.paintData[index];
            }
        }


        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new Exception( "The method or operation is not implemented." );
        }

        public int Count
        {
            get
            {
                return jmpTable.paintData.Count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                throw new Exception( "The method or operation is not implemented." );
            }
        }

        public object SyncRoot
        {
            get
            {
                throw new Exception( "The method or operation is not implemented." );
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return (new FileEnumator( jmpTable.paintData ));
        }

        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            jmpTable.Dispose();
            jmpTable = null;
        }

        #endregion
    }
}
