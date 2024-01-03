using LINQTo271.Views.CrewAssignments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.CrewAssignments
{
    /// <summary>
    /// Summary description for CrewAssignmentsMonthLegendTest.
    /// </summary>
    [TestClass]
    public class CrewAssignmentsMonthLegendTest
    {
        #region Private Members

        private TestCrewAssignmentsMonthLegend _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void CrewAssignmentsMonthLegendTestInitialize()
        {
            _target = new TestCrewAssignmentsMonthLegendBuilder();
        }

        #endregion

        [TestMethod]
        public void TestContructorDoesNotThrowException()
        {
            MyAssert.DoesNotThrow(() => new CrewAssignmentsMonthLegend());
        }
    }

    internal class TestCrewAssignmentsMonthLegendBuilder : TestDataBuilder<TestCrewAssignmentsMonthLegend>
    {
        #region Exposed Methods

        public override TestCrewAssignmentsMonthLegend Build()
        {
            var obj = new TestCrewAssignmentsMonthLegend();
            return obj;
        }

        #endregion
    }

    internal class TestCrewAssignmentsMonthLegend : CrewAssignmentsMonthLegend
    {
    }
}
