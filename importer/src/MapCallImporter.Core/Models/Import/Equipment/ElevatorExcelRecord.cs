using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class ElevatorExcelRecord : EquipmentExcelRecordBase<ElevatorExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 152,
                         EQUIPMENT_PURPOSE = 342;

        public struct Characteristics
        {
            public const int CAPACITY_RATING = 1901,
                             CAPACITY_UOM_E = 1402,
                             ELEVATOR_TYP = 1559,
                             NARUC_MAINTENANCE_ACCOUNT = 2070,
                             NARUC_OPERATIONS_ACCOUNT = 2071,
                             OWNED_BY = 1900,
                             RETEST_REQUIRED = 1483;
        }

        #endregion

        #region Properties

public string CapacityRating {get;set;}
public string CapacityUOM {get;set;}
public string ElevatorType {get;set;}
public string OwnedBy {get;set;}
public string RetestRequired {get;set;}
public string SpecialMtnNote {get;set;}
public string SpecialMtnNoteDet {get;set;}
public string NARUCMaintenanceAc {get;set;}
public string NARUCOperationsAcc {get;set;}

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "ELEVATOR";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2334;
        protected override int NARUCSpecialMtnNoteDetailsId => 2335;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.String(CapacityRating, nameof(CapacityRating), Characteristics.CAPACITY_RATING),
                mapper.DropDown(CapacityUOM, nameof(CapacityUOM), Characteristics.CAPACITY_UOM_E),
                mapper.DropDown(ElevatorType, nameof(ElevatorType), Characteristics.ELEVATOR_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}