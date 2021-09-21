using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace test
{
    public class Svgb
    {
        private bool isRootTag = false;


        private string ReadString(BinaryReader br)
        {
            byte len = br.ReadByte();
            byte[] cars = br.ReadBytes(len);
            return UnicodeEncoding.Unicode.GetString(cars);
        }


        private string ReadPoints(BinaryReader br)
        {
            /*
// TIPO 0: 8bytes
// TIPO 1: 8bytes                          
// TIPO 2: ?                        
// TIPO 3: 24bytes
// TIPO 4: ?
                         
0                        M32.135,7.415   
1                        L14.363,17.432   
1                        V23.167   
3                        C0,0,8.926,15.351,10.468,18.001   
3                        C-2.386,1.704-15.44,11.03-15.44,11.03   
1                        L21.613,12.652   
3                        C0,0,12.907-9.85,14.71-11.226   
3                        C1.979,1.109,16.231,9.101,16.231,9.101  
1                        L16.664-15.132   
3                        C0,0-14.066-6.929-16.888-8.318   
3                        C1.467-3.01,10.531-21.604,10.531-21.604   
1                        L-22.298-9.59   
3                        C0,0-1.486,3.173-2.093,4.467   
3                        C-2.046-0.88-6.573-2.826-6.573-2.826   
3                        S-3.713,2.463-5.696,3.778   
3                        C-0.327-0.744-0.542-1.233-0.657-1.495   
3                        C0.007-0.824,0.213-23.72,0.213-23.72  
1                        L32.135,7.415  
4                        z 
            */
            Int16 qta = br.ReadInt16();
            if (qta == 0)
            {
                br.ReadInt16();
                return "";
            }
            Debug.WriteLine("Points:");
            // Quando c'e' solo 1 punto di tipo 0 allora i bytes sono questi:
            // 01 00  00      02 00       xx xx xx xx    yy yy yy yy
            // Quando c'e' 1 punto e la z...
            // 02 00  00 04   02 00
            byte[] tipi = br.ReadBytes(qta);
            br.ReadBytes(2); // Significato???  02 00
            string s = "";
            for (int i = 0; i < tipi.Length; i++)
            {
                Debug.Write(" " + tipi[i]);
                if (s != "") s += " ";
                switch (tipi[i])
                {
                    case 0:
                        {
                            s += "M" + br.ReadSingle().ToString() + "," + br.ReadSingle().ToString();
                            break;
                        }
                    case 1:
                        {
                            s += "L" + br.ReadSingle().ToString() + "," + br.ReadSingle().ToString();
                            break;
                        }
                    case 3:
                        {
                            s += "C" + br.ReadSingle().ToString() + "," + br.ReadSingle().ToString() + "," + br.ReadSingle().ToString() +
                                 "," + br.ReadSingle().ToString() + "," + br.ReadSingle().ToString() + "," + br.ReadSingle().ToString();
                            break;
                        }
                    case 4:
                        {
                            s += "Z";
                            break;
                        }
                    default:
                        {
                            Debug.Assert(false, "TODO");
                            break;
                        }
                }
            }
            Debug.WriteLine(s);
            return s;
        }


        private string ReadSize(BinaryReader br)
        {
            // 1B 00   01   00 00 C8 42  ---- 4C 00  ----  00   00 80 3F
            // 1A 00   01   00 00 C8 42  ---- 5A 00  ----  01
            // Verifica l'unita' di misura...
            string unit = "";
            if (br.ReadByte() == 1)
            {
                unit = "%";
            }
            else
            {
                unit = "px";
            }
            return br.ReadSingle().ToString() + unit;
        }

        // d=' 32.13499,7.414993  14.36299,17.43199  14.36299,40.59898  14.36299,40.59898,23.28899,55.94998,24.83098,58.59998 22.44499,60.30397,9.390991,69.62997,9.390991,69.62997 31.00398,82.28197  31.00398,82.28197,43.91098,72.43198,45.71397,71.05597 47.69296,72.16496,61.94496,80.15697,61.94496,80.15697 78.60895,65.02498  78.60895,65.02498,64.54295,58.09599,61.72095,56.70699 63.18794,53.69699,72.25194,35.103,72.25194,35.103 49.95395,25.513  49.95395,25.513,48.46796,28.68599,47.86096,29.98 45.81497,29.10001,41.28796,27.15401,41.28796,27.15401 41.28796,27.15401,37.57497,29.617,35.59196,30.93201 35.26497,30.18802,35.04997,29.69902,34.93497,29.43701 34.94196,28.61302,35.14796,5.717026,35.14796,5.717026 32.13499,7.414993 '

        private string FactoryAttrib(BinaryReader br)
        {
            Int16 id = br.ReadInt16();
            switch (id)
            {
                case 0x0000:
                    {
                        if (br.ReadByte() == 1)
                        {
                            return "fill = 'url(#" + ReadString(br) + ")'";
                        }
                        string r = String.Format("{0:X2}", br.ReadByte());
                        string g = String.Format("{0:X2}", br.ReadByte());
                        string b = String.Format("{0:X2}", br.ReadByte());
                        byte mode = br.ReadByte();
                        string colour = "none";
                        switch (mode)
                        {
                            case 0:
                                colour = "#" + r + g + b;
                                break;
                            case 1:
                                // none
                                break;
                            default:
                                Debug.Assert(false, "TODO");
                                break;
                        }
                        return "fill = '" + colour + "'";
                    }
                case 0x0001:
                    {
                        // TODO: Check di questo valore
                        Debug.Assert(br.ReadByte() == 0, "TODO");
                        string r = String.Format("{0:X2}", br.ReadByte());
                        string g = String.Format("{0:X2}", br.ReadByte());
                        string b = String.Format("{0:X2}", br.ReadByte());
                        return "stroke='rgb(" + r + "," + g + "," + b + ")'";
                        //return "stroke:#" + r + g + b + ";";
                    }
                case 0x0002:
                    {
                        //return "stroke-width:" + br.ReadSingle() + "px;";
                        return "stroke-width='" + br.ReadSingle() + "'";
                    }
                case 0x000A:
                    {
                        return "fill-rule='" + ReadString(br)+"'";
                    }
                case 0x000B:
                    {
                        //return "stroke-linecap:" + ReadString(br) + ";";
                        return "stroke-linecap='" + ReadString(br) + "'";
                    }
                case 0x000C:
                    {
                        return "stroke-linejoin='" + ReadString(br) + "'";
                    }
                case 0x0016:
                    {
                        // fill-opacity:1;fill-rule:evenodd;stroke:#000000;stroke-width:1px;stroke-linecap:butt;stroke-linejoin:miter;stroke-opacity:1"
                        // TODO: Gestire il TAG
                        return "fill-opacity='"+br.ReadSingle()+"'";
                    }
                case 0x0017:
                    {
                        // fill-opacity:1;fill-rule:evenodd;stroke:#000000;stroke-width:1px;stroke-linecap:butt;stroke-linejoin:miter;stroke-opacity:1"
                        // TODO: Gestire il TAG
                        return "stroke-opacity='" + br.ReadSingle() + "'";
                    }
                case 0x001A:
                    {
                        // TODO: Verifica correttezza
                        string s = "";
                        if (isRootTag)
                        {
                            s = ReadSize(br);
                        }
                        else
                        {
                            s = br.ReadSingle().ToString();
                        }
                        return "width='" + s + "'";
                    }
                case 0x001B:
                    {
                        // TODO: Verifica correttezza
                        string s = "";
                        if (isRootTag)
                        {
                            s = ReadSize(br);
                        }
                        else
                        {
                            s = br.ReadSingle().ToString();
                        }
                        return "height='" + s + "'";
                    }
                case 0x001C:
                    {
                        return "r = '" + br.ReadSingle() + "'";
                    }
                case 0x002E:
                    {
                        return "cx = '" + br.ReadSingle() + "'";
                    }
                case 0x002F:
                    {
                        return "cy = '" + br.ReadSingle() + "'";
                    }
                case 0x0032:
                    {
                        return "y1='" + br.ReadSingle() + "'";
                    }
                case 0x0033:
                    {
                        return "y2='" + br.ReadSingle() + "'";
                    }
                case 0x0034:
                    {
                        return "x1='" + br.ReadSingle() + "'";
                    }
                case 0x0035:
                    {
                        return "x2='" + br.ReadSingle() + "'";
                    }
                case 0x0042:
                    {
                        string d1 = br.ReadSingle().ToString();
                        string d2 = br.ReadSingle().ToString();
                        string d3 = br.ReadSingle().ToString();
                        string d4 = br.ReadSingle().ToString();
                        string d5 = br.ReadSingle().ToString();
                        string d6 = br.ReadSingle().ToString();
                        br.ReadBytes(4);
                        //transform="matrix(1.83513,0,0,1.30728,-14.676,-2.13038)"
                        //42 00 00 CB D5 01 00 00 00 00 00 F2 52 F1 FF 00 00 00 00 A9 4E 01 00 A0 DE FD FF 03 00 00 00
                        // .....11 11 11 11 33 33 33 33 55 55 55 55 22 22 22 22 44 44 44 44 66 66 66 66[**]00 00 00
                        // [**] varia a seconda di quanti parametri sono stati utilizzati ma non me ne frega...
                        // TODO: Verifica significato restanti bytes???
                        //
                        return "transform = 'matrix(" + d1 + "," + d2 + "," + d4 + "," + d5 + "," + d3 + "," + d6 + ")'";
                    }
                case 0x004C:    // TAG AGGIUNTO DA THEME STUDIO (ha a che fare con height?)
                    {
                        // Forse = version
                        // TODO:... Verifica quale TAG e'...
                        // byte[] b = br.ReadBytes(4);  // 00 00 80 3F
                        float f = br.ReadSingle();  // 1.0
                        string s = String.Format("{0:n1}", f);
                        return "version='"+s+"'";
                    }
                case 0x004E:
                    {
                        string s = ReadPoints(br);
                        s = s.Replace("M", "");
                        s = s.Replace("L", "");
                        s = s.Replace("C", "");
                        s = s.Replace("Z", "");
                        return "points='" + s + "'";
                    }
                case 0x004F:
                    {
                        return "d='" + ReadPoints(br) + "'";
                    }
                case 0x0051:
                    {
                        string r = String.Format("{0:X2}", br.ReadByte());
                        string g = String.Format("{0:X2}", br.ReadByte());
                        string b = String.Format("{0:X2}", br.ReadByte());
                        // TODO: Verifica questo valore
                        Debug.Assert(br.ReadByte() == 0, "TODO");
                        return "style='stop-color:#" + r + g + b + "'";
                    }
                case 0x0054:
                    {
                        return "offset='" + br.ReadSingle() + "'";
                    }
                case 0x0056:
                    {
                        Debug.Assert(br.ReadByte() == 0, "TODO");
                        return "gradientUnits='userSpaceOnUse'";
                    }
                case 0x0058:
                    {
                        return "viewBox ='" + br.ReadSingle() + " " + br.ReadSingle() + " " + br.ReadSingle() + " " + br.ReadSingle() + "'";
                    }
                case 0x0059:
                    {
                        string s = ReadString(br);
                        return "baseProfile='" + s + "' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink'";
                    }
                case 0x005A:    // TAG AGGIUNTO DA THEME STUDIO (ha a che fare con width?)
                    {
                        // TODO:... Interpreta
                        byte b = br.ReadByte();
                        //byte[] b = br.ReadBytes(4);// 01 E8 03 0B
                        return " ";
                    }
                case 0x005C:
                    {
                        return "id ='" + ReadString(br) + "'";
                    }
                case 0x03E8:    // Fine Attributi
                    {
                        // br.BaseStream.Seek(br.BaseStream.Position - 2);
                        return "";
                    }
                case 0x00BE:
                    {
                        Debug.Assert(false, "TODO");
                        return " ";
                    }
            }
            Debug.Assert(false, "Unsupported");
            return "";
        }


        private string ReverseAttribs(BinaryReader br)
        {
            string ris = "";
            string s = "";
            do
            {
                s = FactoryAttrib(br);
                if (s != "")
                {
                    Debug.WriteLine(s);
                }
                if (ris != "" && s != "") ris += " ";
                ris = ris + s;
            } while (s != "");
            return ris;
        }


        private string FactoryTags(BinaryReader br)
        {
            isRootTag = false;
            byte id = br.ReadByte();
            switch (id)
            {
                case 0x00:
                    // OPEN TAG SVG
                    isRootTag = true;
                    return "svg";
                case 0x0B:
                    return "g";
                case 0x1B:
                    return "circle";
                case 0x1D:
                    return "line";
                case 0x1E:
                    return "path";
                case 0x21:
                    return "rect";
                case 0x28:
                    return "linearGradient";
                case 0x2A:
                    return "stop";
                case 0x1F:
                    return "polygon";
                case 0xFE:  // Chiude Tag
                    return "";
                case 0xFF:      // FINE FILE;
                    Debug.Assert(false);
                    return "";
            }
            Debug.Assert(false, "Unsupported");
            return "";
        }


        private string ReverseTags(BinaryReader br)
        {
            Stack<string> stack = new Stack<string>();
            string ris = "";
            string tag = "";

            bool exit = false;
            while (!exit && br.BaseStream.Position < br.BaseStream.Length - 1)
            {
                tag = FactoryTags(br);
                if (tag != "")
                {
                    Debug.WriteLine(tag);
                    stack.Push(tag);
                    ris = ris + "<" + tag + " " + ReverseAttribs(br) + " >\r\n";
                }
                else
                {
                    if (stack.Count > 0)
                        ris = ris + "</" + stack.Pop() + " >\r\n";
                    else
                    {
                        exit = true;
                        //Debug.Assert(false, "Stack Empty");
                    }
                }
            }
            return ris;
        }


        public string DoReversingJob(BinaryReader br)
        {
            Int16 check = br.ReadInt16();
            // Carbide   3RD Ed FP1
            if (check != 0x56CC && check != 0x56CE)
            {
                return "";
            }
            check = br.ReadInt16();
            if (check != 0x03FA) return "";

            string s = "<?xml version='1.0' encoding='UTF-8'?>\r\n";
            s += "<!DOCTYPE svg PUBLIC '-//W3C//DTD SVG 1.1 Tiny//EN' 'http://www.w3.org/Graphics/SVG/1.1/DTD/svg11-tiny.dtd'>\r\n";
            s += "<!-- Generated from SIXPlorer 2.0 -->\r\n";
            s += ReverseTags(br);
            return s;
        }

    }
}
