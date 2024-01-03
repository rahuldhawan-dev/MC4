using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class PowerDisconnectExcelRecordTest : EquipmentExcelRecordTestBase<PowerDisconnectExcelRecord>
    {
        #region Private Methods

        protected override PowerDisconnectExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.AmpRating = "100.00 meh";
            ret.CurrentRating = "100.00 meh";
            ret.OwnedBy = "AW";
            ret.PowerDisconnectTyp = "LOADBREAK DISCONNECT";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-PDCG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<PowerDisconnectExcelRecord> test)
        {
            test.Numerical(x => x.AmpRating, "AMP_RATING");
            test.Numerical(x => x.CurrentRating, "AMP_CURRRATING");
            test.String(x => x.FuseSize, "FUSE_SIZE");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.DropDown(x => x.PowerDisconnectTyp, "PWRDISC_TYP");
            test.DropDown(x => x.VoltRating, "VOLT_RATING");
            test.String(x => x.NARUCMaintenanceAc, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAcc, "NARUC_OPERATIONS_ACCOUNT");

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