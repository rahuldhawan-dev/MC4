using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130218092615), Tags("Production")]
    public class AddMatererialsForEW2 : Migration
    {
        #region Constants

        public struct Sql
        {
            public const string
                INSERT_OPERATING_CENTER_MATERIALS_FORMAT =
                    @"INSERT INTO OperatingCenterStockedMaterials(OperatingCenterID, MaterialID) 
                        SELECT (SELECT OperatingCenterID from OperatingCenters where OperatingCenterCode = '{0}'), (SELECT TOP 1 MaterialID FROM Materials WHERE PartNumber = '{1}')",
                DELETE_OPERATING_CENTER_MATERIALS_FORMAT =
                    @"DELETE FROM OperatingCenterStockedMaterials 
                        WHERE OperatingCenterID  = (SELECT OperatingCenterID from OperatingCenters where OperatingCenterCode = '{0}')
                        AND MaterialID = (SELECT TOP 1 MaterialID FROM Materials WHERE PartNumber = '{1}')";
        }

        #endregion

        public override void Up()
        {
            Execute.Sql(string.Format(Sql.INSERT_OPERATING_CENTER_MATERIALS_FORMAT, "EW2", "1405538"));
            Execute.Sql(string.Format(Sql.INSERT_OPERATING_CENTER_MATERIALS_FORMAT, "EW2", "1405511"));
            Execute.Sql(string.Format(Sql.INSERT_OPERATING_CENTER_MATERIALS_FORMAT, "EW2", "1405512"));
            Execute.Sql(string.Format(Sql.INSERT_OPERATING_CENTER_MATERIALS_FORMAT, "EW2", "1405485"));
            Execute.Sql(string.Format(Sql.INSERT_OPERATING_CENTER_MATERIALS_FORMAT, "EW2", "1405235"));
        }

        public override void Down()
        {
            Execute.Sql(string.Format(Sql.DELETE_OPERATING_CENTER_MATERIALS_FORMAT, "EW2", "1405538"));
            Execute.Sql(string.Format(Sql.DELETE_OPERATING_CENTER_MATERIALS_FORMAT, "EW2", "1405511"));
            Execute.Sql(string.Format(Sql.DELETE_OPERATING_CENTER_MATERIALS_FORMAT, "EW2", "1405512"));
            Execute.Sql(string.Format(Sql.DELETE_OPERATING_CENTER_MATERIALS_FORMAT, "EW2", "1405485"));
            Execute.Sql(string.Format(Sql.DELETE_OPERATING_CENTER_MATERIALS_FORMAT, "EW2", "1405235"));
        }
    }
}
