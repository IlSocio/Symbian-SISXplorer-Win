using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISPrerequisites : SISField
    {
        public SISArray targhetDevices;     // SISDependency
        public SISArray dependencies;       // SISDependency

        public SISPrerequisites(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            targhetDevices = (SISArray)SISField.Factory(br);
            dependencies = (SISArray)SISField.Factory(br);
        }
    }
}
