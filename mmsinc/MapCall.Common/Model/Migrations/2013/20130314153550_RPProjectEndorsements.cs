using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130314153550), Tags("Production")]
    public class RPProjectEndorsements : Migration
    {
        public override void Up()
        {
            Rename.Column("ProjectYear").OnTable("RPProjects").To("OriginationYear");
            Rename.Column("InServiceDate").OnTable("RPProjects").To("ActualInServiceDate");
            Rename.Column("Approved").OnTable("RPProjects").To("BPUApproved");
            Delete.Column("EstimatedInServicePeriod").FromTable("RPProjects");
            Alter.Column("NJAWEstimate").OnTable("RPProjects").AsInt32();

            //NOT ALLOWED
            //Insert.IntoTable("ProjectTypes").Row(new { Description = "New", CreatedBy = "migration" });
            //Insert.IntoTable("RPProjectStatuses").Row(new { Description = "Canceled" });
            Create.Table("RPProjectsHighCostFactors")
                  .WithColumn("RPProjectID").AsInt32().NotNullable()
                  .WithColumn("HighCostFactorID").AsInt32().NotNullable();
            Create.ForeignKey("FK_RPProjectsHighCostFactors_HighCostFactors_HighCostFactorID")
                  .FromTable("RPProjectsHighCostFactors")
                  .ForeignColumn("HighCostFactorID")
                  .ToTable("HighCostFactors")
                  .PrimaryColumn("HighCostFactorID");
            Create.ForeignKey("FK_RPProjectsHighCostFactors_RPProjects_RPProjectID")
                  .FromTable("RPProjectsHighCostFactors")
                  .ForeignColumn("RPProjectID")
                  .ToTable("RPProjects")
                  .PrimaryColumn("RPProjectID");
            Execute.Sql("GRANT ALL ON RPProjectsHighCostFactors TO MCUser");

            Create.Table("EndorsementStatuses")
                  .WithColumn("EndorsementStatusID").AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn("Description").AsAnsiString(50).NotNullable();
            Execute.Sql("GRANT ALL ON EndorsementStatuses TO MCUser");

            //NOT ALLOWED
            //Insert.IntoTable("EndorsementStatuses").Row(new { Description = "Endorsed" });
            //Insert.IntoTable("EndorsementStatuses").Row(new { Description = "Not Endorsed" });
            //Insert.IntoTable("EndorsementStatuses").Row(new { Description = "Requires More Information" });

            Create.Table("RPProjectEndorsements")
                  .WithColumn("RPProjectEndorsementID").AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn("RPProjectID").AsInt32().NotNullable()
                  .WithColumn("tblEmployeeID").AsInt32().NotNullable()
                  .WithColumn("EndorsementStatusID").AsInt32().NotNullable()
                  .WithColumn("EndorsementDate").AsDateTime().NotNullable()
                  .WithColumn("Comment").AsCustom("text").Nullable();
            Create.ForeignKey("FK_RPProjectEndorsements_RPProjects_RPProjectID")
                  .FromTable("RPProjectEndorsements").ForeignColumn("RPProjectID")
                  .ToTable("RPProjects").PrimaryColumn("RPProjectID");
            Create.ForeignKey("FK_RPProjectEndorsements_tblEmployee_tblEmployeeID")
                  .FromTable("RPProjectEndorsements").ForeignColumn("tblEmployeeID")
                  .ToTable("tblEmployee").PrimaryColumn("tblEmployeeID");
            Create.ForeignKey("FK_RPProjectEndorsements_EndorsementStatuses_EndorsementStatusID")
                  .FromTable("RPProjectEndorsements").ForeignColumn("EndorsementStatusID")
                  .ToTable("EndorsementStatuses").PrimaryColumn("EndorsementStatusID");
            Execute.Sql("GRANT ALL ON RPProjectEndorsements TO MCUser");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_RPProjectEndorsements_EndorsementStatuses_EndorsementStatusID")
                  .OnTable("RPProjectEndorsements");
            Delete.Table("EndorsementStatuses");
            Delete.Table("RPProjectEndorsements");
            Alter.Column("NJAWEstimate").OnTable("RPProjects").AsDecimal(18, 2);
            Delete.ForeignKey("FK_RPProjectsHighCostFactors_HighCostFactors_HighCostFactorID")
                  .OnTable("RPProjectsHighCostFactors");
            Delete.ForeignKey("FK_RPProjectsHighCostFactors_RPProjects_RPProjectID")
                  .OnTable("RPProjectsHighCostFactors");
            Delete.Table("RPProjectsHighCostFactors");
            Alter.Table("RPProjects").AddColumn("EstimatedInServicePeriod").AsAnsiString(6).Nullable();
            Rename.Column("OriginationYear").OnTable("RPProjects").To("ProjectYear");
            Rename.Column("ActualInServiceDate").OnTable("RPProjects").To("InServiceDate");
            Rename.Column("BPUApproved").OnTable("RPProjects").To("Approved");
            Execute.Sql(
                "DELETE FROM RPProjectsPipeDataLookupValues WHERE RPProjectId IN (SELECT RPProjectId FROM RPProjects WHERE ProjectTypeId IN (SELECT ProjectTypeId FROM ProjectTypes WHERE Description = 'New'));");
            Execute.Sql(
                "DELETE FROM RPProjects WHERE ProjectTypeId IN (SELECT ProjectTypeId FROM ProjectTypes WHERE Description = 'New');");
            Execute.Sql(
                "DELETE FROM RPProjectsPipeDataLookupValues WHERE RPProjectId IN (SELECT RPProjectId FROM RPProjects WHERE StatusId IN (SELECT RPProjectStatusId FROM RPPRojectStatuses WHERE Description = 'Canceled'));");
            Execute.Sql(
                "DELETE FROM RPProjects WHERE StatusId IN (SELECT RPProjectStatusId FROM RPPRojectStatuses WHERE Description = 'Canceled');");
            Delete.FromTable("ProjectTypes").Row(new {Description = "New"});
            Delete.FromTable("RPProjectStatuses").Row(new {Description = "Canceled"});
        }
    }
}
