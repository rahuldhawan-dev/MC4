using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230502113909547), Tags("Production")]
    public class MC4541_AddEquipmentGroupTable : Migration
    {
        public override void Up()
        {
            Create.Table("EquipmentGroups")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(25).NotNullable().Unique()
                  .WithColumn("Code").AsString(2).NotNullable().Unique();

            Insert.IntoTable("EquipmentGroups")
                  .Row(new {Code = "A", Description = "Accounting"})
                  .Row(new {Code = "B", Description = "Buildings"})
                  .Row(new {Code = "C", Description = "Chemical"})
                  .Row(new {Code = "D", Description = "Dam"})
                  .Row(new {Code = "E", Description = "Electrical"})
                  .Row(new {Code = "FL", Description = "Fleet"})
                  .Row(new {Code = "FM", Description = "Flow Meter"})
                  .Row(new {Code = "H", Description = "Hydrant"})
                  .Row(new {Code = "I", Description = "Instrument"})
                  .Row(new {Code = "K", Description = "Kit"})
                  .Row(new {Code = "LE", Description = "Lab Equipment"})
                  .Row(new {Code = "M", Description = "Mechanical"})
                  .Row(new {Code = "P", Description = "Pipe"})
                  .Row(new {Code = "SW", Description = "Sewer"})
                  .Row(new {Code = "S", Description = "Safety"})
                  .Row(new {Code = "T", Description = "Tank"})
                  .Row(new {Code = "TR", Description = "Treatment"})
                  .Row(new {Code = "V", Description = "Valve"})
                  .Row(new {Code = "W", Description = "Well"});

            Alter.Table("EquipmentTypes").AddForeignKeyColumn("EquipmentGroupId", "EquipmentGroups").NotNullable().WithDefaultValue(1);

            Execute.Sql(@"update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'ADJSPD'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'S') where [Abbreviation] = 'AED'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'AMIDATACOLL'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'BATT'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'BATTCHGR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'M') where [Abbreviation] = 'BLWR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'B') where [Abbreviation] = 'BOILER'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'B') where [Abbreviation] = 'BURNER'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'CALIB'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'CATHODIC'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'C') where [Abbreviation] = 'CHEM-GEN'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'C') where [Abbreviation] = 'CHEM-PIP'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'C') where [Abbreviation] = 'CHMF-DRY'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'C') where [Abbreviation] = 'CHMF-GAS'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'C') where [Abbreviation] = 'CHMF-LIQ'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'CNTRLPNL'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'CNTRLR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'SW') where [Abbreviation] = 'CO'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'SW') where [Abbreviation] = 'COLLSYS'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'COMM-FWL'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'COMM-MOD'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'COMM-RAD'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'COMM-RTR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'COMM-SW'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'COMM-TEL'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'M') where [Abbreviation] = 'COMP'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'CONTACTR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'C') where [Abbreviation] = 'CONTAIN'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'M') where [Abbreviation] = 'CONVEYOR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'D') where [Abbreviation] = 'DAM'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'P') where [Abbreviation] = 'DISTSYS'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'DISTTOOL'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'B') where [Abbreviation] = 'ELEVATOR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'S') where [Abbreviation] = 'ELIGHT'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'M') where [Abbreviation] = 'ENG'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'S') where [Abbreviation] = 'EYEWASH'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'B') where [Abbreviation] = 'FACILITY'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'S') where [Abbreviation] = 'FIRE-AL'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'S') where [Abbreviation] = 'FIRE-EX'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'S') where [Abbreviation] = 'FIRE-SUP'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'FM') where [Abbreviation] = 'FLO-MET'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'FM') where [Abbreviation] = 'FLO-WEIR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'M') where [Abbreviation] = 'GEARBOX'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'GEN'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'SW') where [Abbreviation] = 'GMAIN'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'M') where [Abbreviation] = 'GRINDER'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'M') where [Abbreviation] = 'HOIST'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'B') where [Abbreviation] = 'HVAC-CHL'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'B') where [Abbreviation] = 'HVAC-CMB'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'B') where [Abbreviation] = 'HVAC-DHM'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'B') where [Abbreviation] = 'HVAC-EXC'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'B') where [Abbreviation] = 'HVAC-HTR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'B') where [Abbreviation] = 'HVAC-TWR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'B') where [Abbreviation] = 'HVAC-VNT'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'B') where [Abbreviation] = 'HVAC-WH'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'H') where [Abbreviation] = 'HYD'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'INDICATR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'INST-SW'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'K') where [Abbreviation] = 'KIT'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'LABEQ'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'LK-MON'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'SW') where [Abbreviation] = 'MH'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'M') where [Abbreviation] = 'MIXR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'MOT'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'MOTSTR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'A') where [Abbreviation] = 'NARUC_EQ'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'OIT'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'PC'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'PDMTOOL'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'PHASECON'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'M') where [Abbreviation] = 'PMP-CENT'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'M') where [Abbreviation] = 'PMP-GRND'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'M') where [Abbreviation] = 'PMP-PD'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'S') where [Abbreviation] = 'PPE-ARC'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'S') where [Abbreviation] = 'PPE-FALL'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'S') where [Abbreviation] = 'PPE-FLOT'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'S') where [Abbreviation] = 'PPE-RESP'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'PRESDMP'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'PRNTR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'V') where [Abbreviation] = 'PVLV'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'PWRBRKR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'PWRCOND'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'PWRDISC'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'PWRFEEDR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'PWRMON'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'PWRPNL'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'PWRRELAY'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'PWRSURG'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'RECORDER'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'RTU-PLC'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'S') where [Abbreviation] = 'SAFGASDT'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'S') where [Abbreviation] = 'SAF-SHWR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'SCADASYS'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'SCALE'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'C') where [Abbreviation] = 'SCRBBR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'TR') where [Abbreviation] = 'SCREEN'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'S') where [Abbreviation] = 'SECSYS'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'SERVR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'V') where [Abbreviation] = 'SVLV'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'V') where [Abbreviation] = 'SVLV-BO'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'T') where [Abbreviation] = 'TNK-CHEM'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'T') where [Abbreviation] = 'TNK-FUEL'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'T') where [Abbreviation] = 'TNK-PVAC'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'T') where [Abbreviation] = 'TNK-WNON'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'T') where [Abbreviation] = 'TNK-WPOT'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'T') where [Abbreviation] = 'TNK-WSTE'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'B') where [Abbreviation] = 'TOOL'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'TRAN-SW'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'TR') where [Abbreviation] = 'TRT-AER'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'TR') where [Abbreviation] = 'TRT-CLAR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'TR') where [Abbreviation] = 'TRT-CONT'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'TR') where [Abbreviation] = 'TRT-FILT'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'TR') where [Abbreviation] = 'TRT-SOFT'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'TR') where [Abbreviation] = 'TRT-STRP'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'TR') where [Abbreviation] = 'TRT-UV'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'UPS'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'FL') where [Abbreviation] = 'VEH'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'W') where [Abbreviation] = 'WELL'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'WQANLZR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'E') where [Abbreviation] = 'XFMR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'I') where [Abbreviation] = 'XMTR'
                        update [EquipmentTypes] set [EquipmentGroupId] = (select [Id] from [EquipmentGroups] where [Code] = 'TR') where [Abbreviation] = 'UV-SOUND'
            ");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("EquipmentTypes", "EquipmentGroupId", "EquipmentGroups");
            Delete.Table("EquipmentGroups");
        }
    }
}