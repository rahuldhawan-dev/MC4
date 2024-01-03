using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    class FacilityStatusMap : ClassMap<FacilityStatus>
    {
        #region Constants

        public const string TABLE_NAME = "FacilityStatuses";

        #endregion

        #region Constructors

        public FacilityStatusMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "FacilityStatusID").GeneratedBy.Assigned();

            Map(x => x.Description);

            HasMany(x => x.Facilities).KeyColumn("FacilityStatusID").LazyLoad();
        }

        #endregion
    }
}
