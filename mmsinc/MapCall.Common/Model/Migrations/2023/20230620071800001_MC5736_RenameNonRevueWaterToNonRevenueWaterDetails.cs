using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230620071800001), Tags("Production")]
    public class MC5736_RenameNonRevueWaterToNonRevenueWaterDetails : Migration
    {
        public override void Up()
        {
            Rename.Table("NonRevenueWater").To("NonRevenueWaterEntries");
            Alter.Table("NonRevenueWaterEntries").AddForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId");
            Alter.Table("NonRevenueWaterEntries").AddColumn("Year").AsInt32().Nullable();
            Alter.Table("NonRevenueWaterEntries").AddColumn("Month").AsInt32().Nullable();
            Rename.Column("CreatedBy").OnTable("NonRevenueWaterEntries").To("CreatedById");
            Rename.Column("UpdatedBy").OnTable("NonRevenueWaterEntries").To("UpdatedById");
            Rename.Column("NonRevenueWaterId").OnTable("NonRevenueWaterAdjustments").To("NonRevenueWaterEntryId");
            Rename.Column("NonRevenueWaterId").OnTable("NonRevenueWaterDetails").To("NonRevenueWaterEntryId");
            Execute.Sql("EXEC sp_rename 'PK_NonRevenueWater', 'PK_NonRevenueWaterEntries'");
            Execute.Sql("EXEC sp_rename 'FK_NonRevenueWater_tblPermissions_CreatedBy', 'FK_NonRevenueWaterEntries_tblPermissions_CreatedBy'");
            Execute.Sql("EXEC sp_rename 'FK_NonRevenueWater_tblPermissions_UpdatedBy', 'FK_NonRevenueWaterEntries_tblPermissions_UpdatedBy'");
            Execute.Sql("EXEC sp_rename 'FK_NonRevenueWaterAdjustments_NonRevenueWater_NonRevenueWaterId', 'FK_NonRevenueWaterAdjustments_NonRevenueWaterEntries_NonRevenueWaterEntriesId'");
            Execute.Sql("EXEC sp_rename 'FK_NonRevenueWaterDetails_NonRevenueWater_NonRevenueWaterId', 'FK_NonRevenueWaterDetails_NonRevenueWaterEntries_NonRevenueWaterEntriesId'");
            Delete.Column("OperatingCenter").FromTable("NonRevenueWaterEntries");
        }

        public override void Down()
        {
            Rename.Table("NonRevenueWaterEntries").To("NonRevenueWater");
            Delete.ForeignKey("FK_NonRevenueWaterEntries_OperatingCenters_OperatingCenterId").OnTable("NonRevenueWater");
            Delete.Column("OperatingCenterId").FromTable("NonRevenueWater");
            Delete.Column("Year").FromTable("NonRevenueWater");
            Delete.Column("Month").FromTable("NonRevenueWater");
            Rename.Column("OperatingCenterName").OnTable("NonRevenueWater").To("OperatingCenter");
            Rename.Column("CreatedById").OnTable("NonRevenueWater").To("CreatedBy");
            Rename.Column("UpdatedById").OnTable("NonRevenueWater").To("UpdatedBy");
            Rename.Column("NonRevenueWaterEntryId").OnTable("NonRevenueWaterAdjustments").To("NonRevenueWaterId");
            Rename.Column("NonRevenueWaterEntryId").OnTable("NonRevenueWaterDetails").To("NonRevenueWaterId");
            Execute.Sql("EXEC sp_rename 'PK_NonRevenueWaterEntries', 'PK_NonRevenueWater'");
            Execute.Sql("EXEC sp_rename 'FK_NonRevenueWaterEntries_tblPermissions_CreatedBy', 'FK_NonRevenueWater_tblPermissions_CreatedBy'");
            Execute.Sql("EXEC sp_rename 'FK_NonRevenueWaterEntries_tblPermissions_UpdatedBy', 'FK_NonRevenueWater_tblPermissions_UpdatedBy'");
            Execute.Sql("EXEC sp_rename 'FK_NonRevenueWaterAdjustments_NonRevenueWaterEntries_NonRevenueWaterEntriesId', 'FK_NonRevenueWaterAdjustments_NonRevenueWater_NonRevenueWaterId'");
            Execute.Sql("EXEC sp_rename 'FK_NonRevenueWaterDetails_NonRevenueWaterEntries_NonRevenueWaterEntriesId', 'FK_NonRevenueWaterDetails_NonRevenueWater_NonRevenueWaterId'");
            Alter.Table("NonRevenueWater").AddColumn("OperatingCenter").AsString(100).NotNullable();
        }
    }
}