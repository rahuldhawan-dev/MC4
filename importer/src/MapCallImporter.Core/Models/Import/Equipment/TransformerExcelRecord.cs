using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class TransformerExcelRecord : EquipmentExcelRecordBase<TransformerExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 239, EQUIPMENT_PURPOSE = 420;

        public struct Characteristics
        {
            #region Constants

            public const int AMP_RATING = 818,
                             AMPS_SECONDARY = 1819,
                             APPLICATION_XFMR = 1314,
                             BIL_RATING = 1828,
                             CONTAINS_PCBS = 1984,
                             IMPEDANCE = 1821,
                             INSULATION_CLASS = 945,
                             KVA_RATED = 1822,
                             MOUNTING_XFMR = 889,
                             NARUC_MAINTENANCE_ACCOUNT = 2261,
                             NARUC_OPERATIONS_ACCOUNT = 2262,
                             NEMA_ENCLOSURE = 1241,
                             OIL_CAPACITY_GAL = 1823,
                             OIL_TYPE = 1825,
                             OWNED_BY = 1818,
                             PHASES = 966,
                             SPECIAL_MAINT_NOTES = 1829,
                             TAP_RANGE_TAP = 1827,
                             TEMPERATURE_RISE = 1820,
                             VOLT_RATING = 1190,
                             VOLT_RATING_SECONDARY = 1170,
                             WEIGHT_LBS = 1826,
                             XFMR_PCBS = 1104,
                             XFMR_TYP = 871,
                             XFMR_WINDING_PRI = 1499,
                             XFMR_WINDING_SEC = 1426;

            #endregion
        }

        #endregion

        #region Properties

        public string AmpRating { get; set; }
        public string AmpsSecondary { get; set; }
        public string Application { get; set; }
        public string BILRating { get; set; }
        public string ContainsPCBs { get; set; }
        public string Impedance { get; set; }
        public string InsulationClass { get; set; }
        public string KVARated { get; set; }
        public string Mounting { get; set; }
        public string NEMAEnclosure { get; set; }
        public string OilCapacitygal { get; set; }
        public string OilType { get; set; }
        public string OwnedBy { get; set; }
        public string Phases { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string TapRangeandTap { get; set; }
        public string TemperatureRise { get; set; }
        public string TransformerType { get; set; }
        public string VoltRating { get; set; }
        public string VoltRatingSeconda { get; set; }
        public string Weightlbs { get; set; }
        public string WindingConfigPrim { get; set; }
        public string WindingConfigSecon { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2507;
        protected override int NARUCSpecialMtnNoteDetailsId => 2508;

        protected override string EquipmentType => "TRANSFORMER";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.Numerical(AmpRating, nameof(AmpRating), Characteristics.AMP_RATING),
                mapper.String(AmpsSecondary, nameof(AmpsSecondary), Characteristics.AMPS_SECONDARY),
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_XFMR),
                mapper.String(BILRating, nameof(BILRating), Characteristics.BIL_RATING),
                mapper.String(ContainsPCBs, nameof(ContainsPCBs), Characteristics.CONTAINS_PCBS),
                mapper.String(Impedance, nameof(Impedance), Characteristics.IMPEDANCE),
                mapper.DropDown(InsulationClass, nameof(InsulationClass), Characteristics.INSULATION_CLASS),
                mapper.String(KVARated, nameof(KVARated), Characteristics.KVA_RATED),
                mapper.DropDown(Mounting, nameof(Mounting), Characteristics.MOUNTING_XFMR),
                mapper.DropDown(NEMAEnclosure, nameof(NEMAEnclosure), Characteristics.NEMA_ENCLOSURE),
                mapper.String(OilCapacitygal, nameof(OilCapacitygal), Characteristics.OIL_CAPACITY_GAL),
                mapper.String(OilType, nameof(OilType), Characteristics.OIL_TYPE),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(Phases, nameof(Phases), Characteristics.PHASES),
                mapper.String(TapRangeandTap, nameof(TapRangeandTap), Characteristics.TAP_RANGE_TAP),
                mapper.String(TemperatureRise, nameof(TemperatureRise), Characteristics.TEMPERATURE_RISE),
                mapper.DropDown(TransformerType, nameof(TransformerType), Characteristics.XFMR_TYP),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.DropDown(VoltRatingSeconda, nameof(VoltRatingSeconda), Characteristics.VOLT_RATING_SECONDARY),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(Weightlbs, nameof(Weightlbs), Characteristics.WEIGHT_LBS),
                mapper.DropDown(WindingConfigPrim, nameof(WindingConfigPrim), Characteristics.XFMR_WINDING_PRI),
                mapper.DropDown(WindingConfigSecon, nameof(WindingConfigSecon), Characteristics.XFMR_WINDING_SEC),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}