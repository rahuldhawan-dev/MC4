using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Requisition : IEntityWithCreationTracking<User>
    {
        #region Consts

        public struct StringLengths
        {
            public const int SAP_REQUISITION_NUMBER_MAX_LENGTH = 50;
        }

        public struct DisplayNames
        {
            public const string
                REQUISITION_TYPE = "Purchase Order (PO) Type",
                SAP_REQUISITION_NUMBER = "SAP Purchase Order(PO) #";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [StringLength(StringLengths.SAP_REQUISITION_NUMBER_MAX_LENGTH)]
        [View(DisplayNames.SAP_REQUISITION_NUMBER)]
        public virtual string SAPRequisitionNumber { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual User CreatedBy { get; set; }

        [View(DisplayNames.REQUISITION_TYPE)]
        public virtual RequisitionType RequisitionType { get; set; }

        public virtual WorkOrder WorkOrder { get; set; }

        #endregion
    }
}
