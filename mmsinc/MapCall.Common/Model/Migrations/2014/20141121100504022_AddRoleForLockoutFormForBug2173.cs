using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141121100504022), Tags("Production")]
    public class AddRoleForLockoutFormForBug2173 : Migration
    {
        public const int MODULE_ID = 72;

        public override void Up()
        {
            Execute.Sql(String.Format(@"
DECLARE @applicationId int;
SELECT @applicationId = ApplicationId FROM Applications WHERE Name = 'Operations';

SET IDENTITY_INSERT [Modules] ON;
INSERT INTO Modules ([ApplicationId], [ModuleId], [Name]) VALUES (@applicationId, {0}, 'LockoutForms');
SET IDENTITY_INSERT [Modules] OFF;", MODULE_ID));
            Execute.Sql(String.Format(
                "UPDATE NotificationPurposes SET ModuleId = m.ModuleId from Modules m WHERE m.ModuleId = {0} AND Purpose LIKE '%Lockout Form'",
                MODULE_ID));
        }

        public override void Down()
        {
            // OperationsHealthAndSafety
            Execute.Sql(String.Format(
                "UPDATE NotificationPurposes SET ModuleId = {0} WHERE ModuleId = {1} AND Purpose LIKE '%Lockout Form';",
                35, MODULE_ID));
            Execute.Sql(String.Format("DELETE FROM Roles WHERE ModuleId = {0}", MODULE_ID));
            Execute.Sql(String.Format("DELETE FROM Modules WHERE ModuleId = {0}", MODULE_ID));
        }
    }
}
