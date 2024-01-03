using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class UnionMap : ClassMap<Union>
    {
        #region Constants

        public const string TABLE_NAME = FixTableAndColumnNamesForBug1623.NewTableNames.BARGAINING_UNITS;

        #endregion

        #region Constructors

        public UnionMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id)
               .GeneratedBy.Identity()
               .Column(FixTableAndColumnNamesForBug1623.NewColumnNames.Common.ID);
            Map(x => x.BargainingUnit)
               .Column(FixTableAndColumnNamesForBug1623.NewColumnNames.Common.NAME)
               .Length(Union.StringLengths.BARGAINING_UNIT)
               .Not.Nullable();
            Map(x => x.Icon).Length(Union.StringLengths.ICON).Nullable();

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }

        #endregion
    }
}
