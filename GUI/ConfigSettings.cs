using System;
using System.Collections.Generic;
using System.Text;

namespace SISXplorer
{
    public class ConfigSettings
    {

        public static string GetHex(ulong val)
        {
            return "0x" + String.Format("{" + SISXplorer.Properties.Settings.Default.HexFormat + "}", val);
        }

    }
}
