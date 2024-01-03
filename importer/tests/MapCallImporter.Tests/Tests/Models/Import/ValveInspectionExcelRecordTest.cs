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
    public class ValveInspectionExcelRecordTest : ExcelRecordTestBase<ValveInspection, MyCreateValveInspection, ValveInspectionExcelRecord>
    {
        protected override ValveInspectionExcelRecord CreateTarget()
        {
            return new ValveInspectionExcelRecord {
                SAPEquipmentNumber = "10278997",
                OperatingCenterId = 10,
                InspectedBy = "99999999",
                DateInspected = _now,
                NumberOfTurnsCompleted = 1
            };
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<ValveInspection, MyCreateValveInspection, ValveInspectionExcelRecord> test)
        {
            test.RequiredString(v => v.SAPEquipmentNumber,
                v => v.Valve.SAPEquipmentId);
            test.RequiredDateTime(v => v.DateInspected, v => v.DateInspected);

            test.String(v => v.Notes, v => v.Remarks);
            test.String(v => v.Notification, v => v.SAPNotificationNumber);
            test.EntityRef(v => v.PositionFound, v => v.PositionFound);
            test.EntityRef(v => v.PositionLeft, v => v.PositionLeft);

            test.TestedElsewhere(v => v.AcceptevenifMinReqTurnsnotcompleted);
            test.TestedElsewhere(v => v.NumberOfTurnsCompleted);
            test.TestedElsewhere(v => v.Inspected);
            test.TestedElsewhere(v => v.InspectedBy);

            test.NotMapped(v => v.OperatingCenterId);
        }

        [TestMethod]
        public void TestTurnsNotCompletedMustBeTrueIfNumberOfTurnsCompletedLessThanRequiredForValve()
        {
            decimal valveRequiredTurns = 10;

            WithUnitOfWork(uow => {
                var valve = _target.MapToEntity(uow, 1, MappingHelper).Valve;

                valve.Turns = valveRequiredTurns;
                valveRequiredTurns = valve.MinimumRequiredTurns;
                uow.Update(valve);

                _target.NumberOfTurnsCompleted = 0;
                _target.AcceptevenifMinReqTurnsnotcompleted = null;

                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));

                _target.AcceptevenifMinReqTurnsnotcompleted = true;

                Assert.AreEqual(true, _target.MapToEntity(uow, 1, MappingHelper).TurnsNotCompleted);

                _target.AcceptevenifMinReqTurnsnotcompleted = false;
                _target.NumberOfTurnsCompleted = valveRequiredTurns;

                Assert.AreEqual(valveRequiredTurns, _target.MapToEntity(uow, 1, MappingHelper).Turns);
            });
        }

        [TestMethod]
        public void TestOperatedIsSetToTrueIfProvidedValueIsYes()
        {
            _target.Inspected = "yes";
            _target.PositionFound = 1;
            _target.PositionLeft = 1;

            WithUnitOfWork(uow => Assert.IsTrue(_target.MapToEntity(uow, 1, MappingHelper).Inspected));
        }

        [TestMethod]
        public void TestOperatedIsSetToFalseIfProvidedValueIsNotYes()
        {
            foreach (var value in new[] {null, "no", "", "not yes"})
            {
                _target.Inspected = value;

                WithUnitOfWork(uow => Assert.IsFalse(_target.MapToEntity(uow, 1, MappingHelper).Inspected));
            }
        }

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

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateStuffForValvesInAberdeenNJ(_container);
            TestDataHelper.CreateValidValvesLikeTheValidValvesInTheValidValvesFile(_container);
        }

        #endregion
    }
}