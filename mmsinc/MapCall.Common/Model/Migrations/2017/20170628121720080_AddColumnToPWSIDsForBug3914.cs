using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170628121720080), Tags("Production")]
    public class AddColumnToPWSIDsForBug3914 : Migration
    {
        public override void Up()
        {
            Alter.Table("PublicWaterSupplies").AddColumn("LIMSProfileNumber").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column("LIMSProfileNumber").FromTable("PublicWaterSupplies");
        }
    }
}
