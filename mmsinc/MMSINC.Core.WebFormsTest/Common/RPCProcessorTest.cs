using System;
using System.Linq.Expressions;
using MMSINC.Data.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Common;
using MMSINC.Core.WebFormsTest.Presenter;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINCTestImplementation.Model;
using Rhino.Mocks;

namespace MMSINC.Core.WebFormsTest.Common
{
    /// <summary>
    /// Summary description for EntityRPCProcessorTest
    /// </summary>
    [TestClass]
    public class RPCProcessorTest
    {
        #region Private Members

        private MockRepository _mocks;

        #endregion

        #region Constructors

        #endregion

        #region TestContext Definition

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #endregion

        #region Additional test attributes

        [TestInitialize]
        public void EntityRPCProcessorTestInitialize()
        {
            _mocks = new MockRepository();
        }

        [TestCleanup]
        public void EntityRPCProcessorTestCleanup()
        {
            _mocks.VerifyAll();
        }

        #endregion

        [TestMethod]
        public void TestViewCommandDisplaysDetailAndNotList()
        {
            var repository = _mocks.DynamicMock<IRepository<Employee>>();
            var detailView = _mocks.DynamicMock<IDetailView<Employee>>();
            var view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.VIEW, 1.ToString(), detailView);
            var presenter = _mocks.DynamicMock<IResourceRPCPresenter<Employee>>();
            SetupResult.For(presenter.Repository).Return(repository);
            SetupResult.For(presenter.RPCView).Return(view);
            var target = new RPCProcessor<Employee>(presenter);

            _mocks.ReplayAll();

            target.Process();

            Assert.IsNotNull(view.DetailVisible);
            Assert.IsTrue(view.DetailVisible.Value);
            Assert.IsNotNull(view.DetailVisible);
            Assert.IsFalse(view.ListVisible.Value);
        }

        [TestMethod]
        public void TestCreateCommandDisplaysDetailAndNotList()
        {
            var repository = _mocks.DynamicMock<IRepository<Employee>>();
            var detailView = _mocks.DynamicMock<IDetailView<Employee>>();
            var view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.CREATE, 1.ToString(), detailView);
            var presenter = _mocks.DynamicMock<IResourceRPCPresenter<Employee>>();
            SetupResult.For(presenter.Repository).Return(repository);
            SetupResult.For(presenter.RPCView).Return(view);
            var target = new RPCProcessor<Employee>(presenter);

            _mocks.ReplayAll();

            target.Process();

            Assert.IsNotNull(view.DetailVisible);
            Assert.IsTrue(view.DetailVisible.Value);
            Assert.IsNotNull(view.DetailVisible);
            Assert.IsFalse(view.ListVisible.Value);
        }

        [TestMethod]
        public void TestListCommandDisplaysListAndNotDetail()
        {
            var repository = _mocks.DynamicMock<IRepository<Employee>>();
            var detailView = _mocks.DynamicMock<IDetailView<Employee>>();
            var view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.LIST, 1.ToString(), detailView);
            var presenter = _mocks.DynamicMock<IResourceRPCPresenter<Employee>>();
            SetupResult.For(presenter.Repository).Return(repository);
            SetupResult.For(presenter.RPCView).Return(view);
            var target = new RPCProcessor<Employee>(presenter);

            _mocks.ReplayAll();

            target.Process();

            Assert.IsNotNull(view.DetailVisible);
            Assert.IsFalse(view.DetailVisible.Value);
            Assert.IsNotNull(view.DetailVisible);
            Assert.IsTrue(view.ListVisible.Value);
        }

        [TestMethod]
        public void TestUpdateCommandDisplaysDetailAndNotList()
        {
            var repository = _mocks.DynamicMock<IRepository<Employee>>();
            var detailView = _mocks.DynamicMock<IDetailView<Employee>>();
            var view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.UPDATE, 1.ToString(), detailView);
            var presenter = _mocks.DynamicMock<IResourceRPCPresenter<Employee>>();
            SetupResult.For(presenter.Repository).Return(repository);
            SetupResult.For(presenter.RPCView).Return(view);
            var target = new RPCProcessor<Employee>(presenter);

            _mocks.ReplayAll();

            target.Process();

            Assert.IsNotNull(view.DetailVisible);
            Assert.IsTrue(view.DetailVisible.Value);
            Assert.IsNotNull(view.DetailVisible);
            Assert.IsFalse(view.ListVisible.Value);
        }

        [TestMethod]
        public void TestUpdateCommandSetsSelectedEntityDataKeyOnRepository()
        {
            var repository = _mocks.DynamicMock<IRepository<Employee>>();
            var detailView = _mocks.DynamicMock<IDetailView<Employee>>();
            Expression<Func<Employee, bool>> expression = o => true;
            var view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.UPDATE, 1.ToString(), detailView, expression);
            var presenter = _mocks.DynamicMock<IResourceRPCPresenter<Employee>>();
            SetupResult.For(presenter.Repository).Return(repository);
            SetupResult.For(presenter.RPCView).Return(view);
            var target = new RPCProcessor<Employee>(presenter);

            using (_mocks.Record())
            {
                repository.SetSelectedDataKeyForRPC("1", expression);
            }

            using (_mocks.Playback())
            {
                target.Process();
            }
        }

        [TestMethod]
        public void TestViewCommandSetsSelectedEntityDataKeyOnRepository()
        {
            var repository = _mocks.DynamicMock<IRepository<Employee>>();
            var detailView = _mocks.DynamicMock<IDetailView<Employee>>();
            Expression<Func<Employee, bool>> expression = o => true;
            var view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.VIEW, 1.ToString(), detailView, expression);
            var presenter = _mocks.DynamicMock<IResourceRPCPresenter<Employee>>();
            SetupResult.For(presenter.Repository).Return(repository);
            SetupResult.For(presenter.RPCView).Return(view);
            var target = new RPCProcessor<Employee>(presenter);

            using (_mocks.Record())
            {
                repository.SetSelectedDataKeyForRPC("1", expression);
            }

            using (_mocks.Playback())
            {
                target.Process();
            }
        }

        [TestMethod]
        public void TestUpdateCommandSetsDetailViewReadWrite()
        {
            var repository = _mocks.DynamicMock<IRepository<Employee>>();
            var detailView = _mocks.DynamicMock<IDetailView<Employee>>();
            var view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.UPDATE, 1.ToString(), detailView);
            var presenter = _mocks.DynamicMock<IResourceRPCPresenter<Employee>>();
            SetupResult.For(presenter.Repository).Return(repository);
            SetupResult.For(presenter.RPCView).Return(view);
            var target = new RPCProcessor<Employee>(presenter);

            _mocks.ReplayAll();
            target.Process();

            Assert.IsNotNull(view.DetailReadOnly);
            Assert.IsFalse(view.DetailReadOnly.Value);
        }

        [TestMethod]
        public void TestCreateCommandSetsDetailViewReadWrite()
        {
            var repository = _mocks.DynamicMock<IRepository<Employee>>();
            var detailView = _mocks.DynamicMock<IDetailView<Employee>>();
            var view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.CREATE, 1.ToString(), detailView);
            var presenter = _mocks.DynamicMock<IResourceRPCPresenter<Employee>>();
            SetupResult.For(presenter.Repository).Return(repository);
            SetupResult.For(presenter.RPCView).Return(view);
            var target = new RPCProcessor<Employee>(presenter);

            _mocks.ReplayAll();
            target.Process();

            Assert.IsNotNull(view.DetailReadOnly);
            Assert.IsFalse(view.DetailReadOnly.Value);
        }

        [TestMethod]
        public void TestViewCommandSetsDetailViewReadOnly()
        {
            var repository = _mocks.DynamicMock<IRepository<Employee>>();
            var detailView = _mocks.DynamicMock<IDetailView<Employee>>();
            var view = new MockedResourceRPCView<Employee>(
                RPCCommandNames.VIEW, 1.ToString(), detailView);
            var presenter = _mocks.DynamicMock<IResourceRPCPresenter<Employee>>();
            SetupResult.For(presenter.Repository).Return(repository);
            SetupResult.For(presenter.RPCView).Return(view);
            var target = new RPCProcessor<Employee>(presenter);

            _mocks.ReplayAll();
            target.Process();

            Assert.IsNotNull(view.DetailReadOnly);
            Assert.IsTrue(view.DetailReadOnly.Value);
        }
    }
}
