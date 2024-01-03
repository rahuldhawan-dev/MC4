using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160718133003856), Tags("Production")]
    public class CleanUpExtraScadaReadingStuffForBug3040 : Migration
    {
        public override void Up()
        {
            Alter.Table("FacilityProcessSteps")
                 .AddForeignKeyColumn("ScadaTagNameId", "ScadaTagNames");

            Execute.Sql(
                $"UPDATE FacilityProcessSteps SET ScadaTagNameId = tn.Id FROM ScadaTagNames tn INNER JOIN ScadaSignals s ON tn.ScadaSignalId = s.Id WHERE SignalId = s.Id");

            Delete.ForeignKeyColumn("ScadaTagNames", "EquipmentId", "Equipment", "EquipmentId");
            Delete.ForeignKeyColumn("ScadaTagNames", "ScadaSignalId", "ScadaSignals");

            Alter.Table("ScadaTagNames").AddColumn("Units").AsString(25).Nullable();
            Alter.Table("ScadaTagNames").AddColumn("Inactive").AsBoolean().WithDefaultValue(false);

            //            Execute.Sql($"UPDATE {"ScadaTagNames"} SET Description = t.Description, Units = t.Engunits FROM OPENQUERY(FACILITY_CONNEX, 'SELECT Tagname, Description, Engunits FROM ihTags') AS t WHERE t.Tagname = {"ScadaTagNames"}.Tagname;");

            Delete.ForeignKeyColumn("Equipment", "ScadaUnitOfMeasureId", "UnitsOfMeasure",
                "UnitOfMeasureId");
        }

        public override void Down()
        {
            Alter.Table("Equipment")
                 .AddForeignKeyColumn("ScadaUnitOfMeasureId", "UnitsOfMeasure", "UnitOfMeasureId");

            Delete.Column("Units").FromTable("ScadaTagNames");
            Delete.Column("Inactive").FromTable("ScadaTagNames");

            Alter.Table("ScadaTagNames")
                 .AddForeignKeyColumn("EquipmentId", "Equipment", "EquipmentId");
            Alter.Table("ScadaTagNames")
                 .AddForeignKeyColumn("ScadaSignalId", "ScadaSignals");

            Delete.ForeignKeyColumn("FacilityProcessSteps", "ScadaTagNameId", "ScadaTagNames");
        }
    }
}
