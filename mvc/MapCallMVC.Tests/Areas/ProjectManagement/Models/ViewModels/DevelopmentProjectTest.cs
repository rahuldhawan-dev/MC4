using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.ProjectManagement.Models.ViewModels
{
    [TestClass]
    public class CreateDevelopmentProjectTest : MapCallMvcInMemoryDatabaseTestBase<DevelopmentProject>
    {
        #region Fields

        private ViewModelTester<CreateDevelopmentProject, DevelopmentProject> _vmTester;
        private CreateDevelopmentProject _viewModel;
        private DevelopmentProject _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateDevelopmentProject(_container);
            _entity = new DevelopmentProject();
            _vmTester = new ViewModelTester<CreateDevelopmentProject, DevelopmentProject>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            var authServ = new Mock<IAuthenticationService<User>>();
            _container.Inject(authServ.Object);
            authServ.Setup(x => x.CurrentUser.Id).Returns(1);

            _vmTester.CanMapBothWays(x => x.DeveloperServicesId);
            _vmTester.CanMapBothWays(x => x.WBSNumber);
            _vmTester.CanMapBothWays(x => x.ProjectDescription);
            _vmTester.CanMapBothWays(x => x.StreetName);
            _vmTester.CanMapBothWays(x => x.ForecastedInServiceDate);
            _vmTester.CanMapBothWays(x => x.InServiceDate);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Category);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.BusinessUnit);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PublicWaterSupply);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProjectDescription);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ForecastedInServiceDate);
        }

        #endregion
    }

    [TestClass]
    public class EditDevelopmentProjectTest : InMemoryDatabaseTest<DevelopmentProject>
    {
        #region Fields

        private ViewModelTester<EditDevelopmentProject, DevelopmentProject> _vmTester;
        private EditDevelopmentProject _viewModel;
        private DevelopmentProject _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditDevelopmentProject(_container);
            _entity = new DevelopmentProject();
            _vmTester = new ViewModelTester<EditDevelopmentProject, DevelopmentProject>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.DeveloperServicesId);
            _vmTester.CanMapBothWays(x => x.WBSNumber);
            _vmTester.CanMapBothWays(x => x.ProjectDescription);
            _vmTester.CanMapBothWays(x => x.StreetName);
            _vmTester.CanMapBothWays(x => x.ForecastedInServiceDate);
            _vmTester.CanMapBothWays(x => x.InServiceDate);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Category);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.BusinessUnit);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PublicWaterSupply);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProjectDescription);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ForecastedInServiceDate);
        }

        #endregion
    }
}
