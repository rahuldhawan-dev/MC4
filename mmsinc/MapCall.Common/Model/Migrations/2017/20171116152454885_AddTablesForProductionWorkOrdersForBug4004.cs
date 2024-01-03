using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20171116152454885), Tags("Production")]
    public class AddTablesForProductionWorkOrdersForBug4004 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM modules WHERE [Name] = 'Production Work Management' AND ApplicationID = 2) INSERT INTO Modules Values(2, 'Production Work Management');");
            Create.Table("OrderTypes")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsAnsiString(50).NotNullable()
                  .WithColumn("SAPCode").AsAnsiString(4).NotNullable();
            Execute.Sql("INSERT INTO OrderTypes Values('Operational Activity', '0010');" +
                        "INSERT INTO OrderTypes Values('PM Work Order', '0011');" +
                        "INSERT INTO OrderTypes Values('Corrective Action', '0020');" +
                        "INSERT INTO OrderTypes Values('RP Capital', '0040');");
            this.CreateLookupTableWithValues("ProductionSkillSets", "PR", "MAIN_L", "MAIN_SP", "MS", "TD");

            Alter.Table("PlantMaintenanceActivityTypes").AddForeignKeyColumn("OrderTypeId", "OrderTypes").Nullable();
            Execute.Sql(
                "UPDATE PlantMaintenanceActivityTypes SET OrderTypeId = (SELECT Id from OrderTypes where SAPCode = '0040') where Code = 'RBS';");
            Execute.Sql(
                "UPDATE PlantMaintenanceActivityTypes SET OrderTypeId = (SELECT Id from OrderTypes where SAPCode = '0040') where Code = 'RPQ';");

            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'Reactive Corrective',  'F01', Id from OrderTypes where SAPCode = '0020';");
            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'Emergency Corrective', 'F02', Id from OrderTypes where SAPCode = '0020';");
            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'Result of Inspection', 'F03', Id from OrderTypes where SAPCode = '0020';");
            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'Corrective Training',  'F04', Id from OrderTypes where SAPCode = '0020';");

            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'Predictive Inspection',     'P01', Id from OrderTypes where SAPCode = '0011';");
            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'Predictive Re-Inspection',  'P02', Id from OrderTypes where SAPCode = '0011';");
            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'Preventive Inspection',     'P03', Id from OrderTypes where SAPCode = '0011';");
            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'Preventive Re-Inspection',  'P04', Id from OrderTypes where SAPCode = '0011';");
            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'Preventive Maint Training', 'P07', Id from OrderTypes where SAPCode = '0011';");

            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'General Operations', 'OA1', Id from OrderTypes where SAPCode = '0010';");
            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'Chemical Delivery',  'OA2', Id from OrderTypes where SAPCode = '0010';");
            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'Chemical Issues',    'OA3', Id from OrderTypes where SAPCode = '0010';");
            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'Basin Cleaning',     'OA4', Id from OrderTypes where SAPCode = '0010';");
            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'Training',           'OA5', Id from OrderTypes where SAPCode = '0010';");
            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'Facility Cleaning',  'OA6', Id from OrderTypes where SAPCode = '0010';");
            Execute.Sql(
                "INSERT INTO PlantMaintenanceActivityTypes SELECT 'Landscaping',        'OA7', Id from OrderTypes where SAPCode = '0010';");
            this.CreateLookupTableWithValues("ProductionWorkOrderCancellationReasons", "Created In Error",
                "Customer Request", "Company Error", "Order Past Expiration Date",
                "No Longer Valid", "Supervisor Instructed", "Work Already Completed");
            Alter.Table("ProductionWorkOrderCancellationReasons").AddColumn("SAPCode").AsAnsiString(40).Nullable();
            Execute.Sql(
                "UPDATE ProductionWorkOrderCancellationReasons SET SAPCode = 'CERR' WHERE Description = 'Created In Error';");
            Execute.Sql(
                "UPDATE ProductionWorkOrderCancellationReasons SET SAPCode = 'CUST' WHERE Description = 'Customer Request';");
            Execute.Sql(
                "UPDATE ProductionWorkOrderCancellationReasons SET SAPCode = 'ERROR' WHERE Description = 'Company Error';");
            Execute.Sql(
                "UPDATE ProductionWorkOrderCancellationReasons SET SAPCode = 'EXPR' WHERE Description = 'Order Past Expiration Date';");
            Execute.Sql(
                "UPDATE ProductionWorkOrderCancellationReasons SET SAPCode = 'NVAL' WHERE Description = 'No Longer Valid';");
            Execute.Sql(
                "UPDATE ProductionWorkOrderCancellationReasons SET SAPCode = 'SUPI' WHERE Description = 'Supervisor Instructed';");
            Execute.Sql(
                "UPDATE ProductionWorkOrderCancellationReasons SET SAPCode = 'WCOM' WHERE Description = 'Work Already Completed';");
            Execute.Sql(
                "UPDATE ProductionWorkOrderCancellationReasons SET SAPCode = 'WCOM' WHERE Description = 'Order was Capitalized';");
            Create.Table("ProductionWorkDescriptions")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsAnsiString(50).NotNullable()
                  .WithForeignKeyColumn("EquipmentClassId", "SAPEquipmentTypes")
                  .WithForeignKeyColumn("PlantMaintenanceActivityTypeId", "PlantMaintenanceActivityTypes")
                  .WithForeignKeyColumn("OrderTypeId", "OrderTypes")
                  .WithForeignKeyColumn("ProductionSkillSetId", "ProductionSkillSets")
                  .WithColumn("BreakdownIndicator").AsBoolean().NotNullable().WithDefaultValue(false);

            Create.Table("ProductionWorkOrders")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID", false)
                  .WithForeignKeyColumn("PlanningPlantId", "PlanningPlants")
                  .WithForeignKeyColumn("FacilityId", "tblFacilities", "RecordId", false)
                  .WithColumn("FunctionalLocation").AsAnsiString(30)
                  .Nullable()
                  .WithForeignKeyColumn("EquipmentClassId", "SAPEquipmentTypes")
                  .WithForeignKeyColumn("EquipmentId", "Equipment", "EquipmentId")
                  .WithForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateId", false)
                   //Confirmed with Doug? -- same as 271 priorities
                  .WithForeignKeyColumn("PriorityId", "WorkOrderPriorities", "WorkOrderPriorityID", false)

                   //
                  .WithForeignKeyColumn("ProductionWorkDescriptionId", "ProductionWorkDescriptions", nullable: false)
                   //.WithForeignKeyColumn("OrderTypeId", "OrderTypes", "Id", false)
                   //.WithForeignKeyColumn("PlantMaintenanceActivityTypeId", "PlantMaintenanceActivityTypes")

                   //Confirmed with Doug? -- yes, only employee, default to current employee/user
                  .WithForeignKeyColumn("RequestedById", "tblEmployee", "tblEmployeeId")
                   //Permit Required* -- LOGICAL?

                   //Notes*
                  .WithColumn("Notes").AsAnsiString(255).Nullable()
                   //Date Received
                  .WithColumn("DateReceived").AsDateTime().NotNullable()
                  .WithColumn("BreakdownIndicator").AsBoolean().NotNullable().WithDefaultValue(false)
                  .WithColumn("SAPWorkOrder").AsAnsiString(50).Nullable()
                  .WithColumn("SAPErrorCode").AsCustom("varchar(max)").Nullable()
                  .WithColumn("SAPNotificationNumber").AsInt64().Nullable()
                  .WithColumn("WBSElement").AsAnsiString(30).Nullable()
                  .WithColumn("DateCompleted").AsDateTime().Nullable()
                  .WithForeignKeyColumn("CompletedById", "tblPermissions", "RecID")
                  .WithColumn("ApprovedOn").AsDateTime().Nullable()
                  .WithForeignKeyColumn("ApprovedById", "tblPermissions", "RecID")
                  .WithColumn("MaterialsApprovedOn").AsDateTime().Nullable()
                  .WithForeignKeyColumn("MaterialsApprovedById", "tblPermissions", "RecID")
                  .WithColumn("MaterialsPlannedOn").AsDateTime().Nullable()
                  .WithColumn("DateCancelled").AsDateTime().Nullable()
                  .WithForeignKeyColumn("CancellationReasonId", "ProductionWorkOrderCancellationReasons")
                  .WithForeignKeyColumn("PlantMaintenanceActivityTypeOverrideId", "PlantMaintenanceActivityTypes")
                  .WithColumn("CapitalizationReason").AsCustom("text").Nullable()
                  .WithForeignKeyColumn("CapitalizedFromId", "ProductionWorkOrders")
                  .WithColumn("BasicStart").AsDateTime().Nullable()
                  .WithColumn("BasicFinish").AsDateTime().Nullable()
                //.WithColumn("Approved")
                ;
            Alter.Table("WorkOrderPurposes").AddColumn("IsProduction").AsBoolean().NotNullable()
                 .WithDefaultValue(false);
            Execute.Sql(
                "UPDATE WorkOrderPurposes SET IsProduction = 1 WHERE WorkOrderPurposeId IN (3, 4, 5, 9, 11, 13, 14, 18, 19, 20, 21)");

            Create.Table("ProductionWorkOrdersProductionPrerequisites")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ProductionWorkOrderId", "ProductionWorkOrders")
                  .WithForeignKeyColumn("ProductionPrerequisiteId", "ProductionPrerequisites")
                  .WithColumn("SatisfiedOn").AsDateTime().Nullable()
                  .WithForeignKeyColumn("LinkedDocumentId", "Document", "DocumentID");

            //datatype
            Execute.Sql(
                "if not exists (select 1 from DataType where Table_Name = 'ProductionWorkOrders') insert into DataType Values('Production Work Order', 'ProductionWorkOrders', null)" +
                "if not exists (select 1 from DocumentType where Document_Type = 'Picture' and DataTypeID = (SELECT DataTypeID from DataType where Data_Type = 'Production Work Order')) insert into documentType SELECT 'Picture', DataTypeID from DataType where Data_Type = 'Production Work Order'");

            Create.Table("EmployeeAssignments")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ProductionWorkOrderId", "ProductionWorkOrders")
                  .WithColumn("AssignedOn").AsDateTime().NotNullable()
                  .WithColumn("AssignedFor").AsDateTime().NotNullable()
                  .WithForeignKeyColumn("AssignedToId", "tblEmployee", "tblEmployeeId", false)
                  .WithColumn("DateStarted").AsDateTime().Nullable()
                  .WithColumn("DateEnded").AsDateTime().Nullable();

            Create.Table("EmployeeAssignmentsEmployees")
                  .WithForeignKeyColumn("EmployeeAssignmentId", "EmployeeAssignments", "Id", false)
                  .WithForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeId", false);

            Create.Table("ProductionWorkOrderMaterialUsed")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ProductionWorkOrderId", "ProductionWorkOrders", nullable: false)
                  .WithForeignKeyColumn("MaterialId", "Materials", "MaterialID", true)
                  .WithColumn("Quantity").AsInt32().NotNullable()
                  .WithColumn("NonStockDescription").AsAnsiString(50).Nullable()
                  .WithForeignKeyColumn("StockLocationId", "StockLocations", "StockLocationID");
            Alter.Table("tblJobObservations")
                 .AddForeignKeyColumn("ProductionWorkOrderId", "ProductionWorkOrders");

            Execute.Sql(
                "INSERT INTO Roles (ApplicationID, ModuleID, ActionID, UserId) SELECT 2, ModuleId, 1, 2792 from Modules where Name = 'Production Work Management';" +
                "INSERT INTO Roles(ApplicationID, ModuleID, ActionID, UserId) SELECT 2, ModuleId, 1, 2737 from Modules where Name = 'Production Work Management';");

            Execute.Sql(
                "insert into [ProductionWorkDescriptions] ([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator])  select 'REPAIR', 121, Id, 2, 0 from PlantMaintenanceActivityTypes where Description = 'General Operations';" +
                "insert into[ProductionWorkDescriptions] ([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator])  select 'REPLACE', 134, Id, 1, 0 from PlantMaintenanceActivityTypes where Description = 'Predictive Inspection';" +
                "insert into[ProductionWorkDescriptions] ([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator])  select 'GENERAL REPAIR', 141, Id, 1, 0 from PlantMaintenanceActivityTypes where Description = 'Reactive Corrective';" +
                "insert into[ProductionWorkDescriptions] ([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) select 'LEAK REPAIR', 209, Id, 3, 1 from PlantMaintenanceActivityTypes where Description = 'Emergency Corrective'");
            //Execute.Sql(
            //    "INSERT INTO [dbo].[ProductionWorkDescriptions] ([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) " +
            //    "SELECT 'INSPECTION', 154, Id, 3, 1 FROM PlantMaintenanceActivityTypes where Description = 'Reactive Corrective' UNION ALL " +
            //    "SELECT 'BELT REPLACEMENT', 154, Id, 3, 1 FROM PlantMaintenanceActivityTypes where Description = 'Emergency Corrective' UNION ALL " +
            //    "SELECT 'REPLACE COOLANT LINE', 154, Id, 3, 1 FROM PlantMaintenanceActivityTypes where Description = 'Result of Inspection' UNION ALL " +
            //    "SELECT 'ANGLE GEAR REPAIR', 154, Id, 3, 1 FROM PlantMaintenanceActivityTypes where Description = 'Corrective Training' UNION ALL " +
            //    "SELECT 'PLATE HEAT EXCHANGER', 154, Id, 3, 1 FROM PlantMaintenanceActivityTypes where Description = 'Result of Inspection' UNION ALL " +
            //    "SELECT 'REPLACE ALTERNATOR', 154, Id, 3, 1 FROM PlantMaintenanceActivityTypes where Description = 'Emergency Corrective' UNION ALL " +
            //    "SELECT 'REPLACE TEMP PROBE', 154, Id, 3, 1 FROM PlantMaintenanceActivityTypes where Description = 'Result of Inspection' UNION ALL " +
            //    "SELECT 'LEAKING GASKETS', 154, Id, 3, 1 FROM PlantMaintenanceActivityTypes where Description = 'Corrective Training' UNION ALL " +
            //    "SELECT 'TROUBLESHOOT', 154, Id, 3, 1 FROM PlantMaintenanceActivityTypes where Description = 'Emergency Corrective' UNION ALL " +
            //    "SELECT 'OTHER', 154, Id, 3, 1 FROM PlantMaintenanceActivityTypes where Description = 'Corrective Training'");

            Execute.Sql(
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'PRIMED', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP CENTRIFUGAL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LEAK REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP CENTRIFUGAL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP CENTRIFUGAL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LINE/HOSE/TUBE REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP CENTRIFUGAL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'PUMP FAILURE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP CENTRIFUGAL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'PULLED PUMP', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP CENTRIFUGAL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'BLEED PUMP', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP CENTRIFUGAL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'SUCTION REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP CENTRIFUGAL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE PACKING', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP CENTRIFUGAL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LUBRICATION', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP CENTRIFUGAL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'VALVE REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP CENTRIFUGAL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE VALVE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP CENTRIFUGAL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'PRIMED', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP POSITIVE DISPLACEMENT'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LEAK REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP POSITIVE DISPLACEMENT'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP POSITIVE DISPLACEMENT'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LINE/HOSE/TUBE REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP POSITIVE DISPLACEMENT'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'PUMP FAILURE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP POSITIVE DISPLACEMENT'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'PULLED PUMP', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP POSITIVE DISPLACEMENT'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'BLEED PUMP', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP POSITIVE DISPLACEMENT'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'SUCTION REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP POSITIVE DISPLACEMENT'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE PACKING', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP POSITIVE DISPLACEMENT'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LUBRICATION', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP POSITIVE DISPLACEMENT'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'VALVE REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP POSITIVE DISPLACEMENT'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE VALVE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP POSITIVE DISPLACEMENT'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'PRIMED', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP GRINDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LEAK REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP GRINDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP GRINDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LINE/HOSE/TUBE REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP GRINDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'PUMP FAILURE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP GRINDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'PULLED PUMP', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP GRINDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'BLEED PUMP', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP GRINDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'SUCTION REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP GRINDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE PACKING', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP GRINDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LUBRICATION', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP GRINDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'VALVE REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP GRINDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE VALVE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PUMP GRINDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'EMERGENCY GENERATOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'CLEANING', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'EMERGENCY GENERATOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'RUN', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'EMERGENCY GENERATOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LEAK REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'EMERGENCY GENERATOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE BATTERY', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'EMERGENCY GENERATOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'OIL CHANGE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'EMERGENCY GENERATOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'TEST', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'EMERGENCY GENERATOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'TROUBLESHOOT', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'EMERGENCY GENERATOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GENERATORS'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'CLEANING', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GENERATORS'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'RUN', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GENERATORS'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LEAK REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GENERATORS'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE BATTERY', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GENERATORS'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'OIL CHANGE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GENERATORS'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'TEST', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GENERATORS'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'TROUBLESHOOT', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GENERATORS'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'CLEAN', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL DRY FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LEAK REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL DRY FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE HOSE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL DRY FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL DRY FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INVESTIGATE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL DRY FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE TIMER', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL DRY FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE HELICOIL', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL DRY FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE VALVE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL DRY FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'CLEAN', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GAS FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LEAK REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GAS FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE HOSE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GAS FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GAS FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INVESTIGATE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GAS FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE TIMER', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GAS FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE HELICOIL', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GAS FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE VALVE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL GAS FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'CLEAN', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL LIQUID FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LEAK REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL LIQUID FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE HOSE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL LIQUID FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL LIQUID FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INVESTIGATE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL LIQUID FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE TIMER', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL LIQUID FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE HELICOIL', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL LIQUID FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE VALVE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL LIQUID FEEDER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PLANT VALVE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PLANT VALVE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LEAK REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PLANT VALVE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'ADJUST', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PLANT VALVE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INVESTIGATE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PLANT VALVE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'TROUBLESHOOT', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'PLANT VALVE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ENGINE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'BELT REPLACEMENT', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ENGINE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE COOLANT LINE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ENGINE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'ANGLE GEAR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ENGINE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'PLATE HEAT EXCHANGER', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ENGINE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE ALTERNATOR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ENGINE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE TEMP PROBE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ENGINE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LEAKING GASKETS', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ENGINE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'TROUBLESHOOT', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ENGINE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'MOTOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INVESTIGATE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'MOTOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'SHUT DOWN', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'MOTOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'MOTOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GROUND MOTOR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'MOTOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'TROUBLESHOOT', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'MOTOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'TESTING', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'MOTOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'FILTER CHANGE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FILTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'BACKWASH', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FILTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE SOCK', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FILTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'VALVE REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FILTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FILTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INVESTIGATE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FILTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'SCADA ISSUE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FILTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'METER', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FILTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'ALARM', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FILTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'CALIBRATION', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FILTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE LIGHTING', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FILTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'PARTICLE COUNTER', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FILTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE VALVE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BASIN'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'CLEAN', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BASIN'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BASIN'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE LIFE RING', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BASIN'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INSTALL CAUTION SIGNS', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BASIN'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'PUMP REPAIR/REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BASIN'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE INFLUENT', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BASIN'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'PARTICLE EFFLUENT', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BASIN'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INSTALL', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CONTROL PANEL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'UNLOAD', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CONTROL PANEL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE LIGHT', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CONTROL PANEL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CONTROL PANEL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CONTROL PANEL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INSTALL', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'POWER PANEL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'UNLOAD', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'POWER PANEL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE LIGHT', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'POWER PANEL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'POWER PANEL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'POWER PANEL'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'OZONE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPAIR/REPLACE GENERATOR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'OZONE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INVESTIGATE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'OZONE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'MONITOR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'OZONE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'ALARM', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'OZONE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE LIGHTS', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'OZONE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPAIR/REPLACE DIFFUSER', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'OZONE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'DRAIN LINES', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'OZONE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE UV ANALYZER', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'OZONE'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'LEAK REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL TANK'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INSTRUMENT ISSUE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL TANK'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'DRAIN', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL TANK'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'CLEAN', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL TANK'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'VALVE REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL TANK'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL TANK'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'MIXER', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL TANK'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'CHEMICAL TANK'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ALARM'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ALARM'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ALARM'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INVESTIGATE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ALARM'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BLOWER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BLOWER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'TROUBLESHOOT', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BLOWER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'AIR COMPRESSOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'AIR COMPRESSOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'TROUBLESHOOT', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'AIR COMPRESSOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'VALVE REPAIR/REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'AIR COMPRESSOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'BELT REPAIR/REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'AIR COMPRESSOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INSTALL', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'AIR COMPRESSOR'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'ALARM', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'SCADA'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INSTRUMENT ISSUE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'SCADA'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'COMMUNICATIONS ISSUE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'SCADA'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPORTING  ISSUE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'SCADA'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'SCADA'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FLOW METER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FLOW METER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'TROUBLESHOOT', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FLOW METER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INSTALL', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FLOW METER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'CLEAN', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FLOW METER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'CALIBRATE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FLOW METER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ANALYZER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INSTALL', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ANALYZER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'CALIBRATE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ANALYZER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'ANALYZER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'FACILITY REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FACILITY AND GROUNDS'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'FACILITY AND GROUNDS'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'TRANSMITTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'TRANSMITTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INSTALL', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'TRANSMITTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'CALIBRATE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'TRANSMITTER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'SCADA ISSUE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'STATION'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'STATION'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE LIGHT', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'STATION'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'CLEAN', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'STATION'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'FILL PANEL INSTALL', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'STATION'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'ELECTRICAL MODIFICATION', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'STATION'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INSTALL', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'SWITCH'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'SWITCH'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'SWITCH'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE COVER', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'SWITCH'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'TROUBLESHOOT', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'SWITCH'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BATTERY'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INSTALL', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BATTERY'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'CHARGE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BATTERY'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'TEST', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BATTERY'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'BATTERY'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INSTALL', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'LIGHTING'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'LIGHTING'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'TEST', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'LIGHTING'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'LIGHTING'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'GENERAL REPAIR', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'MIXER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INSTALL', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'MIXER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'MIXER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'INVESTIGATE', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'MIXER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'" +
                "INSERT INTO [ProductionWorkDescriptions]([Description],[EquipmentClassId],[PlantMaintenanceActivityTypeId],[OrderTypeId],[BreakdownIndicator]) SELECT 'REPLACE GEAR BOX', (SELECT ID FROM SAPEquipmentTypes WHERE Description = 'MIXER'), Id, 3 ,0 FROM PlantMaintenanceActivityTypes WHERE Description = 'Reactive Corrective'");

            Create.Table("EmployeeProductionSkillSet")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeId")
                  .WithForeignKeyColumn("ProductionSkillSetId", "ProductionSkillSets");

            Create.Table("MeasurementPointsSAPEquipmentTypes")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("SAPEquipmentTypeId", "SAPEquipmentTypes")
                  .WithForeignKeyColumn("UnitOfMeasureId", "UnitsOfMeasure", "UnitOfMeasureId")
                  .WithColumn("Description").AsAnsiString(50).NotNullable()
                  .WithColumn("Min").AsDecimal().NotNullable()
                  .WithColumn("Max").AsDecimal().NotNullable()
                  .WithColumn("Category").AsAnsiString(1).NotNullable()
                  .WithColumn("Position").AsInt32().NotNullable()
                  .WithColumn("IsActive").AsBoolean().WithDefaultValue(true);

            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 from UnitsOfMeasure where Description = 'Minutes') INSERT INTO UnitsOfMeasure VALUES('Minutes')");
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 from UnitsOfMeasure where Description = 'Hours') INSERT INTO UnitsOfMeasure VALUES('Hours')");
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 from UnitsOfMeasure where Description = 'Percent') INSERT INTO UnitsOfMeasure VALUES('Percent')");
            Execute.Sql(
                "INSERT INTO MeasurementPointsSAPEquipmentTypes SELECT (SELECT Id from SAPEquipmentTypes where Abbreviation = 'GEN'),       UnitOfMeasureId, 'Minutes Ran',                 0, 10000,   'G', 100, 1 from UnitsOfMeasure where Description = 'Minutes';");
            Execute.Sql(
                "INSERT INTO MeasurementPointsSAPEquipmentTypes SELECT (SELECT Id from SAPEquipmentTypes where Abbreviation = 'GEN'),       UnitOfMeasureId, 'Lifetime Run Hours',          0, 100000,  'G', 110, 1 from UnitsOfMeasure where Description = 'Hours';");
            Execute.Sql(
                "INSERT INTO MeasurementPointsSAPEquipmentTypes SELECT (SELECT Id from SAPEquipmentTypes where Abbreviation = 'GEN'),       UnitOfMeasureId, 'Gallons Fuel Left',           0, 100000,  'G', 120, 1 from UnitsOfMeasure where Description = 'Gallons';");
            Execute.Sql(
                "INSERT INTO MeasurementPointsSAPEquipmentTypes SELECT (SELECT Id from SAPEquipmentTypes where Abbreviation = 'PMP-CENT'),  UnitOfMeasureId, 'Lifetime Run Hours',          0, 100000,  'P', 100, 1 from UnitsOfMeasure where Description = 'Hours';");
            Execute.Sql(
                "INSERT INTO MeasurementPointsSAPEquipmentTypes SELECT (SELECT Id from SAPEquipmentTypes where Abbreviation = 'PMP-CENT'),  UnitOfMeasureId, 'Measured Pump Efficiency',    0, 100,     'P', 110, 1 from UnitsOfMeasure where Description = 'Percent';");

            Create.Table("ProductionWorkOrdersEquipment")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ProductionWorkOrderId", "ProductionWorkOrders")
                  .WithForeignKeyColumn("EquipmentId", "Equipment", "EquipmentId")
                  .WithColumn("SAPNotificationNumber").AsInt64().Nullable()
                  .WithColumn("CompletedOn").AsDateTime().Nullable();

            Create.Table("ProductionWorkOrderMeasurementPointValues")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ProductionWorkOrderId", "ProductionWorkOrders")
                  .WithForeignKeyColumn("MeasurementPointSAPEquipmentTypeId", "MeasurementPointsSAPEquipmentTypes")
                  .WithForeignKeyColumn("EquipmentId", "Equipment", "EquipmentId")
                  .WithColumn("Value").AsAnsiString(100).NotNullable()
                  .WithColumn("MeasurementDocId").AsInt32().Nullable();
            Alter.Table("UnitsOfMeasure")
                 .AddColumn("SAPCode").AsAnsiString(4).Nullable();
            Execute.Sql("Update UnitsOfMeasure set SAPCode = 'H' where Description = 'Hours'");
            Execute.Sql("Update UnitsOfMeasure set SAPCode = 'Min' where Description = 'Minutes'");
            Execute.Sql("Update UnitsOfMeasure set SAPCode = '%' where Description = 'Percent'");
        }

        public override void Down()
        {
            Delete.Column("SAPCode").FromTable("UnitsOfMeasure");

            Delete.Table("ProductionWorkOrderMeasurementPointValues");
            Delete.Table("ProductionWorkOrdersEquipment");
            Delete.Table("MeasurementPointsSAPEquipmentTypes");
            Execute.Sql("DELETE FROM ROles where ModuleId = 78 and UserId in (2792, 2737)");
            Delete.ForeignKeyColumn("tblJobObservations", "ProductionWorkOrderId", "ProductionWorkOrders");
            Delete.Table("ProductionWorkOrderMaterialUsed");
            Delete.Table("EmployeeAssignmentsEmployees");
            Delete.Table("EmployeeAssignments");
            //Execute.Sql(
            //    "delete from DocumentType where DataTypeID =  (SELECT DataTypeID from DataType where Data_Type = 'Production Work Order')" +
            //    "delete from DataType where DataTypeID = (SELECT DataTypeID from DataType where Data_Type = 'Production Work Order')");
            Delete.Table("ProductionWorkOrdersProductionPrerequisites");
            Delete.ForeignKeyColumn("PlantMaintenanceActivityTypes", "OrderTypeId", "OrderTypes");
            Delete.Table("ProductionWorkOrders");
            Delete.Table("ProductionWorkDescriptions");
            Delete.Table("EmployeeProductionSkillSet");
            Delete.Table("ProductionSkillSets");
            Delete.Table("ProductionWorkOrderCancellationReasons");
            Execute.Sql("DELETE FROM PlantMaintenanceActivityTypes WHERE Code in ('F01', 'F02', 'F03', 'F04');");
            Execute.Sql("DELETE FROM PlantMaintenanceActivityTypes WHERE Code in ('P01', 'P02', 'P03', 'P04', 'P07');");
            Execute.Sql(
                "DELETE FROM PlantMaintenanceActivityTypes WHERE Code in ('OA1', 'OA2', 'OA3', 'OA4', 'OA5', 'OA6', 'OA7');");
            Delete.Table("OrderTypes");
            Delete.Column("IsProduction").FromTable("WorkOrderPurposes");
        }
    }
}
