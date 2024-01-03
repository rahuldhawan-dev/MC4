using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class EngineExcelRecord : EquipmentExcelRecordBase<EngineExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 154;
        public const int EQUIPMENT_PURPOSE = 345;

        public struct Characteristics
        {
            #region Constants

            public const int APPLICATION_ENG = 1075,
                             ENG_CYLINDERS = 1594,
                             ENG_FUEL_UOM = 1513,
                             ENG_MAX_FUEL = 1593,
                             ENG_TYP = 1457,
                             HP_RATING = 851,
                             OWNED_BY = 1592,
                             SELF_STARTING = 1272,
                             SPECIAL_MAINT_NOTES_DIST = 1596,
                             NARUC_MAINTENANCE_ACCOUNT = 2074,
                             NARUC_OPERATIONS_ACCOUNT = 2075,
                             TNK_VOLUME = 1595;

            #endregion
        }

        #endregion

        #region Properties

        public string Application { get; set; }
        public string EngineType { get; set; }
        public string FuelUOM { get; set; }
        public string HPRating { get; set; }
        public string MaxFuelConsumption { get; set; }
        public string NumberofCylinders { get; set; }
        public string OwnedBy { get; set; }
        public string SelfStarting { get; set; }
        public string SpecialMtnNote { get; set; }
        public string SpecialMtnNoteDet { get; set; }
        public string TnkVolumegal { get; set; }
        public string NARUCMaintenanceAc { get; set; }
        public string NARUCOperationsAcc { get; set; }

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2340;
        protected override int NARUCSpecialMtnNoteDetailsId => 2341;

        protected override string EquipmentType => "ENGINE";

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(
            EquipmentCharacteristicMapper mapper)
        {
            return new [] {
                mapper.DropDown(Application, nameof(Application), Characteristics.APPLICATION_ENG),
                mapper.DropDown(EngineType, nameof(EngineType), Characteristics.ENG_TYP),
                mapper.DropDown(FuelUOM, nameof(FuelUOM), Characteristics.ENG_FUEL_UOM),
                mapper.Numerical(HPRating, nameof(HPRating), Characteristics.HP_RATING),
                mapper.Numerical(MaxFuelConsumption, nameof(MaxFuelConsumption), Characteristics.ENG_MAX_FUEL),
                mapper.Numerical(NumberofCylinders, nameof(NumberofCylinders), Characteristics.ENG_CYLINDERS),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(SelfStarting, nameof(SelfStarting), Characteristics.SELF_STARTING),
                mapper.Numerical(TnkVolumegal, nameof(TnkVolumegal), Characteristics.TNK_VOLUME),
                mapper.DropDown(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES_DIST),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}