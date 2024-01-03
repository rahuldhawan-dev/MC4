using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class StandardOperatingProcedureQuestionMap : ClassMap<StandardOperatingProcedureQuestion>
    {
        public StandardOperatingProcedureQuestionMap()
        {
            Id(x => x.Id);

            Map(x => x.IsActive).Not.Nullable();
            Map(x => x.Answer).Not.Nullable().Length(int.MaxValue).CustomSqlType("ntext");
            Map(x => x.Question).Not.Nullable().Length(int.MaxValue).CustomSqlType("ntext");

            References(x => x.StandardOperatingProcedure)
               .Not.Nullable();
        }
    }
}
