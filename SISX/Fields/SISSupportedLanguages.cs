using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{

    public class SISSupportedLanguages : SISField
    {
        public SISArray languages;  // SISLanguage

        public SISSupportedLanguages(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            languages = SISField.Factory(br) as SISArray;
        }

        public override string ToString()
        {
            return languages.ToString();
        }
    }    
}
