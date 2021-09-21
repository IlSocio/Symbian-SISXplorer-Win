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
using SISX;
using EpocData;


namespace test
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
/*            //SISXUnpacker unpacker = new SISXUnpacker("c:\\online\\SisXplorer\\Themes\\Zod_Acq_N95_byLeo47.sis");
            SISXUnpacker unpacker = new SISXUnpacker("c:\\online\\SisXplorer\\Themes\\n-gage.sisx");
            //SISXUnpacker unpacker = new SISXUnpacker("c:\\online\\SisXplorer\\Themes\\9_n-gage.sisx");
            unpacker.ExtractInDir("C:\\test\\");
            file = "C:\\test\\themepackage.mif";*/
/*            file = "c:\\online\\SisXplorer\\Themes\\zod.mif";
            FileStream strm = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader br = new BinaryReader(strm);
            EpocData.MIF.MIFArchive arc = new MIFArchive(br);
            SvgbDecoder svgb = new SvgbDecoder();
            br.Close();
            strm.Close();

            int i = 0;
            foreach (IImage imgenc in arc.jmpTable.paintData)
            {
                i++;
                string plainSvg = svgb.DecodeFile(imgenc.GetData());
                System.IO.File.WriteAllText("c:\\out\\"+i+".svg", plainSvg);
            }

/*            SvgbDecoder svgb1 = new SvgbDecoder();
            string[] files = System.IO.Directory.GetFiles("C:\\online\\SisXplorer\\svgb\\tmp2\\");
            foreach (string s in files)
            {
                Debug.WriteLine("**** FILE **** " + s);
                svgb1.DecodeFile(s);
            }*/

            SvgbDecoder svgb = new SvgbDecoder();
            textBox1.Text = svgb.DecodeFile(file);
            Console.ReadLine();
        }


        private void button1_Click(object sender, EventArgs e)
        {
              DoReversingJob("C:\\online\\sisxplorer\\svgb\\hw.svgb");
        }
    }
}