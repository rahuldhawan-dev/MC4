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
    public class SAPMiscellaneousWorksTestResults : SAPEntity
    {
        #region Properties

        public virtual string RegisterId { get; set; }
        public virtual string LowMedHighInd { get; set; }
        public virtual string InitialRepair { get; set; }
        public virtual string Accuracy { get; set; }
        public virtual string CalculatedVolume { get; set; }
        public virtual string TestFlowRate { get; set; }
        public virtual string StartRead { get; set; }
        public virtual string EndRead { get; set; }
        public virtual string Result { get; set; }

        #endregion
    }
}
