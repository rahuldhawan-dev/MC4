using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191015124537764), Tags("Production")]
    public class RenameAndCleanupTablesForMC1630 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "delete from Document where exists (select 1 from DocumentType where Document.documentTypeID = DocumentType.DocumentTypeID and exists (select 1 from DataType where DocumentType.DataTypeID = DataType.DataTypeID and Table_Name like '%chemical%' and Table_Name like 'tbl%'))");
            Execute.Sql(
                "delete from DocumentType where exists (select 1 from DataType where DocumentType.DataTypeID = DataType.DataTypeID and Table_Name like '%chemical%' and Table_Name like 'tbl%')");
            Execute.Sql("delete from DataType where Table_Name like '%chemical%' and Table_Name like 'tbl%'");

            #region Chemicals

            Rename.Table("tblChemicals").To("Chemicals");
            Rename.Column("Chemical_ID").OnTable("Chemicals").To("Id");
            Rename.Column("ChemicalName").OnTable("Chemicals").To("Name");
            Rename.Column("ChemPartNumber").OnTable("Chemicals").To("PartNumber");
            Rename.Column("ChemicalConcentration_Liquid").OnTable("Chemicals").To("ChemicalConcentrationLiquid");
            Rename.Column("Concentration_lbsPerGal").OnTable("Chemicals").To("ConcentrationLBSPerGal");
            Rename.Column("CAS_Number").OnTable("Chemicals").To("CASNumber");

            Alter.Table("Chemicals").AlterColumn("Name").AsAnsiString(61).NotNullable();

            this.AddDataType("Chemicals");
            this.AddDocumentType("Chemical Document", "Chemicals");

            #endregion

            #region Vendors

            Rename.Table("tblChemicals_Vendors").To("ChemicalVendors");
            Rename.Column("Vendor_ID").OnTable("ChemicalVendors").To("Id");
            Rename.Column("Vendor_ID_JDE").OnTable("ChemicalVendors").To("JDEVendorId");
            Rename.Column("Order_Contact").OnTable("ChemicalVendors").To("OrderContact");
            Rename.Column("Phone_Office").OnTable("ChemicalVendors").To("PhoneOffice");
            Rename.Column("Phone_Cell").OnTable("ChemicalVendors").To("PhoneCell");
            Delete.Column("Latitude").FromTable("ChemicalVendors");
            Delete.Column("Longitiude").FromTable("ChemicalVendors");

            Delete.FromTable("ChemicalVendors").AllRows();

            Alter.Table("ChemicalVendors")
                 .AlterColumn("Vendor").AsAnsiString(50).NotNullable()
                 .AddForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateId");

            this.AddDataType("ChemicalVendors");
            this.AddDocumentType("Chemical Vendor Document", "ChemicalVendors");

            #endregion

            #region Warehouse Numbers

            Rename.Table("tblChemicals_Warehouse_Numbers").To("ChemicalWarehouseNumbers");
            Rename.Column("Chemical_Warehouse_ID").OnTable("ChemicalWarehouseNumbers").To("Id");
            Alter.Table("ChemicalWarehouseNumbers")
                 .AddForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId");

            Execute.Sql(
                "update ChemicalWarehouseNumbers set OperatingCenterId = OperatingCenters.OperatingCenterId from OperatingCenters where OperatingCenters.OperatingCenterCode = ChemicalWarehouseNumbers.OpCode;");

            Alter.Column("OperatingCenterId").OnTable("ChemicalWarehouseNumbers").AsInt32().NotNullable();
            Alter.Column("WarehouseNumber").OnTable("ChemicalWarehouseNumbers").AsAnsiString(15).NotNullable();

            Delete.Column("OpCode").FromTable("ChemicalWarehouseNumbers");

            this.AddDataType("ChemicalWarehouseNumbers");
            this.AddDocumentType("Chemical Warehouse Number Document", "ChemicalWarehouseNumbers");

            #endregion

            #region Unit Costs

            Rename.Table("tblChemicals_Unit_Cost").To("ChemicalUnitCosts");
            Rename.Column("Chemical_Unit_Cost_ID").OnTable("ChemicalUnitCosts").To("Id");
            Rename.Column("Chemical_ID").OnTable("ChemicalUnitCosts").To("ChemicalId");
            Rename.Column("Start_Date").OnTable("ChemicalUnitCosts").To("StartDate");
            Rename.Column("End_Date").OnTable("ChemicalUnitCosts").To("EndDate");
            Rename.Column("PO_Number").OnTable("ChemicalUnitCosts").To("PONumber");
            Rename.Column("Chemical_Lead_Time_Days").OnTable("ChemicalUnitCosts").To("ChemicalLeadTimeDays");
            Rename.Column("Chemical_Ordering_Process").OnTable("ChemicalUnitCosts").To("ChemicalOrderingProcess");

            Delete.Column("Vendor_ID").FromTable("ChemicalUnitCosts");
            Delete.FromTable("ChemicalUnitCosts").AllRows();

            Alter.Table("ChemicalUnitCosts")
                 .AddForeignKeyColumn("VendorId", "ChemicalVendors")
                 .AddForeignKeyColumn("WarehouseNumberId", "ChemicalWarehouseNumbers")
                 .AlterColumn("ChemicalId").AsInt32().NotNullable();

            Execute.Sql(
                "UPDATE ChemicalUnitCosts SET WarehouseNumberId = ChemicalWarehouseNumbers.Id FROM ChemicalWarehouseNumbers WHERE ltrim(rtrim(ChemicalWarehouseNumbers.WarehouseNumber)) = ltrim(rtrim(ChemicalUnitCosts.WarehouseNumber));");

            Delete.Column("WarehouseNumber").FromTable("ChemicalUnitCosts");

            this.AddDataType("ChemicalUnitCosts");
            this.AddDocumentType("Chemical Unit Cost Document", "ChemicalUnitCosts");

            #endregion

            #region Storage

            Rename.Table("tblChemicals_Storage").To("ChemicalStorage");
            Rename.Column("Chemical_Storage_ID").OnTable("ChemicalStorage").To("Id");
            Rename.Column("Chemical_ID").OnTable("ChemicalStorage").To("ChemicalID");
            Rename.Column("Max_Storage_Quantity_Gallons").OnTable("ChemicalStorage").To("MaxStorageQuantityGallons");
            Rename.Column("Dead_Storage_Quantity_Gallons").OnTable("ChemicalStorage").To("DeadStorageQuantityGallons");
            Rename.Column("Min_Storage_Quantity_Gallons").OnTable("ChemicalStorage").To("MinStorageQuantityGallons");
            Rename.Column("Max_Storage_Quantity_Pounds").OnTable("ChemicalStorage").To("MaxStorageQuantityPounds");
            Rename.Column("Dead_Storage_Quantity_Pounds").OnTable("ChemicalStorage").To("DeadStorageQuantityPounds");
            Rename.Column("Min_Storage_Volume_Pounds").OnTable("ChemicalStorage").To("MinStorageVolumePounds");
            Rename.Column("Reorder_Level_Non_Peak_Production_Gallons").OnTable("ChemicalStorage")
                  .To("ReorderLevelNonPeakProductionGallons");
            Rename.Column("Reorder_Level_Peak_Production_Gallons").OnTable("ChemicalStorage")
                  .To("ReorderLevelPeakProductionGallons");
            Rename.Column("Reorder_Level_Non_Peak_Production_Pounds").OnTable("ChemicalStorage")
                  .To("ReorderLevelNonPeakProductionPounds");
            Rename.Column("Reorder_Level_Peak_Production_Pounds").OnTable("ChemicalStorage")
                  .To("ReorderLevelPeakProductionPounds");
            Rename.Column("Typical_Order_Quantity_Gallons").OnTable("ChemicalStorage")
                  .To("TypicalOrderQuantityGallons");
            Rename.Column("Typical_Order_Quantity_Pounds").OnTable("ChemicalStorage").To("TypicalOrderQuantityPounds");
            Rename.Column("Delivery_Instructions").OnTable("ChemicalStorage").To("DeliveryInstructions");
            Rename.Column("FacilityId").OnTable("ChemicalStorage").To("Facility");

            Execute.Sql(
                "IF EXISTS (SELECT Name FROM sysindexes WHERE Name = 'IX_tblChemicals_Storage_DataTextField') DROP INDEX IX_tblChemicals_Storage_DataTextField on ChemicalStorage;");

            Alter.Table("ChemicalStorage")
                 .AlterColumn("ChemicalId").AsInt32().NotNullable()
                 .AddForeignKeyColumn("WarehouseNumberId", "ChemicalWarehouseNumbers")
                 .AddForeignKeyColumn("FacilityId", "tblFacilities", "RecordId");

            Execute.Sql(
                "UPDATE ChemicalStorage SET WarehouseNumberId = ChemicalWarehouseNumbers.Id FROM ChemicalWarehouseNumbers WHERE ltrim(rtrim(ChemicalWarehouseNumbers.WarehouseNumber)) = ltrim(rtrim(ChemicalStorage.WarehouseNumber));");
            Execute.Sql(
                "UPDATE ChemicalStorage SET FacilityId = tblFacilities.RecordId FROM tblFacilities WHERE ltrim(rtrim(tblFacilities.FacilityId)) = ltrim(rtrim(ChemicalStorage.Facility));");

            Delete.Column("WarehouseNumber").FromTable("ChemicalStorage");
            Delete.Column("Facility").FromTable("ChemicalStorage");

            this.AddDataType("ChemicalStorage");
            this.AddDocumentType("Chemical Storage Document", "ChemicalStorage");

            #endregion

            #region Deliveries

            Rename.Table("tblChemicals_Deliveries").To("ChemicalDeliveries");
            Rename.Column("Chemical_Delivery_Id").OnTable("ChemicalDeliveries").To("Id");
            Rename.Column("Chemical_Storage_Id").OnTable("ChemicalDeliveries").To("StorageId");
            Rename.Column("Chemical_Unit_Cost_ID").OnTable("ChemicalDeliveries").To("UnitCostId");
            Rename.Column("Chemical_ID").OnTable("ChemicalDeliveries").To("ChemicalId");
            Rename.Column("Date_Ordered").OnTable("ChemicalDeliveries").To("DateOrdered");
            Rename.Column("Scheduled_Delivery_Date").OnTable("ChemicalDeliveries").To("ScheduledDeliveryDate");
            Rename.Column("Actual_Delivery_Date").OnTable("ChemicalDeliveries").To("ActualDeliveryDate");
            Rename.Column("Confirmation_Information").OnTable("ChemicalDeliveries").To("ConfirmationInformation");
            Rename.Column("Receipt_Number_JDE").OnTable("ChemicalDeliveries").To("ReceiptNumberJDE");
            Rename.Column("Batch_Number_JDE").OnTable("ChemicalDeliveries").To("BatchNumberJDE");
            Rename.Column("Estimated_Delivery_Quantity_Gallons").OnTable("ChemicalDeliveries")
                  .To("EstimatedDeliveryQuantityGallons");
            Rename.Column("Actual_Delivery_Quantity_Gallons").OnTable("ChemicalDeliveries")
                  .To("ActualDeliveryQuantityGallons");
            Rename.Column("Estimated_Delivery_Quantity_Pounds").OnTable("ChemicalDeliveries")
                  .To("EstimatedDeliveryQuantityPounds");
            Rename.Column("Actual_Delivery_Quantity_Pounds").OnTable("ChemicalDeliveries")
                  .To("ActualDeliveryQuantityPounds");
            Rename.Column("Delivery_Ticket_Number").OnTable("ChemicalDeliveries").To("DeliveryTicketNumber");
            Rename.Column("Delivery_Instructions").OnTable("ChemicalDeliveries").To("DeliveryInstructions");
            Rename.Column("Split_Facility_Delivery").OnTable("ChemicalDeliveries").To("SplitFacilityDelivery");
            Rename.Column("Security_Information").OnTable("ChemicalDeliveries").To("SecurityInformation");

            Alter.Column("UnitCostId").OnTable("ChemicalDeliveries").AsForeignKey("UnitCostId", "ChemicalUnitCosts");
            Alter.Table("ChemicalDeliveries").AlterColumn("StorageId").AsInt32().NotNullable();
            Alter.Table("ChemicalDeliveries").AlterColumn("ChemicalId").AsInt32().NotNullable();

            Delete.Column("Notes").FromTable("ChemicalDeliveries");

            this.AddDataType("ChemicalDeliveries");
            this.AddDocumentType("Chemical Delivery Document", "ChemicalDeliveries");

            #endregion

            #region Inventory Transactions

            Rename.Table("tblChemicals_Inventory_Transactions").To("ChemicalInventoryTransactions");
            Rename.Column("ChemicalInventoryTransactionId").OnTable("ChemicalInventoryTransactions").To("Id");
            Rename.Column("Quantity_Gallons").OnTable("ChemicalInventoryTransactions").To("QuantityGallons");
            Rename.Column("Quantity_Pounds").OnTable("ChemicalInventoryTransactions").To("QuantityPounds");
            Rename.Column("Inventory_Record_Type").OnTable("ChemicalInventoryTransactions").To("InventoryRecordType");

            Delete.FromTable("ChemicalInventoryTransactions").AllRows();
            Alter.Table("ChemicalInventoryTransactions")
                 .AddForeignKeyColumn("StorageId", "ChemicalStorage", nullable: false)
                 .AddForeignKeyColumn("DeliveryId", "ChemicalDeliveries");

            Alter.Column("Date").OnTable("ChemicalInventoryTransactions").AsDateTime().Nullable();

            Delete.Column("Chemical_Storage_ID").FromTable("ChemicalInventoryTransactions");
            Delete.Column("Chemical_Delivery_ID").FromTable("ChemicalInventoryTransactions");
            Delete.Column("Chemical_Inventory_ID").FromTable("ChemicalInventoryTransactions");
            Delete.Column("Notes").FromTable("ChemicalInventoryTransactions");

            this.AddDataType("ChemicalInventoryTransactions");
            this.AddDocumentType("Chemical Inventory Transaction Document", "ChemicalInventoryTransactions");

            #endregion
        }

        public override void Down()
        {
            #region Inventory Transactions

            this.RemoveDocumentTypeAndAllRelatedDocuments("Chemical Inventory Transaction Document",
                "ChemicalInventoryTransactions");
            this.RemoveDataType("ChemicalInventoryTransactions");

            Alter.Column("Date").OnTable("ChemicalInventoryTransactions").AsAnsiString(255).Nullable();

            Create.Column("Chemical_Delivery_ID").OnTable("ChemicalInventoryTransactions").AsAnsiString(255).Nullable();
            Create.Column("Chemical_Inventory_ID").OnTable("ChemicalInventoryTransactions").AsFloat().Nullable();
            Create.Column("Chemical_Storage_ID").OnTable("ChemicalInventoryTransactions").AsInt32().Nullable();
            Create.Column("Notes").OnTable("ChemicalInventoryTransactions").AsCustom("NVARCHAR(MAX)").Nullable();

            Execute.Sql("UPDATE ChemicalInventoryTransactions SET Chemical_Storage_ID = StorageId");

            Delete.ForeignKeyColumn("ChemicalInventoryTransactions", "StorageId", "ChemicalStorage");
            Delete.ForeignKeyColumn("ChemicalInventoryTransactions", "DeliveryId", "ChemicalDeliveries");

            Rename.Column("InventoryRecordType").OnTable("ChemicalInventoryTransactions").To("Inventory_Record_Type");
            Rename.Column("QuantityPounds").OnTable("ChemicalInventoryTransactions").To("Quantity_Pounds");
            Rename.Column("QuantityGallons").OnTable("ChemicalInventoryTransactions").To("Quantity_Gallons");
            Rename.Column("Id").OnTable("ChemicalInventoryTransactions").To("ChemicalInventoryTransactionId");
            Rename.Table("ChemicalInventoryTransactions").To("tblChemicals_Inventory_Transactions");

            #endregion

            #region Deliveries

            this.RemoveDocumentTypeAndAllRelatedDocuments("Chemical Delivery Document", "ChemicalDeliveries");
            this.RemoveDataType("ChemicalDeliveries");

            Create.Column("Notes").OnTable("ChemicalDeliveries").AsCustom("NVARCHAR(MAX)").Nullable();

            Delete.ForeignKey("FK_ChemicalDeliveries_ChemicalUnitCosts_UnitCostId").OnTable("ChemicalDeliveries");

            Rename.Column("SecurityInformation").OnTable("ChemicalDeliveries").To("Security_Information");
            Rename.Column("SplitFacilityDelivery").OnTable("ChemicalDeliveries").To("Split_Facility_Delivery");
            Rename.Column("DeliveryInstructions").OnTable("ChemicalDeliveries").To("Delivery_Instructions");
            Rename.Column("DeliveryTicketNumber").OnTable("ChemicalDeliveries").To("Delivery_Ticket_Number");
            Rename.Column("ActualDeliveryQuantityPounds").OnTable("ChemicalDeliveries")
                  .To("Actual_Delivery_Quantity_Pounds");
            Rename.Column("EstimatedDeliveryQuantityPounds").OnTable("ChemicalDeliveries")
                  .To("Estimated_Delivery_Quantity_Pounds");
            Rename.Column("ActualDeliveryQuantityGallons").OnTable("ChemicalDeliveries")
                  .To("Actual_Delivery_Quantity_Gallons");
            Rename.Column("EstimatedDeliveryQuantityGallons").OnTable("ChemicalDeliveries")
                  .To("Estimated_Delivery_Quantity_Gallons");
            Rename.Column("BatchNumberJDE").OnTable("ChemicalDeliveries").To("Batch_Number_JDE");
            Rename.Column("ReceiptNumberJDE").OnTable("ChemicalDeliveries").To("Receipt_Number_JDE");
            Rename.Column("ConfirmationInformation").OnTable("ChemicalDeliveries").To("Confirmation_Information");
            Rename.Column("ActualDeliveryDate").OnTable("ChemicalDeliveries").To("Actual_Delivery_Date");
            Rename.Column("ScheduledDeliveryDate").OnTable("ChemicalDeliveries").To("Scheduled_Delivery_Date");
            Rename.Column("DateOrdered").OnTable("ChemicalDeliveries").To("Date_Ordered");
            Rename.Column("ChemicalId").OnTable("ChemicalDeliveries").To("Chemical_ID");
            Rename.Column("UnitCostId").OnTable("ChemicalDeliveries").To("Chemical_Unit_Cost_ID");
            Rename.Column("StorageId").OnTable("ChemicalDeliveries").To("Chemical_Storage_Id");
            Rename.Column("Id").OnTable("ChemicalDeliveries").To("Chemical_Delivery_Id");
            Rename.Table("ChemicalDeliveries").To("tblChemicals_Deliveries");

            #endregion

            #region Storage

            this.RemoveDocumentTypeAndAllRelatedDocuments("Chemical Storage Document", "ChemicalStorage");
            this.RemoveDataType("ChemicalStorage");

            Create.Column("WarehouseNumber").OnTable("ChemicalStorage").AsAnsiString(4).Nullable();
            Create.Column("Facility").OnTable("ChemicalStorage").AsAnsiString(10).Nullable();

            Execute.Sql(
                "UPDATE ChemicalStorage SET WarehouseNumber = ChemicalWarehouseNumbers.WarehouseNumber FROM ChemicalWarehouseNumbers WHERE ChemicalWarehouseNumbers.Id = ChemicalStorage.WarehouseNumberId;");
            Execute.Sql(
                "UPDATE ChemicalStorage SET Facility = tblFacilities.FacilityId FROM tblFacilities WHERE tblFacilities.RecordId = ChemicalStorage.FacilityId;");

            Delete.ForeignKeyColumn("ChemicalStorage", "WarehouseNumberId", "ChemicalWarehouseNumbers");
            Delete.ForeignKeyColumn("ChemicalStorage", "FacilityId", "tblFacilities", "RecordId");

            Rename.Column("Facility").OnTable("ChemicalStorage").To("FacilityId");
            Rename.Column("DeliveryInstructions").OnTable("ChemicalStorage").To("Delivery_Instructions");
            Rename.Column("TypicalOrderQuantityPounds").OnTable("ChemicalStorage").To("Typical_Order_Quantity_Pounds");
            Rename.Column("TypicalOrderQuantityGallons").OnTable("ChemicalStorage")
                  .To("Typical_Order_Quantity_Gallons");
            Rename.Column("ReorderLevelPeakProductionPounds").OnTable("ChemicalStorage")
                  .To("Reorder_Level_Peak_Production_Pounds");
            Rename.Column("ReorderLevelNonPeakProductionPounds").OnTable("ChemicalStorage")
                  .To("Reorder_Level_Non_Peak_Production_Pounds");
            Rename.Column("ReorderLevelPeakProductionGallons").OnTable("ChemicalStorage")
                  .To("Reorder_Level_Peak_Production_Gallons");
            Rename.Column("ReorderLevelNonPeakProductionGallons").OnTable("ChemicalStorage")
                  .To("Reorder_Level_Non_Peak_Production_Gallons");
            Rename.Column("MinStorageVolumePounds").OnTable("ChemicalStorage").To("Min_Storage_Volume_Pounds");
            Rename.Column("DeadStorageQuantityPounds").OnTable("ChemicalStorage").To("Dead_Storage_Quantity_Pounds");
            Rename.Column("MaxStorageQuantityPounds").OnTable("ChemicalStorage").To("Max_Storage_Quantity_Pounds");
            Rename.Column("MinStorageQuantityGallons").OnTable("ChemicalStorage").To("Min_Storage_Quantity_Gallons");
            Rename.Column("DeadStorageQuantityGallons").OnTable("ChemicalStorage").To("Dead_Storage_Quantity_Gallons");
            Rename.Column("MaxStorageQuantityGallons").OnTable("ChemicalStorage").To("Max_Storage_Quantity_Gallons");
            Rename.Column("ChemicalID").OnTable("ChemicalStorage").To("Chemical_ID");
            Rename.Column("Id").OnTable("ChemicalStorage").To("Chemical_Storage_ID");
            Rename.Table("ChemicalStorage").To("tblChemicals_Storage");

            #endregion

            #region Unit Costs

            this.RemoveDocumentTypeAndAllRelatedDocuments("Chemical Unit Cost Document", "ChemicalUnitCosts");
            this.RemoveDataType("ChemicalUnitCosts");

            Create.Column("WarehouseNumber").OnTable("ChemicalUnitCosts").AsAnsiString(4).Nullable();

            Execute.Sql(
                "UPDATE ChemicalUnitCosts SET WarehouseNumber = ChemicalWarehouseNumbers.WarehouseNumber FROM ChemicalWarehouseNumbers WHERE ChemicalWarehouseNumbers.Id = ChemicalUnitCosts.WarehouseNumberId;");

            Delete.ForeignKeyColumn("ChemicalUnitCosts", "WarehouseNumberId", "ChemicalWarehouseNumbers");
            Delete.ForeignKeyColumn("ChemicalUnitCosts", "VendorId", "ChemicalVendors");

            Create.Column("Vendor_ID").OnTable("ChemicalUnitCosts").AsInt32().Nullable();

            Rename.Column("ChemicalOrderingProcess").OnTable("ChemicalUnitCosts").To("Chemical_Ordering_Process");
            Rename.Column("ChemicalLeadTimeDays").OnTable("ChemicalUnitCosts").To("Chemical_Lead_Time_Days");
            Rename.Column("PONumber").OnTable("ChemicalUnitCosts").To("PO_Number");
            Rename.Column("EndDate").OnTable("ChemicalUnitCosts").To("End_Date");
            Rename.Column("StartDate").OnTable("ChemicalUnitCosts").To("Start_Date");
            Rename.Column("ChemicalId").OnTable("ChemicalUnitCosts").To("Chemical_ID");
            Rename.Column("Id").OnTable("ChemicalUnitCosts").To("Chemical_Unit_Cost_ID");
            Rename.Table("ChemicalUnitCosts").To("tblChemicals_Unit_Cost");

            #endregion

            #region Warehouse Numbers

            this.RemoveDocumentTypeAndAllRelatedDocuments("Chemical Warehouse Number Document",
                "ChemicalWarehouseNumbers");
            this.RemoveDataType("ChemicalWarehouseNumbers");

            Create.Column("OpCode").OnTable("ChemicalWarehouseNumbers").AsString(5).Nullable();

            Execute.Sql(
                "update ChemicalWarehouseNumbers set OpCode = OperatingCenters.OperatingCenterCode from OperatingCenters where OperatingCenters.OperatingCenterId = ChemicalWarehouseNumbers.OperatingCenterId;");

            Delete.ForeignKeyColumn("ChemicalWarehouseNumbers", "OperatingCenterId", "OperatingCenters",
                "OperatingCenterId");

            Rename.Column("Id").OnTable("ChemicalWarehouseNumbers").To("Chemical_Warehouse_ID");
            Rename.Table("ChemicalWarehouseNumbers").To("tblChemicals_Warehouse_Numbers");

            #endregion

            #region Vendors

            this.RemoveDocumentTypeAndAllRelatedDocuments("Chemical Vendor Document", "ChemicalVendors");
            this.RemoveDataType("ChemicalVendors");

            Delete.ForeignKeyColumn("ChemicalVendors", "CoordinateId", "Coordinates", "CoordinateId");

            Create.Column("Latitude").OnTable("ChemicalVendors").AsAnsiString(25);
            Create.Column("Longitiude").OnTable("ChemicalVendors").AsAnsiString(25);

            Rename.Column("PhoneCell").OnTable("ChemicalVendors").To("Phone_Cell");
            Rename.Column("PhoneOffice").OnTable("ChemicalVendors").To("Phone_Office");
            Rename.Column("OrderContact").OnTable("ChemicalVendors").To("Order_Contact");
            Rename.Column("JDEVendorId").OnTable("ChemicalVendors").To("Vendor_ID_JDE");
            Rename.Column("Id").OnTable("ChemicalVendors").To("Vendor_ID");
            Rename.Table("ChemicalVendors").To("tblChemicals_Vendors");

            #endregion

            #region Chemicals

            this.RemoveDocumentTypeAndAllRelatedDocuments("Chemical Document", "Chemicals");
            this.RemoveDataType("Chemicals");

            Rename.Column("CASNumber").OnTable("Chemicals").To("CAS_Number");
            Rename.Column("ConcentrationLBSPerGal").OnTable("Chemicals").To("Concentration_lbsPerGal");
            Rename.Column("ChemicalConcentrationLiquid").OnTable("Chemicals").To("ChemicalConcentration_Liquid");
            Rename.Column("PartNumber").OnTable("Chemicals").To("ChemPartNumber");
            Rename.Column("Name").OnTable("Chemicals").To("ChemicalName");
            Rename.Column("Id").OnTable("Chemicals").To("Chemical_ID");
            Rename.Table("Chemicals").To("tblChemicals");

            #endregion
        }
    }
}
