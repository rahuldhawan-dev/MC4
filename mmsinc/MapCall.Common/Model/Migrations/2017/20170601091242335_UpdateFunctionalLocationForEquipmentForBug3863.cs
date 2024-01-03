using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170601091242335), Tags("Production")]
    public class UpdateFunctionalLocationForEquipmentForBug3863 : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey("FK_Equipment_FunctionalLocations_FunctionalLocationID").OnTable("Equipment");

            Alter.Table("Equipment").AddColumn("SAPErrorCode").AsCustom("varchar(max)").Nullable();
            Alter.Column("FunctionalLocationId").OnTable("Equipment").AsAnsiString(30).Nullable();
            Execute.Sql(
                "UPDATE Equipment Set FunctionalLocationId = (Select fl.Description from FunctionalLocations fl where fl.FunctionalLocationId = equipment.FunctionalLocationId)");

            this.CreateSAPLookupTable("FunctionalLocationCategories", 2);
            Insert.IntoTable("FunctionalLocationCategories").Rows(
                new {SAPCode = "C", Description = "Waste Water Coll Network"},
                new {SAPCode = "D", Description = "Main Water Dist Network"},
                new {SAPCode = "P", Description = "Potable Water Facility"},
                new {SAPCode = "Q", Description = "Water Quality"},
                new {SAPCode = "S", Description = "Customer location"},
                new {SAPCode = "W", Description = "Waste Water Facility"}
            );

            this.CreateSAPLookupTable("TechnicalObjectTypes", 10);
            Insert.IntoTable("TechnicalObjectTypes").Rows(
                //new {SAPCode = "A000001", Description = "Adjustable Speed Dri"},
                //new {SAPCode = "A000002", Description = "Aerator"},
                //new {SAPCode = "A000003", Description = "Air Compressor"},
                //new {SAPCode = "A000004", Description = "Air/ Vacuum Tank"},
                //new {SAPCode = "A000005", Description = "Arc Flash Protection"},
                //new {SAPCode = "B000001", Description = "Battery"},
                //new {SAPCode = "B000002", Description = "Battery Charger"},
                //new {SAPCode = "B000003", Description = "Blow Off Valve"},
                //new {SAPCode = "B000004", Description = "Blower"},
                //new {SAPCode = "B000005", Description = "Boiler"},
                //new {SAPCode = "B000006", Description = "Burner"},
                //new {SAPCode = "C000001", Description = "Calibration Device"},
                //new {SAPCode = "C000002", Description = "Cathodic Protection"},
                //new {SAPCode = "C000003", Description = "Chemical Treatment"},
                //new {SAPCode = "C000004", Description = "Chemical Gas Feeder"},
                //new {SAPCode = "C000005", Description = "Chemical Generators"},
                //new {SAPCode = "D000001", Description = "Dam"},
                //new {SAPCode = "D000002", Description = "Defibrillator"},
                //new {SAPCode = "D000003", Description = "Distribution System"},
                //new {SAPCode = "D000004", Description = "Distribution Tool"},
                //new {SAPCode = "D000005", Description = "Decant Station"},
                //new {SAPCode = "E000001", Description = "Electrical"},
                //new {SAPCode = "E000002", Description = "Emergency Generator"},
                //new {SAPCode = "E000003", Description = "Emergency Light"},
                //new {SAPCode = "E000004", Description = "Engine"},
                //new {SAPCode = "E000005", Description = "Eyewash"},
                //new {SAPCode = "F000001", Description = "Facility and Grounds"},
                //new {SAPCode = "F000002", Description = "Fall Protection"},
                //new {SAPCode = "F000003", Description = "Filter"},
                //new {SAPCode = "F000004", Description = "Fire Alarm"},
                //new {SAPCode = "F000005", Description = "Fire Extinguisher"},
                //new {SAPCode = "F000006", Description = "Fire Suppression"},
                //new {SAPCode = "F000007", Description = "Firewall"},
                //new {SAPCode = "F000008", Description = "Floatation Device"},
                //new {SAPCode = "F000009", Description = "Flow Meter"},
                //new {SAPCode = "F000010", Description = "Flow Weir"},
                //new {SAPCode = "F000011", Description = "Fuel Tank"},
                new {SAPCode = "FL_ADMIN", Description = "Administration"},
                new {SAPCode = "FL_BAS", Description = "Basin"},
                new {SAPCode = "FL_BSTR", Description = "Booster Station"},
                new {SAPCode = "FL_CONNOBJ", Description = "Connection Object"},
                new {SAPCode = "FL_DAM", Description = "Impounding Structure"},
                new {SAPCode = "FL_DEVLOC", Description = "Device Location"},
                new {SAPCode = "FL_DWSYS", Description = "Drinking Water Syste"},
                new {SAPCode = "FL_GAR", Description = "Garage/Storage/Shop"},
                new {SAPCode = "FL_HYDR", Description = "Hydrant Aux Vlv Rout"},
                new {SAPCode = "FL_LFST", Description = "Lift Station"},
                new {SAPCode = "FL_MAIN", Description = "Buried Linear Assets"},
                new {SAPCode = "FL_MET", Description = "Metering Point"},
                new {SAPCode = "FL_MHR", Description = "Manhole Route"},
                new {SAPCode = "FL_PWR", Description = "Power Generation"},
                new {SAPCode = "FL_RSV", Description = "SourceOfSupply Reser"},
                new {SAPCode = "FL_SENS", Description = "Sensing Point"},
                new {SAPCode = "FL_SITE", Description = "Site"},
                new {SAPCode = "FL_SRCE", Description = "Source of Supply"},
                new {SAPCode = "FL_SSLK", Description = "SourceOfSupply Lake"},
                new {SAPCode = "FL_SSM", Description = "SourceOfSupply Main"},
                new {SAPCode = "FL_TANK", Description = "Finished Water Stora"},
                new {SAPCode = "FL_TUNNEL", Description = "SourceOfSupply Tunen"},
                new {SAPCode = "FL_VLVR", Description = "Street Vlv Route"},
                new {SAPCode = "FL_WELL", Description = "Well"},
                new {SAPCode = "FL_WT", Description = "Water Treatment"},
                new {SAPCode = "FL_WWSYS", Description = "Wastewater System"},
                new {SAPCode = "FL_WWT", Description = "Waste Water Treatmen"}
                //new {SAPCode = "FLT0001", Description = "Cars"},
                //new {SAPCode = "FLT0002", Description = "Dump Truck"},
                //new {SAPCode = "FLT0003", Description = "Fork Lift"},
                //new {SAPCode = "FLT0004", Description = "Tractor"},
                //new {SAPCode = "FLT0005", Description = "BackHoe"},
                //new {SAPCode = "FLT0006", Description = "Compactor"},
                //new {SAPCode = "FLT0007", Description = "Excavator"},
                //new {SAPCode = "FLT0008", Description = "Cranes"},
                //new {SAPCode = "FLT0009", Description = "Drag Lines"},
                //new {SAPCode = "FLT0010", Description = "Boom Truck"},
                //new {SAPCode = "FLT0011", Description = "Utility Van"},
                //new {SAPCode = "FLT0012", Description = "Full-Size Pickup"},
                //new {SAPCode = "G000001", Description = "Gas Detector"},
                //new {SAPCode = "G000002", Description = "Gearbox"},
                //new {SAPCode = "G000003", Description = "Gravity Sewer Main"},
                //new {SAPCode = "G000004", Description = "Grinder"},
                //new {SAPCode = "G000005", Description = "Grinder Pump"},
                //new {SAPCode = "G000006", Description = "Gates"},
                //new {SAPCode = "H000001", Description = "Heat Exchanger"},
                //new {SAPCode = "H000002", Description = "Hoist"},
                //new {SAPCode = "H000003", Description = "HVAC Chiller"},
                //new {SAPCode = "H000004", Description = "HVAC Combination Uni"},
                //new {SAPCode = "H000005", Description = "HVAC Dehumidifier"},
                //new {SAPCode = "H000006", Description = "HVAC Heater"},
                //new {SAPCode = "H000007", Description = "HVAC Ventilator"},
                //new {SAPCode = "H000008", Description = "Hydrant"},
                //new {SAPCode = "H000009", Description = "Hydrant Lateral"},
                //new {SAPCode = "I0000001", Description = "Indicator"},
                //new {SAPCode = "I0000002", Description = "Instruments"},
                //new {SAPCode = "K000001", Description = "Kit (safety, repair,"},
                //new {SAPCode = "L000001", Description = "Lab Equipment"},
                //new {SAPCode = "M000001", Description = "Manhole"},
                //new {SAPCode = "M000002", Description = "Mixer"},
                //new {SAPCode = "M000003", Description = "Modem"},
                //new {SAPCode = "M000004", Description = "Motor"},
                //new {SAPCode = "M000005", Description = "Motor Contactor"},
                //new {SAPCode = "M000006", Description = "Motor Starter"},
                //new {SAPCode = "N000001", Description = "Network Router"},
                //new {SAPCode = "N000002", Description = "Network Switch"},
                //new {SAPCode = "N000003", Description = "Non Potable Water Ta"},
                //new {SAPCode = "O000001", Description = "Ozone Contactors"},
                //new {SAPCode = "O000002", Description = "Operator Computer Te"},
                //new {SAPCode = "P000001", Description = "PC"},
                //new {SAPCode = "P000002", Description = "PDM Tool"},
                //new {SAPCode = "P000003", Description = "Phase Converter"},
                //new {SAPCode = "P000004", Description = "Plant Valve"},
                //new {SAPCode = "P000005", Description = "Potable Water Tank"},
                //new {SAPCode = "P000006", Description = "Power Breaker"},
                //new {SAPCode = "P000007", Description = "Power Conditioner"},
                //new {SAPCode = "P000008", Description = "Power Disconnect"},
                //new {SAPCode = "P000009", Description = "Power Feeder Cable"},
                //new {SAPCode = "P000010", Description = "Power Monitor"},
                //new {SAPCode = "P000011", Description = "Power Panel"},
                //new {SAPCode = "P000012", Description = "Power Relay"},
                //new {SAPCode = "P000013", Description = "Power Surge Protecti"},
                //new {SAPCode = "P000014", Description = "Power Transfer Switc"},
                //new {SAPCode = "P000015", Description = "Pressure Damper"},
                //new {SAPCode = "P000016", Description = "Pipe"},
                //new {SAPCode = "P000017", Description = "Pumps"},
                //new {SAPCode = "P000018", Description = "Pump Grinder"},
                //new {SAPCode = "P000019", Description = "Pump Positive Displa"},
                //new {SAPCode = "R000001", Description = "Reservoir"},
                //new {SAPCode = "R000002", Description = "Respirator"},
                //new {SAPCode = "R000003", Description = "RTU - PLC"},
                //new {SAPCode = "S000001", Description = "Gravity Main Sewer"},
                //new {SAPCode = "S000002", Description = "Force Main Sewer"},
                //new {SAPCode = "S000003", Description = "SCADA Radio"},
                //new {SAPCode = "S000004", Description = "SCADA System Gen"},
                //new {SAPCode = "S000005", Description = "Superpulsators"},
                //new {SAPCode = "S000006", Description = "Screen"},
                //new {SAPCode = "S000007", Description = "Scrubber"},
                //new {SAPCode = "S000008", Description = "Secondary Containmen"},
                //new {SAPCode = "S000009", Description = "Security System"},
                //new {SAPCode = "S000010", Description = "Safety Shower"},
                //new {SAPCode = "S000011", Description = "Service Lateral"},
                //new {SAPCode = "S000012", Description = "Sewer Lateral"},
                //new {SAPCode = "S000013", Description = "Street Valve"},
                //new {SAPCode = "T000001", Description = "Thickners"},
                //new {SAPCode = "T000002", Description = "Tanks"},
                //new {SAPCode = "T000003", Description = "Transformer"},
                //new {SAPCode = "T000004", Description = "Transmitter"},
                //new {SAPCode = "T000005", Description = "Trash Rack"},
                //new {SAPCode = "T000006", Description = "Telecommunication"},
                //new {SAPCode = "U000001", Description = "Uninterupted Power S"},
                //new {SAPCode = "U000002", Description = "UV Sanitizer"},
                //new {SAPCode = "V000001", Description = "Valve"},
                //new {SAPCode = "V000002", Description = "VOC Stripper"},
                //new {SAPCode = "W000001", Description = "Water Main"},
                //new {SAPCode = "W000002", Description = "Water Heater"},
                //new {SAPCode = "W000003", Description = "Water Quality Analyz"},
                //new {SAPCode = "W000004", Description = "Water Treatment Cont"},
                //new {SAPCode = "W000005", Description = "Water Well"},
                //new {SAPCode = "W000006", Description = "Waste Tank"}
            );
            this.CreateLookupTableWithValues("FunctionalLocationClasses",
                "FLOC_LVL_1", "MAINEXTINFO", "WORKMANAGEMENT", "READINGDEVICE", "FLOC_LVL_4P", "FLOC_LVL_4D",
                "FLOC_LVL_3P", "TAPINFORMATION", "SYSDEL_W", "SYSDEL_P", "FLOC_LVL_3D", "ELECGEAR", "DMAZONE",
                "SHAREDSVCINFO", "CLASSIFYDEVLOC", "FLOC_LVL_2", "PROPERTYLOCATOR", "PREMISESERVICECHCK",
                "ZEROUSAGECHECK", "WORKZONE", "HEATTYPE", "FLOC_LVL_6", "FLOC_LVL_5P", "NSIINFORMATION", "NARUC_FL",
                "FLOC_LVL_5D", "MUNICIPALITYCODE", "ZLEGACYPREMISE", "WMAIN", "GMAIN", "FMAIN", "SLATERAL", "WTR_TYPE",
                "WWT_TYPE", "FACILITYRELIABILTY");
        }

        public override void Down()
        {
            Delete.Column("SAPErrorCode").FromTable("Equipment");
            Delete.Table("FunctionalLocationClasses");
            Delete.Table("TechnicalObjectTypes");
            Delete.Table("FunctionalLocationCategories");

            Rename.Column("FunctionalLocationId").OnTable("Equipment").To("FunctionalLocation");
            Alter.Table("Equipment")
                 .AddForeignKeyColumn("FunctionalLocationId", "FunctionalLocations", "FunctionalLocationId");
            Execute.Sql(
                "UPDATE Equipment SET FunctionalLocationId = (select fl.functionallocationId from functionalLocations fl where fl.Description = E.FunctionalLocation and fl.TownID = F.TownID AND AssetTypeID = 9) FROM Equipment E Join tblFacilities f on f.RecordId = E.FacilityID");
            Delete.Column("FunctionalLocation").FromTable("Equipment");
        }
    }
}
