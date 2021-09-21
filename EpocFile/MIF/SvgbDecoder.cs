using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Utility;


namespace EpocData.MIF
{
    public class SvgbDecoder
    {
        private string currentTag = "";


        private string ReadString(BinaryReader br)
        {
            byte len = br.ReadByte();
            if (len == 0) return "";
            byte[] cars = br.ReadBytes(len);
            string s = UnicodeEncoding.Unicode.GetString(cars);
            return s;
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
//            Debug.WriteLine("Points:");
            // Quando c'e' solo 1 punto di tipo 0 allora i bytes sono questi:
            // 01 00  00      02 00       xx xx xx xx    yy yy yy yy
            // Quando c'e' 1 punto e la z...
            // 02 00  00 04   02 00
            byte[] tipi = br.ReadBytes(qta);
            br.ReadBytes(2); // Significato???  02 00
            string s = "M";
            for (int i = 0; i < tipi.Length; i++)
            {
//                Debug.Write(" " + tipi[i]);
                switch (tipi[i])
                {
                    case 0:
                        {
                            s += "" + ReadFloat(br).ToString() + "," + ReadFloat(br).ToString();
                            break;
                        }
                    case 1:
                        {
                            s += "L" + ReadFloat(br).ToString() + "," + ReadFloat(br).ToString();
                            break;
                        }
                    case 2:
                        {
                            s += "Q" + ReadFloat(br).ToString() + "," + ReadFloat(br).ToString() + "," + ReadFloat(br).ToString() + "," + ReadFloat(br).ToString();
                            break;
                        }
                    case 3:
                        {
                            s += "C" + ReadFloat(br).ToString() + "," + ReadFloat(br).ToString() + "," + ReadFloat(br).ToString() +
                                 "," + ReadFloat(br).ToString() + "," + ReadFloat(br).ToString() + "," + ReadFloat(br).ToString();
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
                s += " ";
            }
//            Debug.WriteLine(s);
            return s.Trim();
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
            return ReadFloat(br).ToString() + unit;
        }

        // d=' 32.13499,7.414993  14.36299,17.43199  14.36299,40.59898  14.36299,40.59898,23.28899,55.94998,24.83098,58.59998 22.44499,60.30397,9.390991,69.62997,9.390991,69.62997 31.00398,82.28197  31.00398,82.28197,43.91098,72.43198,45.71397,71.05597 47.69296,72.16496,61.94496,80.15697,61.94496,80.15697 78.60895,65.02498  78.60895,65.02498,64.54295,58.09599,61.72095,56.70699 63.18794,53.69699,72.25194,35.103,72.25194,35.103 49.95395,25.513  49.95395,25.513,48.46796,28.68599,47.86096,29.98 45.81497,29.10001,41.28796,27.15401,41.28796,27.15401 41.28796,27.15401,37.57497,29.617,35.59196,30.93201 35.26497,30.18802,35.04997,29.69902,34.93497,29.43701 34.94196,28.61302,35.14796,5.717026,35.14796,5.717026 32.13499,7.414993 '


        private string ReadColor(BinaryReader br)
        {
            string r = String.Format("{0:X2}", br.ReadByte());
            string g = String.Format("{0:X2}", br.ReadByte());
            string b = String.Format("{0:X2}", br.ReadByte());
            byte mode = br.ReadByte();
            string color = "";
            switch (mode)
            {
                case 0:
                    color = "#" + r + g + b; 
                    break;
                case 1:
                    color = "none";
                    break;
                case 2:
                    return "currentColor";
                    break;
                default:
                    Debug.Assert(false, "TODO");
                    break;
            }
            return color;
        }


        private string ReadFloat(BinaryReader br)
        {
            float f = br.ReadSingle();  // 1.0
            string s = f.ToString();
//            string s = String.Format("{0:n1}", f);
            s = s.Replace(',', '.');
            return s;
        }


        private string FactoryAttrib(BinaryReader br)
        {
            Int16 id = br.ReadInt16();
            switch (id)
            {
                case 0x0000:
                    {
                        if (br.ReadByte() == 1)
                        {
                            return "fill='url(#" + ReadString(br) + ")'";
                        }
                        string color = ReadColor(br);
                        return "fill='" + color + "'";
                    }
                case 0x0001:
                    {
                        return "stroke='" + ReadColor(br) + "'";
                    }
                case 0x0002:
                    {
                        return "stroke-width='" + ReadFloat(br) + "'";
                    }
                case 0x0003:
                    {
                        // TODO: Investigate on Values
                        int i = br.ReadInt32();
                        string s = "";
                        switch (i)
                        {
                            case 0:
                                s = "visible";
                                break;
                            default:
                                Debug.Assert(false, "TODO visibility");
                                break;
                        }

                        return "visibility='" + s + "'";
                    }
                case 0x0004:
                    {
                        return "font-family='" + ReadString(br) + "'";
                    }
                case 0x0005:
                    {
                        return "font-size='" + ReadFloat(br) + "'";
                    }
                case 0x0006:
                    {
                        // TODO: Investigate on values
                        string s = "";
                        Int32 i = br.ReadInt32();
                        switch (i)
                        {
                            case 0:
                                s = "normal";
                                break;
                            case 1:
                                s = "italic";
                                break;
                            default:
                                Debug.Assert(false, "font-style");
                                break;
                        }
                        return "font-style='" + s + "'";
                    }
                case 0x0007:
                    {
                        // TODO: Investigate on values
                        string s = "";
                        Int32 i = br.ReadInt32();
                        switch (i)
                        {
                            case 0:
                                s = "normal";
                                break;
                            case 1:
                                s = "bold";
                                break;
                            default:
                                Debug.Assert(false, "font-weight");
                                break;
                        }
                        return "font-weight='" + s + "'";
                    }
                case 0x0008:
                    {
                        string s = "";
                        byte b = br.ReadByte();
                        for (int i = 0; i < b; i++)
                        {
                            if (s.Length > 0) s += ", ";
                             s += ReadFloat(br);
                        }
                        if (s.Length == 0) s = "none";
                        return "stroke-dasharray='" + s + "'";
                    }
                case 0x0009:
                    {
                        int i = br.ReadInt32();
                        string s = "";
                        switch (i)
                        {
                            case 0:
                                s = "inherit";
                                break;
                            case 0x10:
                                s = "none";
                                break;
                            default:
                                Debug.Assert(false, "TODO display");
                                break;
                        }
                        return "display='" + s + "'";
                    }
                case 0x000A:
                    {
                        return "fill-rule='" + ReadString(br)+"'";
                    }
                case 0x000B:
                    {
                        return "stroke-linecap='" + ReadString(br) + "'";
                    }
                case 0x000C:
                    {
                        return "stroke-linejoin='" + ReadString(br) + "'";
                    }
                case 0x000D:
                    {
                        return "stroke-dashoffset='" + ReadFloat(br) + "'";
                    }
                case 0x000E:
                    {
                        return "stroke-miterlimit='" + ReadFloat(br) + "'";
                    }
                case 0x000F:
                    {
                        return "color='" +ReadColor(br)+ "'";
                    }
                case 0x0010:
                    {
                        string s = "";
                        Int32 i = br.ReadInt32();
                        switch (i)
                        {
                            case -1:
                                s = "inherit";
                                break;
                            case 0:
                                s = "start";
                                break;
                            case 1:
                                s = "middle";
                                break;
                            case 2:
                                s = "end";
                                break;
                            default:
                                Debug.Assert(false, "TODO Anchor");
                                break;
                        }
                        return "text-anchor='" + s + "'";
                    }
                case 0x0011:
                    {
                        // TODO: Investigate on values
                        string s = "";
                        Int32 i = br.ReadInt32();
                        switch (i)
                        {
                            case 1:
                                s = "underline";
                                break;
                            case 3:
                                s = "line-through";
                                break;
                            default:
                                Debug.Assert(false, "text-decoration");
                                break;
                        }
                        return "text-decoration='" + s + "'";
                    }
                case 0x0016:
                    {
                        return "fill-opacity='"+ReadFloat(br)+"'";
                    }
                case 0x0017:
                    {
                        return "stroke-opacity='" + ReadFloat(br) + "'";
                    }
                case 0x0018:
                    {
                        // TODO: tag Rect
                        byte b = br.ReadByte();
                        b = br.ReadByte();
                        b = br.ReadByte();
                        b = br.ReadByte();
                        Debug.WriteLine("TODO:" + currentTag + " Elem:" + string.Format("{0:X2}", id));
                        Debug.Assert(false, "TODO");
                        return "_1TODO_" + currentTag + "_";
                    }
                case 0x001A:
                    {
                        // TODO: Verifica correttezza
                        string s = "";
                        if (currentTag == "svg")
                            s = ReadSize(br);
                        else
                            s = ReadFloat(br);
                        return "width='" + s + "'";
                    }
                case 0x001B:
                    {
                        // TODO: Verifica correttezza
                        string s = "";
                        if (currentTag == "svg")
                            s = ReadSize(br);
                        else
                            s = ReadFloat(br);
                        return "height='" + s + "'";
                    }
                case 0x001C:
                    {
                        return "r='" + ReadFloat(br) + "'";
                    }
                case 0x001D:
                    {
                        return "rx='" + ReadFloat(br) + "'";
                    }
                case 0x001E:
                    {
                        return "ry='" + ReadFloat(br) + "'";
                    }
                case 0x001F:
                    {
                        return "horiz-adv-x='" + ReadFloat(br) + "'";
                    }
                case 0x0019:
                    {
                        // TODO: Check di questo valore
                        return "textLength='" + br.ReadInt32() + "'";
                    }
                case 0x0020:
                    {
                        return "horiz-origin-x='" + ReadFloat(br) + "'";
                    }
                case 0x0021:
                    {
                        return "horiz-origin-y='" + ReadFloat(br) + "'";
                    }
                case 0x0022:
                    {
                        return "ascent='" + ReadFloat(br) + "'";
                    }
                case 0x0023:
                    {
                        return "descent='" + ReadFloat(br) + "'";
                    }
                case 0x0024:
                    {
                        return "alphabetic='" + ReadFloat(br) + "'";
                    }
                case 0x002B:
                    {
                        string s = ReadFloat(br);
                        return "units-per-em='"+s+"'";
                    }
                case 0x002E:
                    {
                        return "cx='" + ReadFloat(br) + "'";
                    }
                case 0x002F:
                    {
                        return "cy='" + ReadFloat(br) + "'";
                    }
                case 0x0030:
                    {
                        string s = "";
                        if (currentTag == "text")
                        {
                            br.ReadByte();
                        }
                        s = ReadFloat(br);
                        return "y='" + s + "'";
                    }
                case 0x0031:
                    {
                        string s = "";
                        if (currentTag == "text")
                        {
                            br.ReadByte();
                        }
                        s = ReadFloat(br);
                        return "x='" + s + "'";
                    }
                case 0x0032:
                    {
                        return "y1='" + ReadFloat(br) + "'";
                    }
                case 0x0033:
                    {
                        return "y2='" + ReadFloat(br) + "'";
                    }
                case 0x0034:
                    {
                        return "x1='" + ReadFloat(br) + "'";
                    }
                case 0x0035:
                    {
                        return "x2='" + ReadFloat(br) + "'";
                    }
                case 0x0036:
                    {
                        return "k='" + ReadFloat(br) + "'";
                    }
                case 0x0037:
                    {
                        return "g1='" + ReadString(br) + "'";
                    }
                case 0x0038:
                    {
                        return "g2='" + ReadString(br) + "'";
                    }
                case 0x0039:
                    {
                        return "u1='" + ReadString(br) + "'";
                    }
                case 0x003A:
                    {
                        return "u2='" + ReadString(br) + "'";
                    }
                case 0x003B:
                    {
                        return "unicode='" + ReadString(br) + "'";
                    }
                case 0x0040:
                    {
                        Debug.Assert(br.ReadByte() == 1, "rotate");
                        string s = ReadFloat(br);
//                        if (r == -100) s = "auto";
                        return "rotate='" + s + "'";
                    }
                case 0x0042:
                    {
                        string d1 = ReadFloat(br).ToString(); //1
                        string d2 = ReadFloat(br).ToString(); //0
                        string d3 = ReadFloat(br).ToString(); //0
                        string d4 = ReadFloat(br).ToString(); //0
                        string d5 = ReadFloat(br).ToString(); //1
                        string d6 = ReadFloat(br).ToString(); //0
   /* Matrix(<a><b><c><d><e><f>)
    * Translate(<tx>[<ty>])
    * Scale(<sx>[<sy>])
    * Rotate(<rotate_angle>[<cx><cy>])
    * SkewX(<skew_angle>)
    * SkewY(<skew_angle>)
    * ref(svg [, <x>, <y>])
                        */
                        byte kind = br.ReadByte();
                        byte[] trash = br.ReadBytes(3);
                        string s = "";
                        switch (kind)
                        {
                            case 0:
                                {
                                    Debug.Assert(false, "Attributo non riconosciuto dal TAG");
                                    //s = "SkewY(" + d1 + ")";
                                    break;
                                }
                            case 1:
                                {
                                    s = "translate(" + d3 + "," + d6 + ")"; // ****************
                                    break;
                                }
                            case 2:
                                {
                                    s = "scale("+d1+","+d5+")";
                                    break;
                                }
                            case 3:
                                {
                                    // TODO: Verifica se l'ordine di d4 e d2 e' corretto
                                    s = "matrix(" + d1 + "," + d4 + "," + d2 + "," + d5 + "," + d3 + "," + d6 + ")";
                                    break;
                                }
                            case 6:
                                {
                                    // TODO: QUESTA SI TRADUCE IN ALTRA TRASFORMAZIONE
                                    s = "matrix(" + d1 + "," + d4 + "," + d2 + "," + d5 + "," + d3 + "," + d6 + ")";
                                    break;
                                }
                            case 7:
                                {
                                    // matrix(a,b,c,d,e,f): applica all'elemento grafico la matrice di trasformazione (3x3) indicata da [[a c e] [b d f] [0 0 1]]
                                    // s = "matrix(" + d1 + "," + d4 + "," + d2 + "," + d5 + "," + d3 + "," + d6 + ")";
                                    s = "matrix(" + d1 + "," + d4 + "," + d2 + "," + d5 + "," + d3 + "," + d6 + ")";
                                    break;
                                }
                            default:
                                {
                                    Debug.WriteLine("TODO:" + currentTag + " Elem:" + string.Format("{0:X2}", id));
                                    Debug.Assert(false);
                                    // TODO: Traformazione corretta
                                    //Debug.Assert(false, "TODO: Trasform");
                                    s = "matrix(" + d1 + "," + d4 + "," + d2 + "," + d5 + "," + d3 + "," + d6 + ")";
                                    break;
                                }
                        }
                        return "transform='"+s+"'";
                    }
                case 0x0047:
                    {
                         // TODO: investigate on b value
                        byte b = br.ReadByte();
                        string s = ReadFloat(br);
                        return "from='" + s + "'";
                    }
                case 0x0048:
                    {
                        // TODO: investigate on b value
                        byte b = br.ReadByte();
                        string s = ReadFloat(br);
                        return "to='" + s + "'";
                    }
                case 0x0049:
                    {
                        string s1 = ReadFloat(br);
                        string s2 = ReadFloat(br);
                        string s3 = ReadFloat(br);
                        return "by='" + s1 + s2 + s3 + "'";
                    }
                case 0x004A:
                    {
                        // TODO: Leggere bene
                        // Debug.WriteLine("TODO:" + currentTag + " Elem:" + string.Format("{0:X2}", id));
                        // Debug.Assert(false, "TODO:" + currentTag);
                        string s = "display";
                        byte b = br.ReadByte(); // 0x09
                        switch (b)
                        {
                            case 5:
                                s = "font-size";
                                break;
                            case 9:
                                s = "display";
                                break;
                            case 0xFF:
                                s = "";
                                break;
                            default:
                                Debug.Assert(false);
                                break;

                            // TODO: Ecc... dato un id rileva il nome...
                            // TODO: utilizza una enumeration???
                        }
                        b = br.ReadByte();      // 0x00
                        b = br.ReadByte();      // 0x35
/*                        switch (b)
                        {
                            case 9:
                                s = "display";
                                break;
                            default:
                                Debug.Assert(false, "TODO AttributeName");
                                break;
                        }*/
                        return "attributeName='" + s + "'";
                    }
                case 0x004C:    // TAG AGGIUNTO DA THEME STUDIO
                    {
                        // Forse = version
                        // byte[] b = br.ReadBytes(4);  // 00 00 80 3F
                        float f = br.ReadSingle();  // 1.0
                        string s = String.Format("{0:n1}", f);
                        s = s.Replace(',','.');
//                        return "xmlns='http://www.w3.org/2000/svg' version='" + s + "'";
                        return "version='" + s + "'";
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
                case 0x0050:
                    {
                        return "type='"+ReadString(br)+"'";
                    }
                case 0x0051:
                    {
                        string r = String.Format("{0:X2}", br.ReadByte());
                        string g = String.Format("{0:X2}", br.ReadByte());
                        string b = String.Format("{0:X2}", br.ReadByte());
                        // TODO: Verifica questo valore
                        Debug.Assert(br.ReadByte() == 0, "TODO RGB");
                        return "stop-color='#" + r + g + b + "'";
                    }
                case 0x0052:
                    {
                        return "fx='"+ReadFloat(br)+"'";
                    }
                case 0x0053:
                    {
                        return "fy='" + ReadFloat(br) + "'";
                    }
                case 0x0054:
                    {
                        return "offset='" + ReadFloat(br) + "'";
                    }
                case 0x0055:
                    {
                        string s = "";
                        switch (br.ReadByte())
                        {
                            case 0:
                                s = "pad";
                                break;
                            case 1:
                                s = "reflect";
                                break;
                            case 2:
                                s = "repeat";
                                break;
                            default:
                                Debug.WriteLine("TODO:" + currentTag + " Elem:" + string.Format("{0:X2}", id));
                                Debug.Assert(false, "TODO: spreadMethod");
                                break;
                        }
                        return "spreadMethod='" + s + "'";
                    }
                case 0x0056:
                    {
                        string s = "";
                        switch (br.ReadByte())
                        {
                            case 0:
                                s = "userSpaceOnUse";
                                break;
                            case 1:
                                s = "objectBoundingBox";
                                break;
                            default:
                                Debug.WriteLine("TODO:" + currentTag + " Elem:" + string.Format("{0:X2}", id));
                                Debug.Assert(false, "TODO GradientUnit");
                                break;
                        }
                        return "gradientUnits='"+s+"'";
                    }
                case 0x0057:
                    {
                        return "stop-Opacity='" + ReadFloat(br) + "'";
                    }
                case 0x0058:
                    {
                        return "viewBox='" + ReadFloat(br) + " " + ReadFloat(br) + " " + ReadFloat(br) + " " + ReadFloat(br) + "'";
                    }
                case 0x0059:
                    {
                        string s = ReadString(br);
                        return "baseProfile='" + s + "' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink'";
                    }
                case 0x005A:    // TAG AGGIUNTO DA THEME STUDIO
                    {
                        // TODO: investigate on values
                        string s = "";
                        switch (br.ReadByte())
                        {
                            case 1:
                                s = "magnify";
                                break;
                            default:
                                Debug.WriteLine("TODO:" + currentTag + " Elem:" + string.Format("{0:X2}", id));
                                Debug.Assert(false, "TODO zoomAndPan");
                                break;
                        }
                        return "zoomAndPan='"+s+"'";
                    }
                case 0x005B:
                    {
                        return "preserveAspectRatio='" + ReadString(br) + "'"; 
                    }
                case 0x005C:
                    {
                        return "id='" + ReadString(br) + "'";
                    }
                case 0x005D:
                    {
                        return "xml:base='"+ ReadString(br) + "'";
                    }
                case 0x005E:
                    {
                        return "xml:lang='" + ReadString(br) + "'";
                    }
                case 0x005F:
                    {
                        return "xml:space='" + ReadString(br) + "'";
                    }
                case 0x0060:
                    {
                        byte qta = br.ReadByte();
                        string s="";
                        for (int i = 0; i < qta; i++)
                        {
                            s+=ReadString(br) + " ";
                        }
                        s = s.Trim();
                        return "requiredExtensions='" + s + "'";
                    }
                case 0x0061:
                    {
                        byte qta = br.ReadByte();
                        string s = "";
                        for (int i = 0; i < qta; i++)
                        {
                            s = s + " " + ReadString(br);
                        }
                        return "requiredFeatures='" + s.Trim() + "'";
                    }
                case 0x0062:
                    {
                        byte qta = br.ReadByte();
                        string s = "";
                       for (int i = 0; i < qta; i++)
                       {
                           s = s + " " + ReadString(br);
                       }
                       return "systemLanguage='" + s.Trim() + "'";
                    }
                case 0x0067:
                    {
                        string s = ReadString(br);
                        if (s.Length == 0)
                        {
                            s = "onLoad"; // TODO: Forse non equivale a questo...
                        }
                        return "xlink:actuate='" + s + "'";
                    }
                case 0x0068:
                    {
                        string s = ReadString(br);
                        if (s.Length == 0)
                        {
                            s = "";
                        }
                        else
                        {
                            Debug.Assert(false, "Controlla che sia una stringa valida");
                        }
                        return "xlink:arcrole='" + s + "'";
                    }
                case 0x0069:
                    {
                        string s = ReadString(br);
                        if (s.Length == 0)
                        {
                            s = "";
                        }
                        else
                        {
                            Debug.Assert(false, "Controlla che sia una stringa valida");
                        }
                        return "xlink:role='" + s + "'";
                    }
                case 0x006A:
                    {
                        string s = ReadString(br);
                        if (s.Length == 0)
                        {
                            s = "other";
                        }
                        return "xlink:show='" + s + "'";
                    }
                case 0x006B:
                    {
                        return "xlink:title='" + ReadString(br) + "'";
                    }
                case 0x006C:
                    {
                        string s = ReadString(br);
                        if (s.Length == 0)
                        {
                            s = "simple";
                        }
                        else
                        {
                            Debug.Assert(false, "Controlla che sia una stringa valida");
                        }
                        return "xlink:type='"+s+"'";
// Modifica del 26/07/08
//                        return "xlink:type='"+ReadString(br)+"'";
                    }
                case 0x006D:
                    {
                        string s = ReadString(br);
                        //byte b = br.ReadByte();
                        // TODO: Verifica il tag use
                        // string s1 = ReadString(br);
                        if (currentTag.ToLower() == "use")
                            Debug.Assert(false, "TODO: Gestione element USE");
                        return "xlink:href='" + s + "'";
                    }
                                case 0x006E:
                                    {
                                        br.ReadByte();
                                        br.ReadByte();
                                        UInt32 sec = br.ReadUInt32();
                                        br.ReadByte();

                                        br.ReadBytes(3);
                                        br.ReadUInt32();
                                        string s = ReadString(br);
                                        br.ReadBytes(4);
                                        UInt32 sec2 = br.ReadUInt32();
                                        Debug.Assert(sec == sec2);
                                        br.ReadByte();

                                        br.ReadBytes(3);
                                        UInt32 sec3 = br.ReadUInt32();
                                        br.ReadByte();
                                        Debug.Assert(sec == sec3);

                                        // TODO: verifica che legga bene
                                        return "begin='" + sec/1000 +"s'";
                                    }
                case 0x006F:
                    {
                        UInt32 sec = br.ReadUInt32();
                        Debug.Assert(br.ReadByte() == 0, "TODO um");
                        // TODO: Interpretare la unita' di misura
                        sec = sec / 1000;
                        return "dur='" + sec + "s'";
                    }
                case 0x0070:
                    {
                        // TODO: verifica che legga bene...
                        return "repeatCount='" + ReadFloat(br) + "'";
                    }
                case 0x0071:
                    {
                        // TODO:
                        //Debug.WriteLine("TODO:" + currentTag + " Elem:" + string.Format("{0:X2}", id));
                        //Debug.Assert(false, "TODO");
                        //string s = ReadFloat(br);
                        UInt32 sec = br.ReadUInt32();
                        return "repeatDur='" + sec / 1000 + "'";
                    }
                case 0x0077:
                    {
                        UInt16 qta = br.ReadUInt16();
                        string s = "";
                        for (int i = 0; i < qta; i++)
                        {
                            Int16 n1 = br.ReadInt16();
                            if (n1 != 0)
                                n1 = 1;
                            if (s != "")
                                s += ";";
                            s += n1;
                        }
                        // TODO: Verifica correttezza di n1
                        return "keyTimes='" + s + "'";
                    }
                case 0x0078:
                    {
                        string s = "";
                        byte b = br.ReadByte();
                        switch (b)
                        {
                            case 0:
                                s = "discrete";
                                break;
                            case 1:
                                s = "linear";
                                break;
                            case 2:
                                s = "paced";
                                break;
                            case 3:
                                s = "spline";
                                    break;
                            default:
                                Debug.Assert(false, "TODO: calc");
                                break;
                        }
                        return "calcMode='"+ s+"'";
                    }
                case 0x007A:
                    {
                        Debug.WriteLine("TODO:" + currentTag + " Elem:" + string.Format("{0:X2}", id));
                        Debug.Assert(false, "TODO");
                        // BOH?!? TAG di animateMotion
                        return " ";
                    }
                case 0x007B:
                    {     
                        // TODO: 
                        //Debug.Assert(false, "TODO GradientTransform");
                        byte b = br.ReadByte();
                        b = br.ReadByte();
                        b = br.ReadByte();
                        b = br.ReadByte();

                        b = br.ReadByte();
                        b = br.ReadByte();
                        b = br.ReadByte();
                        b = br.ReadByte();

                        string d1 = ReadFloat(br).ToString();
                        string d2 = ReadFloat(br).ToString();
                        string d3 = ReadFloat(br).ToString();
                        string d4 = ReadFloat(br).ToString();
                        // TODO: gestire translate oltre che matrix???
                        return "gradientTransform=''";
                    }
                case 0x00A3:
                    {
                        br.ReadByte();
                        UInt16 qta = br.ReadUInt16();
                        string s = "";
                        for (int i = 0; i < qta; i++)
                        {
                            Int32 n1 = br.ReadInt32();
                            if (s != "")
                                s += ";";
                            switch (n1)
                            {
                                case 0x22:
                                    s += "inherit";
                                    break;
                                default:
                                    Debug.WriteLine("TODO:" + currentTag + " Elem:" + string.Format("{0:X2}", id));
                                    Debug.Assert(false);
                                    break;
                            }
                        }
                        // TODO: Verifica valori
                        return "values = '" + s + "'";
                    }
                case 0x0087:
                    {
                    return "fill = 'freeze'";
                    }
/*                case 0x0088:
                    {
                        // TODO: TAG IMAGE
                        return " ";
                    }*/
                /*                case 0x0090:
                                    {
                                        // TODO: TAG PATH
                                    }*/
                case 0x03E8:    // Fine Attributi
                    {
                        // br.BaseStream.Seek(br.BaseStream.Position - 2);
                        return "";
                    }
                case 0x00BE:
                    {
                        Debug.WriteLine("TODO:" + currentTag + " Elem:" + string.Format("{0:X2}", id));
                        Debug.Assert(false, "TAG DI ???");
                        return " ";
                    }
            }
            string err = String.Format("{0:X6}", br.BaseStream.Position-2);
            string val = String.Format("{0:X4}", id);
            //if (id != 0x7b)
               Debug.Assert(false, "Unsupported ATTRIB Offset:" + err + "  Value:"+val + " TAG:"+currentTag);

            // SALTA TUTTI I RESTANTI ATTRIBUTI
            bool found = false;
            while (!found)
            {
                while (br.ReadByte() != 0xE8);
                if (br.ReadByte() == 0x03) found = true;
            }
            return "";
        }


        private string DecodeAttribs(BinaryReader br)
        {
            string ris = "";
            string s = "";
            do
            {
                s = FactoryAttrib(br);
                if (s.StartsWith("id"))
                {
                    Debug.Write("*** ");
                }
                if (s != "")
                {
                    Debug.WriteLine(s);
                }
                if (ris != "" && s != "") ris += " ";
                ris = ris + s;
            } while (s != "");
            return ris;
        }


        private string FactoryElements(BinaryReader br)
        {
            byte id = br.ReadByte();
            switch (id)
            {
                case 0x00:
                    // OPEN TAG SVG
                    return "svg";
                case 0x01:
                    return "altGlyph";
                case 0x02:
                    return "altGlyphDef";
                case 0x03:
                    return "defs";
                case 0x04:
                    return "descr";
                case 0x07:
                    return "title";
                case 0x08:
                    return "font-face-name";
                case 0x09:
                    return "font-face-src";
                case 0x0A:
                    return "font-face-uri";
                case 0x0B:
                    return "g";
                case 0x0C:
                    return "glyphRef";
                case 0x0E:
                    return "script";
                case 0x0F:
                    return "switch";
                case 0x11:
                    return "hkern";
                case 0x12:
                    return "a";
                case 0x13:
                    return "font";
                case 0x14:
                    return "font-face";
                case 0x15:
                    return "glyph";
                case 0x16:
                    return "image";
                case 0x17:
                    return "missing-glyph";
                case 0x18:
                    return "style";
                case 0x19:
                    return "text";
                case 0x1A:
                    return "use";
                case 0x1B:
                    return "circle";
                case 0x1C:
                    return "ellipse";
                case 0x1D:
                    return "line";
                case 0x1E:
                    return "path";
                case 0x1F:
                    return "polygon";
                case 0x20:
                    return "polyline";
                case 0x21:
                    return "rect";
                case 0x22:
                    return "animate";
                case 0x23:
                    return "animateColor";
                case 0x24:
                    return "animateMotion";
                case 0x25:
                    return "animateTransform";
                case 0x26:
                    return "set";
                case 0x27:
                    return "mpath";
                case 0x28:
                    return "linearGradient";
                case 0x29:
                    return "radialGradient";
                case 0x2A:
                    return "stop";
                case 0xFD:
                    {
                        // Dovrebbe esserci del testo ma e' sempre vuoto....
                        byte len = br.ReadByte();
                        byte[] trash = br.ReadBytes(len);
                        for (int i = 0; i < trash.Length - 1; i++)
                        {
                            //Debug.Assert(trash[i] == 0, "Errore Trash");
                        }
                        // Dovrebbero essere tutti vuoti...
                        return FactoryElements(br);
                    }
                case 0xFE:  // Chiude Tag
                    return "";
                case 0xFF:  // FINE FILE;
                    Debug.Assert(false);
                    return "";
            }
            string err = String.Format("{0:X6}", br.BaseStream.Position-1);
            string val = String.Format("{0:X2}", id);
            Debug.Assert(false, "Unsupported TAG Offset:" + err + "   MissingTag:"+val);

            bool found = false;
            while (!found)
            {
                while (br.ReadByte() != 0xE8) ;
                if (br.ReadByte() == 0x03) found = true;
            }
            return "";
        }


        private string DecodeElements(BinaryReader br)
        {
            Stack<string> stack = new Stack<string>();
            string ris = "";
            string element = "";

            bool exit = false;
            while (!exit && br.BaseStream.Position < br.BaseStream.Length - 1)
            {
                element = FactoryElements(br);
                currentTag = element;
                string tabs = "";
                if (element != "")
                {
                    tabs = tabs.PadRight(stack.Count, '\t');
                    Debug.Write(tabs + "<" + element + ">\r\n");
                    stack.Push(element);
                    ris += tabs + "<" + element + " " + DecodeAttribs(br) + " >\r\n";
                }
                else
                {
                    if (stack.Count > 0)
                    {
                        element = stack.Pop();
                        tabs = tabs.PadRight(stack.Count, '\t');
                        ris += tabs + "</" + element + " >\r\n";
                        Debug.Write(tabs + "</" + element + ">\r\n");
                    }
                    else
                    {
                        exit = true;
                        //Debug.Assert(false, "Stack Empty");
                    }
                }
            }
            return ris;
        }


        public string DecodeFile(string filename)
        {
            FileStream strm = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader br = new BinaryReader(strm);
            string ris = DecodeFile(br);
            br.Close();
            strm.Close();
            return ris;
        }


        public string DecodeFile(byte[] data)
        {
            BinaryReader br = new BinaryReader(new MemoryStream(data));
            string ris = DecodeFile(br);
            br.Close();
            return ris;
        }


        public string DecodeFile(BinaryReader br)
        {
            this.currentTag = "";
            Int16 check = br.ReadInt16();
            // Carbide   3RD Ed FP1
            if (check != 0x56CC && check != 0x56CE)
                return "";

            check = br.ReadInt16();
            if (check != 0x03FA) 
                return "";

            string s = "<?xml version='1.0' encoding='UTF-8'?>\r\n";
            s += "<!DOCTYPE svg PUBLIC '-//W3C//DTD SVG 1.1 Tiny//EN' 'http://www.w3.org/Graphics/SVG/1.1/DTD/svg11-tiny.dtd'>\r\n";
            s += "<!-- Generated from SISXDecoder 1.0beta -->\r\n";
            s += DecodeElements(br);
            return s;
        }

    }
}
