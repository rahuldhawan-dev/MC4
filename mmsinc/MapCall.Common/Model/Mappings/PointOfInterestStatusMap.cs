using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class PointOfInterestStatusMap : EntityLookupMap<PointOfInterestStatus>
    {
        public const string TABLE_NAME = "PointOfInterestStatuses";

        public PointOfInterestStatusMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
