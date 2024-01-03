using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ProcessMap : ClassMap<Process>
    {
        #region Constants

        public const string TABLE_NAME = "Processes";

        #endregion

        #region Constructors

        public ProcessMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id);

            Map(x => x.Description)
               .Length(Process.MAX_DESCRIPTION_LENGTH)
               .Not.Nullable()
               .Unique();
            Map(x => x.Sequence)
               .Not.Nullable().Precision(4).Scale(2);
            Map(x => x.ProcessOverview)
               .Length(int.MaxValue).CustomSqlType("text")
               .Nullable();

            References(x => x.ProcessStage)
               .Not.Nullable();

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }

        #endregion
    }
}
