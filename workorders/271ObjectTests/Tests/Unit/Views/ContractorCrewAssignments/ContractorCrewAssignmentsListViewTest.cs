
using LINQTo271.Views.ContractorCrewAssignments;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.ContractorCrewAssignments
{
    [TestClass]
    public class ContractorCrewAssignmentsListViewTest
    {
        #region Private Members

        private TestContractorCrewAssignmentsListView _target;
        private MvpGridView _listControl;
        
        #endregion

        #region Test Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _listControl = new MvpGridView();
            _target =
                new TestContractorCrewAssignmentsListViewBuilder().WithListControl(_listControl);
        }

        #endregion

        [TestMethod]
        public void TestListControlPropertyReturnsListControl()
        {
            Assert.AreSame(_listControl, _target.ListControl);
        }
    }

    internal class TestContractorCrewAssignmentsListViewBuilder : TestDataBuilder<TestContractorCrewAssignmentsListView>
    {
        #region Private Mmebers

        private MvpGridView _listControl = new MvpGridView();
        
        #endregion

        #region Exposed Methods

        public override TestContractorCrewAssignmentsListView Build()
        {
            var view = new TestContractorCrewAssignmentsListView();
            if (_listControl != null)
                view.SetListControl(_listControl);
            return view;
        }

        public TestContractorCrewAssignmentsListViewBuilder WithListControl(MvpGridView listControl)
        {
            _listControl = listControl;
            return this;
        }

        #endregion
    }

    internal class TestContractorCrewAssignmentsListView : ContractorCrewAssignmentsListView
    {
        #region Exposed Methods
        
        public void SetListControl(MvpGridView listControl)
        {
            gvCrewAssignments = listControl;
        }

        #endregion
    }
}
