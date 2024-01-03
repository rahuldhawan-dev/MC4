using MapCall.Common.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.SAP.WorkOrderStatusUpdateWS;
using MMSINC.Utilities;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPWorkOrderStatusUpdateRequest : SAPEntity, IHasStatus, ISAPServiceEntity
    {
        #region Properties

        #region WebService Request Properties

        public virtual string SourceIdentifier { get; set; }
        public IEnumerable<SAPWorkOrderStatusUpdateRecord> SAPWorkOrderStatusUpdateRecords { get; set; }

        #endregion

        #region WebService Response Properties

        [Obsolete("Use SAPStatus Property instead for this class. SAP has both properties for some reason.")]
        public virtual string SAPErrorCode { get; set; }

        public virtual string SAPStatus { get; set; }

        #endregion

        #endregion

        #region Constructors

        public SAPWorkOrderStatusUpdateRequest() { }

        public SAPWorkOrderStatusUpdateRequest(SAPWorkOrderStatusUpdateRequest sapWorkOrderStatusUpdateRequest)
        {
            SourceIdentifier = MAPCALL_SOURCE_IDENTIFIER;

            if (sapWorkOrderStatusUpdateRequest.SAPWorkOrderStatusUpdateRecords != null &&
                sapWorkOrderStatusUpdateRequest.SAPWorkOrderStatusUpdateRecords.Any())
            {
                var WorkOrderStatusUpdateRecords =
                    from wo in sapWorkOrderStatusUpdateRequest.SAPWorkOrderStatusUpdateRecords
                    select new SAPWorkOrderStatusUpdateRecord {
                        StatusNumber = wo.StatusNumber,
                        StatusNonNumber = wo.StatusNonNumber,
                        AssignedEngineer = wo.AssignedEngineer,
                        AssignmentFinish = wo.AssignmentFinish,
                        AssignmentStart = wo.AssignmentStart,
                        DispatcherId = wo.DispatcherId,
                        EngineerId = wo.EngineerId,
                        ItemTimeStamp = wo.ItemTimeStamp,
                        OperationNo = wo.OperationNo,
                        WorkOrderNo = wo.WorkOrderNo
                    };

                SAPWorkOrderStatusUpdateRecords = WorkOrderStatusUpdateRecords.ToList();
            }
        }

        #endregion

        #region Exposed Methods

        public WOStatusUpdateQuery WorkOrderStatusUpdateRequest()
        {
            WOStatusUpdateQuery request = new WOStatusUpdateQuery();
            request.SourceIdentifier = SourceIdentifier;

            if (SAPWorkOrderStatusUpdateRecords != null && SAPWorkOrderStatusUpdateRecords.Any())
            {
                request.Record = SAPWorkOrderStatusUpdateRecords.Select(wo => new WOStatusUpdateQueryRecord {
                    Status_Number = wo.StatusNumber,
                    Status_NonNumber = wo.StatusNonNumber,
                    AssignedEngineer = wo.AssignedEngineer,
                    EngineerId = wo.EngineerId,
                    ItemTimeStamp = wo.ItemTimeStamp,
                    AssignmentFinish = wo.AssignmentFinish,
                    DispatcherId = wo.DispatcherId,
                    OperationNo = wo.OperationNo,
                    AssignmentStart = wo.AssignmentStart,
                    WorkOrderNo = wo.WorkOrderNo
                }).ToArray();
            }

            return request;
        }

        #endregion
    }
}
