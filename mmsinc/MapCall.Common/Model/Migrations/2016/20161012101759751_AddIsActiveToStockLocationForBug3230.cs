using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161012101759751), Tags("Production")]
    public class AddIsActiveToStockLocationForBug3230 : Migration
    {
        public override void Up()
        {
            Alter.Table("StockLocations").AddColumn("IsActive").AsBoolean().WithDefaultValue(false);
            Execute.Sql("update StockLocations SET ISActive = 1");
        }

        public override void Down()
        {
            Delete.Column("IsActive").FromTable("StockLocations");
        }
    }
}
