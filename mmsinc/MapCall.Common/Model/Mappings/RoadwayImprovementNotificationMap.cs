using System;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class RoadwayImprovementNotificationMap : ClassMap<RoadwayImprovementNotification>
    {
        public RoadwayImprovementNotificationMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable();
            Map(x => x.ExpectedProjectStartDate).Not.Nullable();
            Map(x => x.DateReceived).Not.Nullable();
            Map(x => x.PreconMeetingDate).Nullable();

            References(x => x.CreatedBy);
            Map(x => x.CreatedAt).Not.Nullable();

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.Town).Not.Nullable();
            References(x => x.RoadwayImprovementNotificationEntity).Not.Nullable();
            References(x => x.Coordinate).Not.Nullable();
            References(x => x.RoadwayImprovementNotificationStatus).Column("RoadwayImprovementNotificationStatus").Not
                                                                   .Nullable();
            References(x => x.RoadwayImprovementNotificationPreconAction)
               .Column("RoadwayImprovementNotificationPreconActionId")
               .Nullable();

            HasMany(x => x.RoadwayImprovementNotificationDocuments)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.RoadwayImprovementNotificationNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.RoadwayImprovementNotificationStreets)
               .KeyColumn("RoadwayImprovementNotificationId")
               .Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
