using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class UVSanitizerExcelRecordTest : EquipmentExcelRecordTestBase<UVSanitizerExcelRecord>
    {
        #region Private Methods

        protected override UVSanitizerExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-UVSG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<UVSanitizerExcelRecord> test)
        {
            test.DropDown(x => x.UVSystemType, "TRT-UV_TYP");

            test.NotMapped(x => x.NARUCMaintenanceAc);
            test.NotMapped(x => x.NARUCOperationsAcc);
            test.NotMapped(x => x.NumberofLampsModu);
            test.NotMapped(x => x.NumberofUVModules);
            test.NotMapped(x => x.OwnedBy);
            test.NotMapped(x => x.PeakProcessFlowg);
            test.NotMapped(x => x.RetentionTimeseco);
            test.NotMapped(x => x.SpecialMtnNote);
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