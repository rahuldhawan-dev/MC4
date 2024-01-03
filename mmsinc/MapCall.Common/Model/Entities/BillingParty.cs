using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class BillingParty : IEntityLookup
    {
        #region Properties

        public virtual int Id { get; set; }

        [Required, StringLength(CreateTrafficControlTicketsForBug2341.StringLengths.BillingParties.DESCRIPTION)]
        public virtual string Description { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? EstimatedHourlyRate { get; set; }

        public virtual string Payee { get; set; }

        public virtual IList<BillingPartyContact> BillingPartyContacts { get; set; }

        #endregion

        #region Constructors

        public BillingParty()
        {
            BillingPartyContacts = new List<BillingPartyContact>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
