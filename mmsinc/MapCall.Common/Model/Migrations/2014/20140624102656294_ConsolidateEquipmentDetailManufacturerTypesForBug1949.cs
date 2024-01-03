using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140624102656294), Tags("Production")]
    public class ConsolidateEquipmentDetailManufacturerTypesForBug1949 : Migration
    {
        public const int MAX_DESCRIPTION_LENGTH = 50;

        public struct TableNames
        {
            public const string EQUIPMENT_DETAIL_TYPES = "EquipmentDetailTypes",
                                EQUIPMENT_MANUFACTURERS = "EquipmentManufacturers",
                                EQUIPMENT_MANUFACTURER_TYPES = "EquipmentManufacturerTypes";
        }

        public struct ColumnNames
        {
            public const string EQUIPMENT_MANUFACTURER_TYPE_ID = "EquipmentManufacturerTypeID",
                                MANUFACTURER_TYPE_ID = "ManufacturerTypeID",
                                EQUIPMENT_DETAIL_TYPE_ID = "EquipmentDetailTypeID";
        }

        public struct Sql
        {
            public const string ADD_MISSING_EQUIPMENT_MANUFACTURER_TYPES_TO_EQUIPMENT_DETAIL_TYPES =
                                    "INSERT INTO EquipmentDetailTypes SELECT [Description] FROM EquipmentManufacturerTypes WHERE [Description] not in (Select [Description] from EquipmentDetailTypes)",
                                UPDATE_EQUIPMENT_MANUFACTURER_TYPES =
                                    @"  UPDATE
	                                        EquipmentManufacturers
                                        SET
	                                        ManufacturerTypeID = EDT.EquipmentDetailTypeID
                                        FROM
	                                        EquipmentManufacturers EM
                                        LEFT JOIN
	                                        EquipmentManufacturerTypes EMT ON EM.ManufacturerTypeID = EMT.EquipmentManufacturerTypeID
                                        LEFT JOIN
	                                        EquipmentDetailTypes EDT on EDT.Description = EMT.Description
                                        ",
                                ROLLBACK_ADD_REMOVED_EQUIPMENT_MANUFACTURER_TYPES_BACK =
                                    "INSERT INTO EquipmentManufacturerTypes([Description]) Select [Description] from EquipmentDetailTypes where EquipmentDetailTypeID in (Select ManufacturerTypeID from EquipmentManufacturers) order by 1 ",
                                ROLLBACK_SET_EQUIPMENT_MANUFACTURER_TYPES =
                                    @"  UPDATE
	                                        EquipmentManufacturers
                                        SET
	                                        ManufacturerTypeID = EMT.EquipmentManufacturerTypeID
                                        FROM
	                                        EquipmentManufacturers EM
                                        LEFT JOIN
	                                        EquipmentDetailTypes EDT on EDT.EquipmentDetailTypeID = EM.ManufacturerTypeID
                                        LEFT JOIN
	                                        EquipmentManufacturerTypes EMT on EMT.[Description] = EDT.[Description]",
                                CLEANUP = @"
                                    UPDATE Equipment SET Identifier = 'NJSB-56-FLMX-11' WHERE Identifier = 'NJSB-56-FLMX11';
                                    UPDATE Equipment SET Number = parsename(replace(identifier, '-', '.'), 1) WHERE isNull(Number,0) <> parsename(replace(identifier, '-', '.'), 1)	";
        }

        public struct ForeignKeys
        {
            public const string FK_EQUIPMENT_MANUFACTURERS_EQUIPMENT_MANUFACTURER_TYPES =
                                    "FK_EquipmentManufacturers_EquipmentManufacturerTypes_EquipmentManufacturerTypeID",
                                FK_EQUIPMENT_MANUFACTURERS_EQUIPMENT_DETAIL_TYPES =
                                    "FK_EquipmentManufacturers_EquipmentDetailTypes_ManufacturerTypeId";
        }

        public override void Up()
        {
            // Create Missing Values
            Execute.Sql(Sql.ADD_MISSING_EQUIPMENT_MANUFACTURER_TYPES_TO_EQUIPMENT_DETAIL_TYPES);

            // Remove FK
            Delete.ForeignKey(ForeignKeys.FK_EQUIPMENT_MANUFACTURERS_EQUIPMENT_MANUFACTURER_TYPES)
                  .OnTable(TableNames.EQUIPMENT_MANUFACTURERS);

            // Update values with new ID
            Execute.Sql(Sql.UPDATE_EQUIPMENT_MANUFACTURER_TYPES);

            // Create New FK
            Create.ForeignKey(ForeignKeys.FK_EQUIPMENT_MANUFACTURERS_EQUIPMENT_DETAIL_TYPES)
                  .FromTable(TableNames.EQUIPMENT_MANUFACTURERS).ForeignColumn(ColumnNames.MANUFACTURER_TYPE_ID)
                  .ToTable(TableNames.EQUIPMENT_DETAIL_TYPES).PrimaryColumn(ColumnNames.EQUIPMENT_DETAIL_TYPE_ID);

            // Remove Table
            Delete.Table(TableNames.EQUIPMENT_MANUFACTURER_TYPES);

            Execute.Sql(Sql.CLEANUP);
        }

        public override void Down()
        {
            // Add Table Back
            Create.Table(TableNames.EQUIPMENT_MANUFACTURER_TYPES)
                  .WithColumn(ColumnNames.EQUIPMENT_MANUFACTURER_TYPE_ID).AsInt32().NotNullable().PrimaryKey()
                  .Identity()
                  .WithColumn("Description").AsString().NotNullable().Unique();

            // Remove New Foreign Key
            Delete.ForeignKey(ForeignKeys.FK_EQUIPMENT_MANUFACTURERS_EQUIPMENT_DETAIL_TYPES)
                  .OnTable(TableNames.EQUIPMENT_MANUFACTURERS);

            // Populate Table
            Execute.Sql(Sql.ROLLBACK_ADD_REMOVED_EQUIPMENT_MANUFACTURER_TYPES_BACK);
            Execute.Sql(Sql.ROLLBACK_SET_EQUIPMENT_MANUFACTURER_TYPES);

            // Add Old Foreign Key Back
            Create.ForeignKey(ForeignKeys.FK_EQUIPMENT_MANUFACTURERS_EQUIPMENT_MANUFACTURER_TYPES)
                  .FromTable(TableNames.EQUIPMENT_MANUFACTURERS)
                  .ForeignColumn(ColumnNames.MANUFACTURER_TYPE_ID)
                  .ToTable(TableNames.EQUIPMENT_MANUFACTURER_TYPES)
                  .PrimaryColumn(ColumnNames.EQUIPMENT_MANUFACTURER_TYPE_ID);
        }
    }
}
