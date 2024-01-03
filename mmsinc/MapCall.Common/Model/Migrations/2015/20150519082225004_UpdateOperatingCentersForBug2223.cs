using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150519082225004), Tags("Production")]
    public class UpdateOperatingCentersForBug222 : Migration
    {
        private const string OPERATING_CENTERS = "OperatingCenters";

        public override void Up()
        {
            Alter.Table(OPERATING_CENTERS)
                 .AddColumn("HydrantInspectionFrequency").AsInt32().NotNullable().WithDefaultValue(1)
                 .AddColumn("HydrantInspectionFrequencyUnitId").AsInt32().Nullable()
                 .ForeignKey("FK_OperatingCenters_RecurringFrequencyUnits_HydrantInspectionFrequencyUnitId",
                      "RecurringFrequencyUnits", "Id")
                 .AddColumn("LargeValveInspectionFrequency").AsInt32().NotNullable().WithDefaultValue(1)
                 .AddColumn("LargeValveInspectionFrequencyUnitId").AsInt32().Nullable()
                 .ForeignKey("FK_OperatingCenters_RecurringFrequencyUnits_LargeValveInspectionFrequencyUnitId",
                      "RecurringFrequencyUnits", "Id")
                 .AddColumn("SmallValveInspectionFrequency").AsInt32().NotNullable().WithDefaultValue(1)
                 .AddColumn("SmallValveInspectionFrequencyUnitId").AsInt32().Nullable()
                 .ForeignKey("FK_OperatingCenters_RecurringFrequencyUnits_SmallValveInspectionFrequencyUnitId",
                      "RecurringFrequencyUnits", "Id");

            Execute.Sql(
                "UPDATE [OperatingCenters] SET [HydrantInspectionFrequency] = [HydInspFreq] WHERE [HydInspFreq] is not null");
            Execute.Sql(
                "UPDATE [OperatingCenters] SET [LargeValveInspectionFrequency] = [ValLgInspFreq] WHERE [ValLgInspFreq] is not null");
            Execute.Sql(
                "UPDATE [OperatingCenters] SET [SmallValveInspectionFrequency] = [ValSmInspFreq] WHERE [ValSmInspFreq] is not null");

            // All of these are Y except for three bad rows with D, so they're all gonna be 1 Year.
            Execute.Sql(
                "UPDATE [OperatingCenters] SET [HydrantInspectionFrequencyUnitId] = (select top 1 Id from [RecurringFrequencyUnits] where [Description] = 'Year')");
            Execute.Sql(
                "UPDATE [OperatingCenters] SET [LargeValveInspectionFrequencyUnitId] = (select top 1 Id from [RecurringFrequencyUnits] where [Description] = 'Year')");
            Execute.Sql(
                "UPDATE [OperatingCenters] SET [SmallValveInspectionFrequencyUnitId] = (select top 1 Id from [RecurringFrequencyUnits] where [Description] = 'Year')");

            // Get rid of nullability
            Alter.Column("HydrantInspectionFrequencyUnitId").OnTable(OPERATING_CENTERS).AsInt32().NotNullable();
            Alter.Column("LargeValveInspectionFrequencyUnitId").OnTable(OPERATING_CENTERS).AsInt32().NotNullable();
            Alter.Column("SmallValveInspectionFrequencyUnitId").OnTable(OPERATING_CENTERS).AsInt32().NotNullable();

            Delete.Column("HydInspFreq").FromTable(OPERATING_CENTERS);
            Delete.Column("HydInspFreqUnit").FromTable(OPERATING_CENTERS);
            Delete.Column("ValLgInspFreq").FromTable(OPERATING_CENTERS);
            Delete.Column("ValLgInspFreqUnit").FromTable(OPERATING_CENTERS);
            Delete.Column("ValSmInspFreq").FromTable(OPERATING_CENTERS);
            Delete.Column("ValSmInspFreqUnit").FromTable(OPERATING_CENTERS);
        }

        public override void Down()
        {
            Alter.Table(OPERATING_CENTERS)
                 .AddColumn("HydInspFreq").AsString(10).Nullable()
                 .AddColumn("HydInspFreqUnit").AsString(50).Nullable()
                 .AddColumn("ValLgInspFreq").AsString(10).Nullable()
                 .AddColumn("ValLgInspFreqUnit").AsString(50).Nullable()
                 .AddColumn("ValSmInspFreq").AsString(10).Nullable()
                 .AddColumn("ValSmInspFreqUnit").AsString(50).Nullable();

            Execute.Sql("UPDATE [OperatingCenters] SET [HydInspFreq] = [HydrantInspectionFrequency]");
            Execute.Sql("UPDATE [OperatingCenters] SET [ValLgInspFreq] = [LargeValveInspectionFrequency]");
            Execute.Sql("UPDATE [OperatingCenters] SET [ValSmInspFreq] = [SmallValveInspectionFrequency]");
            Execute.Sql(
                "UPDATE [OperatingCenters] SET [HydInspFreqUnit] = 'Y', [ValLgInspFreqUnit] = 'Y', [ValSmInspFreqUnit] = 'Y'"); // These are all Year so.

            Delete.ForeignKey("FK_OperatingCenters_RecurringFrequencyUnits_HydrantInspectionFrequencyUnitId")
                  .OnTable(OPERATING_CENTERS);
            Delete.ForeignKey("FK_OperatingCenters_RecurringFrequencyUnits_LargeValveInspectionFrequencyUnitId")
                  .OnTable(OPERATING_CENTERS);
            Delete.ForeignKey("FK_OperatingCenters_RecurringFrequencyUnits_SmallValveInspectionFrequencyUnitId")
                  .OnTable(OPERATING_CENTERS);
            Delete.Column("HydrantInspectionFrequency").FromTable(OPERATING_CENTERS);
            Delete.Column("HydrantInspectionFrequencyUnitId").FromTable(OPERATING_CENTERS);
            Delete.Column("LargeValveInspectionFrequency").FromTable(OPERATING_CENTERS);
            Delete.Column("LargeValveInspectionFrequencyUnitId").FromTable(OPERATING_CENTERS);
            Delete.Column("SmallValveInspectionFrequency").FromTable(OPERATING_CENTERS);
            Delete.Column("SmallValveInspectionFrequencyUnitId").FromTable(OPERATING_CENTERS);
        }
    }
}
