﻿using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class HVACCombinationUnitExcelRecordTest : EquipmentExcelRecordTestBase<HVACCombinationUnitExcelRecord>
    {
        #region Private Methods

        protected override HVACCombinationUnitExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-HCUG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<HVACCombinationUnitExcelRecord> test)
        {
            test.Numerical(x => x.AmpRating, "AMP_RATING");
            test.DropDown(x => x.Application, "APPLICATION_HVAC-CMB");
            test.DropDown(x => x.DutyCycle, "DUTY_CYCLE");
            test.DropDown(x => x.EnergyType, "ENERGY_TP");
            test.DropDown(x => x.HVACCombination, "HVAC-CMB_TYP");
            test.DropDown(x => x.OutputUOM, "OUTPUT_UOM");
            test.String(x => x.OutputValue, "OUTPUT_VALUE");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES");
            test.DropDown(x => x.VoltRating, "VOLT_RATING");
            test.String(x => x.NARUCMaintenanceAccount, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAccount, "NARUC_OPERATIONS_ACCOUNT");

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
