using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ClaimsRepresentativeMap : ClassMap<ClaimsRepresentative>
    {
        #region Constructors

        public ClaimsRepresentativeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable().Unique();
        }

        #endregion
    }
}
