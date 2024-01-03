using System;
using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140725160141932), Tags("Production")]
    public class AddRoleForBug1986 : Migration
    {
        public const int APPLICATION_ID = 19, MODULE_ID = 64;

        public override void Up()
        {
            Execute.Sql(String.Format(@"
SET IDENTITY_INSERT [Applications] ON;
INSERT INTO Applications ([ApplicationId], [Name]) VALUES ({0}, 'General');
SET IDENTITY_INSERT [Applications] OFF;

SET IDENTITY_INSERT [Modules] ON;
INSERT INTO Modules ([ApplicationId], [ModuleId], [Name]) VALUES ({0}, {1}, 'Towns');
SET IDENTITY_INSERT [Modules] OFF;", APPLICATION_ID, MODULE_ID));
        }

        public override void Down()
        {
            this.DeleteApplication("General");
        }
    }
}
