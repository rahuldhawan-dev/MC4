using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class StandardOperatingProcedureReviewAnswerMap : ClassMap<StandardOperatingProcedureReviewAnswer>
    {
        public StandardOperatingProcedureReviewAnswerMap()
        {
            Id(x => x.Id);

            Map(x => x.Answer).Not.Nullable().Length(int.MaxValue).CustomSqlType("ntext");
            Map(x => x.IsCorrect).Nullable();

            References(x => x.Question, "StandardOperatingProcedureQuestionId")
               .Not.Nullable();
            References(x => x.Review, "StandardOperatingProcedureReviewId")
               .Not.Nullable();
        }
    }
}
