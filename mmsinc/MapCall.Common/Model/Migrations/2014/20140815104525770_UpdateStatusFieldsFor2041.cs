using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140815104525770), Tags("Production")]
    public class UpdateStatusFieldsFor2041 : Migration
    {
        public const string TABLE_NAME = "RPProjects";

        public const int
            LOCAL_APPROVAL = 66,
            ASSET_PLANNING_APPROVAL = 67,
            ASSET_PLANNING_ENDORSEMENT = 68,
            CAPITAL_PLANNING = 69;

        public struct Sql
        {
            public const string
                UPDATE_STATUSES = @"
                    UPDATE [RPProjects] SET [StatusID] = (SELECT [RPProjectStatusID] FROM [RPProjectStatuses] WHERE [Description] = 'Submitted') WHERE IsNull([StatusID],0) = 0 
                    UPDATE [RPProjectStatuses] SET [Description] = 'AP Approved' WHERE [Description] = 'Approved'
                    UPDATE [RPProjects] SET [StatusID] = (SELECT [RPProjectStatusID] FROM [RPProjectStatuses] WHERE [Description] = 'Complete') WHERE isNull([StatusID], 0) <> 3 AND [ActualInServiceDate] IS NOT NULL
                    DBCC CheckIdent (RPProjectStatuses, reseed, 9)                    
                    INSERT INTO [RPProjectStatuses] VALUES('BPU Accepted')
                    INSERT INTO [RPProjectStatuses] VALUES('Manager Endorsed')
                    INSERT INTO [RPProjectStatuses] VALUES('AP Endorsed')
					INSERT INTO [RPProjectStatuses] VALUES('Municipal Relocation Approved')
                    UPDATE [RPProjects] SET [StatusID] = (SELECT [RPProjectStatusID] FROM [RPProjectStatuses] WHERE [Description] = 'BPU Accepted') WHERE [BPUAccepted] in (1,2) AND StatusID = 1
                    UPDATE [RPProjectStatuses] SET [Description] = 'BPU Substituted Out' WHERE [Description] = 'Deferred'
                    UPDATE [RPProjects] SET [StatusID] = (SELECT [RPProjectStatusID] FROM [RPProjectStatuses] WHERE [Description] = 'BPU Substituted Out') WHERE [BPUSubstitutedOut] = 1
                    UPDATE [RPProjects] SET [StatusID] = (SELECT [RPProjectStatusID] FROM [RPProjectStatuses] WHERE [Description] = 'Canceled') WHERE StatusID = (SELECT [RPProjectStatusID] FROM [RPProjectStatuses] WHERE [Description] = 'Duplicate')
                    DELETE FROM [RPProjectStatuses] WHERE [Description] = 'Duplicate'",
                ROLLBACK_STATUSES = @"
	                UPDATE [RPProjects] SET [BPUAccepted] = 1 WHERE [StatusID] = (SELECT [RPPRojectStatusID] FROM [RPProjectStatuses] WHERE [Description] = 'BPU Accepted')
	                UPDATE [RPProjects] SET [BPUSubstitutedOut] = 1 WHERE [StatusID] = (SELECT [RPPRojectStatusID] FROM [RPProjectStatuses] WHERE [Description] = 'BPU Substituted Out')
	                UPDATE [RPProjectStatuses] SET [Description] = 'Deferred' WHERE [Description] = 'BPU Substituted Out'
	                UPDATE [RPProjectStatuses] SET [Description] = 'Approved' WHERE [Description] = 'AP Approved'
	                UPDATE [RPProjects] SET StatusID = (SELECT [RPProjectStatusID] FROM [RPProjectStatuses] WHERE [Description] = 'Submitted') WHERE [StatusID] = (SELECT [RPPRojectStatusID] FROM [RPProjectStatuses] WHERE [Description] = 'BPU Accepted')
                    DELETE FROM RPProjectEndorsements WHERE RPProjectId IN (SELECT RPProjectId FROM RPProjects WHERE StatusId IN (SELECT RPProjectStatusId FROM RPProjectStatuses WHERE [Description] in ('BPU Accepted', 'Municipal Relocation Approved', 'Manager Endorsed', 'AP Endorsed')));
                    DELETE FROM RPProjectsHighCostFactors WHERE RPProjectId IN (SELECT RPProjectId FROM RPProjects WHERE StatusId IN (SELECT RPProjectStatusId FROM RPProjectStatuses WHERE [Description] in ('BPU Accepted', 'Municipal Relocation Approved', 'Manager Endorsed', 'AP Endorsed')));
                    DELETE FROM RPProjectsPipeDataLookupValues WHERE RPProjectId IN (SELECT RPProjectId FROM RPProjects WHERE StatusId IN (SELECT RPProjectStatusId FROM RPProjectStatuses WHERE [Description] in ('BPU Accepted', 'Municipal Relocation Approved', 'Manager Endorsed', 'AP Endorsed')));
                    DELETE FROM [RPProjects] WHERE StatusId IN (SELECT RPProjectStatusId FROM [RPProjectStatuses] WHERE [Description] in ('BPU Accepted', 'Municipal Relocation Approved', 'Manager Endorsed', 'AP Endorsed'))
                    DELETE FROM [RPProjectStatuses] WHERE [Description] in ('BPU Accepted', 'Municipal Relocation Approved', 'Manager Endorsed', 'AP Endorsed')
                    SET IDENTITY_INSERT RPProjectStatuses ON
                    INSERT INTO RPProjectStatuses(RPProjectStatusID, Description) Values(9,'Duplicate')
                    SET IDENTITY_INSERT RPProjectStatuses OFF",
                INSERT_ROLES = @"
                    declare @applicationId int;
                    set @applicationId = (select top 1 ApplicationId from Applications where Name = 'Field Services')
                    set identity_insert Modules on
                        insert into Modules (ModuleId, ApplicationId, Name) values ({0}, @applicationId, 'Local Approval')
                        insert into Modules (ModuleId, ApplicationId, Name) values ({1}, @applicationId, 'Asset Planning Approval')
                        insert into Modules (ModuleId, ApplicationId, Name) values ({2}, @applicationId, 'Asset Planning Endorsement')
                        insert into Modules (ModuleId, ApplicationId, Name) values ({3}, @applicationId, 'Capital Planning')
                    set identity_insert Modules off",
                DELETE_ROLES = @"
                    DELETE Roles where ModuleId = 66
                    DELETE Modules where ModuleId = 66
                    DELETE Roles where ModuleId = 67
                    DELETE Modules where ModuleId = 67
                    DELETE Roles where ModuleId = 68
                    DELETE Modules where ModuleId = 68
                    DELETE Roles where ModuleId = 69
                    DELETE Modules where ModuleId = 69";
        }

        public override void Up()
        {
            Execute.Sql(String.Format(Sql.INSERT_ROLES, LOCAL_APPROVAL, ASSET_PLANNING_APPROVAL,
                ASSET_PLANNING_ENDORSEMENT, CAPITAL_PLANNING));
            Execute.Sql(Sql.UPDATE_STATUSES);
            Delete.Column("BPUAccepted").FromTable(TABLE_NAME);
            Delete.Column("BPUSubstitutedOut").FromTable(TABLE_NAME);
        }

        public override void Down()
        {
            Alter.Table(TABLE_NAME)
                 .AddColumn("BPUAccepted").AsBoolean().Nullable()
                 .AddColumn("BPUSubstitutedOut").AsBoolean().Nullable();
            Execute.Sql(Sql.ROLLBACK_STATUSES);
            Execute.Sql(Sql.DELETE_ROLES);
        }
    }
}
