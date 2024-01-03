using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PumpGrinderExcelRecord : EquipmentExcelRecordBase<PumpGrinderExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 191,
            EQUIPMENT_PURPOSE = 402;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_PMP_GRND = 1432,
                             BHP_RATING = 833,
                             FLOW_MAXIMUM = 1628,
                             FLOW_RATING = 1626,
                             FLOW_UOM = 1299,
                             LUBE_TP = 993,
                             LUBE_TP_2 = 917,
                             ORIENTATION = 1138,
                             OWNED_BY = 1625,
                             PMP_BEARINGNUMUPPER_INNER = 1798,
                             PMP_BEARING_TP_COUP_END = 1072,
                             PMP_BEARING_TP_FREE_END = 1061,
                             PMP_BEARINGTP_LOWER_OUTER = 1799,
                             PMP_BEARINGTP_UPPER_INNER = 1797,
                             PMP_DISCHARGE_SIZE = 1255,
                             PMP_EFICIENCY = 1629,
                             PMP_IMPELLER_MATL = 1495,
                             PMP_IMPELLER_SIZE = 1796,
                             PMP_INLET_SIZE = 1555,
                             PMP_MATERIAL = 1324,
                             PMP_NPSH_RATING = 1631,
                             PMP_SEAL_TP = 1295,
                             PMP_SHUT_OFF_HEAD = 1630,
                             PMP_STAGES = 870,
                             PMP_TDH_RATING = 1627,
                             PMP_BEARINGNUMLOWER_OUTER = 1800,
                             PMP_GRND_TYP = 1114,
                             ROTATION_DIRECTION = 933,
                             RPM_RATING = 968,
                             NARUC_MAINTENANCE_ACCOUNT = 2147,
                             NARUC_OPERATIONS_ACCOUNT = 2148,
                             SPECIAL_MAINT_NOTES_DIST = 1634;

            #endregion
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string BHPRating { get; set; }
        public string FlowMaximum { get; set; }
        public string FlowRating { get; set; }
        public string FlowUOM { get; set; }
        public string GrinderPumpType { get; set; }
        public string LubeType1 { get; set; }
        public string LubeType2 { get; set; }
        public string Orientation { get; set; }
        public string OwnedBy { get; set; }
        public string PmpBearinglowero { get; set; }
        public string PmpBearingupperi { get; set; }
        public string PmpBearingTPlower { get; set; }
        public string PmpBearingTPupper { get; set; }
        public string PmpDischargeSize { get; set; }
        public string PmpEfficiencyfact { get; set; }
        public string PmpImpellerMatl { get; set; }
        public string PmpImpellerSize { get; set; }
        public string PmpInletSize { get; set; }
        public string PmpMaterial { get; set; }
        public string PmpNPSHRating { get; set; }
        public string PmpSealType { get; set; }
        public string PmpShutoffHead { get; set; }
        public string PmpStages { get; set; }
        public string PmpTDHRating { get; set; }
        public string RPMRating { get; set; }
        public string RotationDirection { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2412;
        protected override int NARUCSpecialMtnNoteDetailsId => 2413;

        protected override string EquipmentType => "PUMP GRINDER";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_PMP_GRND),
                mapper.Numerical(BHPRating, nameof(BHPRating), Characteristics.BHP_RATING),
                mapper.Numerical(FlowMaximum, nameof(FlowMaximum), Characteristics.FLOW_MAXIMUM),
                mapper.Numerical(FlowRating, nameof(FlowRating), Characteristics.FLOW_RATING),
                mapper.DropDown(FlowUOM, nameof(FlowUOM), Characteristics.FLOW_UOM),
                mapper.DropDown(GrinderPumpType, nameof(GrinderPumpType), Characteristics.PMP_GRND_TYP),
                mapper.DropDown(LubeType1, nameof(LubeType1), Characteristics.LUBE_TP),
                mapper.DropDown(LubeType2, nameof(LubeType2), Characteristics.LUBE_TP_2),
                mapper.DropDown(Orientation, nameof(Orientation), Characteristics.ORIENTATION),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(PmpBearinglowero, nameof(PmpBearinglowero), Characteristics.PMP_BEARINGNUMLOWER_OUTER),
                mapper.String(PmpBearingupperi, nameof(PmpBearingupperi), Characteristics.PMP_BEARINGNUMUPPER_INNER),
                mapper.String(PmpBearingTPlower, nameof(PmpBearingTPlower), Characteristics.PMP_BEARINGTP_LOWER_OUTER),
                mapper.String(PmpBearingTPupper, nameof(PmpBearingTPupper), Characteristics.PMP_BEARINGTP_UPPER_INNER),
                mapper.DropDown(PmpDischargeSize, nameof(PmpDischargeSize), Characteristics.PMP_DISCHARGE_SIZE),
                mapper.Numerical(PmpEfficiencyfact, nameof(PmpEfficiencyfact), Characteristics.PMP_EFICIENCY),
                mapper.DropDown(PmpImpellerMatl, nameof(PmpImpellerMatl), Characteristics.PMP_IMPELLER_MATL),
                mapper.String(PmpImpellerSize, nameof(PmpImpellerSize), Characteristics.PMP_IMPELLER_SIZE),
                mapper.DropDown(PmpInletSize, nameof(PmpInletSize), Characteristics.PMP_INLET_SIZE),
                mapper.DropDown(PmpMaterial, nameof(PmpMaterial), Characteristics.PMP_MATERIAL),
                mapper.String(PmpNPSHRating, nameof(PmpNPSHRating), Characteristics.PMP_NPSH_RATING),
                mapper.DropDown(PmpSealType, nameof(PmpSealType), Characteristics.PMP_SEAL_TP),
                mapper.String(PmpShutoffHead, nameof(PmpShutoffHead), Characteristics.PMP_SHUT_OFF_HEAD),
                mapper.DropDown(PmpStages, nameof(PmpStages), Characteristics.PMP_STAGES),
                mapper.String(PmpTDHRating, nameof(PmpTDHRating), Characteristics.PMP_TDH_RATING),
                mapper.DropDown(RotationDirection, nameof(RotationDirection), Characteristics.ROTATION_DIRECTION),
                mapper.DropDown(RPMRating, nameof(RPMRating), Characteristics.RPM_RATING),
                mapper.DropDown(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES_DIST),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}