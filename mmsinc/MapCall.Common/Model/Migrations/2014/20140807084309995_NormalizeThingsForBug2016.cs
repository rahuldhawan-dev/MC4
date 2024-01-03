using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140807084309995), Tags("Production")]
    public class NormalizeThingsForBug2016 : Migration
    {
        public struct TableNames
        {
            public const string FUNCTIONAL_AREAS = "FunctionalAreas";
        }

        public struct ColumnNames
        {
            public struct Common
            {
                public const string ID = "Id", DESCRIPTION = "Description";
            }
        }

        public override void Up()
        {
            Create.Table(TableNames.FUNCTIONAL_AREAS)
                  .WithColumn(ColumnNames.Common.ID).AsInt32().NotNullable().Identity().PrimaryKey()
                  .WithColumn(ColumnNames.Common.DESCRIPTION).AsAnsiString(20).NotNullable();

            Delete.ForeignKey("FK_tblSOP_Lookup").OnTable("tblSOP");

            Execute.Sql(
                "INSERT INTO FunctionalAreas (Description) SELECT DISTINCT LookupValue FROM Lookup WHERE LookupType = 'Functional_Area'");
            Execute.Sql(
                "UPDATE tblSOP SET Functional_Area = fa.Id FROM FunctionalAreas fa INNER JOIN Lookup l ON l.LookupValue = fa.Description WHERE l.LookupId = Functional_Area;");

            Create.ForeignKey("FK_tblSOP_FunctionalAreas_Functional_Area")
                  .FromTable("tblSOP")
                  .ForeignColumn("Functional_Area")
                  .ToTable("FunctionalAreas")
                  .PrimaryColumn("Id");

            Execute.Sql(
                @"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_tblFacilities_Coorinates') ALTER TABLE dbo.tblFacilities DROP CONSTRAINT FK_tblFacilities_Coorinates;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_tblNJAWValves_Towns_Town') ALTER TABLE dbo.tblNJAWValves DROP CONSTRAINT FK_tblNJAWValves_Towns_Town;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_SewerMainCleanings_Towns_TownID') ALTER TABLE dbo.SewerMainCleanings DROP CONSTRAINT FK_SewerMainCleanings_Towns_TownID;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_SewerOverflows_tblNJAWTownNames') ALTER TABLE dbo.SewerOverflows DROP CONSTRAINT FK_SewerOverflows_tblNJAWTownNames;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_MeterRoutes_tblNJAWTownNames_TownID') ALTER TABLE dbo.MeterRoutes DROP CONSTRAINT FK_MeterRoutes_tblNJAWTownNames_TownID;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_MeterChangeOuts_tblNJAWTownNames') ALTER TABLE dbo.MeterChangeOuts DROP CONSTRAINT FK_MeterChangeOuts_tblNJAWTownNames;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_TownValveZones_tblNJAWTownNames_TownID') ALTER TABLE dbo.TownValveZones DROP CONSTRAINT FK_TownValveZones_tblNJAWTownNames_TownID;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_OperatingCenterAssetTypes_AssetTypes') ALTER TABLE dbo.OperatingCenterAssetTypes DROP CONSTRAINT FK_OperatingCenterAssetTypes_AssetTypes;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_OperatingCenterAssetTypes_tblOpCntr') ALTER TABLE dbo.OperatingCenterAssetTypes DROP CONSTRAINT FK_OperatingCenterAssetTypes_tblOpCntr;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_SewerManholes_tblNJAWTownNames') ALTER TABLE dbo.SewerManholes DROP CONSTRAINT FK_SewerManholes_tblNJAWTownNames;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_StreetOpeningPermits_WorkOrders') ALTER TABLE dbo.StreetOpeningPermits DROP CONSTRAINT FK_StreetOpeningPermits_WorkOrders;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK__MainBreak__MainB__73BE2936') ALTER TABLE dbo.MainBreaks DROP CONSTRAINT FK__MainBreak__MainB__73BE2936;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK__MainBreak__WorkO__7977028C') ALTER TABLE dbo.MainBreaks DROP CONSTRAINT FK__MainBreak__WorkO__7977028C;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK__MainBreak__MainF__7882DE53') ALTER TABLE dbo.MainBreaks DROP CONSTRAINT FK__MainBreak__MainF__7882DE53;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK__MainBreak__MainB__74B24D6F') ALTER TABLE dbo.MainBreaks DROP CONSTRAINT FK__MainBreak__MainB__74B24D6F;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK__MainBreak__MainC__778EBA1A') ALTER TABLE dbo.MainBreaks DROP CONSTRAINT FK__MainBreak__MainC__778EBA1A;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK__MainBreak__MainB__769A95E1') ALTER TABLE dbo.MainBreaks DROP CONSTRAINT FK__MainBreak__MainB__769A95E1;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK__MainBreak__MainB__72CA04FD') ALTER TABLE dbo.MainBreaks DROP CONSTRAINT FK__MainBreak__MainB__72CA04FD;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_H2OSurveys_tblNJAWTownNames') ALTER TABLE dbo.H2OSurveys DROP CONSTRAINT FK_H2OSurveys_tblNJAWTownNames;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_StormWaterAssets_tblNJAWTownNames') ALTER TABLE dbo.StormWaterAssets DROP CONSTRAINT FK_StormWaterAssets_tblNJAWTownNames;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_SpoilFinalProcessingLocations_tblNJAWTownNames_TownID') ALTER TABLE dbo.SpoilFinalProcessingLocations DROP CONSTRAINT FK_SpoilFinalProcessingLocations_tblNJAWTownNames_TownID;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_SpoilStorageLocations_tblNJAWTownNames_TownID') ALTER TABLE dbo.SpoilStorageLocations DROP CONSTRAINT FK_SpoilStorageLocations_tblNJAWTownNames_TownID;");

            Execute.Sql(@"declare @oldName varchar(255);
declare @parentTable varchar(255);
declare @foreignTable varchar(255);
declare @parentColumn varchar(255);
declare @newName varchar(255);

DECLARE fkey_cursor CURSOR FOR
SELECT RC.CONSTRAINT_NAME OldName
, KF.TABLE_NAME ParentTable
, KP.TABLE_NAME ForeignTable
, KF.COLUMN_NAME ParentColum
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC
JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KF ON RC.CONSTRAINT_NAME = KF.CONSTRAINT_NAME
JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KP ON RC.UNIQUE_CONSTRAINT_NAME = KP.CONSTRAINT_NAME
WHERE RC.CONSTRAINT_NAME <> 'FK_' + KF.TABLE_NAME + '_' + KP.TABLE_NAME + '_' + KF.COLUMN_NAME
AND KF.TABLE_NAME NOT LIKE 'aspnet_%';

OPEN fkey_cursor;

FETCH NEXT FROM fkey_cursor
INTO @oldName, @parentTable, @foreignTable, @parentColumn;

WHILE @@FETCH_STATUS = 0
BEGIN
  SELECT @newName = 'FK_' + @parentTable + '_' + @foreignTable + '_' + @parentColumn;

  IF LOWER(@oldName) <> LOWER(@newName)
  BEGIN
    exec('sp_rename ''' + @oldName + ''', ''' + @newName + '''');
  END

  FETCH NEXT FROM fkey_cursor
  INTO @oldName, @parentTable, @foreignTable, @parentColumn;
END

CLOSE fkey_cursor;
DEALLOCATE fkey_cursor;");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_tblSOP_FunctionalAreas_Functional_Area").OnTable("tblSOP");

            Execute.Sql(
                "UPDATE tblSOP SET Functional_Area = l.LookupId FROM Lookup l INNER JOIN FunctionalAreas fa ON fa.Description = l.LookupValue WHERE fa.Id = Functional_Area AND l.TableName = 'tblSOP'");

            Create.ForeignKey("FK_tblSOP_Lookup")
                  .FromTable("tblSOP")
                  .ForeignColumn("Functional_Area")
                  .ToTable("Lookup")
                  .PrimaryColumn("LookupId");

            Delete.Table(TableNames.FUNCTIONAL_AREAS);
        }
    }
}
