using System;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ContractorContactType : ReadOnlyEntityLookup
    {
        public virtual ContactType ContactType { get; set; }
        public override string Description => ContactType.Description;
    }

    [Serializable]
    public class ContractorContactTypeDisplayItem : DisplayItem<ContractorContactType>
    {
        [SelectDynamic("Id", Field = "ContactType")]
        public override int Id { get; set; }

        [SelectDynamic("Description")]
        public string ContactType { get; set; }

        public override string Display => ContactType;
    }
}
