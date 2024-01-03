using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141024091322426), Tags("Production")]
    public class AddDropDownsForRPProjectsForBug2116 : Migration
    {
        public struct TableNames
        {
            public const string RPPROJECT_REGULATORY_STATUSES = "RPProjectRegulatoryStatuses",
                                RPPROJECT_STATUSES = "RPProjectStatuses",
                                RPPROJECTS = "RPProjects";
        }

        public struct ColumnNames
        {
            public const string RPPROJECT_REGULATORY_STATUS_ID = "RPProjectRegulatoryStatusId";
        }

        public struct Sql
        {
            public const string UPDATE_RPPROJECT_STATUSES = @"
                                    UPDATE 
	                                    RPProjects 
                                    SET 
	                                    RPProjectRegulatoryStatusId = (Select Id from RPProjectRegulatoryStatuses where Description = 'BPU Approved'),
	                                    StatusId = (Select RPProjectStatusId from RPProjectStatuses where Description = 'AP Approved')
                                    WHERE 
	                                    StatusId = (Select RPProjectStatusId from RPProjectStatuses where Description = 'BPU Accepted');
                                    UPDATE 
	                                    RPProjects 
                                    SET 
	                                    RPProjectRegulatoryStatusId = (Select Id from RPProjectRegulatoryStatuses where Description = 'BPU Substituted Out'),
	                                    StatusId = (Select RPProjectStatusId from RPProjectStatuses where Description = 'AP Approved')
                                    WHERE 
	                                    StatusId = (Select RPProjectStatusId from RPProjectStatuses where Description = 'BPU Substituted Out');
                                    INSERT INTO RPProjectStatuses Values('Proposed');",
                                ROLLBACK_RPPROJECT_STATUSES = @"
                                    UPDATE
	                                    RPProjects 
                                    SET 
	                                    StatusId = (Select RPProjectStatusId from RPProjectStatuses where Description = 'BPU Accepted')
                                    WHERE 
	                                    RPProjectRegulatoryStatusId = (Select Id from RPProjectRegulatoryStatuses where Description = 'BPU Approved');
                                    UPDATE
	                                    RPProjects 
                                    SET 
	                                    StatusId = (Select RPProjectStatusId from RPProjectStatuses where Description = 'BPU Substituted Out')
                                    WHERE 
	                                    RPProjectRegulatoryStatusId = (Select Id from RPProjectRegulatoryStatuses where Description = 'BPU Substituted Out');
                                    DELETE FROM RPProjectEndorsements WHERE RPProjectId IN (SELECT RPProjectId FROM RPProjects WHERE StatusId = (SELECT RPProjectStatusId FROM RPProjectStatuses WHERE Description = 'Proposed'));
                                    DELETE FROM RPProjectsHighCostFactors WHERE RPProjectId IN (SELECT RPProjectId FROM RPProjects WHERE StatusId = (SELECT RPProjectStatusId FROM RPProjectStatuses WHERE Description = 'Proposed'));
                                    DELETE FROM RPProjectsPipeDataLookupValues WHERE RPProjectId IN (SELECT RPProjectId FROM RPProjects WHERE StatusId = (SELECT RPProjectStatusId FROM RPProjectStatuses WHERE Description = 'Proposed'));
                                    DELETE FROM RPProjects WHERE StatusId = (SELECT RPProjectStatusId FROM RPProjectStatuses WHERE Description = 'Proposed');
                                    DELETE FROM RPPRojectStatuses WHERE Description = 'Proposed';";
        }

        public override void Up()
        {
            this.CreateLookupTableWithValues(TableNames.RPPROJECT_REGULATORY_STATUSES, "BPU Submitted", "BPU Approved",
                "BPU Substituted In", "BPU Substituted Out");

            Alter.Table(TableNames.RPPROJECTS)
                 .AddForeignKeyColumn(
                      ColumnNames.RPPROJECT_REGULATORY_STATUS_ID,
                      TableNames.RPPROJECT_REGULATORY_STATUSES);

            Execute.Sql(Sql.UPDATE_RPPROJECT_STATUSES);
        }

        public override void Down()
        {
            Execute.Sql(Sql.ROLLBACK_RPPROJECT_STATUSES);

            Delete.ForeignKeyColumn(
                TableNames.RPPROJECTS,
                ColumnNames.RPPROJECT_REGULATORY_STATUS_ID,
                TableNames.RPPROJECT_REGULATORY_STATUSES);

            Delete.Table(TableNames.RPPROJECT_REGULATORY_STATUSES);
        }
    }
}
