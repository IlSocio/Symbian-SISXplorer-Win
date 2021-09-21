using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EpocData;
using EpocData.MBM;
using Utility;
using System.Threading;


namespace SISXplorer
{
    public partial class DlgExtract : Form
    {
        private Semaphore sem;
        private List<IImage> mbmFile;
        private string path;
        private int[] filter;
//        private bool cancelled = false;

        protected DlgExtract()
        {
            InitializeComponent();
            sem = new Semaphore(0,1);
        }

        public DlgExtract(List<IImage> aMbmFile, ICollection indexes, string aPath)
            : this()
        {
            if (indexes != null)
            {
                filter = new int[indexes.Count];
                int i = 0;
                foreach (int num in indexes)
                {
                    filter[i] = num;
                    i++;
                }
            }
            else
            {
                filter = new int[aMbmFile.Count];
                for (int i = 0; i < aMbmFile.Count; i++)
                    filter[i] = i;
            }
            mbmFile = aMbmFile;
            path = aPath;
            if (!path.EndsWith( "\\" )) path += "\\";
            label1.Text = path;
        }

        public DlgExtract(List<IImage> aMbmFile, string aPath)
            : this( aMbmFile, null, aPath )
        {
        }


        /*public void ExtractAllTo(MBMFile mbmFile, string path)
        {
        }


        public void ExtractAllTo(MBMFile mbmFile, string path)
        {
        }*/


        private void exportWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            ExportEventArgs args = e.Argument as ExportEventArgs;
            List<IImage> images = args.file;
            string path = args.path;
            int processed = 0;
            foreach (int index in filter)
            {
                IImage img = mbmFile[index] as IImage;
                processed++;
                img.SaveTo( path + (index+1));
                int perc = processed * 100 / images.Count;
                worker.ReportProgress(perc, processed);
                sem.WaitOne();

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
            }
            e.Result = processed;
        }

        private void exportWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int num = (int)(e.UserState);
            if (e.ProgressPercentage > 100)
                progressBar1.Value = 100;
            else
                progressBar1.Value = e.ProgressPercentage;
            this.Text = "Extraction... " + progressBar1.Value + "%";
            this.label2.Text = num + "/" + mbmFile.Count;
            sem.Release();
        }

        private void exportWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Visible = false;
            button2.Visible = true;
            progressBar1.Value = 0;
            if (e.Cancelled)
            {
                label2.Text = this.Text = "Extraction Cancelled!";
                this.DialogResult = DialogResult.Cancel;
                return;
            }
            label2.Text = e.Result + " Images Extracted";
            this.Text = "Extraction Completed!";
        }

        private void DlgExtract_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {   // Cancel
//            cancelled = true;
            exportWorker.CancelAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {   // OK
            this.DialogResult = DialogResult.OK;
        }

        private void DlgExtract_Activated(object sender, EventArgs e)
        {

        }

        private void DlgExtract_Shown(object sender, EventArgs e)
        {
            exportWorker.RunWorkerAsync(new ExportEventArgs(mbmFile, path));
        }


        class ExportEventArgs
        {
            public List<IImage> file;
            public string path;

            public ExportEventArgs(List<IImage> aFile, string aPath)
            {
                file = aFile;
                path = aPath;
            }
        }

    
    }
}