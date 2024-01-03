using System;

namespace Permits.Data.Client.Entities
{
    public class CompanyReport
    {
        public virtual int PermitId { get; set; }
        public virtual string ArbitraryIdentifier { get; set; }
        public virtual string StreetAddress { get; set; }
        public virtual string Town { get; set; }
        public virtual string County { get; set; }
        public virtual string State { get; set; }
        public virtual decimal PermitFee { get; set; }
        public virtual decimal InspectionFee { get; set; }
        public virtual decimal BondFee { get; set; }
        public virtual decimal TotalCharged { get; set; }
        public virtual bool? Reconciled { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? CanceledAt { get; set; }
        public virtual DateTime? SubmittedAt { get; set; }
        public virtual DateTime? PaymentReceivedAt { get; set; }
        public virtual bool Refunded { get; set; }
    }
}
