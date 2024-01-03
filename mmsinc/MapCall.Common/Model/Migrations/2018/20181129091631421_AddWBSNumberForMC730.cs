using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181129091631421), Tags("Production")]
    public class AddWBSNumberForMC730 : Migration
    {
        public override void Up()
        {
            Alter.Table("Equipment")
                 .AddColumn("WBSNumber")
                 .AsString(18)
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("WBSNumber").FromTable("Equipment");
        }
    }
}
