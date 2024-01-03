using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161004145932433), Tags("Production")]
    public class AddManyToManyForAllocatioGroupingsAndNotesForBug3171 : Migration
    {
        public const string TABLE_NAME = "AllocationPermitsAllocationPermitWithdrawalNodes";

        public override void Up()
        {
            var aps = "AllocationPermits";
            var apwns = "AllocationPermitWithdrawalNodes";
            Create.Table(TABLE_NAME)
                  .WithForeignKeyColumn("AllocationPermitID", aps, "AllocationPermitID")
                  .WithForeignKeyColumn("AllocationPermitWithdrawalNodeID", apwns, "AllocationPermitWithdrawalNodeID");
            Execute.Sql("INSERT INTO " + TABLE_NAME +
                        " SELECT AllocationPermitID, AllocationPermitWithdrawalNodeID FROM " + apwns);
            Delete.ForeignKeyColumn(apwns, "AllocationPermitID", aps, "AllocationPermitID");
        }

        public override void Down()
        {
            var aps = "AllocationPermits";
            var apwns = "AllocationPermitWithdrawalNodes";
            Alter.Table(apwns)
                 .AddForeignKeyColumn("AllocationPermitID", aps, "AllocationPermitID");
            Execute.Sql("UPDATE " + apwns + " " +
                        "SET AllocationPermitID = " +
                        "  (SELECT TOP 1 AllocationPermitID FROM [" + TABLE_NAME +
                        "] wgawn WHERE wgawn.AllocationPermitWithdrawalNodeID = " + apwns +
                        ".AllocationPermitWithdrawalNodeID)");
            Delete.Table(TABLE_NAME);
        }
    }
}
