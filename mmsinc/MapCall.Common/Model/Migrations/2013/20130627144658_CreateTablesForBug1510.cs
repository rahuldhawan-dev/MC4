using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130627144658)]
    public class CreateTablesForBug1510 : Migration
    {
        public struct TableNames
        {
            public const string FILTER_MEDIA_TYPES = "FilterMediaTypes",
                                FILTER_MEDIA_LEVEL_CONTROL_METHODS = "FilterMediaLevelControlMethods",
                                FILTER_MEDIA_WASH_TYPES = "FilterMediaWashTypes",
                                FILTER_MEDIA_FILTER_TYPES = "FilterMediaFilterTypes",
                                FILTER_MEDIA_LOCATIONS = "FilterMediaLocations",
                                FILTER_MEDIA = "FilterMedia";
        }

        public struct ColumnNames
        {
            public struct FilterMediaTypes
            {
                public const string ID = "FilterMediaTypeId",
                                    DESCRIPTION = "Description";
            }

            public struct FilterMediaLevelControlMethods
            {
                public const string ID = "FilterMediaLevelControlMethodsId",
                                    DESCRIPTION = "Description";
            }

            public struct FilterMediaWashTypes
            {
                public const string ID = "FilterMediaWashTypeId",
                                    DESCRIPTION = "Description";
            }

            public struct FilterMediaFilterTypes
            {
                public const string ID = "FilterMediaFilterTypeId",
                                    DESCRIPTION = "Description";
            }

            public struct FilterMediaLocations
            {
                public const string ID = "FilterMediaLocationId",
                                    DESCRIPTION = "Description";
            }

            public struct FilterMedia
            {
                public const string ID = "FilterMediaId",
                                    FILTER_NUMBER = "FilterNumber",
                                    EQUIPMENT_IDENTIFIER = "EquipmentIdentifier",
                                    YEAR_IN_SERVICE = "YearInService",
                                    FILTER_TYPE_ID = "FilterTypeId",
                                    ESTIMATED_MEDIA_LIFECYCLE = "EstimatedMediaLifecycle",
                                    CAPACITY_MGD = "CapacityMGD",
                                    COEFFICIENT = "Coefficient",
                                    FILTER_DIMENSIONS = "FilterDimensions",
                                    MEDIA_AREA = "MediaArea",
                                    MEDIA_VOLUME = "MediaVolume",
                                    GRAVEL_SUPPORT_MEDIA = "GravelSupportMedia",
                                    MONTHLY_MEDIA_EXPENSE = "MonthlyMediaExpense",
                                    ANNUAL_INSPECTION_COSTS = "AnnualInspectionCosts",
                                    ANNUAL_ANALYSIS_COSTS = "AnnualAnalysisCosts",
                                    ANNUAL_COMPANY_LABOR_COSTS = "AnnualCompanyLaborCosts",
                                    EQUIPMENT_CRITICAL_RATING = "EquipmentCriticalRating",
                                    YEAR_LAST_PAINTED = "YearLastPainted",
                                    SERVED_BY_STANDBY_POWER = "ServedByStandbyPower",
                                    TURBIDIMETER_MODEL = "TurbidimeterModel",
                                    LEVEL_CONTROL_METHOD_ID = "LevelControlMethodId",
                                    NOTES = "Notes",
                                    PRODUCT_CODE = "ProductCode",
                                    ANTHRACITE_DEPTH = "AnthraciteDepth",
                                    GAC_DEPTH = "GACDepth",
                                    SAND_DEPTH = "SandDepth",
                                    GRAVEL_DEPTH = "GravelDepth",
                                    LAST_TIME_CHANGED = "LastTimeChanged",
                                    LAST_TIME_CLEANED = "LastTimeCleaned",
                                    AIR_SCOURING = "AirScouring",
                                    RECYCLING = "Recycling",
                                    COMMENT = "Comment",
                                    EQUIPMENT_ID = "EquipmentId",
                                    WASH_TYPE_ID = "WashTypeId",
                                    MEDIA_TYPE_ID = "MediaTypeId",
                                    LOCATION_ID = "LocationId";
            }
        }

        public struct StringLengths
        {
            public struct FilterMediaTypes
            {
                public const int DESCRIPTION = 25;
            }

            public struct FilterMediaLevelControlMethods
            {
                public const int DESCRIPTION = 30;
            }

            public struct FilterMediaWashTypes
            {
                public const int DESCRIPTION = 20;
            }

            public struct FilterMediaFilterTypes
            {
                public const int DESCRIPTION = 30;
            }

            public struct FilterMediaLocations
            {
                public const int DESCRIPTION = 10;
            }

            public struct FilterMedia
            {
                public const int EQUIPMENT_IDENTIFIER = 255,
                                 COEFFICIENT = 255,
                                 FILTER_DIMENSIONS = 255,
                                 TURBIDIMETER_MODEL = 255,
                                 NOTES = 255,
                                 PRODUCT_CODE = 255,
                                 COMMENT = 255;
            }
        }

        public struct ForeignKeyNames
        {
            public struct FilterMedia
            {
                public const string FACILITY = "FK_FilterMedia_tblFacilities_FacilityId",
                                    FILTER_TYPE = "FK_FilterMedia_FilterMediaFilterTypes_FilterTypeId",
                                    LEVEL_CONTROL_METHOD =
                                        "FK_FilterMedia_FilterMediaLevelControlMethods_LevelControlMethodId",
                                    EQUIPMENT = "FK_FilterMedia_Equipment_EquipmentId",
                                    WASH_TYPE = "FK_FilterMedia_FilterMediaWashTypes_WashTypeId",
                                    MEDIA_TYPE = "FK_FilterMedia_FilterMediaTypes_MediaTypeId",
                                    LOCATION = "FK_FilterMedia_FilterMediaLocations_LocationId";
            }
        }

        public override void Up()
        {
            Create.PrimaryKey("PK_tblEquipment").OnTable("tblEquipment").Column("RecordId");

            Create.Table(TableNames.FILTER_MEDIA_TYPES)
                  .WithColumn(ColumnNames.FilterMediaTypes.ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(ColumnNames.FilterMediaTypes.DESCRIPTION)
                  .AsAnsiString(StringLengths.FilterMediaTypes.DESCRIPTION).NotNullable();

            Insert.IntoTable(TableNames.FILTER_MEDIA_TYPES).Row(new {Description = "Anthracite / Sand"});
            Insert.IntoTable(TableNames.FILTER_MEDIA_TYPES).Row(new {Description = "GAC"});
            Insert.IntoTable(TableNames.FILTER_MEDIA_TYPES).Row(new {Description = "Green Sand / Anthracite"});

            Create.Table(TableNames.FILTER_MEDIA_LEVEL_CONTROL_METHODS)
                  .WithColumn(ColumnNames.FilterMediaLevelControlMethods.ID).AsInt32().PrimaryKey().Identity()
                  .NotNullable()
                  .WithColumn(ColumnNames.FilterMediaLevelControlMethods.DESCRIPTION)
                  .AsAnsiString(StringLengths.FilterMediaLevelControlMethods.DESCRIPTION).NotNullable();

            Insert.IntoTable(TableNames.FILTER_MEDIA_LEVEL_CONTROL_METHODS)
                  .Row(new {Description = "Effluent Rate of Flow Control"});
            Insert.IntoTable(TableNames.FILTER_MEDIA_LEVEL_CONTROL_METHODS)
                  .Row(new {Description = "Influent Rate of Flow Control"});
            Insert.IntoTable(TableNames.FILTER_MEDIA_LEVEL_CONTROL_METHODS).Row(new {Description = "Influent Valve"});

            Create.Table(TableNames.FILTER_MEDIA_WASH_TYPES)
                  .WithColumn(ColumnNames.FilterMediaWashTypes.ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(ColumnNames.FilterMediaWashTypes.DESCRIPTION)
                  .AsAnsiString(StringLengths.FilterMediaWashTypes.DESCRIPTION).NotNullable();

            Insert.IntoTable(TableNames.FILTER_MEDIA_WASH_TYPES).Row(new {Description = "Surface"});
            Insert.IntoTable(TableNames.FILTER_MEDIA_WASH_TYPES).Row(new {Description = "Air"});

            Create.Table(TableNames.FILTER_MEDIA_FILTER_TYPES)
                  .WithColumn(ColumnNames.FilterMediaFilterTypes.ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(ColumnNames.FilterMediaFilterTypes.DESCRIPTION)
                  .AsAnsiString(StringLengths.FilterMediaFilterTypes.DESCRIPTION).NotNullable();

            Insert.IntoTable(TableNames.FILTER_MEDIA_FILTER_TYPES).Row(new {Description = "Concrete Gravity"});
            Insert.IntoTable(TableNames.FILTER_MEDIA_FILTER_TYPES).Row(new {Description = "Pressure Vessel"});
            Insert.IntoTable(TableNames.FILTER_MEDIA_FILTER_TYPES).Row(new {Description = "Steel Gravity"});

            Create.Table(TableNames.FILTER_MEDIA_LOCATIONS)
                  .WithColumn(ColumnNames.FilterMediaLocations.ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(ColumnNames.FilterMediaLocations.DESCRIPTION)
                  .AsAnsiString(StringLengths.FilterMediaLocations.DESCRIPTION).NotNullable();

            Insert.IntoTable(TableNames.FILTER_MEDIA_LOCATIONS).Row(new {Description = "Indoor"});
            Insert.IntoTable(TableNames.FILTER_MEDIA_LOCATIONS).Row(new {Description = "Outdoor"});

            Create.Table(TableNames.FILTER_MEDIA)
                  .WithColumn(ColumnNames.FilterMedia.ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(ColumnNames.FilterMedia.FILTER_NUMBER).AsInt32().NotNullable()
                  .WithColumn(ColumnNames.FilterMedia.EQUIPMENT_IDENTIFIER)
                  .AsAnsiString(StringLengths.FilterMedia.EQUIPMENT_IDENTIFIER).Nullable()
                  .WithColumn(ColumnNames.FilterMedia.YEAR_IN_SERVICE).AsInt32().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.FILTER_TYPE_ID).AsInt32()
                  .ForeignKey(ForeignKeyNames.FilterMedia.FILTER_TYPE, TableNames.FILTER_MEDIA_FILTER_TYPES,
                       ColumnNames.FilterMediaFilterTypes.ID).NotNullable()
                  .WithColumn(ColumnNames.FilterMedia.ESTIMATED_MEDIA_LIFECYCLE).AsInt32().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.CAPACITY_MGD).AsFloat().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.COEFFICIENT).AsAnsiString(StringLengths.FilterMedia.COEFFICIENT)
                  .Nullable()
                  .WithColumn(ColumnNames.FilterMedia.FILTER_DIMENSIONS)
                  .AsAnsiString(StringLengths.FilterMedia.FILTER_DIMENSIONS).Nullable()
                  .WithColumn(ColumnNames.FilterMedia.MEDIA_AREA).AsInt32().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.MEDIA_VOLUME).AsInt32().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.GRAVEL_SUPPORT_MEDIA).AsBoolean().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.MONTHLY_MEDIA_EXPENSE).AsCurrency().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.ANNUAL_INSPECTION_COSTS).AsCurrency().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.ANNUAL_ANALYSIS_COSTS).AsCurrency().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.ANNUAL_COMPANY_LABOR_COSTS).AsCurrency().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.EQUIPMENT_CRITICAL_RATING).AsInt32().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.YEAR_LAST_PAINTED).AsInt32().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.SERVED_BY_STANDBY_POWER).AsBoolean().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.TURBIDIMETER_MODEL)
                  .AsAnsiString(StringLengths.FilterMedia.TURBIDIMETER_MODEL).Nullable()
                  .WithColumn(ColumnNames.FilterMedia.LEVEL_CONTROL_METHOD_ID).AsInt32().ForeignKey(
                       ForeignKeyNames.FilterMedia.LEVEL_CONTROL_METHOD, TableNames.FILTER_MEDIA_LEVEL_CONTROL_METHODS,
                       ColumnNames.FilterMediaLevelControlMethods.ID).Nullable()
                  .WithColumn(ColumnNames.FilterMedia.NOTES).AsAnsiString(StringLengths.FilterMedia.NOTES).Nullable()
                  .WithColumn(ColumnNames.FilterMedia.PRODUCT_CODE).AsAnsiString(StringLengths.FilterMedia.PRODUCT_CODE)
                  .Nullable()
                  .WithColumn(ColumnNames.FilterMedia.ANTHRACITE_DEPTH).AsInt32().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.GAC_DEPTH).AsInt32().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.SAND_DEPTH).AsInt32().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.GRAVEL_DEPTH).AsInt32().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.LAST_TIME_CHANGED).AsDateTime().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.LAST_TIME_CLEANED).AsDateTime().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.AIR_SCOURING).AsBoolean().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.RECYCLING).AsBoolean().Nullable()
                  .WithColumn(ColumnNames.FilterMedia.COMMENT).AsAnsiString(StringLengths.FilterMedia.COMMENT)
                  .Nullable()
                  .WithColumn(ColumnNames.FilterMedia.EQUIPMENT_ID).AsInt32()
                  .ForeignKey(ForeignKeyNames.FilterMedia.EQUIPMENT, "tblEquipment", "RecordId").Nullable()
                  .WithColumn(ColumnNames.FilterMedia.WASH_TYPE_ID).AsInt32()
                  .ForeignKey(ForeignKeyNames.FilterMedia.WASH_TYPE, TableNames.FILTER_MEDIA_WASH_TYPES,
                       ColumnNames.FilterMediaWashTypes.ID).Nullable()
                  .WithColumn(ColumnNames.FilterMedia.MEDIA_TYPE_ID).AsInt32()
                  .ForeignKey(ForeignKeyNames.FilterMedia.MEDIA_TYPE, TableNames.FILTER_MEDIA_TYPES,
                       ColumnNames.FilterMediaTypes.ID).NotNullable()
                  .WithColumn(ColumnNames.FilterMedia.LOCATION_ID).AsInt32()
                  .ForeignKey(ForeignKeyNames.FilterMedia.LOCATION, TableNames.FILTER_MEDIA_LOCATIONS,
                       ColumnNames.FilterMediaLocations.ID).Nullable();
        }

        public override void Down()
        {
            Delete.Table(TableNames.FILTER_MEDIA);
            Delete.Table(TableNames.FILTER_MEDIA_TYPES);
            Delete.Table(TableNames.FILTER_MEDIA_LEVEL_CONTROL_METHODS);
            Delete.Table(TableNames.FILTER_MEDIA_WASH_TYPES);
            Delete.Table(TableNames.FILTER_MEDIA_FILTER_TYPES);
            Delete.Table(TableNames.FILTER_MEDIA_LOCATIONS);
            Delete.PrimaryKey("PK_tblEquipment").FromTable("tblEquipment");
        }
    }
}
