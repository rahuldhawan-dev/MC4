using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ContractorContactTypeMap : ClassMap<ContractorContactType>
    {
        #region Constructors

        public ContractorContactTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            References(x => x.ContactType).Not.Nullable();
        }

        #endregion
    }
}
