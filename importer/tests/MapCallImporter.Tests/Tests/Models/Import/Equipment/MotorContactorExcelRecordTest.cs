﻿using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class MotorContactorExcelRecordTest : EquipmentExcelRecordTestBase<MotorContactorExcelRecord>
    {
        #region Private Methods

        protected override MotorContactorExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-MTCG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<MotorContactorExcelRecord> test)
        {
            test.Numerical(x => x.AmpRating, "AMP_RATING");
            test.DropDown(x => x.ContactorType, "CONTACTR_TYP");
            test.String(x => x.InterruptingCapacit, "INTERRUPTING_CAPACITY");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES");
            test.DropDown(x => x.VoltRating, "VOLT_RATING");
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
