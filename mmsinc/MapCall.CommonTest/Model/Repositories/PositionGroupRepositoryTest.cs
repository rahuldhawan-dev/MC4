using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        PositionGroupRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<PositionGroup, PositionGroupRepository>
    {
        #region Setup/Teardown

        private DateTime _now;

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Use(new TestDateTimeProvider(_now = DateTime.Now));
        }

        #endregion

        #region GetTrainingClassificationSumReport

        [TestMethod]
        public void TestGetTrainingClassificationSumReport()
        {
            var position = GetEntityFactory<PositionGroup>().Create();
            var requirement = GetEntityFactory<TrainingRequirement>().Create();
            position.CommonName.TrainingRequirements.Add(requirement);
            Session.SaveOrUpdate(position.CommonName);
            var employee = GetEntityFactory<Employee>().Create(new {
                PositionGroup = position
            });
            var module = GetEntityFactory<TrainingModule>().Create(new {
                TrainingRequirement = requirement
            });
            var record = GetEntityFactory<TrainingRecord>().Create(new {
                TrainingModule = module,
                HeldOn = _now.AddWeeks(-1)
            });
            GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new {
                Employee = employee,
                LinkedId = record.Id
            });
            Session.Flush();
            Session.Clear();

            var results = Repository.GetTrainingClassificationSumReport(new TestSearchTrainingClassificationSum());
            var result = results.Single();

            Assert.AreEqual(1, result.NumberOfEmployees);
        }

        [TestMethod]
        public void TestGetTrainingClassificationSumReportFiltersByYearIfSent()
        {
            var position = GetEntityFactory<PositionGroup>().Create();
            var requirement = GetEntityFactory<TrainingRequirement>().Create();
            position.CommonName.TrainingRequirements.Add(requirement);
            Session.SaveOrUpdate(position.CommonName);
            var employee = GetEntityFactory<Employee>().Create(new {
                PositionGroup = position
            });
            var module = GetEntityFactory<TrainingModule>().Create(new {
                TrainingRequirement = requirement
            });
            var record0 = GetEntityFactory<TrainingRecord>().Create(new {
                TrainingModule = module,
                HeldOn = _now.AddMinutes(-1)
            });
            var record1 = GetEntityFactory<TrainingRecord>().Create(new {
                TrainingModule = module,
                HeldOn = _now.AddMinutes(-1).AddYears(-1)
            });
            GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new {
                Employee = employee,
                LinkedId = record0.Id
            });
            GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new {
                Employee = employee,
                LinkedId = record1.Id
            });
            Session.Flush();
            Session.Clear();

            var results =
                Repository.GetTrainingClassificationSumReport(
                    new TestSearchTrainingClassificationSum {Year = _now.Year});
            var result = results.Single();

            Assert.AreEqual(1, result.NumberOfEmployees);
        }

        #endregion

        #region Nested Type: TestSearchTrainingClassificationSum

        private class TestSearchTrainingClassificationSum : SearchSet<TrainingClassificationSumReportItem>,
            ISearchTrainingClassificationSum
        {
            [Search(CanMap = false)]
            public int? Year { get; set; }

            public int? OperatingCenter { get; set; }
            public bool? OSHARequirement { get; set; }
            public string ClassId { get; set; }
            public int? PositionGroupCommonName { get; set; }
            public int? TrainingModule { get; set; }
            public int? Id { get; set; }
        }

        #endregion
    }
}
