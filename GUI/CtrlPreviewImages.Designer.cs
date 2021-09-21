namespace SISXplorer
{
    partial class CtrlPreviewImages
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtCounter = new System.Windows.Forms.ToolStripTextBox();
            this.labQty = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.labXY = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExtr = new System.Windows.Forms.ToolStripButton();
            this.btnExtrAll = new System.Windows.Forms.ToolStripButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.extractSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
//            this.axRenesisCtrl1 = new AxRenesisXLib.AxRenesisCtrl();
            this.ctrlThumb1 = new SISXplorer.CtrlThumb();
            this.ctrlThumList1 = new SISXplorer.JThumbnailView();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.axRenesisCtrl1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.txtCounter,
            this.labQty,
            this.toolStripLabel1,
            this.toolStripSeparator2,
            this.labXY,
            this.toolStripSeparator3,
            this.btnExtr,
            this.btnExtrAll});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(433, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::SISXplorer.Properties.Resources.prev;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Previous Frame";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::SISXplorer.Properties.Resources.next;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Next Frame";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // txtCounter
            // 
            this.txtCounter.AcceptsReturn = true;
            this.txtCounter.Name = "txtCounter";
            this.txtCounter.Size = new System.Drawing.Size(30, 25);
            this.txtCounter.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCounter.Leave += new System.EventHandler(this.txtCounter_Leave);
            this.txtCounter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCounter_KeyPress);
            // 
            // labQty
            // 
            this.labQty.Name = "labQty";
            this.labQty.Size = new System.Drawing.Size(17, 22);
            this.labQty.Text = "/0";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(42, 22);
            this.toolStripLabel1.Text = "Frames";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // labXY
            // 
            this.labXY.Name = "labXY";
            this.labXY.Size = new System.Drawing.Size(37, 22);
            this.labXY.Text = "0x0x0";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExtr
            // 
            this.btnExtr.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExtr.Image = global::SISXplorer.Properties.Resources.ExtrImg;
            this.btnExtr.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExtr.Name = "btnExtr";
            this.btnExtr.Size = new System.Drawing.Size(23, 22);
            this.btnExtr.Text = "Extract Frame...";
            this.btnExtr.Click += new System.EventHandler(this.btnExtr_Click);
            // 
            // btnExtrAll
            // 
            this.btnExtrAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnExtrAll.Image = global::SISXplorer.Properties.Resources.ExtrImgAll;
            this.btnExtrAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExtrAll.Name = "btnExtrAll";
            this.btnExtrAll.Size = new System.Drawing.Size(23, 22);
            this.btnExtrAll.Text = "Extract All Frames...";
            this.btnExtrAll.Click += new System.EventHandler(this.btnExtrAll_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Select Destination Folder For Extraction";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractSelectionToolStripMenuItem,
            this.extractAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(166, 48);
            // 
            // extractSelectionToolStripMenuItem
            // 
            this.extractSelectionToolStripMenuItem.Image = global::SISXplorer.Properties.Resources.ExtrImg;
            this.extractSelectionToolStripMenuItem.Name = "extractSelectionToolStripMenuItem";
            this.extractSelectionToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.extractSelectionToolStripMenuItem.Text = "Extract Frame...";
            this.extractSelectionToolStripMenuItem.Click += new System.EventHandler(this.extractSelectionToolStripMenuItem_Click);
            // 
            // extractAllToolStripMenuItem
            // 
            this.extractAllToolStripMenuItem.Image = global::SISXplorer.Properties.Resources.ExtrImgAll;
            this.extractAllToolStripMenuItem.Name = "extractAllToolStripMenuItem";
            this.extractAllToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.extractAllToolStripMenuItem.Text = "Extract All...";
            this.extractAllToolStripMenuItem.Click += new System.EventHandler(this.extractAllToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ctrlThumb1);
            // 
            // splitContainer1.Panel2
            // 
//            this.splitContainer1.Panel2.Controls.Add(this.axRenesisCtrl1);
            this.splitContainer1.Panel2.Controls.Add(this.ctrlThumList1);
            this.splitContainer1.Size = new System.Drawing.Size(433, 322);
            this.splitContainer1.SplitterDistance = 144;
            this.splitContainer1.TabIndex = 2;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(179, 70);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+A";
            this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = global::SISXplorer.Properties.Resources.ExtrImgAll;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(178, 22);
            this.toolStripMenuItem1.Text = "Extract Selection...";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Image = global::SISXplorer.Properties.Resources.ExtrImgAll;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(178, 22);
            this.toolStripMenuItem2.Text = "Extract All...";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.extractAllToolStripMenuItem_Click);
/*            // 
            // axRenesisCtrl1
            // 
            this.axRenesisCtrl1.ContextMenuStrip = this.contextMenuStrip1;
            this.axRenesisCtrl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.axRenesisCtrl1.Enabled = true;
            this.axRenesisCtrl1.Location = new System.Drawing.Point(258, 0);
            this.axRenesisCtrl1.Name = "axRenesisCtrl1";
            this.axRenesisCtrl1.Size = new System.Drawing.Size(171, 170);
            this.axRenesisCtrl1.TabIndex = 7;
            this.axRenesisCtrl1.ContextMenuStripChanged += new System.EventHandler(this.axRenesisCtrl1_ContextMenuStripChanged);
            this.axRenesisCtrl1.MouseCaptureChanged += new System.EventHandler(this.axRenesisCtrl1_MouseCaptureChanged);*/
            // 
            // ctrlThumb1
            // 
            this.ctrlThumb1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlThumb1.Location = new System.Drawing.Point(3, 3);
            this.ctrlThumb1.Name = "ctrlThumb1";
            this.ctrlThumb1.Size = new System.Drawing.Size(423, 134);
            this.ctrlThumb1.TabIndex = 0;
            // 
            // ctrlThumList1
            // 
            this.ctrlThumList1.ContextMenuStrip = this.contextMenuStrip2;
            this.ctrlThumList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlThumList1.Location = new System.Drawing.Point(0, 0);
            this.ctrlThumList1.Name = "ctrlThumList1";
            this.ctrlThumList1.Size = new System.Drawing.Size(429, 170);
            this.ctrlThumList1.TabIndex = 6;
            this.ctrlThumList1.ThumbBorderColor = System.Drawing.Color.LightGray;
            this.ctrlThumList1.AfterSelectItem += new SISXplorer.JThumbnailView.AfterSelectEventHandler(this.ctrlThumList1_AfterSelectItem);
            // 
            // CtrlPreviewImages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "CtrlPreviewImages";
            this.Size = new System.Drawing.Size(433, 347);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.axRenesisCtrl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox txtCounter;
        private System.Windows.Forms.ToolStripLabel labQty;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel labXY;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnExtr;
        private System.Windows.Forms.ToolStripButton btnExtrAll;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem extractSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractAllToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private SISXplorer.JThumbnailView ctrlThumList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private CtrlThumb ctrlThumb1;
//        private AxRenesisXLib.AxRenesisCtrl axRenesisCtrl1;
    }
}
