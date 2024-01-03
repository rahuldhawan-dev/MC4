using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class
        RoadwayImprovementNotificationStreetStatusMap : EntityLookupMap<RoadwayImprovementNotificationStreetStatus>
    {
        public const string TABLE_NAME = "RoadwayImprovementNotificationStreetStatuses";

        public RoadwayImprovementNotificationStreetStatusMap()
        {
            Table(TABLE_NAME);
        }
    }
}
