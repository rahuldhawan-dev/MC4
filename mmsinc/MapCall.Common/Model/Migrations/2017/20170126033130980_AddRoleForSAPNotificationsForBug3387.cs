using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170126033130980), Tags("Production")]
    public class AddRoleForSAPNotificationsForBug3387 : Migration
    {
        public const int MODULE_ID = 77;

        public override void Up()
        {
            Execute.Sql(String.Format(@"
DECLARE @applicationId int;
SELECT @applicationId = ApplicationId FROM Applications WHERE Name = 'Field Services';

SET IDENTITY_INSERT [Modules] ON;
INSERT INTO Modules ([ApplicationId], [ModuleId], [Name]) VALUES (@applicationId, {0}, 'SAPNotifications');
SET IDENTITY_INSERT [Modules] OFF;
INSERT INTO Roles VALUES(null, @applicationId, {0}, 1, 376); -- doug
INSERT INTO Roles VALUES(null, @applicationId, {0}, 1, 1650); -- nicole
INSERT INTO Roles VALUES(null, @applicationId, {0}, 1, 2734); -- sachin
INSERT INTO Roles VALUES(null, @applicationId, {0}, 1, 2737); -- apurva
--INSERT INTO Roles VALUES(null, @applicationId, {0}, 1, ); -- deepa
", MODULE_ID));
        }

        public override void Down()
        {
            Execute.Sql(String.Format("DELETE FROM Roles WHERE ModuleId = {0}", MODULE_ID));
            Execute.Sql(String.Format("DELETE FROM Modules WHERE ModuleId = {0}", MODULE_ID));
        }
    }
}
