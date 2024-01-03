using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160414164524359), Tags("Production")]
    public class AddFieldToMainCrossingsForBug2875 : Migration
    {
        public override void Up()
        {
            Alter.Table("MainCrossings").AddColumn("MainInCasing").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("MainInCasing").FromTable("MainCrossings");
        }
    }
}
