using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190723081547671), Tags("Production")]
    public class MC1497AddRoleModuleForShortCycle : Migration
    {
        public override void Up()
        {
            Execute.Sql("SET IDENTITY_INSERT Modules ON;" +
                        "INSERT INTO Modules(ModuleID, ApplicationID, [Name]) Values(79, 1, 'Short Cycle');" +
                        "SET IDENTITY_INSERT Modules OFF;");
            Execute.Sql(
                "INSERT INTO Roles " +
                "SELECT NULL as OperatingCenterID, 1 as ApplicationID, 79 as ModuleID, 1 as ActionID, RecID as UserId " +
                "FROM tblPermissions " +
                "WHERE Username IN ('jaisons', 'agrawaa', 'ramasav', 'premdav', 'yadavk', 'GARCHAH', 'EMMANEK', 'MODIH', 'ZAMBONM', 'TOMARA')");
        }

        public override void Down()
        {
            Execute.Sql("DELETE FROM Roles WHERE ModuleID = 79;" +
                        "DELETE FROM Modules WHERE ModuleID = 79");
        }
    }
}
