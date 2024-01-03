using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200424142805210), Tags("Production")]
    public class AddValueToNoReadReasonsForMC2156 : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("NoReadReasons").Row(new {Description = "Inspect Only"});
        }

        public override void Down()
        {
            Delete.FromTable("NoReadReasons").Row(new {Description = "Inspect Only"});
        }
    }
}
