using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131219133208), Tags("Production")]
    public class RenameBPUApprovedToBPUAcceptedOnRPProjects : Migration
    {
        public override void Up()
        {
            Rename.Column("BPUApproved").OnTable("RPProjects").To("BPUAccepted");
        }

        public override void Down()
        {
            Rename.Column("BPUAccepted").OnTable("RPProjects").To("BPUApproved");
        }
    }
}
