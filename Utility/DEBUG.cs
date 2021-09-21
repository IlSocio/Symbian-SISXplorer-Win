
// #define TIMESTAMP

using System.Diagnostics;
using System;
using System.IO;
using System.Data;


namespace Utility
{

	/// <summary>
	/// Summary description for Debug.
	/// </summary>
	public class Debug
	{

		public static DateTime last = DateTime.Now;
        private static Object thisLock = new Object();

		public Debug()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public static void WriteLine<T>(T s)
        {
            Write(s);
             System.Diagnostics.Debug.WriteLine("");
        }

		public static void Write<T>(T s) 
		{
			#if TIMESTAMP
				DateTime now = DateTime.Now;
				TimeSpan ris = now-last;
				last = now;
                string strTime1 = "["+now.Hour+":"+now.Minute+":"+now.Second+"."+now.Millisecond;
                strTime1 = strTime1.PadRight( 20 );
                string strTime2 = "+"+ris.Hours+":"+ris.Minutes+":"+ris.Seconds +"."+ ris.Milliseconds +"]";
                strTime2 = strTime2.PadRight( 15 );
				System.Diagnostics.Debug.Write("\t\t"+strTime1+strTime2);
                LogToFile( "Log_TimeStamp.txt", strTime1 + strTime2+s );
            #endif
             System.Diagnostics.Debug.Write(s);
             System.Diagnostics.Debug.Flush();
		}

		public static void Assert(bool condition) 
		{
             System.Diagnostics.Debug.Assert(condition, "Errore!!!");
          //  if (!condition) throw new Exception( "AssertionFailed:");
        }

		public static void Assert(bool condition, string message) 
		{
             System.Diagnostics.Debug.Assert(condition, message);
           // if (!condition) throw new Exception( "AssertionFailed:" + message );
        }


        public static void LogToFile(string message)
        {
            LogToFile( "LogFile.txt", message );
        }

        public static void LogToFile(string filename, string message)
        {
            LogToFile( filename, message, true );
        }

        public static void LogToFile(string filename, string message, bool append)
        {
            lock (thisLock)
            {
            StreamWriter sw = new StreamWriter( filename, append );
            sw.WriteLine( message );
            sw.Flush();
            sw.Close();
        }
        }


        public static void PrintTableCustom(System.Data.DataTable table, string header, string filename)
        {
            lock (thisLock)
            {
            StreamWriter sw = new StreamWriter( filename, false );
            sw.WriteLine( header );
            foreach (System.Data.DataRow aRow in table.Rows)
            {
                if ((aRow.RowState != DataRowState.Deleted) && (aRow.RowState != DataRowState.Detached))
                {
                    sw.WriteLine( aRow.ToString());
                }
            }
            sw.Flush();
            sw.Close();
        }
        }

        public static void PrintTable(System.Data.DataTable table)
        {
            PrintTable( table, table.TableName);
        }

        public static void PrintTable(System.Data.DataTable table, string filename)
        {
            lock (thisLock)
            {

            StreamWriter sw = new StreamWriter( filename, false );
            foreach (System.Data.DataColumn aCol in table.Columns)
            {
                sw.Write( aCol.ColumnName + "\t" );
            }
            sw.WriteLine();
            foreach (System.Data.DataRow aRow in table.Rows)
            {
                if ((aRow.RowState != DataRowState.Deleted) && (aRow.RowState != DataRowState.Detached))
                {
                    foreach (System.Data.DataColumn aCol in table.Columns)
                    {
                        string val = "";
                        if (aRow[aCol] != null) val = aRow[aCol].ToString();
                        sw.Write( val + "\t" );
                    }
                    sw.WriteLine();
                }
            }
            sw.Flush();
            sw.Close();
        }
        }


	}
}
