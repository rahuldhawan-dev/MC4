using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160419134935854), Tags("Production")]
    public class CreateAPCInspectionItemsTableForBug2891 : Migration
    {
        public struct TableNames
        {
            public const string APC_INSPECTION_ITEMS = "APCInspectionItems",
                                APC_INSPECTION_ITEM_TYPES = "APCInspectionItemTypes";
        }

        public struct StringLengths
        {
            public const int AREA = 50, DESCRIPTION = 255;
        }

        public override void Up()
        {
            Create.Table(TableNames.APC_INSPECTION_ITEM_TYPES)
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(StringLengths.DESCRIPTION).NotNullable();

            Create.Table(TableNames.APC_INSPECTION_ITEMS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId", false)
                  .WithForeignKeyColumn("FacilityId", "tblFacilities", "RecordId", false)
                  .WithColumn("Area").AsString(StringLengths.AREA).NotNullable()
                  .WithColumn("Description").AsString(StringLengths.DESCRIPTION).NotNullable()
                  .WithForeignKeyColumn("TypeId", TableNames.APC_INSPECTION_ITEM_TYPES, nullable: false)
                  .WithColumn("DateReported").AsDateTime().NotNullable()
                  .WithForeignKeyColumn("AssignedToId", "tblEmployee", "tblEmployeeId", false)
                  .WithColumn("DateRectified").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table(TableNames.APC_INSPECTION_ITEMS);
            Delete.Table(TableNames.APC_INSPECTION_ITEM_TYPES);
        }
    }
}
