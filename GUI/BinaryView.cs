using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;


namespace SISXplorer
{
    /// <summary>
    /// BinaryView class. It is used to calculate hex view parameters.
    /// </summary>
    public abstract class BinaryView
    {

        private static int OFFSET_WIDTH = 6;
        private static int DATA_WIDTH = 16;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BinaryView()
        {
        }

        //            totalWidth = offsetWidth + 2 + dataWidth * 3 + ((dataWidth / 8) - 1) + 1 + dataWidth;
        //            hexWidth = totalWidth - dataWidth;

        /// <summary>
        /// Generate hex view text string by calling <see cref="GetBinaryViewText"/>.
        /// </summary>
        /// <param name="data">source byte array.</param>
        /// <returns>output string.</returns>
        public static string GenerateText(byte[] data, int offsetWidth, int dataWidth)
        {
            string retval = "";
            IEnumerator enumer = GetEnumerator( data, offsetWidth, dataWidth );
            while (enumer.MoveNext())
            {
                retval += enumer.Current as string + "\r\n";
            }
            return retval;
        }


        public static string GenerateText(byte[] data)
        {
            return GenerateText( data, OFFSET_WIDTH, DATA_WIDTH );
        }


        public static IEnumerator GetEnumerator(byte[] data, int offsetWidth1, int dataWidth1)
        {
            return new BinaryViewerEnumerator( data, offsetWidth1, dataWidth1 );
        }
    }


    public class BinaryViewerEnumerator : IEnumerator
    {
        private int offset;
        private byte[] data;
        private string currLine;
        private int line = 0;
        private int dataWidth;
        private int offsetWidth;
        private int totalWidth = 0;
        private int hexWidth = 0;
        private string offForm;

        public BinaryViewerEnumerator(byte[] aData, int aOffsetWidth, int aDataWidth)
        {
            offsetWidth = aOffsetWidth;
            currLine = "";
            offset = 0;
            data = aData;
            dataWidth = aDataWidth;
            totalWidth = offsetWidth + 2 + dataWidth * 3 + ((dataWidth / 8) - 1) + 1 + dataWidth;
            hexWidth = totalWidth - dataWidth;
            offForm = "{0:X" + offsetWidth + "}";
        }


        private string GetBinaryTextLine()
        {
            int i = 0;
            string lineStr = String.Format( offForm, (line++) * dataWidth );
            lineStr = lineStr.Substring( lineStr.Length - offsetWidth );
            lineStr += "  ";
            int lineStart = offset;
            for (i = 0; i < dataWidth; i++)
            {
                lineStr += String.Format( "{0:X2} ", data[offset++] );
                if (offset >= data.Length) break;
                if ((i + 1) % 8 == 0 && i != 0 && (i + 1) < dataWidth) lineStr += " ";/**/
            }
            lineStr += " ";
            int lineEnd = offset;
            lineStr = lineStr.PadRight( hexWidth, ' ' );
            for (i = lineStart; i < lineEnd; i++)
            {
                if (data[i] < 32 || data[i] >= 128)
                    lineStr += '.';
                else
                    lineStr += (char)data[i];
            }
            lineStr = lineStr.PadRight( totalWidth, ' ' );
            return lineStr.TrimStart();
        }

        #region IEnumerator Members

        public object Current
        {
            get
            {
                return currLine;
            }
        }

        public bool MoveNext()
        {
            if (offset >= data.Length)
            {
                currLine = "";
                return false;
            }
            currLine = GetBinaryTextLine();
            return true;
        }

        public void Reset()
        {
            currLine = "";
            offset = 0;
            line = 0;
        }

        #endregion
    }

    /// <summary>
    /// ByteLocation class is used by <see cref="BinaryView"/> to transfer 
    /// location parameters.
    /// </summary>
    public class ByteLocation
    {
        /// <summary>
        /// line number.
        /// </summary>
        public int line = 0;

        /// <summary>
        /// Hex encoded data length.
        /// </summary>
        public int hexColLen = 3;

        /// <summary>
        /// Hex encoded data offset.
        /// </summary>
        public int hexOffset = 0;

        /// <summary>
        /// Character length.
        /// </summary>
        public int chColLen = 1;

        /// <summary>
        /// Character offset.
        /// </summary>
        public int chOffset = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ByteLocation()
        {
        }
    }

}
