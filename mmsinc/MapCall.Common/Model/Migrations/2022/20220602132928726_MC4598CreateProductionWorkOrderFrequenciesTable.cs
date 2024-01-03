using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220602132928726), Tags("Production")]
    public class MC4598CreateProductionWorkOrderFrequenciesTable : Migration
    {
        public override void Up()
        {
            Create.Table("ProductionWorkOrderFrequencies")
                  .WithIdentityColumn()
                  .WithColumn("Name").AsAnsiString(50).NotNullable().WithDefaultValue("")
                  .WithColumn("Abbreviation").AsAnsiString(3).Nullable()
                  .WithColumn("Description").AsAnsiString(250).Nullable();

            Delete.Column("Frequency").FromTable("MaintenancePlans");
            Delete.ForeignKeyColumn("MaintenancePlans", "RecurringFrequencyUnitId", "RecurringFrequencyUnits");

            Execute.Sql("INSERT INTO [ProductionWorkOrderFrequencies] ([Name], Abbreviation, [Description]) " +
                        "VALUES " +
                        "   (" + "'Daily', 'D', 'Work order would be generated daily')" +
                        "   ,(" + "'Weekly', 'W', 'Work order would be generated every Sunday')" +
                        "   ,(" + "'Bi-Monthly', 'BM', 'Work order would be generated on the 1st and 15th of every month')" +
                        "   ,(" + "'Monthly', '1M', 'Work order would be generated on the 1st of every month')" +
                        "   ,(" + "'Quarterly', '3M', 'Work order would be generated on the 1st day of each quarter month (Jan, Apr, Jul, and Oct)')" +
                        "   ,(" + "'Every 4 Months', '4M', 'Work order would be generated on the 1st day of these months (Jan, May, Sept)')" +
                        "   ,(" + "'Bi-Annual', '6M', 'Work order would be generated on January 1st and July 1st')" +
                        "   ,(" + "'Annual', '1Y', 'Work order would be generated once a year on Jan 1st')" +
                        "   ,(" + "'Every Two Years', '2Y', 'Work order would be generated every two years on Jan 1st')" +
                        "   ,(" + "'Every Three Years', '3Y', 'Work order would be generated every three years on Jan 1st')" +
                        "   ,(" + "'Every Four Years', '4Y', 'Work order would be generated every four years on Jan 1st')" +
                        "   ,(" + "'Every Five Years', '5Y', 'Work order would be generated every five years on Jan 1st')" +
                        "   ,(" + "'Every Ten Years', '10Y', 'Work order would be generated every ten years on Jan 1st')" +
                        "   ,(" + "'Every Fifteen Years', '15Y', 'Work order would be generated every fifteen years on Jan 1st');");

            Alter.Table("MaintenancePlans")
                 .AddForeignKeyColumn("ProductionWorkOrderFrequencyId", "ProductionWorkOrderFrequencies", nullable: false).WithDefaultValue(1);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("MaintenancePlans", "ProductionWorkOrderFrequencyId", "ProductionWorkOrderFrequencies");
            Delete.Table("ProductionWorkOrderFrequencies");

            Alter.Table("MaintenancePlans")
                 .AddColumn("Frequency").AsInt32().NotNullable().WithDefaultValue(1)
                 .AddForeignKeyColumn("RecurringFrequencyUnitId", "RecurringFrequencyUnits");
        }
    }
}