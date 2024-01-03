using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class CrewAssignmentTest : InMemoryDatabaseTest<CrewAssignment>
    {
        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {

            _user = GetFactory<ContractorUserFactory>().Create();
            _crew = GetFactory<CrewFactory>().Create(new {Contractor = _user.Contractor});
        }

        #endregion

        #region Fields

        private ContractorUser _user;
        private Crew _crew;

        #endregion

        #region Other

        #region IsOpen

        [TestMethod]
        public void TestIsOpenReturnsFalseWhenStartDateAndEndDateAreBothSet()
        {
            var target = new CrewAssignment {
                DateStarted = DateTime.Now.AddDays(-1),
                DateEnded = DateTime.Now
            };

            Assert.IsFalse(target.IsOpen);
        }

        [TestMethod]
        public void TestIsOpenReturnsFalseWhenStartDateAndEndDateAreBothUnset()
        {
            var target = new CrewAssignment {
                DateStarted = DateTime.Now.AddDays(-1),
                DateEnded = DateTime.Now
            };

            Assert.IsFalse(target.IsOpen);
        }

        [TestMethod]
        public void TestIsOpenReturnsTrueWhenStartDateIsSetButNotEndDate()
        {
            var target = new CrewAssignment {
                DateStarted = DateTime.Now.AddDays(-1)
            };

            Assert.IsTrue(target.IsOpen);
        }

        #endregion

        #region TimeToComplete

        [TestMethod]
        public void TestTimeToCompleteReturnsDifferenceBetweenStartAndEndTimesIfSet()
        {
            var start = DateTime.Now;
            var end = start.AddHours(5).AddMinutes(30);
            var expected = end - start;
            var target = new CrewAssignment {
                DateStarted = start,
                DateEnded = end
            };

            Assert.AreEqual(expected, target.TimeToComplete);
        }

        [TestMethod]
        public void TestTimeToCompleteReturnsNullIfStartOrEndTimeNotSet()
        {
            var target = new CrewAssignment();

            Assert.IsNull(target.TimeToComplete);
        }

        #endregion

        #region TotalManHours

        [TestMethod]
        public void TestTotalManHoursReturnsNullIfDateStartedIsNull()
        {
            var target = new CrewAssignment {
                DateEnded = DateTime.Now,
                EmployeesOnJob = 2
            };

            Assert.IsNull(target.TotalManHours);
        }

        [TestMethod]
        public void TestTotalManHoursReturnsNullIfDateEndedIsNull()
        {
            var target = new CrewAssignment {
                DateStarted = DateTime.Now,
                EmployeesOnJob = 2
            };

            Assert.IsNull(target.TotalManHours);
        }

        [TestMethod]
        public void TestTotalManHoursReturnsNullIfEmployeesOnJobIsNull()
        {
            var target = new CrewAssignment {
                DateStarted = DateTime.Now.AddHours(-1),
                DateEnded = DateTime.Now
            };

            Assert.IsNull(target.TotalManHours);
        }

        [TestMethod]
        public void TestTotalManHoursReturnsTheNumberOfHoursBetweenStartAndEndMultipliedByNumberOfEmployeesOnJob()
        {
            var target = new CrewAssignment {
                DateStarted = DateTime.Now.AddHours(-1),
                DateEnded = DateTime.Now,
                EmployeesOnJob = 1
            };

            Assert.AreEqual(1, (int)target.TotalManHours.Value);

            target.EmployeesOnJob = 2;

            Assert.AreEqual(2, (int)target.TotalManHours.Value);

            target.DateEnded = DateTime.Now.AddHours(1);

            Assert.AreEqual(4, (int)target.TotalManHours.Value);
        }

        #endregion

        #endregion
    }
}
