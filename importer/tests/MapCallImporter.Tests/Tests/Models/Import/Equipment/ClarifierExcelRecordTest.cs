using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class ClarifierExcelRecordTest : EquipmentExcelRecordTestBase<ClarifierExcelRecord>
    {
        #region Private Methods

        protected override ClarifierExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-CFRG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<ClarifierExcelRecord> test)
        {
            test.DropDown(x => x.ClarifierType, "TRT-CLAR_TYP");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.Application, "APPLICATION");
            test.String(x => x.SurfaceAreasqft, "SURFACE_AREA_SQFT");
            test.String(x => x.FlowNormalRange, "FLOW_NORMAL_RANGE");
            test.String(x => x.ContactTimeminute, "CONTACT_TIME_MINUTES");
            test.DropDown(x => x.IndoorOutdoor, "LOCATION");
            test.DropDown(x => x.MaterialofConstruc, "MATERIAL_OF_CONSTRUCTION");
            test.DropDown(x => x.AutoSludgeRemoval, "AUTO_REMOVAL");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES");
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