using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import.Equipment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallImporter.Tests.Models.Import.Equipment
{
    [TestClass]
    public class SCADARadioExcelRecordTest : EquipmentExcelRecordTestBase<SCADARadioExcelRecord>
    {
        #region Private Methods

        protected override SCADARadioExcelRecord CreateTarget()
        {
            var ret = base.CreateTarget();

            ret.AntennaType = "YAGI";
            ret.Application = "CONTROL AND DAQ";
            ret.BaudRate = "2400";
            ret.CommunicationRadio = "COMM-RAD*";
            ret.CommunicationType = "NONE";
            ret.OwnedBy = "AW";
            ret.Powerwatts = "1.21 giga";
            ret.StandbyPowerType = "NONE";

            return ret;
        }

        #endregion

        #region Private Properties

        protected override string ExpectedIdentifier => "NJ7-10-SRDG-1";

        #endregion

        #region Mapping

        protected override void TestCharacteristicMappings(
            EquipmentCharacteristicMappingTester<SCADARadioExcelRecord> test)
        {
            test.Currency(x => x.AnnualFee, "ANNUAL_COST");
            test.DropDown(x => x.AntennaType, "ANTENNA_TP");
            test.DropDown(x => x.Application, "APPLICATION_COMM-RAD");
            test.DropDown(x => x.BaudRate, "BAUD_RATE");
            test.DropDown(x => x.CommunicationRadio, "COMM-RAD_TYP");
            test.DropDown(x => x.CommunicationType, "COMMUNICATION_TP");
            test.String(x => x.FCCLicense, "FCC_LICENSE#");
            test.String(x => x.OwnedBy, "OWNED_BY");
            test.String(x => x.Powerwatts, "POWER_WATTS");
            test.String(x => x.ReceiveFrequency, "RECEIVE_FREQUENCY");
            test.DropDown(x => x.StandbyPowerType, "STANDBY_POWER_TP");
            test.String(x => x.TransmitFrequency, "TRANSMIT_FREQUENCY");
            test.String(x => x.SpecialMtnNote, "SPECIAL_MAINT_NOTES");
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