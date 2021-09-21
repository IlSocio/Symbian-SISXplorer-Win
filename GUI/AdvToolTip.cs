using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;


namespace SISXplorer
{
    public partial class AdvToolTip : System.Windows.Forms.ToolTip 
    {
        public AdvToolTip() : base()
        {
            InitializeComponent();
        }


        public AdvToolTip(IContainer container) : base(container)
        {
            container.Add(this);
            InitializeComponent();
        }


        private string GetHex(ulong val)
        {
            return ConfigSettings.GetHex(val);
        }


        public void SetAdvToolTip(Control ctrl, ulong value)
        {
/*            this.AutomaticDelay = 5000;
            this.UseAnimation = true;
            this.AutoPopDelay = 7000;
            this.InitialDelay = 0;
            this.ReshowDelay = 0;
            this.UseAnimation = false;
            this.UseFading = false;*/
            //            this.ShowAlways = true;
            ctrl.Text = GetHex(value);
            this.SetToolTip(ctrl, "Dec: " + value);
        }


        public void SetAdvToolTip(Control ctrl, string descr, ulong value)
        {
/*            this.AutoPopDelay = 7000;
            this.InitialDelay = 0;
            this.ReshowDelay = 0;
            this.UseAnimation = false;
            this.UseFading = false;*/
            //            this.ShowAlways = true;
            ctrl.Text = descr;
            this.SetToolTip(ctrl, "Hex: " + GetHex(value) + "\nDec: " + value);
        }
    }
}
