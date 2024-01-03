using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPDeviceRemovalReplaceRegister : SAPEntity
    {
        #region Properties

        public virtual string Dials { get; set; }
        public virtual string UOM { get; set; }
        public virtual string Size { get; set; }
        public virtual string MIUNumber { get; set; }

        public virtual string EncoderID { get; set; }

        //public virtual string CurrentRead { get; set; }
        public virtual string ReadType { get; set; }
        public virtual string OldRead { get; set; }
        public virtual string NewRead { get; set; }

        #endregion
    }
}
