using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class StandardOperatingProcedureReviewMap : ClassMap<StandardOperatingProcedureReview>
    {
        public StandardOperatingProcedureReviewMap()
        {
            Id(x => x.Id).Not.Nullable();

            Map(x => x.AnsweredAt).Not.Nullable();
            Map(x => x.ReviewedAt).Nullable(); // Review happens after answering so must be null

            References(x => x.AnsweredBy, "AnsweredByUserId").Not.Nullable();
            References(x => x.ReviewedBy, "ReviewedByUserId").Nullable();
            References(x => x.StandardOperatingProcedure).Not.Nullable();

            HasMany(x => x.Answers).KeyColumn("StandardOperatingProcedureReviewId")
                                   .Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
