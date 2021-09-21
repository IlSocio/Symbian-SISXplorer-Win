using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using EpocData.RSC;
using href.Utils;


namespace SISXplorer
{
    public partial class CtrlUniRes : UserControl
    {
        private int _id = 0;

        protected CtrlUniRes()
        {
            InitializeComponent();
        }


        public CtrlUniRes(int id, Resource res)
        {
            InitializeComponent();
            ID = id;
            checkBox2.Checked = res.isCompressed;
            // this.SuspendLayout();
            // this.groupBox3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            lblRuns.Text = "";
            if (res.isCompressed)
            {
                lblRuns.Text = res.TotChunks + " Runs";
            }
            lblOffset.Text = "Offset: " + ConfigSettings.GetHex(res.offset);

            int heightToAdd = 0;
            for (int i = 0; i < res.TotChunks; i++)
            {
                if (heightToAdd > 0) heightToAdd += 6;
                Chunk chunk = res.GetChunk(i);

/*                if (chunk.data.Length>6 && enc == EncodingTools.DetectInputCodepage(chunk.data))
                {
                    CtrlText ctrlText = new CtrlText();
                    ctrlText.Dock = System.Windows.Forms.DockStyle.Top;
                    ctrlText.Location = new System.Drawing.Point(3, 3);
                    ctrlText.Name = "ctrlHex" + i;
                    ctrlText.Size = new System.Drawing.Size(244, 21);
                    heightToAdd += 20;
                    ctrlText.ShowData(enc.GetString(chunk.data));
                    this.flowLayoutPanel1.Controls.Add(ctrlText);
                }
                else
                {*/
                    byte[] data = chunk.data;
                    // TODO: se data puo' essere rappresentata come stringa unicode allora usa una textbox invece dell'hexview

                    int totLines = (int)Math.Ceiling((double)data.Length / 8);
                    CtrlHex ctrlHex = new CtrlHex();
                    ctrlHex.DataWidth = 8;
                    ctrlHex.OffsetWidth = 0;
                    ctrlHex.ShowProgress = false;
                    ctrlHex.Dock = System.Windows.Forms.DockStyle.Top;
                    ctrlHex.Location = new System.Drawing.Point(3, 3);
                    ctrlHex.Name = "ctrlHex" + i;
                    ctrlHex.Size = new System.Drawing.Size(244, 21);

                    Font tempFont = ctrlHex.Font;
                    int Margin = ctrlHex.Bounds.Height - ctrlHex.ClientSize.Height;
                    int newHeight = (TextRenderer.MeasureText(" ", tempFont).Height * totLines) + Margin + 2;
                    ctrlHex.Height = newHeight + 4;
                    heightToAdd += newHeight + 4;

                    this.flowLayoutPanel1.Controls.Add(ctrlHex);
                    ctrlHex.ShowData(data);
               // }
            }
            this.Height += heightToAdd;
//                        this.ResumeLayout();
            //            this.groupBox3.ResumeLayout();
                     this.flowLayoutPanel1.ResumeLayout();
            // ID = res.
            // TODO: In base alla risorsa, aggiungi o togli elementi... e modifica la grandezza del componente...
        }


        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                this.groupBox3.Text = " ID: " + value + " ";
            }
        }
    }
}
