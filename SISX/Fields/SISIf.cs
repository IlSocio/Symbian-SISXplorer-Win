using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{

    public class SISIf : SISField
    {
        public SISExpression expression;
        public SISInstallBlock installBlock;
        public SISArray elseIfs;            // SISElseIf

        public SISIf(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            expression = (SISExpression)SISField.Factory(br);
            installBlock = (SISInstallBlock)SISField.Factory(br);
            elseIfs = (SISArray)SISField.Factory(br);
        }

        public override string ToString()
        {
            string s = "If ( " + expression.ToString() + " ) \r\n";
            s +="{\r\n";
            s += "\t" + installBlock.ToString().Replace("\r\n", "\r\n\t");
            s += "\r\n}";
            if (elseIfs.fields.Count > 0)
            {
                foreach (SISElseIf elseIf in elseIfs.fields) 
                {
                    s += " " + elseIf.ToString();
                }
            }
            return s;
        }

    }
}
