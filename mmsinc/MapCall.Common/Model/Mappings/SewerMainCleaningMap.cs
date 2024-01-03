using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SewerMainCleaningMap : ClassMap<SewerMainCleaning>
    {
        public SewerMainCleaningMap()
        {
            Id(x => x.Id, "SewerMainCleaningID");

            Map(x => x.Date).Nullable();
            Map(x => x.GallonsOfWaterUsed).Nullable();
            Map(x => x.FootageOfMainInspected).Nullable();
            Map(x => x.Opening1Catchbasin, "OpeningCatchbasin1").Nullable();
            Map(x => x.TableNotes, "Notes").Nullable();
            Map(x => x.Overflow).Not.Nullable();
            Map(x => x.Opening2Catchbasin, "OpeningCatchbasin2").Nullable();
            Map(x => x.OverflowOpening1).Nullable();
            Map(x => x.OverflowOpening2).Nullable();
            Map(x => x.SAPErrorCode).Nullable();
            Map(x => x.SAPNotificationNumber).Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.InspectedDate).Nullable();
            Map(x => x.NeedsToSync).Not.Nullable();
            Map(x => x.LastSyncedAt).Nullable();
            Map(x => x.Opening2IsATerminus).Nullable();
            Map(x => x.BlockageFound).Not.Nullable();
            Map(x => x.Month).Formula("month(Date)").ReadOnly();
            Map(x => x.Year).Formula("year(Date)").ReadOnly();

            References(x => x.Street).Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.CrossStreet, "IntersectingStreetId").Nullable();
            References(x => x.Town).Nullable();
            References(x => x.HydrantUsed).Nullable();
            References(x => x.Opening1Condition, "ConditionOfOpening1Id").Nullable();
            References(x => x.Opening1FrameAndCover).Nullable();
            References(x => x.Opening2Condition, "ConditionofOpening2Id").Nullable();
            References(x => x.Opening2FrameAndCover).Nullable();
            References(x => x.CleaningSchedule).Nullable();
            References(x => x.Opening1, "Opening1Id").Nullable();
            References(x => x.Opening2, "Opening2Id").Nullable();
            References(x => x.CrossStreet2, "IntersectingStreet2").Nullable();
            References(x => x.CreatedBy, "CreatedBy").Nullable();
            References(x => x.SewerOverflow).Nullable();
            References(x => x.CauseOfBlockage).Nullable();
            References(x => x.InspectionType).Nullable();
            References(x => x.InspectionGrade).Nullable();

            HasMany(x => x.SewerMainCleaningDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.SewerMainCleaningNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
