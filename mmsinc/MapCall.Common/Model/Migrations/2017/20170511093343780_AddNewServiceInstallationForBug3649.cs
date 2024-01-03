using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170511093343780), Tags("Production")]
    public class AddNewServiceInstallationForBug3649 : Migration
    {
        #region Constants

        public struct StringLengths
        {
            public const int
                METER_MANUFACTURER_SERIAL_NUMBER = 18,
                SERVICE_TYPE = 30,
                MANUFACTURER = 30,
                METER_SERIAL_NUMBER = 18,
                MATERIAL_NUMBER = 18,
                METER_SIZE = 30,
                TP_ENCODER_ID = 30,
                CURRENT_READ = 17,
                RFMIU = 30,
                SIZE = 30,
                DIALS = 30,
                UNIT_OF_MEASURE = 30,
                SAP_CODE = 3,
                CODE_GROUP = 10,
                DESCRIPTION = 50;
        }

        #endregion

        public override void Up()
        {
            Alter.Table("SAPWorkOrderPurposes").AddColumn("CodeGroup").AsAnsiString(StringLengths.CODE_GROUP)
                 .Nullable();
            Execute.Sql("UPDATE [SAPWorkOrderPurposes] set CodeGroup = 'N-D-PUR1';");
            this.CreateLookupTableWithValues("ServiceInstallationReasons", "Customer Request", "New Service",
                "Install RF", "AMI New Install");

            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 from SmallMeterLocations where Description = 'Kitchen') INSERT INTO SmallMeterLocations(Description) Values('Kitchen');");
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 from SmallMeterLocations where Description = 'Unable to Verify') INSERT INTO SmallMeterLocations(Description) Values('Unable To Verify');");
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 from MeterDirections where Description = 'Unable to Verify') INSERT INTO MeterDirections Values('Unable to Verify');");
            // Setup MeterSupplementLocation/SmallMeterLocation Mapping

            Create.Table("MeterLocationMeterPositions")
                  .WithForeignKeyColumn("MeterLocationId", "MeterSupplementalLocations")
                  .WithForeignKeyColumn("SmallMeterLocationId", "SmallMeterLocations");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 1, Id from SmallMeterLocations where Description = 'Cellar / Basement'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 1, Id from SmallMeterLocations where Description = 'Bathroom'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 1, Id from SmallMeterLocations where Description = 'Closet'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 1, Id from SmallMeterLocations where Description = 'Crawl Space'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 1, Id from SmallMeterLocations where Description = 'Utility Room'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 1, Id from SmallMeterLocations where Description = 'Garage'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 1, Id from SmallMeterLocations where Description = 'Shop'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 1, Id from SmallMeterLocations where Description = 'Main Building'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 1, Id from SmallMeterLocations where Description = 'Kitchen'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 1, Id from SmallMeterLocations where Description = 'Unable to Verify'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 1, Id from SmallMeterLocations where Description = 'Wall'");

            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 2, Id from SmallMeterLocations where Description = 'Curb'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 2, Id from SmallMeterLocations where Description = 'Sidewalk'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 2, Id from SmallMeterLocations where Description = 'Driveway'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 2, Id from SmallMeterLocations where Description = 'Parking Lot'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 2, Id from SmallMeterLocations where Description = 'Main Building'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 2, Id from SmallMeterLocations where Description = 'Fence'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 2, Id from SmallMeterLocations where Description = 'Property Line'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 2, Id from SmallMeterLocations where Description = 'Street'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 2, Id from SmallMeterLocations where Description = 'Lawn / Field'");
            Execute.Sql(
                "INSERT INTO MeterLocationMeterPositions SELECT 2, Id from SmallMeterLocations where Description = 'Vault'");

            Execute.Sql("INSERT INTO MeterLocationMeterPositions SELECT 3, Id from SmallMeterLocations");

            //ReadTypes -- ServiceInstallationReadTypes
            Create.Table("ServiceInstallationReadTypes")
                  .WithIdentityColumn()
                  .WithColumn("SAPCode").AsAnsiString(StringLengths.SAP_CODE).NotNullable()
                  .WithColumn("Description").AsAnsiString(StringLengths.DESCRIPTION);
            Insert.IntoTable("ServiceInstallationReadTypes").Rows(
                new {SAPCode = "01", Description = "Keyed"},
                new {SAPCode = "03", Description = "Neptune Radio Read ITRON Meter"},
                new {SAPCode = "08", Description = "TP Probe Neptune Only"},
                new {SAPCode = "11", Description = "AMR Neptune"},
                new {SAPCode = "12", Description = "AMI Neptune"},
                new {SAPCode = "13", Description = "AMR Mueller"},
                new {SAPCode = "14", Description = "AMI Mueller"},
                new {SAPCode = "15", Description = "AMR Itron - MVRS"},
                new {SAPCode = "16", Description = "AMI Itron - MVRS"},
                new {SAPCode = "19", Description = "TP Probe Mueller only"},
                new {SAPCode = "20", Description = "AMI Aclara - Equinox"},
                new {SAPCode = "21", Description = "AMI Aclara - MVRS"},
                new {SAPCode = "22", Description = "AMI - Badger"},
                new {SAPCode = "23", Description = "AMR - Badger"},
                new {SAPCode = "24", Description = "AMI - Sensus"},
                new {SAPCode = "25", Description = "AMR - Sensus"},
                new {SAPCode = "E", Description = "AMR Radio Reads - MVRS"},
                new {SAPCode = "T", Description = "TP MVRS only"},
                new {SAPCode = "X", Description = "TP (WRR MVRS Record)"}
            );

            // ActivityType1
            Create.Table("ServiceInstallationFirstActivities")
                  .WithIdentityColumn()
                  .WithColumn("CodeGroup").AsAnsiString(StringLengths.CODE_GROUP).NotNullable()
                  .WithColumn("SAPCode").AsAnsiString(StringLengths.SAP_CODE).NotNullable()
                  .WithColumn("Description").AsAnsiString(StringLengths.DESCRIPTION);
            Execute.Sql("INSERT INTO ServiceInstallationFirstActivities Values('C-A-ALL1','I49','AMI New Install')");
            Execute.Sql("INSERT INTO ServiceInstallationFirstActivities Values('C-A-ALL1','I18','Meter Installed')");

            // ActivityType2
            Create.Table("ServiceInstallationSecondActivities")
                  .WithIdentityColumn()
                  .WithColumn("CodeGroup").AsAnsiString(StringLengths.CODE_GROUP).NotNullable()
                  .WithColumn("SAPCode").AsAnsiString(StringLengths.SAP_CODE).NotNullable()
                  .WithColumn("Description").AsAnsiString(StringLengths.DESCRIPTION);
            Execute.Sql("INSERT INTO ServiceInstallationSecondActivities Values('C-A-ALL2','I33','RF Install')");
            Execute.Sql("INSERT INTO ServiceInstallationSecondActivities Values('C-A-ALL2','I49','AMI New Install')");

            // ActivityType3
            Create.Table("ServiceInstallationThirdActivities")
                  .WithIdentityColumn()
                  .WithColumn("CodeGroup").AsAnsiString(StringLengths.CODE_GROUP).NotNullable()
                  .WithColumn("SAPCode").AsAnsiString(StringLengths.SAP_CODE).NotNullable()
                  .WithColumn("Description").AsAnsiString(StringLengths.DESCRIPTION);
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I01', 'Called Ahead - no answer');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I02', 'Cross Connection Inspected');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I03', 'Curb Box Repaired');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I04', 'Curb Box/Pit Cleaned Out');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I05', 'Curb Stop Located/marked out');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I06', 'Leak Inspection -  Curb/Street');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I07', 'Leak Inspection - Company Issue');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I08', 'Leak Inspection - Customer Issue');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I09', 'Left Company Information');");
            Execute.Sql("INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I10', 'Locked Setting');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I11', 'Meter  - Verified movement');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I12', 'Meter - Verified no movement');");
            Execute.Sql("INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I13', 'Meter # Verified');");
            Execute.Sql("INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I14', 'Meter Abandoned');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I15', 'Meter Backwards - Corrected');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I16', 'Meter Backwards - Not Corrected');");
            Execute.Sql("INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I17', 'Meter Changed');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I19', 'Meter Moved to Curb');");
            Execute.Sql("INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I20', 'Meter Read');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I21', 'Meter Read/Inspected with Cust.');");
            Execute.Sql("INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I22', 'Meter Removed');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I23', 'Meter Tested in place');");
            Execute.Sql("INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I24', 'Pit Pumped Out');");
            Execute.Sql("INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I25', 'Pit Repair');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I26', 'Posted for Non-Pay');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I27', 'Pressure regulator - Changed, AW owned');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I28', 'Pressure Test - Company issue');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I29', 'Pressure Test - Customer Issue');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I30', 'Reading Device Repaired');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I31', 'Reading Device Replaced');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I32', 'Register Replaced');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I34', 'Sewer Issue - Company Responsible');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I35', 'Sewer Issue - not AW');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I36', 'Smart Valve Installed');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I37', 'Sounded Service - no noise');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I38', 'Sounded Service - noise');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I39', 'Touchpad Installed');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I40', 'Unable to Execute');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I41', 'Unable to Obtain Meter Read');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I42', 'Unable to Turn off/on');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I43', 'Water Sample Collected');");
            Execute.Sql("INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I44', 'Water Turned Off');");
            Execute.Sql("INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I45', 'Water Turned On');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I46', 'Witnessed Tap Destroy');");
            Execute.Sql("INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I47', 'Gave Time to Pay');");
            Execute.Sql("INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I48', 'Property Vacant');");
            Execute.Sql(
                "INSERT INTO ServiceInstallationThirdActivities Values('C-A-ALL8','I50', 'AMI Repair/Replace');");

            // Additional Work
            Create.Table("ServiceInstallationAdditionalWorkTypes")
                  .WithIdentityColumn()
                  .WithColumn("CodeGroup").AsAnsiString(StringLengths.CODE_GROUP).NotNullable()
                  .WithColumn("SAPCode").AsAnsiString(StringLengths.SAP_CODE).NotNullable()
                  .WithColumn("Description").AsAnsiString(StringLengths.DESCRIPTION);
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F01','Raise/lower pit to grade')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F02','Raise/lower curb box to grade')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F03','Locate curb stop')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F04','Repair/replace curb box')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F05','Repairs to Ring and Cover')");
            Execute.Sql("INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F06','Repairs to pit')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F07','Curb stop broken')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F08','Low Pressure-company side')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F09','Leak on company side')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F10','Locate Meter Pit')");
            Execute.Sql("INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F11','Restoration')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F12','Locate Curb Stop - Emergency')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F13','Locate Meter Pit - Emergency')");
            Execute.Sql("INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F14','Install Pit')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F15','Replace Meter Pit Top Section')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F16','Add Curb Box Offset')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F17','Valve inoperable')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F18','Vault inaccessible')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F19','Poor Pipe Integrity')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F20','Customers valve not working')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F21','Locate and operate Gate valve')");
            Execute.Sql("INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F22','Replace Meter')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F23','Remove Meter Demo')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F24','By pass not working')");
            Execute.Sql("INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F25','Pump Out Vault')");
            Execute.Sql("INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F26','Clean Curb Box')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F27','Install Resetter')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F28','Clean Curb Box and Install Resetter')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F29','Check for leak in Curb Box')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationAdditionalWorkTypes Values('CIS-I-01','F30','Repair Reading Device')");

            // Service Found
            // Service Left
            Create.Table("ServiceInstallationPositions")
                  .WithIdentityColumn()
                  .WithColumn("CodeGroup").AsAnsiString(StringLengths.CODE_GROUP).NotNullable()
                  .WithColumn("SAPCode").AsAnsiString(StringLengths.SAP_CODE).NotNullable()
                  .WithColumn("Description").AsAnsiString(StringLengths.DESCRIPTION);
            Execute.Sql("INSERT INTO ServiceInstallationPositions Values('CIS-I-FD', 'I01','On at Curb Stop')");
            Execute.Sql("INSERT INTO ServiceInstallationPositions Values('CIS-I-FD', 'I02','Off at the Curb Stop')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationPositions Values('CIS-I-FD', 'I03','On at Customer Valve Inside')");
            Execute.Sql(
                "INSERT INTO ServiceInstallationPositions Values('CIS-I-FD', 'I04','Off at Customer Valve Inside')");
            Execute.Sql("INSERT INTO ServiceInstallationPositions Values('CIS-I-FD', 'I05','On at Pit')");
            Execute.Sql("INSERT INTO ServiceInstallationPositions Values('CIS-I-FD', 'I06','Off at Pit')");
            Execute.Sql("INSERT INTO ServiceInstallationPositions Values('CIS-I-FD', 'I07','On at Corporation')");
            Execute.Sql("INSERT INTO ServiceInstallationPositions Values('CIS-I-FD', 'I08','Off at Corporation')");
            Execute.Sql("INSERT INTO ServiceInstallationPositions Values('CIS-I-FD', 'I09','Unable to Verify')");

            Create.Table("SAPProjectTypes")
                  .WithIdentityColumn()
                  .WithColumn("SAPCode").AsAnsiString(StringLengths.SAP_CODE).NotNullable()
                  .WithColumn("Description").AsAnsiString(StringLengths.DESCRIPTION);
            Execute.Sql("INSERT INTO SAPProjectTypes Values('AV','OTHBSC - 18680215-Rogue Creek Opex & Cap')");
            Execute.Sql("INSERT INTO SAPProjectTypes Values('BL','CAP - PP - Blanket')");
            Execute.Sql("INSERT INTO SAPProjectTypes Values('DC','CAP - PP - DV Assets-Pd Upon Comp')");
            Execute.Sql("INSERT INTO SAPProjectTypes Values('DU','CAP - PP - DV Assets-Pd Upfront')");
            Execute.Sql("INSERT INTO SAPProjectTypes Values('ET','CAP - Expenditure Type')");
            Execute.Sql("INSERT INTO SAPProjectTypes Values('IG','CAP - PP - Investment >30 Days')");
            Execute.Sql("INSERT INTO SAPProjectTypes Values('IL','CAP - PP - Investment <30 Days')");
            Execute.Sql("INSERT INTO SAPProjectTypes Values('LL','CAP - PP - Land & Land Rights Purchase')");
            Execute.Sql("INSERT INTO SAPProjectTypes Values('OM','O&M - Functional Class')");
            Execute.Sql("INSERT INTO SAPProjectTypes Values('PE','CAP - Planning Element Only')");
            Execute.Sql("INSERT INTO SAPProjectTypes Values('PS','CAP - PP - Comprehensive Planning Study')");
            Execute.Sql("INSERT INTO SAPProjectTypes Values('RA','CAP - PP - Regulatory Asset-Capital')");
            Execute.Sql("INSERT INTO SAPProjectTypes Values('RO','CAP - PP - Retirement Only')");
            Execute.Sql("INSERT INTO SAPProjectTypes Values('SE','CAP - Service Company Eligible')");
            Execute.Sql("INSERT INTO SAPProjectTypes Values('ST','CAP - Project Stage')");
            Execute.Sql("INSERT INTO SAPProjectTypes Values('TA','CAP - Budget Tracking Assignment')");

            Create.Table("ServiceInstallations")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("WorkOrderID", "WorkOrders", "WorkOrderID")
                   // --- Meter Properties 
                  .WithColumn("MeterManufacturerSerialNumber")
                  .AsAnsiString(StringLengths.METER_MANUFACTURER_SERIAL_NUMBER)
                  .NotNullable()
                  .WithColumn("Manufacturer").AsAnsiString(StringLengths.MANUFACTURER).Nullable()
                  .WithColumn("ServiceType").AsAnsiString(StringLengths.SERVICE_TYPE).Nullable()
                  .WithColumn("MeterSerialNumber").AsAnsiString(StringLengths.METER_SERIAL_NUMBER).Nullable()
                  .WithColumn("MaterialNumber").AsAnsiString(StringLengths.MATERIAL_NUMBER).Nullable()
                  .WithColumn("MeterSize").AsAnsiString(StringLengths.METER_SIZE).Nullable()
                  .WithForeignKeyColumn("MeterLocationId", "MeterSupplementalLocations").NotNullable()
                  .WithForeignKeyColumn("MeterPositionalLocationId", "SmallMeterLocations").NotNullable()
                  .WithForeignKeyColumn("MeterDirectionalLocationId", "MeterDirections").NotNullable()
                  .WithForeignKeyColumn("ReadingDevicePositionId", "SmallMeterLocations").NotNullable()
                  .WithForeignKeyColumn("ReadingDeviceSupplementalId", "MeterSupplementalLocations").NotNullable()
                  .WithForeignKeyColumn("ReadingDeviceDirectionalLocationId", "MeterDirections").NotNullable()
                   // --- Register 1 Information
                  .WithColumn("Register1Dials").AsAnsiString(StringLengths.DIALS).Nullable()
                  .WithColumn("Register1UnitOfMeasure").AsAnsiString(StringLengths.UNIT_OF_MEASURE).Nullable()
                  .WithForeignKeyColumn("Register1ReadTypeId", "ServiceInstallationReadTypes").NotNullable()
                  .WithColumn("Register1RFMIU").AsAnsiString(StringLengths.RFMIU).NotNullable()
                  .WithColumn("Register1Size").AsAnsiString(StringLengths.SIZE).Nullable()
                  .WithColumn("Register1TPEncoderID").AsAnsiString(StringLengths.TP_ENCODER_ID).Nullable()
                  .WithColumn("Register1CurrentRead").AsAnsiString(StringLengths.CURRENT_READ).NotNullable()
                   // --- Register 2 Information    
                  .WithColumn("Register2Dials").AsAnsiString(StringLengths.DIALS).Nullable()
                  .WithColumn("Register2UnitOfMeasure").AsAnsiString(StringLengths.UNIT_OF_MEASURE).Nullable()
                  .WithForeignKeyColumn("Register2ReadTypeId", "ServiceInstallationReadTypes").Nullable()
                  .WithColumn("Register2RFMIU").AsAnsiString(StringLengths.RFMIU).Nullable()
                  .WithColumn("Register2Size").AsAnsiString(StringLengths.SIZE).Nullable()
                  .WithColumn("Register2TPEncoderID").AsAnsiString(StringLengths.TP_ENCODER_ID).Nullable()
                  .WithColumn("Register2CurrentRead").AsAnsiString(StringLengths.CURRENT_READ).Nullable()
                   // --- FIELD ACTIVITY
                  .WithForeignKeyColumn("Activity1Id", "ServiceInstallationFirstActivities").NotNullable()
                  .WithForeignKeyColumn("Activity2Id", "ServiceInstallationSecondActivities").Nullable()
                  .WithForeignKeyColumn("Activity3Id", "ServiceInstallationThirdActivities").Nullable()
                  .WithForeignKeyColumn("AdditionalWorkNeededId", "ServiceInstallationAdditionalWorkTypes").Nullable()
                  .WithForeignKeyColumn("PurposeId", "SAPWorkOrderPurposes").Nullable()
                  .WithForeignKeyColumn("ServiceFoundId", "ServiceInstallationPositions").NotNullable()
                  .WithForeignKeyColumn("ServiceLeftId", "ServiceInstallationPositions").NotNullable()
                  .WithColumn("OperatedPointOfControl").AsBoolean().NotNullable()
                  .WithForeignKeyColumn("ServiceInstallationReasonId", "ServiceInstallationReasons").NotNullable() //2
                  .WithColumn("MeterLocationInformation").AsCustom("text").NotNullable()
                  .WithColumn("SAPErrorCode").AsCustom("varchar(max)").Nullable();

            Alter.Table("Services").AddColumn("DeviceLocation").AsAnsiString(30).Nullable();
            Alter.Table("Services").AddColumn("DeviceLocationUnavailable").AsBoolean().Nullable();
            Alter.Table("Services").AddColumn("SAPErrorCode").AsCustom("varchar(max)").Nullable();
            Alter.Table("ServiceInstallationPurposes")
                 .AddColumn("SAPCode")
                 .AsAnsiString(StringLengths.SAP_CODE)
                 .Nullable();

            Execute.Sql("update ServiceInstallationPurposes set SAPCode = '02' WHERE Description = 'Main Extension'" +
                        "update ServiceInstallationPurposes set SAPCode = '22' WHERE Description = 'New Service'" +
                        "update ServiceInstallationPurposes set SAPCode = '12' WHERE Description = 'Customer Request'");

            //meter location
            Alter.Table("MeterSupplementalLocations").AddColumn("SAPCode").AsAnsiString(8).Nullable();
            Execute.Sql("update MeterSupplementalLocations SET SAPCode = 'IS' WHERE Id = 1;" +
                        "update MeterSupplementalLocations SET SAPCode = 'OS' WHERE Id = 2;" +
                        "update MeterSupplementalLocations SET SAPCode = 'SA' WHERE Id = 3;");

            //meter positional location
            Alter.Table("SmallMeterLocations").AddColumn("SAPCode").AsAnsiString(8).Nullable();
            Execute.Sql("UPDATE SmallMeterLocations SET SAPCode = '1A' WHERE Id = 1; " +
                        "UPDATE SmallMeterLocations SET SAPCode = '4A' WHERE Id = 2;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '1U' WHERE Id = 3;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '1B' WHERE Id = 4;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '1C' WHERE Id = 5;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '1D' WHERE Id = 6;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '4B' WHERE Id = 7;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '4C' WHERE Id = 8;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '4D' WHERE Id = 9;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '4G' WHERE Id = 10;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '4H' WHERE Id = 11;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '4I' WHERE Id = 12;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '4J' WHERE Id = 13;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '4K' WHERE Id = 14;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '7A' WHERE Id = 15;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '7B' WHERE Id = 16;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '7D' WHERE Id = 17;" +
                        "UPDATE SmallMeterLocations SET SAPCode = '7E' WHERE Id = 18;" +
                        "UPDATE SmallMeterLocations SET SAPCode = 'VT' WHERE Id = 19;" + // VAULT
                        "UPDATE SmallMeterLocations SET SAPCode = 'KT' WHERE Id = 20;" +
                        "UPDATE SmallMeterLocations SET SAPCode = 'UV' WHERE Id = 21;");
            //meter directional location
            Alter.Table("MeterDirections").AddColumn("SAPCode").AsAnsiString(8).Nullable();
            Execute.Sql(
                "UPDATE MeterDirections SET SAPCode = 'FR' WHERE Id = 1;" + // FRONT
                "UPDATE MeterDirections SET SAPCode = 'LS' WHERE Id = 2;" + // left
                "UPDATE MeterDirections SET SAPCode = 'RR' WHERE Id = 3;" + // rear
                "UPDATE MeterDirections SET SAPCode = 'RS' WHERE Id = 4;" + // right
                "UPDATE MeterDirections SET SAPCode = 'UY' WHERE Id = 5;"); // unknown

            Execute.Sql(
                "if not exists (select 1 from SAPWorkOrderSteps where Description = 'UPDATE WITH NMI') insert into SAPWorkOrderSteps values('UPDATE WITH NMI');" +
                "if not exists(select 1 from SAPWorkOrderSteps where Description = 'NMI') insert into SAPWorkOrderSteps values('NMI');");
        }

        public override void Down()
        {
            Delete.Table("SAPProjectTypes");
            Delete.Table("ServiceInstallations");
            Delete.Table("ServiceInstallationPositions");
            Delete.Table("ServiceInstallationAdditionalWorkTypes");
            Delete.Table("ServiceInstallationThirdActivities");
            Delete.Table("ServiceInstallationSecondActivities");
            Delete.Table("ServiceInstallationFirstActivities");
            Delete.Table("ServiceInstallationReadTypes");
            Delete.Table("MeterLocationMeterPositions");
            Delete.Table("ServiceInstallationReasons");
            Delete.Column("CodeGroup").FromTable("SAPWorkOrderPurposes");
            Delete.Column("DeviceLocation").FromTable("Services");
            Delete.Column("DeviceLocationUnavailable").FromTable("Services");
            Delete.Column("SAPCode").FromTable("ServiceInstallationPurposes");
            Delete.Column("SAPErrorCode").FromTable("Services");

            Delete.Column("SAPCode").FromTable("MeterSupplementalLocations");
            Delete.Column("SAPCode").FromTable("SmallMeterLocations");
            Delete.Column("SAPCode").FromTable("MeterDirections");
        }
    }
}
