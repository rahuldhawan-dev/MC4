using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class TelephoneExcelRecordTest : EquipmentExcelRecordTestBase<TelephoneExcelRecord>
    {
        #region Private Methods

        protected override TelephoneExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            return ret;
        }

        #endregion

        #region Properties

        protected override string ExpectedIdentifier => "NJ7-10-TPNG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<TelephoneExcelRecord> test)
        {
            test.Currency(x => x.AnnualFee, "ANNUAL_COST");
            test.DropDown(x => x.AntennaType, "ANTENNA_TP");
            test.DropDown(x => x.Application, "APPLICATION_COMM-TEL");
            test.DropDown(x => x.BaudRate, "BAUD_RATE");
            test.DropDown(x => x.CommunicationTeleph, "COMM-TEL_TYP");
            test.DropDown(x => x.CommunicationType, "COMMUNICATION_TP");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.Powerwatts, "POWER (WATTS)");
            test.DropDown(x => x.StandbyPowerType, "STANDBY_POWER_TP");
            test.String(x => x.NARUCMaintenanceAccount, "NARUC_MAINTENANCE_ACCOUNT");
            test.String(x => x.NARUCOperationsAccount, "NARUC_OPERATIONS_ACCOUNT");

            test.NotMapped(x => x.SpecialMtnNote);
            test.NotMapped(x => x.SpecialMtnNoteDet);
        }

        [TestMethod]
        public override void TestMappings()
        {
            base.TestMappings();
        }

        [TestMethod]
        public void TestLockoutRequiredPrerequisiteIsNotAdded()
        {
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, _mappingHelper);

                MyAssert.DoesNotContain(result.ProductionPrerequisites,
                    pp => pp.Id == ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT);
            });
        }

        #endregion
    }
}
