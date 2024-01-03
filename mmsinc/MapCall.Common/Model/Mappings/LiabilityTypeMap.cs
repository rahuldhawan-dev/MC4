using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class LiabilityTypeMap : ClassMap<LiabilityType>
    {
        #region Constructors

        public LiabilityTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable().Unique();
        }

        #endregion
    }
}
