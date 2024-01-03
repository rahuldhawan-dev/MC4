using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200911110401737), Tags("Production")]
    public class RenameSewerManholesForMC2397 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("SewerOpeningTypes", "Catch Basin", "Clean Out", "Lamphole", "Manhole");
            Alter.Table("SewerManholes")
                 .AddForeignKeyColumn("SewerOpeningTypeId", "SewerOpeningTypes");
            Execute.Sql("UPDATE SewerManholes SET SewerOpeningTypeId = 4");
            Alter.Column("SewerOpeningTypeId")
                 .OnTable("SewerManholes").AsInt32().NotNullable();

            Rename.Column("SewerManholeInspectionFrequency").OnTable("OperatingCenters")
                  .To("SewerOpeningInspectionFrequency");
            Rename.Column("SewerManholeInspectionFrequencyUnitId").OnTable("OperatingCenters")
                  .To("SewerOpeningInspectionFrequencyUnitId");

            Rename.Table("SewerManholes").To("SewerOpenings");
            Rename.Column("SewerManholeID").OnTable("SewerOpenings").To("Id");
            Rename.Column("ManholeNumber").OnTable("SewerOpenings").To("OpeningNumber");
            Rename.Column("ManholeSuffix").OnTable("SewerOpenings").To("OpeningSuffix");
            Rename.Column("IsDoghouseManhole").OnTable("SewerOpenings").To("IsDoghouseOpening");
            Rename.Column("SewerManholeMaterialId").OnTable("SewerOpenings").To("SewerOpeningMaterialId");

            Rename.Table("SewerManholeInspections").To("SewerOpeningInspections");
            Rename.Column("SewerManholeId").OnTable("SewerOpeningInspections").To("SewerOpeningId");

            Rename.Column("OverflowManhole1").OnTable("SewerMainCleanings").To("OverflowOpening1");
            Rename.Column("OverflowManhole2").OnTable("SewerMainCleanings").To("OverflowOpening2");
            Rename.Column("ManholeCatchbasin1").OnTable("SewerMainCleanings").To("OpeningCatchbasin1");
            Rename.Column("ConditionOfManhole1ID").OnTable("SewerMainCleanings").To("ConditionOfOpening1Id");
            Rename.Column("Manhole1FrameAndCoverID").OnTable("SewerMainCleanings").To("Opening1FrameAndCoverId");
            Rename.Column("ManholeCatchbasin2").OnTable("SewerMainCleanings").To("OpeningCatchbasin2");
            Rename.Column("ConditionOfManhole2ID").OnTable("SewerMainCleanings").To("ConditionOfOpening2Id");
            Rename.Column("Manhole2FrameAndCoverID").OnTable("SewerMainCleanings").To("Opening2FrameAndCoverId");

            Rename.Column("Manhole1").OnTable("SewerMainCleanings").To("Opening1Id");
            Rename.Column("Manhole2").OnTable("SewerMainCleanings").To("Opening2Id");

            Rename.Table("SewerManholeConnections").To("SewerOpeningConnections");
            Rename.Column("SewerManholeConnectionID").OnTable("SewerOpeningConnections").To("Id");
            Rename.Column("DownstreamSewerManholeID").OnTable("SewerOpeningConnections").To("DownstreamSewerOpeningId");
            Rename.Column("UpstreamSewerManholeID").OnTable("SewerOpeningConnections").To("UpstreamSewerOpeningId");

            Rename.Column("SewerManholeID").OnTable("WorkOrders").To("SewerOpeningId");

            Rename.Table("SewerManholeMaterials").To("SewerOpeningMaterials");
            Rename.Column("SewerManholeMaterialID").OnTable("SewerOpeningMaterials").To("Id");

            Rename.Table("ManholeConditions").To("OpeningConditions");
            Rename.Column("ManholeConditionID").OnTable("OpeningConditions").To("Id");
            Rename.Column("ManholeCondition").OnTable("OpeningConditions").To("Description");

            Rename.Table("ManholeFrameAndCovers").To("OpeningFrameAndCovers");
            Rename.Column("ManholeFrameAndCoverID").OnTable("OpeningFrameAndCovers").To("Id");
            Rename.Column("ManholeFrameAndCover").OnTable("OpeningFrameAndCovers").To("Description");

            Execute.Sql("update AssetTypes set [Description] = 'Sewer Opening' where AssetTypeID = 5");

            Execute.Sql(
                "UPDATE WorkDescriptions SET Description = 'SEWER OPENING REPAIR' WHERE WorkDescriptionID = 92;" +
                "UPDATE WorkDescriptions SET Description = 'SEWER OPENING REPLACE' WHERE WorkDescriptionID = 93;" +
                "UPDATE WorkDescriptions SET Description = 'SEWER OPENING INSTALLATION' WHERE WorkDescriptionID = 94;" +
                "UPDATE WorkDescriptions SET Description = 'SEWER INVESTIGATION-OPENING' WHERE WorkDescriptionID = 130;" +
                "UPDATE WorkDescriptions SET Description = 'SEWER OPENING LANDSCAPING' WHERE WorkDescriptionID = 153;" +
                "UPDATE WorkDescriptions SET Description = 'SEWER OPENING RESTORATION INVESTIGATION' WHERE WorkDescriptionID = 154;" +
                "UPDATE WorkDescriptions SET Description = 'SEWER OPENING RESTORATION REPAIR' WHERE WorkDescriptionID = 155;" +
                "UPDATE WorkDescriptions SET Description = 'NO ISSUE FOUND - Sewer Opening' WHERE WorkDescriptionID = 248;" +
                "UPDATE WorkDescriptions SET Description = 'SEWER OPENING RETIREMENT' WHERE WorkDescriptionID = 281;" +
                "UPDATE WorkCategories SET [Description] = 'Sewer Opening Repair' where WorkCategoryID = 39;" +
                "UPDATE WorkCategories SET [Description] = 'Sewer Opening Replace' where WorkCategoryID = 40;" +
                "UPDATE WorkCategories SET [Description] = 'Sewer Opening Installation' where WorkCategoryID = 41;" +
                "UPDATE WorkCategories SET [Description] = 'Sewer Opening Retirement' where WorkCategoryID = 59");
            Execute.Sql("UPDATE NotificationPurposes SET Purpose = 'SewerOpening' WHERE NotificationPurposeID = 80;");
        }

        public override void Down()
        {
            Execute.Sql("UPDATE NotificationPurposes SET Purpose = 'SewerManhole' WHERE NotificationPurposeID = 80;");
            Execute.Sql(
                "UPDATE WorkDescriptions SET Description = 'SEWER MANHOLE REPAIR' WHERE WorkDescriptionID = 92;" +
                "UPDATE WorkDescriptions SET Description = 'SEWER MANHOLE REPLACE' WHERE WorkDescriptionID = 93;" +
                "UPDATE WorkDescriptions SET Description = 'SEWER MANHOLE INSTALLATION' WHERE WorkDescriptionID = 94;" +
                "UPDATE WorkDescriptions SET Description = 'SEWER INVESTIGATION-MANHOLE' WHERE WorkDescriptionID = 130;" +
                "UPDATE WorkDescriptions SET Description = 'SEWER MANHOLE LANDSCAPING' WHERE WorkDescriptionID = 153;" +
                "UPDATE WorkDescriptions SET Description = 'SEWER MANHOLE RESTORATION INVESTIGATION' WHERE WorkDescriptionID = 154;" +
                "UPDATE WorkDescriptions SET Description = 'SEWER MANHOLE RESTORATION REPAIR' WHERE WorkDescriptionID = 155;" +
                "UPDATE WorkDescriptions SET Description = 'NO ISSUE FOUND - Sewer Manhole' WHERE WorkDescriptionID = 248;" +
                "UPDATE WorkDescriptions SET Description = 'SEWER MANHOLE RETIREMENT' WHERE WorkDescriptionID = 281;" +
                "UPDATE WorkCategories SET [Description] = 'Sewer Manhole Repair' where WorkCategoryID = 39;" +
                "UPDATE WorkCategories SET [Description] = 'Sewer Manhole Replace' where WorkCategoryID = 40;" +
                "UPDATE WorkCategories SET [Description] = 'Sewer Manhole Installation' where WorkCategoryID = 41;" +
                "UPDATE WorkCategories SET [Description] = 'Sewer Manhole Retirement' where WorkCategoryID = 59");

            Execute.Sql("update AssetTypes set [Description] = 'Sewer Manhole' where AssetTypeID = 5");

            Rename.Table("OpeningConditions").To("ManholeConditions");
            Rename.Column("Id").OnTable("ManholeConditions").To("ManholeConditionID");
            Rename.Column("Description").OnTable("ManholeConditions").To("ManholeCondition");

            Rename.Table("OpeningFrameAndCovers").To("ManholeFrameAndCovers");
            Rename.Column("Id").OnTable("ManholeFrameAndCovers").To("ManholeFrameAndCoverID");
            Rename.Column("Description").OnTable("ManholeFrameAndCovers").To("ManholeFrameAndCover");

            Rename.Column("Id").OnTable("SewerOpeningMaterials").To("SewerManholeMaterialID");
            Rename.Table("SewerOpeningMaterials").To("SewerManholeMaterials");
            Rename.Column("SewerOpeningId").OnTable("WorkOrders").To("SewerManholeID");

            Rename.Column("Id").OnTable("SewerOpeningConnections").To("SewerManholeConnectionID");
            Rename.Column("DownstreamSewerOpeningId").OnTable("SewerOpeningConnections").To("DownstreamSewerManholeID");
            Rename.Column("UpstreamSewerOpeningId").OnTable("SewerOpeningConnections").To("UpstreamSewerManholeID");
            Rename.Table("SewerOpeningConnections").To("SewerManholeConnections");

            Rename.Column("Opening1Id").OnTable("SewerMainCleanings").To("Manhole1");
            Rename.Column("Opening2Id").OnTable("SewerMainCleanings").To("Manhole2");

            Rename.Column("OverflowOpening1").OnTable("SewerMainCleanings").To("OverflowManhole1");
            Rename.Column("OverflowOpening2").OnTable("SewerMainCleanings").To("OverflowManhole2");
            Rename.Column("OpeningCatchbasin1").OnTable("SewerMainCleanings").To("ManholeCatchbasin1");
            Rename.Column("ConditionOfOpening1Id").OnTable("SewerMainCleanings").To("ConditionOfManhole1ID");
            Rename.Column("Opening1FrameAndCoverId").OnTable("SewerMainCleanings").To("Manhole1FrameAndCoverID");
            Rename.Column("OpeningCatchbasin2").OnTable("SewerMainCleanings").To("ManholeCatchbasin2");
            Rename.Column("ConditionOfOpening2Id").OnTable("SewerMainCleanings").To("ConditionOfManhole2ID");
            Rename.Column("Opening2FrameAndCoverId").OnTable("SewerMainCleanings").To("Manhole2FrameAndCoverID");

            Rename.Column("SewerOpeningId").OnTable("SewerOpeningInspections").To("SewerManholeId");
            Rename.Table("SewerOpeningInspections").To("SewerManholeInspections");

            Rename.Column("SewerOpeningMaterialId").OnTable("SewerOpenings").To("SewerManholeMaterialId");
            Rename.Column("IsDoghouseOpening").OnTable("SewerOpenings").To("IsDoghouseManhole");
            Rename.Column("OpeningSuffix").OnTable("SewerOpenings").To("ManholeSuffix");
            Rename.Column("OpeningNumber").OnTable("SewerOpenings").To("ManholeNumber");
            Rename.Column("Id").OnTable("SewerOpenings").To("SewerManholeID");
            Rename.Table("SewerOpenings").To("SewerManholes");

            Rename.Column("SewerOpeningInspectionFrequency").OnTable("OperatingCenters")
                  .To("SewerManholeInspectionFrequency");
            Rename.Column("SewerOpeningInspectionFrequencyUnitId").OnTable("OperatingCenters")
                  .To("SewerManholeInspectionFrequencyUnitId");

            Delete.ForeignKeyColumn("SewerManholes", "SewerOpeningTypeId", "SewerOpeningTypes");
            Delete.Table("SewerOpeningTypes");
        }
    }
}
