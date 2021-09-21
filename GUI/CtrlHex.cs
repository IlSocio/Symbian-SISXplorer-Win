using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using Utility;
using System.Threading;


namespace SISXplorer
{
    public partial class CtrlHex : UserControl
    {
        private Semaphore sem;
        private int _offsetWidth = 6;
        private int _dataWidth = 16;
        private byte[] newData;
        private int lineLength;

        public CtrlHex()
        {
            InitializeComponent();
            sem = new Semaphore(0, 1);
        }


        public bool ShowProgress
        {
            get
            {
                return progressBar1.Visible;
            }
            set
            {
                this.progressBar1.Visible = value;
            }
        }

        public int OffsetWidth
        {
            get
            {
                return _offsetWidth;
            }   
            set
            {
                _offsetWidth = value;
            }
        }


        public int DataWidth
        {
            get
            {
                return _dataWidth;
            }
            set
            {
                _dataWidth = value;
            }
        }


        public void Clear()
        {
            newData = null;
            colorHexWorker.CancelAsync();
            showHexWorker.CancelAsync();
            richHexView.Clear();
        }

        public void ShowData(byte[] data)
        {
            richHexView.Clear();
//            newData = null;
            if (colorHexWorker.IsBusy)
            {
                newData = data;
                colorHexWorker.CancelAsync();
                return;
            }
            if (showHexWorker.IsBusy)
            {
                newData = data;
                showHexWorker.CancelAsync();
                return;
            }
            
            showHexWorker.RunWorkerAsync( data );
        }


/*        private void ApplyColors()
        {
            float size = (float)9;
            richHexView.SelectAll();
            richHexView.SelectionColor = Color.FromKnownColor( KnownColor.DarkGreen );
            int start = bv.OffsetWidth;
            int len = bv.HexWidth - bv.OffsetWidth;
            while (start < richHexView.Text.Length)
            {
                richHexView.Select( start, len );                
                richHexView.SelectionColor = Color.Black;
                start += bv.TotalWidth + 1; // crlf
            }
        }/**/


        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            //            Debug.WriteLine(this.Name + " Show HEX BEGIN ");
            BackgroundWorker worker = sender as BackgroundWorker;
            byte[] data = e.Argument as byte[];
            IEnumerator iter = BinaryView.GetEnumerator( data, OffsetWidth, DataWidth );
            int processed = 0;
            int oldPerc = 0;
            int perc= 0;
            StringBuilder sb = new StringBuilder(10000);// Con StringBuilder e' passato da 24 a 3
            while (iter.MoveNext())
            {
                sb.Append( iter.Current.ToString()+"\n" );

                processed += 16;
                oldPerc = perc;
                double d = processed * 100;
                d /= data.Length;
                perc = processed * 100 / data.Length;
                //Debug.WriteLine("Perc:" + perc + "  OldPerc:" + oldPerc + " D:"+d + " totLen:"+data.Length);
                //*****************
                if (perc != oldPerc)
                {
                    //                    Debug.WriteLine(this.Name + " Report Progress " +perc);
                    worker.ReportProgress(perc);
                    sem.WaitOne();
                }
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
            }
            if (sb.Length>0)
                sb.Length = sb.Length - 1;
            e.Result = sb.ToString();
        }


        private void backgroundWorker1_ProgressChanged_1(object sender, ProgressChangedEventArgs e)
        {
            Application.DoEvents();
//            Debug.WriteLine(this.Name + " Progress Changed "+e.ProgressPercentage);
            if (e.ProgressPercentage > 100) 
                progressBar1.Value = 100;
            else
                progressBar1.Value = e.ProgressPercentage;
            sem.Release();
        }


        private void backgroundWorker1_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
//            Debug.WriteLine(this.Name + " Show HEX END ");
            
            if (e.Cancelled)
            {
                progressBar1.Value = 0;
                if (newData != null)
                    showHexWorker.RunWorkerAsync( newData );
                return;
            }
            newData = null;
            richHexView.Text = e.Result as string;

/*            if (AutoSize)
            {
                Font tempFont = richHexView.Font;
                int textLength = richHexView.Text.Length;
                int textLines = richHexView.GetLineFromCharIndex(textLength) + 1;
                int Margin = richHexView.Bounds.Height - richHexView.ClientSize.Height;
                int newHeight = (TextRenderer.MeasureText(" ", tempFont).Height * textLines) + Margin + 2;
                this.Height = newHeight + progressBar1.Height + 2;
                this.Size = new Size(this.Width, newHeight + progressBar1.Height +2);
            }*/

            progressBar1.Value = 0;

            lineLength = richHexView.Text.IndexOf( '\n'  );
 //           Debug.WriteLine(this.Name + " Completed: " + progressBar1.Value);
            //            Debug.WriteLine("***" + this.Name + richHexView.Text.Substring(0, lineLength) + "***");

            //colorHexWorker.RunWorkerAsync(richHexView.Text.Length);/**/
        }


        public Font Font
        {
            get
            {
                return richHexView.Font;
            }
        }        

/*        public void Fit()
        {
            Font tempFont = richHexView.Font;
            int textLength = richHexView.Text.Length;
            int textLines = richHexView.GetLineFromCharIndex(textLength) + 1;
            int Margin = richHexView.Bounds.Height - richHexView.ClientSize.Height;
            int newHeight = (TextRenderer.MeasureText(" ", tempFont).Height * textLines) + Margin + 2;
            if (ShowProgress)
                newHeight += progressBar1.Height;
            this.Height = newHeight + 2;
        }*/


        private void richHexView_TextChanged(object sender, EventArgs e)
        {

        }


        private void colorHexWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int totLen = (int)e.Argument;
            if (lineLength < 1) return;
            BackgroundWorker worker = sender as BackgroundWorker;
            int start = OffsetWidth;
            int perc = 0;
            while (start < totLen)
            {
                perc = start * 100 / totLen;
//                Debug.WriteLine(this.Name + "\tDo Report:" + perc + "\tstart:" + start + "\ttotLen:" + totLen);
                sem.WaitOne();
                worker.ReportProgress(perc, start);
//                System.Threading.Thread.Sleep(100);
                start += lineLength + 1; //crlf
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
            }/**/
        }

        private void colorHexWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            /*            int wid = lineLength - DataWidth - OffsetWidth;
                        int totLen = (int)e.UserState;
                        int perc = e.ProgressPercentage;
                        long start = perc * 100 / totLen;
                        if (e.ProgressPercentage > 100)
                            progressBar1.Value = 100;
                        else
                            progressBar1.Value = e.ProgressPercentage;
                        for (int j = 0; j < 10000; j++)
                        {
                            richHexView.Select((int)start, wid);
                            start += lineLength + 1;
                        }
                        richHexView.SelectionColor = Color.DarkGreen;*/

            int wid = lineLength - DataWidth - OffsetWidth;
            int start = (int)e.UserState;
            // BackgroundWorker bw = sender as BackgroundWorker;
            // bw.CancelAsync();
            // Debug.WriteLine(this.Name + " ProgressChanged:" + e.ProgressPercentage + "\t" + start + "\t" + wid);
            //return;
            if (e.ProgressPercentage > 100)
                progressBar1.Value = 100;
            else
                progressBar1.Value = e.ProgressPercentage;
            richHexView.Select(start, wid);
            richHexView.SelectionColor = Color.DarkGreen;
            sem.Release();
            //            System.Threading.Thread.Sleep(1000);
            //richHexView.SelectionColor = Color.Black;*/
        }

        private void colorHexWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                progressBar1.Value = 0;
                if (newData != null) 
                    showHexWorker.RunWorkerAsync( newData );
                return;
            }
            newData = null;
            //System.Diagnostics.Debug.WriteLine( "END Color" );
            progressBar1.Value = 0;
        }
    }
}
