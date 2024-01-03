using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class LocalMap : ClassMap<Local>
    {
        #region Constants

        public const string TABLE_NAME = FixTableAndColumnNamesForBug1623.NewTableNames.LOCALS;

        #endregion

        #region Constructors

        public LocalMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, FixTableAndColumnNamesForBug1623.NewColumnNames.Common.ID);

            Map(x => x.Name)
               .Length(Local.StringLengths.LOCAL).Not.Nullable();
            Map(x => x.Description)
               .Length(Local.StringLengths.DESCRIPTION).Nullable();
            Map(x => x.IsActive).Not.Nullable();
            Map(x => x.SAPUnionDescription).Not.Nullable();

            References(x => x.Union, "BargainingUnitId");

            // Coordinate is nullable, but no nulls exist in the db and it's a required field.
            References(x => x.Coordinate)
               .Nullable();
            References(x => x.OperatingCenter,
                    FixTableAndColumnNamesForBug1623.NewColumnNames.Common.OPERATING_CENTER_ID)
               .Not.Nullable();
            References(x => x.Division).Nullable();
            References(x => x.State).Nullable();

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }

        #endregion
    }
}
