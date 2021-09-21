using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using EpocData;
using EpocData.MBM;
using Utility;
using System.Threading;


namespace SISXplorer
{
    public partial class JThumbnailView : UserControl
    {
        private Semaphore sem;
        public delegate void AfterSelectEventHandler(object sender, int itemIndex);
        public event AfterSelectEventHandler AfterSelectItem;
        
//        private BackgroundWorker myWorker = new BackgroundWorker();
        private ICollection newMbmFile;


        private Color thumbBorderColor;
        public Color ThumbBorderColor
        {
            get { return thumbBorderColor; }
            set { thumbBorderColor = value; }
        }


        private delegate void SetThumbnailDelegate(Image image);
        private void AddThumbnail(Image image)
        {
            if (this.InvokeRequired)
            {
                SetThumbnailDelegate d = new SetThumbnailDelegate(AddThumbnail);
                this.Invoke(d, new object[] { image });
            }
            else
            {
                if (image == null) return;
                imageList1.Images.Add( image ); //Images[i].repl  
                int index = imageList1.Images.Count - 1;      // La prima posizione e' l'immagine vuota
                listView1.Items[index - 1].ImageIndex = index;
            }
        }


        public JThumbnailView()
        {
            InitializeComponent();            
            listView1.UseCompatibleStateImageBehavior = false;
            sem = new Semaphore(0, 1);
        }


        public void Clear()
        {
            newMbmFile = null;
            myWorker.CancelAsync();
            listView1.Clear();
            imageList1.Images.Clear();
            AddDefaultThumb(); // Aggiunge l'immagine vuota alla lista LargeImageList
        }

        ///  <summary>
        /// Required, but not used
        /// </summary>
        /// <returns>true</returns>
        public bool ThumbnailCallback()
        {
            return true;
        }

        
        public Image GetThumbNail(Bitmap bmp)
        {
            return GetThumbNail( bmp, imageList1.ImageSize.Width, imageList1.ImageSize.Height, thumbBorderColor );
        }

        public Image GetThumbNail(Bitmap bmp, int imgWidth, int imgHeight, Color penColor)
        {
            if (bmp == null) return null;
            imgWidth = bmp.Width > imgWidth ? imgWidth : bmp.Width;
            imgHeight = bmp.Height > imgHeight ? imgHeight : bmp.Height;

//            Image retBmp = bmp.GetThumbnailImage( imgWidth, imgHeight, new System.Drawing.Image.GetThumbnailImageAbort( ThumbnailCallback ), IntPtr.Zero );
            Bitmap retBmp = new Bitmap(imgWidth, imgHeight, PixelFormat.Format64bppPArgb);

            Graphics grp = Graphics.FromImage(retBmp);

            int tnWidth = imgWidth, tnHeight = imgHeight;

            if (bmp.Width > bmp.Height)
                tnHeight = (int)(((float)bmp.Height / (float)bmp.Width) * tnWidth);
            else if (bmp.Width < bmp.Height)
                tnWidth = (int)(((float)bmp.Width / (float)bmp.Height) * tnHeight);

            int iLeft = (imgWidth / 2) - (tnWidth / 2);
            int iTop = (imgHeight / 2) - (tnHeight / 2);

            grp.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            grp.InterpolationMode = InterpolationMode.Low;
            //            grp.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //            grp.PixelOffsetMode = PixelOffsetMode.HighQuality;
//            grp.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grp.DrawImage( bmp, iLeft, iTop, tnWidth, tnHeight );

            Pen pn = new Pen( thumbBorderColor, 1 ); //Color.Wheat
            grp.DrawRectangle(pn, 0, 0, retBmp.Width-1, retBmp.Height-1);/**/

            return retBmp;
        }

        private void AddDefaultThumb()
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap( imageList1.ImageSize.Width, imageList1.ImageSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb );
            Graphics grp = Graphics.FromImage(bmp);
            Brush brs = new SolidBrush(Color.White);
            Rectangle rect = new Rectangle(0, 0, bmp.Width - 1, bmp.Height - 1);
            grp.FillRectangle(brs, rect);
            Pen pn = new Pen( thumbBorderColor, 1 );

            grp.DrawRectangle(pn, 0, 0, bmp.Width - 1, bmp.Height - 1);
            imageList1.Images.Add( bmp );
        }


        public void ReLoadItems(ICollection mbmFile)
        {
            if (myWorker.IsBusy)
            {
                newMbmFile = mbmFile;
                myWorker.CancelAsync();
                return;
            }
            Clear();
            if (mbmFile == null)
            {
                return;
            }

            // Aggiunge le varie tessere...
            int tot = mbmFile.Count;
            ListViewItem[] items = new ListViewItem[tot];
            for (int i = 1; i <= tot; i++)
                items[i - 1] = new ListViewItem("Image" + i);
            listView1.Items.AddRange(items);

            // Inizia il caricamento dei Thumbs...
            myWorker.RunWorkerAsync(mbmFile);
        }


        public ICollection SelectedIndices
        {
            get
            {
                return listView1.SelectedIndices;
            }
        }

        public void SelectAllItems()
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Selected = true;
            }
        }


        private void listView1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void listView1_ItemActivate_1(object sender, EventArgs e)
        {
//            if (AfterSelectItem != null && listView1.SelectedItems.Count > 0)
//                AfterSelectItem(this, listView1.SelectedItems[0].Index);
        }


        private void myWorker_DoWork_1(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            ICollection fileList = (ICollection)e.Argument;

            foreach (IImage img in fileList)
            {
                if (img != null)
                {
                    Bitmap bmp = img.GetData() as Bitmap;
                    bw.ReportProgress(0, bmp);
                    sem.WaitOne();
                }
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
            }/**/
            e.Result = fileList.Count;
        }


        private void myWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is Bitmap)
            {
                Bitmap bmp = e.UserState as Bitmap;
                AddThumbnail(GetThumbNail(bmp));
                Application.DoEvents();
            }
            sem.Release();
            /*            if (state == false)
                        {
                            listView1.Items.Add( i + ".bmp" );
                        }
                        else
                        {
                            //Bitmap bmp = this.mbmFile[i-1];
                            //SetThumbnail( GetThumbNail( bmp ) );
                        }*/
        }


        private void myWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            if (e.Cancelled)
            {
                //if (newMbmFile != null)
                ReLoadItems(newMbmFile);
                return;
            }
            newMbmFile = null;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AfterSelectItem != null && listView1.SelectedItems.Count > 0)
                AfterSelectItem(this, listView1.SelectedItems[0].Index);
        }

    }
}
