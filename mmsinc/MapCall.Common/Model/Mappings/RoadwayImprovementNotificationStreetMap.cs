using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class RoadwayImprovementNotificationStreetMap : ClassMap<RoadwayImprovementNotificationStreet>
    {
        public const string TABLE_NAME =
            AddRoadwayNotificationStreetsForBug2596.TableNames.ROADWAY_NOTIFICATION_STREETS;

        public RoadwayImprovementNotificationStreetMap()
        {
            Table(TABLE_NAME);

            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.RoadwayImprovementNotification).Not.Nullable();
            References(x => x.Street).Not.Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.MainSize).Nullable();
            References(x => x.MainType).Nullable();

            // TODO: This is nullable, but it's a required field and every row has a value.
            References(x => x.RoadwayImprovementNotificationStreetStatus).Column("ReviewStatusId").Nullable();

            Map(x => x.StartPoint).Nullable();
            Map(x => x.Terminus).Nullable();
            Map(x => x.MainBreakActivity).Nullable();
            Map(x => x.NumberOfServicesToBeReplaced).Nullable();
            Map(x => x.OpenWorkOrders)
               .Column(AddOpenWorkOrdersToRoadwayImprovementNotificationStreetsForBug2628.COLUMN_NAME)
               .Nullable();
            Map(x => x.MoratoriumEndDate).Nullable();
            Map(x => x.Notes).Length(int.MaxValue).CustomSqlType("ntext").Nullable();
        }
    }
}
