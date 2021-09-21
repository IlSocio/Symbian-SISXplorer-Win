using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SISXplorer
{
    public partial class FrmAbout : Form
    {
        public FrmAbout()
        {
            InitializeComponent();
        }

        private void FrmAbout_DoubleClick(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void FrmAbout_Shown(object sender, EventArgs e)
        {
            this.CenterToParent();
        }
    }
}