using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200428113524815), Tags("Production")]
    public class IncreaseLengthOfPositionGroupsBusinessUnitDescriptionForMC2226 : Migration
    {
        public override void Up()
        {
            Alter.Column("BusinessUnitDescription").OnTable("PositionGroups").AsString(50).NotNullable();
        }

        public override void Down() { }
    }
}
