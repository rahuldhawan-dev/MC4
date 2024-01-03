using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;
using static Humanizer.In;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230322130900001), Tags("Production")]
    public class MC5498_AddEveryTwoMonthsFrequencyAndSortOrderField : Migration
    {
        public override void Up()
        {
            Alter.Table("ProductionWorkOrderFrequencies").AddColumn("SortOrder").AsInt32().NotNullable().WithDefaultValue(999);

            Execute.Sql("UPDATE [ProductionWorkOrderFrequencies] " +
                        "SET SortOrder = 1 WHERE Name = 'Daily'; " +
                        "UPDATE [ProductionWorkOrderFrequencies] " +
                        "SET SortOrder = 2 WHERE Name = 'Weekly'; " +
                        "UPDATE [ProductionWorkOrderFrequencies] " +
                        "SET SortOrder = 3 WHERE Name = 'Twice Per Month'; " +
                        "UPDATE [ProductionWorkOrderFrequencies] " +
                        "SET SortOrder = 4 WHERE Name = 'Monthly'; " +
                        "UPDATE [ProductionWorkOrderFrequencies] " +
                        "SET SortOrder = 6 WHERE Name = 'Quarterly'; " +
                        "UPDATE [ProductionWorkOrderFrequencies] " +
                        "SET SortOrder = 7 WHERE Name = 'Every Four Months'; " +
                        "UPDATE [ProductionWorkOrderFrequencies] " +
                        "SET SortOrder = 8 WHERE Name = 'Every Six Months'; " +
                        "UPDATE [ProductionWorkOrderFrequencies] " +
                        "SET SortOrder = 9 WHERE Name = 'Annual'; " +
                        "UPDATE [ProductionWorkOrderFrequencies] " +
                        "SET SortOrder = 10 WHERE Name = 'Every Two Years'; " +
                        "UPDATE [ProductionWorkOrderFrequencies] " +
                        "SET SortOrder = 11 WHERE Name = 'Every Three Years'; " +
                        "UPDATE [ProductionWorkOrderFrequencies] " +
                        "SET SortOrder = 12 WHERE Name = 'Every Four Years'; " +
                        "UPDATE [ProductionWorkOrderFrequencies] " +
                        "SET SortOrder = 13 WHERE Name = 'Every Five Years'; " +
                        "UPDATE [ProductionWorkOrderFrequencies] " +
                        "SET SortOrder = 14 WHERE Name = 'Every Ten Years'; " +
                        "UPDATE [ProductionWorkOrderFrequencies] " +
                        "SET SortOrder = 15 WHERE Name = 'Every Fifteen Years'; ");

            Insert.IntoTable("ProductionWorkOrderFrequencies").Row(new {
                Name = "Every Two Months",
                Abbreviation = "2M",
                Description = "Work order would be generated on the 1st of every other month (Jan, Mar, May, Jul, Sep, and Nov)",
                SortOrder = 5
            });
        }

        public override void Down()
        {
            Delete.Column("SortOrder").FromTable("ProductionWorkOrderFrequencies");

            Delete.FromTable("ProductionWorkOrderFrequencies").Row(new { Name = "Every Two Months" });
        }
    }
}