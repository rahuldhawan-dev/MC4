using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150903095025129), Tags("Production")]
    public class RemoveHydrantsPremiseNumberColumnForBug2578 : Migration
    {
        #region Constants

        public const string UPDATE_HYDRANTS_WITH_PREMISE_NUMBER_WHERE_TOWNS_ONLY_HAVE_ONE_FIRE_DISTRICT = @"
UPDATE 
	Hydrants
SET
	FireDistrictID = (SELECT FDT.FireDistrictID FROM FireDistrictsTowns FDT WHERE FDT.TownID = H.Town)
FROM 
	Hydrants H
LEFT JOIN
	FireDistrict FD ON FD.FireDistrictID = H.FireDistrictID
LEFT JOIN
	Towns T ON T.TownID = H.Town
WHERE 
	H.FireDistrictID IS NULL
AND
	(SELECT COUNT(1) FROM FireDistrictsTowns FDT WHERE FDT.TownID = H.Town) = 1";

        #endregion

        public override void Up()
        {
            Delete.Column("PremiseNumber").FromTable("Hydrants");
            Execute.Sql(UPDATE_HYDRANTS_WITH_PREMISE_NUMBER_WHERE_TOWNS_ONLY_HAVE_ONE_FIRE_DISTRICT);
        }

        public override void Down()
        {
            Alter.Table("Hydrants").AddColumn("PremiseNumber").AsAnsiString(10).Nullable();
            Execute.Sql(
                "UPDATE Hydrants SET PremiseNumber = (Select PremiseNumber from FireDistrict FD where FD.FireDistrictID = Hydrants.FireDistrictID)");
        }
    }
}
