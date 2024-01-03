using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160517162142960), Tags("Production")]
    public class AlterFieldLengthAPCItemsForBug2961 : Migration
    {
        public override void Up()
        {
            Alter.Column("Description").OnTable("APCInspectionItems").AsAnsiString(255).Nullable();
        }

        public override void Down()
        {
            //Alter.Column("Description").OnTable("APCInspectionItems").AsAnsiString(50).Nullable();
        }
    }
}
