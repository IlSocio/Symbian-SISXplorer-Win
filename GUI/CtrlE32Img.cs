using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using EpocData.E32Image;
using SISXplorer.Properties;
using Utility;
using EpocData;


namespace SISXplorer
{

    public partial class CtrlE32Img : UserControl
    {
        public CtrlE32Img()
        {
            InitializeComponent();
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
                ClearTextBox(ctrl1);
        }


        public void Clear()
        {
            // TODO: Svuota tutti i campi
            ClearTextBox(this);
            ctrlShowCapab1.ShowCapabilities(0);
        }


        public void ShowInfo(E32File aFile)
        {
            toolTip1.SetAdvToolTip(txtUid1, aFile.uid1);
            toolTip1.SetAdvToolTip(txtUid2, aFile.uid2);
            toolTip1.SetAdvToolTip(txtUid3, aFile.uid3);
            toolTip1.SetAdvToolTip(txtUidCrc, aFile.uidCrc);

            string s_uid1 = Bits.GetStringFromEnum<UID1s>(aFile.uid1);
            string s_uid2 = Bits.GetStringFromEnum<UID2s>(aFile.uid2);
            string s_target = Bits.GetStringFromEnum<UID2s_TargetType>(aFile.uid2);

            // UID1
            if (s_uid1 != "")
                toolTip1.SetAdvToolTip(txtUid1, s_uid1, aFile.uid1);

            // UID2
            if (s_uid2 != "")
            {
                toolTip1.SetAdvToolTip(txtUid2, s_uid2, aFile.uid2);
                if (s_target != "") 
                {
                    string tt = toolTip1.GetToolTip(txtUid2);
                    toolTip1.SetToolTip(txtUid2, "TargetType: " + s_target + "\n" + tt);
                }
            }

            toolTip1.SetAdvToolTip(txtHeapMin, aFile.iHeapSizeMin);
            toolTip1.SetAdvToolTip(txtHeapMax, aFile.iHeapSizeMax);
            toolTip1.SetAdvToolTip(txtStack, aFile.iStackSize);
            toolTip1.SetAdvToolTip(txtBss, aFile.iBssSize);

            toolTip1.SetAdvToolTip(txtModVer, aFile.iModuleVersion);
            toolTip1.SetAdvToolTip(txtPetVer, aFile.iToolsVersion);            
//            txtTime = aFile.iTimeHi + aFile.iTimeLo

/*            long longDate;
                                    longDate <<= 32;
                                    longDate += 0x00e0eb0a;
 * longDate = 0x00e0eb0a;
                                                longDate <<= 32;
                                                longDate += 0xd2525b80;
 */
            long one = aFile.iTimeHi; // 0x00e10af8     // 00e10e5f
            long two = aFile.iTimeLo; // 0x634c2780     // 4c055f00
            long oneTwo = one;
            oneTwo <<= 32;
            oneTwo += two;
            oneTwo /= 1000000;
            oneTwo -= 3600;
            long sub = 730497 * 24;
            sub *= 3600;
            oneTwo -= sub;
            DateTime time = DateTime.Parse("01/01/2000");
            time = time.AddSeconds(oneTwo);
            string s = time.ToShortDateString() + "  " + time.ToShortTimeString();
            txtTime.Text = s;
            
            toolTip1.SetAdvToolTip(txtFlags, aFile.iFlags);

            toolTip1.SetAdvToolTip(txtImp, aFile.iImportOffset);
            toolTip1.SetAdvToolTip(txtCodeReloc, aFile.iCodeRelocOffset);
            toolTip1.SetAdvToolTip(txtDataReloc, aFile.iDataRelocOffset);

            toolTip1.SetAdvToolTip(txtCodeSize, aFile.iCodeSize);
            toolTip1.SetAdvToolTip(txtDataSize, aFile.iDataSize);
            toolTip1.SetAdvToolTip(txtEntryPoint, aFile.iEntryPoint);
            toolTip1.SetAdvToolTip(txtCodeBase, aFile.iCodeBase);
            toolTip1.SetAdvToolTip(txtDataBase, aFile.iDataBase);

            toolTip1.SetAdvToolTip(txtExpOff, aFile.iExportDirOffset);
            toolTip1.SetAdvToolTip(txtExpCount, aFile.iExportDirCount);
            toolTip1.SetAdvToolTip(txtTextSize, aFile.iTextSize);
            toolTip1.SetAdvToolTip(txtCodeOff, aFile.iCodeOffset);
            toolTip1.SetAdvToolTip(txtDataOff, aFile.iDataOffset);

            s = Bits.GetStringFromEnum<TProcessPriority>(aFile.iProcessPriority);
            if (s == "") 
            {
                s = GetPrioDescr(aFile.iProcessPriority);
            }
            toolTip1.SetAdvToolTip(txtPriority, s, aFile.iProcessPriority);

            s = Bits.GetStringFromEnum<TCpu>(aFile.iCpuIdentifier);
            toolTip1.SetAdvToolTip(txtCPU, s, aFile.iCpuIdentifier);
            toolTip1.SetAdvToolTip(txtSecure, aFile.iSecureID);
            toolTip1.SetAdvToolTip(txtVendor, aFile.iVendorID);

            ctrlShowCapab1.ShowCapabilities(aFile.iCaps);
        }


        private string GetPrioDescr(int prio)
        {
            uint prevItem = 0;
            foreach (int item in Enum.GetValues(typeof(TProcessPriority)))
            {
                if (item > prio)
                {
                    string prevString = Bits.GetStringFromEnum<TProcessPriority>((uint)prevItem);
                    if (prevString == "")
                        prevString = "EPriorityLow-" + ConfigSettings.GetHex((ulong) (TProcessPriority.EPriorityLow-prio));
                    else
                        prevString += "+" + ConfigSettings.GetHex((ulong)(prio - prevItem));
                    return prevString;
                }
                prevItem = (uint)item;
            }
            return "";
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
