using System;using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SISX.Fields;


namespace SISXplorer
{
    public partial class CtrlShowCapab : UserControl
    {
        private static Font fntBold;
        private static Font fntRegular;

        public CtrlShowCapab()
        {
            InitializeComponent();
            fntBold = new Font( checkBox10.Font, FontStyle.Bold );
            fntRegular = new Font( checkBox10.Font, FontStyle.Regular | FontStyle.Italic );
        }


        private void ShowCapabilities(Control ctrl, uint value)
        {
            if (ctrl == null) return;
            if (ctrl is CheckBox)
            {
                // In base a tag ed a value imposta il flag...
                CheckBox chk = ctrl as CheckBox;
                int tagId = int.Parse(chk.Tag.ToString());
                uint mask = (uint)(1 << tagId);
                uint ris = mask & value;
                chk.Checked = ( ris > 0 );

                // Imposta il font
                if (chk.Checked)
                    chk.Font = fntBold;
                else
                    chk.Font = fntRegular;
                return;
            }
            foreach (Control ctrl1 in ctrl.Controls)
                ShowCapabilities(ctrl1, value);
        }


        public void ShowCapabilities(uint value)
        {
            ShowCapabilities(this, value);
        }


        public void ShowCapabilities(SISCapabilities capab)
        {
            uint value = 0;
            if (capab != null)
            {
                if (capab.capabilities.Length > 1)
                {
                    throw new Exception("Error Capabilitites");
                }
                value = capab.capabilities[0];
            }
            ShowCapabilities(value);
        }


        private void CtrlShowCapab_Load(object sender, EventArgs e)
        {

        }
    }
}
