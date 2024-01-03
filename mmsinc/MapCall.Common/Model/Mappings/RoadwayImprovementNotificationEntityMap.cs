using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class RoadwayImprovementNotificationEntityMap : EntityLookupMap<RoadwayImprovementNotificationEntity>
    {
        public const string TABLE_NAME = "RoadwayImprovementNotificationEntities";

        public RoadwayImprovementNotificationEntityMap()
        {
            Table(TABLE_NAME);
        }
    }
}
