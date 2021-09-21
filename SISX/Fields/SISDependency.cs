using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISDependency : SISField
    {
        public SISUid uid;
        public SISVersionRange versionRange;
        public SISArray dependencyNames;    // SISString


        public SISDependency(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            uid = (SISUid) SISField.Factory(br);
            SISField fld = SISField.Factory(br);
            if (fld is SISVersionRange)
            {
                versionRange = (SISVersionRange) fld;
                fld = (SISArray) SISField.Factory(br);
            }
            dependencyNames = (SISArray) fld;
        }
    }
}
