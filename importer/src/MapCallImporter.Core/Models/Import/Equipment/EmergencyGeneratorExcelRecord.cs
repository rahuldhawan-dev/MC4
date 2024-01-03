using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class EmergencyGeneratorExcelRecord : EquipmentExcelRecordBase<EmergencyGeneratorExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 163, EQUIPMENT_PURPOSE = 343;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_GEN = 1110,
                             APPLICATION_TNK_FUEL = 1598,
                             FUEL_TNK = 1599,
                             FUEL_TYPE = 1597,
                             GEN_CURRENT = 1602,
                             GEN_KW = 1601,
                             GEN_LOAN = 1293,
                             GEN_PORTABLE = 1584,
                             GEN_TYP = 1215,
                             GEN_VOLTAGE_TP = 1469,
                             OWNED_BY = 1600,
                             PHASES = 1339,
                             RPM_RATING = 971,
                             SELF_STARTING = 1420,
                             SPECIAL_MAINT_NOTES_DIST = 1603,
                             NARUC_MAINTENANCE_ACCOUNT = 2092,
                             NARUC_OPERATIONS_ACCOUNT = 2093,
                             VOLT_RATING = 905;

            #endregion
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string AvailableforEMGLo { get; set; }
        public string FUEL_TYPE { get; set; }
        public string FuelTank { get; set; }
        public string FuelType { get; set; }
        public string GeneratorType { get; set; }
        public string KWRating { get; set; }
        public string MaxOutputCurrent { get; set; }
        public string OwnedBy { get; set; }
        public string Phases { get; set; }
        public string Portable { get; set; }
        public string RPMRating { get; set; }
        public string SelfStarting { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string VoltRating { get; set; }
        public string VoltageACDC { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;
        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2358;
        protected override int NARUCSpecialMtnNoteDetailsId => 2359;

        protected override string EquipmentType => "EMERGENCY GENERATOR";
        
        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_GEN),
                mapper.DropDown(AvailableforEMGLo, nameof(AvailableforEMGLo), Characteristics.GEN_LOAN),
                mapper.DropDown(FuelType, nameof(FuelType), Characteristics.FUEL_TYPE),
                mapper.DropDown(FuelTank, nameof(FuelTank), Characteristics.FUEL_TNK),
                mapper.DropDown(GeneratorType, nameof(GeneratorType), Characteristics.GEN_TYP),
                mapper.Numerical(KWRating, nameof(KWRating), Characteristics.GEN_KW),
                mapper.Numerical(MaxOutputCurrent, nameof(MaxOutputCurrent), Characteristics.GEN_CURRENT),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(Phases, nameof(Phases), Characteristics.PHASES),
                mapper.DropDown(Portable, nameof(Portable), Characteristics.GEN_PORTABLE),
                mapper.DropDown(RPMRating, nameof(RPMRating), Characteristics.RPM_RATING),
                mapper.DropDown(SelfStarting, nameof(SelfStarting), Characteristics.SELF_STARTING),
                mapper.DropDown(VoltRating, nameof(VoltRating), Characteristics.VOLT_RATING),
                mapper.DropDown(VoltageACDC, nameof(VoltageACDC), Characteristics.GEN_VOLTAGE_TP),
                mapper.DropDown(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES_DIST),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}