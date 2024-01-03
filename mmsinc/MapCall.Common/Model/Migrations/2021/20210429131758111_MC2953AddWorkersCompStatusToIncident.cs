using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210429131758111), Tags("Production")]
    public class MC2953AddWorkersCompStatusToIncident : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("WorkersCompensationClaimStatuses", "No Claim Created", "Open", "Closed - Accepted (Compensable)", "Closed - Denied (Not Compensable)");
            Alter.Table("Incidents").AddForeignKeyColumn("WorkersCompensationClaimStatusId", "WorkersCompensationClaimStatuses");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Incidents", "WorkersCompensationClaimStatusId", "WorkersCompensationClaimStatuses");
            Delete.Table("WorkersCompensationClaimStatuses");
        }
    }
}

