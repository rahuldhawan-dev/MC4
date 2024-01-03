using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140402141055), Tags("Production")]
    public class AddFKForOperatingCenterStateForBug1839 : Migration
    {
        public override void Up()
        {
            Execute.Sql("update OperatingCenters set [State] = LEFT(OperatingCenterCode, 2) where [State] is null");
            Execute.Sql(
                "update OperatingCenters set [StateID] = (select stateID from states S where S.Abbreviation = OperatingCenters.[state]) WHERE StateID is null");
            Alter.Column("StateId").OnTable("OperatingCenters").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Alter.Column("StateId").OnTable("OperatingCenters").AsInt32().Nullable();
        }
    }
}
