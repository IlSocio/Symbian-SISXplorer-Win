using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Diagnostics;


namespace SISX.Fields
{
    public class SISController : SISField
    {
        public SISInfo info;
        public SISSupportedOptions options;
        public SISSupportedLanguages languages;
        public SISPrerequisites prerequisites;
        public SISProperties properties;
        public SISLogo logo;
        public SISInstallBlock installBlock; // MetaData ed embedding SIS
        public ArrayList signatures; // SISSignatureCertificateChain;

        /// <summary>
        /// Indice relativo all'array di SISData per identificare la SISDataUnit di questo controller all'interno dell'array SISData
        /// </summary>
        public SISDataIndex dataIndex;


        public SISController(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            signatures = new ArrayList();
            info = (SISInfo) SISField.Factory(br);
            options = (SISSupportedOptions) SISField.Factory(br);
            languages = (SISSupportedLanguages) SISField.Factory(br);
            prerequisites = (SISPrerequisites) SISField.Factory(br);
            properties = (SISProperties)SISField.Factory(br);
            SISField fld = SISField.Factory(br);
            if (fld is SISLogo)
            {
                logo = fld as SISLogo;
                fld = SISField.Factory(br);
            }

            installBlock = (SISInstallBlock) fld;

            fld = SISField.Factory(br);
            while (fld is SISSignatureCertificateChain)
            {
                signatures.Add(fld);
                fld = SISField.Factory(br);
            }

            dataIndex = (SISDataIndex)fld;
        }
    }
}
