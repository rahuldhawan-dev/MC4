using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class PumpCentrifugalExcelRecord : EquipmentExcelRecordBase<PumpCentrifugalExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 190, EQUIPMENT_PURPOSE = 401;

        public struct Characteristics
        {
            public const int APPLICATION_PMP_CENT = 1452,
                             BEARINGNUM_FREE_END = 1623,
                             BHP_RATING = 830,
                             FLOW_MAXIMUM = 1617,
                             FLOW_RATING = 1615,
                             FLOW_UOM = 1492,
                             HORSE_POWER = 1857,
                             LUBE_TP = 1446,
                             LUBE_TP_2 = 901,
                             NARUC_MAINTENANCE_ACCOUNT = 2145,
                             NARUC_OPERATIONS_ACCOUNT = 2146,
                             ORIENTATION = 1087,
                             OWNED_BY = 1614,
                             PMP_BEARING_TP_COUP_END = 1359,
                             PMP_BEARING_TP_FREE_END = 980,
                             PMP_DISCHARGE_SIZE = 1016,
                             PMP_EFICIENCY = 1619,
                             PMP_IMPELLER_MATL = 1403,
                             PMP_IMPELLER_SIZE = 1618,
                             PMP_INLET_SIZE = 1301,
                             PMP_MATERIAL = 1270,
                             PMP_NPSH_RATING = 1621,
                             PMP_SEAL_TP = 1404,
                             PMP_SHUT_OFF_HEAD = 1620,
                             PMP_STAGES = 1307,
                             PMP_TDH_RATING = 1616,
                             PMP_BEARING = 1689,
                             PMP_CENT_TYP = 1471,
                             ROTATION_DIRECTION = 1018,
                             RPM_RATING = 1512,
                             SPECIAL_MAINT_NOTES_DIST = 1624,
                             VOLTAGE = 1858;
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string BHPRating { get; set; }
        public string CentrifugalPumpTyp { get; set; }
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
        public string RPMRating { get; set; }
        public string RotationDirection { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2410;
        protected override int NARUCSpecialMtnNoteDetailsId => 2411;

        protected override string EquipmentType => "PUMP CENTRIFUGAL";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_PMP_CENT),
                mapper.Numerical(BHPRating, nameof(BHPRating), Characteristics.BHP_RATING),
                mapper.DropDown(CentrifugalPumpTyp, nameof(CentrifugalPumpTyp), Characteristics.PMP_CENT_TYP),
                mapper.Numerical(FlowMaximum, nameof(FlowMaximum), Characteristics.FLOW_MAXIMUM),
                mapper.Numerical(FlowRating, nameof(FlowRating), Characteristics.FLOW_RATING),
                mapper.DropDown(FlowUOM, nameof(FlowUOM), Characteristics.FLOW_UOM),
                mapper.DropDown(LubeType1, nameof(LubeType1), Characteristics.LUBE_TP),
                mapper.DropDown(LubeType2, nameof(LubeType2), Characteristics.LUBE_TP_2),
                mapper.DropDown(Orientation, nameof(Orientation), Characteristics.ORIENTATION),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                //mapper.Numerical(PmpBearinglowero, nameof(PmpBearinglowero), Characteristics.PM),
                //mapper.Numerical(PmpBearingupperi, nameof(PmpBearingupperi), Characteristics.),
                //mapper.DropDown(PmpBearingTPlower, nameof(PmpBearingTPlower), Characteristics.),
                //mapper.DropDown(PmpBearingTPupper, nameof(PmpBearingTPupper), Characteristics.),
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
                mapper.DropDown(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES_DIST),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}