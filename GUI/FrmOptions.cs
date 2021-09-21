using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SISXplorer.Properties;


namespace SISXplorer
{
    public partial class FrmOptions : Form
    {
        public FrmOptions()
        {
            InitializeComponent();
        }

        private void FrmAbout_Shown(object sender, EventArgs e)
        {
            this.CenterToParent();
            checkBox2.Checked = Settings.Default.InfoSIS;
            checkBox3.Checked = Settings.Default.InfoFile;
            checkBox1.Checked = Settings.Default.HexViewer;
            checkBox4.Checked = Settings.Default.RSCViewer;
            checkBox5.Checked = Settings.Default.MIFViewer;
            checkBox6.Checked = Settings.Default.MBMViewer;
            checkBox7.Checked = Settings.Default.E32Image;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.InfoSIS = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.InfoFile = checkBox3.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.HexViewer = checkBox1.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.RSCViewer = checkBox4.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.MIFViewer = checkBox5.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.MBMViewer = checkBox6.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.E32Image = checkBox7.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Settings.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Settings.Default.Reload();
        }
    }
}