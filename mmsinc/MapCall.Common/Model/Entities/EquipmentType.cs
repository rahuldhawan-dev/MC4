using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentType : IEntity, IValidatableObject
    {
        #region Constants

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

        //TODO: Move to SAPCode field
        public static readonly int[] SyncronizedEquipmentTypes = {
            Indices.ADJSPD,
            Indices.AED,
            Indices.BATT,
            Indices.BATTCHGR,
            Indices.BLWR,
            Indices.BOILER,
            Indices.BURNER,
            Indices.CALIB,
            Indices.CATHODIC,
            Indices.CHEM_GEN,
            Indices.CHEM_PIP,
            Indices.CHMF_DRY,
            Indices.CHMF_GAS,
            Indices.CHMF_LIQ,
            Indices.CNTRLPNL,
            Indices.CNTRLR,
            Indices.CO,
            Indices.COLLSYS,
            Indices.COMM_FWL,
            Indices.COMM_MOD,
            Indices.COMM_RAD,
            Indices.COMM_RTR,
            Indices.COMM_SW,
            Indices.COMM_TEL,
            Indices.COMP,
            Indices.CONTACTR,
            Indices.CONTAIN,
            Indices.CONVEYOR,
            Indices.DAM,
            Indices.DISTSYS,
            Indices.DISTTOOL,
            Indices.ELEVATOR,
            Indices.ELIGHT,
            Indices.ENG,
            Indices.EYEWASH,
            Indices.FACILITY,
            Indices.FIRE_AL,
            Indices.FIRE_EX,
            Indices.FIRE_SUP,
            Indices.FLO_MET,
            Indices.FLO_WEIR,
            Indices.GEARBOX,
            Indices.GEN,
            Indices.GMAIN,
            Indices.GRINDER,
            Indices.HOIST,
            Indices.HVAC_CHL,
            Indices.HVAC_CMB,
            Indices.HVAC_DHM,
            Indices.HVAC_EXC,
            Indices.HVAC_HTR,
            Indices.HVAC_TWR,
            Indices.HVAC_VNT,
            Indices.HVAC_WH,
            Indices.HYD,
            Indices.INDICATR,
            Indices.INST_SW,
            Indices.KIT,
            Indices.LABEQ,
            Indices.LK_MON,
            Indices.MH,
            Indices.MIXR,
            Indices.MOT,
            Indices.MOTSTR,
            Indices.NARUC_EQ,
            Indices.OIT,
            Indices.PC,
            Indices.PDMTOOL,
            Indices.PHASECON,
            Indices.PMP_CENT,
            Indices.PMP_GRND,
            Indices.PMP_PD,
            Indices.PPE_ARC,
            Indices.PPE_FALL,
            Indices.PPE_FLOT,
            Indices.PPE_RESP,
            Indices.PRESDMP,
            Indices.PRNTR,
            Indices.PVLV,
            Indices.PWRBRKR,
            Indices.PWRCOND,
            Indices.PWRDISC,
            Indices.PWRFEEDR,
            Indices.PWRMON,
            Indices.PWRPNL,
            Indices.PWRRELAY,
            Indices.PWRSURG,
            Indices.RECORDER,
            Indices.RTU_PLC,
            Indices.SAFGASDT,
            Indices.SAF_SHWR,
            Indices.SCADASYS,
            Indices.SCALE,
            Indices.SCRBBR,
            Indices.SCREEN,
            Indices.SECSYS,
            Indices.SERVR,
            Indices.SVLV,
            Indices.SVLV_BO,
            Indices.TNK_CHEM,
            Indices.TNK_FUEL,
            Indices.TNK_PVAC,
            Indices.TNK_WNON,
            Indices.TNK_WPOT,
            Indices.TNK_WSTE,
            Indices.TOOL,
            Indices.TRAN_SW,
            Indices.TRT_AER,
            Indices.TRT_CLAR,
            Indices.TRT_CONT,
            Indices.TRT_FILT,
            Indices.TRT_SOFT,
            Indices.TRT_STRP,
            Indices.TRT_UV,
            Indices.UPS,
            Indices.VEH,
            Indices.WELL,
            Indices.WQANLZR,
            Indices.XFMR,
            Indices.XMTR,
            Indices.AMIDATACOLL,
            Indices.UV_SOUND,
        };

        public static readonly int[] TankTypes = {
            Indices.TNK_CHEM, Indices.TNK_FUEL, Indices.TNK_PVAC, Indices.TNK_WNON, Indices.TNK_WPOT, Indices.TNK_WSTE
        };

        public struct ComparisonValue
        {
            public const string TNK = "TNK-",
                                POTABLE_WATER_TANK = "TNK-WPOT";
        }

        public struct DisplayNames
        {
            public const string DESCRIPTION = "Equipment Type Description";
        }

        #endregion

        #region Private Members

        private EquipmentTypeDisplayItem _display;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Abbreviation { get; set; }

        [View(DisplayName = DisplayNames.DESCRIPTION)]
        public virtual string Description { get; set; }

        public virtual bool IsLockoutRequired { get; set; }

        public virtual ProductionAssetType ProductionAssetType { get; set; }

        public virtual bool IsEligibleForRedTagPermit { get; set; }

        public virtual string EquipmentCategory { get; set; }
        public virtual string ReferenceEquipmentNumber { get; set; }

        public virtual IList<EquipmentCharacteristicField> CharacteristicFields { get; set; }
        public virtual IList<EquipmentPurpose> EquipmentPurposes { get; set; }
        public virtual EquipmentGroup EquipmentGroup { get; set; }

        public virtual string Display => (_display ?? (_display = new EquipmentTypeDisplayItem {
            Abbreviation = Abbreviation,
            Description = Description
        })).Display;

        public virtual IList<MeasurementPointEquipmentType> MeasurementPoints { get; set; } = new List<MeasurementPointEquipmentType>();

        #endregion

        #region Constructors

        public EquipmentType()
        {
            CharacteristicFields = new List<EquipmentCharacteristicField>();
            EquipmentPurposes = new List<EquipmentPurpose>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Display;
        }

        #endregion
    }

    [Serializable]
    public class EquipmentTypeDisplayItem : DisplayItem<EquipmentType>
    {
        public string Abbreviation { get; set; }
        public string Description { get; set; }

        public override string Display => $"{Abbreviation} - {Description}";
    }
}
