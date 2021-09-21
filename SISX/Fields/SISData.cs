using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISData : SISField
    {
        /// <summary>
        /// C'e' una DataUnit per ogni SISController
        /// </summary>
        public SISArray dataUnits;  // SISArray di SISDataUnit

        public SISData(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            dataUnits = SISField.Factory(br) as SISArray;
        }
    }
}
