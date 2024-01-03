using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201207155722666), Tags("Production")]
    public class CreateNewApplicationIDAndRole : Migration
    {
        public const int APPLICATION_ID = 23,
                         MODULE_ID = 90;
        public const string MODULE = "'EAM Asset Management'",
                            APPLICATION = "'Engineering'";

        public override void Up()
        {
            Execute.Sql(string.Format(@"
            SET IDENTITY_INSERT [Applications] ON;
            INSERT INTO Applications ([ApplicationId], [Name]) VALUES ({0}, {3});
            SET IDENTITY_INSERT [Applications] OFF;

            SET IDENTITY_INSERT [Modules] ON;
            INSERT INTO Modules ([ApplicationId], [ModuleId], [Name]) VALUES ({0}, {1}, {2});
            SET IDENTITY_INSERT [Modules] OFF;", APPLICATION_ID, MODULE_ID, MODULE, APPLICATION));
        }

        public override void Down()
        {
            Delete.FromTable("Modules").Row(new {moduleApplicationID = APPLICATION_ID, Name = MODULE});
            this.DeleteApplication(APPLICATION);
        }
    }
}