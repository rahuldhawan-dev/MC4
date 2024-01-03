using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200325111921048), Tags("Production")]
    public class UpdateFacilityEquipmentRolesForMC1890 : Migration
    {
        public override void Up()
        {
            // Update HumanResourcesFacilities to be ProductionFacilities
            Execute.Sql("UPDATE Modules SET ApplicationID = 2 WHERE ModuleID = 29;");
            Execute.Sql("UPDATE Roles SET ApplicationID = 2 WHERE ModuleID = 29 AND ApplicationID = 3;");
            // Add the new Production - Equipment Role - 82
            Execute.Sql("SET IDENTITY_INSERT Modules ON;" +
                        "INSERT INTO Modules(ModuleID, ApplicationID, [Name]) Values(82, 2, 'Equipment');" +
                        "SET IDENTITY_INSERT Modules OFF;");
            // Add all the users with Production - Facility to Production - Equipment with the same roles they have
            Execute.Sql(
                "INSERT INTO ROLES SELECT OperatingCenterID, ApplicationID, 82, ActionID, UserId FROM ROLES WHERE ModuleID = 29 and ApplicationID = 2");

            // Lets give everyone with Production/Facility Add/Edit/Delete/UserAdmin that doesn't already have a Read role that
            // read role because we're going to remove their Add/Edit/Delete/UserAdmin atfer this statement
            Execute.Sql(@"
                INSERT INTO 
                    Roles(OperatingCenterID, ApplicationID, ModuleID, ActionID, UserId)
                SELECT 
                    Distinct R.OperatingCenterID, R.ApplicationID, R.ModuleID, 2, R.UserId
                FROM
                    Roles R
                JOIN 
                    tblPermissions P on P.RecID = R.UserId
                LEFT JOIN    
                    Roles RR 
                on
                    IsNull(RR.OperatingCenterID, '') = IsNull(R.OperatingCenterID,'') AND RR.ApplicationID = R.ApplicationID AND RR.ModuleID = R.ModuleID AND R.UserId = RR.UserId AND RR.ActionID = 2
                WHERE  
                    R.ApplicationID = 2
                AND 
                    R.ModuleId = 29
                AND 
                    R.ActionID in (1, 3, 4, 5)
                AND
                    RR.RoleID is null
                AND
                    P.IsUserAdministrator = 0");
            // Now lets remove the Add/Edit/Delete/UserAdmin roles
            Execute.Sql(@"
                DELETE
                    R
                FROM
                    Roles R
                JOIN
                    tblPermissions P on P.RecID = R.UserId
                WHERE   
                    ApplicationID = 2
                AND 
                    ModuleId = 29
                AND 
                    ActionID in (1, 3, 4, 5)
                AND
                    IsUserAdministrator = 0
                ");
        }

        public override void Down()
        {
            //Remove new module and roles/users linked to it
            Execute.Sql("DELETE FROM Roles WHERE ModuleID = 82;" +
                        "DELETE FROM Modules WHERE ModuleID = 82");
            //Move Production - Facilities back to HumanResources - Facilities
            Execute.Sql("UPDATE Modules SET ApplicationID = 3 WHERE ModuleID = 29;");
            Execute.Sql("UPDATE Roles SET ApplicationID = 3 WHERE ModuleID = 29 AND ApplicationID = 2;");
        }
    }
}
