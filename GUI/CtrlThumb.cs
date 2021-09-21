using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SISXplorer
{
    public partial class CtrlThumb : UserControl
    {
        public CtrlThumb()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            pictureBox1.Image = null;
            axRenesisCtrl1.Hide();
            pictureBox1.Show();
        }

        private void ShowThumb(Image img)
        {
            if (img == null)
            {
                Clear();
                return;
            }
            Image thumbImg = img.GetThumbnailImage(64, 64, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            ShowImage(thumbImg);
        }

        public void ShowImage(Image img)
        {
            this.axRenesisCtrl1.Hide();
            this.pictureBox1.Show();
            if (img == null)
            {
                Clear();
                return;
            }
            if (img.Width < pictureBox1.Width && img.Height < pictureBox1.Height)
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            else
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = img;
        }


        /*private Bitmap Capture(Control window, Rectangle rc)
        {
            Bitmap memoryImage = null;

            // Create new graphics object using handle to window.
            using (Graphics graphics = window.CreateGraphics())
            {
                memoryImage = new Bitmap(rc.Width,
                              rc.Height, graphics);

                using (Graphics memoryGrahics =
                        Graphics.FromImage(memoryImage))
                {
                    memoryGrahics.CopyFromScreen(rc.X, rc.Y,
                       0, 0, rc.Size, CopyPixelOperation.SourceCopy);
                }
            }
            return memoryImage;
        }*/


        public void ShowImage(EpocData.MIF.MifPaintDataSection mif)
        {
            this.pictureBox1.Hide();
            this.axRenesisCtrl1.Show();
            if (mif == null)
            {
                Clear();
                return;
            }           
            String s = mif.GetData() as string;
            int retry=3;
            while (retry > 0)
            {
                try
                {
                    axRenesisCtrl1.OpenSVGSource(s);
                    retry=0;
                }
                catch (Exception ex)
                {
                    retry--;
                    System.Threading.Thread.Sleep(1000);
                }
            }
            // axRenesisCtrl1.window.alert("Hello from Delphi!");
            //                System.Xml.XmlDocument dom = axRenesisCtrl1.getSVGDocument() as System.Xml.XmlDocument;
            //                System.Xml.XmlNodeList list = dom.GetElementsByTagName("width");
            /* function InitContextMenu()
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
            axRenesisCtrl1.Width = 100;
            axRenesisCtrl1.Height = 100;
            //            IntPtr hdcScreen = GetDC(GetDesktopWindow());                        
//            Image img = ScreenCapture.CaptureWindow(axRenesisCtrl1.Handle);
//            Bitmap bmp = Capture(axRenesisCtrl1, region);
//            ShowImage(img);            
//            axRenesisCtrl1.OpenSVGSource("");
//            this.axRenesisCtrl1.Hide();
        }


        ///  <summary>
        /// Required, but not used
        /// </summary>
        /// <returns>true</returns>
        public bool ThumbnailCallback()
        {
            return true;
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            Image img = pictureBox1.Image;
            if (img == null) 
                return;
            if (img.Width < pictureBox1.Width && img.Height < pictureBox1.Height)
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            else
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if (pictureBox1.Visible == false && axRenesisCtrl1.Visible == true)
            {
                axRenesisCtrl1.Update();
                Image img = ScreenCapture.CaptureWindow(axRenesisCtrl1.Handle);
                axRenesisCtrl1.OpenSVGSource("");
                ShowImage(img);
            }
            base.OnPaint(e);
        }


        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {

        }


        private void CtrlThumb_DragDrop(object sender, DragEventArgs e)
        {

        }
    }

}
