using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Update;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallImporter.Tests.Models.Update
{
    [TestClass]
    public class ServiceBlockZonesExcelRecordTest : ExcelRecordTestBase<Service, MyEditService, ServiceBlockZonesExcelRecord>
    {
        #region Constants

        public const int SERVICE_ID = 1;
        public const string BLOCK = "6";

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateSomeServicesInAberdeenNJ(_container);
        }

        #endregion

        protected override ServiceBlockZonesExcelRecord CreateTarget()
        {
            return new ServiceBlockZonesExcelRecord { Id = SERVICE_ID };
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<Service, MyEditService, ServiceBlockZonesExcelRecord> test)
        {
            test.String(x => x.Block, x => x.Block);

            test.TestedElsewhere(x => x.Id);
        }

        [TestMethod]
        public void TestRecordIsLookedUpById()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(SERVICE_ID, _target.MapToEntity(uow, 2, MappingHelper).Id);
            });
        }

        [TestMethod]
        public void TestThrowsWhenServiceCannotBeFoundInDatabase()
        {
            _target.Id = 99999999;

            WithUnitOfWork(uow => { ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)); });
        }
    }
}
