using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallImporter.Tests.Models.Import
{
    [TestClass]
    public class HydrantInspectionExcelRecordTest : ExcelRecordTestBase<HydrantInspection, MyCreateHydrantInspection, HydrantInspectionExcelRecord>
    {
        protected override HydrantInspectionExcelRecord CreateTarget()
        {
            return new HydrantInspectionExcelRecord {
                SAPEquipmentNumber = "20072439",
                InspectionType = 2,
                DateInspected = _now,
                InspectedBy = "99999999"
            };
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<HydrantInspection, MyCreateHydrantInspection, HydrantInspectionExcelRecord> test)
        {
            test.RequiredString(h => h.SAPEquipmentNumber,
                h => h.Hydrant.SAPEquipmentId);
            test.RequiredEntityRef(h => h.InspectionType, h => h.HydrantInspectionType);
            test.RequiredDateTime(h => h.DateInspected, h => h.DateInspected);

            test.Decimal(h => h.GPM, h => h.GPM);
            test.Decimal(h => h.MinutesFlowed, h => h.MinutesFlowed);
            test.String(h => h.Notes, h => h.Remarks);
            test.String(h => h.NotificationNumber, h => h.SAPNotificationNumber);
            test.Decimal(h => h.StaticPressure, h => h.StaticPressure);

            test.TestedElsewhere(h => h.InspectedBy);
        }

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateStuffForHydrantsInAberdeenNJ(_container);
            TestDataHelper.CreateValidHydrantsLikeTheValidHydrantsInTheValidHydrantsFile(_container);
        }

        #endregion

        [TestMethod]
        public void TestInspectedByMapsToUser()
        {
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, MappingHelper).InspectedBy;
                var employee = uow.Find<Employee>(result.Employee.Id);

                Assert.AreEqual(_target.InspectedBy, employee.EmployeeId);
            });
        }

        [TestMethod]
        public void TestSAPErrorCodeIsSet()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(HydrantExcelRecord.SAP_RETRY_ERROR_CODE,
                    _target.MapToEntity(uow, 1, MappingHelper).SAPErrorCode);
            });
        }
    }
}