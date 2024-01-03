using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141030150653501), Tags("Production")]
    public class MakeGrievanceStatusesFlyForBug2168 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
UPDATE UnionGrievances SET StatusId = newS.Id FROM GrievanceStatuses newS WHERE EXISTS (SELECT 1 FROM GrievanceStatuses s WHERE s.Description LIKE 'Active_Step %' AND s.Id = UnionGrievances.StatusId) AND newS.Description = 'Active';
UPDATE UnionGrievances SET StatusId = newS.Id FROM GrievanceStatuses newS WHERE EXISTS (SELECT 1 FROM GrievanceStatuses s WHERE s.Description = 'Non Grievable-Returned' AND s.Id = UnionGrievances.StatusId) AND newS. Description = 'Non-Grievable Returned';
DELETE FROM GrievanceStatuses WHERE Description LIKE 'Active_Step %' OR Description = 'Non Grievable-Returned';");
        }

        public override void Down() { }
    }
}
