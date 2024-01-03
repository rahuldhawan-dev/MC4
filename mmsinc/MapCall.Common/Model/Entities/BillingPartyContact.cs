using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class BillingPartyContact : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual BillingParty BillingParty { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual ContactType ContactType { get; set; }

        #endregion
    }

    [Serializable]
    public class BillingPartyContactType : IEntityLookup
    {
        public virtual int Id { get; set; }
        public virtual ContactType ContactType { get; set; }
        public virtual string Description => ContactType.Description;

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    [Serializable]
    public class BillingPartyContactTypeDisplayItem : DisplayItem<BillingPartyContactType>
    {
        [SelectDynamic("Id", Field = "ContactType")]
        public override int Id { get; set; }

        [SelectDynamic("Description")]
        public string ContactType { get; set; }

        public override string Display => ContactType;
    }
}
