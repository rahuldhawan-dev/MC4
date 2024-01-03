using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230626131906283), Tags("Production")]
    public class MC5388_AddColumnsToEquipmentTypeTableAndPopulate : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("ProductionAssetTypes", 25,
                "Hydrant",
                "Mechanical",
                "Electrical",
                "Tanks",
                "Sewer",
                "Valve",
                "Opening",
                "Vehicle");

            Alter.Table("EquipmentTypes")
                 .AddColumn("EquipmentCategory")
                 .AsString(1).Nullable();

            Alter.Table("EquipmentTypes")
                 .AddColumn("ReferenceEquipmentNumber")
                 .AsString(60).Nullable();

            Alter.Table("EquipmentTypes").AddForeignKeyColumn("ProductionAssetTypeId", "ProductionAssetTypes");
            
            Update.Table("EquipmentTypes").Set(new {
                ProductionAssetTypeId = ProductionAssetType.Indices.HYDRANT,
                EquipmentCategory = "H", 
                ReferenceEquipmentNumber = "REFERENCE-HYD"
            }).Where(new { Id = EquipmentType.Indices.HYD });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.MECHANICAL,
                EquipmentCategory = "M",
                ReferenceEquipmentNumber = "REFERENCE-ENG"
            }).Where(new { Id = EquipmentType.Indices.ENG });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.ELECTRICAL,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-GEN"
            }).Where(new { Id = EquipmentType.Indices.GEN });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.ELECTRICAL,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-MOT"
            }).Where(new { Id = EquipmentType.Indices.MOT });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.MECHANICAL,
                EquipmentCategory = "M",
                ReferenceEquipmentNumber = "REFERENCE-PMP-CENT"
            }).Where(new { Id = EquipmentType.Indices.PMP_CENT });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.MECHANICAL,
                EquipmentCategory = "M",
                ReferenceEquipmentNumber = "REFERENCE-PMP-GRND"
            }).Where(new { Id = EquipmentType.Indices.PMP_GRND });
            
            Update.Table("EquipmentTypes").Set(new { 
                ProductionAssetTypeId = ProductionAssetType.Indices.MECHANICAL,
                EquipmentCategory = "M",
                ReferenceEquipmentNumber = "REFERENCE-PMP-PD"
            }).Where(new { Id = EquipmentType.Indices.PMP_PD });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.ELECTRICAL,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-RTU-PLC"
            }).Where(new { Id = EquipmentType.Indices.RTU_PLC });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "T",
                ReferenceEquipmentNumber = "REFERENCE-TNK-CHEM"
            }).Where(new { Id = EquipmentType.Indices.TNK_CHEM });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "T",
                ReferenceEquipmentNumber = "REFERENCE-TNK-FUEL"
            }).Where(new { Id = EquipmentType.Indices.TNK_FUEL });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "T",
                ReferenceEquipmentNumber = "REFERENCE-TNK-PVAC"
            }).Where(new { Id = EquipmentType.Indices.TNK_PVAC });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "T",
                ReferenceEquipmentNumber = "REFERENCE-TNK-WNON"
            }).Where(new { Id = EquipmentType.Indices.TNK_WNON });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "T",
                ReferenceEquipmentNumber = "REFERENCE-TNK-WPOT"
            }).Where(new { Id = EquipmentType.Indices.TNK_WPOT });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "T",
                ReferenceEquipmentNumber = "REFERENCE-TNK-WSTE"
            }).Where(new { Id = EquipmentType.Indices.TNK_WSTE });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-PDMTOOL"
            }).Where(new { Id = EquipmentType.Indices.PDMTOOL });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "C",
                ReferenceEquipmentNumber = "REFERENCE-SCRBBR"
            }).Where(new { Id = EquipmentType.Indices.SCRBBR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-SCALE"
            }).Where(new { Id = EquipmentType.Indices.SCALE });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "S",
                ReferenceEquipmentNumber = "REFERENCE-PPE-ARC"
            }).Where(new { Id = EquipmentType.Indices.PPE_ARC });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "3",
                ReferenceEquipmentNumber = "REFERENCE-TRT-UV"
            }).Where(new { Id = EquipmentType.Indices.TRT_UV });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-PWRFEEDR"
            }).Where(new { Id = EquipmentType.Indices.PWRFEEDR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "B",
                ReferenceEquipmentNumber = "REFERENCE-HVAC-VNT"
            }).Where(new { Id = EquipmentType.Indices.HVAC_VNT });
            
            Update.Table("EquipmentTypes").Set(new { 
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-PRNTR",
            }).Where(new { Id = EquipmentType.Indices.PRNTR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-RECORDER"
            }).Where(new { Id = EquipmentType.Indices.RECORDER });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-XFMR",
            }).Where(new { Id = EquipmentType.Indices.XFMR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-PWRSURG"
            }).Where(new { Id = EquipmentType.Indices.PWRSURG });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-COMM-TEL"
            }).Where(new { Id = EquipmentType.Indices.COMM_TEL });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "M",
                ReferenceEquipmentNumber = "REFERENCE-MIXR"
            }).Where(new { Id = EquipmentType.Indices.MIXR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "K",
                ReferenceEquipmentNumber = "REFERENCE-KIT"
            }).Where(new { Id = EquipmentType.Indices.KIT });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-INST-SW"
            }).Where(new { Id = EquipmentType.Indices.INST_SW });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "3",
                ReferenceEquipmentNumber = "REFERENCE-TRT-CLAR"
            }).Where(new { Id = EquipmentType.Indices.TRT_CLAR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "B",
                ReferenceEquipmentNumber = "REFERENCE-HVAC-HTR"
            }).Where(new { Id = EquipmentType.Indices.HVAC_HTR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "S",
                ReferenceEquipmentNumber = "REFERENCE-FIRE-SUP"
            }).Where(new { Id = EquipmentType.Indices.FIRE_SUP });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-BATT"
            }).Where(new { Id = EquipmentType.Indices.BATT });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "5",
                ReferenceEquipmentNumber = "REFERENCE-DAM"
            }).Where(new { Id = EquipmentType.Indices.DAM });
            
            Update.Table("EquipmentTypes").Set(new { 
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-CALIB",
            }).Where(new { Id = EquipmentType.Indices.CALIB });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "B",
                ReferenceEquipmentNumber = "REFERENCE-BURNER",
            }).Where(new { Id = EquipmentType.Indices.BURNER });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "B",
                ReferenceEquipmentNumber = "REFERENCE-BOILER",
            }).Where(new { Id = EquipmentType.Indices.BOILER });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "B",
                ReferenceEquipmentNumber = "REFERENCE-HVAC-DHM",
            }).Where(new { Id = EquipmentType.Indices.HVAC_DHM });
            
            Update.Table("EquipmentTypes").Set(new { 
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "3",
                ReferenceEquipmentNumber = "REFERENCE-TRT-STRP",
            }).Where(new { Id = EquipmentType.Indices.TRT_STRP });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "C",
                ReferenceEquipmentNumber = "REFERENCE-CHMF-LIQ",
            }).Where(new { Id = EquipmentType.Indices.CHMF_LIQ });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-COMM-FWL",
            }).Where(new { Id = EquipmentType.Indices.COMM_FWL });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "C",
                ReferenceEquipmentNumber = "REFERENCE-CHMF-DRY",
            }).Where(new { Id = EquipmentType.Indices.CHMF_DRY });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "C",
                ReferenceEquipmentNumber = "REFERENCE-CHEM-PIP",
            }).Where(new { Id = EquipmentType.Indices.CHEM_PIP });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "C",
                ReferenceEquipmentNumber = "REFERENCE-CHEM-GEN",
            }).Where(new { Id = EquipmentType.Indices.CHEM_GEN });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "B",
                ReferenceEquipmentNumber = "REFERENCE-HVAC-CMB",
            }).Where(new { Id = EquipmentType.Indices.HVAC_CMB });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "3",
                ReferenceEquipmentNumber = "REFERENCE-TRT-SOFT",
            }).Where(new { Id = EquipmentType.Indices.TRT_SOFT });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "B",
                ReferenceEquipmentNumber = "REFERENCE-HVAC-CHL",
            }).Where(new { Id = EquipmentType.Indices.HVAC_CHL });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "3",
                ReferenceEquipmentNumber = "REFERENCE-TRT-FILT",
            }).Where(new { Id = EquipmentType.Indices.TRT_FILT });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "M",
                ReferenceEquipmentNumber = "REFERENCE-HOIST",
            }).Where(new { Id = EquipmentType.Indices.HOIST });
            
            Update.Table("EquipmentTypes").Set(new { 
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-CATHODIC",
            }).Where(new { Id = EquipmentType.Indices.CATHODIC });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "3",
                ReferenceEquipmentNumber = "REFERENCE-TRT-AER",
            }).Where(new { Id = EquipmentType.Indices.TRT_AER });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-TRAN-SW",
            }).Where(new { Id = EquipmentType.Indices.TRAN_SW });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-LK-MON",
            }).Where(new { Id = EquipmentType.Indices.LK_MON });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-LABEQ",
            }).Where(new { Id = EquipmentType.Indices.LABEQ });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-SCADASYS",
            }).Where(new { Id = EquipmentType.Indices.SCADASYS });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "S",
                ReferenceEquipmentNumber = "REFERENCE-SAF-SHWR",
            }).Where(new { Id = EquipmentType.Indices.SAF_SHWR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "S",
                ReferenceEquipmentNumber = "REFERENCE-SAFGASDT",
            }).Where(new { Id = EquipmentType.Indices.SAFGASDT });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "3",
                ReferenceEquipmentNumber = "REFERENCE-TRT-CONT",
            }).Where(new { Id = EquipmentType.Indices.TRT_CONT });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "M",
                ReferenceEquipmentNumber = "REFERENCE-GRINDER",
            }).Where(new { Id = EquipmentType.Indices.GRINDER });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-PWRRELAY",
            }).Where(new { Id = EquipmentType.Indices.PWRRELAY });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-PWRPNL",
            }).Where(new { Id = EquipmentType.Indices.PWRPNL });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-PWRMON",
            }).Where(new { Id = EquipmentType.Indices.PWRMON });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "B",
                ReferenceEquipmentNumber = "REFERENCE-HVAC-TWR",
            }).Where(new { Id = EquipmentType.Indices.HVAC_TWR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-PHASECON",
            }).Where(new { Id = EquipmentType.Indices.PHASECON });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "B",
                ReferenceEquipmentNumber = "REFERENCE-TOOL",
            }).Where(new { Id = EquipmentType.Indices.TOOL });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "S",
                ReferenceEquipmentNumber = "REFERENCE-FIRE-EX",
            }).Where(new { Id = EquipmentType.Indices.FIRE_EX });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "S",
                ReferenceEquipmentNumber = "REFERENCE-AED",
            }).Where(new { Id = EquipmentType.Indices.AED });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-WQANLZR",
            }).Where(new { Id = EquipmentType.Indices.WQANLZR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "2",
                ReferenceEquipmentNumber = "REFERENCE-WELL",
            }).Where(new { Id = EquipmentType.Indices.WELL });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-XMTR",
            }).Where(new { Id = EquipmentType.Indices.XMTR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-PWRDISC",
            }).Where(new { Id = EquipmentType.Indices.PWRDISC });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-PWRCOND",
            }).Where(new { Id = EquipmentType.Indices.PWRCOND });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-ADJSPD",
            }).Where(new { Id = EquipmentType.Indices.ADJSPD });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-PC",
            }).Where(new { Id = EquipmentType.Indices.PC });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-OIT",
            }).Where(new { Id = EquipmentType.Indices.OIT });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-INDICATR",
            }).Where(new { Id = EquipmentType.Indices.INDICATR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "B",
                ReferenceEquipmentNumber = "REFERENCE-HVAC-WH",
            }).Where(new { Id = EquipmentType.Indices.HVAC_WH });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-PRESDMP",
            }).Where(new { Id = EquipmentType.Indices.PRESDMP });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-SERVR",
            }).Where(new { Id = EquipmentType.Indices.SERVR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-UPS",
            }).Where(new { Id = EquipmentType.Indices.UPS });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-PWRBRKR",
            }).Where(new { Id = EquipmentType.Indices.PWRBRKR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "V",
                ReferenceEquipmentNumber = "REFERENCE-PVLV",
            }).Where(new { Id = EquipmentType.Indices.PVLV });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "S",
                ReferenceEquipmentNumber = "REFERENCE-ELIGHT",
            }).Where(new { Id = EquipmentType.Indices.ELIGHT });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-MOTSTR",
            }).Where(new { Id = EquipmentType.Indices.MOTSTR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "S",
                ReferenceEquipmentNumber = "REFERENCE-PPE-RESP",
            }).Where(new { Id = EquipmentType.Indices.PPE_RESP });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "S",
                ReferenceEquipmentNumber = "REFERENCE-SECSYS",
            }).Where(new { Id = EquipmentType.Indices.SECSYS });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "S",
                ReferenceEquipmentNumber = "REFERENCE-PPE-FLOT",
            }).Where(new { Id = EquipmentType.Indices.PPE_FLOT });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "3",
                ReferenceEquipmentNumber = "REFERENCE-SCREEN",
            }).Where(new { Id = EquipmentType.Indices.SCREEN });
            
            Update.Table("EquipmentTypes").Set(new { 
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "S",
                ReferenceEquipmentNumber = "REFERENCE-PPE-FALL",
            }).Where(new { Id = EquipmentType.Indices.PPE_FALL });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "S",
                ReferenceEquipmentNumber = "REFERENCE-FIRE-AL",
            }).Where(new { Id = EquipmentType.Indices.FIRE_AL });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "B",
                ReferenceEquipmentNumber = "REFERENCE-FACILITY",
            }).Where(new { Id = EquipmentType.Indices.FACILITY });
            
            Update.Table("EquipmentTypes").Set(new { 
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "S",
                ReferenceEquipmentNumber = "REFERENCE-EYEWASH",
            }).Where(new { Id = EquipmentType.Indices.EYEWASH });
            
            Update.Table("EquipmentTypes").Set(new { 
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "M",
                ReferenceEquipmentNumber = "REFERENCE-BLWR",
            }).Where(new { Id = EquipmentType.Indices.BLWR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-BATTCHGR",
            }).Where(new { Id = EquipmentType.Indices.BATTCHGR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-GEARBOX",
            }).Where(new { Id = EquipmentType.Indices.GEARBOX });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "M",
                ReferenceEquipmentNumber = "REFERENCE-CONVEYOR",
            }).Where(new { Id = EquipmentType.Indices.CONVEYOR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "1",
                ReferenceEquipmentNumber = "REFERENCE-FLO-WEIR",
            }).Where(new { Id = EquipmentType.Indices.FLO_WEIR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "B",
                ReferenceEquipmentNumber = "REFERENCE-ELEVATOR",
            }).Where(new { Id = EquipmentType.Indices.ELEVATOR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "C",
                ReferenceEquipmentNumber = "REFERENCE-CONTAIN",
            }).Where(new { Id = EquipmentType.Indices.CONTAIN });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "1",
                ReferenceEquipmentNumber = "REFERENCE-FLO-MET",
            }).Where(new { Id = EquipmentType.Indices.FLO_MET });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "E",
                ReferenceEquipmentNumber = "REFERENCE-CONTACTR",
            }).Where(new { Id = EquipmentType.Indices.CONTACTR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-COMM-SW",
            }).Where(new { Id = EquipmentType.Indices.COMM_SW });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-COMM-RTR",
            }).Where(new { Id = EquipmentType.Indices.COMM_RTR });
            
            Update.Table("EquipmentTypes").Set(new { 
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-COMM-RAD",
            }).Where(new { Id = EquipmentType.Indices.COMM_RAD });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "C",
                ReferenceEquipmentNumber = "REFERENCE-CHMF-GAS",
            }).Where(new { Id = EquipmentType.Indices.CHMF_GAS });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "B",
                ReferenceEquipmentNumber = "REFERENCE-HVAC-EXC",
            }).Where(new { Id = EquipmentType.Indices.HVAC_EXC });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "M",
                ReferenceEquipmentNumber = "REFERENCE-COMP",
            }).Where(new { Id = EquipmentType.Indices.COMP });
            
            Update.Table("EquipmentTypes").Set(new { 
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-COMM-MOD",
            }).Where(new { Id = EquipmentType.Indices.COMM_MOD });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-CNTRLR",
            }).Where(new { Id = EquipmentType.Indices.CNTRLR });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "6",
                ReferenceEquipmentNumber = "REFERENCE-CNTRLPNL",
            }).Where(new { Id = EquipmentType.Indices.CNTRLPNL });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.TANKS,
                EquipmentCategory = "3",
                ReferenceEquipmentNumber = "REFERENCE-UV-SOUND",
            }).Where(new { Id = EquipmentType.Indices.UV_SOUND });
            
            Update.Table("EquipmentTypes").Set(new {  
                ProductionAssetTypeId = ProductionAssetType.Indices.SEWER,
                EquipmentCategory = "4",
                ReferenceEquipmentNumber = "REFERENCE-CO",
            }).Where(new { Id = EquipmentType.Indices.CO });
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("EquipmentTypes", "ProductionAssetTypeId", "ProductionAssetTypes");
            Delete.Column("ReferenceEquipmentNumber").FromTable("EquipmentTypes");
            Delete.Column("EquipmentCategory").FromTable("EquipmentTypes");
            Delete.Table("ProductionAssetTypes");
        }

        public class ProductionAssetType
        {
            public struct Indices
            {
                public const int
                    HYDRANT = 1,
                    MECHANICAL = 2,
                    ELECTRICAL = 3,
                    TANKS = 4,
                    SEWER = 5,
                    VALVE = 6,
                    OPENING = 7,
                    VEHICLE = 8;
            }
        }

        public class EquipmentType
        {
            public struct Indices
            {
                public const int
                    ADJSPD = 121,
                    AED = 122,
                    BATT = 123,
                    BATTCHGR = 124,
                    BLWR = 125,
                    BOILER = 126,
                    BURNER = 127,
                    CALIB = 128,
                    CATHODIC = 129,
                    CHEM_GEN = 130,
                    CHEM_PIP = 131,
                    CHMF_DRY = 132,
                    CHMF_GAS = 133,
                    CHMF_LIQ = 134,
                    CNTRLPNL = 135,
                    CNTRLR = 136,
                    CO = 137,
                    COLLSYS = 138,
                    COMM_FWL = 139,
                    COMM_MOD = 140,
                    COMM_RAD = 141,
                    COMM_RTR = 142,
                    COMM_SW = 143,
                    COMM_TEL = 144,
                    COMP = 145,
                    CONTACTR = 146,
                    CONTAIN = 147,
                    CONVEYOR = 148,
                    DAM = 149,
                    DISTSYS = 150,
                    DISTTOOL = 151,
                    ELEVATOR = 152,
                    ELIGHT = 153,
                    ENG = 154,
                    EYEWASH = 155,
                    FACILITY = 156,
                    FIRE_AL = 157,
                    FIRE_EX = 158,
                    FIRE_SUP = 159,
                    FLO_MET = 160,
                    FLO_WEIR = 161,
                    GEARBOX = 162,
                    GEN = 163,
                    GMAIN = 164,
                    GRINDER = 165,
                    HOIST = 166,
                    HVAC_CHL = 167,
                    HVAC_CMB = 168,
                    HVAC_DHM = 169,
                    HVAC_EXC = 170,
                    HVAC_HTR = 171,
                    HVAC_TWR = 172,
                    HVAC_VNT = 173,
                    HVAC_WH = 174,
                    HYD = 175,
                    INDICATR = 176,
                    INST_SW = 177,
                    KIT = 178,
                    LABEQ = 179,
                    LK_MON = 180,
                    MH = 181,
                    MIXR = 182,
                    MOT = 183,
                    MOTSTR = 184,
                    NARUC_EQ = 185,
                    OIT = 186,
                    PC = 187,
                    PDMTOOL = 188,
                    PHASECON = 189,
                    PMP_CENT = 190,
                    PMP_GRND = 191,
                    PMP_PD = 192,
                    PPE_ARC = 193,
                    PPE_FALL = 194,
                    PPE_FLOT = 195,
                    PPE_RESP = 196,
                    PRESDMP = 197,
                    PRNTR = 198,
                    PVLV = 199,
                    PWRBRKR = 200,
                    PWRCOND = 201,
                    PWRDISC = 202,
                    PWRFEEDR = 203,
                    PWRMON = 204,
                    PWRPNL = 205,
                    PWRRELAY = 206,
                    PWRSURG = 207,
                    RECORDER = 208,
                    RTU_PLC = 209,
                    SAFGASDT = 210,
                    SAF_SHWR = 211,
                    SCADASYS = 212,
                    SCALE = 213,
                    SCRBBR = 214,
                    SCREEN = 215,
                    SECSYS = 216,
                    SERVR = 217,
                    SVLV = 218,
                    SVLV_BO = 219,
                    TNK_CHEM = 220,
                    TNK_FUEL = 221,
                    TNK_PVAC = 222,
                    TNK_WNON = 223,
                    TNK_WPOT = 224,
                    TNK_WSTE = 225,
                    TOOL = 226,
                    TRAN_SW = 227,
                    TRT_AER = 228,
                    TRT_CLAR = 229,
                    TRT_CONT = 230,
                    TRT_FILT = 231,
                    TRT_SOFT = 232,
                    TRT_STRP = 233,
                    TRT_UV = 234,
                    UPS = 235,
                    VEH = 236,
                    WELL = 237,
                    WQANLZR = 238,
                    XFMR = 239,
                    XMTR = 240,
                    AMIDATACOLL = 241,
                    UV_SOUND = 242;
            }
        }
    }
}

