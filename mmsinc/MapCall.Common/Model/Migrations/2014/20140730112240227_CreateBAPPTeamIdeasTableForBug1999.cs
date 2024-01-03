using System;
using FluentMigrator;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140730112240227), Tags("Production")]
    public class CreateBAPPTeamIdeasTableForBug1999 : Migration
    {
        public const int APPLICATION_ID = 20, MODULE_ID = 65;

        public struct TableNames
        {
            public const string SAFETY_IMPLEMENTATION_CATEGORIES = "SafetyImplementationCategories",
                                BAPP_TEAM_IDEAS = "BAPPTeamIdeas",
                                BAPP_TEAMS = "BAPPTeams";
        }

        public struct StringLengths
        {
            public struct BappTeams
            {
                public const int DESCRIPTION = 50;
            }

            public struct SafetyImplementationCategories
            {
                public const int DESCRIPTION = 50;
            }
        }

        public override void Up()
        {
            Execute.Sql(String.Format(@"
SET IDENTITY_INSERT [Applications] ON;
INSERT INTO Applications ([ApplicationId], [Name]) VALUES ({0}, 'BAPP Team Sharing');
SET IDENTITY_INSERT [Applications] OFF;

SET IDENTITY_INSERT [Modules] ON;
INSERT INTO Modules ([ApplicationId], [ModuleId], [Name]) VALUES ({0}, {1}, 'General');
SET IDENTITY_INSERT [Modules] OFF;", APPLICATION_ID, MODULE_ID));

            Create.Table(TableNames.BAPP_TEAMS)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("Description").AsString(StringLengths.BappTeams.DESCRIPTION).NotNullable();

            Create.Table(TableNames.SAFETY_IMPLEMENTATION_CATEGORIES)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("Description").AsString(StringLengths.SafetyImplementationCategories.DESCRIPTION)
                  .NotNullable();

            Create.Table(TableNames.BAPP_TEAM_IDEAS)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("BappTeamId").AsInt32()
                  .ForeignKey(String.Format("FK_{0}_{1}_BappTeamId", TableNames.BAPP_TEAM_IDEAS, TableNames.BAPP_TEAMS),
                       TableNames.BAPP_TEAMS, "Id").NotNullable()
                  .WithColumn("OperatingCenterId").AsInt32()
                  .ForeignKey(String.Format("FK_{0}_OperatingCenters_OperatingCenterId", TableNames.BAPP_TEAM_IDEAS),
                       "OperatingCenters", "OperatingCenterId").NotNullable()
                  .WithColumn("ContactId").AsInt32()
                  .ForeignKey(String.Format("FK_{0}_tblPermissions_ContactId", TableNames.BAPP_TEAM_IDEAS),
                       "tblPermissions", "RecId").NotNullable()
                  .WithColumn("SafetyImplementationCategoryId").AsInt32().ForeignKey(
                       String.Format("FK_{0}_{1}_SafetyImplementationCategoryId", TableNames.BAPP_TEAM_IDEAS,
                           TableNames.SAFETY_IMPLEMENTATION_CATEGORIES), TableNames.SAFETY_IMPLEMENTATION_CATEGORIES,
                       "Id").NotNullable()
                  .WithColumn("Description").AsCustom("text");
        }

        public override void Down()
        {
            this.DeleteApplication("BAPP Team Sharing");

            Delete.Table(TableNames.BAPP_TEAM_IDEAS);
            Delete.Table(TableNames.SAFETY_IMPLEMENTATION_CATEGORIES);
            Delete.Table(TableNames.BAPP_TEAMS);
        }
    }
}
