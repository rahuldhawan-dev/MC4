using System;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINC.Testing.DesignPatterns;
using MMSINCTestImplementation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace MMSINC.Core.WebFormsTest.Common
{
    /// <summary>
    /// Summary description for EntityRPCProcessorTest
    /// </summary>
    [TestClass]
    public class EntityRPCProcessorTest
    {
        #region Private Members

        private MockRepository _mocks;

        #endregion

        #region Private Methods

        private TestEntityRPCProcessorBuilder GetSampleEntityRPCProcessorBuilder()
        {
            return new TestEntityRPCProcessorBuilder(_mocks);
        }

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
        public void TestConstructorSetsPresenter()
        {
            var presenter = _mocks.DynamicMock<IResourceRPCPresenter<Employee>>();
            var target =
                GetSampleEntityRPCProcessorBuilder().WithPresenter(presenter).Build();

            _mocks.ReplayAll();

            Assert.AreSame(presenter, target.Presenter);
        }

        [TestMethod]
        public void TestProcessSetsPresenterRepositorySelectedEntityIndexWhenCommandIsUpdate()
        {
            var command = RPCCommandNames.UPDATE;
            var argument = 1.ToString();
            var view = _mocks.DynamicMock<IResourceRPCView<Employee>>();
            var repository = _mocks.DynamicMock<IRepository<Employee>>();
            var target =
                GetSampleEntityRPCProcessorBuilder().WithView(view).WithRepository(repository).WithCommand(command)
                                                    .WithArgument(argument).Build();

            using (_mocks.Record())
            {
                repository.RestoreFromPersistedState(Int32.Parse(argument));
            }

            using (_mocks.Playback())
            {
                target.Process();
            }
        }

        [TestMethod]
        public void TestProcessSetsPresenterRepositorySelectedEntityIndexWhenCommandIsView()
        {
            var command = RPCCommandNames.VIEW;
            var argument = 1.ToString();
            var view = _mocks.DynamicMock<IResourceRPCView<Employee>>();
            var repository = _mocks.DynamicMock<IRepository<Employee>>();
            var target =
                GetSampleEntityRPCProcessorBuilder().WithView(view).WithRepository(repository).WithCommand(command)
                                                    .WithArgument(argument).Build();

            using (_mocks.Record())
            {
                repository.RestoreFromPersistedState(Int32.Parse(argument));
            }

            using (_mocks.Playback())
            {
                target.Process();
            }
        }

        [TestMethod]
        public void TestProcessSetsViewToDetailModeWhenCommandIsUpdate()
        {
            var command = RPCCommandNames.UPDATE;
            var argument = 1.ToString();
            var view = _mocks.DynamicMock<IResourceRPCView<Employee>>();
            var repository = _mocks.DynamicMock<IRepository<Employee>>();
            var target =
                GetSampleEntityRPCProcessorBuilder().WithView(view).WithRepository(repository).WithCommand(command)
                                                    .WithArgument(argument).Build();

            using (_mocks.Record())
            {
                view.SetViewMode(ResourceViewMode.Detail);
            }

            using (_mocks.Playback())
            {
                target.Process();
            }
        }

        [TestMethod]
        public void TestProcessSetsViewToDetailModeWhenCommandIsView()
        {
            var command = RPCCommandNames.VIEW;
            var argument = 1.ToString();
            var view = _mocks.DynamicMock<IResourceRPCView<Employee>>();
            var repository = _mocks.DynamicMock<IRepository<Employee>>();
            var target =
                GetSampleEntityRPCProcessorBuilder().WithView(view).WithRepository(repository).WithCommand(command)
                                                    .WithArgument(argument).Build();

            using (_mocks.Record())
            {
                view.SetViewMode(ResourceViewMode.Detail);
            }

            using (_mocks.Playback())
            {
                target.Process();
            }
        }

        [TestMethod]
        public void TestProcessSetsViewToDetailModeWhenCommandIsCreate()
        {
            var command = RPCCommandNames.CREATE;
            var view = _mocks.DynamicMock<IResourceRPCView<Employee>>();
            var target =
                GetSampleEntityRPCProcessorBuilder().WithView(view).WithCommand(command)
                                                    .Build();

            using (_mocks.Record())
            {
                view.SetViewMode(ResourceViewMode.Detail);
            }

            using (_mocks.Playback())
            {
                target.Process();
            }
        }

        [TestMethod]
        public void TestProcessSetsViewToListModeWhenCommandIsList()
        {
            var command = RPCCommandNames.LIST;
            var view = _mocks.DynamicMock<IResourceRPCView<Employee>>();
            var target =
                GetSampleEntityRPCProcessorBuilder().WithView(view).WithCommand(command)
                                                    .Build();

            using (_mocks.Record())
            {
                view.SetViewMode(ResourceViewMode.List);
            }

            using (_mocks.Playback())
            {
                target.Process();
            }
        }
    }

    internal class TestEntityRPCProcessorBuilder : TestDataBuilder<EntityRPCProcessor<Employee>>
    {
        #region Private Members

        private IResourceRPCPresenter<Employee> _presenter;
        private readonly MockRepository _mocks;
        private IRepository<Employee> _repository;
        private IResourceRPCView<Employee> _view;
        private string _command, _argument;

        #endregion

        #region Constructors

        public TestEntityRPCProcessorBuilder(MockRepository mocks)
        {
            _mocks = mocks;
            _presenter = _mocks.DynamicMock<IResourceRPCPresenter<Employee>>();
        }

        #endregion

        #region Exposed Methods

        public override EntityRPCProcessor<Employee> Build()
        {
            var processor =
                new EntityRPCProcessor<Employee>(_presenter);
            if (_repository != null)
                SetupResult.For(_presenter.Repository).Return(_repository);
            if (_view != null)
            {
                SetupResult.For(_presenter.RPCView).Return(_view);
                if (_command != null)
                    SetupResult.For(_view.Command).Return(_command);
                if (_argument != null)
                    SetupResult.For(_view.Argument).Return(_argument);
            }

            return processor;
        }

        public TestEntityRPCProcessorBuilder WithPresenter(IResourceRPCPresenter<Employee> presenter)
        {
            _presenter = presenter;
            return this;
        }

        public TestEntityRPCProcessorBuilder WithRepository(IRepository<Employee> repository)
        {
            _repository = repository;
            return this;
        }

        public TestEntityRPCProcessorBuilder WithView(IResourceRPCView<Employee> view)
        {
            _view = view;
            return this;
        }

        public TestEntityRPCProcessorBuilder WithCommand(string command)
        {
            _command = command;
            return this;
        }

        public TestEntityRPCProcessorBuilder WithArgument(string argument)
        {
            _argument = argument;
            return this;
        }

        #endregion
    }
}
