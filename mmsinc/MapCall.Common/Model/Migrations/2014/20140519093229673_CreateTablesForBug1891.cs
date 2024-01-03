using FluentMigrator;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140519093229673), Tags("Production")]
    public class CreateTablesForBug1891 : Migration
    {
        public const string NUMBER_REGEX = @"^-?(\d+(?:\.\d+)?)$";

        public static object[] FIELD_TYPES = {
            new {DataType = "String", Regex = @"^(.+)$"},
            new {DataType = "Number", Regex = NUMBER_REGEX},
            new {DataType = "Currency", Regex = NUMBER_REGEX},
            new {DataType = "Date", Regex = @"^(0?[1-9]|1[012])/(0?[1-9]|[12][0-9]|3[01])/(19|20)\d\d$"},
            new {DataType = "DropDown", Regex = @"^(\d+)$"},
        };

        public struct TableNames
        {
            public const string ABC_INDICATORS = "ABCIndicators",
                                EQUIPMENT_CHARACTERISTIC_FIELD_TYPES = "EquipmentCharacteristicFieldTypes",
                                SAP_EQUIPMENT_TYPES = "SAPEquipmentTypes",
                                SAP_EQUIPMENT_MANUFACTURERS = "SAPEquipmentManufacturers",
                                EQUIPMENT_CHARACTERISTIC_FIELDS = "EquipmentCharacteristicFields",
                                EQUIPMENT_CHARACTERISTICS = "EquipmentCharacteristics",
                                EQUIPMENT_CHARACTERISTIC_DROP_DOWN_VALUES = "EquipmentCharacteristicDropDownValues";
        }

        public struct ColumnNames
        {
            public struct ABCIndicators
            {
                public const string DESCRIPTION = "Description";
            }

            public struct EquipmentCharacteristicFieldTypes
            {
                public const string DATA_TYPE = "DataType",
                                    REGEX = "Regex";
            }

            public struct SAPEquipmentTypes
            {
                public const string EQUIPMENT_TYPE_ID = "EquipmentTypeId",
                                    ABBREVIATION = "Abbreviation",
                                    DESCRIPTION = "Description";
            }

            public struct SAPEquipmentManufacturers
            {
                public const string SAP_EQUIPMENT_TYPE_ID = "SAPEquipmentTypeId",
                                    DESCRIPTION = "Description";
            }

            public struct EquipmentCharacteristicFields
            {
                public const string FIELD_NAME = "FieldName",
                                    SAP_EQUIPMENT_TYPE_ID = "SAPEquipmentTypeId",
                                    FIELD_TYPE_ID = "FieldTypeId",
                                    REQUIRED = "Required";
            }

            public struct EquipmentCharacteristics
            {
                public const string EQUIPMENT_ID = "EquipmentId",
                                    FIELD_ID = "FieldId",
                                    VALUE = "Value";
            }

            public struct EquipmentCharacteristicDropDownValues
            {
                public const string VALUE = "Value",
                                    FIELD_ID = "FieldId";
            }

            public struct Common
            {
                public const string ID = "Id";
            }
        }

        public struct StringLengths
        {
            public struct ABCIndicators
            {
                public const int DESCRIPTION = 20;
            }

            public struct EquipmentCharacteristicFieldTypes
            {
                public const int DATA_TYPE = 255, REGEX = 255;
            }

            public struct SAPEquipmentTypes
            {
                public const int ABBREVIATION = 8, DESCRIPTION = 50;
            }

            public struct SAPEquipmentManufacturers
            {
                public const int DESCRIPTION = 50;
            }

            public struct EquipmentCharacteristicFields
            {
                public const int FIELD_NAME = 255;
            }

            public struct EquipmentCharacteristics
            {
                public const int VALUE = 255;
            }

            public struct EquipmentCharacteristicDropDownValues
            {
                public const int VALUE = 255;
            }
        }

        public override void Up()
        {
            Create.Table(TableNames.ABC_INDICATORS)
                  .WithColumn(ColumnNames.Common.ID).AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn(ColumnNames.ABCIndicators.DESCRIPTION).AsString(StringLengths.ABCIndicators.DESCRIPTION)
                  .NotNullable();

            Alter.Table("Equipment")
                 .AddColumn("ABCIndicatorId").AsInt32().ForeignKey("FK_Equipment_ABCIndicators_ABCIndicatorId",
                      TableNames.ABC_INDICATORS, ColumnNames.Common.ID).Nullable();

            Create.Table(TableNames.EQUIPMENT_CHARACTERISTIC_FIELD_TYPES)
                  .WithColumn(ColumnNames.Common.ID).AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn(ColumnNames.EquipmentCharacteristicFieldTypes.DATA_TYPE)
                  .AsString(StringLengths.EquipmentCharacteristicFieldTypes.DATA_TYPE).NotNullable()
                  .WithColumn(ColumnNames.EquipmentCharacteristicFieldTypes.REGEX)
                  .AsString(StringLengths.EquipmentCharacteristicFieldTypes.REGEX).Nullable();

            Create.Table(TableNames.SAP_EQUIPMENT_TYPES)
                  .WithColumn(ColumnNames.Common.ID).AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn(ColumnNames.SAPEquipmentTypes.ABBREVIATION)
                  .AsString(StringLengths.SAPEquipmentTypes.ABBREVIATION).NotNullable()
                  .WithColumn(ColumnNames.SAPEquipmentTypes.DESCRIPTION)
                  .AsString(StringLengths.SAPEquipmentTypes.DESCRIPTION).NotNullable()
                  .WithColumn(ColumnNames.SAPEquipmentTypes.EQUIPMENT_TYPE_ID).AsInt32().Nullable().ForeignKey(
                       string.Format("FK_{0}_{1}_{2}", TableNames.SAP_EQUIPMENT_TYPES,
                           "EquipmentTypes", ColumnNames.SAPEquipmentTypes.EQUIPMENT_TYPE_ID),
                       "EquipmentTypes", "EquipmentTypeId");

            Create.Table(TableNames.SAP_EQUIPMENT_MANUFACTURERS)
                  .WithColumn(ColumnNames.Common.ID).AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn(ColumnNames.SAPEquipmentManufacturers.SAP_EQUIPMENT_TYPE_ID).AsInt32().NotNullable()
                  .ForeignKey("FK_SAPEquipmentManufacturers_SAPEquipmentTypes_SAPEquipmentTypeId", "SAPEquipmentTypes",
                       "Id")
                  .WithColumn(ColumnNames.SAPEquipmentManufacturers.DESCRIPTION)
                  .AsString(StringLengths.SAPEquipmentManufacturers.DESCRIPTION).NotNullable();

            Alter.Table("Equipment")
                 .AddColumn("SAPEquipmentManufacturerId").AsInt32()
                 .ForeignKey("FK_Equipment_SAPEquipmentManufacturers_SAPEquipmentManufacturerId",
                      TableNames.SAP_EQUIPMENT_MANUFACTURERS, ColumnNames.Common.ID).Nullable();

            Create.Table(TableNames.EQUIPMENT_CHARACTERISTIC_FIELDS)
                  .WithColumn(ColumnNames.Common.ID).AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn(ColumnNames.EquipmentCharacteristicFields.FIELD_NAME)
                  .AsString(StringLengths.EquipmentCharacteristicFields.FIELD_NAME).NotNullable()
                  .WithColumn(ColumnNames.EquipmentCharacteristicFields.SAP_EQUIPMENT_TYPE_ID).AsInt32().NotNullable()
                  .ForeignKey(
                       string.Format("FK_{0}_{1}_{2}", TableNames.EQUIPMENT_CHARACTERISTIC_FIELDS,
                           TableNames.SAP_EQUIPMENT_TYPES,
                           ColumnNames.EquipmentCharacteristicFields.SAP_EQUIPMENT_TYPE_ID),
                       TableNames.SAP_EQUIPMENT_TYPES, "Id")
                  .WithColumn(ColumnNames.EquipmentCharacteristicFields.FIELD_TYPE_ID).AsInt32().NotNullable()
                  .ForeignKey(
                       string.Format("FK_{0}_{1}_{2}", TableNames.EQUIPMENT_CHARACTERISTIC_FIELDS,
                           TableNames.EQUIPMENT_CHARACTERISTIC_FIELD_TYPES,
                           ColumnNames.EquipmentCharacteristicFields.FIELD_TYPE_ID),
                       TableNames.EQUIPMENT_CHARACTERISTIC_FIELD_TYPES, ColumnNames.Common.ID)
                  .WithColumn(ColumnNames.EquipmentCharacteristicFields.REQUIRED).AsBoolean().NotNullable();

            Create.Table(TableNames.EQUIPMENT_CHARACTERISTICS)
                  .WithColumn(ColumnNames.Common.ID).AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn(ColumnNames.EquipmentCharacteristics.EQUIPMENT_ID).AsInt32().NotNullable().ForeignKey(
                       string.Format("FK_{0}_{1}_{2}", TableNames.EQUIPMENT_CHARACTERISTICS,
                           "Equipment",
                           ColumnNames.EquipmentCharacteristics.EQUIPMENT_ID),
                       "Equipment", "EquipmentId")
                  .WithColumn(ColumnNames.EquipmentCharacteristics.FIELD_ID).AsInt32().NotNullable().ForeignKey(
                       string.Format("FK_{0}_{1}_{2}", TableNames.EQUIPMENT_CHARACTERISTICS,
                           TableNames.EQUIPMENT_CHARACTERISTIC_FIELDS,
                           ColumnNames.EquipmentCharacteristics.FIELD_ID),
                       TableNames.EQUIPMENT_CHARACTERISTIC_FIELDS, ColumnNames.Common.ID)
                  .WithColumn(ColumnNames.EquipmentCharacteristics.VALUE)
                  .AsString(StringLengths.EquipmentCharacteristics.VALUE);

            Create.Table(TableNames.EQUIPMENT_CHARACTERISTIC_DROP_DOWN_VALUES)
                  .WithColumn(ColumnNames.Common.ID).AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn(ColumnNames.EquipmentCharacteristicDropDownValues.VALUE)
                  .AsString(StringLengths.EquipmentCharacteristicDropDownValues.VALUE)
                  .WithColumn(ColumnNames.EquipmentCharacteristicDropDownValues.FIELD_ID).AsInt32().NotNullable()
                  .ForeignKey(
                       string.Format("FK_{0}_{1}_{2}", TableNames.EQUIPMENT_CHARACTERISTIC_DROP_DOWN_VALUES,
                           TableNames.EQUIPMENT_CHARACTERISTIC_FIELDS,
                           ColumnNames.EquipmentCharacteristicDropDownValues.FIELD_ID),
                       TableNames.EQUIPMENT_CHARACTERISTIC_FIELDS, ColumnNames.Common.ID);

            dynamic fieldTypeInsert = Insert.IntoTable(TableNames.EQUIPMENT_CHARACTERISTIC_FIELD_TYPES);

            FIELD_TYPES.Each(o => fieldTypeInsert = fieldTypeInsert.Row(o));

            dynamic abcIndicatorInsert = Insert.IntoTable(TableNames.ABC_INDICATORS);

            new[] {"Low", "Medium", "High"}.Each(
                s => abcIndicatorInsert = abcIndicatorInsert.Row(new {Description = s}));

            Execute.Sql(@"
                declare @moduleId int
                set @moduleId = (select ModuleID from [Modules] where Name = 'Facilities')
                insert into [NotificationPurposes] ([ModuleID], [Purpose]) VALUES(@moduleId, 'Equipment Record Created')
            ");
        }

        public override void Down()
        {
            Execute.Sql(@"
                declare @purposeId int
                set @purposeId = (select top 1 NotificationPurposeID from [NotificationPurposes] where [Purpose] = 'Equipment Record Created')
                
                delete from [NotificationConfigurations] where [NotificationPurposeID] = @purposeId
                delete from [NotificationPurposes] where [NotificationPurposeID] = @purposeId 
            ");

            Delete.ForeignKey("FK_SAPEquipmentManufacturers_SAPEquipmentTypes_SAPEquipmentTypeId")
                  .OnTable(TableNames.SAP_EQUIPMENT_MANUFACTURERS);
            Delete.ForeignKey("FK_Equipment_ABCIndicators_ABCIndicatorId").OnTable("Equipment");
            Delete.ForeignKey("FK_Equipment_SAPEquipmentManufacturers_SAPEquipmentManufacturerId").OnTable("Equipment");
            Delete.Column("ABCIndicatorId").FromTable("Equipment");
            Delete.Column("SAPEquipmentManufacturerId").FromTable("Equipment");

            Delete.Table(TableNames.ABC_INDICATORS);
            Delete.Table(TableNames.EQUIPMENT_CHARACTERISTIC_DROP_DOWN_VALUES);
            Delete.Table(TableNames.EQUIPMENT_CHARACTERISTICS);
            Delete.Table(TableNames.EQUIPMENT_CHARACTERISTIC_FIELDS);
            Delete.Table(TableNames.SAP_EQUIPMENT_TYPES);
            Delete.Table(TableNames.SAP_EQUIPMENT_MANUFACTURERS);
            Delete.Table(TableNames.EQUIPMENT_CHARACTERISTIC_FIELD_TYPES);
        }
    }
}
