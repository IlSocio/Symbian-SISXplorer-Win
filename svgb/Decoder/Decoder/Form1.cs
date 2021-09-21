using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using EpocData.MIF;


namespace Decoder
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.button1.PerformClick();
        }


        private void DoReversingJob(string file) 
        {
            FileStream strm = new FileStream(file, FileMode.Open, FileAccess.Read,FileShare.Read);
            BinaryReader br = new BinaryReader(strm);

            Svgb svgb = new Svgb();

            textBox1.Text = svgb.DoReversingJob(br);

            Console.ReadLine();
            br.Close();
            strm.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
//            DoReversingJob("C:\\online\\svgb\\hw.svgb");
              DoReversingJob("C:\\online\\SisXplorer\\svgb\\hw.svgb");
        }
    }
}