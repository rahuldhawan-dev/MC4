using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Utilities;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CreateGISLayerUpdateTest : MapCallMvcInMemoryDatabaseTestBase<GISLayerUpdate>
    {
        #region Fields

        private ViewModelTester<CreateGISLayerUpdate, GISLayerUpdate> _vmTester;
        private CreateGISLayerUpdate _viewModel;
        private GISLayerUpdate _entity;
        private User _currentUser;

        #endregion

        #region Init/Cleanup

        private IAuthenticationService<User> CreateAuthenticationService()
        {
            return (new MockAuthenticationService<User>(_currentUser = GetFactory<MapCall.Common.Testing.Data.UserFactory>().Create())).Object;
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Use<DateTimeProvider>();
            i.For<IAuthenticationService<User>>().Use(() => CreateAuthenticationService());
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateGISLayerUpdate(_container);
            _entity = new GISLayerUpdate();
            _vmTester = new ViewModelTester<CreateGISLayerUpdate, GISLayerUpdate>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Updated);
            _vmTester.CanMapBothWays(x => x.IsActive);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Updated);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsActive);
        }

        #endregion
    }

    [TestClass]
    public class EditGISLayerUpdateTest : InMemoryDatabaseTest<GISLayerUpdate>
    {
        #region Fields

        private ViewModelTester<EditGISLayerUpdate, GISLayerUpdate> _vmTester;
        private EditGISLayerUpdate _viewModel;
        private GISLayerUpdate _entity;
        private User _currentUser;

        #endregion

        #region Init/Cleanup

        private IAuthenticationService<User> CreateAuthenticationService()
        {
            return new MockAuthenticationService<User>(_currentUser = GetFactory<MapCall.Common.Testing.Data.UserFactory>().Create()).Object;
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Use<DateTimeProvider>();
            i.For<IAuthenticationService<User>>().Use(() => CreateAuthenticationService());
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditGISLayerUpdate(_container);
            _entity = new GISLayerUpdate();
            _vmTester = new ViewModelTester<EditGISLayerUpdate, GISLayerUpdate>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Updated);
            _vmTester.CanMapBothWays(x => x.IsActive);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Updated);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsActive);
        }

        #endregion
    }
}
