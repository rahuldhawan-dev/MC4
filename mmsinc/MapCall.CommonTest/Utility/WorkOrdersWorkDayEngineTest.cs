using System;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Utility
{
    [TestClass]
    public class WorkOrdersWorkDayEngineTest
    {
        [TestMethod]
        public void
            TestGetCallDateForRoutineMarkoutReturnsDateThreeWorkDaysInThePast()
        {
            var dateNeeded = new DateTime(2008, 11, 10);
            var expected = new DateTime(2008, 11, 3);
            var actual = WorkOrdersWorkDayEngine.GetCallDate(dateNeeded,
                MarkoutRequirementEnum.Routine);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetCallDateAroundIndependenceDay2011()
        {
            var dateNeeded = new DateTime(2011, 7, 8);
            var expected = new DateTime(2011, 7, 1);
            var actual = WorkOrdersWorkDayEngine.GetCallDate(dateNeeded,
                MarkoutRequirementEnum.Routine);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetReadyDateForRoutineMarkoutReturnsDateThreeWorkDaysInTheFuture()
        {
            // Monday, November 3, 2008
            var dateCalled = new DateTime(2008, 11, 3);
            // Monday, November 10, 2008
            var expected = new DateTime(2008, 11, 8);
            // this jumps over Election Day and a weekend
            var actual = WorkOrdersWorkDayEngine.GetReadyDate(dateCalled,
                MarkoutRequirementEnum.Routine);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetReadyDateForRoutineMarkoutReturnsSaturdayOfWeekIfDateIsTuesday()
        {
            // Tuesday, November 18, 2008
            var dateCalled = new DateTime(2008, 11, 18);
            // Saturday, November 22, 2008
            var expected = new DateTime(2008, 11, 22);
            // this does not jump any holidays
            var actual = WorkOrdersWorkDayEngine.GetReadyDate(dateCalled,
                MarkoutRequirementEnum.Routine);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void
            TestGetReadyDateForRoutineMarkoutObservesRegularWorkDayRulesIfDateIsTuesdayButThereIsSubsequentHoliday()
        {
            // Tuesday, April 7, 2008
            var dateCalled = new DateTime(2009, 4, 7);
            // Tuesday, April 14, 2008
            var expected = new DateTime(2009, 4, 14);
            // this jumps over Good Friday
            var actual = WorkOrdersWorkDayEngine.GetReadyDate(dateCalled,
                MarkoutRequirementEnum.Routine);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetExpirationDateForRoutineMarkoutWhenWorkWasNotStartedReturnsDateTenWorkDaysInTheFuture()
        {
            // Monday, November 3, 2008
            var dateCalled = new DateTime(2008, 11, 3);

            // moved back on 2011-04-05 to resolve an issue
            // Wednesday, November 19, 2008
            var expected = new DateTime(2008, 11, 19);
            // this jumps over Election Day, /* Armistice */ Veteran's Day,
            // and a couple of weekends
            var actual =
                WorkOrdersWorkDayEngine.GetExpirationDate(dateCalled,
                    MarkoutRequirementEnum.Routine, false);

            Assert.AreEqual(expected, actual);

            // Friday, March 25, 2011
            dateCalled = new DateTime(2011, 3, 25);
            // Friday, April 8, 2011
            expected = new DateTime(2011, 4, 8);

            actual = WorkOrdersWorkDayEngine.GetExpirationDate(dateCalled,
                MarkoutRequirementEnum.Routine, false);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetExpirationdateUsesMondayAsDateCalledWhenDateCalledIsSaturdayOrSunday()
        {
            // Sat, November 1, 2008
            var dateCalled = new DateTime(2008, 11, 1);

            // moved back on 2011-04-05 to resolve an issue
            // Wednesday, November 19, 2008
            var expected = new DateTime(2008, 11, 19);
            // this jumps over Election Day, /* Armistice */ Veteran's Day,
            // and a couple of weekends
            var actual =
                WorkOrdersWorkDayEngine.GetExpirationDate(dateCalled,
                    MarkoutRequirementEnum.Routine, false);

            Assert.AreEqual(expected, actual);

            // Sun, November 1, 2008
            dateCalled = new DateTime(2008, 11, 1);

            // this jumps over Election Day, /* Armistice */ Veteran's Day,
            // and a couple of weekends
            actual =
                WorkOrdersWorkDayEngine.GetExpirationDate(dateCalled,
                    MarkoutRequirementEnum.Routine, false);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetExpirationDateForRoutineMarkoutWhenWorkWasStarted()
        {
            // Monday, November 3, 2008
            var dateCalled = new DateTime(2008, 11, 3);

            // moved back on 2011-04-05 to resolve an issue
            // Monday, January 12, 2009
            var expected = new DateTime(2009, 1, 12);
            // this jumps over Election Day, /* Armistice */ Veteran's Day,
            // and a couple of weekends
            var actual =
                WorkOrdersWorkDayEngine.GetExpirationDate(dateCalled,
                    MarkoutRequirementEnum.Routine, true);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetExpirationDateDefaultsWorkStartedArgumentToFalse()
        {
            // Monday, November 3, 2008
            var dateCalled = new DateTime(2008, 11, 3);

            // moved back on 2011-04-05 to resolve an issue
            // Wednesday, November 19, 2008
            var expected = new DateTime(2008, 11, 19);
            // this jumps over Election Day, /* Armistice */ Veteran's Day,
            // and a couple of weekends
            var actual =
                WorkOrdersWorkDayEngine.GetExpirationDate(dateCalled,
                    MarkoutRequirementEnum.Routine);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetReadyDateForEmergencyMarkout()
        {
            // Monday, November 20, 2008
            var dateCalled = new DateTime(2008, 11, 20);

            // Thursday, November 20, 2008
            var expected = new DateTime(2008, 11, 20);

            var actual =
                WorkOrdersWorkDayEngine.GetReadyDate(dateCalled,
                    MarkoutRequirementEnum.Emergency);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetExpirationDateForEmergencyMarkout()
        {
            // Thursday, November 20, 2008
            var dateCalled = new DateTime(2008, 11, 20);

            // Thursday, November 20, 2008
            var expected = new DateTime(2008, 11, 20);

            var actual =
                WorkOrdersWorkDayEngine.GetExpirationDate(dateCalled,
                    MarkoutRequirementEnum.Emergency);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetReadyDateWhenNoMarkoutRequiredThrowsException()
        {
            // Monday, November 3, 2008
            var dateCalled = new DateTime(2008, 11, 3);
            DateTime? expected = null;
            var requirement = MarkoutRequirementEnum.None;

            MyAssert.Throws(
                () =>
                    expected =
                        WorkOrdersWorkDayEngine.GetReadyDate(dateCalled,
                            requirement), typeof(DomainLogicException));

            Assert.IsNull(expected);
        }

        [TestMethod]
        public void TestGetExpirationDateWhenNoMarkoutRequiredThrowsException()
        {
            // Monday, November 3, 2008
            var dateCalled = new DateTime(2008, 11, 3);
            DateTime? expected = null;
            var requirement = MarkoutRequirementEnum.None;

            MyAssert.Throws(
                () =>
                    expected =
                        WorkOrdersWorkDayEngine.GetExpirationDate(dateCalled,
                            requirement, false), typeof(DomainLogicException));

            MyAssert.Throws(
                () =>
                    expected =
                        WorkOrdersWorkDayEngine.GetExpirationDate(dateCalled,
                            requirement, true), typeof(DomainLogicException));

            Assert.IsNull(expected);
        }

        [TestMethod]
        public void TestGetReadyDateForRoutineMarkoutUsesMondayAsCallInDateWhenOnASaturdayOrSunday()
        {
            // Sat, June 5, 2010
            var dateCalled = new DateTime(2010, 6, 5);
            // Fri, June , 2010
            var expected = new DateTime(2010, 6, 11);

            var actual = WorkOrdersWorkDayEngine.GetReadyDate(dateCalled,
                MarkoutRequirementEnum.Routine);

            Assert.AreEqual(expected, actual);

            // Sun, June 6, 2010
            dateCalled = new DateTime(2010, 6, 6);

            actual = WorkOrdersWorkDayEngine.GetReadyDate(dateCalled,
                MarkoutRequirementEnum.Routine);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetExpirationDateAroundPresidentsDay2011()
        {
            var dateCalled = new DateTime(2011, 2, 13);
            var expected = new DateTime(2011, 2, 18);
            var requirement = MarkoutRequirementEnum.Routine;

            var actual = WorkOrdersWorkDayEngine.GetReadyDate(dateCalled,
                requirement);

            Assert.AreEqual(expected, actual);

            dateCalled = new DateTime(2011, 2, 20);
            expected = new DateTime(2011, 2, 26);
            requirement = MarkoutRequirementEnum.Routine;

            actual = WorkOrdersWorkDayEngine.GetReadyDate(dateCalled,
                requirement);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetExpirationDateAroundGoodFriday2011()
        {
            var dateCalled = new DateTime(2011, 4, 22);
            var expected = new DateTime(2011, 4, 29);
            var requirement = MarkoutRequirementEnum.Routine;

            var actual = WorkOrdersWorkDayEngine.GetReadyDate(dateCalled,
                requirement);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestReadyDateCanBeAHolidayOrWeekend()
        {
            // as per bug # 958
            var dateCalled = new DateTime(2011, 4, 18);
            var expected = new DateTime(2011, 4, 22);
            var requirement = MarkoutRequirementEnum.Routine;

            var actual = WorkOrdersWorkDayEngine.GetReadyDate(dateCalled,
                requirement);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetExpirationDateFromAugustFirst2011()
        {
            var dateCalled = new DateTime(2011, 8, 1);
            var expected = new DateTime(2011, 8, 15);
            var requirement = MarkoutRequirementEnum.Routine;

            var actual = WorkOrdersWorkDayEngine.GetExpirationDate(dateCalled,
                requirement);

            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class WorkOrdersWorkDayEngineConfigurationTest
    {
        #region Private Static Members

        private static WorkOrdersWorkDayEngineConfiguration _config;

        #endregion

        #region Static Properties

        private static WorkOrdersWorkDayEngineConfiguration configuration
        {
            get
            {
                if (_config == null)
                    _config = new WorkOrdersWorkDayEngineConfiguration();
                return _config;
            }
        }

        #endregion

        [TestMethod]
        public void TestUsesChristmas()
        {
            Assert.IsTrue(configuration.UseChristmas);
        }

        [TestMethod]
        public void TestUsesColumbusDay()
        {
            Assert.IsTrue(configuration.UseColumbusDay);
        }

        [TestMethod]
        public void TestUsesElectionDay()
        {
            Assert.IsTrue(configuration.UseElectionDay);
        }

        [TestMethod]
        public void TestUsesGoodFriday()
        {
            Assert.IsTrue(configuration.UseGoodFriday);
        }

        [TestMethod]
        public void TestUsesIndependenceDay()
        {
            Assert.IsTrue(configuration.UseIndependenceDay);
        }

        [TestMethod]
        public void TestUsesLaborDay()
        {
            Assert.IsTrue(configuration.UseLaborDay);
        }

        [TestMethod]
        public void TestUsesLincolnsBirthday()
        {
            Assert.IsFalse(configuration.UseLincolnsBirthday);
        }

        [TestMethod]
        public void TestUsesMartinLutherKingDay()
        {
            Assert.IsTrue(configuration.UseMartinLutherKingDay);
        }

        [TestMethod]
        public void TestsUsesMemorialDay()
        {
            Assert.IsTrue(configuration.UseMemorialDay);
        }

        [TestMethod]
        public void TestUsesNewYearsDay()
        {
            Assert.IsTrue(configuration.UseNewYearsDay);
        }

        [TestMethod]
        public void TestUsesThanksgiving()
        {
            Assert.IsTrue(configuration.UseThanksgiving);
        }

        [TestMethod]
        public void TestUsesVeteransDay()
        {
            Assert.IsTrue(configuration.UseVeteransDay);
        }

        [TestMethod]
        public void TestUsesWashingtonsBirthday()
        {
            Assert.IsTrue(configuration.UseWashingtonsBirthday);
        }
    }
}
