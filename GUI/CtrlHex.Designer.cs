namespace SISXplorer
{
    partial class CtrlHex
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.showHexWorker = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.richHexView = new System.Windows.Forms.RichTextBox();
            this.colorHexWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // showHexWorker
            // 
            this.showHexWorker.WorkerReportsProgress = true;
            this.showHexWorker.WorkerSupportsCancellation = true;
            this.showHexWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork_1);
            this.showHexWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted_1);
            this.showHexWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged_1);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 127);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(150, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // richHexView
            // 
            this.richHexView.BackColor = System.Drawing.SystemColors.Info;
            this.richHexView.Cursor = System.Windows.Forms.Cursors.Default;
            this.richHexView.DetectUrls = false;
            this.richHexView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richHexView.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richHexView.Location = new System.Drawing.Point(0, 0);
            this.richHexView.Name = "richHexView";
            this.richHexView.ReadOnly = true;
            this.richHexView.Size = new System.Drawing.Size(150, 127);
            this.richHexView.TabIndex = 2;
            this.richHexView.Text = "";
            this.richHexView.WordWrap = false;
            // 
            // colorHexWorker
            // 
            this.colorHexWorker.WorkerReportsProgress = true;
            this.colorHexWorker.WorkerSupportsCancellation = true;
            this.colorHexWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.colorHexWorker_DoWork);
            this.colorHexWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.colorHexWorker_RunWorkerCompleted);
            this.colorHexWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.colorHexWorker_ProgressChanged);
            // 
            // CtrlHex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richHexView);
            this.Controls.Add(this.progressBar1);
            this.Name = "CtrlHex";
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker showHexWorker;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RichTextBox richHexView;
        private System.ComponentModel.BackgroundWorker colorHexWorker;
    }
}
