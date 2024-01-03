using MapCall.Common.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.SAP.MiscellaneousWorksWS;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPMiscellaneousWorksRegisterInformation : SAPEntity
    {
        #region Properties

        public virtual string Size { get; set; }
        public virtual string MIUnumber { get; set; }
        public virtual string EncoderID { get; set; }
        public virtual string Read { get; set; }
        public virtual string ReadType { get; set; }

        #endregion
    }
}
