using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SISX;
using SISX.Fields;
using Utility;



namespace SISXplorer
{
    public partial class CtrlInfoFile : UserControl
    {
        public CtrlInfoFile()
        {
            InitializeComponent();
            Clear();
            ctrlHexHash.ShowProgress = false;
        }


/*        private void ShowOperation(uint op)
        {
            foreach (int item in Enum.GetValues( typeof( TSISFileOperation ) ))
            {
                int mask = (int)(item & op);
                if (mask == item)
                {                    
                    TSISFileOperation oper = (TSISFileOperation) (mask);
                    if (s != "") s += "; ";
                    s += oper.ToString();
                }
            }
            textBox1.Text = s;
        }*/
        

        private string GetOperationOptions(uint op)
        {
            string s = Bits.GetStringFromBitField<TSISFileOperationOption>( op );
            s += Bits.GetStringFromBitField<TInstFileRunOption>( op );
            s += Bits.GetStringFromBitField<TInstTextOption>( op );
            return s;
        }


        private void ClearTextBox(Control ctrl)
        {
            if (ctrl == null) return;
            if (ctrl is TextBox)
            {
                // In base a tag ed a value imposta il flag...
                TextBox txt = ctrl as TextBox;
                txt.Clear();
                return;
            }
            foreach (Control ctrl1 in ctrl.Controls)
                ClearTextBox( ctrl1 );
        }


        public void Clear()
        {
            ctrlHexHash.Clear();
            ClearTextBox( this );
            ctrlShowCapab1.ShowCapabilities( null );
        }


        public void ShowInfo(SISFileDescription fileDescr)
        {
            ctrlHexHash.ShowData( fileDescr.hash.hashData.data );
            string s = Bits.GetStringFromBitField<TSISFileOperation>( fileDescr.operation );
            toolTip1.SetAdvToolTip(textBox1, s, fileDescr.operation);

            s = GetOperationOptions(fileDescr.operationOptions);
            toolTip1.SetAdvToolTip(textBox3, s, fileDescr.operationOptions);

            textBox2.Text = fileDescr.target.ToString();
            toolTip1.SetAdvToolTip(textBox7, fileDescr.compressedLength);
            toolTip1.SetAdvToolTip(textBox8, fileDescr.uncompressedLength);
            //textBox6.Text = fileDescr.hash.hashData.data;
            //textBox3.Text = fileDescr.operationOptions.ToString();
            textBox4.Text = fileDescr.mimeType.ToString();
            toolTip1.SetAdvToolTip(textBox9, fileDescr.fileIndex);
            ctrlShowCapab1.ShowCapabilities( fileDescr.capabilities );
        }


        private void CtrlInfoFile_Load(object sender, EventArgs e)
        {
        }
    }
}
