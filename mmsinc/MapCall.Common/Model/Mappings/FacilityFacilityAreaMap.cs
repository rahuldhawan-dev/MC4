using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityFacilityAreaMap : ClassMap<FacilityFacilityArea>
    {
        #region Constants

        public const string TABLE_NAME = "FacilitiesFacilityAreas";

        #endregion

        #region Constructors

        public FacilityFacilityAreaMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.Facility).Not.Nullable();
            References(x => x.FacilityArea, "FacilityAreaId").Not.Nullable();
            References(x => x.FacilitySubArea, "FacilitySubAreaId").Nullable();
            References(x => x.Coordinate, "CoordinateId").Nullable();
        }

        #endregion
    }
}
