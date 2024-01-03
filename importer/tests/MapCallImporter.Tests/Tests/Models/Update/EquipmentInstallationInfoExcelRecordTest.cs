using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Update;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallImporter.Tests.Models.Update
{
    [TestClass]
    public class EquipmentInstallationInfoExcelRecordTest : ExcelRecordTestBase<Equipment, MyEditEquipment, EquipmentInstallationInfoExcelRecord>
    {
        #region Constants

        public const int SAP_EQUIPMENT_ID = 5017540;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateSomeAdjustableSpeedDrivesInAberdeenNJ(_container);
        }

        #endregion

        protected override EquipmentInstallationInfoExcelRecord CreateTarget()
        {
            return new EquipmentInstallationInfoExcelRecord { SAPEquipmentId = SAP_EQUIPMENT_ID };
        }

        [TestMethod]
        public void TestRecordIsLookedUpBySAPEquipmentId()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(SAP_EQUIPMENT_ID, _target.MapToEntity(uow, 2, MappingHelper).SAPEquipmentId);
            });
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<Equipment, MyEditEquipment, EquipmentInstallationInfoExcelRecord> test)
        {
            test.DateTime(x => x.DateInstalled, x => x.DateInstalled);

            //Date retired is conditionally set based on the equipment status field, that property is tested elsewhere for the applicable conditions, need to skip testing it here
            test.TestedElsewhere(x => x.DateRetired);
            test.TestedElsewhere(x => x.SAPEquipmentId);
            test.TestedElsewhere(x => x.ScadaTagName);

            test.NotMapped(x => x.Description);
        }

        [TestMethod]
        public void TestScadaTagNameIsLookedUp()
        {
            var id = 666;
            var tagName = id.ToString();

            _target.ScadaTagName = tagName;

            WithUnitOfWork(uow => {
                uow.SqlQuery($"INSERT INTO ScadaTagNames (Id, TagName) VALUES ({id}, '{tagName}');").ExecuteUpdate();

                Assert.AreEqual(id, _target.MapToEntity(uow, 2, MappingHelper).ScadaTagName.Id);
            });
        }

        [TestMethod]
        public void TestDoesRetryThingy()
        {
            WithUnitOfWork(uow => Assert.AreEqual("RETRY::",
                _target.MapToEntity(uow, 1, MappingHelper).SAPErrorCode));
        }
    }
}