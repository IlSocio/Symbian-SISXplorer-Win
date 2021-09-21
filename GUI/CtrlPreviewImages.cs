using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using EpocData;
using EpocData.MBM;
using Utility;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;



namespace SISXplorer
{

    public partial class CtrlPreviewImages : UserControl, IDisposable
    {
        private int _posImg = -9999;
        private List<IImage> _imgList = null;    // OWN

        /// <summary>
        /// Quando cambia posMbm viene aggiornata la visualizzazione dell'immagine centrale
        /// </summary>
        private int posImg
        {
            get
            {
                return _posImg;
            }
            set
            {
                if (imgList == null || imgList.Count == 0)
                {
                    _posImg = -1;
                    ctrlThumb1.Clear();
                    return;
                }
                if (value == _posImg) return;
                if (value < 0) value = imgList.Count - 1;
                if (value >= imgList.Count) value = 0;
                _posImg = value;
                UpdateImageView();
            }
        }


        /// <summary>
        /// Quando mbmFile cambia aggiorna anche la visualizzazione dei pulsanti
        /// </summary>
        private List<IImage> imgList
        {
            get
            {
                return _imgList;
            }
            set
            {
                if (_imgList == value) return;
                ctrlThumb1.Clear();
                _imgList = value;
                posImg = 0;
                bool enabled = (value != null && value.Count > 0);
                toolStripButton1.Enabled = toolStripButton2.Enabled = btnExtr.Enabled = btnExtrAll.Enabled = enabled;
                txtCounter.Text = "" + (posImg + 1);
                txtCounter.ReadOnly = !enabled;
                labQty.Text = "/0";
                if (enabled)
                {
                    labQty.Text = "/" + imgList.Count;
                }
                ctrlThumList1.ReLoadItems( value );
                posImg = 0;
            }
        }



        public CtrlPreviewImages()
        {
            InitializeComponent();
        }


        public void Clear()
        {
            ctrlThumb1.Clear();
            labXY.Text = "";
            imgList = null;
        }

        /// <summary>
        /// Acquisisce anche l'ownership del file
        /// </summary>
        public void ShowImages(List<IImage> list)
        {
//            tst = new Test();
            imgList = list;
            posImg = 0;
        }


        private void UpdateImageView()
        {
            IImage img = imgList[posImg] as IImage;
            if (img == null)
            {
                labXY.Text = "";
                ctrlThumb1.Clear();
                return;
            }

            txtCounter.Text = "" + (posImg + 1);
            Debug.WriteLine("*************Immagine:"+txtCounter.Text);
            if (img is EpocData.MIF.PaintDataSection)
            {
                this.ctrlThumb1.ShowImage(img as EpocData.MIF.PaintDataSection);
                // E' un .SVG compresso oppure no...
            }

            if (img is EpocData.MBM.PaintDataSection)
            {
                Bitmap bmp = img.GetData() as Bitmap;
                Debug.Assert(bmp != null, "Errore BMP = null");
                string fmt = bmp.PixelFormat.ToString();
                fmt = fmt.Replace("Format", "");
                fmt = fmt.Replace("565", "");
                fmt = fmt.Replace("Rgb", "");
                fmt = fmt.Replace("Indexed", "");
                fmt = fmt.Replace("bpp", "");
                labXY.Text = bmp.Width + "x" + bmp.Height + "x" + fmt;
                this.ctrlThumb1.ShowImage(bmp);
            }
/*            System.IO.FileStream sr = new System.IO.FileStream("c:\\1.svg", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            byte[] data = new byte[sr.Length];
            sr.Read(data, 0, (int)sr.Length);
            sr.Close();*/
//            string s = Encoding.ASCII.GetString(img.GetRawData());
//            this.axRenesisCtrl1.OpenSVGSource(s);

            // HDC  Rederizza in un HDC...
            //this.axRenesisCtrl1.OpenFile("c:\\1.svg");
/*            Graphics gSrc = this.CreateGraphics();
                        Bitmap clipboardImage = new Bitmap(100, 100);
                        Graphics g = Graphics.FromImage(clipboardImage);
            // definisco gli estremi del poligono da disegnare (nell'esempio un rettangolo 50x100)
                        Point[] points = { new Point(0, 0), new Point(100, 0), new Point(100, 50), new Point(50, 0), new Point(0, 0) };
                        // definisco il gradiente da utilizzare
                        Brush brush = new LinearGradientBrush(new Rectangle(0, 0, 5, 50), Color.SteelBlue, Color.White, LinearGradientMode.Vertical);
                        // riempio il mio poligono
                        g.FillPolygon(brush, points, FillMode.Alternate);

            // TODO: Copia da gSrc a gDest
                         IntPtr hdcSrc = gSrc.GetHdc();
                         IntPtr hdcDst = g.GetHdc();
                                     GDI32.BitBlt(
                                          hdcDst.ToInt32(),
                                          0, 0,
                                          this.axRenesisCtrl1.ClientRectangle.Width, this.axRenesisCtrl1.ClientRectangle.Height,
                                          hdcSrc.ToInt32(),
                                          0, 0,
                                          (int)GDI32.TernaryRasterOperations.SRCCOPY
                                          );
                                    gSrc.ReleaseHdc(hdcSrc);
                                    g.ReleaseHdc(hdcDst);
                                    g.Dispose();
                                    Clipboard.SetDataObject(clipboardImage, true);*/

/*                        Bitmap clipboardImage = new Bitmap(100, 100);
                        Graphics g = Graphics.FromImage(clipboardImage);
                        // definisco gli estremi del poligono da disegnare (nell'esempio un rettangolo 50x100)
                        Point[] points = { new Point(0, 0), new Point(100, 0), new Point(100, 50), new Point(50, 0), new Point(0, 0) };
                        // definisco il gradiente da utilizzare
                        Brush brush = new LinearGradientBrush(new Rectangle(0, 0, 5, 50), Color.SteelBlue, Color.White, LinearGradientMode.Vertical);
                        // riempio il mio poligono
                        g.FillPolygon(brush, points, FillMode.Alternate);
                        Clipboard.SetDataObject(clipboardImage, true);
                        g.Dispose(); 
*/
            // IntPtr hdcSrc = g.GetHdc();
            //            IntPtr hdcDst = gDest.GetHdc();
            /*             GDI32.BitBlt(
                              hdcDst.ToInt32(),
                              0, 0,
                              this.axRenesisCtrl1.ClientRectangle.Width, this.axRenesisCtrl1.ClientRectangle.Height,
                              hdcSrc.ToInt32(),
                              0, 0,
                              (int)GDI32.TernaryRasterOperations.SRCCOPY
                              );*/
//            g.ReleaseHdc(hdcSrc);
//            gDest.ReleaseHdc(hdcDst);
        }


/*
        BUT AS A WORK AROUND IT IS POSSIBLE TO do it as follows :
. function InitContextMenu()
{
menuList=document.getElementsByTagName("menu");

for (i=0;i<menuList.length;i++) {
if ( menuList.item(0).getAttribute('id')=="myCustomMenu" ) {
var newMenuRoot = document.getElementsByTagName("menu").item(i);
break;
}
}
contextMenu.replaceChild( newMenuRoot, contextMenu.documentElement );
//getURL("customcontextmenu.xml", callback);
}*/



        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            posImg--;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            posImg++;
        }

        private void txtCounter_Leave(object sender, EventArgs e)
        {
            int ris = 0;
            if (!int.TryParse( txtCounter.Text, out ris )) ris = 0;
            posImg = ris - 1;
        }

        private void txtCounter_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine( e.KeyChar );
            if (e.KeyChar == '\r') ctrlThumb1.Focus();
        }

        private void btnExtr_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
                return;
            IImage img = imgList[posImg] as IImage;
            img.SaveTo( folderBrowserDialog1.SelectedPath + "\\" + (posImg + 1));
        }

        private void btnExtrAll_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
                return;

            DlgExtract dlg = new DlgExtract(imgList, folderBrowserDialog1.SelectedPath);
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                GUI.ShowWarning("Extraction Cancelled!");
            }
        }


        #region IDisposable Members

        void IDisposable.Dispose()
        {
            foreach (IImage img in imgList)
            {
                img.Dispose();
            }            
            _imgList.Clear();
        }

        #endregion

        private void extractSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnExtr.PerformClick();
        }

        private void extractAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnExtrAll.PerformClick();
        }

        private void ctrlThumList1_AfterSelectItem(object sender, int itemIndex)
        {
            posImg = itemIndex;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
                return;
            
            DlgExtract dlg = new DlgExtract( imgList, ctrlThumList1.SelectedIndices, folderBrowserDialog1.SelectedPath );
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                GUI.ShowWarning( "Extraction Cancelled!" );
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ctrlThumList1.SelectAllItems();
        }

        private void axRenesisCtrl1_MouseCaptureChanged(object sender, EventArgs e)
        {

        }

        private void axRenesisCtrl1_ContextMenuStripChanged(object sender, EventArgs e)
        {

        }

    }

    /*
    class GDI32
    {
        [DllImport("GDI32.dll")]
        public static extern bool BitBlt(int hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, int hdcSrc, int nXSrc, int nYSrc, int dwRop);

        public enum StretchMode
        {
            STRETCH_ANDSCANS = 1,
            STRETCH_ORSCANS = 2,
            STRETCH_DELETESCANS = 3,
            STRETCH_HALFTONE = 4,
        }


        public enum TernaryRasterOperations
        {
            SRCCOPY = 0x00CC0020,
            SRCPAINT = 0x00EE0086, 
            SRCAND = 0x008800C6, //
            SRCINVERT = 0x00660046, //
            SRCERASE = 0x00440328, 
            NOTSRCCOPY = 0x00330008, 
            NOTSRCERASE = 0x001100A6, 
            MERGECOPY = 0x00C000CA,
            MERGEPAINT = 0x00BB0226, 
            PATCOPY = 0x00F00021, 
            PATPAINT = 0x00FB0A09, 
            PATINVERT = 0x005A0049, 
            DSTINVERT = 0x00550009, 
            BLACKNESS = 0x00000042, 
            WHITENESS = 0x00FF0062,
        };
    }*/


}
