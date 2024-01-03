using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130430090847), Tags("Production")]
    public class AddGeneratorsForBug1462 : Migration
    {
        #region Constants

        public struct Sql
        {
            public const string TRANSFORM_EQUIPMENT_TYPE_EQUIPMENT_DETAIL_TYPE = @"
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Generator') WHERE Abbreviation in ('EPND', 'EPNG', 'EPDC', 'EPGS');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Chemical Feed Dry') WHERE Abbreviation in ('CFDR');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Chemical Feed Liquid') WHERE Abbreviation in ('CFLQ');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Chemical Tank') WHERE Abbreviation in ('CSFO', 'CSPL', 'CSTK');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Circuit Breaker') WHERE Abbreviation in ('ECBK');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Compressor') WHERE Abbreviation in ('ACMP');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Disconnect') WHERE Abbreviation in ('EDIS');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Feeder Cable Buss') WHERE Abbreviation in ('EFCB');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Filter') WHERE Abbreviation in ('FTFL');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Generator') WHERE Abbreviation in ('EPDC', 'EPGS', 'EPND', 'EPNG');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Instrument') WHERE Abbreviation in ('ICLA', 'IFLM', 'IFLT', 'IFLU', 'ILHT', 'ILVT', 'IMGM', 'IMPA', 'INTU', 'IOXM', 'IOZA', 'IPHA', 'IPTR', 'ITMT', 'WPSG', 'WTGA');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Motor Control Center') WHERE Abbreviation in ('EMCC', 'EMST');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Motor') WHERE Abbreviation in ('EMTR');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Protective Relay Meter') WHERE Abbreviation in ('EPRM');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Pump') WHERE Abbreviation in ('AVCP', 'PBSR', 'PCRP', 'PCTP', 'PDWP', 'PEMP', 'PHYD', 'PRWP', 'PSAM', 'PSLF', 'PSLG', 'PSMP', 'PSUP', 'PVCP', 'PWLL', 'PWWP');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'RTU') WHERE Abbreviation in ('IRTU');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'RTU Radio') WHERE Abbreviation in ('IRAD');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Transfer Switch') WHERE Abbreviation in ('ETSW');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Transformers') WHERE Abbreviation in ('ETRM');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Valve') WHERE Abbreviation in ('CLAV', 'CLBV', 'CLGV', 'CLRV', 'CLVR', 'CLYV', 'PVAV', 'VALT', 'VAWS', 'VCHK', 'VDCV', 'VDRN', 'VEFF', 'VFLC', 'VFRI', 'VFSW', 'VFTW', 'VINF', 'VISO', 'VPIL', 'VPRC', 'VPRR', 'VRPZ', 'VSBD', 'VSLC', 'VSLG', 'VSPL', 'VVBV', 'VVNT', 'VWSH', 'WFLC', 'WPVO');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'VFD') WHERE Abbreviation in ('EVFD');
                                    UPDATE EquipmentTypes SET DetailTypeID = (Select EquipmentDetailTypeID from EquipmentDetailTypes where Description = 'Well') WHERE Abbreviation in ('WELL');",
                                TRANSFORM_EQUIPMENT_DETAIL_TYPES = "INSERT INTO EquipmentDetailTypes Values('{0}');";
        }

        public struct Tables
        {
            public const string GENERATOR_DETAILS = "Generators",
                                EQUIPMENT = "Equipment",
                                EQUIPMENT_MANUFACTURERS = "EquipmentManufacturers",
                                EQUIPMENT_MODELS = "EquipmentModels",
                                EMERGENCY_POWER_TYPES = "EmergencyPowerTypes",
                                FUEL_TYPES = "FuelTypes",
                                EQUIPMENT_TYPES = "EquipmentTypes",
                                EQUIPMENT_DETAIL_TYPES = "EquipmentDetailTypes",
                                EQUIPMENT_MANUFACTURER_TYPES = "EquipmentManufacturerTypes";
        }

        public struct Columns
        {
            public const string GENERATOR_DETAIL_ID = "GeneratorID",
                                EQUIPMENT_ID = "EquipmentID",
                                EMERGENCY_POWER_TYPE_ID = "EmergencyPowerTypeID",
                                ENGINE_MANUFACTURER_ID = "EngineManufacturerID",
                                ENGINE_MODEL_ID = "EngineModelID",
                                ENGINE_SERIAL_NUMBER = "EngineSerialNumber",
                                GENERATOR_MANUFACTURER_ID = "GeneratorManufacturerID",
                                GENERATOR_MODEL_ID = "GeneratorModelID",
                                GENERATOR_SERIAL_NUMBER = "GeneratorSerialNumber",
                                OUTPUT_VOLTAGE = "OutputVoltage",
                                OUTPUT_KW = "OutputKW", //int
                                LOAD_CAPACITY = "LoadCapacity", //int
                                HAS_PARALLEL_ELECTRIC_OPERATION = "HasParallelElectricOperation",
                                HAS_AUTOMATIC_START = "HasAutomaticStart",
                                HAS_AUTOMATIC_POWER_TRANSFER = "HasAutomaticPowerTransfer",
                                IS_PORTABLE = "IsPortable",
                                FUEL_TYPE_ID = "FuelTypeID",
                                SCADA = "SCADA", //bool
                                TRAILER_VIN = "TrailerVin",
                                GVWR = "GVWR",
                                FUEL_GPH = "FuelGPH", //decimal
                                BTU = "BTU", // int
                                HP = "HP", //int
                                AQ_PERMIT_NUMBER = "AQPermitNumber",
                                DESCRIPTION = "Description",
                                EQUIPMENT_MANUFACTURER_ID = "EquipmentManufacturerID",
                                EQUIPMENT_MODEL_ID = "EquipmentModelID",
                                EQUIPMENT_DETAIL_TYPE_ID = "EquipmentDetailTypeID",
                                DETAIL_TYPE_ID = "DetailTypeID",
                                EQUIPMENT_MANUFACTURER_TYPE_ID = "EquipmentManufacturerTypeID",
                                MANUFACTURER_TYPE_ID = "ManufacturerTypeID";
        }

        public struct ForeignKeys
        {
            public const string FK_GENERATOR_DETAILS_EQUIPMENT = "FK_Generators_Equipment_EquipmentID",
                                FK_GENERATOR_DETAILS_EMERGENCY_POWER_TYPE =
                                    "FK_Generators_EmergencyPowerTypes_EmergencyPowerTypeID",
                                FK_GENERATOR_DETAILS_ENGINE_MANUFACTURER =
                                    "FK_Generators_EquipmentManufacturers_EngineManufacturerID",
                                FK_GENERATOR_DETAILS_ENGINE_MODEL = "FK_Generators_EquipmentModels_EngineModelID",
                                FK_GENERATOR_DETAILS_GENERATOR_MANUFACTURER =
                                    "FK_Generators_EquipmentManufacturers_GeneratorManufacturerID",
                                FK_GENERATOR_DETAILS_GENERATOR_MODEL =
                                    "FK_Generators_EquipmentManufacturers_GeneratorModelID",
                                FK_GENERATOR_DETAILS_FUEL_TYPE = "FK_Generators_FuelTypes_FuelTypeID",
                                FK_EQUIPMENT_TYPES_EQUIPMENT_DETAIL_TYPES =
                                    "FK_EquipmentTypes_EquipmentDetailTypes_DetailTypeID",
                                FK_EQUIPMENT_MANUFACTURERS_EQUIPMENT_MANUFACTURER_TYPES =
                                    "FK_EquipmentManufacturers_EquipmentManufacturerTypes_EquipmentManufacturerTypeID";
        }

        public struct StringLengths
        {
            public const int ENGINE_SERIAL_NUMBER = 50,
                             GENERATOR_SERIAL_NUMBER = 50,
                             OUTPUT_VOLTAGE = 20,
                             TRAILER_VIN = 20,
                             GVWR = 20,
                             AQ_PERMIT_NUMBER = 50,
                             DESCRIPTION = 50;
        }

        #endregion

        public override void Up()
        {
            #region Tables

            Create.Table(Tables.FUEL_TYPES)
                  .WithColumn(Columns.FUEL_TYPE_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.EMERGENCY_POWER_TYPES)
                  .WithColumn(Columns.EMERGENCY_POWER_TYPE_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.GENERATOR_DETAILS)
                  .WithColumn(Columns.GENERATOR_DETAIL_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.EQUIPMENT_ID).AsInt32().NotNullable()
                  .WithColumn(Columns.EMERGENCY_POWER_TYPE_ID).AsInt32().Nullable()
                  .WithColumn(Columns.ENGINE_MANUFACTURER_ID).AsInt32().Nullable()
                  .WithColumn(Columns.ENGINE_MODEL_ID).AsInt32().Nullable()
                  .WithColumn(Columns.ENGINE_SERIAL_NUMBER).AsAnsiString(StringLengths.ENGINE_SERIAL_NUMBER).Nullable()
                  .WithColumn(Columns.GENERATOR_MANUFACTURER_ID).AsInt32().Nullable()
                  .WithColumn(Columns.GENERATOR_MODEL_ID).AsInt32().Nullable()
                  .WithColumn(Columns.GENERATOR_SERIAL_NUMBER).AsAnsiString(StringLengths.GENERATOR_SERIAL_NUMBER)
                  .Nullable()
                  .WithColumn(Columns.OUTPUT_VOLTAGE).AsDecimal(18, 2).Nullable()
                  .WithColumn(Columns.OUTPUT_KW).AsDecimal(18, 2).Nullable()
                  .WithColumn(Columns.LOAD_CAPACITY).AsDecimal(18, 2).Nullable()
                  .WithColumn(Columns.HAS_PARALLEL_ELECTRIC_OPERATION).AsBoolean().Nullable()
                  .WithColumn(Columns.HAS_AUTOMATIC_START).AsBoolean().Nullable()
                  .WithColumn(Columns.HAS_AUTOMATIC_POWER_TRANSFER).AsBoolean().Nullable()
                  .WithColumn(Columns.IS_PORTABLE).AsBoolean().Nullable()
                  .WithColumn(Columns.FUEL_TYPE_ID).AsInt32().Nullable()
                  .WithColumn(Columns.SCADA).AsBoolean().Nullable()
                  .WithColumn(Columns.TRAILER_VIN).AsAnsiString(StringLengths.TRAILER_VIN).Nullable()
                  .WithColumn(Columns.GVWR).AsAnsiString(StringLengths.GVWR).Nullable()
                  .WithColumn(Columns.FUEL_GPH).AsDecimal(18, 2).Nullable()
                  .WithColumn(Columns.BTU).AsInt32().Nullable()
                  .WithColumn(Columns.HP).AsInt32().Nullable()
                  .WithColumn(Columns.AQ_PERMIT_NUMBER).AsAnsiString(StringLengths.AQ_PERMIT_NUMBER).Nullable();
            Create.Table(Tables.EQUIPMENT_DETAIL_TYPES)
                  .WithColumn(Columns.EQUIPMENT_DETAIL_TYPE_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Create.Table(Tables.EQUIPMENT_MANUFACTURER_TYPES)
                  .WithColumn(Columns.EQUIPMENT_MANUFACTURER_TYPE_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();
            Alter.Table(Tables.EQUIPMENT_TYPES)
                 .AddColumn(Columns.DETAIL_TYPE_ID).AsInt32().Nullable();
            Alter.Table(Tables.EQUIPMENT_MANUFACTURERS)
                 .AddColumn(Columns.MANUFACTURER_TYPE_ID).AsInt32().Nullable();

            #endregion

            #region Foreign Keys

            Create.ForeignKey(ForeignKeys.FK_GENERATOR_DETAILS_EMERGENCY_POWER_TYPE)
                  .FromTable(Tables.GENERATOR_DETAILS).ForeignColumn(Columns.EMERGENCY_POWER_TYPE_ID)
                  .ToTable(Tables.EMERGENCY_POWER_TYPES).PrimaryColumn(Columns.EMERGENCY_POWER_TYPE_ID);
            Create.ForeignKey(ForeignKeys.FK_GENERATOR_DETAILS_EQUIPMENT)
                  .FromTable(Tables.GENERATOR_DETAILS).ForeignColumn(Columns.EQUIPMENT_ID)
                  .ToTable(Tables.EQUIPMENT).PrimaryColumn(Columns.EQUIPMENT_ID);
            Create.ForeignKey(ForeignKeys.FK_GENERATOR_DETAILS_ENGINE_MANUFACTURER)
                  .FromTable(Tables.GENERATOR_DETAILS).ForeignColumn(Columns.ENGINE_MANUFACTURER_ID)
                  .ToTable(Tables.EQUIPMENT_MANUFACTURERS).PrimaryColumn(Columns.EQUIPMENT_MANUFACTURER_ID);
            Create.ForeignKey(ForeignKeys.FK_GENERATOR_DETAILS_ENGINE_MODEL)
                  .FromTable(Tables.GENERATOR_DETAILS).ForeignColumn(Columns.ENGINE_MODEL_ID)
                  .ToTable(Tables.EQUIPMENT_MODELS).PrimaryColumn(Columns.EQUIPMENT_MODEL_ID);
            Create.ForeignKey(ForeignKeys.FK_GENERATOR_DETAILS_FUEL_TYPE)
                  .FromTable(Tables.GENERATOR_DETAILS).ForeignColumn(Columns.FUEL_TYPE_ID)
                  .ToTable(Tables.FUEL_TYPES).PrimaryColumn(Columns.FUEL_TYPE_ID);
            Create.ForeignKey(ForeignKeys.FK_GENERATOR_DETAILS_GENERATOR_MANUFACTURER)
                  .FromTable(Tables.GENERATOR_DETAILS).ForeignColumn(Columns.GENERATOR_MANUFACTURER_ID)
                  .ToTable(Tables.EQUIPMENT_MANUFACTURERS).PrimaryColumn(Columns.EQUIPMENT_MANUFACTURER_ID);
            Create.ForeignKey(ForeignKeys.FK_GENERATOR_DETAILS_GENERATOR_MODEL)
                  .FromTable(Tables.GENERATOR_DETAILS).ForeignColumn(Columns.GENERATOR_MODEL_ID)
                  .ToTable(Tables.EQUIPMENT_MODELS).PrimaryColumn(Columns.EQUIPMENT_MODEL_ID);
            Create.ForeignKey(ForeignKeys.FK_EQUIPMENT_TYPES_EQUIPMENT_DETAIL_TYPES)
                  .FromTable(Tables.EQUIPMENT_TYPES).ForeignColumn(Columns.DETAIL_TYPE_ID)
                  .ToTable(Tables.EQUIPMENT_DETAIL_TYPES).PrimaryColumn(Columns.EQUIPMENT_DETAIL_TYPE_ID);
            Create.ForeignKey(ForeignKeys.FK_EQUIPMENT_MANUFACTURERS_EQUIPMENT_MANUFACTURER_TYPES)
                  .FromTable(Tables.EQUIPMENT_MANUFACTURERS).ForeignColumn(Columns.MANUFACTURER_TYPE_ID)
                  .ToTable(Tables.EQUIPMENT_MANUFACTURER_TYPES).PrimaryColumn(Columns.EQUIPMENT_MANUFACTURER_TYPE_ID);

            #endregion

            #region Transform

            #region EquipmentDetailTypes

            // The data for this table is in another database so this is here to avoid the 
            // other less desirable tricks that would be needed to make this work.
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Chemical Feed Dry"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Chemical Feed Liquid"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Chemical Tank"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Circuit Breaker"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Compressor"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Disconnect"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Feeder Cable Buss"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Filter"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Generator"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Instrument"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Motor Control Center"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Motor"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Protective Relay Meter"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Pump"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "RTU"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "RTU Radio"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Transfer Switch"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Transformers"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Valve"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "VFD"));
            Execute.Sql(String.Format(Sql.TRANSFORM_EQUIPMENT_DETAIL_TYPES, "Well"));

            #endregion

            Execute.Sql(Sql.TRANSFORM_EQUIPMENT_TYPE_EQUIPMENT_DETAIL_TYPE);

            #endregion
        }

        public override void Down()
        {
            //FOREIGN KEYS
            Delete.ForeignKey(ForeignKeys.FK_EQUIPMENT_MANUFACTURERS_EQUIPMENT_MANUFACTURER_TYPES)
                  .OnTable(Tables.EQUIPMENT_MANUFACTURERS);
            Delete.ForeignKey(ForeignKeys.FK_GENERATOR_DETAILS_EMERGENCY_POWER_TYPE).OnTable(Tables.GENERATOR_DETAILS);
            Delete.ForeignKey(ForeignKeys.FK_GENERATOR_DETAILS_ENGINE_MANUFACTURER).OnTable(Tables.GENERATOR_DETAILS);
            Delete.ForeignKey(ForeignKeys.FK_GENERATOR_DETAILS_EQUIPMENT).OnTable(Tables.GENERATOR_DETAILS);
            Delete.ForeignKey(ForeignKeys.FK_GENERATOR_DETAILS_FUEL_TYPE).OnTable(Tables.GENERATOR_DETAILS);
            Delete.ForeignKey(ForeignKeys.FK_GENERATOR_DETAILS_GENERATOR_MANUFACTURER)
                  .OnTable(Tables.GENERATOR_DETAILS);
            Delete.ForeignKey(ForeignKeys.FK_EQUIPMENT_TYPES_EQUIPMENT_DETAIL_TYPES).OnTable(Tables.EQUIPMENT_TYPES);

            //TABLES
            Delete.Table(Tables.EQUIPMENT_MANUFACTURER_TYPES);
            Delete.Table(Tables.EQUIPMENT_DETAIL_TYPES);
            Delete.Table(Tables.FUEL_TYPES);
            Delete.Table(Tables.EMERGENCY_POWER_TYPES);
            Delete.Table(Tables.GENERATOR_DETAILS);

            Delete.Column(Columns.MANUFACTURER_TYPE_ID).FromTable(Tables.EQUIPMENT_MANUFACTURERS);
            Delete.Column(Columns.DETAIL_TYPE_ID).FromTable(Tables.EQUIPMENT_TYPES);
        }
    }
}
