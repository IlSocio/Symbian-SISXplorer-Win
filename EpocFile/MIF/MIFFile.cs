using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Drawing;


namespace EpocData.MIF
{
/*    class MIFFileEnumator : IEnumerator
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
    }*/

    public class MIFFile : EpocFile, IDisposable
    {
//        public UInt32 head1; // 42 23 23 34
//        public UInt32 head2; // 02 00 00 00 
//        public UInt32 head3; // 10 00 00 00 
        public JmpTable jmpTable;

        public MIFFile(BinaryReader br) : base(br)
        {
            Debug.Assert( uid1 == 0x34232342 );
            Debug.Assert( uid2 == 0x00000002 );
            Debug.Assert( uid3 == 0x00000010 );
            jmpTable = new JmpTable( br );
        }


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
