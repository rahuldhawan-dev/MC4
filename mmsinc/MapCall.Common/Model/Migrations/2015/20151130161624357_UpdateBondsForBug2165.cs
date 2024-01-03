using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151130161624357), Tags("Production")]
    public class UpdateBondsForBug2165 : Migration
    {
        public override void Up()
        {
            Alter.Table("Bonds").AddForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId");
            Execute.Sql(@"update bonds set operatingCenterID = 14 where countyId = 15 and TOwnId Is null;
update 
	bonds 
set 
	OperatingCenterId = (select top 1 OperatingCenterId from OperatingCentersTowns oct where oct.TownId = T.TownID)
from
	bonds B 
join
	Towns T on T.TownId = B.TownID 
where 
	T.TownID is not null");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Bonds", "OperatingCenterId", "OperatingCenters", "OperatingCenterId");
        }
    }
}
