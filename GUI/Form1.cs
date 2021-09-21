using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SISX;
using SISX.Fields;
using System.Collections;
using Utility;
using EpocData;
using EpocData.MBM;
using EpocData.E32Image;
using EpocData.MIF;
using EpocData.RSC;
using href.Utils;
using SISXplorer.Properties;


namespace SISXplorer
{

    public partial class Form1 : Form
    {
        private RecentList recent;
        private FrmAbout splash;

        public Form1()
        {            
            InitializeComponent();
            splash = new FrmAbout();
            recent = new RecentList( "Symbian-Toys", "SISXplorer" );
            updateRecentListGUI();            
        }


        private void updateRecentListGUI()
        {
            recentFilesToolStripMenuItem.DropDownItems.Clear();
            foreach (string recentFile in recent.GetRecentList())
            {
                ToolStripMenuItem recentItem = new System.Windows.Forms.ToolStripMenuItem();
                recentItem.Text = recentFile;
                recentItem.Click += new System.EventHandler( this.recentItem_Click );
                recentFilesToolStripMenuItem.DropDownItems.Add( recentItem );
                //                ToolStripItem itm = new ToolStripItem();                
            }
        }

        /// <summary>
        /// Cambio Visualizzazione TreeView On/Off
        /// </summary>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tree.ShowUsingTree = btnTreeOn.Checked;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.ShowHideTabs();
            ctrlHexFile.ShowProgress = true; // Non so xche' ma di tanto in tanto torna ad essere false...
            Size = Properties.Settings.Default.Size;
            tree.ShowUsingTree = Properties.Settings.Default.TreeView;
            btnTreeOn.Checked = Properties.Settings.Default.TreeView;
            splitContainer1.SplitterDistance = Properties.Settings.Default.SplitterDistance;
            if (Properties.Settings.Default.Location.X == 0 && Properties.Settings.Default.Location.Y == 0)
                this.CenterToScreen();
            else
                Location = Properties.Settings.Default.Location;/**/

            List<string> files = new List<string>();
            
            bool skipExe = true;
            foreach (string s in Environment.GetCommandLineArgs())
            {
                if (skipExe) 
                {
                    skipExe = false;
                    continue;
                }
                files.Add(s);
            }
            this.OpenSISXFiles(files.ToArray());
        }


        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            LaunchBrowser("donate.aspx?project=sisxplorer");
//            System.Diagnostics.Process.Start( "http://www.symbian-toys.com/sisxplorer.aspx" );
        }


        private void splitContainer1_SplitterMoved_1(object sender, SplitterEventArgs e)
        {
            tabsColl.Refresh();
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            OpenSISXFiles( openFileDialog1.FileNames );
        }


        private void OpenSISXFiles(string[] filenames)
        {
            foreach (string fileName in filenames)
            {
                if (!File.Exists(fileName)) continue;
                if (fileName.Contains(SISEntry.TempDir)) continue;
                tree.AddSIS( fileName );
                recent.UpdateRecentFiles( fileName );
            }
            updateRecentListGUI();
        }
        
        /// <summary>
        /// Estrae tutti i files .sisx
        /// </summary>
        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {            
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;
            string path = folderBrowserDialog1.SelectedPath;
            tree.ExtractAllTo( path );            
        }


        /// <summary>
        /// Chiude tutti i files .sisx
        /// </summary>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            tree.RemoveAllSIS();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (GUI.ShowQuery( "Exit from SISXplorer?", MessageBoxButtons.YesNo ) != DialogResult.Yes) e.Cancel = true;
            else tree.RemoveAllSIS();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.Location = Location;
            Properties.Settings.Default.SplitterDistance = splitContainer1.SplitterDistance;
            Properties.Settings.Default.Size = Size;
//            Properties.Settings.Default.TreeView = tree.ShowUsingTree;
            Properties.Settings.Default.Save();
        }


        private void ShowHideTab(TabPage aTab, bool view)
        {
            tabsColl.SuspendLayout();
            if (view)
            {
                if (!tabsColl.TabPages.Contains(aTab))
                    tabsColl.TabPages.Add(aTab);
            }
            else
            {
                if (tabsColl.TabPages.Contains(aTab))
                    tabsColl.TabPages.Remove(aTab);
            }
            tabsColl.ResumeLayout();
        }


        private void ShowHideTabs()
        {
            bool infosis = Settings.Default.InfoSIS;
            bool info = Settings.Default.InfoFile;
            bool hex = Settings.Default.HexViewer;
            bool e32 = Settings.Default.E32Image;
            bool mbm = Settings.Default.MBMViewer;
            bool mif = Settings.Default.MIFViewer;
            bool txt = Settings.Default.TXTViewer;
            bool rsc = Settings.Default.RSCViewer;
            ShowHideTabs(infosis, info, hex, e32, mbm, mif, txt, rsc);
        }


        private void ShowHideTabs(bool infosis, bool info, bool hex, bool e32, bool mbm, bool mif, bool txt, bool rsc)
        {
            ShowHideTab(tabInfoSISX, infosis);
            ShowHideTab(tabInfoFile, info);
            ShowHideTab(tabHexView, hex);
            ShowHideTab(tabE32, e32);
            ShowHideTab(tabMbmView, mbm);
            ShowHideTab(tabMifView, mif);
            ShowHideTab(tabTxtView, txt);
            ShowHideTab(tabRSC, rsc);
        }


        private void tree_AfterSelect(object sender, string aFileName)
        {
            Debug.WriteLine("AfterSelect: "+aFileName);
            bool infosis = false;
            bool infofile = false;
            bool hex = false;
            bool e32 = false;
            bool mbm = false;
            bool mif = false;
            bool txt = false;
            bool rsc = false;

            ctrlInfoFile1.Clear();
            ctrlHexFile.Clear();
            ctrlPreviewMbm1.Clear();
            ctrlE32Img1.Clear();
            ctrlPreviewMif1.Clear();
            ctrlText1.Clear();
            ctrlRSC1.Clear();

            SISEntry sisEntry = tree.SelectedSISEntry;
            SISController cnt = tree.SelectedSISController;
            if (sisEntry != null)
            {
                if (Settings.Default.InfoSIS)
                {
                    infosis = true;
                    ctrlInfoSISX1.ShowInfo(sisEntry, cnt);
                }
            }
            else
                ctrlInfoSISX1.Clear();

            SISFileDescription fileDescr = tree.SelectedSISFileDescription;
//            if (fileDescr == null)
//                ctrlInfoFile1.Clear();

            
            // TODO: Sarebbe utile avere una unica schermata che riunisce sia le info del sisx che quelle del file
            if (tree.IsSisSelected)
            {   // Selezionato un archivio .sisx
                if (tree.IsSisEmbeddedSelected) // L'archivio e' di tipo embedded
                {
                    ShowHideTabs(infosis, false, false, false, false, false, false, false);
                    return;
                }
            }
            else
            {
                // Selezionato un file
                if (aFileName == "") // es. archivio e' stato chiuso, non c'e' nessun file selezionato
                {
                    ShowHideTabs();
                    return;
                }

                if (Settings.Default.InfoFile)
                {
                    infofile = true;
                }
            }

            int oldIndex = tabsColl.SelectedIndex;
            if (oldIndex < 0) oldIndex = 0;
            FileStream sr = null;
            int i=0;
            Byte[] data = null;
            do
            {
                EpocFile epocFile = null;
                try
                {
                    //             aFileName = "C:\\Documents and Settings\\ue_bellino\\Desktop\\SISX\\mbm\\33.mbm";
                    sr = new FileStream(aFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    data = new byte[sr.Length];
                    sr.Read(data, 0, (int)sr.Length);
                    sr.Close();

                    epocFile = EpocFile.Factory(data);
                    i = 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("SLEEP - Errore :" + ex);
                    System.Threading.Thread.Sleep(1000);
                    i++;
                }

                if (i == 0)
                {
                    // Visualizzazione HEX
                    if (Settings.Default.HexViewer)
                    {
                        hex = true;
                        ctrlHexFile.ShowData(data);
                    }

                    // Visualizzazione MBM
                    if (epocFile is MBMArchive && Settings.Default.MBMViewer) // File MBM
                    {
                        mbm = true;
                        MBMArchive mbmFile = epocFile as MBMArchive;
                        ctrlPreviewMbm1.ShowImages(mbmFile.jmpTable.paintData);
                    }

                    // Visualizzazione MIF
                    if (epocFile is MIFArchive && Settings.Default.MIFViewer)
                    {
                        mif = true;
                        MIFArchive mifFile = epocFile as MIFArchive;
                        ctrlPreviewMif1.ShowImages(mifFile.jmpTable.paintData);
                    }

                    // Visualizzazione E32Image
                    if (epocFile is E32File && Settings.Default.E32Image)
                    {
                        e32 = true;
                        ctrlE32Img1.ShowInfo(epocFile as E32File);
                    }

                    // Visualizzazione RSC
                    if (epocFile is RSCFile && Settings.Default.RSCViewer)
                    {
                        rsc = true;
                        ctrlRSC1.ShowInfo(epocFile as RSCFile);
                    }

                    // Visualizzazione TXT
                    if (!mbm && !e32 && !mif & Settings.Default.TXTViewer)
                    {                       
                        Encoding enc = EncodingTools.DetectInputCodepage(data);
                        if (enc == Encoding.ASCII || enc == Encoding.UTF8 || enc == Encoding.UTF7 || enc == Encoding.UTF32 || enc == Encoding.Unicode)
                        {
                            txt = true;
                            ctrlText1.ShowData(data, enc);
                        }
                    }

                    if (infofile)
                    {
                        ctrlInfoFile1.ShowInfo(fileDescr);
                    }
                }
            } while (i > 0 && i < 3);
            ShowHideTabs(infosis, infofile, hex, e32, mbm, mif, txt, rsc);
            tabsColl.SelectedIndex = oldIndex;
        }



        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            // If the data is a file or a bitmap, display the copy cursor.
            if (e.Data.GetDataPresent( DataFormats.FileDrop ))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        
        /// <summary>
        /// Apre un file .sisx
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openBtn.PerformClick();
        }

        private void donationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LaunchBrowser("donate.aspx?project=sisxplorer");
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splash.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LaunchBrowser(string page)
        {
            try
            {
                System.Diagnostics.Process.Start( "http://www.symbian-toys.com/"+page );
            }
            catch (Exception)
            {
            }
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            LaunchBrowser( "guardian.aspx" );
        }

        private void toolStripStatusLabel4_Click(object sender, EventArgs e)
        {
            LaunchBrowser( "flashsms.aspx" );
        }

        private void toolStripStatusLabel6_Click(object sender, EventArgs e)
        {
            LaunchBrowser( "unlockme.aspx" );
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            splash.Hide();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {            
            splash.StartPosition = FormStartPosition.CenterScreen;
            splash.Show(this);
        }

        private void recentItem_Click(object sender, EventArgs e)
        {
            string s = sender.ToString();
            OpenSISXFiles( new string[] { s } );
        }


        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            Debug.WriteLine( sender );
            string[] items = (string[])e.Data.GetData( DataFormats.FileDrop );
            this.OpenSISXFiles(items);
        }


        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmOptions options = new FrmOptions();
            options.ShowDialog();
            //ShowHideTabs();
        }



    }

}