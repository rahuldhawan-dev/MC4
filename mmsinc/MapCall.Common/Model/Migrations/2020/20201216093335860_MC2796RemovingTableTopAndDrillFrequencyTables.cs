using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201216093335860), Tags("Production")]
    public class MC2796RemovingTableTopAndDrillFrequencyTables : Migration
    {
        public override void Up()
        {
            Delete.Table("TabletopFrequencies");
            Delete.Table("DrillFrequencies");
        }

        public override void Down()
        {
            Create.Table("TabletopFrequencies")
                  .WithColumn("Id").AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn("Description").AsAnsiString(50).NotNullable();
            Create.Table("DrillFrequencies")
                  .WithColumn("Id").AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn("Description").AsAnsiString(50).NotNullable();
        }
    }
}
