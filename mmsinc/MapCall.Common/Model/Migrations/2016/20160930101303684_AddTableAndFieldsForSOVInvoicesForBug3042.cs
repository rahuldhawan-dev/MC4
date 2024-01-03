using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160930101303684), Tags("Production")]
    public class AddTableAndFieldsForSOVInvoicesForBug3042 : Migration
    {
        #region Constants

        public const int WORK_ORDER_INVOICE = 75;
        public const string MODULE_NAME = "Work Order Invoice";

        #endregion

        public override void Up()
        {
            #region Operating Centers

            Alter.Table("OperatingCenters")
                 .AddColumn("HasWorkOrderInvoicing")
                 .AsBoolean()
                 .NotNullable()
                 .WithDefaultValue(0);
            Execute.Sql(
                "UPDATE OperatingCenters SET HasWorkOrderInvoicing = 1, PermitsOMUserName ='permits-nj6-om@mapcall.com', PermitsCapitalUserName = 'permits-nj6-cap@mapcall.com' WHERE OperatingCenterCode = 'SOV';");

            Alter.Column("Description").OnTable("AssetTypes").AsAnsiString(50).NotNullable();

            Execute.Sql(
                "INSERT INTO [OperatingCenterAssetTypes] SELECT (select operatingCenterId from OperatingCenters where OperatingCenterCode ='SOV'), (select AssetTypeID from AssetTypes where Description = 'Equipment')");

            // Operating Center Stocked Materials
            Execute.Sql(
                "insert into OperatingCenterStockedMaterials select (select operatingCenterID from OperatingCenters where OperatingCenterCode = 'SOV'), MaterialId from OperatingCenterStockedMaterials where OperatingCenterID = 12");
            // Operating Center Stock Locations
            Execute.Sql(
                "insert into StockLocations select Description, (select operatingCenterID from OperatingCenters where OperatingCenterCode = 'SOV'), SAPStockLocation from StockLocations where OperatingCenterId = 12");

            #endregion

            #region WorkOrderInvoices

            this.CreateLookupTableWithValues("ScheduleOfValueTypes", "Unit Cost", "Time & Material");

            Create.Table("WorkOrderInvoices")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("WorkOrderId", "WorkOrders", "WorkOrderID")
                  .WithForeignKeyColumn("ScheduleOfValueTypeId", "ScheduleOfValueTypes").NotNullable()
                  .WithColumn("InvoiceDate").AsDateTime().Nullable()
                  .WithColumn("IncludeMaterials").AsBoolean().NotNullable().WithDefaultValue(0)
                  .WithColumn("SubmittedDate").AsDateTime().Nullable()
                  .WithColumn("CanceledDate").AsDateTime().Nullable();

            Execute.Sql("SET IDENTITY_INSERT Modules ON;" +
                        "INSERT INTO Modules(ModuleID, ApplicationID, Name) " +
                        "SELECT " + WORK_ORDER_INVOICE + ", ApplicationID, '" + MODULE_NAME + "' " +
                        "FROM Applications WHERE Name = 'Field Services';" +
                        "SET IDENTITY_INSERT Modules OFF;");

            this.CreateLookupTableWithValues("WorkOrderInvoiceStatuses", "Pending", "Submitted", "Canceled");
            this.CreateDocumentType("WorkOrderInvoices", MODULE_NAME, "Work Order Invoice Document");

            this.CreateNotificationPurpose("Field Services", "Work Order Invoice", "Work Order Invoice Submitted");
            this.CreateNotificationPurpose("Field Services", "Work Order Invoice", "Work Order Invoice Canceled");

            // Add existing SOV users into new role
            Execute.Sql("INSERT INTO ROLES " +
                        "SELECT OperatingCenterID, ApplicationID, " + WORK_ORDER_INVOICE + ", ActionId, UserId " +
                        "FROM Roles " +
                        "WHERE OperatingCenterID IN (select OperatingCenterID from OperatingCenters where OperatingCenterCode = 'SOV')");

            #endregion

            #region Units of Measure

            Execute.Sql("INSERT INTO UnitsOfMeasure Values('HR')");
            Execute.Sql("INSERT INTO UnitsOfMeasure Values('Tons')");
            Execute.Sql("INSERT INTO UnitsOfMeasure Values('Load')");
            Execute.Sql("INSERT INTO UnitsOfMeasure Values('SF')");
            Execute.Sql("INSERT INTO UnitsOfMeasure Values('LF')");
            Execute.Sql("INSERT INTO UnitsOfMeasure Values('EA')");
            Execute.Sql("INSERT INTO UnitsOfMeasure Values('Yard')");

            #endregion

            #region ScheduleOfValuesCategories

            Create.Table("ScheduleOfValueCategories")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsAnsiString(255).NotNullable()
                  .WithForeignKeyColumn("ScheduleOfValueTypeId", "ScheduleOfValueTypes").NotNullable();

            // Bool column for labor or normal?
            // normal
            Execute.Sql(
                "INSERT INTO [ScheduleOfValueCategories] Values('New Small Diameter Services (Includes New Tap on Main)', 1)");
            Execute.Sql(
                "INSERT INTO [ScheduleOfValueCategories] Values('Replace (Renewal) Small Diameter Services  (Includes New Tap on Main)', 1)");
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('New Large Diameter Services', 1)");
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Abandon / Retire Services', 1)");
            Execute.Sql(
                "INSERT INTO [ScheduleOfValueCategories] Values('Transfer Existing Small Diameter Services (2\" and less in dia.)', 1)");

            Execute.Sql(
                "INSERT INTO [ScheduleOfValueCategories] Values('Transfer Existing Large Diameter Services', 1)");
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Valve Replacement on Existing Mains', 1)");
            Execute.Sql(
                "INSERT INTO [ScheduleOfValueCategories] Values('Test Hole Excavation (OD Reads or Depth of Services or Mains)', 1)");
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Fire Hydrant Installation - New', 1)");
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Fire Hydrant Installation - Replacement', 1)");

            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Shut Down, Cut and Cap  (up to 12\")', 1)");
            Execute.Sql(
                "INSERT INTO [ScheduleOfValueCategories] Values('Temporary Asphalt Paving (depth as required by State or Municipality)', 2)");
            Execute.Sql(
                "INSERT INTO [ScheduleOfValueCategories] Values('Temporary Sidewalk, Curb  & Driveway Replacement', 2)");
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Saw Cut', 2)");
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Curb Box', 1)");

            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Meter Chamber (pit)', 1)");
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Valve Box', 1)");
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Downtime', 2)");
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Meter Replacement', 1)");
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('New Meter', 1)");

            // eq/labor
            //Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Emergency Repair Labor, Equipment, Restoration Rates (Materials, Traffic Control, Permit)');");
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Labor (All wages are $/hour)', 2);");
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Equipment', 2);");
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Restoration materials', 2);");
            Execute.Sql(
                "INSERT INTO [ScheduleOfValueCategories] Values('Temporary Asphalt Paving (depth as required by State or Municipality)', 2);");
            Execute.Sql(
                "INSERT INTO [ScheduleOfValueCategories] Values('Temporary Sidewalk, Curb  & Driveway Replacement', 2);");

            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Permanent Restoration', 2);");
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Other (Unit Cost)', 1);"); //27
            Execute.Sql("INSERT INTO [ScheduleOfValueCategories] Values('Other (Time & Material)', 2);"); //28

            #endregion

            #region ScheduleOfValues

            Create.Table("ScheduleOfValues")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ScheduleOfValueCategoryId", "ScheduleOfValueCategories")
                  .WithForeignKeyColumn("UnitOfMeasureId", "UnitsOfMeasure", "UnitOfMeasureID")
                  .WithColumn("Description").AsAnsiString(255).NotNullable()
                  .WithColumn("LaborUnitCost").AsDecimal(10, 2).Nullable()
                  .WithColumn("LaborUnitOvertimeCost").AsDecimal(10, 2).Nullable()
                  .WithColumn("MaterialCost").AsDecimal(10, 2).Nullable()
                  .WithColumn("MiscCost").AsDecimal(10, 2).Nullable();

            //This query still generates some bad units of measure ton vs tons, /LF, and some others, check the table for nulls after the import to fix.
            //SELECT '" UNION ALL SELECT <CATEGORYID>, ''' + replace(replace(F2, '''', ''''''), '"', '\"') + ''', (select UnitOfMeasureID from unitsofMeasure where Description = ''' + replace(F3, '\','') + '''),' + isNull(cast(F4 as varchar),'NULL') + ',' + isnull(cast(F5 as varchar),'NULL') + ',' + isNull(cast(F9 as varchar), 'NULL') + '" + ' from _sovs
            Execute.Sql(
                "INSERT INTO ScheduleOfValues(ScheduleOfValueCategoryId,Description,UnitOfMeasureId,LaborUnitCost,MaterialCost,MiscCost)" +
                // New Small Diameter Services (8)
                " SELECT 1, '1\" Short Service (<=16ft)  Main to curb stop', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),1631.00,175.00,100.00" +
                " UNION ALL SELECT 1, '1\" Long Service (16'' - 40ft)  Main to curb stop', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),1970.00,260.00,100.00" +
                " UNION ALL SELECT 1, '1\"  Long Service (40'' - 60ft) Main to curb stop', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),2070.00,360.00,100.00" +
                " UNION ALL SELECT 1, '1\" Extra Long Service (>60ft) price per foot after 60'' + item c', (select UnitOfMeasureID from unitsofMeasure where Description = 'LF'),30.35,4.00,2.00" +
                " UNION ALL SELECT 1, '2\" Short Service (<=16ft) Main to curb stop', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),1631.00,600.00,100.00" +
                " UNION ALL SELECT 1, '2\" Long Service (16'' - 40ft) Main to curb stop', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),1970.00,840.00,100.00" +
                " UNION ALL SELECT 1, '2\"  Long Service (40'' - 60ft) Main to curb stop', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),2070.00,1040.00,100.00" +
                " UNION ALL SELECT 1, '2\" Extra Long Service (>60ft)  price per foot after 60'' + item g', (select UnitOfMeasureID from unitsofMeasure where Description = 'LF'),30.35,10.00,2.00" +

                // Replace(Renewal) Small Diameter Services.... (8)
                " UNION ALL SELECT 2, '1\" Short Service (<= 16ft)  Main to curb stop', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),1631.00,175.00,100.00" +
                " UNION ALL SELECT 2, '1\" Long Service (16'' - 40ft)  Main to curb stop', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),1970.00,260.00,100.00" +
                " UNION ALL SELECT 2, '1\"  Long Service (40'' - 60ft) Main to curb stop', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),2070.00,360.00,100.00" +
                " UNION ALL SELECT 2, '1\" Extra Long Service (> 60ft) price per foot after 60'' + item c', (select UnitOfMeasureID from unitsofMeasure where Description = 'LF'),30.35,4.00,2.00" +
                " UNION ALL SELECT 2, '2\" Short Service (<= 16ft) Main to curb stop', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),1631.00,600.00,100.00" +
                " UNION ALL SELECT 2, '2\" Long Service (16'' - 40ft) Main to curb stop', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),1970.00,840.00,100.00" +
                " UNION ALL SELECT 2, '2\"  Long Service (40'' - 60ft) Main to curb stop', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),2070.00,1040.00,100.00" +
                " UNION ALL SELECT 2, '2\" Extra Long Service (> 60ft)  price per foot after 60'' + item g', (select UnitOfMeasureID from unitsofMeasure where Description = 'LF'),30.35,10.00,2.00" +

                // New Large Diameter Services (6)
                " UNION ALL SELECT 3, '4\" to 8\" diameter Service Line price per foot (<= 16ft)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),2350.00,1688.00,100.00" +
                " UNION ALL SELECT 3, '4\" to 8\" diameter Service Line price per foot (<= 40ft)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),7100.00,2000.00,100.00" +
                " UNION ALL SELECT 3, '4\" to 8\" diameter Service Line price per foot (> 40ft) + item 1b', (select UnitOfMeasureID from unitsofMeasure where Description = 'LF'),146.68,13.00,2.00" +
                " UNION ALL SELECT 3, '10\" diameter Service Line price per foot (<= 16ft)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),11790.00,2800.00,100.00" +
                " UNION ALL SELECT 3, '10\" diameter Service Line price per foot (<= 40ft)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),15360.00,3300.00,100.00" +
                " UNION ALL SELECT 3, '10\" diameter Service Line price per foot (> 40ft) + item 2b', (select UnitOfMeasureID from unitsofMeasure where Description = 'LF'),148.71,18.00,2.00" +

                // Abandon / Retire Services (3)
                " UNION ALL SELECT 4, '<= 2\"  Services', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),1213.92,100.00,100.00" +
                " UNION ALL SELECT 4, 'Unit price for abandoning services 3\" to 12\" in dia., including removal of tee and valve, and installation of pipe & sleeves', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),2488.54,500.00,100.00" +
                " UNION ALL SELECT 4, 'Unit price for abandoning services 3\" to 12\" in dia., close valve and cut and cap service line', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),2023.20,100.00,100.00" +

                // Transfer Existing Small Diameter Services (1)
                " UNION ALL SELECT 5, 'Transfer Existing Small Diameter Services (2\" and less in dia.)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),1340.37,200.00,100.00" +

                // Transfer Existing Large Diameter Services (2)
                " UNION ALL SELECT 6, '4\" to 8\" Service Line', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),3570.95,1650.00,100.00" +
                " UNION ALL SELECT 6, '10\"  Service Line ', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),11764.91,2900.00,100.00" +

                // Valve Replacement on Existing Mains (6)
                " UNION ALL SELECT 7, 'Unit price for complete replacement of one  valve from 2” to 8”  ', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),1628.68,1012.00,100.00" +
                " UNION ALL SELECT 7, 'Unit price for complete replacement of one  valve from 10” to 12”  ', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),2346.91,1380.00,100.00" +
                " UNION ALL SELECT 7, 'Unit price for the complete replacement of each additional 2\" - 8\" valve in the same excavation.', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),263.02,1012.00,100.00" +
                " UNION ALL SELECT 7, 'Unit price for the complete replacement of each additional 10”-12” valve in the same  excavation', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),531.09,1380.00,100.00" +
                " UNION ALL SELECT 7, 'Unit price for each 4” to 8” line stop ', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),7587.00,1200.00,100.00" +
                " UNION ALL SELECT 7, 'Unit price for each 10”  line stop', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),10116.00,2200.00,100.00" +

                // Test Hole Excavation (OD Reads or Depth of Services or Mains) (3)
                " UNION ALL SELECT 8, 'Standard Opening 8 x 8 x 5 foot deep', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),1517.40,NULL,100.00" +
                " UNION ALL SELECT 8, 'Standard Opening 8 x 8 x greater than 5'' but less than or = to 10'' foot deep ', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),2529.00,NULL,100.00" +
                " UNION ALL SELECT 8, 'Price per additional foot of depth for holes >10'' deep. + item b.', (select UnitOfMeasureID from unitsofMeasure where Description = 'LF'),252.90,NULL,2.00" +

                // Fire Hydrant Installation - New  (2)
                " UNION ALL SELECT 9, 'Short Installation With Restraint  (<= 20'' ft)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),3743.00,3150.00,100.00" +
                " UNION ALL SELECT 9, 'Long Installation With Restraint  (> 20'' ft)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),4046.40,3550.00,100.00" +

                // Fire Hydrant Installation - Replacement (6)
                " UNION ALL SELECT 10, 'Replace Hydrant with Restraint *', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),1718.00,1000.00,100.00" +
                " UNION ALL SELECT 10, 'Replace Hydrant and Lateral With Restraint (Short <= 20 ft)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),2174.94,1500.00,100.00" +
                " UNION ALL SELECT 10, 'Replace Hydrant and Lateral With Restraint (Long > 20 ft)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),4754.52,1800.00,100.00" +
                " UNION ALL SELECT 10, 'Retirement of Hydrant, Cut, Cap, and remove Gate Box - cut out existing Tee and replace with straight pipe', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),1719.72,500.00,100.00" +
                " UNION ALL SELECT 10, 'Replace Hydrant and lateral with restraint (Short <= 20ft) Including shut down and valve replacement', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),2529.00,1800.00,100.00" +
                " UNION ALL SELECT 10, 'Replace hydrant and lateral with restraint (Long > 20ft)  Including shut down and valve replacement', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),2832.48,2000.00,100.00" +

                // Shut Down, Cut and Cap  (up to 12") (1)
                " UNION ALL SELECT 11, 'Shut Down, Cut and Cap  (up to 12\")', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),2351.97,400.00,100.00" +

                // Temporary Asphalt Paving(depth as required by State or Municipality) (5)
                " UNION ALL SELECT 12, 'Cold Patch', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),1.69,NULL,2.00" +
                " UNION ALL SELECT 12, 'Hot Patch  (up to 4\") ', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),2.02,NULL,2.00" +
                " UNION ALL SELECT 12, 'Hot Patch  7\" ', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),3.54,NULL,2.00" +
                " UNION ALL SELECT 12, '4\" Base to Grade Course', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),2.02,NULL,2.00" +
                " UNION ALL SELECT 12, '6\" Base to Grade Course', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),3.03,NULL,2.00" +

                // Temporary Sidewalk, Curb  & Driveway Replacement (5)
                " UNION ALL SELECT 13, 'Cold Patch', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),10.12,NULL,2.00" +
                " UNION ALL SELECT 13, 'Hot Patch  (up to 4\") ', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),13.15,NULL,2.00" +
                " UNION ALL SELECT 13, 'Hot Patch  7\" ', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),20.23,NULL,2.00" +
                " UNION ALL SELECT 13, '4\" Base to Grade Course', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),10.12,NULL,2.00" +
                " UNION ALL SELECT 13, '6\" Base to Grade Course', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),5.82,NULL,2.00" +

                // Saw Cut (1)
                " UNION ALL SELECT 14, 'Saw Cut  ', (select UnitOfMeasureID from unitsofMeasure where Description = 'LF'),5.06,NULL,2.00" +

                // Curb Box (2)
                " UNION ALL SELECT 15, 'Partial Excavation ', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),199.25,30.00,NULL" +
                " UNION ALL SELECT 15, 'Full Excavation', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),436.40,30.00,NULL" +

                // Meter Chamber(pit) (3)
                " UNION ALL SELECT 16, 'Partial Excavation of meter chamber (1\" service)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),318.65,270.00,100.00" +
                " UNION ALL SELECT 16, 'Full Excavation of meter Chamber (1\" service)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),637.31,270.00,100.00" +
                " UNION ALL SELECT 16, 'Replace/Install meter chamber & meter set work (1\" service)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),637.31,600.00,100.00" +

                // Valve Box (2)
                " UNION ALL SELECT 17, 'Partial Excavation ', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),169.25,60.00,NULL" +
                " UNION ALL SELECT 17, 'Full Excavaton', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),316.40,150.00,NULL" +

                // DownTime (1)
                " UNION ALL SELECT 18, 'Downtime', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),796.65,NULL,100.00" +

                // Meter Replacement(2)
                " UNION ALL SELECT 19, '5/8\" through 1\" (does not include customer plumbing repairs)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),67.00,175.00,100.00" +
                " UNION ALL SELECT 19, '1.5\" through 2\" (does not include customer plumbing repairs)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),125.00,375.00,100.00" +

                // New Meter (2)
                " UNION ALL SELECT 20, '5/8\" through 1\" (does not include customer plumbing repairs)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),73.00,175.00,100.00" +
                " UNION ALL SELECT 20, '1.5\" through 2\" (does not include customer plumbing repairs)', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),132.00,375.00,100.00" +
                "");

            Execute.Sql(
                "INSERT INTO ScheduleOfValues(ScheduleOfValueCategoryId, Description, UnitOfMeasureId, LaborUnitCost, LaborUnitOvertimeCost) " +
                // Labor (All wages are $/hour)
                " SELECT 21, 'Foreman', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),85.47,112.00" +
                " UNION ALL SELECT 21, 'Mechanic', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),96.23,137.20" +
                " UNION ALL SELECT 21, 'Laborer', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),81.76,107.00" +
                " UNION ALL SELECT 21, 'Equipment Operator', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),98.43,144.00" +
                " UNION ALL SELECT 21, 'Welder', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),93.74,125.55" +
                " UNION ALL SELECT 21, 'Driver', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),82.63,109.10" +
                " UNION ALL SELECT 21, 'Flagger', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),81.76,100.38" +
                " UNION ALL SELECT 21, 'American Water Supervisor', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),91.84,137.76" +
                " UNION ALL SELECT 21, 'Downtime', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),796.65,NULL" +
                // Equipment
                " UNION ALL SELECT 22, 'Price per hour for Crew Truck (includes all tools and equipment to perform job)', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),50.00,NULL" +
                " UNION ALL SELECT 22, 'Price per hour for Single Axle Dump Truck', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),48.00,NULL" +
                " UNION ALL SELECT 22, 'Price per hour for Tadem Dump Truck', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),100.00,NULL" +
                " UNION ALL SELECT 22, 'Price per hour for Backhoe (John Deere 310 or equivalent includes delivery to and from work site)', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),45.00,NULL" +
                " UNION ALL SELECT 22, 'Price per hour for Excavator (John Deere 160 or equivalent includes delivery to and from work site)', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),69.00,NULL" +
                " UNION ALL SELECT 22, 'Price per hour for Excavator (John Deere 225 or equivalent includes delivery to and from work site)', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),111.00,NULL" +
                " UNION ALL SELECT 22, 'Price per hour for Excavator (John Deere 300 or equivalent includes delivery to and from work site)', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),146.00,NULL" +
                " UNION ALL SELECT 22, 'Price per hour for Hoe-Ram Breaker (or equivalent includes delivery to and from work site)', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),175.00,NULL" +
                " UNION ALL SELECT 22, 'Price per hour for Arrow Board (includes delivery to and from work site)', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),20.00,NULL" +
                " UNION ALL SELECT 22, 'Price per hour for Message Board (includes delivery to and from work site)', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),19.00,NULL" +
                " UNION ALL SELECT 22, 'Price per hour for TMA (crash truck includes delivery to and from work site)', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),31.50,NULL" +
                " UNION ALL SELECT 22, 'Mini Excavator Rubber Track (CAT 305 or equivalent)', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),33.50,NULL" +
                " UNION ALL SELECT 22, 'Laymor (small 3 wheel broom)', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),21.00,NULL" +
                " UNION ALL SELECT 22, 'Wheel Loader (CAT 936 or equivalent)', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),68.50,NULL" +
                " UNION ALL SELECT 22, 'Dozer (John Deere 450 or equivalent)', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),54.00,NULL" +
                " UNION ALL SELECT 22, 'Trench Box up to 8'' X 14''', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),10.00,NULL" +
                " UNION ALL SELECT 22, 'Trench Box over to 8'' X 14''', (select UnitOfMeasureID from unitsofMeasure where Description = 'HR'),13.00,NULL" +
                // Restoration materials
                " UNION ALL SELECT 23, 'RCA', (select UnitOfMeasureID from unitsofMeasure where Description = 'Tons'),13.50,NULL" +
                " UNION ALL SELECT 23, 'DGA', (select UnitOfMeasureID from unitsofMeasure where Description = 'Tons'),14.00,NULL" +
                " UNION ALL SELECT 23, '3/4\" clean stone', (select UnitOfMeasureID from unitsofMeasure where Description = 'Tons'),15.00,NULL" +
                " UNION ALL SELECT 23, 'Disposal of Asphalt', (select UnitOfMeasureID from unitsofMeasure where Description = 'Load'),300.00,NULL" +
                " UNION ALL SELECT 23, 'Disposal of Spoils', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),150.00,NULL" +
                // Temporary Asphalt Paving (depth as required by State or Municipality)
                " UNION ALL SELECT 24, 'Cold Patch', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),1.69,NULL" +
                " UNION ALL SELECT 24, 'Hot Patch  (up to 4\") ', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),2.02,NULL" +
                " UNION ALL SELECT 24, 'Hot Patch  7\" ', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),3.54,NULL" +
                " UNION ALL SELECT 24, '4\" Base to Grade Course', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),2.02,NULL" +
                " UNION ALL SELECT 24, '6\" Base to Grade Course', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),3.03,NULL" +
                // Temporary Sidewalk, Curb  & Driveway Replacement 
                " UNION ALL SELECT 25, 'Cold Patch', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),10.12,NULL" +
                " UNION ALL SELECT 25, 'Hot Patch  (up to 4\") ', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),13.15,NULL" +
                " UNION ALL SELECT 25, 'Hot Patch  7\" ', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),20.23,NULL" +
                " UNION ALL SELECT 25, '4\" Base to Grade Course', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),10.12,NULL" +
                " UNION ALL SELECT 25, '6\" Base to Grade Course', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),5.82,NULL" +
                // Permanent Restoration
                " UNION ALL SELECT 26, '4\" Bit. Base course (1 - 500 sf)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),8.00,NULL" +
                " UNION ALL SELECT 26, '5\" Bit. Base course (1 - 500 sf)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),8.50,NULL" +
                " UNION ALL SELECT 26, '6\" Bit. Base course (1 - 500 sf)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),9.00,NULL" +
                " UNION ALL SELECT 26, '8\" Bit. Base course (1 -500 sf)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),10.00,NULL" +
                " UNION ALL SELECT 26, '10\" Bit. Base course (1 - 500 sf)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),12.50,NULL" +
                " UNION ALL SELECT 26, '2\" Surface Course (1 - 100 sf)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),3.00,NULL" +
                " UNION ALL SELECT 26, '2.5\" Surface Course (1 - 100 sf)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),3.75,NULL" +
                " UNION ALL SELECT 26, '3\" Surface Course (1 - 100 sf)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),4.50,NULL" +
                " UNION ALL SELECT 26, '6\" Concrete Pavement', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),13.75,NULL" +
                " UNION ALL SELECT 26, '8\" Concrete Pavement', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),16.25,NULL" +
                " UNION ALL SELECT 26, '10\" Concrete Pavement', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),18.75,NULL" +
                " UNION ALL SELECT 26, '6x18 Curbing (0-50LF)', (select UnitOfMeasureID from unitsofMeasure where Description = 'LF'),31.25,NULL" +
                " UNION ALL SELECT 26, 'Mono w/ gutter Curbing (0-50LF)', (select UnitOfMeasureID from unitsofMeasure where Description = 'LF'),37.50,NULL" +
                " UNION ALL SELECT 26, 'Belgium Curbing (0-50LF)', (select UnitOfMeasureID from unitsofMeasure where Description = 'LF'),37.50,NULL" +
                " UNION ALL SELECT 26, 'Asphalt Curbing (0-50LF)', (select UnitOfMeasureID from unitsofMeasure where Description = 'LF'),15.00,NULL" +
                " UNION ALL SELECT 26, 'RCA Subgrade', (select UnitOfMeasureID from unitsofMeasure where Description = 'Tons'),20.00,NULL" +
                " UNION ALL SELECT 26, '6\" Driveways/Apron Concrete (1-100 SF)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),13.00,NULL" +
                " UNION ALL SELECT 26, '4\" Driveways/Apron Bituminous (1-500 SF)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),6.00,NULL" +
                " UNION ALL SELECT 26, '4\" Sidewalk Concrete (1-100 SF)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),12.50,NULL" +
                " UNION ALL SELECT 26, '2\" Sidewalk Bituminous (1-500SF)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),6.00,NULL" +
                " UNION ALL SELECT 26, '4\" Breakout Asphalt', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),4.25,NULL" +
                " UNION ALL SELECT 26, '6\" Breakout Asphalt', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),4.25,NULL" +
                " UNION ALL SELECT 26, '8\" Breakout Asphalt', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),4.25,NULL" +
                " UNION ALL SELECT 26, '10\" Breakout Asphalt', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),6.50,NULL" +
                " UNION ALL SELECT 26, '12\" Breakout Asphalt', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),7.75,NULL" +
                " UNION ALL SELECT 26, '4\" Breakout Concrete', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),6.50,NULL" +
                " UNION ALL SELECT 26, '6\" Breakout Concrete', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),6.50,NULL" +
                " UNION ALL SELECT 26, '10\" Breakout Concrete', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),8.75,NULL" +
                " UNION ALL SELECT 26, '2\" Milling Concrete (1-1000 SF)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),4.75,NULL" +
                " UNION ALL SELECT 26, '3\" Milling Concrete (1-1000 SF)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),6.75,NULL" +
                " UNION ALL SELECT 26, '2\" Milling Asphalt (1-1000 SF)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),3.50,NULL" +
                " UNION ALL SELECT 26, '4\" Striping', (select UnitOfMeasureID from unitsofMeasure where Description = 'LF'),3.00,NULL" +
                " UNION ALL SELECT 26, 'Topsoil and Seed (0-50SF)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),2.00,NULL" +
                " UNION ALL SELECT 26, 'Topsoil and Sod (0-50SF)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),2.50,NULL" +
                " UNION ALL SELECT 26, 'Topsoil and Zozya', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),3.00,NULL" +
                " UNION ALL SELECT 26, 'Asphalt Priority Response', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),500.00,NULL" +
                " UNION ALL SELECT 26, 'Asphalt Emergency Response', (select UnitOfMeasureID from unitsofMeasure where Description = 'EA'),1000.00,NULL" +
                " UNION ALL SELECT 26, 'Infrared Paving (30-500SF)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),6.00,NULL" +
                " UNION ALL SELECT 26, 'Infrared Paving (over 500SF)', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),5.85,NULL" +
                " UNION ALL SELECT 26, 'Cementitious', (select UnitOfMeasureID from unitsofMeasure where Description = 'Yard'),475.00,NULL" +
                " UNION ALL SELECT 26, 'Pavers', (select UnitOfMeasureID from unitsofMeasure where Description = 'SF'),18.00,NULL" +
                " UNION ALL SELECT 27, 'Other', null, null, null" +
                " UNION ALL SELECT 28, 'Other', null, null, null" +
                "");

            #endregion

            #region WorkOrder

            Alter.Table("WorkOrders")
                 .AddColumn("RequiresInvoice").AsBoolean().Nullable();

            #endregion

            #region WorkOrder Children

            // link work order invoices to things
            // prices change over time, so they are needed here as well

            // WorkOrdersScheduleOfValues
            // Foreman will enter these values for the work orders, they will be copied
            // to the invoice when it is created and record the costs at that time.
            Create.Table("WorkOrdersScheduleOfValues")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("WorkOrderID", "WorkOrders", "WorkOrderID")
                  .WithForeignKeyColumn("ScheduleOfValueId", "ScheduleOfValues")
                  .WithColumn("IsOvertime").AsBoolean().NotNullable().WithDefaultValue(false)
                  .WithColumn("Total").AsDecimal(10, 2).NotNullable()
                  .WithColumn("LaborUnitCost").AsDecimal(10, 2).NotNullable()
                  .WithColumn("OtherDescription").AsAnsiString(50).Nullable();

            // WorkOrderInvoicesScheduleOfValues
            Create.Table("WorkOrderInvoicesScheduleOfValues")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("WorkOrderInvoiceId", "WorkOrderInvoices")
                  .WithForeignKeyColumn("ScheduleOfValueId", "ScheduleOfValues")
                  .WithColumn("LaborUnitCost").AsDecimal(10, 2).Nullable()
                  .WithColumn("MaterialCost").AsDecimal(10, 2).Nullable()
                  .WithColumn("MiscCost").AsDecimal(10, 2).Nullable()
                  .WithColumn("Total").AsDecimal(10, 2).Nullable()
                  .WithColumn("IsOvertime").AsBoolean().NotNullable().WithDefaultValue(false)
                  .WithColumn("OtherDescription").AsAnsiString(50).Nullable();

            #endregion
        }

        public override void Down()
        {
            Delete.Table("WorkOrderInvoicesScheduleOfValues");
            Delete.Table("WorkOrdersScheduleOfValues");
            Delete.Table("ScheduleOfValues");
            Delete.Table("ScheduleOfValueCategories");
            Delete.Table("WorkOrderInvoiceStatuses");

            Delete.Column("RequiresInvoice").FromTable("WorkOrders");

            // UnitsOfMeasure
            Execute.Sql("DELETE FROM UnitsOfMeasure where Description = 'HR'");
            Execute.Sql("DELETE FROM UnitsOfMeasure where Description = 'Tons'");
            Execute.Sql("DELETE FROM UnitsOfMeasure where Description = 'Load'");
            Execute.Sql("DELETE FROM UnitsOfMeasure where Description = 'SF'");
            Execute.Sql("DELETE FROM UnitsOfMeasure where Description = 'LF'");
            Execute.Sql("DELETE FROM UnitsOfMeasure where Description = 'EA'");
            Execute.Sql("DELETE FROM UnitsOfMeasure where Description = 'Yard'");

            //WorkOrderInvoices
            this.DeleteNotificationPurpose("Field Services", "Work Order Invoice", "Work Order Invoice Submitted");
            this.DeleteNotificationPurpose("Field Services", "Work Order Invoice", "Work Order Invoice Canceled");
            this.DeleteDataType("WorkOrderInvoices");
            Delete.Table("WorkOrderInvoices");
            Delete.Table("ScheduleOfValueTypes");
            Execute.Sql("DELETE FROM Roles where ModuleId = " + WORK_ORDER_INVOICE);
            Execute.Sql("DELETE FROM Modules where ModuleId = " + WORK_ORDER_INVOICE);

            // OperatingCenters
            Delete.Column("HasWorkOrderInvoicing").FromTable("OperatingCenters");
            // Operating Center Stocked Materials
            Execute.Sql(
                "Delete from OperatingCenterStockedMaterials where OperatingCenterID = (select OperatingCenterID from OperatingCenters where OperatingCenterCode = 'SOV')");
            // Operating Center Stock Locations
            // Execute.Sql("DELETE FROM StockLocations WHERE OperatingCenterID = (select OperatingCenterID from OperatingCenters where OperatingCenterCode = 'SOV');");
        }
    }
}
