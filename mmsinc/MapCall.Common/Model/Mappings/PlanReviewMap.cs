using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PlanReviewMap : ClassMap<PlanReview>
    {
        public PlanReviewMap()
        {
            Id(x => x.Id);

            Map(x => x.ReviewDate).Not.Nullable();
            Map(x => x.ReviewChangeNotes).Length(PlanReview.StringLengths.REVIEW_CHANGE_NOTES).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.NextReviewDate).Not.Nullable();

            References(x => x.CreatedBy, "CreatedByUserId").Not.Nullable();
            References(x => x.Plan).Not.Nullable();
            References(x => x.ReviewedBy).Not.Nullable();
        }
    }
}
