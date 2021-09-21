using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Utility;


namespace SISX.Fields
{
    public class SISInstallBlock : SISField
    {
        public SISArray files;          // SISFileDescription
        public SISArray embeddedSIS;    // SISController
        public SISArray ifBlocks;       // SISIf

        public SISInstallBlock(BinaryReader br)
            : base( br )
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            files = (SISArray)SISField.Factory( br );
            embeddedSIS = (SISArray)SISField.Factory( br );   // TODO: Non gestisce ancora bene l'embedding di altri SIS
            ifBlocks = (SISArray)SISField.Factory( br );
        }

        public override string ToString()
        {
            string s = "";

            if (s != "") s += "\r\n";
            foreach (SISFileDescription file in files.fields)
            {
                string oper = Bits.GetStringFromBitField < TSISFileOperation > (file.operation);
                if (oper == "") 
                    oper = TSISFileOperation.EOpInstall.ToString();

                uint options = file.operationOptions;
                string optionStr = Bits.GetStringFromBitField<TSISFileOperationOption>( options );
                optionStr = Bits.GetStringFromBitField<TInstFileRunOption>( options, optionStr );
                optionStr = Bits.GetStringFromBitField<TInstTextOption>( options, optionStr );
                if (optionStr != "") optionStr += ", ";

                if (s != "") s += "\r\n";
                s += oper + "( ";
                s += optionStr;

                if (file.target.aString.ToLower() != "")
                    s += "\""+file.target+"\"";
                else
                    s += "FileIndex" + file.fileIndex;

                s += " );";
            }

            foreach (SISController cnt in embeddedSIS.fields)
            {                
                if (s != "") s += "\r\n\r\n";
                string name = "foo";
                if (cnt.info.names.fields.Count > 0) name = cnt.info.names.fields[0].ToString();
                s += "EMBEDDED_SIS( \""+ name +".sis\" )\r\n";
                s += "{\r\n";
                // Prima di concatenare al risultato rimpiazza "\r\n" con "\r\n\t"
                s += "\t"+ cnt.installBlock.ToString().Replace("\r\n", "\r\n\t");
                s += "\r\n}";
            }
            foreach (SISIf ifBlock in ifBlocks.fields)
            {
                if (s != "") s += "\r\n\r\n";
                s += ifBlock.ToString();
            }
            return s;
        }
    }
}
