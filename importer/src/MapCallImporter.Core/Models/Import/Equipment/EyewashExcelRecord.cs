using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCallImporter.Models.Import.Equipment
{
    public class EyewashExcelRecord : EquipmentExcelRecordBase<EyewashExcelRecord>
    {
        #region Constants

        public const int EQUIPMENT_TYPE = 155,
                         EQUIPMENT_PURPOSE = 346;

        public struct Characteristics
        {
            public const int EYEWASH_TYP = 957,
                             NARUC_MAINTENANCE_ACCOUNT = 2076,
                             NARUC_OPERATIONS_ACCOUNT = 2077,
                             OWNED_BY = 1730,
                             RETEST_REQUIRED = 890,
                             SPECIAL_MAINT_NOTES = 1731;
        }

        #endregion

        #region Properties

public string EyewashType {get;set;}
public string OwnedBy {get;set;}
public string RetestRequired {get;set;}
public string SpecialMtnNote {get;set;}
public string SpecialMtnNoteDet {get;set;}
public string NARUCMaintenanceAc {get;set;}
public string NARUCOperationsAcc {get;set;}

        protected override int EquipmentTypeId => EQUIPMENT_TYPE;

        protected override string EquipmentType => "EYEWASH";

        protected override int EquipmentPurposeId => EQUIPMENT_PURPOSE;
        protected override int NARUCSpecialMtnNotesId => 2342;
        protected override int NARUCSpecialMtnNoteDetailsId => 2343;

        #endregion

        #region Private Methods

        protected override IEnumerable<EquipmentCharacteristic> InnerMapCharacteristics(EquipmentCharacteristicMapper mapper)
        {
            return new[] {
                mapper.DropDown(EyewashType, nameof(EyewashType), Characteristics.EYEWASH_TYP),
                mapper.String(OwnedBy, nameof(OwnedBy), Characteristics.OWNED_BY),
                mapper.DropDown(RetestRequired, nameof(RetestRequired), Characteristics.RETEST_REQUIRED),
                mapper.String(SpecialMtnNote, nameof(SpecialMtnNote), Characteristics.SPECIAL_MAINT_NOTES),
                mapper.String(NARUCMaintenanceAc, nameof(NARUCMaintenanceAc), Characteristics.NARUC_MAINTENANCE_ACCOUNT),
                mapper.String(NARUCOperationsAcc, nameof(NARUCOperationsAcc), Characteristics.NARUC_OPERATIONS_ACCOUNT),
            };
        }

        #endregion
    }
}