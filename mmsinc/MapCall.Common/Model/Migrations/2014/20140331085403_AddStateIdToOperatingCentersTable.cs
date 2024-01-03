using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140331085403), Tags("Production")]
    public class AddStateIdToOperatingCentersTable : Migration
    {
        public override void Up()
        {
            Alter.Table("OperatingCenters")
                 .AddColumn("StateId")
                 .AsInt32()
                 .Nullable()
                 .ForeignKey("FK_OperatingCenters_States_StateId", "States", "stateID");

            Execute.Sql(
                "UPDATE [OperatingCenters] SET [StateId] = (select stateId from [States] where [Abbreviation] = [State])");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_OperatingCenters_States_StateId").OnTable("OperatingCenters");
            Delete.Column("StateId").FromTable("OperatingCenters");
        }
    }
}
