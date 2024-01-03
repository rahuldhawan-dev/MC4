using LINQTo271.Views.ContractorCrewAssignments;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.ContractorCrewAssignments
{
    [TestClass]
    public class ContractorCrewAssignmentsResourceViewTest
    {
        #region Private Members

        private TestContractorCrewAssignmentsResourceView _target;

        #endregion

        #region Test Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new TestContractorCrewAssignmentsResourceViewBuilder();
        }

        #endregion
    }

    internal class TestContractorCrewAssignmentsResourceViewBuilder : TestDataBuilder<TestContractorCrewAssignmentsResourceView>
    {
        #region Exposed Methods

        public override TestContractorCrewAssignmentsResourceView Build()
        {
            var obj = new TestContractorCrewAssignmentsResourceView();
            return obj;
        }

        #endregion

    }

    internal class TestContractorCrewAssignmentsResourceView : ContractorCrewAssignmentsResourceView
    {
        
    }
}
