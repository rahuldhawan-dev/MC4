using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class GeneralLiabilityClaimTypeMap : ClassMap<GeneralLiabilityClaimType>
    {
        #region Constructors

        public GeneralLiabilityClaimTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable().Unique();
        }

        #endregion
    }
}
