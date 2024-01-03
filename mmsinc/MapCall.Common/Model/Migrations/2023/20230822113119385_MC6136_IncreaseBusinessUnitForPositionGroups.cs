using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230822113119385), Tags("Production")]
    public class MC6136_IncreaseBusinessUnitForPositionGroups : Migration
    {
        public override void Up()
        {
            Alter.Column("BusinessUnit").OnTable("PositionGroups").AsString(256).NotNullable();
        }

        public override void Down()
        {
            // Down would break things
        }
    }
}

