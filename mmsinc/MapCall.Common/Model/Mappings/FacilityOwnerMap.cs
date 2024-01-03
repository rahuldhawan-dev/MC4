using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityOwnerMap : ClassMap<FacilityOwner>
    {
        #region Constructors

        public FacilityOwnerMap()
        {
            Id(x => x.Id, "FacilityOwnerID").GeneratedBy.Assigned();

            Map(x => x.Description);

            HasMany(x => x.Facilities).KeyColumn("FacilityOwnerID").LazyLoad();
        }

        #endregion
    }
}
