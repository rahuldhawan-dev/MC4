using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ServiceRestoration : IEntity
    {
        #region Constants

        public struct StringLengths
        {
            public const int
                FINAL_COMPLETION_BY = 30,
                FINAL_INVOICE_NUMBER = 15,
                PARTIAL_COMPLETION_BY = 30,
                PARTIAL_INVOICE_NUMBER = 15,
                PURCHASE_ORDER_NUMBER = 20;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual User ApprovedBy { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? ApprovedOn { get; set; }

        public virtual bool Cancel { get; set; }

        [DisplayName("Estimated Paving Square Footage")]
        public virtual decimal? EstimatedRestorationAmount { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? EstimatedValue { get; set; }

        public virtual ServiceRestorationContractor FinalRestorationCompletionBy { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? FinalRestorationCost { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? FinalRestorationDate { get; set; }

        public virtual string FinalRestorationInvoiceNumber { get; set; }
        public virtual RestorationMethod FinalRestorationMethod { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? FinalRestorationAmount { get; set; }

        public virtual decimal? FinalRestorationTrafficControlHours { get; set; }
        public virtual User InitiatedBy { get; set; }
        public virtual string Notes { get; set; }

        public virtual ServiceRestorationContractor PartialRestorationCompletionBy { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? PartialRestorationDate { get; set; }

        public virtual string PartialRestorationInvoiceNumber { get; set; }
        public virtual RestorationMethod PartialRestorationMethod { get; set; }

        [DisplayName("Partial Paving Square Footage")]
        public virtual decimal? PartialRestorationAmount { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        [DisplayName("Partial Invoice Amount")]
        public virtual decimal? PartialRestorationCost { get; set; }

        public virtual decimal? PartialRestorationTrafficControlHours { get; set; }

        public virtual User RejectedBy { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? RejectedOn { get; set; }

        public virtual Service Service { get; set; }
        public virtual RestorationType RestorationType { get; set; }
        public virtual string PurchaseOrderNumber { get; set; }

        #region Logical Fields

        public virtual bool Approved { get; set; }

        #endregion

        #endregion
    }
}
