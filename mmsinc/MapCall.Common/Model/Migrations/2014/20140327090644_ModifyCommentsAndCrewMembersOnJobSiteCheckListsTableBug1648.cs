using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140327090644), Tags("Production")]
    public class ModifyCommentsAndCrewMembersOnJobSiteCheckListsTableBug1648 : Migration
    {
        #region Consts

        public const int CREW_MEMBERS_CREATED_BY_MAX_LENGTH = 50,
                         COMMENTS_CREATED_BY_MAX_LENGTH = 50;

        #endregion

        public override void Up()
        {
            // Create JobSiteCheckListCrewMembers table
            Create.Table("JobSiteCheckListCrewMembers")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("JobSiteCheckListId").AsInt32().NotNullable()
                  .ForeignKey("FK_JobSiteCheckListCrewMembers_JobSiteCheckLists_JobSiteCheckListId",
                       "JobSiteCheckLists", "Id")
                  .WithColumn("CrewMembers").AsCustom("ntext").NotNullable()
                  .WithColumn("CreatedOn").AsDateTime().NotNullable()
                  .WithColumn("CreatedBy")
                  .AsInt32()
                  .ForeignKey("FK_JobSiteCheckListCrewMembers_tblPermissions_CreatedBy", "tblPermissions", "RecID")
                  .NotNullable();

            // Create JobSiteCheckListComments table
            Create.Table("JobSiteCheckListComments")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("JobSiteCheckListId").AsInt32().NotNullable()
                  .ForeignKey("FK_JobSiteCheckListComments_JobSiteCheckLists_JobSiteCheckListId", "JobSiteCheckLists",
                       "Id")
                  .WithColumn("Comments").AsCustom("ntext").NotNullable()
                  .WithColumn("CreatedOn").AsDateTime().NotNullable()
                  .WithColumn("CreatedBy")
                  .AsInt32()
                  .ForeignKey("FK_JobSiteCheckListComments_tblPermissions_CreatedBy", "tblPermissions", "RecID")
                  .NotNullable();

            // Insert new CrewMembers records from existing field

            Execute.Sql(
                @"insert into [JobSiteCheckListCrewMembers] (CrewMembers, JobSiteCheckListId, CreatedBy, CreatedOn)
select 
    CrewMembers,
    Id as JobSiteCheckListId, 
    CreatedBy = (select top 1 RecId from tblPermissions where username = CreatedBy), 
    CreatedOn 
from [JobSiteCheckLists]");

            // Insert new Comments records from existing field

            Execute.Sql(
                @"insert into [JobSiteCheckListComments] (Comments, JobSiteCheckListId, CreatedBy, CreatedOn)
select 
    Comments, 
    Id as JobSiteCheckListId,
    CreatedBy = (select top 1 RecId from tblPermissions where username = CreatedBy), 
    CreatedOn 
from [JobSiteCheckLists]");

            // Drop CrewMember and comments fields from JSCL table

            Delete.Column("Comments").FromTable("JobSiteCheckLists");
            Delete.Column("CrewMembers").FromTable("JobSiteCheckLists");
        }

        public override void Down()
        {
            Alter.Table("JobSiteCheckLists")
                 .AddColumn("Comments").AsCustom("ntext").Nullable()
                 .AddColumn("CrewMembers").AsCustom("ntext").Nullable();

            Execute.Sql(@"
    UPDATE [JobSiteCheckLists] 
	    SET JobSiteCheckLists.Comments = JobSiteCheckListComments.Comments
	    FROM JobSiteCheckLists
	    INNER JOIN 
		    JobSiteCheckListComments
	    ON
		    JobSiteCheckLists.Id = JobSiteCheckListComments.JobSiteCheckListId

		
    UPDATE [JobSiteCheckLists] 
	    SET JobSiteCheckLists.CrewMembers = JobSiteCheckListCrewMembers.CrewMembers
	    FROM JobSiteCheckLists
	    INNER JOIN 
		    JobSiteCheckListCrewMembers
	    ON
		    JobSiteCheckLists.Id = JobSiteCheckListCrewMembers.JobSiteCheckListId
");

            Delete.ForeignKey("FK_JobSiteCheckListCrewMembers_tblPermissions_CreatedBy")
                  .OnTable("JobSiteCheckListCrewMembers");
            Delete.ForeignKey("FK_JobSiteCheckListComments_tblPermissions_CreatedBy")
                  .OnTable("JobSiteCheckListComments");
            Delete.ForeignKey("FK_JobSiteCheckListCrewMembers_JobSiteCheckLists_JobSiteCheckListId")
                  .OnTable("JobSiteCheckListCrewMembers");
            Delete.ForeignKey("FK_JobSiteCheckListComments_JobSiteCheckLists_JobSiteCheckListId")
                  .OnTable("JobSiteCheckListComments");
            Delete.Table("JobSiteCheckListCrewMembers");
            Delete.Table("JobSiteCheckListComments");
        }
    }
}
