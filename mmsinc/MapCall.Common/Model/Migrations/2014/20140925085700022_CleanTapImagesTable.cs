using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140925085700022), Tags("Production")]
    public class CleanTapImagesTable : Migration
    {
        #region Consts

        public const string TAP_IMAGES = "TapImages";

        #endregion

        #region Private Methods

        private void PopulateMissingTownDataOnTapImages()
        {
            Execute.Sql(@"
declare @brownsvilleTownship int
set @brownsvilleTownship = (select top 1 TownID from Towns where Town = 'BROWNSVILLE TOWNSHIP')
declare @brownsvilleBoro int
set @brownsvilleBoro = (select top 1 TownID from Towns where Town = 'BROWNSVILLE BORO')
declare @redStone int
set @redStone = (select top 1 TownID from Towns where Town = 'REDSTONE TOWNSHIP')

update TapImages set TownID = @brownsvilleTownship where Town = 'BROWNSVILLE TOWNSHIP'
update TapImages set TownID = @brownsvilleBoro where Town = 'BROWNSVILLE BORO'
-- Grindstone is located inside Redstone Township. There's only one image that has Grindstone.
update TapImages set TownID = @redStone where Town = 'Grindstone'
");
        }

        private void CleanUpOperatingCenterData()
        {
            Alter.Table(TAP_IMAGES)
                 .AddColumn("OperatingCenterId").AsInt32().Nullable()
                 .ForeignKey("FK_TapImages_OperatingCenters_OperatingCenterId", "OperatingCenters",
                      "OperatingCenterId");

            // First set the OperatingCenterIds for rows that actually have OperatignCenter values set.
            Execute.Sql(@"
update [TapImages] set TapImages.OperatingCenterId = oc.OperatingCenterId
from [TapImages]
inner join [OperatingCenters] oc on TapImages.OperatingCenter = oc.OperatingCenterCode 
");

            // Then set the OperatingCenterIds for rows that do not have OperatingCenters but do have Towns.
            Execute.Sql(@"
                update [TapImages] set OperatingCenterId = oct.OperatingCenterId
                from [TapImages]
                left join [OperatingCentersTowns] oct on oct.TownID = TapImages.TownID 
                where [TapImages].OperatingCenterId is null
                ");

            Delete.Column("OperatingCenter").FromTable(TAP_IMAGES);

            Alter.Table(TAP_IMAGES)
                 .AlterColumn("OperatingCenterId").AsInt32().NotNullable();
        }

        private void CleanBadStringData()
        {
            // Clean up Streets for use with StreetRepository searches.
            Execute.Sql(@"
update Streets set StreetPrefix = null where StreetPrefix = ''
update Streets set StreetSuffix = null where StreetSuffix = ''"
            );

            Execute.Sql(@"
update TapImages set PremiseNumber = LTRIM(RTRIM(PremiseNumber)) 
update TapImages set Street = null where Street = ''
update TapImages set StreetSuffix = null where StreetSuffix = ''
update TapImages set StreetPrefix = null where StreetPrefix = ''
update TapImages set ApartmentNumber = null where ApartmentNumber = ''
update TapImages set TownSection = null where TownSection = ''
update TapImages set Lot = null where Lot = ''
update TapImages set Block = null where Block = ''
update TapImages set ServiceSize = null where ServiceSize = ''
update TapImages set ServiceMaterial = null where ServiceMaterial = ''
update TapImages set LengthofService = null where LengthOfService = ''
update TapImages set MainSize = null where MainSize = ''
update TapImages set WorkOrderNumber = null where WorkOrderNumber = '' or WorkOrderNumber = '.'
");
        }

        #endregion

        #region Public Methods

        public override void Up()
        {
            PopulateMissingTownDataOnTapImages();
            Execute.Sql(
                "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[TapImages]') AND name = N'_dta_index_Taps_22_2068202418__K1_K4_K5_K15_K7_K43_K47') DROP INDEX [_dta_index_Taps_22_2068202418__K1_K4_K5_K15_K7_K43_K47] ON [dbo].[TapImages]");
            Execute.Sql(
                "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[TapImages]') AND name = N'_dta_index_Taps_22_2068202418__K2') DROP INDEX [_dta_index_Taps_22_2068202418__K2] ON [dbo].[TapImages]");
            Execute.Sql(
                "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[TapImages]') AND name = N'_dta_index_Taps_22_2068202418__K2_K4') DROP INDEX [_dta_index_Taps_22_2068202418__K2_K4] ON [dbo].[TapImages]");

            Delete.Column("DateCreated").FromTable(TAP_IMAGES);
            Delete.Column("Town").FromTable(TAP_IMAGES);
            Delete.Column("State").FromTable(TAP_IMAGES);

            // can be retrieved through TownId
            Delete.Column("District").FromTable(TAP_IMAGES);

            // F11-F14 are all poorly mishmashed data of different types.
            Delete.Column("F11Label").FromTable(TAP_IMAGES);
            Delete.Column("F12Label").FromTable(TAP_IMAGES);
            Delete.Column("F13Label").FromTable(TAP_IMAGES);
            Delete.Column("F14Label").FromTable(TAP_IMAGES);

            // All rows have null values
            Delete.Column("GeoGrid").FromTable(TAP_IMAGES);

            // Only three rows have values.
            Delete.Column("PreviousServiceSize").FromTable(TAP_IMAGES);

            // Only four rows have values
            Delete.Column("TapSize").FromTable(TAP_IMAGES);

            // Only one row has a value
            Delete.Column("MainMaterial").FromTable(TAP_IMAGES);

            // Ten or fewer rows with values.
            Delete.Column("CustomerFirstName").FromTable(TAP_IMAGES);
            Delete.Column("CustomerLastName").FromTable(TAP_IMAGES);

            // Not referenced anywhere and only zeroes are being inserted now.
            Delete.Column("OldTapID").FromTable(TAP_IMAGES);

            // Not used anywhere. Only 16 rows have "1" for a value, all others are zero or null.
            Delete.Column("imgInData").FromTable(TAP_IMAGES);

            // Only two rows have values.
            Delete.Column("OriginalInstallationDate").FromTable(TAP_IMAGES);

            // There's a handful(roughly 100) of useless rows that need to go away so FileList and fld can be not-nullable.
            // These images are useless since they point at literally nothing.
            Execute.Sql("delete from [TapImages] where fld is null or filelist is null");

            Alter.Table(TAP_IMAGES)
                 .AlterColumn("fld").AsString(30).NotNullable()
                 .AlterColumn("filelist").AsString(255).NotNullable()
                 .AlterColumn("TownID").AsInt32().NotNullable();

            CleanUpOperatingCenterData();

            Alter.Table(TAP_IMAGES)
                 .AddColumn("DateCompletedCorrected")
                 .AsDateTime()
                 .Nullable();

            Execute.Sql("update [TapImages] set [DateCompletedCorrected] = CONVERT(datetime, [DateCompleted], 0)");

            Delete.Column("DateCompleted")
                  .FromTable(TAP_IMAGES);

            Rename.Column("DateCompletedCorrected")
                  .OnTable(TAP_IMAGES)
                  .To("DateCompleted");

            CleanBadStringData();
        }

        public override void Down()
        {
            Alter.Table(TAP_IMAGES)
                 .AlterColumn("TownID").AsInt32().Nullable()
                 .AddColumn("DateCreated").AsDateTime().Nullable()
                 .AddColumn("Town").AsString(50).Nullable()
                 .AddColumn("State").AsString(2).Nullable()
                 .AddColumn("District").AsString(6).Nullable()
                 .AddColumn("F11Label").AsString(50).Nullable()
                 .AddColumn("F12Label").AsString(50).Nullable()
                 .AddColumn("F13Label").AsString(50).Nullable()
                 .AddColumn("F14Label").AsString(50).Nullable()
                 .AddColumn("GeoGrid").AsString(50).Nullable()
                 .AddColumn("PreviousServiceSize").AsString(50).Nullable()
                 .AddColumn("TapSize").AsString(50).Nullable()
                 .AddColumn("MainMaterial").AsString(50).Nullable()
                 .AddColumn("CustomerFirstName").AsString(255).Nullable()
                 .AddColumn("CustomerLastName").AsString(255).Nullable()
                 .AddColumn("OldTapID").AsInt32().Nullable()
                 .AddColumn("OperatingCenter").AsString(50).Nullable()
                 .AddColumn("OriginalInstallationDate").AsString(50).Nullable()
                 .AddColumn("imgInData").AsBoolean().Nullable();

            Execute.Sql(@"
update [TapImages] set TapImages.OperatingCenter = oc.OperatingCenterCode
from [TapImages]
inner join [OperatingCenters] oc on TapImages.OperatingCenterId = oc.OperatingCenterId 
");
            Delete.ForeignKey("FK_TapImages_OperatingCenters_OperatingCenterId").OnTable(TAP_IMAGES);
            Delete.Column("OperatingCenterId").FromTable(TAP_IMAGES);

            Rename.Column("DateCompleted")
                  .OnTable(TAP_IMAGES)
                  .To("DateCompletedCorrected");

            Alter.Table(TAP_IMAGES)
                 .AddColumn("DateCompleted").AsString(50).Nullable();

            Execute.Sql("update [TapImages] set [DateCompleted] = CONVERT(varchar(50), [DateCompletedCorrected], 0)");

            Delete.Column("DateCompletedCorrected").FromTable(TAP_IMAGES);
        }

        #endregion
    }
}
