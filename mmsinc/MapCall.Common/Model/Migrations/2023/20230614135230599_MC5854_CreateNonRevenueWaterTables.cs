using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230614135230599), Tags("Production")]
    public class MC5854_CreateNonRevenueWaterTables : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("NonRevenueWater")
                  .WithIdentityColumn()
                  .WithColumn("OperatingCenter").AsString(100).NotNullable()
                  .WithColumn("HasBeenReportedToHyperion").AsBoolean().NotNullable().WithDefaultValue(false)
                  .WithColumn("CreatedAt").AsDateTime().NotNullable()
                  .WithForeignKeyColumn("CreatedBy", "tblPermissions", "RecId", nullable: false)
                  .WithColumn("UpdatedAt").AsDateTime().Nullable()
                  .WithForeignKeyColumn("UpdatedBy", "tblPermissions", "RecId");

            Create.Table("NonRevenueWaterDetails")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("NonRevenueWaterId", "NonRevenueWater", nullable: false)
                  .WithColumn("Month").AsString(10).NotNullable()
                  .WithColumn("Year").AsString(4).NotNullable()
                  .WithColumn("BusinessUnit").AsString(6).NotNullable()
                  .WithColumn("WorkDescription").AsString(50).NotNullable()
                  .WithColumn("TotalGallons").AsInt32().NotNullable();

            Create.Table("NonRevenueWaterAdjustments")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("NonRevenueWaterId", "NonRevenueWater", nullable: false)
                  .WithColumn("Comments").AsString(100).NotNullable()
                  .WithColumn("TotalGallons").AsInt32().NotNullable();
        }
    }
}

