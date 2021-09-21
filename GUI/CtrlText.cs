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


namespace SISXplorer
{
    public partial class CtrlText : UserControl
    {

        public CtrlText()
        {
            InitializeComponent();
        }


        public void Clear()
        {
            richTextView.Clear();
        }


        public void ShowData(string s)
        {
            richTextView.Clear();
            richTextView.Text = s;
        }

        
        public void ShowData(byte[] data, Encoding enc)
        {
            ShowData(enc.GetString(data));
        }


        private void richHexView_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
