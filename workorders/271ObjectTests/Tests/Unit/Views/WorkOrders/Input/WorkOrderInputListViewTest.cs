using System;
using System.Web.Mvc;
using System.Web.UI;
using LINQTo271.Views.WorkOrders.Input;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;
using StructureMap;
using WorkOrders;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.Input
{
    /// <summary>
    /// Summary description for WorkOrdersListViewTest
    /// </summary>
    [TestClass]
    public class WorkOrderInputListViewTest : EventFiringTestClass
    {
        #region Private Members

        private IListPresenter<WorkOrder> _presenter;
        private TestWorkOrderInputListView _target;
        private IListControl _listControl;
        private IUpdatePanel _updatePanel;
        private IContainer _container;

        #endregion

        #region Private Static Methods

        private static void InvokePageLoad(WorkOrderInputListView view)
        {
            InvokeEventByName(view, "Page_Load", GetEventArgArray(EventArgs.Empty));
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _container = new Container();
            _presenter = _mocks.DynamicMock<IListPresenter<WorkOrder>>();
            _listControl = _mocks.DynamicMock<IListControl>();
            _updatePanel = _mocks.DynamicMock<IUpdatePanel>();
            _target =
                new TestWorkOrderInputListViewBuilder().WithListControl(
                    _listControl).WithUpdatePanel(_updatePanel);
            _container.Inject(_presenter);
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestHidAssetIDTextChangedCallsDataBindOnListControlAndUpdateOnUpdatePanel()
        {
            using (_mocks.Record())
            {
                _listControl.DataBind();
                _updatePanel.Update();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "hidAssetID_TextChanged");
            }
        }

        [TestMethod]
        public void TestSetViewControlsDoesNothing()
        {
            _presenter = _mocks.CreateMock<IListPresenter<WorkOrder>>();
            _listControl = _mocks.CreateMock<IListControl>();
            _updatePanel = _mocks.CreateMock<IUpdatePanel>();
            _target = new TestWorkOrderInputListViewBuilder()
                .WithListControl(_listControl)
                .WithUpdatePanel(_updatePanel);

            _mocks.ReplayAll();

            _target.SetViewControlsVisible(true);
            _target.SetViewControlsVisible(false);
        }

        [TestMethod]
        public void TestPhasePropertyDenotesInput()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(_target.Phase, WorkOrderPhase.Input);
        }
    }

    internal class TestWorkOrderInputListViewBuilder : TestDataBuilder<TestWorkOrderInputListView>
    {
        #region Private Members

        private bool _isPostBack;
        private IListControl _listControl = new MvpGridView();
        private IUpdatePanel _updatePanel = new MvpUpdatePanel();

        #endregion

        #region Exposed Methods

        public override TestWorkOrderInputListView Build()
        {
            var view = new TestWorkOrderInputListView();
            view.SetPostBack(_isPostBack);
            if (_listControl != null)
                view.SetListControl(_listControl);
            if (_updatePanel != null)
                view.SetUpdatePanel(_updatePanel);
            return view;
        }

        public TestWorkOrderInputListViewBuilder WithPostBack(bool postBack)
        {
            _isPostBack = postBack;
            return this;
        }

        public TestWorkOrderInputListViewBuilder WithListControl(IListControl listControl)
        {
            _listControl = listControl;
            return this;
        }

        public TestWorkOrderInputListViewBuilder WithUpdatePanel(IUpdatePanel updatePanel)
        {
            _updatePanel = updatePanel;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderInputListView : WorkOrderInputListView
    {
        #region Exposed Methods

        public void SetPostBack(bool postBack)
        {
            _isMvpPostBack = postBack;
        }

        public void SetListControl(IListControl listControl)
        {
            gvWorkOrders = listControl;
        }

        public void SetUpdatePanel(IUpdatePanel updatePanel)
        {
            upWorkOrderHistory = updatePanel;
        }

        #endregion
    }

    internal class MockPage : Page
    {
    }
}
