﻿using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200204141708887), Tags("Production")]
    public class MC1848BuildNewTaskGroupTableAndInsertValues : Migration
    {
        private string InsertSql =
            "INSERT INTO TaskGroups(TaskGroupId, Description)\r\nVALUES('CH01', 'PM HYPO GEN CELL CONNECT & CABLE CLEAN'),\r\n('CH02','PM HYPO GEN INFRARED TEMP RECORDING'),\r\n('CH03','PM CHEM FLEX WHIPS INSP & REPLACE'),\r\n('CH04','PM CL2 GAS SYS SAFETY CHECKS'),\r\n('CH05','PM CL2 GAS STRAINER CLEANING'),\r\n('CH06','PM CHLORTEC SODIUMHYPO GEN 22A'),\r\n('CH07','PM SODIUM HYPO GEN INSP 22'),\r\n('CH08','PM CHLORTEC SODIUMHYPO GEN KEY COMP 22B'),\r\n('CH09','PM CHLORTEC SODIUMHYPO GEN FULL INSP 22C'),\r\n('CH10','PM TOXIC GAS SYSTEM INSPECTION'),\r\n('CH11','PM WEEKLY GAS CL2 SYSTEM VISUAL'),\r\n('CH12','PM ANNUAL GAS CL2 SYSTEM OVERHAUL'),\r\n('CH13','PM POLYMER FEEDER MAINTENANCE'),\r\n('CH14','PM LIQUID CHEMICAL FEED'),\r\n('CH15','PM CHEMICAL PUMP AND TANK INSPECTION'),\r\n('CH16','PM SECONDARY CONTAINMENT INSPECTION'),\r\n('CH17','PM CL2 B KIT COMPONENT REPLACE'),\r\n('CH18','PM CL2 B KIT INSPECTION'),\r\n('CH19','PM LIQUID SULFIDE ANALYSIS'),\r\n('CH20','PM AIR SULFIDE MONITORING'),\r\n('CH21','PM PROGRESSIVE CAVITY PUMP INSPECTION'),\r\n('CH22','PM ELECTRONIC SCALE LOAD CELL INSPECTION'),\r\n('CH23','PM BRINE TANK CLEANING'),\r\n('CH24','PM CHEMICAL TANK INTEGRITY TESTING'),\r\n('CH25','PM SLURRY TANK PIPE CLEANING'),\r\n('CH26','PM FUEL TANK DRAW OFF DIESEL TANK WATER'),\r\n('CH27','PM CL2 A KIT 100 AND 150 LB CYLINDER INSPECTION'),\r\n('CH28','PM CHEMICAL TANK INSPECTION'),\r\n('CH29','PM CL2 EMERGENCY KIT C INSPECTION'),\r\n('CH30','PMI CL2 CYLINDER RECOVERY VESSEL INSP'),\r\n('E01','PD BATTERY/CHARGER INSP'),\r\n('E02','PD IR ELECTRO-ROT GEAR'),\r\n('E03','PD MOTOR (CRITICAL) WINDING ANALYSIS'),\r\n('E04','PD TRANSFORMER OIL ANALYSIS'),\r\n('E05','PD DRY TRANSFORMER MAINT'),\r\n('E06','PM MONTHLY CATHODIC PROT 43'),\r\n('E07','PM ANNUAL CATHODIC PROT 43A'),\r\n('E08','PM GENERAL ELECTRICAL GEAR INSP'),\r\n('E09','PM MOTOR FULL INSP (ANNUAL OR AS REQ)'),\r\n('E10','PM KEY ELECTRIC MOTOR OVERHAUL'),\r\n('E11','PM MOTOR STARTER MAINT'),\r\n('E12','PM PROTECTIVE RELAY INSPS'),\r\n('E13','PM SURGE ARRESTOR INSP'),\r\n('E14','PM UPS UNINTERRUPTIBLE POWER INSP'),\r\n('E15','PM PARTICLE COUNTERS INSP'),\r\n('H01','PM HYD (WET) INSP'),\r\n('H02','PM HYD (DRY) INSP'),\r\n('H04','PM HYD UNI-DIRECTIONAL FLUSHING'),\r\n('H05','PM HYDRANT PAINTING'),\r\n('H06','PM HYD (DRY) WINTERIZE NON-DRAIN'),\r\n('I01','PM SCADA RADIO INSP'),\r\n('I02','PM VENTURI FLUSH'),\r\n('I03','PM LARGE FLOWMETER MAINT'),\r\n('I04','PM WEATHER SYSTEM READING'),\r\n('I05','PM PC/SCADA HMI COMPUTER INSP'),\r\n('I06','PM RTU BATTERY REPLACE INSP'),\r\n('I07','PM ALL SCADA CABINET INSP'),\r\n('I08','PM AMMONIA PROBE'),\r\n('I09','PM WQANLZR DO PROBE PM'),\r\n('I10','PM WQ ANALYZERS'),\r\n('I11','PM TURBIDIMET LAMP REPLACE'),\r\n('I12','PM CL2 TUBING REPL HACH CL-17'),\r\n('I12A','PM FLOW TUBE CLEANING'),\r\n('I13','PM CL2 CLEANING HACH CL-17'),\r\n('I14','PM CL2 REAGENT REPL HACH CL-17'),\r\n('I15','PM CONTINUOUS CLEAN/CALIB FLUORIDE'),\r\n('I16','PM PH ANALYZER CLEAN/CALIB FOXBORO873'),\r\n('I17','PM PH ANALYZER CLEAN/CALIB FOXBORO873'),\r\n('I18','PM CL2 ANALYZER CALIB CL2 SENSOR PROBE'),\r\n('I19','PM CLEAN/CALIB TURBIDIMET ANALYZER'),\r\n('I20','PM GEN INSTRUMENT CALIBRATION'),\r\n('I21','PM BASIN CLARIFIER/SLUDGE LEVEL DETECTOR'),\r\n('I22','PM ALUM ROTOMETER CLEANING'),\r\n('I23','PM HIVOLT DETECTOR INSP'),\r\n('I29','PM CLEAN/CALIB TURBIDIMET MODEL 8220'),\r\n('I30','PM FILT EXPANSION PROBE CLEANING'),\r\n('M01','PD WIRE TO WATER EFFICIENCY'),\r\n('M02','PD LUBRICATING OIL ANALYSIS'),\r\n('M03','PD VIBR ANALYSIS CRITICAL ROTATING EQUIP'),\r\n('M04','PM ENV BLWR/STRIPPER/SCRUBBER INSP'),\r\n('M05','PM BOILER INSPECTION'),\r\n('M06','PM AIR COMPRESSOR & VACUUM SYST'),\r\n('M07','PM DAM INSPECTION'),\r\n('M08','PM GENERATOR FULL INSP ANNUAL'),\r\n('M09','PM GAS AND DIESEL ENGINES CHECK'),\r\n('M10','PM ENGINE CONSUMABLE REPLACEMENT GEN'),\r\n('M11','PM GAS TURBINE GENERATOR MAINTENANCE'),\r\n('M12','PM GENERATOR EXERCISING'),\r\n('M13','PM GEARBOX INSPECTION'),\r\n('M14','PM ALTERNATE BACKUP GEAR'),\r\n('M15','PM HOIST MAINT'),\r\n('M16','PM HVAC EQUIPMENT INSP'),\r\n('M16A','PM HVAC HEATER INSP'),\r\n('M17','PM BLOWDOWN HOT WATER HEATERS'),\r\n('M18','PM RAPID MIXER INSPECTION'),\r\n('M19','PM SYNCHRONOUS ELECTRIC MOT RING STONING'),\r\n('M20','PM CENTRIFUGAL PUMP INSP'),\r\n('M21','PM CENT SUBMERSIBLE PUMP INSEPECTION'),\r\n('M22','PM CENT WELL PUMP INSP'),\r\n('M23','PM CENT KEY PUMP OVERHAUL'),\r\n('M24','PM DIAPHRAGM CHEMICAL PUMP PM'),\r\n('M25','PM PRESSURE REG ALTITUDE VLV INSP'),\r\n('M26','PM PVLV BACKFLOW BACKFLOW PREV INSP'),\r\n('M27','PM PVLV BACKFLOW TESTING DEVICE CALIB'),\r\n('M28','PM PVLV PRIMARY SLUDGE VLV INSP'),\r\n('M29','PM PLANT/LIFT STATION VALVE'),\r\n('M30','PM ROTATING/ALL GENERAL LUBRICATION'),\r\n('M31','PM MECHANICAL BAR SCREEN'),\r\n('M32','PM LAWN SPRINKLER'),\r\n('M33','PM TANK & CLEARWELL'),\r\n('M34','PM PORT WATER STOR FOR OUTAGE TNK MAINT'),\r\n('M35','PM TNK WSTE SLUDGE TANK CLEANING'),\r\n('M36','PM PRIMARY CLARIFIER CLEANING'),\r\n('M37','PM WEEKLY FLOCCULATOR DRIVE INSP'),\r\n('M38','PM MONTHLY FLOCCULATOR DRIVE INSP'),\r\n('M40','PM FILTER BED INSP'),\r\n('M41','PM FILTER UNDERDRAIN INSP'),\r\n('M42','PM 39 UV SYSTEM INSPECTION'),\r\n('M43','PM 39A UV SYSTEM INSPECTION'),\r\n('M44','PM CLARIFIER INTERNAL INSP & CLEANING'),\r\n('M45','PM VACTOR TRUCK INSPECTION'),\r\n('M46','PM MOBILE EQUIP READINESS CHECK'),\r\n('M47','PM W-D WELL ALARM MAINT'),\r\n('M48','PM WELL STATIC LEVELS'),\r\n('M49','PM WET WELL PM CLEANING'),\r\n('M50','PM LIME SLAKER CLEANING'),\r\n('M51','PM GAS ENGINE EXHAUST TESTING'),\r\n('M52','PM RUN TIME HOURS'),\r\n('M53','PM AIR HANDLERS MAINT'),\r\n('M54','PM TELESCOPIC VALVE MAINT'),\r\n('M55','PM BASKET STRAINER CLEANING'),\r\n('M56','PM AMMONIA SYSTEM MAINT'),\r\n('M57','PM LIME BLOWER SYSTEM AND & INSPECTION'),\r\n('M58','PM ENG MAJOR OVERHAULS'),\r\n('M59','PM GAS ENG CATALYST CLEANING'),\r\n('M60','PM ROOF MAINTENANCE'),\r\n('M61','PM FILTER MEDIA INSP'),\r\n('M63','PMI ELEC SOLENOID ONLY ACTUATED VLV'),\r\n('M64','PMI GAS REGULATOR VLV'),\r\n('M65','PMI HYDRAULIC ACTUATED PVLV W/ PILOTS'),\r\n('M66','PMI MANUALLY OPERATED PVLV'),\r\n('M67','PMI PNEUMATIC ACTUATED PVLV'),\r\n('M70','PMI ELECTRIC MOTOR PVLV'),\r\n('M71','PMI MANUAL VLVS BELOW GRND MAINT'),\r\n('M72','PMI PERISTALTIC PUMP'),\r\n('M75','PMI MEDIA REPLACEMENT'),\r\n('S01','PM DEFIBRILLATOR INSP'),\r\n('S02','PM ELEVATOR INSP'),\r\n('S03','PM EMERGENCY LIGHTING'),\r\n('S04','PM DRINKING FOUNTAIN FILT CHANGE'),\r\n('S05','PM STAT INSP'),\r\n('S06','PM SEASONAL STAT WINTER/SUMMER PREP'),\r\n('S07','PM FACILITY WW LIFT STAT INSP'),\r\n('S08','PM FACILITY GARAGE DOOR INSP'),\r\n('S09','PM FIRE &/OR BURGULAR ALARM INSP'),\r\n('S10','PM MONTHLY FIRE EX INSP'),\r\n('S11','PM ANNUAL FIRE EX INSP'),\r\n('S12','PM WATER MONTHLY FIRE SUPP SYST INSP'),\r\n('S13','PM FIRE SUPPRESSION SYST INSP'),\r\n('S14','PM INVENTORY FOR SAFETY KITS'),\r\n('S15','PM ELECTRIC GLOVES INSP'),\r\n('S16','PM FALL PROTECTION INSP'),\r\n('S17','PM FALL ARREST SHOCK LANYARD INSP'),\r\n('S18','PM CARABINER INSP'),\r\n('S19','PM RESPIRATOR CARTRIDGE REPLACE'),\r\n('S20','PM SCBA GENERIC INSP'),\r\n('S21','PM SUIT A SUIT INSP'),\r\n('S22','PM EYEWASH & SAFETY SHOWER INSP'),\r\n('S23','PM SAFGASDT 9A GAS DETECTORS INSP'),\r\n('S24','PM GAS DETECTORS CALIB & INSP'),\r\n('S25','PM FENCING & GATE INSP'),\r\n('S26','PM MANLIFT PM'),\r\n('S27','PM FORKTRUCK FORK LIFT'),\r\n('S28','PM LADDER INSP'),\r\n('S29','PMI ALARM TESTING EQUIP HI LO'),\r\n('T01','TR  FIRE EXT. REFRESHER'),\r\n('T02','BULK FUEL TANK INSPECTION'),\r\n('V01','PM PRESSURE REGULATING VALVE INSPECTION'),\r\n('V32','PMI DISTRIBUTION/TRANS VALVE INSP'),\r\n('V33','PM HYD AUXILLARY VALVES INSP'),\r\n('W01','PM SEWER JET RODDING'),\r\n('W02','PM SLUDGE PIT CHANNEL CLEANING'),\r\n('W03','PM MANHOLE INSPECTION')";

        private string DeleteSql = "DELETE FROM TaskGroups WHERE 1 = 1";
        private string InsertIntoRole = "INSERT INTO Modules(ApplicationId, Name) VALUES(2, 'Production Planned Work')";

        private string DeleteFromRole =
            "DELETE FROM Modules WHERE ApplicationId = 2 AND Name = 'Production Planned Work'";

        public override void Up()
        {
            Execute.Sql(InsertIntoRole);
            Create.Table("TaskGroups")
                  .WithIdentityColumn()
                  .WithColumn("TaskGroupId").AsString(5).NotNullable()
                  .WithColumn("Description").AsString(50).NotNullable()
                  .WithColumn("Required").AsBoolean().Nullable()
                  .WithColumn("Frequency").AsInt32().Nullable();
            Execute.Sql(InsertSql);
        }

        public override void Down()
        {
            Execute.Sql(DeleteFromRole);
            Execute.Sql(DeleteSql);
            Delete.Table("TaskGroups");
        }
    }
}
