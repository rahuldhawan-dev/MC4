using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140203113757), Tags("Production")]
    public class CleanUpOrphanedGrievanceAndTailgateTalkEmployeeLinks : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "delete from EmployeeLink where DataTypeID = 20 and not exists (select 1 from UnionGrievances g where g.Id = DataLinkId);");
            Execute.Sql(
                "delete from EmployeeLink where DataTypeID = 81 and not exists (select 1 from tblTailgateTalks t where t.TailgateTalkID = DataLinkId);");
        }

        public override void Down() { }
    }
}
