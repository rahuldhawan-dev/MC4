using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140130102101), Tags("Production")]
    public class UpdateEnvironmentalPermitTablesForBug1627 : Migration
    {
        // NOTE: Permits table has been renamed EnvironmentPermits, throughout the migration it is still refered to as Permits/PermitID 

        #region Constants

        public struct Sql
        {
            public const string
                UPDATE_ALLOCATION_PERMIT_DETAILS_PERMIT_ID =
                    " UPDATE [" + Tables.ALLOCATION_PERMITS + "] SET [" + Columns.PERMIT_ID + "] = (SELECT TOP 1 " +
                    Columns.PERMIT_ID +
                    " FROM " + Tables.PERMITS + " P WHERE P.[" + Columns.PERMIT_NUMBER + "] = [" +
                    Columns.SUB_PERMIT_NUMBER + "] " +
                    " AND P.OpCode = [" + Tables.ALLOCATION_PERMITS + "].OpCode ORDER BY " + Columns.PERMIT_ID +
                    " DESC)",
                ROLLBACK_ALLOCATION_PERMIT_DETAILS_PERMIT_ID =
                    " UPDATE [" + Tables.ALLOCATION_PERMITS + "] SET [" + Columns.SUB_PERMIT_NUMBER +
                    "] = (SELECT TOP 1 " + Columns.PERMIT_NUMBER +
                    " FROM " + Tables.PERMITS + " P WHERE P." + Columns.PERMIT_ID + " = " + Columns.PERMIT_ID +
                    " AND P.OpCode = [" + Tables.ALLOCATION_PERMITS + "].OpCode ORDER BY " + Columns.PERMIT_ID +
                    " DESC)",
                UPDATE_ALLOCATION_PERMITS_OPERATING_CENTER_ID =
                    " UPDATE [" + Tables.ALLOCATION_PERMITS + "] SET [" + Columns.OPERATING_CENTER_ID +
                    "] = (SELECT TOP 1 [" + Columns.OPERATING_CENTER_ID + "]" +
                    " FROM [" + Tables.OPERATING_CENTERS + "] OC WHERE OC.[" + Columns.OPERATING_CENTER_CODE + "] = " +
                    " [" + Tables.ALLOCATION_PERMITS + "].[" + Columns.OP_CODE + "])",
                ROLLBACK_ALLOCATION_PERMITS_OPERATING_CENTER_ID =
                    " UPDATE [" + Tables.ALLOCATION_PERMITS + "] SET [" + Columns.OP_CODE + "] = (SELECT TOP 1 [" +
                    Columns.OPERATING_CENTER_CODE + "]" +
                    " FROM [" + Tables.OPERATING_CENTERS + "] OC WHERE OC.[" + Columns.OPERATING_CENTER_ID + "] = " +
                    " [" + Tables.ALLOCATION_PERMITS + "].[" + Columns.OPERATING_CENTER_ID + "])",
                INSERT_PERMITS_OPERATING_CENTER_ID =
                    "INSERT INTO [" + Tables.ENVIRONMENTAL_PERMITS_OPERATING_CENTERS + "]([" +
                    Columns.OPERATING_CENTER_ID + "],[" + Columns.PERMIT_ID + "]) " +
                    "SELECT (SELECT " + Columns.OPERATING_CENTER_ID + " from OperatingCenters where " +
                    Columns.OPERATING_CENTER_CODE + " = " + Columns.OP_CODE + "),[" + Columns.PERMIT_ID + "] " +
                    "FROM [" + Tables.PERMITS + "] WHERE (SELECT " + Columns.OPERATING_CENTER_ID +
                    " from OperatingCenters where " + Columns.OPERATING_CENTER_CODE + " = " + Columns.OP_CODE +
                    ") IS NOT NULL",
                ROLLBACK_PERMITS_OPERATING_CENTER_ID = "UPDATE [" + Tables.PERMITS + "] " +
                                                       "SET [" + Columns.OP_CODE + "] = (SELECT " +
                                                       Columns.OPERATING_CENTER_CODE + " from " +
                                                       Tables.OPERATING_CENTERS + " OC where " +
                                                       Columns.OPERATING_CENTER_ID + " = " +
                                                       "(SELECT TOP 1  " + Columns.OPERATING_CENTER_ID + " from " +
                                                       Tables.ENVIRONMENTAL_PERMITS_OPERATING_CENTERS + " epoc " +
                                                       "where epoc." + Columns.PERMIT_ID + " = ep." +
                                                       Columns.PERMIT_ID + ")) FROM " + Tables.PERMITS + " ep",
                INSERT_ENVIRONMENTAL_PERMIT_TYPES = "INSERT INTO [" + Tables.ENVIRONMENTAL_PERMIT_TYPES +
                                                    "] SELECT LookupValue FROM [" + Tables.LOOKUPS +
                                                    "] WHERE LookupType = 'Permit_Type'",
                INSERT_ENVIRONMENTAL_PERMIT_STATUSES = "INSERT INTO [" + Tables.ENVIRONMENTAL_PERMIT_STATUSES +
                                                       "] SELECT LookupValue FROM [" + Tables.LOOKUPS +
                                                       "] WHERE LookupType = 'Permit_Status'",
                UPDATE_PERMIT_TYPES = " UPDATE " + Tables.PERMITS + " SET " + Columns.PERMIT_TYPE + " = " +
                                      " (SELECT [" + Columns.PERMIT_TYPE + "] FROM [" +
                                      Tables.ENVIRONMENTAL_PERMIT_TYPES + "] " +
                                      "  WHERE " + Columns.DESCRIPTION +
                                      " = (SELECT LookupValue from LOOKUP where LookupID = [" + Tables.PERMITS + "].[" +
                                      Columns.PERMIT_TYPE + "]))",
                UPDATE_PERMIT_STATUSES = " UPDATE " + Tables.PERMITS + " SET " + Columns.PERMIT_STATUS + " = " +
                                         " (SELECT [" + Columns.PERMIT_STATUS + "] FROM [" +
                                         Tables.ENVIRONMENTAL_PERMIT_STATUSES + "] " +
                                         "  WHERE " + Columns.DESCRIPTION +
                                         " = (SELECT LookupValue FROM Lookup WHERE LookupID = [" + Tables.PERMITS +
                                         "].[" + Columns.PERMIT_STATUS + "]))",
                ROLLBACK_PERMIT_TYPES =
                    " UPDATE " + Tables.PERMITS + " SET " + Columns.PERMIT_TYPE + " = " +
                    " (SELECT [LookupID] FROM [" + Tables.LOOKUPS + "] WHERE LookupType = 'Permit_Type' AND " +
                    " [LookupValue] = (SELECT Description from " + Tables.ENVIRONMENTAL_PERMIT_TYPES + " WHERE " +
                    Columns.PERMIT_TYPE + " = [" + Tables.PERMITS + "].[" + Columns.PERMIT_TYPE + "]))",
                ROLLBACK_PERMIT_STATUSES =
                    " UPDATE " + Tables.PERMITS + " SET " + Columns.PERMIT_STATUS + " = " +
                    " (SELECT [LookupID] FROM [" + Tables.LOOKUPS + "] WHERE LookupType = 'Permit_Status' AND " +
                    " [LookupValue] = (SELECT Description from " + Tables.ENVIRONMENTAL_PERMIT_STATUSES + " WHERE " +
                    Columns.PERMIT_STATUS + " = [" + Tables.PERMITS + "].[" + Columns.PERMIT_STATUS + "]))",
                UPDATE_PERMITS_PWSID =
                    "UPDATE [" + Tables.PERMITS + "] SET [" + Columns.PUBLIC_WATER_SUPPLY_ID + "] = " +
                    "(SELECT TOP 1 [" + Columns.PUBLIC_WATER_SUPPLY_PUBLIC_WATER_SUPPLY_ID + "] " +
                    "FROM [" + Tables.PUBLIC_WATER_SUPPLIES + "] WHERE [" + Tables.PUBLIC_WATER_SUPPLIES +
                    "].[PWSID] = " +
                    "'NJ' + RIGHT('0000000' + EnvironmentalPermits." + Columns.PUBLIC_WATER_SUPPLY_ID + ", 7))",
                //UPDATE_ALLOCATION_PERMITS_PWSID     = "", // already correct
                ROLLBACK_PERMITS_PWSID =
                    "UPDATE [" + Tables.PERMITS + "] SET [" + Columns.PUBLIC_WATER_SUPPLY_ID + "] = " +
                    "(SELECT [PWSID] " +
                    "FROM [" + Tables.PUBLIC_WATER_SUPPLIES + "] WHERE " +
                    "[" + Tables.PUBLIC_WATER_SUPPLIES + "].[" + Columns.PUBLIC_WATER_SUPPLY_PUBLIC_WATER_SUPPLY_ID +
                    "]" +
                    " = " + Columns.PUBLIC_WATER_SUPPLY_ID + ")",
                //ROLLBACK_ALLOCATION_PERMITS_PWSID   = "", // already correct
                UPDATE_SURFACE_SUPPLY =
                    "UPDATE [" + Tables.ALLOCATION_PERMITS +
                    "] SET SurfaceSupply = 1 WHERE IsNull(SurfaceSupply,'') = 'YES';" +
                    "UPDATE [" + Tables.ALLOCATION_PERMITS +
                    "] SET SurfaceSupply = 0 WHERE IsNull(SurfaceSupply,'') <> '1';",
                UPDATE_GROUND_SUPPLY =
                    "UPDATE [" + Tables.ALLOCATION_PERMITS +
                    "] SET GroundSupply = 1 WHERE IsNull(GroundSupply,'') = 'YES';" +
                    "UPDATE [" + Tables.ALLOCATION_PERMITS +
                    "] SET GroundSupply = 0 WHERE IsNull(GroundSupply,'') <> '1';",
                UPDATE_ACTIVE_PERMIT =
                    "UPDATE [" + Tables.ALLOCATION_PERMITS +
                    "] SET ActivePermit = 1 WHERE IsNull(ActivePermit,'') = 'True' OR IsNull(ActivePermit,'') = '1';" +
                    "UPDATE [" + Tables.ALLOCATION_PERMITS +
                    "] SET ActivePermit = 0 WHERE IsNull(ActivePermit,'') <> '1';",
                ROLLBACK_SURFACE_SUPPLY =
                    "UPDATE [" + Tables.ALLOCATION_PERMITS +
                    "] SET SurfaceSupply = 'YES' WHERE IsNull(SurfaceSupply,'') = '1';" +
                    "UPDATE [" + Tables.ALLOCATION_PERMITS +
                    "] SET SurfaceSupply = 'NO' WHERE IsNull(SurfaceSupply,'') <> 'YES';",
                ROLLBACK_GROUND_SUPPLY =
                    "UPDATE [" + Tables.ALLOCATION_PERMITS +
                    "] SET GroundSupply = 'YES' WHERE IsNull(GroundSupply,'') = '1';" +
                    "UPDATE [" + Tables.ALLOCATION_PERMITS +
                    "] SET GroundSupply = 'NO' WHERE IsNull(GroundSupply,'') <> 'YES';",
                ROLLBACK_ACTIVE_PERMIT =
                    "UPDATE [" + Tables.ALLOCATION_PERMITS +
                    "] SET ActivePermit = 'True' WHERE IsNull(ActivePermit,'') = '1';" +
                    "UPDATE [" + Tables.ALLOCATION_PERMITS +
                    "] SET ActivePermit = 'False' WHERE IsNull(ActivePermit,'') <> 'True';",
                CLEAN_UP_MANY_TO_MANY =
                    "DELETE FROM tblPermitFacility where FacilityID not in (SElect RecordID from tblFacilities);" +
                    "DELETE FROM tblPermitFacility where PermitId not in (Select RecordID from tblPermit)",
                UPDATE_DATA_TYPE =
                    "UPDATE DataType SET Data_Type = 'Environmental Permit', Table_Name = 'EnvironmentalPermits' where DataTypeID = 11;" +
                    "UPDATE DataType SET Data_Type = 'Allocation Permit', Table_Name = 'AllocationPermits' where DataTypeID = 35;",
                ROLLBACK_DATA_TYPE =
                    "UPDATE DataType SET Data_Type = 'Permit', Table_Name = 'tblPermit' where DataTypeID = 11;" +
                    "UPDATE DataType SET Data_Type = 'AllocationPermitsMaster', Table_Name = 'PFM_PermitsAllocation' where DataTypeID = 35;",
                INSERT_LOOKUP_VALUES = "INSERT INTO " + Tables.ALLOCATION_CATEGORIES +
                                       "(Description) Values('Surface Water');" +
                                       "INSERT INTO " + Tables.ALLOCATION_CATEGORIES +
                                       "(Description) Values('Ground Water');" +
                                       "INSERT INTO " + Tables.ALLOCATION_CATEGORIES + "(Description) Values('ASR');";
        }

        public struct OldTables
        {
            public const string PERMITS = "tblPermit",
                                ALLOCATION_PERMITS = "PFM_PermitsAllocation",
                                ENVIRONMENTAL_PERMITS_FACILITIES = "tblPermitFacility",
                                ENVIRONMENTAL_PERMITS_EQUIPMENT = "tblPermitEquipment";
        }

        public struct Tables
        {
            public const string PERMITS = "EnvironmentalPermits",
                                ALLOCATION_PERMITS = "AllocationPermits",
                                ALLOCATION_PERMIT_WITHDRAWAL_NODES = "AllocationPermitWithdrawalNodes",
                                ALLOCATION_CATEGORIES = "AllocationCategories",
                                ENVIRONMENTAL_PERMITS_OPERATING_CENTERS = "EnvironmentalPermitsOperatingCenters",
                                FACILITIES = "tblFacilities",
                                COORDINATES = "Coordinates",
                                OPERATING_CENTERS = "OperatingCenters",
                                ENVIRONMENTAL_PERMIT_TYPES = "EnvironmentalPermitTypes",
                                ENVIRONMENTAL_PERMIT_STATUSES = "EnvironmentalPermitStatuses",
                                LOOKUPS = "Lookup",
                                PUBLIC_WATER_SUPPLIES = "tblPWSID",
                                ENVIRONMENTAL_PERMITS_FACILITIES = "EnvironmentalPermitsFacilities",
                                ENVIRONMENTAL_PERMITS_EQUIPMENT = "EnvironmentalPermitsEquipment",
                                EQUIPMENT = "Equipment";
        }

        public struct OldColumns
        {
            public const string PERMIT_ID = "RecordID",
                                PERMIT_ID_PERMIT_ID = "PermitID",
                                PERMIT_NUMBER = "Permit_Number",
                                ALLOCATION_PERMIT_ID = "RecordID",
                                ALLOCATION_PERMIT_WITHDRAWAL_NODE_ID = "AllocationPermitWithdrawalNodeID",
                                ALLOCATION_CATEGORY = "AllocationCategoryID",
                                PROGRAM_INTEREST_NUMBER = "Program_Interest_Number",
                                PERMIT_CROSS_REFERENCE_NUMBER = "Permit_Cross_Reference_Number",
                                PERMIT_EFFECTIVE_DATE = "Permit_Effective_Date",
                                PERMIT_RENEWAL_DATE = "Permit_Renewal_Date",
                                PERMIT_EXPIRATION_DATE = "Permit_Expiration_Date",
                                ANNUAL_FEE = "Annual_Fee",
                                PERMIT_TYPE = "Permit_Type",
                                PERMIT_STATUS = "Permit_Status",
                                PUBLIC_WATER_SUPPLY_ID = "PWSID",
                                PERMITS_EQUIPMENT_ID = "ID",
                                PERMITS_FACILITY_ID = "ID";
        }

        public struct Columns
        {
            public const string PERMIT_ID = "EnvironmentalPermitID",
                                PERMIT_NUMBER = "PermitNumber",
                                ALLOCATION_PERMIT_ID = "AllocationPermitID",
                                SUB_PERMIT_NUMBER = "SubPermitNumber",
                                GPM = "GPM",
                                ALLOCATION_CATEGORY_ID = "AllocationCategoryID",
                                DESCRIPTION = "Description",
                                ALLOCATION_PERMIT_WITHDRAWAL_NODE_ID = "AllocationPermitWithdrawalNodeID",
                                WELL_PERMIT_NUMBER = "WellPermitNumber",
                                FACILITY_RECORD_ID = "RecordID",
                                FACILITY_ID = "FacilityID",
                                COORDINATE_ID = "CoordinateID",
                                ALLOWABLE_GPM = "AllowableGPM",
                                ALLOWABLE_GPD = "AllowableGPD",
                                ALLOWABLE_MGM = "AllowableMGM",
                                CAPABLE_GPM = "CapableGPM",
                                WITHDRAWAL_CONSTRAINT = "WithdrawalConstraint",
                                HAS_STAND_BY_POWER = "HasStandByPower",
                                OPERATING_CENTER_ID = "OperatingCenterID",
                                OPERATING_CENTER_CODE = "OperatingCenterCode",
                                OP_CODE = "OpCode",
                                PROGRAM_INTEREST_NUMBER = "ProgramInterestNumber",
                                PERMIT_CROSS_REFERENCE_NUMBER = "PermitCrossReferenceNumber",
                                PERMIT_EFFECTIVE_DATE = "PermitEffectiveDate",
                                PERMIT_RENEWAL_DATE = "PermitRenewalDate",
                                PERMIT_EXPIRATION_DATE = "PermitExpirationDate",
                                ANNUAL_FEE = "AnnualFee",
                                PERMIT_TYPE = "EnvironmentalPermitTypeID",
                                PERMIT_STATUS = "EnvironmentalPermitStatusID",
                                PUBLIC_WATER_SUPPLY_ID = "PublicWaterSupplyID",
                                PUBLIC_WATER_SUPPLY_PUBLIC_WATER_SUPPLY_ID = "RecordID",
                                SURFACE_SUPPLY = "SurfaceSupply",
                                GROUND_SUPPLY = "GroundSupply",
                                ACTIVE_PERMIT = "ActivePermit",
                                EQUIPMENT_ID = "EquipmentID",
                                CAPACITY_UNDER_STANDBY_POWER = "CapacityUnderStandbyPower";
        }

        public struct StringLengths
        {
            public const int SUB_PERMIT_NUMBER = 30,
                             WELL_PERMIT_NUMBER = 25,
                             DESCRIPTION = 50,
                             OP_CODE = 10,
                             SURFACE_SUPPLY = 64,
                             GROUND_SUPPLY = 64,
                             ACTIVE_PERMIT = 64,
                             PWSID = 50,
                             SYSTEM = 50,
                             GEOLOGICAL_FORMATION = 64,
                             SUB_ALLOCATION_NUMBER = 50,
                             SOURCE_DESCRIPTION = 100,
                             SOURCE_RESTRICTIONS = 255,
                             NOTES = 4000,
                             PERMIT_TYPE = 50;
        }

        public struct ForeignKeys
        {
            //coordinateid, categoryid, allocation permitID, facilityID
            public const string FK_ALLOCATION_PERMITS_PERMITS =
                                    "FK_AllocationPermits_EnvironmentalPermits_EnvironmentalPermitID",
                                FK_ALLOCATION_PERMITS_OPERATING_CENTERS =
                                    "FK_AllocationPermits_OperatingCenters_OperatingCenterID",
                                FK_ALLOCATION_PERMIT_WITHDRAWAL_NODES_ALLOCATION_PERMITS =
                                    "FK_AllocationPermitWithdrawalNodes_AllocationPermits_AllocationPermitID",
                                FK_ALLOCATION_PERMIT_WITHDRAWAL_NODES_CATEGORIES =
                                    "FK_AllocationPermitWithdrawalNodes_AllocationCategories_AllocationCategoryID",
                                FK_ALLOCATION_PERMIT_WITHDRAWAL_NODES_FACILITIES =
                                    "FK_AllocationPermitWithdrawalNodes_tblFacilities_FacilityID",
                                FK_ALLOCATION_PERMIT_WITHDRAWAL_NODES_COORDINATES =
                                    "FK_AllocationPermitWithdrawalNodes_Coordinates_CoordinateID",
                                FK_ENVIRONMENTAL_PERMITS_OPERATING_CENTERS_PERMITS =
                                    "FK_EnvironmentalPermitsOperatingCenters_EnvironmentalPermits_EnvironmentalPermitID",
                                FK_ENVIRONMENTAL_PERMITS_OPERATING_CENTERS_OPERATING_CENTERS =
                                    "FK_EnvironmentalPermitsOperatingCenters_OperatingCenters_OperatingCenterID",
                                FK_PERMITS_PERMIT_TYPES =
                                    "FK_EnvironmentalPermits_EnvironmentalPermitTypes_EnvironmentalPermitTypeID",
                                FK_PERMITS_PERMIT_STATUSES =
                                    "FK_EnvironmentalPermits_EnvironmentalPermitStatuses_EnvironmentalPermitStatusID",
                                FK_PERMITS_PWSIDS = "FK_EnvironmentalPermits_tblPWSID_PublicWaterSupplyID",
                                FK_PERMITS_FACILITIES_PERMITS =
                                    "FK_EnvironmentalPermitsFacilities_EnvironmentalPermits_EnvironmentalPermitID",
                                FK_PERMITS_FACILITIES_FACILITIES =
                                    "FK_EnvironmentalPermitsFacilities_tblFacilities_FacilityID",
                                FK_PERMITS_EQUIPMENT_PERMITS =
                                    "FK_EnvironmentalPermitsEquipment_EnvironmentalPermits_EnvironmentalPermitID",
                                FK_PERMITS_EQUIPMENT_EQUIPMENT =
                                    "FK_EnvironmentalPermitsEquipment_Equipment_EquipmentID";
        }

        #endregion

        public override void Up()
        {
            #region Clean Up

            Execute.Sql(Sql.CLEAN_UP_MANY_TO_MANY);

            #endregion

            #region Renames

            Rename.Table(OldTables.PERMITS).To(Tables.PERMITS);
            Rename.Column(OldColumns.PERMIT_ID).OnTable(Tables.PERMITS).To(Columns.PERMIT_ID);
            Rename.Column(OldColumns.PERMIT_NUMBER).OnTable(Tables.PERMITS).To(Columns.PERMIT_NUMBER);
            Rename.Column(OldColumns.PROGRAM_INTEREST_NUMBER).OnTable(Tables.PERMITS)
                  .To(Columns.PROGRAM_INTEREST_NUMBER);
            Rename.Column(OldColumns.PERMIT_CROSS_REFERENCE_NUMBER).OnTable(Tables.PERMITS)
                  .To(Columns.PERMIT_CROSS_REFERENCE_NUMBER);
            Rename.Column(OldColumns.PERMIT_EFFECTIVE_DATE).OnTable(Tables.PERMITS).To(Columns.PERMIT_EFFECTIVE_DATE);
            Rename.Column(OldColumns.PERMIT_RENEWAL_DATE).OnTable(Tables.PERMITS).To(Columns.PERMIT_RENEWAL_DATE);
            Rename.Column(OldColumns.PERMIT_EXPIRATION_DATE).OnTable(Tables.PERMITS).To(Columns.PERMIT_EXPIRATION_DATE);
            Rename.Column(OldColumns.ANNUAL_FEE).OnTable(Tables.PERMITS).To(Columns.ANNUAL_FEE);
            Rename.Column(OldColumns.PERMIT_TYPE).OnTable(Tables.PERMITS).To(Columns.PERMIT_TYPE);
            Rename.Column(OldColumns.PERMIT_STATUS).OnTable(Tables.PERMITS).To(Columns.PERMIT_STATUS);
            Rename.Column(OldColumns.PUBLIC_WATER_SUPPLY_ID).OnTable(Tables.PERMITS).To(Columns.PUBLIC_WATER_SUPPLY_ID);

            Rename.Table(OldTables.ALLOCATION_PERMITS).To(Tables.ALLOCATION_PERMITS);
            Rename.Column(OldColumns.ALLOCATION_PERMIT_ID).OnTable(Tables.ALLOCATION_PERMITS)
                  .To(Columns.ALLOCATION_PERMIT_ID);

            Rename.Table(OldTables.ENVIRONMENTAL_PERMITS_EQUIPMENT).To(Tables.ENVIRONMENTAL_PERMITS_EQUIPMENT);
            Rename.Column(OldColumns.PERMIT_ID_PERMIT_ID).OnTable(Tables.ENVIRONMENTAL_PERMITS_EQUIPMENT)
                  .To(Columns.PERMIT_ID);

            Rename.Table(OldTables.ENVIRONMENTAL_PERMITS_FACILITIES).To(Tables.ENVIRONMENTAL_PERMITS_FACILITIES);
            Rename.Column(OldColumns.PERMIT_ID_PERMIT_ID).OnTable(Tables.ENVIRONMENTAL_PERMITS_FACILITIES)
                  .To(Columns.PERMIT_ID);

            Rename.Column(OldColumns.PUBLIC_WATER_SUPPLY_ID).OnTable(Tables.ALLOCATION_PERMITS)
                  .To(Columns.PUBLIC_WATER_SUPPLY_ID);

            #endregion

            #region Lookups

            Create.Table(Tables.ALLOCATION_CATEGORIES)
                  .WithColumn(Columns.ALLOCATION_CATEGORY_ID).AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Description").AsString(StringLengths.DESCRIPTION).NotNullable().Unique();
            Create.Table(Tables.ENVIRONMENTAL_PERMIT_TYPES)
                  .WithColumn(Columns.PERMIT_TYPE).AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Description").AsString(StringLengths.DESCRIPTION).NotNullable().Unique();
            Execute.Sql(Sql.INSERT_ENVIRONMENTAL_PERMIT_TYPES);
            Create.Table(Tables.ENVIRONMENTAL_PERMIT_STATUSES)
                  .WithColumn(Columns.PERMIT_STATUS).AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Description").AsString(StringLengths.DESCRIPTION).NotNullable().Unique();
            Execute.Sql(Sql.INSERT_ENVIRONMENTAL_PERMIT_STATUSES);

            #endregion

            #region AllocationPermitDetails

            Alter.Table(Tables.ALLOCATION_PERMITS).AddColumn(Columns.PERMIT_ID).AsInt32().Nullable();
            Execute.Sql(Sql.UPDATE_ALLOCATION_PERMIT_DETAILS_PERMIT_ID);

            Delete.Column(Columns.SUB_PERMIT_NUMBER).FromTable(Tables.ALLOCATION_PERMITS);
            Alter.Table(Tables.ALLOCATION_PERMITS).AddColumn(Columns.GPM).AsDecimal(18, 2).Nullable();

            Alter.Table(Tables.ALLOCATION_PERMITS).AddColumn(Columns.OPERATING_CENTER_ID).AsInt32().Nullable();
            Execute.Sql(Sql.UPDATE_ALLOCATION_PERMITS_OPERATING_CENTER_ID);
            Delete.Column(Columns.OP_CODE).FromTable(Tables.ALLOCATION_PERMITS);

            Execute.Sql(Sql.UPDATE_SURFACE_SUPPLY);
            Execute.Sql(Sql.UPDATE_GROUND_SUPPLY);
            Execute.Sql(Sql.UPDATE_ACTIVE_PERMIT);
            Alter.Table(Tables.ALLOCATION_PERMITS).AlterColumn(Columns.SURFACE_SUPPLY).AsBoolean().Nullable();
            Alter.Table(Tables.ALLOCATION_PERMITS).AlterColumn(Columns.GROUND_SUPPLY).AsBoolean().Nullable();
            Alter.Table(Tables.ALLOCATION_PERMITS).AlterColumn(Columns.ACTIVE_PERMIT).AsBoolean().Nullable();

            #endregion

            #region Create AllocationPermitWithdrawalNodes

            Create.Table(Tables.ALLOCATION_PERMIT_WITHDRAWAL_NODES)
                  .WithColumn(Columns.ALLOCATION_PERMIT_WITHDRAWAL_NODE_ID).AsInt32().NotNullable().PrimaryKey()
                  .Identity()
                  .WithColumn(Columns.ALLOCATION_PERMIT_ID).AsInt32().NotNullable()
                  .WithColumn(Columns.ALLOCATION_CATEGORY_ID).AsInt32().Nullable()
                  .WithColumn(Columns.WELL_PERMIT_NUMBER).AsAnsiString(StringLengths.WELL_PERMIT_NUMBER).Nullable()
                  .WithColumn(Columns.FACILITY_ID).AsInt32().Nullable()
                  .WithColumn(Columns.DESCRIPTION).AsCustom("ntext").Nullable()
                  .WithColumn(Columns.COORDINATE_ID).AsInt32().Nullable()
                  .WithColumn(Columns.ALLOWABLE_GPM).AsDecimal(18, 2).Nullable()
                  .WithColumn(Columns.ALLOWABLE_GPD).AsDecimal(18, 2).Nullable()
                  .WithColumn(Columns.ALLOWABLE_MGM).AsDecimal(18, 2).Nullable()
                  .WithColumn(Columns.CAPABLE_GPM).AsDecimal(18, 2).Nullable()
                  .WithColumn(Columns.WITHDRAWAL_CONSTRAINT).AsCustom("ntext").Nullable()
                  .WithColumn(Columns.HAS_STAND_BY_POWER).AsBoolean().Nullable()
                  .WithColumn(Columns.CAPACITY_UNDER_STANDBY_POWER).AsDecimal(18, 2).Nullable();

            #endregion

            #region Environmental Permits

            Create.Table(Tables.ENVIRONMENTAL_PERMITS_OPERATING_CENTERS)
                  .WithColumn(Columns.PERMIT_ID).AsInt32().NotNullable()
                  .WithColumn(Columns.OPERATING_CENTER_ID).AsInt32().NotNullable();

            Execute.Sql(Sql.INSERT_PERMITS_OPERATING_CENTER_ID);

            Delete.Column(Columns.OP_CODE).FromTable(Tables.PERMITS);

            Execute.Sql(Sql.UPDATE_PERMIT_STATUSES);
            Execute.Sql(Sql.UPDATE_PERMIT_TYPES);

            Execute.Sql(Sql.UPDATE_PERMITS_PWSID);

            Alter.Table(Tables.PERMITS).AlterColumn(Columns.PUBLIC_WATER_SUPPLY_ID).AsInt32().Nullable();

            Execute.Sql("ALTER TABLE [dbo].[" + Tables.ENVIRONMENTAL_PERMITS_EQUIPMENT +
                        "] DROP CONSTRAINT [PK_tblPermitEquipment]");
            Execute.Sql("ALTER TABLE [dbo].[" + Tables.ENVIRONMENTAL_PERMITS_FACILITIES +
                        "] DROP CONSTRAINT [PK_tblPermitFacility]");

            Delete.Column(OldColumns.PERMITS_EQUIPMENT_ID).FromTable(Tables.ENVIRONMENTAL_PERMITS_EQUIPMENT);
            Delete.Column(OldColumns.PERMITS_FACILITY_ID).FromTable(Tables.ENVIRONMENTAL_PERMITS_FACILITIES);

            #endregion

            #region Foreign Keys

            Create.ForeignKey(ForeignKeys.FK_ALLOCATION_PERMITS_PERMITS)
                  .FromTable(Tables.ALLOCATION_PERMITS).ForeignColumn(Columns.PERMIT_ID)
                  .ToTable(Tables.PERMITS).PrimaryColumn(Columns.PERMIT_ID);
            Create.ForeignKey(ForeignKeys.FK_ALLOCATION_PERMIT_WITHDRAWAL_NODES_ALLOCATION_PERMITS)
                  .FromTable(Tables.ALLOCATION_PERMIT_WITHDRAWAL_NODES).ForeignColumn(Columns.ALLOCATION_PERMIT_ID)
                  .ToTable(Tables.ALLOCATION_PERMITS).PrimaryColumn(Columns.ALLOCATION_PERMIT_ID);
            Create.ForeignKey(ForeignKeys.FK_ALLOCATION_PERMIT_WITHDRAWAL_NODES_CATEGORIES)
                  .FromTable(Tables.ALLOCATION_PERMIT_WITHDRAWAL_NODES).ForeignColumn(Columns.ALLOCATION_CATEGORY_ID)
                  .ToTable(Tables.ALLOCATION_CATEGORIES).PrimaryColumn(Columns.ALLOCATION_CATEGORY_ID);
            Create.ForeignKey(ForeignKeys.FK_ALLOCATION_PERMIT_WITHDRAWAL_NODES_FACILITIES)
                  .FromTable(Tables.ALLOCATION_PERMIT_WITHDRAWAL_NODES).ForeignColumn(Columns.FACILITY_ID)
                  .ToTable(Tables.FACILITIES).PrimaryColumn(Columns.FACILITY_RECORD_ID);
            Create.ForeignKey(ForeignKeys.FK_ALLOCATION_PERMIT_WITHDRAWAL_NODES_COORDINATES)
                  .FromTable(Tables.ALLOCATION_PERMIT_WITHDRAWAL_NODES).ForeignColumn(Columns.COORDINATE_ID)
                  .ToTable(Tables.COORDINATES).PrimaryColumn(Columns.COORDINATE_ID);
            Create.ForeignKey(ForeignKeys.FK_ENVIRONMENTAL_PERMITS_OPERATING_CENTERS_PERMITS)
                  .FromTable(Tables.ENVIRONMENTAL_PERMITS_OPERATING_CENTERS).ForeignColumn(Columns.PERMIT_ID)
                  .ToTable(Tables.PERMITS).PrimaryColumn(Columns.PERMIT_ID);
            Create.ForeignKey(ForeignKeys.FK_ENVIRONMENTAL_PERMITS_OPERATING_CENTERS_OPERATING_CENTERS)
                  .FromTable(Tables.ENVIRONMENTAL_PERMITS_OPERATING_CENTERS).ForeignColumn(Columns.OPERATING_CENTER_ID)
                  .ToTable(Tables.OPERATING_CENTERS).PrimaryColumn(Columns.OPERATING_CENTER_ID);
            Create.ForeignKey(ForeignKeys.FK_ALLOCATION_PERMITS_OPERATING_CENTERS)
                  .FromTable(Tables.ALLOCATION_PERMITS).ForeignColumn(Columns.OPERATING_CENTER_ID)
                  .ToTable(Tables.OPERATING_CENTERS).PrimaryColumn(Columns.OPERATING_CENTER_ID);
            Create.ForeignKey(ForeignKeys.FK_PERMITS_PERMIT_STATUSES)
                  .FromTable(Tables.PERMITS).ForeignColumn(Columns.PERMIT_STATUS)
                  .ToTable(Tables.ENVIRONMENTAL_PERMIT_STATUSES).PrimaryColumn(Columns.PERMIT_STATUS);
            Create.ForeignKey(ForeignKeys.FK_PERMITS_PERMIT_TYPES)
                  .FromTable(Tables.PERMITS).ForeignColumn(Columns.PERMIT_TYPE)
                  .ToTable(Tables.ENVIRONMENTAL_PERMIT_TYPES).PrimaryColumn(Columns.PERMIT_TYPE);
            Create.ForeignKey(ForeignKeys.FK_PERMITS_PWSIDS)
                  .FromTable(Tables.PERMITS).ForeignColumn(Columns.PUBLIC_WATER_SUPPLY_ID)
                  .ToTable(Tables.PUBLIC_WATER_SUPPLIES)
                  .PrimaryColumn(Columns.PUBLIC_WATER_SUPPLY_PUBLIC_WATER_SUPPLY_ID);
            Create.ForeignKey(ForeignKeys.FK_PERMITS_EQUIPMENT_PERMITS)
                  .FromTable(Tables.ENVIRONMENTAL_PERMITS_EQUIPMENT).ForeignColumn(Columns.PERMIT_ID)
                  .ToTable(Tables.PERMITS).PrimaryColumn(Columns.PERMIT_ID);
            Create.ForeignKey(ForeignKeys.FK_PERMITS_EQUIPMENT_EQUIPMENT)
                  .FromTable(Tables.ENVIRONMENTAL_PERMITS_EQUIPMENT).ForeignColumn(Columns.EQUIPMENT_ID)
                  .ToTable(Tables.EQUIPMENT).PrimaryColumn(Columns.EQUIPMENT_ID);
            Create.ForeignKey(ForeignKeys.FK_PERMITS_FACILITIES_PERMITS)
                  .FromTable(Tables.ENVIRONMENTAL_PERMITS_FACILITIES).ForeignColumn(Columns.PERMIT_ID)
                  .ToTable(Tables.PERMITS).PrimaryColumn(Columns.PERMIT_ID);
            Create.ForeignKey(ForeignKeys.FK_PERMITS_FACILITIES_FACILITIES)
                  .FromTable(Tables.ENVIRONMENTAL_PERMITS_FACILITIES).ForeignColumn(Columns.FACILITY_ID)
                  .ToTable(Tables.FACILITIES).PrimaryColumn(Columns.FACILITY_RECORD_ID);

            #endregion

            Execute.Sql(Sql.UPDATE_DATA_TYPE);
            Execute.Sql(Sql.INSERT_LOOKUP_VALUES);
        }

        public override void Down()
        {
            #region Foreign Keys

            Delete.ForeignKey(ForeignKeys.FK_ALLOCATION_PERMITS_PERMITS)
                  .OnTable(Tables.ALLOCATION_PERMITS);
            Delete.ForeignKey(ForeignKeys.FK_ALLOCATION_PERMIT_WITHDRAWAL_NODES_ALLOCATION_PERMITS)
                  .OnTable(Tables.ALLOCATION_PERMIT_WITHDRAWAL_NODES);
            Delete.ForeignKey(ForeignKeys.FK_ALLOCATION_PERMIT_WITHDRAWAL_NODES_CATEGORIES)
                  .OnTable(Tables.ALLOCATION_PERMIT_WITHDRAWAL_NODES);
            Delete.ForeignKey(ForeignKeys.FK_ALLOCATION_PERMIT_WITHDRAWAL_NODES_FACILITIES)
                  .OnTable(Tables.ALLOCATION_PERMIT_WITHDRAWAL_NODES);
            Delete.ForeignKey(ForeignKeys.FK_ALLOCATION_PERMIT_WITHDRAWAL_NODES_COORDINATES)
                  .OnTable(Tables.ALLOCATION_PERMIT_WITHDRAWAL_NODES);
            Delete.ForeignKey(ForeignKeys.FK_ENVIRONMENTAL_PERMITS_OPERATING_CENTERS_PERMITS)
                  .OnTable(Tables.ENVIRONMENTAL_PERMITS_OPERATING_CENTERS);
            Delete.ForeignKey(ForeignKeys.FK_ENVIRONMENTAL_PERMITS_OPERATING_CENTERS_OPERATING_CENTERS)
                  .OnTable(Tables.ENVIRONMENTAL_PERMITS_OPERATING_CENTERS);
            Delete.ForeignKey(ForeignKeys.FK_ALLOCATION_PERMITS_OPERATING_CENTERS)
                  .OnTable(Tables.ALLOCATION_PERMITS);
            Delete.ForeignKey(ForeignKeys.FK_PERMITS_PERMIT_STATUSES)
                  .OnTable(Tables.PERMITS);
            Delete.ForeignKey(ForeignKeys.FK_PERMITS_PERMIT_TYPES)
                  .OnTable(Tables.PERMITS);
            Delete.ForeignKey(ForeignKeys.FK_PERMITS_PWSIDS)
                  .OnTable(Tables.PERMITS);

            Delete.ForeignKey(ForeignKeys.FK_PERMITS_EQUIPMENT_PERMITS)
                  .OnTable(Tables.ENVIRONMENTAL_PERMITS_EQUIPMENT);
            Delete.ForeignKey(ForeignKeys.FK_PERMITS_EQUIPMENT_EQUIPMENT)
                  .OnTable(Tables.ENVIRONMENTAL_PERMITS_EQUIPMENT);
            Delete.ForeignKey(ForeignKeys.FK_PERMITS_FACILITIES_PERMITS)
                  .OnTable(Tables.ENVIRONMENTAL_PERMITS_FACILITIES);
            Delete.ForeignKey(ForeignKeys.FK_PERMITS_FACILITIES_FACILITIES)
                  .OnTable(Tables.ENVIRONMENTAL_PERMITS_FACILITIES);

            #endregion

            #region Environmental Permits

            Execute.Sql(Sql.ROLLBACK_PERMIT_STATUSES);
            Execute.Sql(Sql.ROLLBACK_PERMIT_TYPES);

            Alter.Table(Tables.PERMITS).AddColumn(Columns.OP_CODE).AsAnsiString(StringLengths.OP_CODE).Nullable();
            Execute.Sql(Sql.ROLLBACK_PERMITS_OPERATING_CENTER_ID);
            Delete.Table(Tables.ENVIRONMENTAL_PERMITS_OPERATING_CENTERS);

            Alter.Table(Tables.PERMITS)
                 .AlterColumn(Columns.PUBLIC_WATER_SUPPLY_ID)
                 .AsAnsiString(StringLengths.PWSID)
                 .Nullable();
            Execute.Sql(Sql.ROLLBACK_PERMITS_PWSID);

            Alter.Table(Tables.ENVIRONMENTAL_PERMITS_FACILITIES)
                 .AddColumn(OldColumns.PERMITS_FACILITY_ID)
                 .AsInt32()
                 .NotNullable()
                 .Identity();
            Alter.Table(Tables.ENVIRONMENTAL_PERMITS_EQUIPMENT)
                 .AddColumn(OldColumns.PERMITS_EQUIPMENT_ID)
                 .AsInt32()
                 .NotNullable()
                 .Identity();
            Execute.Sql("ALTER TABLE [dbo].[" + Tables.ENVIRONMENTAL_PERMITS_EQUIPMENT +
                        "] ADD  CONSTRAINT [PK_tblPermitEquipment] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]");
            Execute.Sql("ALTER TABLE [dbo].[" + Tables.ENVIRONMENTAL_PERMITS_FACILITIES +
                        "] ADD  CONSTRAINT [PK_tblPermitFacility] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY]");

            #endregion

            #region AllocationPermitWithdrawalNodes

            Delete.Table(Tables.ALLOCATION_PERMIT_WITHDRAWAL_NODES);

            #endregion

            #region AllocationPermits

            Alter.Table(Tables.ALLOCATION_PERMITS).AlterColumn(Columns.SURFACE_SUPPLY)
                 .AsAnsiString(StringLengths.SURFACE_SUPPLY).Nullable();
            Alter.Table(Tables.ALLOCATION_PERMITS).AlterColumn(Columns.GROUND_SUPPLY)
                 .AsAnsiString(StringLengths.GROUND_SUPPLY).Nullable();
            Alter.Table(Tables.ALLOCATION_PERMITS).AlterColumn(Columns.ACTIVE_PERMIT)
                 .AsAnsiString(StringLengths.ACTIVE_PERMIT).Nullable();
            Execute.Sql(Sql.ROLLBACK_SURFACE_SUPPLY);
            Execute.Sql(Sql.ROLLBACK_GROUND_SUPPLY);
            Execute.Sql(Sql.ROLLBACK_ACTIVE_PERMIT);

            Delete.Column(Columns.GPM).FromTable(Tables.ALLOCATION_PERMITS);
            // ORDER IS IMPORTANT, DO NOT MOVE
            Alter.Table(Tables.ALLOCATION_PERMITS).AddColumn(Columns.OP_CODE).AsAnsiString(StringLengths.OP_CODE)
                 .Nullable();
            Execute.Sql(Sql.ROLLBACK_ALLOCATION_PERMITS_OPERATING_CENTER_ID);
            Delete.Column(Columns.OPERATING_CENTER_ID).FromTable(Tables.ALLOCATION_PERMITS);

            Alter.Table(Tables.ALLOCATION_PERMITS).AddColumn(Columns.SUB_PERMIT_NUMBER)
                 .AsAnsiString(StringLengths.SUB_PERMIT_NUMBER).Nullable();
            Execute.Sql(Sql.ROLLBACK_ALLOCATION_PERMIT_DETAILS_PERMIT_ID);
            Delete.Column(Columns.PERMIT_ID).FromTable(Tables.ALLOCATION_PERMITS);

            #endregion

            #region Lookups

            Delete.Table(Tables.ALLOCATION_CATEGORIES);
            Delete.Table(Tables.ENVIRONMENTAL_PERMIT_STATUSES);
            Delete.Table(Tables.ENVIRONMENTAL_PERMIT_TYPES);

            #endregion

            #region Renames

            Rename.Column(Columns.PERMIT_ID).OnTable(Tables.ENVIRONMENTAL_PERMITS_EQUIPMENT).To("PermitID");
            Rename.Table(Tables.ENVIRONMENTAL_PERMITS_EQUIPMENT).To(OldTables.ENVIRONMENTAL_PERMITS_EQUIPMENT);

            Rename.Column(Columns.PERMIT_ID).OnTable(Tables.ENVIRONMENTAL_PERMITS_FACILITIES).To("PermitID");
            Rename.Table(Tables.ENVIRONMENTAL_PERMITS_FACILITIES).To(OldTables.ENVIRONMENTAL_PERMITS_FACILITIES);

            Rename.Column(Columns.PERMIT_NUMBER).OnTable(Tables.PERMITS).To(OldColumns.PERMIT_NUMBER);
            Rename.Column(Columns.PERMIT_ID).OnTable(Tables.PERMITS).To(OldColumns.PERMIT_ID);
            Rename.Column(Columns.PROGRAM_INTEREST_NUMBER).OnTable(Tables.PERMITS)
                  .To(OldColumns.PROGRAM_INTEREST_NUMBER);
            Rename.Column(Columns.PERMIT_CROSS_REFERENCE_NUMBER).OnTable(Tables.PERMITS)
                  .To(OldColumns.PERMIT_CROSS_REFERENCE_NUMBER);
            Rename.Column(Columns.PERMIT_EFFECTIVE_DATE).OnTable(Tables.PERMITS).To(OldColumns.PERMIT_EFFECTIVE_DATE);
            Rename.Column(Columns.PERMIT_RENEWAL_DATE).OnTable(Tables.PERMITS).To(OldColumns.PERMIT_RENEWAL_DATE);
            Rename.Column(Columns.PERMIT_EXPIRATION_DATE).OnTable(Tables.PERMITS).To(OldColumns.PERMIT_EXPIRATION_DATE);
            Rename.Column(Columns.ANNUAL_FEE).OnTable(Tables.PERMITS).To(OldColumns.ANNUAL_FEE);
            Rename.Column(Columns.PERMIT_TYPE).OnTable(Tables.PERMITS).To(OldColumns.PERMIT_TYPE);
            Rename.Column(Columns.PERMIT_STATUS).OnTable(Tables.PERMITS).To(OldColumns.PERMIT_STATUS);
            Rename.Column(Columns.PUBLIC_WATER_SUPPLY_ID).OnTable(Tables.PERMITS).To(OldColumns.PUBLIC_WATER_SUPPLY_ID);

            Rename.Table(Tables.PERMITS).To(OldTables.PERMITS);
            Rename.Column(Columns.ALLOCATION_PERMIT_ID).OnTable(Tables.ALLOCATION_PERMITS)
                  .To(OldColumns.ALLOCATION_PERMIT_ID);

            Rename.Column(Columns.PUBLIC_WATER_SUPPLY_ID).OnTable(Tables.ALLOCATION_PERMITS)
                  .To(OldColumns.PUBLIC_WATER_SUPPLY_ID);
            Rename.Table(Tables.ALLOCATION_PERMITS).To(OldTables.ALLOCATION_PERMITS);

            #endregion

            Execute.Sql(Sql.ROLLBACK_DATA_TYPE);
        }
    }
}
