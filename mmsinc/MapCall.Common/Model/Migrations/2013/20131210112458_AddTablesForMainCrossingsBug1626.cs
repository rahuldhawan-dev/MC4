using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131210112458), Tags("Production")]
    public class AddTablesForMainCrossingsBug1626 : Migration
    {
        #region Constants

        public struct Columns
        {
            public const string MAIN_CROSSING_ID = "MainCrossingID",
                                TOWN_ID = "TownID",
                                OPERATING_CENTER_ID = "OperatingCenterID",
                                CROSSING_CATEGORY_ID = "CrossingCategoryID",
                                STREAM_ID = "BodyOfWaterID",
                                PWSID = "PWSID",
                                MAIN_MATERIAL_ID = "MainMaterialID",
                                MAIN_DIAMETER_ID = "MainDiameterID",
                                LENGTH_OF_SEGMENT = "LengthOfSegment",
                                COMPANY_OWNED = "IsCompanyOwned",
                                CLOSEST_STREET = "ClosestStreetID",
                                COORDINATE_ID = "CoordinateID",
                                CRITICAL_ASSET = "IsCriticalAsset",
                                MAX_DAILY_FLOW = "MaximumDailyFlow",
                                CUSTOMER_RANGE_ID = "CustomerRangeID",
                                COMMENTS = "Comments",
                                PRESSURE_ZONE_ID = "PressureZoneID",
                                DESCRIPTION = "Description",
                                PWSID_ID = "RecordId",
                                BODY_OF_WATER_ID = "BodyOfWaterID",
                                PIPE_DIAMETER_ID = "PipeDiameterID",
                                PIPE_MATERIAL_ID = "PipeMaterialID",
                                STREET_ID = "StreetID",
                                SUPPORT_STRUCTURE_ID = "SupportStructureID";
        }

        public struct Tables
        {
            public const string MAIN_CROSSINGS = "MainCrossings",
                                PRESSURE_ZONES = "PressureZones", //
                                CUSTOMER_RANGES = "CustomerRanges", //
                                BODIES_OF_WATER = "BodiesOfWater", // existing table
                                CROSSING_CATEGORIES = "CrossingCategories", //
                                PIPE_DIAMETERS = "PipeDiameters",
                                PIPE_MATERIALS = "PipeMaterials",
                                PWSID = "tblPWSID",
                                TOWNS = "Towns",
                                STREETS = "Streets",
                                OPERATING_CENTERS = "OperatingCenters",
                                COORDINATES = "Coordinates",
                                SUPPORT_STRUCTURES = "SupportStructures";
        }

        public struct ForeignKeys
        {
            public const string MAIN_CROSSINGS_PRESSURE_ZONES = "FK_MainCrossings_PressureZones_PressureZoneID",
                                MAIN_CROSSINGS_CUSTOMER_RANGES = "FK_MainCrossings_CustomerRanges_CustomerRangeID",
                                MAIN_CROSSINGS_BODIES_OF_WATER = "FK_MainCrossings_BodiesOfWater_BodyOfWaterID",
                                MAIN_CROSSINGS_CROSSING_CATEGORIES =
                                    "FK_MainCrossings_CrossingCategories_CrossingCategoryID",
                                MAIN_CROSSINGS_PIPE_DIAMETERS = "FK_MainCrossings_PipeDiameters_MainDiameterID",
                                MAIN_CROSSINGS_PIPE_MATERIALS = "FK_MainCrossings_PipeMaterials_MainMaterialID",
                                MAIN_CROSSINGS_PWSID = "FK_MainCrossings_tblPWSID_PWSID",
                                MAIN_CROSSINGS_TOWN_ID = "FK_MainCrossings_Towns_TownID",
                                MAIN_CROSSINGS_OPERATING_CENTER_ID =
                                    "FK_MainCrossings_OperatingCenters_OperatingCenterID",
                                MAIN_CROSSINGS_STREETS = "FK_MainCrossings_Streets_ClosestStreetID",
                                MAIN_CROSSINGS_COORDINATES = "FK_MainCrossings_Coordinates_CoordinateID",
                                MAIN_CROSSINGS_SUPPORT_STRUCTURES =
                                    "FK_MainCrossings_SupportStructures_SupportStructureID";
        }

        public struct StringLengths
        {
            public const int DESCRIPTION = 50;
        }

        #endregion

        public override void Up()
        {
            #region TABLES

            Create.Table(Tables.PRESSURE_ZONES)
                  .WithColumn(Columns.PRESSURE_ZONE_ID).AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.CROSSING_CATEGORIES)
                  .WithColumn(Columns.CROSSING_CATEGORY_ID).AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.CUSTOMER_RANGES)
                  .WithColumn(Columns.CUSTOMER_RANGE_ID).AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.SUPPORT_STRUCTURES)
                  .WithColumn(Columns.SUPPORT_STRUCTURE_ID).AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();

            Create.Table(Tables.MAIN_CROSSINGS)
                  .WithColumn(Columns.MAIN_CROSSING_ID).AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn(Columns.OPERATING_CENTER_ID).AsInt32().NotNullable()
                  .WithColumn(Columns.TOWN_ID).AsInt32().Nullable()
                  .WithColumn(Columns.CROSSING_CATEGORY_ID).AsInt32().NotNullable()
                  .WithColumn(Columns.STREAM_ID).AsInt32().Nullable()
                  .WithColumn(Columns.PWSID).AsInt32().Nullable()
                  .WithColumn(Columns.COMPANY_OWNED).AsBoolean().Nullable()
                  .WithColumn(Columns.MAIN_MATERIAL_ID).AsInt32().Nullable()
                  .WithColumn(Columns.MAIN_DIAMETER_ID).AsInt32().Nullable()
                  .WithColumn(Columns.LENGTH_OF_SEGMENT).AsDecimal(18, 2).Nullable()
                  .WithColumn(Columns.CLOSEST_STREET).AsInt32().Nullable()
                  .WithColumn(Columns.COORDINATE_ID).AsInt32().Nullable()
                  .WithColumn(Columns.CRITICAL_ASSET).AsBoolean().Nullable()
                  .WithColumn(Columns.MAX_DAILY_FLOW).AsDecimal(18, 2).Nullable()
                  .WithColumn(Columns.CUSTOMER_RANGE_ID).AsInt32().Nullable()
                  .WithColumn(Columns.COMMENTS).AsCustom("text").Nullable()
                  .WithColumn(Columns.PRESSURE_ZONE_ID).AsInt32().Nullable()
                  .WithColumn(Columns.SUPPORT_STRUCTURE_ID).AsInt32().Nullable();

            #endregion

            #region FOREIGN KEYS

            Create.ForeignKey(ForeignKeys.MAIN_CROSSINGS_PRESSURE_ZONES)
                  .FromTable(Tables.MAIN_CROSSINGS).ForeignColumn(Columns.PRESSURE_ZONE_ID)
                  .ToTable(Tables.PRESSURE_ZONES).PrimaryColumn(Columns.PRESSURE_ZONE_ID);
            Create.ForeignKey(ForeignKeys.MAIN_CROSSINGS_CUSTOMER_RANGES)
                  .FromTable(Tables.MAIN_CROSSINGS).ForeignColumn(Columns.CUSTOMER_RANGE_ID)
                  .ToTable(Tables.CUSTOMER_RANGES).PrimaryColumn(Columns.CUSTOMER_RANGE_ID);
            Create.ForeignKey(ForeignKeys.MAIN_CROSSINGS_BODIES_OF_WATER)
                  .FromTable(Tables.MAIN_CROSSINGS).ForeignColumn(Columns.BODY_OF_WATER_ID)
                  .ToTable(Tables.BODIES_OF_WATER).PrimaryColumn(Columns.BODY_OF_WATER_ID);
            Create.ForeignKey(ForeignKeys.MAIN_CROSSINGS_CROSSING_CATEGORIES)
                  .FromTable(Tables.MAIN_CROSSINGS).ForeignColumn(Columns.CROSSING_CATEGORY_ID)
                  .ToTable(Tables.CROSSING_CATEGORIES).PrimaryColumn(Columns.CROSSING_CATEGORY_ID);
            Create.ForeignKey(ForeignKeys.MAIN_CROSSINGS_PIPE_DIAMETERS)
                  .FromTable(Tables.MAIN_CROSSINGS).ForeignColumn(Columns.MAIN_DIAMETER_ID)
                  .ToTable(Tables.PIPE_DIAMETERS).PrimaryColumn(Columns.PIPE_DIAMETER_ID);

            Create.ForeignKey(ForeignKeys.MAIN_CROSSINGS_PIPE_MATERIALS)
                  .FromTable(Tables.MAIN_CROSSINGS).ForeignColumn(Columns.MAIN_MATERIAL_ID)
                  .ToTable(Tables.PIPE_MATERIALS).PrimaryColumn(Columns.PIPE_MATERIAL_ID);
            Create.ForeignKey(ForeignKeys.MAIN_CROSSINGS_PWSID)
                  .FromTable(Tables.MAIN_CROSSINGS).ForeignColumn(Columns.PWSID)
                  .ToTable(Tables.PWSID).PrimaryColumn(Columns.PWSID_ID);
            Create.ForeignKey(ForeignKeys.MAIN_CROSSINGS_TOWN_ID)
                  .FromTable(Tables.MAIN_CROSSINGS).ForeignColumn(Columns.TOWN_ID)
                  .ToTable(Tables.TOWNS).PrimaryColumn(Columns.TOWN_ID);
            Create.ForeignKey(ForeignKeys.MAIN_CROSSINGS_OPERATING_CENTER_ID)
                  .FromTable(Tables.MAIN_CROSSINGS).ForeignColumn(Columns.OPERATING_CENTER_ID)
                  .ToTable(Tables.OPERATING_CENTERS).PrimaryColumn(Columns.OPERATING_CENTER_ID);
            Create.ForeignKey(ForeignKeys.MAIN_CROSSINGS_STREETS)
                  .FromTable(Tables.MAIN_CROSSINGS).ForeignColumn(Columns.CLOSEST_STREET)
                  .ToTable(Tables.STREETS).PrimaryColumn(Columns.STREET_ID);

            Create.ForeignKey(ForeignKeys.MAIN_CROSSINGS_COORDINATES)
                  .FromTable(Tables.MAIN_CROSSINGS).ForeignColumn(Columns.COORDINATE_ID)
                  .ToTable(Tables.COORDINATES).PrimaryColumn(Columns.COORDINATE_ID);
            Create.ForeignKey(ForeignKeys.MAIN_CROSSINGS_SUPPORT_STRUCTURES)
                  .FromTable(Tables.MAIN_CROSSINGS).ForeignColumn(Columns.SUPPORT_STRUCTURE_ID)
                  .ToTable(Tables.SUPPORT_STRUCTURES).PrimaryColumn(Columns.SUPPORT_STRUCTURE_ID);

            #endregion
        }

        public override void Down()
        {
            #region FOREIGN KEYS

            Delete.ForeignKey(ForeignKeys.MAIN_CROSSINGS_COORDINATES).OnTable(Tables.MAIN_CROSSINGS);
            Delete.ForeignKey(ForeignKeys.MAIN_CROSSINGS_STREETS).OnTable(Tables.MAIN_CROSSINGS);
            Delete.ForeignKey(ForeignKeys.MAIN_CROSSINGS_OPERATING_CENTER_ID).OnTable(Tables.MAIN_CROSSINGS);
            Delete.ForeignKey(ForeignKeys.MAIN_CROSSINGS_TOWN_ID).OnTable(Tables.MAIN_CROSSINGS);
            Delete.ForeignKey(ForeignKeys.MAIN_CROSSINGS_PWSID).OnTable(Tables.MAIN_CROSSINGS);

            Delete.ForeignKey(ForeignKeys.MAIN_CROSSINGS_PIPE_MATERIALS).OnTable(Tables.MAIN_CROSSINGS);
            Delete.ForeignKey(ForeignKeys.MAIN_CROSSINGS_PIPE_DIAMETERS).OnTable(Tables.MAIN_CROSSINGS);
            Delete.ForeignKey(ForeignKeys.MAIN_CROSSINGS_CROSSING_CATEGORIES).OnTable(Tables.MAIN_CROSSINGS);
            Delete.ForeignKey(ForeignKeys.MAIN_CROSSINGS_BODIES_OF_WATER).OnTable(Tables.MAIN_CROSSINGS);
            Delete.ForeignKey(ForeignKeys.MAIN_CROSSINGS_CUSTOMER_RANGES).OnTable(Tables.MAIN_CROSSINGS);

            Delete.ForeignKey(ForeignKeys.MAIN_CROSSINGS_PRESSURE_ZONES).OnTable(Tables.MAIN_CROSSINGS);
            Delete.ForeignKey(ForeignKeys.MAIN_CROSSINGS_SUPPORT_STRUCTURES).OnTable(Tables.MAIN_CROSSINGS);

            #endregion

            #region Tables

            Delete.Table(Tables.SUPPORT_STRUCTURES);
            Delete.Table(Tables.PRESSURE_ZONES);
            Delete.Table(Tables.CROSSING_CATEGORIES);
            Delete.Table(Tables.CUSTOMER_RANGES);
            Delete.Table(Tables.MAIN_CROSSINGS);

            #endregion
        }
    }
}
