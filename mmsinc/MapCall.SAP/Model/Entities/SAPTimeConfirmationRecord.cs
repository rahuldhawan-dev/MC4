using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPTimeConfirmationRecord : SAPEntity, IHasStatus
    {
        #region Properties

        public virtual string UserID { get; set; }
        public virtual string SAPOrderNo { get; set; }
        public virtual string OperationID { get; set; }
        public virtual string DateCompleted { get; set; }
        public virtual string Employee { get; set; }
        public virtual decimal? ActualWork { get; set; }
        public virtual string UOM { get; set; }
        public virtual string Finalize { get; set; }
        public virtual string StartDate { get; set; }
        public virtual string StartTime { get; set; }
        public virtual string EndDate { get; set; }
        public virtual string EndTime { get; set; }
        public virtual string ReasonCode { get; set; }
        public virtual string FSRComments { get; set; }

        #endregion

        #region WebService Response Properties

        public virtual string SAPStatus { get; set; }

        #endregion
    }
}
