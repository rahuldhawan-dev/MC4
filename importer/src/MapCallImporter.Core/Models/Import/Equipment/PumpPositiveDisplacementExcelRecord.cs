using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PumpPositiveDisplacementExcelRecord : EquipmentExcelRecordBase<PumpPositiveDisplacementExcelRecord>
    {
        #region Constants

        // pump positive displacement
        public const int EQUIPMENT_TYPE = 192, EQUIPMENT_PURPOSE = 403;

        public struct Characteristics
        {
            public const int APPLICATION_PMP_PD = 1547,
                             BHP_RATING = 832,
                             FLOW_MAXIMUM = 1638,
                             FLOW_RATING = 1636,
                             FLOW_UOM = 1587,
                             LUBE_TP = 1346,
                             LUBE_TP_2 = 1368,
                             NARUC_MAINTENANCE_ACCOUNT = 2149,
                             NARUC_OPERATIONS_ACCOUNT = 2150,
                             ORIENTATION = 1478,
                             OWNED_BY = 1635,
                             PMP_BEARING_TP_COUP_END = 1508,
                             PMP_BEARING_TP_FREE_END = 1409,
                             PMP_DISCHARGE_SIZE = 1332,
                             PMP_EFICIENCY = 1640,
                             PMP_IMPELLER_MATL = 1224,
                             PMP_IMPELLER_SIZE = 1639,
                             PMP_INLET_SIZE = 1321,
                             PMP_MATERIAL = 1438,
                             PMP_NPSH_RATING = 1642,
                             PMP_SEAL_TP = 1328,
                             PMP_SHUT_OFF_HEAD = 1641,
                             PMP_STAGES = 1532,
                             PMP_TDH_RATING = 1637,
                             PMP_BEARING_LOWER_OUTER = 1862,
                             PMP_BEARING_UPPER_INNER = 1860,
                             PMP_BEARINGTP_LOWER_OUTER = 1861,
                             PMP_BEARINGTP_UPPER_INNER = 1859,
                             PMP_PD_TYP = 1411,
                             ROTATION_DIRECTION = 1383,
                             RPM_RATING = 1405,
                             SPECIAL_MAINT_NOTES_DIST = 1645;
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string BHPRating { get; set; }
        public string FlowMaximum { get; set; }
        public string FlowRating { get; set; }
        public string FlowUOM { get; set; }
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
        public string PositiveDisplacemen { get; set; }
        public string RPMRating { get; set; }
        public string RotationDirection { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2414;
        protected override int NARUCSpecialMtnNoteDetailsId => 2415;

        protected override string EquipmentType => "PUMP POSITIVE DISPLACEMENT";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_PMP_PD),
                mapper.Numerical(BHPRating, nameof(BHPRating), Characteristics.BHP_RATING),
                mapper.Numerical(FlowMaximum, nameof(FlowMaximum), Characteristics.FLOW_MAXIMUM),
                mapper.Numerical(FlowRating, nameof(FlowRating), Characteristics.FLOW_RATING),
                mapper.DropDown(FlowUOM, nameof(FlowUOM), Characteristics.FLOW_UOM),
                mapper.DropDown(LubeType1, nameof(LubeType1), Characteristics.LUBE_TP),
                mapper.DropDown(LubeType2, nameof(LubeType2), Characteristics.LUBE_TP_2),
                mapper.DropDown(Orientation, nameof(Orientation), Characteristics.ORIENTATION),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.String(PmpBearinglowero, nameof(PmpBearinglowero), Characteristics.PMP_BEARING_LOWER_OUTER),
                mapper.String(PmpBearingupperi, nameof(PmpBearingupperi), Characteristics.PMP_BEARING_UPPER_INNER),
                mapper.String(PmpBearingTPlower, nameof(PmpBearingTPlower), Characteristics.PMP_BEARINGTP_LOWER_OUTER),
                mapper.String(PmpBearingTPupper, nameof(PmpBearingTPupper), Characteristics.PMP_BEARINGTP_UPPER_INNER),
                mapper.DropDown(PmpDischargeSize, nameof(PmpDischargeSize), Characteristics.PMP_DISCHARGE_SIZE),
                mapper.Numerical(PmpEfficiencyfact, nameof(PmpEfficiencyfact), Characteristics.PMP_EFICIENCY),
                mapper.DropDown(PmpImpellerMatl, nameof(PmpImpellerMatl), Characteristics.PMP_IMPELLER_MATL),
                mapper.Numerical(PmpImpellerSize, nameof(PmpImpellerSize), Characteristics.PMP_IMPELLER_SIZE),
                mapper.DropDown(PmpInletSize, nameof(PmpInletSize), Characteristics.PMP_INLET_SIZE),
                mapper.DropDown(PmpMaterial, nameof(PmpMaterial), Characteristics.PMP_MATERIAL),
                mapper.String(PmpNPSHRating, nameof(PmpNPSHRating), Characteristics.PMP_NPSH_RATING),
                mapper.DropDown(PmpSealType, nameof(PmpSealType), Characteristics.PMP_SEAL_TP),
                mapper.String(PmpShutoffHead, nameof(PmpShutoffHead), Characteristics.PMP_SHUT_OFF_HEAD),
                mapper.DropDown(PmpStages, nameof(PmpStages), Characteristics.PMP_STAGES),
                mapper.String(PmpTDHRating, nameof(PmpTDHRating), Characteristics.PMP_TDH_RATING),
                mapper.DropDown(RotationDirection, nameof(RotationDirection), Characteristics.ROTATION_DIRECTION),
                mapper.DropDown(RPMRating, nameof(RPMRating), Characteristics.RPM_RATING),
                mapper.DropDown(PositiveDisplacemen, nameof(PositiveDisplacemen), Characteristics.PMP_PD_TYP),
                mapper.DropDown(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES_DIST),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}