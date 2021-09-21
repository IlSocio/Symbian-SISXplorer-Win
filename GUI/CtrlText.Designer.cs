namespace SISXplorer
{
    partial class CtrlText
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
            this.richTextView = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextView
            // 
            this.richTextView.BackColor = System.Drawing.SystemColors.Info;
            this.richTextView.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextView.DetectUrls = false;
            this.richTextView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextView.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextView.Location = new System.Drawing.Point(0, 0);
            this.richTextView.Name = "richTextView";
            this.richTextView.ReadOnly = true;
            this.richTextView.Size = new System.Drawing.Size(150, 150);
            this.richTextView.TabIndex = 2;
            this.richTextView.Text = "";
            this.richTextView.WordWrap = false;
            // 
            // CtrlText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richTextView);
            this.Name = "CtrlText";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextView;
    }
}
