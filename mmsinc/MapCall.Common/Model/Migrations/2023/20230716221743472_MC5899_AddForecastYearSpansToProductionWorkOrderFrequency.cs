using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230716221743472), Tags("Production")]
    public class MC5899_AddForecastYearSpansToProductionWorkOrderFrequency : Migration
    {
        public override void Up()
        {
            Alter.Table("ProductionWorkOrderFrequencies")
                 .AddColumn("ForecastYearSpan")
                 .AsInt32()
                 .NotNullable()
                 .SetExistingRowsTo(1); // Most of the frequencies span 1 year

            Execute.Sql("UPDATE ProductionWorkOrderFrequencies SET ForecastYearSpan = 2 WHERE Abbreviation = '2Y'");
            Execute.Sql("UPDATE ProductionWorkOrderFrequencies SET ForecastYearSpan = 3 WHERE Abbreviation = '3Y'");
            Execute.Sql("UPDATE ProductionWorkOrderFrequencies SET ForecastYearSpan = 4 WHERE Abbreviation = '4Y'");
            Execute.Sql("UPDATE ProductionWorkOrderFrequencies SET ForecastYearSpan = 5 WHERE Abbreviation = '5Y'");
            Execute.Sql("UPDATE ProductionWorkOrderFrequencies SET ForecastYearSpan = 10 WHERE Abbreviation = '10Y'");
            Execute.Sql("UPDATE ProductionWorkOrderFrequencies SET ForecastYearSpan = 15 WHERE Abbreviation = '15Y'");
        }

        public override void Down()
        {
            Delete.Column("ForecastYearSpan").FromTable("ProductionWorkOrderFrequencies");
        }
    }
}

