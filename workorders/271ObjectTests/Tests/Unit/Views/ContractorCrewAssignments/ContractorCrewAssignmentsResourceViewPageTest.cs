using LINQTo271.Views.ContractorCrewAssignments;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.ContractorCrewAssignments
{
    [TestClass]
    public class ContractorCrewAssignmentsResourceViewPageTest
    {
        #region Private Members

        private TestContractorCrewAssignmentsResourceViewPage _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void CrewAssignmentsResourceViewPageTestInitialize()
        {
            _target = new TestContractorCrewAssignmentsResourceViewPageBuilder();
        }

        #endregion
    }

    internal class TestContractorCrewAssignmentsResourceViewPageBuilder : TestDataBuilder<TestContractorCrewAssignmentsResourceViewPage>
    {
        #region Exposed Methods

        public override TestContractorCrewAssignmentsResourceViewPage Build()
        {
            var obj = new TestContractorCrewAssignmentsResourceViewPage();
            return obj;
        }

        #endregion
    }

    internal class TestContractorCrewAssignmentsResourceViewPage : ContractorCrewAssignmentsResourceViewPage
    {
    }
}
