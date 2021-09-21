using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using EpocData.RSC;
using SISXplorer.Properties;
using Utility;
using EpocData;
using System.Threading;


namespace SISXplorer
{

    public partial class CtrlRSC : UserControl
    {
        private Semaphore sem;
        private RSCFile rscFile;
        private RSCFile newRscFile;


        public CtrlRSC()
        {
            InitializeComponent();
            sem = new Semaphore(0, 1);
        }


        public void Clear()
        {
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.Controls.Clear();
            this.flowLayoutPanel1.ResumeLayout();
        }


        public void ShowInfo(RSCFile aFile)
        {

            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
                newRscFile = aFile;
                return;
            }
            rscFile = aFile;
            Clear();
            backgroundWorker1.RunWorkerAsync(aFile);
            /*
            this.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            int i = 0;
            foreach (Resource res in aFile.resources)
            {
                this.flowLayoutPanel1.Controls.Add( new CtrlUniRes(i, res));
                i++;
            }
            this.flowLayoutPanel1.ResumeLayout();
            this.ResumeLayout();
            */
        }



        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < rscFile.resources.Count; i++)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                backgroundWorker1.ReportProgress(0, i);
                sem.WaitOne();
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int i = (int) e.UserState;
//            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.Controls.Add(new CtrlUniRes(i, rscFile.resources[i]));
            sem.Release();
//            this.flowLayoutPanel1.ResumeLayout();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                if (newRscFile != null)
                {
                    this.ShowInfo(newRscFile);
                    return;
                }
            }
            newRscFile = null;
        }
    }
}
