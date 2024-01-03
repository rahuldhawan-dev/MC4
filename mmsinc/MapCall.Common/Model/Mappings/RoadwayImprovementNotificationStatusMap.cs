using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class RoadwayImprovementNotificationStatusMap : EntityLookupMap<RoadwayImprovementNotificationStatus>
    {
        public const string TABLE_NAME = "RoadwayImprovementNotificationStatuses";

        public RoadwayImprovementNotificationStatusMap()
        {
            Table(TABLE_NAME);
        }
    }
}
