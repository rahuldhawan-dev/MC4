using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131114082143), Tags("Production")]
    public class AlterPremiseNumberForWorkOrders : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkOrders").AlterColumn("PremiseNumber").AsAnsiString(10).Nullable();
        }

        public override void Down()
        {
            //Alter.Table("WorkOrders").AlterColumn("PremiseNumber").AsAnsiString(9).Nullable();
        }
    }
}
