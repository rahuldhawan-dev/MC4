using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211011142730650), Tags("Production")]
    public class MC2979AddNewFacilitiesInspectionQuestionCategoriesTable : Migration
    {
        public struct Questions
        {
            public const string
                SITE_ORIENTATION_MATERIALS = "Site orientation materials for visitors are available and current",
                AREA_WITH_FREE_OF_DEBRIS_OR_WEED = "Areas where people will walk or work are free of weeds/debris",
                LADDLER_GUARDRAILS_HANDRAILS_CONDITION = "Ladders/Guardrails/Handrails in good condition",
                ACCESS_EGRESS_OBSTRUCTIONS = "Access/Egress points clear of obstructions",
                WALKING_WORKING_AREAS = "All walking-working surfaces such as, stairs, aisles, and walkways free of obstructions or slip, trip, fall hazards",
                EXTERIOR_FACILITY = "Exterior of facility generally clean and orderly",
                INTERIOR_FACILITY = "Interior of facility generally clean and orderly",
                AREA_OF_WORK = "Areas where people will walk or work are adequately illuminate to safely operate after dark",
                FLOOR_GUARDED = "All floor holes, ladder access and wall openings are guarded or barricaded to prevent a fall",
                SAFETY_EQUIPMENT = "Safety equipment such as, safety showers, eywashes, emergency stops, fire alarm, fire extinguishers, and medical supplies are unobstructed",
                WASTE_MANAGEMENT = "Waste receptacles clean, adequately segregated, and not overflowing",
                COMPANY_MATERIALS = "Only company materials and equipment are stored in the facility",
                GENERAL_HOUSEKEEPING = "General housekeeping in overall good condition",
                FIRST_AID_KIT = "First Aid kit on-site and stocked",
                SHOWER_EYEWASH_STATIONS = "Shower/Eyewash stations accessible and inspected monthly",
                EMERGENCY_EVACUATION = "Emergency evacuation routes posted",
                EXITS_MARKED = "Exits clearly marked",
                AED_INSPECTION = "AEDs operational and inspected monthly",
                EMERGENCY_PLAN = "Emergency Response Plan on-site with up-to-date information",
                EMERGENCY_RESPONSE = "Emergency Response Plan is uploaded into MapCall and current",
                VISITOR_ACCESS = "Visitor access control in place (sign in, site briefing, who's on site)",
                PERIMETER_PROTECTION = "Perimeter of site barricaded or otherwise protected",
                FENCING_GATES = "Fencing/Gates in acceptable condition",
                TRESPASSING = "No Trespassing sign(s) posted on outside of structure",
                FACILITY_SECURED = "All doors, hatches, opening, etc. properly secured",
                FACILITY_MONITORED = "Facility/Equipment monitored via SCADA",
                SECURITY_SYSTEM = "Security system operational",
                FIRE_ALARM = "Fire alarm system certified and tested annually",
                FIRE_EXISTS = "Fire exits clearly labeled, illuminated and unobstructed from debris/clutter",
                FACILITY_UTILIZING = "Facility is utilizing a Red Tag system to identify out of service fire protection equipment",
                SPRINKLER_SYSTEM = "Sprinkler systems are not obstructed and are maintained and inspected",
                FLAMMABLE_MATERIALS_STORED = "Flammable materials stored in flammable storage cabinet",
                FLAMMABLE_STORAGE = "Flammable storage cabinets properly maintained",
                NO_COMBUSTIBLE = "No combustable or flammable materials stored near electrical  breakers",
                STOCKPILING = "Stockpiling of combustible materials prevented",
                FIRE_EXTINGUISHER_ACCESSIBLE = "Fire extinguishers accessible and appropriate class for present hazards",
                FIRE_EXTINGUISHER_LOCATIONS = "Fire extiniguisher locations visibly marked and unobstructed",
                MONTHLY_FIRE_EXTINGUISHER = "Monthly fire extinguisher inspections documented",
                DESIGNATED_HOT_WORK = "Designated Hot Work areas are labeled and free of combustibles and flammables",
                DEMONSTRATED_CONSISTENT = "Demonstrated consistent use of Hot Work Permit in MapCall and adherance to process",
                SIGNS_REQUIRED = "Signs with required PPE posted",
                SUPPLY_REQUIRED = "Supply of required PPE stocked",
                PPE_STORED = "PPE stored properly",
                PPE_CLEANING = "PPE cleaning supplies available where shared PPE is used",
                DOCUMENTATION_PPE_INSPECTIONS = "Documentation of PPE inspections available for high voltage gloves, body harnesses, and non-disposable respirators",
                CHEMICAL_MATERIALS = "Chemicals/materials properly stored and maintained",
                CHEMICAL_CONTAINERS = "Chemicals/materials properly stored and maintained",
                SDS_ACCESSIBLE = "SDSs accessible and up-to-date",
                APPROPRIATE_CHEMICAL_HAZARD = "Appropriate chemical hazard warning signs posted",
                EMERGENCY_SPILL = "Emergency spill/repair kits readily accessible and equipped for the appropriate volume",
                AREA_CLEAN = "Area clean and free of chemical spills",
                INCOMPATIBLE_MATERIALS = "Incompatible materials stored separately",
                CHEMICAL_CONTAINERS_CLOSED = "Chemical containers closed and secured when not in use",
                CHEMICAL_CONTAINERS_GOOD = "Chemical containers closed and secured when not in use",
                COMPRESSED_GAS_CYLINDERS = "Compressed gas cylinders secured and stored properly",
                SECONDARY_CONTAINMENT = "Secondary containment in good condition",
                CHEMICAL_WASTE = "Chemical waste is labeled and dated, segregated, inspected, and disposed of in accordance with state regulations",
                PROPER_DOCUMENTATION = "Proper documentation of disposed material",
                CHEMICAL_FILL_LINES = "Chemical fill lines locked when not in use",
                CONFIRMATION_CHEMICALS = "Confirmation that chemicals are recieved in accordance with practice",
                ALARM_ASSET_RECORDS = "Alarm asset records (CL2, O3, O2, etc.) reviewed in MapCall with demonstrated adherance to PM schedule and are in proper working order",
                HAND_TOOLS = "Hand tools in good condition",
                EQUIPMENT_TOOLS = "Equipemt and tools are from dirt, visible leaks, and working properly",
                NO_BROKEN_DAMAGED = "No broken, damaged, or worn tools present",
                ELECTRICAL_CABINETS = "Electrical cabinets/equipment in good condition and labeled",
                CORDS_PROPER_TYPE = "Extension cords of the proper type and size used and in good conditon",
                CORDS_ELEVATED = "Extension cords are elevated where possible",
                CORDS_NOT_RUN_CEILINGS = "Extension cords are not run through ceilings, walls, or doorways",
                CORDS_CROSS_WALKWAYS = "Extension cords which cross walkways are secured",
                CORDS_CROSS_VEHICLE = "Extension cords which cross vehicle paths are shielded",
                GFCI_ADAPTERS = "GFCI adapters, pigtails or power strips are in use where GFCI circuit breakers are not installed",
                VOLTAGE_POSTED = "Voltage posted on electrical components",
                LIVE_ELECTRICAL_COMPONENTS = "Live electrical components properly guarded",
                MOVING_ROTATING_PARTS = "All moving/rotating parts properly guarded and any manufacturer guards reamin in place",
                EQUIPMENTS_SECURELY_PLACED = "Equipment/machinery securely placed/anchored",
                MANUFACTURES_MANUAL = "Manufacturer's manuals available on site for powered equipment",
                ARC_FLASH_LABELING = "ARC flash labeling in place and current",
                EQUIPMENT_STORED = "Equipment stored further than 3 ft from electrical panels",
                PERMIT_REQUIRED = "All permit-required confined spaces physically labeled and identified on the site inventory list",
                CONFINED_SPACE = "All confined space air monitoring equipment is inventoried in MapCall and calibration is up to date",
                EXHAUST_FAN = "Exhaust fan functioning properly",
                ACCESS_PERMIT_REQUIRED = "Access to all permit required confined spaces are guarded",
                EMERGENCY_RETRIEVAL = "Emergency retrieval equipment in good condition",
                RESCUE_SERVICES = "Rescue services identified",
                DEMONSTRATED_CONSISTENT_CONFINED_SPACE = "Demonstrated consistent use of Confined Space Form in MapCall and adherance to process",
                VEHICLES_CLEAN = "Vehicles clean and well organized",
                ANNUAL_DOT = "Annual DOT inspection current",
                DAILY_INSPECTIONS = "Daily inspections completed and recorded",
                SAFETY_FEATURES_FUNCTIONING = "All safety features functioning (backup alarms, lights, brakes, etc)",
                APPROPRIATE_TRAFFIC_CONTROL = "Appropriate traffic control equipment available",
                EMPLOYEE_ON_SITE = "Employee on-site were current with all required OSHA training (cross-check with LEARN)";
        }

        public override void Up()
        {
            this.CreateLookupTableWithValues("FacilityInspectionFormQuestionCategories", "GENERAL WORK AREA/CONDITIONS", "EMERGENCY RESPONSE/FIRST AID",
                "SECURITY", "FIRE SAFETY", "PERSONAL PROTECTIVE EQUIPMENT", "CHEMICAL STORAGE/HAZ COM", "EQUIPMENT/TOOLS", "CONFINED SPACE", "VEHICLE/MOTORIZED EQUIPMENT", "OSHA TRAINING");

            Create.Table("FacilityInspectionFormQuestions")
                  .WithIdentityColumn()
                  .WithColumn("Weightage").AsInt32()
                  .WithForeignKeyColumn("CategoryId", "FacilityInspectionFormQuestionCategories")
                  .WithColumn("Question").AsCustom("text").NotNullable()
                  .WithColumn("DisplayOrder").AsInt32().NotNullable();

            // Populate Questions
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.GENERAL_WORK_AREA_CONDITIONS,
                    DisplayOrder = 1,
                    Question = Questions.SITE_ORIENTATION_MATERIALS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.GENERAL_WORK_AREA_CONDITIONS,
                    DisplayOrder = 2,
                    Question = Questions.AREA_WITH_FREE_OF_DEBRIS_OR_WEED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.GENERAL_WORK_AREA_CONDITIONS,
                    DisplayOrder = 3,
                    Question = Questions.LADDLER_GUARDRAILS_HANDRAILS_CONDITION
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.GENERAL_WORK_AREA_CONDITIONS,
                    DisplayOrder = 4,
                    Question = Questions.ACCESS_EGRESS_OBSTRUCTIONS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.GENERAL_WORK_AREA_CONDITIONS,
                    DisplayOrder = 5,
                    Question = Questions.WALKING_WORKING_AREAS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.GENERAL_WORK_AREA_CONDITIONS,
                    DisplayOrder = 6,
                    Question = Questions.EXTERIOR_FACILITY
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.GENERAL_WORK_AREA_CONDITIONS,
                    DisplayOrder = 7,
                    Question = Questions.INTERIOR_FACILITY
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.GENERAL_WORK_AREA_CONDITIONS,
                    DisplayOrder = 8,
                    Question = Questions.AREA_OF_WORK
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.GENERAL_WORK_AREA_CONDITIONS,
                    DisplayOrder = 9,
                    Question = Questions.FLOOR_GUARDED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.GENERAL_WORK_AREA_CONDITIONS,
                    DisplayOrder = 10,
                    Question = Questions.SAFETY_EQUIPMENT
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.GENERAL_WORK_AREA_CONDITIONS,
                    DisplayOrder = 11,
                    Question = Questions.WASTE_MANAGEMENT
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.GENERAL_WORK_AREA_CONDITIONS,
                    DisplayOrder = 12,
                    Question = Questions.COMPANY_MATERIALS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.GENERAL_WORK_AREA_CONDITIONS,
                    DisplayOrder = 13,
                    Question = Questions.GENERAL_HOUSEKEEPING
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EMERGENCY_RESPONSE_FIRST_AID,
                    DisplayOrder = 14,
                    Question = Questions.FIRST_AID_KIT
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EMERGENCY_RESPONSE_FIRST_AID,
                    DisplayOrder = 15,
                    Question = Questions.SHOWER_EYEWASH_STATIONS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EMERGENCY_RESPONSE_FIRST_AID,
                    DisplayOrder = 16,
                    Question = Questions.EMERGENCY_EVACUATION
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EMERGENCY_RESPONSE_FIRST_AID,
                    DisplayOrder = 17,
                    Question = Questions.EXITS_MARKED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EMERGENCY_RESPONSE_FIRST_AID,
                    DisplayOrder = 18,
                    Question = Questions.AED_INSPECTION
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EMERGENCY_RESPONSE_FIRST_AID,
                    DisplayOrder = 19,
                    Question = Questions.EMERGENCY_PLAN
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EMERGENCY_RESPONSE_FIRST_AID,
                    DisplayOrder = 20,
                    Question = Questions.EMERGENCY_RESPONSE
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.SECURITY,
                    DisplayOrder = 21,
                    Question = Questions.VISITOR_ACCESS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.SECURITY,
                    DisplayOrder = 22,
                    Question = Questions.PERIMETER_PROTECTION
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.SECURITY,
                    DisplayOrder = 23,
                    Question = Questions.FENCING_GATES
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.SECURITY,
                    DisplayOrder = 24,
                    Question = Questions.TRESPASSING
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.SECURITY,
                    DisplayOrder = 25,
                    Question = Questions.FACILITY_SECURED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.SECURITY,
                    DisplayOrder = 26,
                    Question = Questions.FACILITY_MONITORED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.SECURITY,
                    DisplayOrder = 27,
                    Question = Questions.SECURITY_SYSTEM
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.FIRE_SAFETY,
                    DisplayOrder = 28,
                    Question = Questions.FIRE_ALARM
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.FIRE_SAFETY,
                    DisplayOrder = 29,
                    Question = Questions.FIRE_EXISTS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.FIRE_SAFETY,
                    DisplayOrder = 30,
                    Question = Questions.FACILITY_UTILIZING
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.FIRE_SAFETY,
                    DisplayOrder = 31,
                    Question = Questions.SPRINKLER_SYSTEM
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.FIRE_SAFETY,
                    DisplayOrder = 32,
                    Question = Questions.FLAMMABLE_MATERIALS_STORED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.FIRE_SAFETY,
                    DisplayOrder = 33,
                    Question = Questions.FLAMMABLE_STORAGE
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.FIRE_SAFETY,
                    DisplayOrder = 34,
                    Question = Questions.NO_COMBUSTIBLE
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.FIRE_SAFETY,
                    DisplayOrder = 35,
                    Question = Questions.STOCKPILING
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.FIRE_SAFETY,
                    DisplayOrder = 36,
                    Question = Questions.FIRE_EXTINGUISHER_ACCESSIBLE
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.FIRE_SAFETY,
                    DisplayOrder = 37,
                    Question = Questions.FIRE_EXTINGUISHER_LOCATIONS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.FIRE_SAFETY,
                    DisplayOrder = 38,
                    Question = Questions.MONTHLY_FIRE_EXTINGUISHER
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.FIRE_SAFETY,
                    DisplayOrder = 39,
                    Question = Questions.DESIGNATED_HOT_WORK
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.FIRE_SAFETY,
                    DisplayOrder = 40,
                    Question = Questions.DEMONSTRATED_CONSISTENT
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.PERSONAL_PROTECTIVE_EQUIPMENT,
                    DisplayOrder = 41,
                    Question = Questions.SIGNS_REQUIRED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.PERSONAL_PROTECTIVE_EQUIPMENT,
                    DisplayOrder = 42,
                    Question = Questions.SUPPLY_REQUIRED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.PERSONAL_PROTECTIVE_EQUIPMENT,
                    DisplayOrder = 43,
                    Question = Questions.PPE_STORED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.PERSONAL_PROTECTIVE_EQUIPMENT,
                    DisplayOrder = 44,
                    Question = Questions.PPE_CLEANING
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.PERSONAL_PROTECTIVE_EQUIPMENT,
                    DisplayOrder = 45,
                    Question = Questions.DOCUMENTATION_PPE_INSPECTIONS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 46,
                    Question = Questions.CHEMICAL_MATERIALS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 47,
                    Question = Questions.CHEMICAL_CONTAINERS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 48,
                    Question = Questions.SDS_ACCESSIBLE
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 49,
                    Question = Questions.APPROPRIATE_CHEMICAL_HAZARD
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 50,
                    Question = Questions.EMERGENCY_SPILL
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 51,
                    Question = Questions.AREA_CLEAN
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 52,
                    Question = Questions.INCOMPATIBLE_MATERIALS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 53,
                    Question = Questions.CHEMICAL_CONTAINERS_CLOSED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 54,
                    Question = Questions.CHEMICAL_CONTAINERS_GOOD
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 55,
                    Question = Questions.COMPRESSED_GAS_CYLINDERS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 56,
                    Question = Questions.SECONDARY_CONTAINMENT
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 57,
                    Question = Questions.CHEMICAL_WASTE
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 58,
                    Question = Questions.PROPER_DOCUMENTATION
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 59,
                    Question = Questions.CHEMICAL_FILL_LINES
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 60,
                    Question = Questions.CONFIRMATION_CHEMICALS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CHEMICAL_STORAGE_HAZ_COM,
                    DisplayOrder = 61,
                    Question = Questions.ALARM_ASSET_RECORDS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 62,
                    Question = Questions.HAND_TOOLS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 63,
                    Question = Questions.EQUIPMENT_TOOLS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 64,
                    Question = Questions.NO_BROKEN_DAMAGED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 65,
                    Question = Questions.ELECTRICAL_CABINETS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 66,
                    Question = Questions.CORDS_PROPER_TYPE
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 67,
                    Question = Questions.CORDS_ELEVATED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 68,
                    Question = Questions.CORDS_NOT_RUN_CEILINGS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 69,
                    Question = Questions.CORDS_CROSS_WALKWAYS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 70,
                    Question = Questions.CORDS_CROSS_VEHICLE
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 71,
                    Question = Questions.GFCI_ADAPTERS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 72,
                    Question = Questions.VOLTAGE_POSTED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 73,
                    Question = Questions.LIVE_ELECTRICAL_COMPONENTS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 74,
                    Question = Questions.MOVING_ROTATING_PARTS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 75,
                    Question = Questions.EQUIPMENTS_SECURELY_PLACED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 76,
                    Question = Questions.MANUFACTURES_MANUAL
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 77,
                    Question = Questions.ARC_FLASH_LABELING
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.EQUIPMENT_TOOLS,
                    DisplayOrder = 78,
                    Question = Questions.EQUIPMENT_STORED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CONFINED_SPACE,
                    DisplayOrder = 79,
                    Question = Questions.PERMIT_REQUIRED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CONFINED_SPACE,
                    DisplayOrder = 80,
                    Question = Questions.CONFINED_SPACE
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CONFINED_SPACE,
                    DisplayOrder = 81,
                    Question = Questions.EXHAUST_FAN
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CONFINED_SPACE,
                    DisplayOrder = 82,
                    Question = Questions.ACCESS_PERMIT_REQUIRED
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CONFINED_SPACE,
                    DisplayOrder = 83,
                    Question = Questions.EMERGENCY_RETRIEVAL
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CONFINED_SPACE,
                    DisplayOrder = 84,
                    Question = Questions.RESCUE_SERVICES
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.CONFINED_SPACE,
                    DisplayOrder = 85,
                    Question = Questions.DEMONSTRATED_CONSISTENT_CONFINED_SPACE
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.VEHICLE_MOTORIZED_EQUIPMENT,
                    DisplayOrder = 86,
                    Question = Questions.VEHICLES_CLEAN
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.VEHICLE_MOTORIZED_EQUIPMENT,
                    DisplayOrder = 87,
                    Question = Questions.ANNUAL_DOT
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 1,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.VEHICLE_MOTORIZED_EQUIPMENT,
                    DisplayOrder = 88,
                    Question = Questions.DAILY_INSPECTIONS
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.VEHICLE_MOTORIZED_EQUIPMENT,
                    DisplayOrder = 89,
                    Question = Questions.SAFETY_FEATURES_FUNCTIONING
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.VEHICLE_MOTORIZED_EQUIPMENT,
                    DisplayOrder = 90,
                    Question = Questions.APPROPRIATE_TRAFFIC_CONTROL
                });
            Insert.IntoTable("FacilityInspectionFormQuestions").Rows(
                new {
                    Weightage = 3,
                    CategoryId = FacilityInspectionFormQuestionCategory.Indices.OSHA_TRAINING,
                    DisplayOrder = 91,
                    Question = Questions.EMPLOYEE_ON_SITE
                });

            Create.Table("FacilityInspectionFormAnswers")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ApcInspectionItemId", "APCInspectionItems")
                  .WithForeignKeyColumn("FacilityInspectionFormQuestionId", "FacilityInspectionFormQuestions")
                  .WithColumn("IsSafe").AsBoolean().Nullable()
                  .WithColumn("Comments").AsAnsiString(255).Nullable()
                  .WithColumn("IsPictureTaken").AsBoolean().WithDefaultValue(false).Nullable();
        }

        public override void Down()
        {
            Delete.Table("FacilityInspectionFormAnswers");
            Delete.Table("FacilityInspectionFormQuestions");
            Delete.Table("FacilityInspectionFormQuestionCategories");
        }

        private static class FacilityInspectionFormQuestionCategory
        {
            public struct Indices
            {
                public const int GENERAL_WORK_AREA_CONDITIONS = 1,
                                 EMERGENCY_RESPONSE_FIRST_AID = 2,
                                 SECURITY = 3,
                                 FIRE_SAFETY = 4,
                                 PERSONAL_PROTECTIVE_EQUIPMENT = 5,
                                 CHEMICAL_STORAGE_HAZ_COM = 6,
                                 EQUIPMENT_TOOLS = 7,
                                 CONFINED_SPACE = 8,
                                 VEHICLE_MOTORIZED_EQUIPMENT = 9,
                                 OSHA_TRAINING = 10;
            }
        }
    }
}

