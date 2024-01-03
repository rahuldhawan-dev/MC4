﻿using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class ChemicalTankExcelRecordTest : EquipmentExcelRecordTestBase<ChemicalTankExcelRecord>
    {
        #region Private Methods

        protected override ChemicalTankExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.OwnedBy = "AW";
            ret.TankChemical = "NON-PRESSURIZED CHEM";
            ret.TnkAutoRefill = "Y";
            ret.TnkMaterial = "STEEL";
            ret.TnkPressureRating = "VACUUM";
            ret.Underground = "Y";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-CTKG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<ChemicalTankExcelRecord> test)
        {
            test.DropDown(x => x.Application, "APPLICATION_TNK-CHEM");
            test.DropDown(x => x.IndoorOutdoor, "LOCATION");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.TankChemical, "TNK-CHEM_TYP");
            test.DropDown(x => x.TnkAutoRefill, "TNK_AUTO_REFILL");
            test.Numerical(x => x.TnkDiameter, "TNK_DIAMETER");
            test.DropDown(x => x.TnkMaterial, "TNK_MATERIAL");
            test.DropDown(x => x.TnkPressureRating, "TNK_PRESSURE_RATING");
            test.Numerical(x => x.TnkSideLengthft, "TNK_SIDE_LENGTH");
            test.DropDown(x => x.TnkStateInspection, "TNK_STATE_INSPECTION_REQ");
            test.Numerical(x => x.TnkVolumegal, "TNK_VOLUME");
            test.DropDown(x => x.Underground, "UNDERGROUND");
            test.DropDown(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES_DIST");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

            test.NotMapped(x => x.SpecialMtnNoteDet);
        }

        [TestMethod]
        public override void TestMappings()
        {
            base.TestMappings();
        }

        [TestMethod]
        public void TestLockoutRequiredPrerequisiteIsAdded()
        {
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, _mappingHelper);

                MyAssert.Contains(result.ProductionPrerequisites,
                    pp => pp.Id == ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT);
            });
        }

        #endregion
    }
}