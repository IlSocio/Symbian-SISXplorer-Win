using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SISXplorer
{
    public partial class CtrlBox : UserControl
    {
        public CtrlBox()
        {
            InitializeComponent();
        }

        public string Label
        {
            get
            {
                return this.label2.Text;
            }
            set
            {
                label2.Text = value;
//                this.Refresh();
            }
        }

        public override string Text
        {
            get
            {
                return textBox2.Text;
            }
            set
            {
                textBox2.Text = value;                
            }
        }
    }
}
