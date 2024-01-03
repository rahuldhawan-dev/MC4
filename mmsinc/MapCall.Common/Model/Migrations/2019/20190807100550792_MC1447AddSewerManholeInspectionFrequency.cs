using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190807100550792), Tags("Production")]
    public class AddSewerManholeInspectionFrequency : Migration
    {
        public override void Up()
        {
            Alter.Table("OperatingCenters")
                 .AddColumn("SewerManholeInspectionFrequency").AsInt32().NotNullable().WithDefaultValue(1)
                 .AddColumn("SewerManholeInspectionFrequencyUnitId").AsInt32().NotNullable()
                 .WithDefaultValue(4) // 4 == Year
                 .ForeignKey("FK_OperatingCenters_RecurringFrequencyUnits_SewerManholeInspectionFrequencyUnitId",
                      "RecurringFrequencyUnits", "Id");

            // Fix the incorrectly named foreign key on SewerManholeConnections.
            Execute.Sql(
                "sp_rename 'FK_SewerManholes_RecurringFrequencyUnits_InspectionFrequencyUnitId', 'FK_SewerManholeConnections_RecurringFrequencyUnits_InspectionFrequencyUnitId'");

            Alter.Table("SewerManholes")
                 .AddColumn("InspectionFrequency").AsInt32().Nullable()
                 .AddColumn("InspectionFrequencyUnitId").AsInt32().Nullable()
                 .ForeignKey("FK_SewerManholes_RecurringFrequencyUnits_InspectionFrequencyUnitId",
                      "RecurringFrequencyUnits", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_SewerManholes_RecurringFrequencyUnits_InspectionFrequencyUnitId")
                  .OnTable("SewerManholes");
            Delete.Column("InspectionFrequencyUnitId").FromTable("SewerManholes");
            Delete.Column("InspectionFrequency").FromTable("SewerManholes");

            Execute.Sql(
                "sp_rename 'FK_SewerManholeConnections_RecurringFrequencyUnits_InspectionFrequencyUnitId', 'FK_SewerManholes_RecurringFrequencyUnits_InspectionFrequencyUnitId'");

            Delete.ForeignKey("FK_OperatingCenters_RecurringFrequencyUnits_SewerManholeInspectionFrequencyUnitId")
                  .OnTable("OperatingCenters");
            Delete.Column("SewerManholeInspectionFrequencyUnitId").FromTable("OperatingCenters");
            Delete.Column("SewerManholeInspectionFrequency").FromTable("OperatingCenters");
        }
    }
}
