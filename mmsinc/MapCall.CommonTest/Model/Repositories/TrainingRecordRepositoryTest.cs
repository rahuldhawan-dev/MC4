using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using NHibernate.Linq;
using StructureMap;
using System.Linq;
using DataType = MapCall.Common.Model.Entities.DataType;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        TrainingRecordRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<TrainingRecord, TrainingRecordRepository>
    {
        #region Init/Cleanup

        private DateTime _now;

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);

            i.For<IDateTimeProvider>().Use(new TestDateTimeProvider(_now = DateTime.Now));
        }

        #endregion

        #region Delete

        [TestMethod]
        public void TestDeleteCascadesDeletesForLinkedEmployees()
        {
            var dataType1 = GetEntityFactory<DataType>().Create(new {
                TableName = TrainingRecordMap.TABLE_NAME, Name = TrainingRecord.DataTypeNames.EMPLOYEES_ATTENDED
            });
            var dataType2 = GetEntityFactory<DataType>().Create(new {
                TableName = TrainingRecordMap.TABLE_NAME, Name = TrainingRecord.DataTypeNames.EMPLOYEES_SCHEDULED
            });
            var employees = GetEntityFactory<Employee>().CreateArray(2);
            var trainingRecord = GetEntityFactory<TrainingRecord>().Create();
            var employeeLink1 = GetEntityFactory<EmployeeLink>().Create(new {
                Employee = employees[0],
                LinkedId = trainingRecord.Id,
                DataType = dataType1
            });
            var employeeLink2 = GetEntityFactory<EmployeeLink>().Create(new {
                Employee = employees[1],
                LinkedId = trainingRecord.Id,
                DataType = dataType2
            });
            Session.Flush();
            Session.Clear();
            trainingRecord = Repository.Find(trainingRecord.Id);
            //Sanity Check. Make sure it's adding the employee record.
            Assert.AreEqual(trainingRecord.LinkedEmployeesAttended.First().Employee.Id, employees[0].Id);
            Assert.AreEqual(trainingRecord.LinkedEmployeesScheduled.First().Employee.Id, employees[1].Id);

            Repository.Delete(trainingRecord);

            employeeLink1 = Session.Query<EmployeeLink>().SingleOrDefault(x => x.Id == employeeLink1.Id);

            Assert.IsNull(employeeLink1);

            employeeLink2 = Session.Query<EmployeeLink>().SingleOrDefault(x => x.Id == employeeLink2.Id);

            Assert.IsNull(employeeLink2);
        }

        #endregion

        #region GetTotalTrainingHours

        [TestMethod]
        public void TestGetTotalTrainingHoursSetsFieldsFromThings()
        {
            var record = GetEntityFactory<TrainingRecord>().Create(new {
                HeldOn = _now.AddDays(-1)
            });
            var attended = GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new {
                LinkedId = record.Id
            });
            var scheduled = GetFactory<TrainingRecordScheduledEmployeeFactory>().Create(new {
                LinkedId = record.Id,
                Employee = attended.Employee
            });
            GetEntityFactory<TrainingSession>().Create(new {
                StartDateTime = _now,
                EndDateTime = _now.AddHours(1),
                TrainingRecord = record
            });
            record.TrainingModule.TrainingRequirement.PositionGroupCommonNames.Add(attended.Employee.PositionGroup
               .CommonName);
            Session.Save(record.TrainingModule.TrainingRequirement);
            Session.Flush();
            Session.Clear();

            var results = Repository.GetTotalTrainingHours(new TestSearchTrainingTotalHours());
            var result = results.Single();

            Assert.AreEqual(attended.Employee.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(record.TrainingModule.TrainingRequirement.IsOSHARequirement, result.OSHARequirement);
            Assert.AreEqual(record.TrainingModule.AmericanWaterCourseNumber, result.ClassId);
            Assert.AreEqual(record.TrainingModule.Title, result.TrainingModule);
            Assert.AreEqual(attended.Employee.PositionGroup.CommonName.Description, result.CommonName);
            Assert.AreEqual(attended.Employee.PositionGroup.PositionDescription, result.Position);
            Assert.AreEqual(attended.Employee.PositionGroup.Group, result.PositionGroup);
            Assert.AreEqual(_now.Year, result.Year);
            Assert.AreEqual(1, result.TotalEmployeesAttended);
            Assert.AreEqual(1, result.TotalEmployeesScheduled);
            Assert.AreEqual(1, result.TotalHours);
        }

        [TestMethod]
        public void TestGetTotalTrainingHoursOnlyGetsRecordsFromSearchedYear()
        {
            var lastYear = _now.AddYears(-2);

            // this year record
            var thisYearRecord = GetEntityFactory<TrainingRecord>().Create(new {
                HeldOn = _now.AddDays(-1)
            });
            var attendedThisYear = GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new {
                LinkedId = thisYearRecord.Id
            });
            GetFactory<TrainingRecordScheduledEmployeeFactory>().Create(new {
                LinkedId = thisYearRecord.Id,
                Employee = attendedThisYear.Employee
            });
            GetEntityFactory<TrainingSession>().Create(new {
                StartDateTime = _now,
                EndDateTime = _now.AddHours(1),
                TrainingRecord = thisYearRecord
            });
            thisYearRecord.TrainingModule.TrainingRequirement.PositionGroupCommonNames.Add(attendedThisYear.Employee
               .PositionGroup.CommonName);
            Session.Save(thisYearRecord.TrainingModule.TrainingRequirement);

            // last year record
            var lastYearRecord = GetEntityFactory<TrainingRecord>().Create(new {
                HeldOn = lastYear
            });
            var attendedlastYear = GetFactory<TrainingRecordAttendedEmployeeFactory>().Create(new {
                LinkedId = lastYearRecord.Id,
                Employee = attendedThisYear.Employee
            });
            GetFactory<TrainingRecordScheduledEmployeeFactory>().Create(new {
                LinkedId = lastYearRecord.Id,
                Employee = attendedlastYear.Employee
            });
            GetEntityFactory<TrainingSession>().Create(new {
                StartDateTime = lastYear,
                EndDateTime = lastYear.AddHours(1),
                TrainingRecord = lastYearRecord
            });
            lastYearRecord.TrainingModule.TrainingRequirement.PositionGroupCommonNames.Add(attendedlastYear.Employee
               .PositionGroup.CommonName);
            Session.Save(lastYearRecord.TrainingModule.TrainingRequirement);

            Session.Flush();
            Session.Clear();

            var results = Repository.GetTotalTrainingHours(new TestSearchTrainingTotalHours {
                Year = _now.Year
            });
            var result = results.Single();

            Assert.AreEqual(1, result.TotalEmployeesAttended);
            Assert.AreEqual(1, result.TotalEmployeesScheduled);
            Assert.AreEqual(1, result.TotalHours);
        }

        #endregion

        private class TestSearchTrainingTotalHours : SearchSet<TrainingTotalHoursReportItem>, ISearchTrainingTotalHours
        {
            [Search(CanMap = false)]
            public int? Year { get; set; }
        }
    }
}
