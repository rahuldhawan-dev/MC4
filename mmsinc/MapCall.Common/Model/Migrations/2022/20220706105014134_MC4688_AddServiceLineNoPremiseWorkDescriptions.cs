using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220706105014134), Tags("Production")]
    public class MC4688_AddServiceLineNoPremiseWorkDescriptions : Migration
    {
        public override void Up()
        {
            Insert
               .IntoTable("WorkDescriptions")
               .Rows(
                    new {
                        Description = "SERVICE LINE RETIRE NO PREMISE",
                        AssetTypeId = 4,
                        TimeToComplete = 2.5m,
                        WorkCategoryId = 15,
                        AccountingTypeId = 3,
                        FirstRestorationAccountingCodeId = 26,
                        FirstRestorationCostBreakdown = 100,
                        FirstRestorationProductCodeId = 1,
                        ShowBusinessUnit = false,
                        ShowApprovalAccounting = true,
                        EditOnly = false,
                        Revisit = false,
                        PlantMaintenanceActivityTypeId = 5,
                        IsActive = true,
                        MarkoutRequired = true,
                        MaterialsRequired = false,
                        JobSiteCheckListRequired = true
                    },
                    new {
                        Description = "SERVICE LINE RETIRE-LEAD NO PREMISE",
                        AssetTypeId = 4,
                        TimeToComplete = 2.5m,
                        WorkCategoryId = 15,
                        AccountingTypeId = 3,
                        FirstRestorationAccountingCodeId = 26,
                        FirstRestorationCostBreakdown = 100,
                        FirstRestorationProductCodeId = 1,
                        ShowBusinessUnit = false,
                        ShowApprovalAccounting = true,
                        EditOnly = false,
                        Revisit = false,
                        PlantMaintenanceActivityTypeId = 5,
                        IsActive = true,
                        MarkoutRequired = true,
                        MaterialsRequired = false,
                        JobSiteCheckListRequired = true
                    });
        }

        public override void Down()
        {
            Delete
               .FromTable("WorkDescriptions")
               .Rows(
                    new { Description = "SERVICE LINE RETIRE NO PREMISE" },
                    new { Description = "SERVICE LINE RETIRE-LEAD NO PREMISE" });
        }
    }
}

