using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{

    public class SISElseIf : SISField
    {
        public SISExpression expression;
        public SISInstallBlock installBlock;

        public SISElseIf(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            expression = (SISExpression)SISField.Factory(br);
            installBlock = (SISInstallBlock)SISField.Factory(br);
        }

        public override string ToString()
        {
            string s = "Else If ( " + expression.ToString() + " ) \r\n";
            s += "{\r\n";
            s += "\t" + installBlock.ToString().Replace( "\r\n", "\r\n\t" );
            s += "\r\n}";
            return s;
        }

    }
}
