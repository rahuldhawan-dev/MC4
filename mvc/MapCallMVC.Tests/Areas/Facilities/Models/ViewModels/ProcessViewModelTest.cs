using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Facilities.Models.ViewModels
{
    [TestClass]
    public class ProcessViewModelTest : MapCallMvcInMemoryDatabaseTestBase<Process>
    {
        #region Fields

        private Process _entity;
        private ProcessViewModel _viewModel;
        private ViewModelTester<ProcessViewModel, Process> _vmTester;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _authServ = new Mock<IAuthenticationService<User>>();
            _user = new User();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _container.Inject(_authServ.Object);
            
            _entity = GetFactory<ProcessFactory>().Create();
            _viewModel = _viewModelFactory.Build<ProcessViewModel, Process>(_entity);
            _vmTester = new ViewModelTester<ProcessViewModel, Process>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Sequence);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProcessStage);
        }

        [TestMethod]
        public void TestSimplePropsCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.Sequence);
        }

        [TestMethod]
        public void TestProcessStageCanMapBothWays()
        {
            var emp = GetFactory<ProcessStageFactory>().Create();
            _entity.ProcessStage = emp;
            _vmTester.MapToViewModel();
            Assert.AreEqual(emp.Id, _viewModel.ProcessStage);

            _entity.ProcessStage = null;
            _vmTester.MapToEntity();
            Assert.AreSame(emp, _entity.ProcessStage);
        }
        

        #endregion
    }
}
