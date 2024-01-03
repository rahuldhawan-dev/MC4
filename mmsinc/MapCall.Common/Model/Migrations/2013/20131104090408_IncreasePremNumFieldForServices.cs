using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131104090408), Tags("Production")]
    public class IncreasePremNumFieldForServices : Migration
    {
        public override void Up()
        {
            Alter.Table("tblNJAWService").AlterColumn("PremNum").AsAnsiString(10).Nullable();
            Alter.Table("tblNJAWHydrant").AlterColumn("PremiseNumber").AsAnsiString(10).Nullable();
        }

        public override void Down()
        {
            //Alter.Table("tblNJAWService").AlterColumn("PremNum").AsAnsiString(9).Nullable();
        }
    }
}
