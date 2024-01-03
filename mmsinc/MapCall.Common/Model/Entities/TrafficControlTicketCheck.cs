using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TrafficControlTicketCheck : IEntity
    {
        public virtual int Id { get; set; }
        public virtual TrafficControlTicket TrafficControlTicket { get; set; }
        public virtual BillingParty BillingParty => TrafficControlTicket.BillingParty;

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY, ApplyFormatInEditMode = false)]
        public virtual decimal Amount { get; set; }

        public virtual int CheckNumber { get; set; }
        public virtual string Memo { get; set; }
        public virtual bool? Reconciled { get; set; }
        public virtual bool Unique { get; set; }
    }
}
