using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SISXplorer
{
    public abstract partial class AbstractCtrl : UserControl
    {
        public AbstractCtrl()
        {
            InitializeComponent();
        }

        public abstract void Clear();

        public abstract void ShowData(object obj);
    }
}
