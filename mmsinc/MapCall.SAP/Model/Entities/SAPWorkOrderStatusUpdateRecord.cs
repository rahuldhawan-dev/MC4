using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPWorkOrderStatusUpdateRecord : SAPEntity
    {
        #region Properties

        public virtual string WorkOrderNo { get; set; }
        public virtual string OperationNo { get; set; }
        public virtual string AssignmentStart { get; set; }
        public virtual string AssignmentFinish { get; set; }
        public virtual string StatusNumber { get; set; }
        public virtual string StatusNonNumber { get; set; }
        public virtual string AssignedEngineer { get; set; }
        public virtual string DispatcherId { get; set; }
        public virtual string EngineerId { get; set; }
        public virtual string ItemTimeStamp { get; set; }

        #endregion
    }
}
