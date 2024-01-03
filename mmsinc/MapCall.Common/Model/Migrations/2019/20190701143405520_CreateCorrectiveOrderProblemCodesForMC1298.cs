using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190701143405520), Tags("Production")]
    public class CreateCorrectiveOrderProblemCodesForMC1298 : Migration
    {
        public override void Up()
        {
            Create.LookupTable("CorrectiveOrderProblemCodes", 200)
                  .WithColumn("Code").AsString(200).NotNullable();

            Insert.IntoTable("CorrectiveOrderProblemCodes")
                  .Rows(new {Code = "NOISE", Description = "abnormal noise / sound / rubbing / hitting"},
                       new {Code = "CORROSION", Description = "corroded / dirty / fouled / stuck"},
                       new {Code = "DAMAGED", Description = "broken / bent / fractured / punctured"},
                       new {
                           Code = "LEAKING",
                           Description = "external leakage / spray / dripping / spilling/internal leak"
                       },
                       new {Code = "FUNCTION", Description = "operation function bad / poor production rate /"},
                       new {Code = "LOOSE", Description = "loose / weak tension"},
                       new {Code = "POWER_ELEC", Description = "power problems / overload / tripping /"},
                       new {Code = "FLUID_LEVEL", Description = "fluid level inadequate"},
                       new {Code = "OVERHEATING", Description = "overheating / smoke smell / sparks / fire "},
                       new {Code = "VIBRATING", Description = "excessive vibration"},
                       new {Code = "COMMUNICATION", Description = "no radio comm / no scada indication / "},
                       new {Code = "FAIL TO START", Description = "Eqmnt will not start on demand"},
                       new {Code = "FAIL TO STOP", Description = "Eqmnt will not stop on demand"},
                       new {Code = "BREAKDOWN", Description = "Eqmt stopped during operation/ "},
                       new {Code = "ABNORMAL READING", Description = "instrument reading outside normal parameters"},
                       new {Code = "PLUGGED/CHOKED", Description = "No Flow/Reduced Flow"},
                       new {Code = "PESTS", Description = "infestation/ insects/ animal"},
                       new {Code = "PRESSURE", Description = "high pressure / low pressure"},
                       new {Code = "DIRTY", Description = "dirt inhibiting operation"},
                       new {Code = "CALIBRATION", Description = "out of cal/ inspection out of date/ out of spec"},
                       new {Code = "FOULED", Description = "sticking/ plugged/ clogged/jammed"},
                       new {Code = "SMELL", Description = "abnormal smell"},
                       new {Code = "ALARM", Description = "alarm indication local or scada"},
                       new {Code = "OTHER", Description = ""},
                       new {Code = "UNKNOWN", Description = ""});

            Alter.Table("ProductionWorkOrders")
                 .AddForeignKeyColumn("CorrectiveOrderProblemCodeId", "CorrectiveOrderProblemCodes")
                 .AddColumn("OtherProblemNotes").AsText().Nullable();

            Create.Table("CorrectiveOrderProblemCodesSAPEquipmentTypes")
                  .WithForeignKeyColumn("CorrectiveOrderProblemCodeId", "CorrectiveOrderProblemCodes", nullable: false)
                  .WithForeignKeyColumn("SAPEquipmentTypeId", "SAPEquipmentTypes", nullable: false);

            Execute.Sql(@"
INSERT INTO CorrectiveOrderProblemCodesSAPEquipmentTypes (CorrectiveOrderProblemCodeId, SAPEquipmentTypeId)
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'NOISE' WHERE t.Abbreviation IN ('PMP-CENT', 'PMP-GRND', 'PMP-PD', 'MOT', 'MOTSTR', 'GEARBOX', 'MIXR', 'COMP', 'ENG', 'HOIST', 'GEN', 'TRT-CLAR', 'TRT-SOFT', 'HVAC-CHL', 'HVAC-CMB', 'HVAC-DHM', 'HVAC-EXC', 'HVAC-HTR', 'HVAC-TWR', 'HVAC-VNT', 'HVAC-WH', 'BOILER', 'PVLV', 'BLWR')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c on c.Code = 'CORROSION'
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c on c.Code = 'DAMAGED'
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'LEAKING' WHERE t.Abbreviation IN ('PMP-CENT', 'PMP-GRND', 'PMP-PD', 'PVLV', ' TNK-CHEM', 'TNK-FUEL', 'TNK-PVAC', 'TNK-WNON', 'TNK-WPOT', 'TNK-WSTE', 'XMTR', 'WQANLZR', 'FLO-MET', 'DAM', 'CHMF-GAS', 'CHMF-DRY', ' HVAC-CHL', 'HVAC-CMB', 'HVAC-DHM', 'HVAC-EXC', 'HVAC-HTR', 'HVAC-TWR', 'HVAC-VNT', 'HVAC-WH', 'BATT')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'FUNCTION' WHERE t.Abbreviation IN ('PMP-CENT', 'PMP-GRND', 'PMP-PD', 'MOT', 'MOTSTR', 'GEARBOX', 'PVLV', 'BLWR', 'BATT', 'SCALE', 'XMTR', 'WQANLZR', 'COMP', ' FLO-MET', 'CHMF-GAS', 'CHMF-DRY', ' HVAC-CHL', 'HVAC-CMB', 'HVAC-DHM', 'HVAC-EXC', 'HVAC-HTR', 'HVAC-TWR', 'HVAC-VNT', 'HVAC-WH', 'BATT')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'LOOSE' WHERE t.Abbreviation IN ('PMP-CENT', 'PMP-GRND', 'PMP-PD', 'MOT', 'MOTSTR', 'GEARBOX', 'PWRBRKR', 'PWRCOND', 'PWRDISC', 'PWRFEEDR', 'PWRMON', 'PWRPNL', 'PWRRELAY', 'PWRSURG')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'POWER_ELEC' WHERE t.Abbreviation IN ('PWRBRKR', 'MOT', 'MOTSTR', 'GEN', 'BATT', 'TRAN-SW', 'PWRCOND', 'PWRDISC', 'PWRFEEDR', 'PWRMON', 'PWRPNL', 'PWRRELAY', 'PWRSURG', 'ADJSPD')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'FLUID_LEVEL' WHERE t.Abbreviation IN ('TNK-CHEM', 'TNK-FUEL', 'TNK-PVAC', 'TNK-WNON', 'TNK-WPOT', 'TNK-WSTE', 'BATT', 'TRT-AER', 'TRT-CLAR', 'TRT-CONT', 'TRT-FILT', 'TRT-SOFT', 'TRT-STRP', 'TRT-UV')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'OVERHEATING' WHERE t.Abbreviation IN ('PMP-CENT', 'PMP-GRND', 'PMP-PD', 'MOT', 'MOTSTR', 'GEARBOX', 'BLWR', 'MIXR', 'COMP', 'ENG', 'GEN', ' HVAC-CHL', 'HVAC-CMB', 'HVAC-DHM', 'HVAC-EXC', 'HVAC-HTR', 'HVAC-TWR', 'HVAC-VNT', 'HVAC-WH', 'BOILER')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'VIBRATING' WHERE t.Abbreviation IN ('PMP-CENT', 'PMP-GRND', 'PMP-PD', 'MOT', 'MOTSTR', 'GEARBOX', 'BLWR', 'COMP', 'ENG', 'GEN')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'COMMUNICATION' WHERE t.Abbreviation IN ('XMTR', 'COMM-MOD', 'COMM-RAD', 'SCADASYS')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'FAIL TO START' WHERE t.Abbreviation IN ('GEN', 'MOT', 'MOTSTR', 'HOIST', 'TRT-CLAR', 'HVAC-CHL', 'HVAC-CMB', 'HVAC-DHM', 'HVAC-EXC', 'HVAC-HTR', 'HVAC-TWR', 'HVAC-VNT', 'HVAC-WH', 'PVLV')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'FAIL TO STOP' WHERE t.Abbreviation IN ('GEN', 'MOT', 'MOTSTR', 'HOIST', 'TRT-CLAR', 'HVAC-CHL', 'HVAC-CMB', 'HVAC-DHM', 'HVAC-EXC', 'HVAC-HTR', 'HVAC-TWR', 'HVAC-VNT', 'HVAC-WH', 'PVLV')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'BREAKDOWN' WHERE t.Abbreviation IN ('GEN', 'MOT', 'MOTSTR', 'HOIST', 'TRT-CLAR', 'HVAC-CHL', 'HVAC-CMB', 'HVAC-DHM', 'HVAC-EXC', 'HVAC-HTR', 'HVAC-TWR', 'HVAC-VNT', 'HVAC-WH', 'PVLV')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'ABNORMAL READING' WHERE t.Abbreviation IN ('WQANLZR', 'XMTR', 'FLO-MET', 'SCALE')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'PLUGGED/CHOKED' WHERE t.Abbreviation IN ('PMP-CENT', 'PMP-GRND', 'PMP-PD', 'PVLV')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'PESTS' WHERE t.Abbreviation IN ('XFMR', 'PWRDISC', 'MOT', 'MOTSTR', 'FACILITY', 'CNTRLPNL', 'GEN', 'PWRPNL')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'PRESSURE' WHERE t.Abbreviation IN ('PMP-CENT', 'PMP-GRND', 'PMP-PD', 'PVLV')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'DIRTY'
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'CALIBRATION' WHERE t.Abbreviation IN ('XMTR', 'FLO-MET', 'WQANLZR')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'FOULED'
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'SMELL'
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'ALARM' WHERE t.Abbreviation IN ('FIRE-AL', 'FIRE-SUP', 'INST-SW', 'PWRBRKR', 'PWRCOND', 'PWRDISC', 'PWRFEEDR', 'PWRMON', 'PWRPNL', 'PWRRELAY', 'PWRSURG', 'RTU-PLC', 'SCADASYS', 'SECSYS', 'SERVR', 'UPS', 'WQANLZR', 'XMTR')
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'OTHER'
UNION
SELECT c.Id, t.Id FROM SAPEquipmentTypes t LEFT JOIN CorrectiveOrderProblemCodes c ON c.Code = 'UNKNOWN'");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("ProductionWorkOrders", "CorrectiveOrderProblemCodeId",
                "CorrectiveOrderProblemCodes");

            Delete.Column("OtherProblemNotes").FromTable("ProductionWorkOrders");

            Delete.Table("CorrectiveOrderProblemCodes");
            Delete.Table("CorrectiveOrderProblemCodesSAPEquipmentTypes");
        }
    }
}
