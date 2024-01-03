using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MarkoutViolationMap : ClassMap<MarkoutViolation>
    {
        public MarkoutViolationMap()
        {
            Id(x => x.Id);

            Map(x => x.MarkoutViolationStatus).Nullable();
            Map(x => x.Violation).Nullable();
            Map(x => x.DateOfViolationNotice).Nullable();
            Map(x => x.MarkoutRequestNumber).Nullable();
            Map(x => x.OCNumber).Nullable();
            Map(x => x.OperatorOfFacility).Nullable();
            Map(x => x.Location).Nullable();
            Map(x => x.DateOfProbableViolation).Nullable();
            Map(x => x.MarkoutPerformedBy).Nullable();
            Map(x => x.RootCause).Nullable();
            Map(x => x.Contest).Nullable();
            Map(x => x.FineAmount).Nullable();

            // This is a required field, but it's nullable in the database
            // due to a single row with a null value.
            References(x => x.OperatingCenter).Nullable();
            References(x => x.Town).Nullable();
            References(x => x.WorkOrder).Nullable();
            References(x => x.Coordinate).Nullable();

            HasMany(x => x.MarkoutViolationNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.MarkoutViolationDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
