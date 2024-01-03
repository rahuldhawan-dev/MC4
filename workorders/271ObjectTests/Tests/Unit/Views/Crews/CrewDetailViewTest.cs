using System.Web.UI.WebControls;
using LINQTo271.Views.Crews;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.Crews
{
    /// <summary>
    /// Summary description for CrewDetailViewTest
    /// </summary>
    [TestClass]
    public class CrewDetailViewTest : EventFiringTestClass
    {
        #region Private Members

        private TestCrewDetailView _target;
        private IDetailControl _detailControl;
        private IObjectContainerDataSource _dataSource;
        private Button _btnCancel, _btnEdit, _btnSave;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _detailControl = _mocks.DynamicMock<IDetailControl>();
            _dataSource = _mocks.DynamicMock<IObjectContainerDataSource>();
            _btnCancel = new Button();
            _btnEdit = new Button();
            _btnSave = new Button();
            _target =
                new TestCrewDetailViewBuilder()
                    .WithDataSource(_dataSource)
                    .WithDetailControl(_detailControl)
                    .WithCancelButton(_btnCancel)
                    .WithEditButton(_btnEdit)
                    .WithSaveButton(_btnSave);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestDataSourcePropertyReturnsDataSource()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_dataSource, _target.DataSource);
        }

        [TestMethod]
        public void TestDetailControlPropertyReturnsCrewsDetailsView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_detailControl, _target.DetailControl);
        }
    }

    internal class TestCrewDetailViewBuilder : TestDataBuilder<TestCrewDetailView>
    {
        #region Private Members

        private IDetailControl _dv = new MvpDetailsView();
        private Button _btnEdit = new Button(),
                       _btnSave = new Button(),
                       _btnCancel = new Button();
        private IObjectContainerDataSource _ds = new MvpObjectContainerDataSource();

        #endregion

        #region Exposed Methods

        public override TestCrewDetailView Build()
        {
            var view = new TestCrewDetailView();
            if (_dv != null)
                view.SetDetailControl(_dv);
            if (_ds != null)
                view.SetDataSource(_ds);
            if (_btnEdit != null)
                view.SetEditButton(_btnEdit);
            if (_btnSave != null)
                view.SetSaveButton(_btnSave);
            if (_btnCancel != null)
                view.SetCancelButton(_btnCancel);
            return view;
        }

        public TestCrewDetailViewBuilder WithDetailControl(IDetailControl dv)
        {
            _dv = dv;
            return this;
        }

        public TestCrewDetailViewBuilder WithEditButton(Button button)
        {
            _btnEdit = button;
            return this;
        }

        public TestCrewDetailViewBuilder WithSaveButton(Button button)
        {
            _btnSave = button;
            return this;
        }

        public TestCrewDetailViewBuilder WithCancelButton(Button button)
        {
            _btnCancel = button;
            return this;
        }

        public TestCrewDetailViewBuilder WithDataSource(IObjectContainerDataSource ds)
        {
            _ds = ds;
            return this;
        }

        #endregion
    }

    internal class TestCrewDetailView : CrewDetailView
    {
        #region Exposed Methods

        public void SetDetailControl(IDetailControl dv)
        {
            dvCrew = dv;
        }

        public void SetEditButton(Button btn)
        {
            btnEdit = btn;
        }

        public void SetSaveButton(Button btn)
        {
            btnSave = btn;
        }

        public void SetCancelButton(Button btn)
        {
            btnCancel = btn;
        }

        public void SetDataSource(IObjectContainerDataSource ds)
        {
            odsCrew = ds;
        }

        #endregion
    }
}
