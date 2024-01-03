using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Update;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallImporter.Tests.Models.Update
{
    [TestClass]
    public class ValveInspectionFrequenciesExcelRecordTest : ExcelRecordTestBase<Valve, MyEditValve, ValveInspectionFrequenciesExcelRecord>
    {
        #region Constants

        public const int VALVE_ID = 752600;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateValvesForInspectionFrequenciesInAberdeenNJ(_container);
        }

        #endregion

        protected override ValveInspectionFrequenciesExcelRecord CreateTarget()
        {
            return new ValveInspectionFrequenciesExcelRecord { Id = VALVE_ID };
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<Valve, MyEditValve, ValveInspectionFrequenciesExcelRecord> test)
        {
            test.Int(x => x.InspectionFrequency, x => x.InspectionFrequency);

            test.EntityRef(x => x.FrequencyUnitId, x => x.InspectionFrequencyUnit);
            test.EntityRef(x => x.ValveZone, x => x.ValveZone);

            test.TestedElsewhere(x => x.Id);
        }

        [TestMethod]
        public void TestRecordIsLookedUpById()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(VALVE_ID, _target.MapToEntity(uow, 2, MappingHelper).Id);
            });
        }
    }
}
