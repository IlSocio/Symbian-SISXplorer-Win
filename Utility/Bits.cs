using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public class Bits
    {
        public static string GetStringFromEnum<E>(uint op)
        {
            foreach (int item in Enum.GetValues( typeof( E ) ))
            {
                if (item == op)
                {
                    E oper = (E)Enum.Parse( typeof( E ), item.ToString() );
                    return oper.ToString();
                }
            }
            return "";
        }


        public static string GetStringFromBitField<E>(uint op, string s)
        {
            foreach (int item in Enum.GetValues( typeof( E ) ))
            {
                int mask = (int)(item & op);
                if (mask == item)
                {
                    //                    E oper = mask;
                    E oper = (E)Enum.Parse( typeof( E ), item.ToString() );
                    if (s != "") s += "; ";
                    s += oper.ToString();
                }
            }
            return s;
        }


        public static string GetStringFromBitField<E>(uint op)
        {
            return GetStringFromBitField<E>( op, "" );
        }

    }
}

