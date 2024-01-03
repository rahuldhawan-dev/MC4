using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130424085509), Tags("Production")]
    public class CreateEquipmentEquipmentTypeForBug1442 : Migration
    {
        #region Constants

        public struct Sql
        {
            public const string TRANSFORM_EQUIPMENT =
                                    @"INSERT INTO [Equipment] ([Identifier],[Description],[TypeID],[Number],[FacilityID],[StatusID],[CriticalRating],[SerialNumber],[YearInstalled],[HasLockoutRequirement],[IsConfinedSpace],[PSMTCPA],[SafetyNotes],[MaintenanceNotes],[OperationNotes])
                                                        SELECT 
	                                                        EquipmentID,
	                                                        isNull(EquipmentDescription,''),
	                                                        (SELECT EquipmentTypeID FROM EquipmentTypes et WHERE et.[Description] = isNull(EquipmentType,'') 
		                                                        AND et.CategoryID = (select EquipmentCategoryID FROM EquipmentCategories WHERE [Description] = isNull(EquipmentCategorization,''))
		                                                        AND et.SubCategoryID = (select EquipmentSubCategoryID FROM EquipmentSubCategories WHERE [Description] = ISNULL(EquipmentSubCategorization, ''))
		                                                        AND et.Abbreviation = EquipmentTypeAbbreviation), --[TypeID],
                                                            EquipmentNumber,
                                                            (SELECT RecordID from tblFacilities F where F.FacilityID = isNull(tblEquipment.FacilityID,'')),
	                                                        (SELECT EquipmentStatusID FROM EquipmentStatuses where [Description] = ISNULL(EquipmentStatus,'')),
                                                            EquipmentCriticalRating,
                                                            SerialNumber,
                                                            YearInstalled,
	                                                        case when (upper(LockoutRequirement) = 'YES') then 1 when (UPPER(LockoutRequirement) = 'NO') then 0 else null end,
                                                            case when (upper(ConfinedSpaceEquipment) = 'YES') then 1 when (UPPER(ConfinedSpaceEquipment) = 'NO') then 0 else null end,
	                                                        PSMTCPA,
	                                                        SafetyNotes,
	                                                        MaintenanceNotes,
	                                                        OperationNotes
                                                        FROM
                                                            tblEquipment",
                                TRANSFORM_EQUIPMENT_CATEGORIES =
                                    "INSERT INTO EquipmentCategories SELECT DISTINCT EquipmentCategorization FROM tblEquipment WHERE EquipmentCategorization IS NOT NULL ORDER BY 1",
                                TRANSFORM_EQUIPMENT_SUB_CATEGORIES =
                                    "INSERT INTO EquipmentSubCategories SELECT DISTINCT EquipmentSubCategorization FROM tblEquipment ORDER BY 1",
                                TRANSFORM_EQUIPMENT_TYPE =
                                    @"INSERT INTO EquipmentTypes(Description, CategoryID, SubCategoryID, Abbreviation)
                                                            SELECT DISTINCT 
	                                                            EquipmentType, 
	                                                            (SELECT EquipmentCategoryID FROM EquipmentCategories WHERE [Description] = isNull(EquipmentCategorization,'')), 
	                                                            (SELECT EquipmentSubCategoryID FROM EquipmentSubCategories WHERE [Description] = ISNULL(EquipmentSubCategorization, '')),	
	                                                            EquipmentTypeAbbreviation
                                                            FROM tblEquipment ORDER BY EquipmentType",
                                TRANSFORM_EQUIPMENT_STATUSES =
                                    "INSERT INTO EquipmentStatuses select distinct EquipmentStatus from tblEquipment where EquipmentStatus is not null",
                                CLEANUP =
                                    @"UPDATE tblEquipment SET EquipmentSubCategorization = RTRIM(ltrim(replace(EquipmentSubCategorization, char(160), ' ')));
                                            UPDATE tblEquipment SET EquipmentSubCategorization = 'Air Pneumatics' WHERE EquipmentSubCategorization IN ('Air Pnuematics', 'Air Pnuemantics');
                                            UPDATE tblEquipment SET EquipmentSubCategorization = 'Flocculation - Clarification' WHERE EquipmentSubCategorization IN ('Flocuclation - Clarification','Floculation - Clarification');
                                            UPDATE tblEquipment SET EquipmentCategorization = RTRIM(ltrim(replace(EquipmentCategorization, char(160), ' ')));
                                            UPDATE tblEquipment SET EquipmentCategorization = NULL WHERE ISNULL(EquipmentCategorization, '') = '';
                                            UPDATE tblEquipment SET EquipmentStatus = 'Out of Service' WHERE EquipmentStatus = 'Not in Service';
                                            UPDATE tblEquipment SET EquipmentStatus = 'In Service' WHERE EquipmentStatus = 'IN SERVICE';
                                            UPDATE tblEquipment SET EquipmentStatus = null WHERE isNull(EquipmentStatus,'') = '';";
        }

        public struct Tables
        {
            public const string EQUIPMENT = "Equipment",
                                EQUIPMENT_TYPES = "EquipmentTypes",
                                EQUIPMENT_STATUSES = "EquipmentStatuses",
                                EQUIPMENT_MANUFACTURERS = "EquipmentManufacturers",
                                EQUIPMENT_MODELS = "EquipmentModels",
                                EQUIPMENT_CATEGORIES = "EquipmentCategories",
                                EQUIPMENT_SUB_CATEGORIES = "EquipmentSubCategories",
                                FACILITIES = "tblFacilities";
        }

        public struct Columns
        {
            public const string EQUIPMENT_ID = "EquipmentID", // PK
                                IDENTIFIER = "Identifier",
                                EQUIPMENT_TYPE_ID = "EquipmentTypeID",
                                TYPE_ID = "TypeID",
                                EQUIPMENT_STATUS_ID = "EquipmentStatusID",
                                STATUS_ID = "StatusID",
                                EQUIPMENT_MANUFACTURER_ID = "EquipmentManufacturerID",
                                MANUFACTURER_ID = "ManufacturerID",
                                EQUIPMENT_MODEL_ID = "EquipmentModelID",
                                MODEL_ID = "ModelID",
                                EQUIPMENT_CATEGORY_ID = "EquipmentCategoryID",
                                CATEGORY_ID = "CategoryID",
                                EQUIPMENT_SUB_CATEGORY_ID = "EquipmentSubCategoryID",
                                SUB_CATEGORY_ID = "SubCategoryID",
                                DESCRIPTION = "Description",
                                ABBREVIATION = "Abbreviation",
                                FACILITY_ID = "FacilityID",
                                FACILITY_PK = "RecordID",
                                NUMBER = "Number",
                                CRITICAL_RATING = "CriticalRating",
                                SERIAL_NUMBER = "SerialNumber",
                                YEAR_INSTALLED = "YearInstalled",
                                HAS_LOCKOUT_REQUIREMENT = "HasLockoutRequirement",
                                IS_CONFINED_SPACE = "IsConfinedSpace",
                                PSMTCPA = "PSMTCPA",
                                SAFETY_NOTES = "SafetyNotes",
                                MAINENANCE_NOTES = "MaintenanceNotes",
                                OPERATION_NOTES = "OperationNotes";
        }

        public struct ForeignKeys
        {
            public const string FK_EQUIPMENT_EQUIPMENT_TYPES = "FK_Equipment_EquipmentTypes_TypeID",
                                FK_EQUIPMENT_FACILITIES =
                                    "FK_Equipment_tblFacilities_FacilityID", //tblFacilities.RecordID 
                                FK_EQUIPMENT_EQUIPMENT_STATUSES = "FK_Equipment_EquipmentStatuses_StatusID",
                                FK_EQUIPMENT_EQUIPMENT_MANUFACTURERS =
                                    "FK_Equipment_EquipmentManufacturers_ManufacturerID",
                                FK_EQUIPMENT_EQUIPMENT_MODELS = "FK_Equipment_EquipmentModels_ModelID",
                                FK_EQUIPMENT_MODELS_EQUIPMENT_MANUFACTURES =
                                    "FK_EquipmentModels_EquipmentManufacturers_ManufacturerID",
                                FK_EQUIPMENT_TYPES_EQUIPMENT_CATEGORIES =
                                    "FK_EquipmentTypes_EquipmentCategories_CategoryID",
                                FK_EQUIPMENT_TYPES_EQUIPMENT_SUB_CATEGORIES =
                                    "FK_EquipmentTypes_EquipmentSubCategories_SubCategoryID";
        }

        public struct StringLengths
        {
            public const int EQUIPMENT_DESCRIPTION = 80,
                             DESCRIPTION = 50,
                             ABBREVIATION = 4,
                             IDENTIFIER = 20,
                             SERIAL_NUMBER = 50;
        }

        #endregion

        public override void Up()
        {
            #region TABLES

            Create.Table(Tables.EQUIPMENT_SUB_CATEGORIES)
                  .WithColumn(Columns.EQUIPMENT_SUB_CATEGORY_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.EQUIPMENT_CATEGORIES)
                  .WithColumn(Columns.EQUIPMENT_CATEGORY_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.EQUIPMENT_STATUSES)
                  .WithColumn(Columns.EQUIPMENT_STATUS_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.EQUIPMENT_MANUFACTURERS)
                  .WithColumn(Columns.EQUIPMENT_MANUFACTURER_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.EQUIPMENT_MODELS)
                  .WithColumn(Columns.EQUIPMENT_MODEL_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable()
                  .WithColumn(Columns.MANUFACTURER_ID).AsInt32().NotNullable();
            Create.Table(Tables.EQUIPMENT_TYPES)
                  .WithColumn(Columns.EQUIPMENT_TYPE_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable()
                  .WithColumn(Columns.ABBREVIATION).AsAnsiString(StringLengths.ABBREVIATION)
                  .WithColumn(Columns.CATEGORY_ID).AsInt32().Nullable()
                  .WithColumn(Columns.SUB_CATEGORY_ID).AsInt32().Nullable();
            Create.Table(Tables.EQUIPMENT)
                  .WithColumn(Columns.EQUIPMENT_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.IDENTIFIER).AsAnsiString(StringLengths.IDENTIFIER).NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.EQUIPMENT_DESCRIPTION).NotNullable()
                  .WithColumn(Columns.TYPE_ID).AsInt32().Nullable()
                  .WithColumn(Columns.NUMBER).AsInt32().Nullable()
                  .WithColumn(Columns.FACILITY_ID).AsInt32().Nullable()
                  .WithColumn(Columns.STATUS_ID).AsInt32().Nullable()
                  .WithColumn(Columns.CRITICAL_RATING).AsInt32().Nullable()
                  .WithColumn(Columns.SERIAL_NUMBER).AsAnsiString(StringLengths.SERIAL_NUMBER).Nullable()
                  .WithColumn(Columns.MANUFACTURER_ID).AsInt32().Nullable()
                  .WithColumn(Columns.MODEL_ID).AsInt32().Nullable()
                  .WithColumn(Columns.YEAR_INSTALLED).AsInt32().Nullable()
                  .WithColumn(Columns.HAS_LOCKOUT_REQUIREMENT).AsBoolean().Nullable()
                  .WithColumn(Columns.IS_CONFINED_SPACE).AsBoolean().Nullable()
                  .WithColumn(Columns.PSMTCPA).AsBoolean().Nullable()
                  .WithColumn(Columns.SAFETY_NOTES).AsCustom("text").Nullable()
                  .WithColumn(Columns.MAINENANCE_NOTES).AsCustom("text").Nullable()
                  .WithColumn(Columns.OPERATION_NOTES).AsCustom("text").Nullable();

            #endregion

            #region FOREIGN KEYS

            Create.ForeignKey(ForeignKeys.FK_EQUIPMENT_EQUIPMENT_MANUFACTURERS)
                  .FromTable(Tables.EQUIPMENT).ForeignColumn(Columns.MANUFACTURER_ID)
                  .ToTable(Tables.EQUIPMENT_MANUFACTURERS).PrimaryColumn(Columns.EQUIPMENT_MANUFACTURER_ID);

            Create.ForeignKey(ForeignKeys.FK_EQUIPMENT_EQUIPMENT_MODELS)
                  .FromTable(Tables.EQUIPMENT).ForeignColumn(Columns.MODEL_ID)
                  .ToTable(Tables.EQUIPMENT_MODELS).PrimaryColumn(Columns.EQUIPMENT_MODEL_ID);

            Create.ForeignKey(ForeignKeys.FK_EQUIPMENT_EQUIPMENT_STATUSES)
                  .FromTable(Tables.EQUIPMENT).ForeignColumn(Columns.STATUS_ID)
                  .ToTable(Tables.EQUIPMENT_STATUSES).PrimaryColumn(Columns.EQUIPMENT_STATUS_ID);

            Create.ForeignKey(ForeignKeys.FK_EQUIPMENT_EQUIPMENT_TYPES)
                  .FromTable(Tables.EQUIPMENT).ForeignColumn(Columns.TYPE_ID)
                  .ToTable(Tables.EQUIPMENT_TYPES).PrimaryColumn(Columns.EQUIPMENT_TYPE_ID);

            Create.ForeignKey(ForeignKeys.FK_EQUIPMENT_FACILITIES)
                  .FromTable(Tables.EQUIPMENT).ForeignColumn(Columns.FACILITY_ID)
                  .ToTable(Tables.FACILITIES).PrimaryColumn(Columns.FACILITY_PK);

            Create.ForeignKey(ForeignKeys.FK_EQUIPMENT_MODELS_EQUIPMENT_MANUFACTURES)
                  .FromTable(Tables.EQUIPMENT_MODELS).ForeignColumn(Columns.MANUFACTURER_ID)
                  .ToTable(Tables.EQUIPMENT_MANUFACTURERS).PrimaryColumn(Columns.EQUIPMENT_MANUFACTURER_ID);

            Create.ForeignKey(ForeignKeys.FK_EQUIPMENT_TYPES_EQUIPMENT_CATEGORIES)
                  .FromTable(Tables.EQUIPMENT_TYPES).ForeignColumn(Columns.CATEGORY_ID)
                  .ToTable(Tables.EQUIPMENT_CATEGORIES).PrimaryColumn(Columns.EQUIPMENT_CATEGORY_ID);

            Create.ForeignKey(ForeignKeys.FK_EQUIPMENT_TYPES_EQUIPMENT_SUB_CATEGORIES)
                  .FromTable(Tables.EQUIPMENT_TYPES).ForeignColumn(Columns.SUB_CATEGORY_ID)
                  .ToTable(Tables.EQUIPMENT_SUB_CATEGORIES).PrimaryColumn(Columns.EQUIPMENT_SUB_CATEGORY_ID);

            #endregion

            // TRANFORM EXISTING DATA
            Execute.Sql(Sql.CLEANUP);
            Execute.Sql(Sql.TRANSFORM_EQUIPMENT_SUB_CATEGORIES);
            Execute.Sql(Sql.TRANSFORM_EQUIPMENT_CATEGORIES);
            Execute.Sql(Sql.TRANSFORM_EQUIPMENT_TYPE);

            Execute.Sql(Sql.TRANSFORM_EQUIPMENT_STATUSES);
            Execute.Sql(Sql.TRANSFORM_EQUIPMENT);
        }

        public override void Down()
        {
            // REMOVE FOREIGN KEYS
            Delete.ForeignKey(ForeignKeys.FK_EQUIPMENT_EQUIPMENT_MANUFACTURERS).OnTable(Tables.EQUIPMENT);
            Delete.ForeignKey(ForeignKeys.FK_EQUIPMENT_EQUIPMENT_MODELS).OnTable(Tables.EQUIPMENT);
            Delete.ForeignKey(ForeignKeys.FK_EQUIPMENT_EQUIPMENT_STATUSES).OnTable(Tables.EQUIPMENT);
            Delete.ForeignKey(ForeignKeys.FK_EQUIPMENT_EQUIPMENT_TYPES).OnTable(Tables.EQUIPMENT);
            Delete.ForeignKey(ForeignKeys.FK_EQUIPMENT_FACILITIES).OnTable(Tables.EQUIPMENT);
            Delete.ForeignKey(ForeignKeys.FK_EQUIPMENT_MODELS_EQUIPMENT_MANUFACTURES).OnTable(Tables.EQUIPMENT_MODELS);
            Delete.ForeignKey(ForeignKeys.FK_EQUIPMENT_TYPES_EQUIPMENT_CATEGORIES).OnTable(Tables.EQUIPMENT_TYPES);
            Delete.ForeignKey(ForeignKeys.FK_EQUIPMENT_TYPES_EQUIPMENT_SUB_CATEGORIES).OnTable(Tables.EQUIPMENT_TYPES);

            // DELETE TABLES
            Delete.Table(Tables.EQUIPMENT_SUB_CATEGORIES);
            Delete.Table(Tables.EQUIPMENT_CATEGORIES);
            Delete.Table(Tables.EQUIPMENT_TYPES);
            Delete.Table(Tables.EQUIPMENT_STATUSES);
            Delete.Table(Tables.EQUIPMENT_MODELS);
            Delete.Table(Tables.EQUIPMENT_MANUFACTURERS);
            Delete.Table(Tables.EQUIPMENT);
        }
    }
}
