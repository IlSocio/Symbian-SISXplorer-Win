using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SISX;
using SISX.Fields;
using System.IO;
using Utility;


namespace SISXplorer
{
    public partial class CtrlInfoSISX : UserControl
    {
        private SISController ctrl;


        public CtrlInfoSISX()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            ClearTextBox( this );
            txtScript.Clear();
            comboBox1.Items.Clear();
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


        public void ShowInfo(SISEntry sisEntry, SISController cnt) 
        {                        
            // sisEntry.sisFile
            ctrl = cnt;
            toolTip1.SetAdvToolTip(textBox1, sisEntry.sisFile.hdr.uid1);
            toolTip1.SetAdvToolTip(textBox3, sisEntry.sisFile.hdr.uid2);
            toolTip1.SetAdvToolTip(textBox2, sisEntry.sisFile.hdr.uid3);
            toolTip1.SetAdvToolTip(textBox4, sisEntry.sisFile.hdr.uidChecksum);
            SISInfo info = cnt.info;
            textBox5.Text = info.creationTime.ToString();
            toolTip1.SetAdvToolTip(textBox7, info.uid.uid);
            //textBox7.Text = info.uid.ToString();
            textBox8.Text = info.vendorName.ToString();
            textBox10.Text = info.version.ToString();

            string s = Bits.GetStringFromBitField<TInstallType>( info.installType );
            toolTip1.SetAdvToolTip(textBox15, s, info.installType);

            s = Bits.GetStringFromBitField<TInstallFlags>(info.installFlags);
            toolTip1.SetAdvToolTip(textBox16, s, info.installFlags);

            toolTip1.SetAdvToolTip(textBox11, sisEntry.sisFile.cnt.controllerCompressed.length);
            toolTip1.SetAdvToolTip(textBox12, sisEntry.sisFile.cnt.controllerCompressed.uncompressedDataSize);
            FileInfo fileInfo = new FileInfo(sisEntry.FileName);
            textBox18.Text = fileInfo.Name;
            toolTip1.SetAdvToolTip(textBox19, (ulong)fileInfo.Length);
            textBox20.Text = fileInfo.CreationTime.ToShortDateString() + "  " + fileInfo.CreationTime.ToShortTimeString();
            textBox21.Text = fileInfo.LastWriteTime.ToShortDateString() + "  " + fileInfo.LastWriteTime.ToShortTimeString();

            comboBox1.Items.Clear();
            foreach (SISLanguage lang in cnt.languages.languages.fields)
            {
                comboBox1.Items.Add( lang.ToString() );
            }
            comboBox1.SelectedIndex = 0;            
            /*            foreach (SISController cont in cnt.installBlock.embeddedSIS) 
                        {
                            cont.info.names;
                        }*/
            
/*            string s = "";
            foreach (SISProperty prop in cnt.properties.properties.fields)
            {
                s += prop.key + " = " + prop.value + "\n";
            }
            textBox17.Text = s;*/
            /*foreach (SISSignatureCertificateChain signChain in cnt.signatures)
            {                 
                //foreach (SISSignature signature in signChain.signatures.fields)
                //{
                //  signature.signatureAlgorithm.algorithmIdentifier; SHA1+RSA oppure SHA1+DSA
                //  ctrlHex1.ShowData( signature.signatureData.data );
                //}

                // dati in formato ASN.1 ( contiene X509 certificate )
               ctrlHex1.ShowData( signChain.certificateChain.certificateData.data );
            }*/
            // TODO: visualizza nel pannello info il sisx selezionato
            // Prerequisites: TarghetDevices, Dependencies
            // Properties: Coppie di key/value
            // ifBlocks per script di installazione
            // Eventuale embedded SIS... TODO...
            // Signatures Certificate Chain
        }

        private void CtrlInfoSISX_Load(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string script = ctrl.installBlock.ToString();
            int index = comboBox1.SelectedIndex;
            string txt13 = "";
            int i = 1;
            foreach (SISSupportedOption option in ctrl.options.options.fields)
            {
                string optName = "";
                if (index < option.names.fields.Count)
                    optName = option.names.fields[index].ToString();

                if (txt13 != "") txt13 += "\r\n";
                txt13 += optName;
                script = script.Replace( "option" + i, "Option( \"" + optName + "\" )" );
                i++;
            }
            textBox13.Text = txt13;
            txtScript.Text = script;

            textBox9.Text = "";
            if (index < ctrl.info.vendorNames.fields.Count)
                textBox9.Text = ctrl.info.vendorNames.fields[index].ToString();

            textBox6.Text = "";
            if (index < ctrl.info.names.fields.Count)
                textBox6.Text = ctrl.info.names.fields[index].ToString();

        }
    }
}
