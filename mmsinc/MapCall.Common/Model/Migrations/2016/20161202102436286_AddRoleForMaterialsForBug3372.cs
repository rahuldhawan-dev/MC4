using System;
using FluentMigrator;

namespace MapCall.Common.Model
{
    [Migration(20161202102436286), Tags("Production")]
    public class AddRoleForMaterialsForBug3372 : Migration
    {
        public const int MODULE_ID = 76;

        public override void Up()
        {
            Execute.Sql(String.Format(@"
DECLARE @applicationId int;
SELECT @applicationId = ApplicationId FROM Applications WHERE Name = 'Field Services';

SET IDENTITY_INSERT [Modules] ON;
INSERT INTO Modules ([ApplicationId], [ModuleId], [Name]) VALUES (@applicationId, {0}, 'Materials');
SET IDENTITY_INSERT [Modules] OFF;
INSERT INTO Roles VALUES(null, @applicationId, {0}, 1, 376); -- doug
INSERT INTO Roles VALUES(null, @applicationId, {0}, 1, 1650); -- nicole
", MODULE_ID));
        }

        public override void Down()
        {
            Execute.Sql(String.Format("DELETE FROM Roles WHERE ModuleId = {0}", MODULE_ID));
            Execute.Sql(String.Format("DELETE FROM Modules WHERE ModuleId = {0}", MODULE_ID));
        }
    }
}
