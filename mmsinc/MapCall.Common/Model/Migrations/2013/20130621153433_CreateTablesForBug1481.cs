using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130621153433)]
    public class CreateTablesForBug1481 : Migration
    {
        public struct TableNames
        {
            public const string INTERCONNECTION_PURCHASE_SELL_TRANSFER = "InterconnectionPurchaseSellTransfer",
                                INTERCONNECTION_OPERATING_STATUSES = "InterconnectionOperatingStatuses",
                                INTERCONNECTION_CATEGORIES = "InterconnectionCategories",
                                INTERCONNECTION_DELIVERY_METHODS = "InterconnectionDeliveryMethods",
                                INTERCONNECTION_FLOW_CONTROL_METHODS = "InterconnectionFlowControlMethods",
                                INTERCONNECTION_DIRECTIONS = "InterconnectionDirections",
                                INTERCONNECTION_TYPES = "InterconnectionTypes",
                                INTERCONNECTIONS = "Interconnections",
                                INTERCONNECTIONS_METERS = "InterconnectionsMeters";
        }

        public struct ColumnNames
        {
            public struct InterconnectionPurchaseSellTransfer
            {
                public const string ID = "InterconnectionPurchaseSellTransferId",
                                    DESCRIPTION = "Description";
            }

            public struct InterconnectionOperatingStatuses
            {
                public const string ID = "InterconnectionOperatingStatusId",
                                    DESCRIPTION = "Description";
            }

            public struct InterconnectionCategories
            {
                public const string ID = "InterconnectionCategoryId",
                                    DESCRIPTION = "Description";
            }

            public struct InterconnectionDeliveryMethods
            {
                public const string ID = "InterconnectionDeliveryMethodId",
                                    DESCRIPTION = "Description";
            }

            public struct InterconnectionFlowControlMethods
            {
                public const string ID = "InterconnectionFlowControlMethodId",
                                    DESCRIPTION = "Description";
            }

            public struct InterconnectionDirections
            {
                public const string ID = "InterconnectionDirectionId",
                                    DESCRIPTION = "Description";
            }

            public struct InterconnectionTypes
            {
                public const string ID = "InterconnectionTypeId",
                                    DESCRIPTION = "Description";
            }

            public struct Interconnections
            {
                public const string ID = "InterconnectionId",
                                    DATE_ADDED = "DateAdded",
                                    NJDEP_DESIGNATION = "NJDEPDesignation",
                                    PURCHASE_SELL_TRANSFER_ID = "PurchaseSellTransferId",
                                    PROGRAM_INTEREST_NUMBER = "ProgramInterestNumber",
                                    PURCHASED_WATER_ACCOUNT_NUMBER = "PurchasedWaterAccountNumber",
                                    SOLD_WATER_ACCOUNT_NUMBER = "SoldWaterAccountNumber",
                                    OPERATING_STATUS_ID = "OperatingStatusId",
                                    CATEGORY_ID = "CategoryId",
                                    DIRECT_CONNECTION = "DirectConnection",
                                    DELIVERY_METHOD_ID = "DeliveryMethodId",
                                    FLOW_CONTROL_METHOD_ID = "FlowControlMethodId",
                                    INLET_CONNECTION_SIZE = "InletConnectionSize",
                                    OUTLET_CONNECTION_SIZE = "OutletConnectionSize",
                                    INLET_STATIC_PRESSURE = "InletStaticPressure",
                                    OUTLET_STATIC_PRESSURE = "OutletStaticPressure",
                                    MAXIMUM_FLOW_CAPACITY = "MaximumFlowCapacity",
                                    MAXIMUM_FLOW_CAPACITY_STRESSED_CONDITION = "MaximumFlowCapacityStressedCondition",
                                    DISTRIBUTION_PIPING_RESTRICTIONS = "DistributionPipingRestrictions",
                                    WATER_QUALITY = "WaterQuality",
                                    FLUORIDATED_SUPPLY_RECEIVING_PURVEYOR = "FluoridatedSupplyReceivingPurveyor",
                                    FLUORIDATED_SUPPLY_DELIVERY_PURVEYOR = "FluoridatedSupplyDeliveryPurveyor",
                                    CHLORAMINE_RESIDUAL_RECEIVING_PURVEYOR = "ChloramineResidualReceivingPurveyor",
                                    CHLORAMINE_RESIDUAL_DELIVERY_PURVEYOR = "ChloramineResidualDeliveryPurveyor",
                                    CORROSION_INHIBITOR_RECEIVING_PURVEYOR = "CorrosionInhibitorReceivingPurveyor",
                                    CORROSION_INHIBITOR_DELIVERY_PURVEYOR = "CorrosionInhibitorDeliveryPurveyor",
                                    REVERSIBLE_CAPACITY = "ReversibleCapacity",
                                    DIRECTION_ID = "DirectionId",
                                    TYPE_ID = "TypeId",
                                    ANNUAL_TEST_REQUIRED = "AnnualTestRequired",
                                    LATITUDE = "Latitude",
                                    LONGITUDE = "Longitude",
                                    CONTRACT = "Contract",
                                    CONTRACT_MAX_SUMMER = "ContractMaxSummer",
                                    CONTRACT_MIN_SUMMER = "ContractMinSummer",
                                    CONTRACT_MAX_WINTER = "ContractMaxWinter",
                                    CONTRACT_MIN_WINTER = "ContractMinWinter",
                                    FACILITY_ID = "FacilityId",
                                    INLET_PWSID_ID = "InletPWSIDId",
                                    OUTLET_PWSID_ID = "OutletPWSIDId",
                                    COORDINATE_ID = "CoordinateId";
            }

            public struct InterconnectionsMeters
            {
                public const string INTERCONNECTION_ID = "InterconnectionId",
                                    METER_ID = "MeterId";
            }
        }

        public struct StringLengths
        {
            public struct InterconnectionPurchaseSellTransfer
            {
                public const int DESCRIPTION = 8;
            }

            public struct InterconnectionOperatingStatuses
            {
                public const int DESCRIPTION = 20;
            }

            public struct InterconnectionCategories
            {
                public const int DESCRIPTION = 35;
            }

            public struct InterconnectionDeliveryMethods
            {
                public const int DESCRIPTION = 15;
            }

            public struct InterconnectionFlowControlMethods
            {
                public const int DESCRIPTION = 25;
            }

            public struct InterconnectionDirections
            {
                public const int DESCRIPTION = 20;
            }

            public struct InterconnectionTypes
            {
                public const int DESCRIPTION = 10;
            }

            public struct Interconnections
            {
                public const int NJDEP_DESIGNATION = 30,
                                 PROGRAM_INTEREST_NUMBER = 50,
                                 PURCHASED_WATER_ACCOUNT_NUMBER = 50,
                                 SOLD_WATER_ACCOUNT_NUMBER = 50,
                                 WATER_QUALITY = 255,
                                 LATITUDE = 50,
                                 LONGITUDE = 50;
            }
        }

        public struct ForeignKeyNames
        {
            public struct Interconnections
            {
                public const string PURCHASE_SELL_TRANSFER =
                                        "FK_Interconnections_InterconnectionPurchaseSellTransfer_PurchaseSellTransferId",
                                    OPERATING_STATUS =
                                        "FK_Interconnections_InterconnectionOperatingStatuses_OperatingStatusId",
                                    CATEGORY = "FK_Interconnections_InterconnectionCategories_CategoryId",
                                    DELIVERY_METHOD =
                                        "FK_Interconnections_InterconnectionDeliveryMethods_DeliveryMethodId",
                                    FLOW_CONTROL_METHOD =
                                        "FK_Interconnections_InterconnectionFlowControlMethods_FlowControlMethodId",
                                    DIRECTION = "FK_Interconnections_InterconnectionDirections_DirectionId",
                                    TYPE = "FK_Interconnections_InterconnectionTypes_TypeId",
                                    FACILITY = "FK_Interconnections_tblFacilities_FacilityId",
                                    INLET_PWSID = "FK_Interconnections_tblPWSID_InletPWSIDId",
                                    OUTLET_PWSID = "FK_Interconnections_tblPWSID_OutletPWSIDId",
                                    COORDINATE = "FK_Interconnections_Coordinates_CoordinateId";
            }

            public struct InterconnectionsMeters
            {
                public const string INTERCONNECTION = "FK_InterconnectionsMeters_Interconnections_InterconnectionId",
                                    METER = "FK_InterconnectionsMeters_Meters_MeterId";
            }
        }

        public override void Up()
        {
            Create.Table(TableNames.INTERCONNECTION_PURCHASE_SELL_TRANSFER)
                  .WithColumn(ColumnNames.InterconnectionPurchaseSellTransfer.ID).AsInt32().PrimaryKey().Identity()
                  .NotNullable()
                  .WithColumn(ColumnNames.InterconnectionPurchaseSellTransfer.DESCRIPTION)
                  .AsAnsiString(StringLengths.InterconnectionPurchaseSellTransfer.DESCRIPTION).NotNullable();

            Insert.IntoTable(TableNames.INTERCONNECTION_PURCHASE_SELL_TRANSFER).Row(new {Description = "Purchase"});
            Insert.IntoTable(TableNames.INTERCONNECTION_PURCHASE_SELL_TRANSFER).Row(new {Description = "Sell"});
            Insert.IntoTable(TableNames.INTERCONNECTION_PURCHASE_SELL_TRANSFER).Row(new {Description = "Transfer"});

            Create.Table(TableNames.INTERCONNECTION_OPERATING_STATUSES)
                  .WithColumn(ColumnNames.InterconnectionOperatingStatuses.ID).AsInt32().PrimaryKey().Identity()
                  .NotNullable()
                  .WithColumn(ColumnNames.InterconnectionOperatingStatuses.DESCRIPTION)
                  .AsAnsiString(StringLengths.InterconnectionOperatingStatuses.DESCRIPTION).NotNullable();

            Insert.IntoTable(TableNames.INTERCONNECTION_OPERATING_STATUSES).Row(new {Description = "Active"});
            Insert.IntoTable(TableNames.INTERCONNECTION_OPERATING_STATUSES).Row(new {Description = "Inactive"});

            Create.Table(TableNames.INTERCONNECTION_CATEGORIES)
                  .WithColumn(ColumnNames.InterconnectionCategories.ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(ColumnNames.InterconnectionCategories.DESCRIPTION)
                  .AsAnsiString(StringLengths.InterconnectionCategories.DESCRIPTION).NotNullable();

            Insert.IntoTable(TableNames.INTERCONNECTION_CATEGORIES).Row(new {Description = "Emergency"});
            Insert.IntoTable(TableNames.INTERCONNECTION_CATEGORIES).Row(new {Description = "Emergency Purchase"});
            Insert.IntoTable(TableNames.INTERCONNECTION_CATEGORIES).Row(new {Description = "Emergency Resale"});
            Insert.IntoTable(TableNames.INTERCONNECTION_CATEGORIES).Row(new {Description = "Purchase"});
            Insert.IntoTable(TableNames.INTERCONNECTION_CATEGORIES).Row(new {Description = "Resale"});

            Create.Table(TableNames.INTERCONNECTION_DELIVERY_METHODS)
                  .WithColumn(ColumnNames.InterconnectionDeliveryMethods.ID).AsInt32().PrimaryKey().Identity()
                  .NotNullable()
                  .WithColumn(ColumnNames.InterconnectionDeliveryMethods.DESCRIPTION)
                  .AsAnsiString(StringLengths.InterconnectionDeliveryMethods.DESCRIPTION).NotNullable();

            Insert.IntoTable(TableNames.INTERCONNECTION_DELIVERY_METHODS).Row(new {Description = "Flow Control"});
            Insert.IntoTable(TableNames.INTERCONNECTION_DELIVERY_METHODS).Row(new {Description = "Gravity Fed"});
            Insert.IntoTable(TableNames.INTERCONNECTION_DELIVERY_METHODS).Row(new {Description = "Pumped"});

            Create.Table(TableNames.INTERCONNECTION_FLOW_CONTROL_METHODS)
                  .WithColumn(ColumnNames.InterconnectionFlowControlMethods.ID).AsInt32().PrimaryKey().Identity()
                  .NotNullable()
                  .WithColumn(ColumnNames.InterconnectionFlowControlMethods.DESCRIPTION)
                  .AsAnsiString(StringLengths.InterconnectionFlowControlMethods.DESCRIPTION).NotNullable();

            Insert.IntoTable(TableNames.INTERCONNECTION_FLOW_CONTROL_METHODS).Row(new {Description = "Control Valve"});
            Insert.IntoTable(TableNames.INTERCONNECTION_FLOW_CONTROL_METHODS)
                  .Row(new {Description = "Pressure Control Valve"});
            Insert.IntoTable(TableNames.INTERCONNECTION_FLOW_CONTROL_METHODS).Row(new {Description = "No Control"});

            Create.Table(TableNames.INTERCONNECTION_DIRECTIONS)
                  .WithColumn(ColumnNames.InterconnectionDirections.ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(ColumnNames.InterconnectionDirections.DESCRIPTION)
                  .AsAnsiString(StringLengths.InterconnectionDirections.DESCRIPTION).NotNullable();

            Insert.IntoTable(TableNames.INTERCONNECTION_DIRECTIONS).Row(new {Description = "One-Way"});
            Insert.IntoTable(TableNames.INTERCONNECTION_DIRECTIONS).Row(new {Description = "Two-Way"});

            Create.Table(TableNames.INTERCONNECTION_TYPES)
                  .WithColumn(ColumnNames.InterconnectionTypes.ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(ColumnNames.InterconnectionTypes.DESCRIPTION)
                  .AsAnsiString(StringLengths.InterconnectionTypes.DESCRIPTION).NotNullable();

            Insert.IntoTable(TableNames.INTERCONNECTION_TYPES).Row(new {Description = "Emergency"});
            Insert.IntoTable(TableNames.INTERCONNECTION_TYPES).Row(new {Description = "Normal"});

            Create.Table(TableNames.INTERCONNECTIONS)
                  .WithColumn(ColumnNames.Interconnections.ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(ColumnNames.Interconnections.DATE_ADDED).AsDateTime().Nullable()
                  .WithColumn(ColumnNames.Interconnections.NJDEP_DESIGNATION)
                  .AsAnsiString(StringLengths.Interconnections.NJDEP_DESIGNATION).Nullable()
                  .WithColumn(ColumnNames.Interconnections.PURCHASE_SELL_TRANSFER_ID).AsInt32().ForeignKey(
                       ForeignKeyNames.Interconnections.PURCHASE_SELL_TRANSFER,
                       TableNames.INTERCONNECTION_PURCHASE_SELL_TRANSFER,
                       ColumnNames.InterconnectionPurchaseSellTransfer.ID).Nullable()
                  .WithColumn(ColumnNames.Interconnections.PROGRAM_INTEREST_NUMBER)
                  .AsAnsiString(StringLengths.Interconnections.PROGRAM_INTEREST_NUMBER).Nullable()
                  .WithColumn(ColumnNames.Interconnections.PURCHASED_WATER_ACCOUNT_NUMBER)
                  .AsAnsiString(StringLengths.Interconnections.PURCHASED_WATER_ACCOUNT_NUMBER).Nullable()
                  .WithColumn(ColumnNames.Interconnections.SOLD_WATER_ACCOUNT_NUMBER)
                  .AsAnsiString(StringLengths.Interconnections.SOLD_WATER_ACCOUNT_NUMBER).Nullable()
                  .WithColumn(ColumnNames.Interconnections.OPERATING_STATUS_ID).AsInt32().ForeignKey(
                       ForeignKeyNames.Interconnections.OPERATING_STATUS, TableNames.INTERCONNECTION_OPERATING_STATUSES,
                       ColumnNames.InterconnectionOperatingStatuses.ID).Nullable()
                  .WithColumn(ColumnNames.Interconnections.CATEGORY_ID).AsInt32()
                  .ForeignKey(ForeignKeyNames.Interconnections.CATEGORY, TableNames.INTERCONNECTION_CATEGORIES,
                       ColumnNames.InterconnectionCategories.ID).Nullable()
                  .WithColumn(ColumnNames.Interconnections.DIRECT_CONNECTION).AsBoolean().Nullable()
                  .WithColumn(ColumnNames.Interconnections.DELIVERY_METHOD_ID).AsInt32().ForeignKey(
                       ForeignKeyNames.Interconnections.DELIVERY_METHOD, TableNames.INTERCONNECTION_DELIVERY_METHODS,
                       ColumnNames.InterconnectionDeliveryMethods.ID).Nullable()
                  .WithColumn(ColumnNames.Interconnections.FLOW_CONTROL_METHOD_ID).AsInt32().ForeignKey(
                       ForeignKeyNames.Interconnections.FLOW_CONTROL_METHOD,
                       TableNames.INTERCONNECTION_FLOW_CONTROL_METHODS,
                       ColumnNames.InterconnectionFlowControlMethods.ID).Nullable()
                  .WithColumn(ColumnNames.Interconnections.INLET_CONNECTION_SIZE).AsInt32().Nullable()
                  .WithColumn(ColumnNames.Interconnections.OUTLET_CONNECTION_SIZE).AsInt32().Nullable()
                  .WithColumn(ColumnNames.Interconnections.INLET_STATIC_PRESSURE).AsInt32().Nullable()
                  .WithColumn(ColumnNames.Interconnections.OUTLET_STATIC_PRESSURE).AsInt32().Nullable()
                  .WithColumn(ColumnNames.Interconnections.MAXIMUM_FLOW_CAPACITY).AsFloat().Nullable()
                  .WithColumn(ColumnNames.Interconnections.MAXIMUM_FLOW_CAPACITY_STRESSED_CONDITION).AsFloat()
                  .Nullable()
                  .WithColumn(ColumnNames.Interconnections.DISTRIBUTION_PIPING_RESTRICTIONS).AsBoolean().Nullable()
                  .WithColumn(ColumnNames.Interconnections.WATER_QUALITY)
                  .AsAnsiString(StringLengths.Interconnections.WATER_QUALITY).Nullable()
                  .WithColumn(ColumnNames.Interconnections.FLUORIDATED_SUPPLY_DELIVERY_PURVEYOR).AsBoolean()
                  .NotNullable()
                  .WithColumn(ColumnNames.Interconnections.FLUORIDATED_SUPPLY_RECEIVING_PURVEYOR).AsBoolean()
                  .NotNullable()
                  .WithColumn(ColumnNames.Interconnections.CHLORAMINE_RESIDUAL_DELIVERY_PURVEYOR).AsBoolean()
                  .NotNullable()
                  .WithColumn(ColumnNames.Interconnections.CHLORAMINE_RESIDUAL_RECEIVING_PURVEYOR).AsBoolean()
                  .NotNullable()
                  .WithColumn(ColumnNames.Interconnections.CORROSION_INHIBITOR_DELIVERY_PURVEYOR).AsBoolean()
                  .NotNullable()
                  .WithColumn(ColumnNames.Interconnections.CORROSION_INHIBITOR_RECEIVING_PURVEYOR).AsBoolean()
                  .NotNullable()
                  .WithColumn(ColumnNames.Interconnections.REVERSIBLE_CAPACITY).AsFloat().Nullable()
                  .WithColumn(ColumnNames.Interconnections.DIRECTION_ID).AsInt32()
                  .ForeignKey(ForeignKeyNames.Interconnections.DIRECTION, TableNames.INTERCONNECTION_DIRECTIONS,
                       ColumnNames.InterconnectionDirections.ID).Nullable()
                  .WithColumn(ColumnNames.Interconnections.TYPE_ID).AsInt32()
                  .ForeignKey(ForeignKeyNames.Interconnections.TYPE, TableNames.INTERCONNECTION_TYPES,
                       ColumnNames.InterconnectionTypes.ID).Nullable()
                  .WithColumn(ColumnNames.Interconnections.ANNUAL_TEST_REQUIRED).AsBoolean().NotNullable()
                  .WithColumn(ColumnNames.Interconnections.LATITUDE)
                  .AsAnsiString(StringLengths.Interconnections.LATITUDE).Nullable()
                  .WithColumn(ColumnNames.Interconnections.LONGITUDE)
                  .AsAnsiString(StringLengths.Interconnections.LONGITUDE).Nullable()
                  .WithColumn(ColumnNames.Interconnections.CONTRACT).AsBoolean().Nullable()
                  .WithColumn(ColumnNames.Interconnections.CONTRACT_MAX_SUMMER).AsFloat().Nullable()
                  .WithColumn(ColumnNames.Interconnections.CONTRACT_MIN_SUMMER).AsFloat().Nullable()
                  .WithColumn(ColumnNames.Interconnections.CONTRACT_MAX_WINTER).AsFloat().Nullable()
                  .WithColumn(ColumnNames.Interconnections.CONTRACT_MIN_WINTER).AsFloat().Nullable()
                  .WithColumn(ColumnNames.Interconnections.FACILITY_ID).AsInt32()
                  .ForeignKey(ForeignKeyNames.Interconnections.FACILITY, "tblFacilities", "RecordId").Nullable()
                  .WithColumn(ColumnNames.Interconnections.INLET_PWSID_ID).AsInt32()
                  .ForeignKey(ForeignKeyNames.Interconnections.INLET_PWSID, "tblPWSID", "RecordId").Nullable()
                  .WithColumn(ColumnNames.Interconnections.OUTLET_PWSID_ID).AsInt32()
                  .ForeignKey(ForeignKeyNames.Interconnections.OUTLET_PWSID, "tblPWSID", "RecordId").Nullable()
                  .WithColumn(ColumnNames.Interconnections.COORDINATE_ID).AsInt32()
                  .ForeignKey(ForeignKeyNames.Interconnections.COORDINATE, "Coordinates", "CoordinateId").Nullable();
            Create.Table(TableNames.INTERCONNECTIONS_METERS)
                  .WithColumn(ColumnNames.InterconnectionsMeters.INTERCONNECTION_ID).AsInt32().ForeignKey(
                       ForeignKeyNames.InterconnectionsMeters.INTERCONNECTION, TableNames.INTERCONNECTIONS,
                       ColumnNames.Interconnections.ID)
                  .WithColumn(ColumnNames.InterconnectionsMeters.METER_ID).AsInt32()
                  .ForeignKey(ForeignKeyNames.InterconnectionsMeters.METER, "Meters", "MeterId");
        }

        public override void Down()
        {
            Delete.Table(TableNames.INTERCONNECTIONS_METERS);
            Delete.Table(TableNames.INTERCONNECTIONS);
            Delete.Table(TableNames.INTERCONNECTION_TYPES);
            Delete.Table(TableNames.INTERCONNECTION_DIRECTIONS);
            Delete.Table(TableNames.INTERCONNECTION_FLOW_CONTROL_METHODS);
            Delete.Table(TableNames.INTERCONNECTION_DELIVERY_METHODS);
            Delete.Table(TableNames.INTERCONNECTION_CATEGORIES);
            Delete.Table(TableNames.INTERCONNECTION_OPERATING_STATUSES);
            Delete.Table(TableNames.INTERCONNECTION_PURCHASE_SELL_TRANSFER);
        }
    }
}
