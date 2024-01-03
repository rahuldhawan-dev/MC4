using System;
using LINQTo271.Views.CrewAssignments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.DesignPatterns;

namespace _271ObjectTests.Tests.Unit.Views.CrewAssignments
{
    /// <summary>
    /// Summary description for DateTimeEventArgsTest.
    /// </summary>
    [TestClass]
    public class DateTimeEventArgsTest
    {
        #region Private Members

        private DateTime _date;
        private TestDateTimeEventArgs _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void DateTimeEventArgsTestInitialize()
        {
            _date = DateTime.Now;
            _target = new TestDateTimeEventArgsBuilder(_date);
        }

        #endregion

        [TestMethod]
        public void TestConstructorSetsDateValue()
        {
            var target = new DateTimeEventArgs(_date);

            Assert.AreEqual(_date, target.Date);
        }
    }

    internal class TestDateTimeEventArgsBuilder : TestDataBuilder<TestDateTimeEventArgs>
    {
        #region Private Members

        private readonly DateTime _date;

        #endregion

        #region Constructors

        internal TestDateTimeEventArgsBuilder(DateTime date)
        {
            _date = date;
        }

        #endregion

        #region Exposed Methods

        public override TestDateTimeEventArgs Build()
        {
            var obj = new TestDateTimeEventArgs(_date);
            return obj;
        }

        #endregion
    }

    internal class TestDateTimeEventArgs : DateTimeEventArgs
    {
        #region Constructors

        public TestDateTimeEventArgs(DateTime date) : base(date)
        {
        }

        #endregion
    }
}
