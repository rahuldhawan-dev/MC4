using System;
using LINQTo271.Views.Crews;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using Rhino.Mocks;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.Crews
{
    /// <summary>
    /// Summary description for CrewResourceViewTest
    /// </summary>
    [TestClass]
    public class CrewResourceViewTest
    {
        #region Private Members

        private MockRepository _mocks;
        private TestCrewResourceView _target;
        private IButton _btnBackToList;
        private IDetailView<Crew> _detailView;
        private IListView<Crew> _listView;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void CrewResourceViewTestInitialize()
        {
            _mocks = new MockRepository();
            _btnBackToList = _mocks.DynamicMock<IButton>();
            _detailView = _mocks.DynamicMock<IDetailView<Crew>>();
            _listView = _mocks.DynamicMock<IListView<Crew>>();
            _target =
                new TestCrewResourceViewBuilder()
                    .WithBackToListButton(_btnBackToList)
                    .WithDetailView(_detailView)
                    .WithListView(_listView);
        }

        [TestCleanup]
        public void CrewResourceViewTestCleanup()
        {
            _mocks.VerifyAll();
        }

        #endregion

        [TestMethod]
        public void TestBackToListPropertyReturnsBackToListButton()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_btnBackToList, _target.BackToListButton);
        }

        [TestMethod]
        public void TestDetailViewPropertyReturnsDetailView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_detailView, _target.DetailView);
        }

        [TestMethod]
        public void TestListViewPropertyReturnsListView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_listView, _target.ListView);
        }

        [TestMethod]
        public void TestSearchViewPropertyReturnsNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.SearchView);
        }

        [TestMethod]
        public void TestSetViewModeHidesDetailViewWhenNewModeIsNotDetail()
        {
            foreach (ResourceViewMode mode in Enum.GetValues(typeof(ResourceViewMode)))
            {
                if (mode == ResourceViewMode.Detail)
                    continue;

                using (_mocks.Record())
                {
                    _detailView.Visible = false;
                }

                using (_mocks.Playback())
                {
                    _target.SetViewMode(mode);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSetViewModeShowsDetailViewWhenNewModeIsDetail()
        {
            using (_mocks.Record())
            {
                _detailView.Visible = true;
            }

            using (_mocks.Playback())
            {
                _target.SetViewMode(ResourceViewMode.Detail);

            }
        }
    }

    internal class TestCrewResourceViewBuilder : TestDataBuilder<TestCrewResourceView>
    {
        #region Private Members

        private IButton _btnBackToList = new MvpButton();

        private IDetailView<Crew> _detailView = new CrewDetailView();

        private IListView<Crew> _listView = new CrewListView();

        #endregion

        #region Exposed Methods

        public override TestCrewResourceView Build()
        {
            var view = new TestCrewResourceView();
            if (_btnBackToList != null)
                view.SetBackToListButton(_btnBackToList);
            if (_detailView != null)
                view.SetDetailView(_detailView);
            if (_listView != null)
                view.SetListView(_listView);
            return view;
        }

        public TestCrewResourceViewBuilder WithBackToListButton(IButton btn)
        {
            _btnBackToList = btn;
            return this;
        }

        public TestCrewResourceViewBuilder WithDetailView(IDetailView<Crew> detailView)
        {
            _detailView = detailView;
            return this;
        }

        public TestCrewResourceViewBuilder WithListView(IListView<Crew> listView)
        {
            _listView = listView;
            return this;
        }

        #endregion
    }

    internal class TestCrewResourceView : CrewResourceView
    {
        #region Exposed Methods

        public void SetBackToListButton(IButton btn)
        {
            btnBackToList = btn;
        }

        public void SetDetailView(IDetailView<Crew> detailView)
        {
            cdvCrew = detailView;
        }

        public void SetListView(IListView<Crew> listView)
        {
            clvCrews = listView;
        }

        #endregion
    }
}
