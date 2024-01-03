using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140227134137), Tags("Production")]
    public class ChangeGrievancesOperatingCenterToBeRequiredBug1761 : Migration
    {
        public override void Up()
        {
            // Doug already fixed the one Grievance that didn't have an OperatingCenter set.
            // So we're just gonna set it to NJ7 here.

            Execute.Sql(
                "update [UnionGrievances] set [OperatingCenterId] = (select top 1 [OperatingCenterId] from [OperatingCenters] where [OperatingCenterCode] = 'NJ7') where OperatingCenterId is null");

            Alter.Column("OperatingCenterId")
                 .OnTable("UnionGrievances")
                 .AsInt32()
                 .NotNullable();
        }

        public override void Down()
        {
            Alter.Column("OperatingCenterId")
                 .OnTable("UnionGrievances")
                 .AsInt32()
                 .Nullable();
        }
    }
}
