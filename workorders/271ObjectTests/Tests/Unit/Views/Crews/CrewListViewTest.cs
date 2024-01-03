using System.Web.UI.WebControls;
using LINQTo271.Views.Crews;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.Crews
{
    /// <summary>
    /// Summary description for CrewListViewTest
    /// </summary>
    [TestClass]
    public class CrewListViewTest
    {
        #region Private Members

        private TestCrewListView _target;
        private Button _btnCreate;
        private MvpGridView _listControl;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void CrewListViewTestInitialize()
        {
            _btnCreate = new Button();
            _listControl = new MvpGridView();
            _target =
                new TestCrewListViewBuilder().WithCreateButton(_btnCreate).
                    WithListControl(_listControl);
        }

        #endregion

        [TestMethod]
        public void TestListControlPropertyReturnsListControl()
        {
            Assert.AreSame(_listControl, _target.ListControl);
        }

        [TestMethod]
        public void TestSetViewControlsVisibleTogglesVisibilityOfCreateButton()
        {
            _btnCreate.Visible = false;

            _target.SetViewControlsVisible(true);

            Assert.IsTrue(_btnCreate.Visible);

            _target.SetViewControlsVisible(false);

            Assert.IsFalse(_btnCreate.Visible);
        }
    }

    internal class TestCrewListViewBuilder : TestDataBuilder<TestCrewListView>
    {
        #region Private Members

        private MvpGridView _listControl = new MvpGridView();
        private Button _btnCreate = new Button();

        #endregion

        #region Exposed Methods

        public override TestCrewListView Build()
        {
            var view = new TestCrewListView();
            if (_listControl != null)
                view.SetListControl(_listControl);
            if (_btnCreate != null)
                view.SetCreateButton(_btnCreate);
            return view;
        }

        public TestCrewListViewBuilder WithListControl(MvpGridView listControl)
        {
            _listControl = listControl;
            return this;
        }

        public TestCrewListViewBuilder WithCreateButton(Button btnCreate)
        {
            _btnCreate = btnCreate;
            return this;
        }

        #endregion
    }

    internal class TestCrewListView : CrewListView
    {
        #region Exposed Methods

        public void SetListControl(MvpGridView listControl)
        {
            gvCrews = listControl;
        }

        public void SetCreateButton(Button createButton)
        {
            btnCreate = createButton;
        }

        #endregion
    }
}
