using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace SISX.Fields
{
    public class SISContent : SISField
    {
        public SISControllerChecksum controllerChecksum = null;
        public SISDataChecksum dataChecksum = null;
        public SISCompressed controllerCompressed;    // <SISCONTROLLER>

        /// <summary>
        /// Dati veri e propri di tutti i Controllers
        /// </summary>
        public SISData data; 

        /// <summary>
        /// Contiene i Metadati del file SISX
        /// </summary>
        public SISController _controller;


        public SISContent(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            // Verifica presenza/assenza di controllerChecksum (opzionale)
            SISField fld = SISField.Factory(br);
            if (fld is SISControllerChecksum) 
            {
                controllerChecksum = fld as SISControllerChecksum;
                fld = SISField.Factory(br);
            }

            // Verifica presenza/assenza di dataChecksum (opzionale)
            if (fld is SISDataChecksum)
            {
                dataChecksum = fld as SISDataChecksum;
                fld = SISField.Factory(br);
            }

            System.Diagnostics.Debug.Assert(fld is SISCompressed);
            controllerCompressed = fld as SISCompressed;

            // Crea l'istanza di SISController utilizzando i dati di decompressedData            
            MemoryStream ms_in = new MemoryStream(controllerCompressed.data, false);
            BinaryReader br_in = new BinaryReader(ms_in);
            _controller = SISField.Factory(br_in) as SISController;
            br_in.Close();
            ms_in.Close();

            data = SISField.Factory(br) as SISData;
        }


    }
}

