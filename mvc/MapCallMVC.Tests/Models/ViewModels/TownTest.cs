using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CreateTownTest : MapCallMvcInMemoryDatabaseTestBase<Town>
    {
        #region Private Members

        private ViewModelTester<CreateTown, Town> _vmTester;
        private CreateTown _viewModel;
        private Town _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<ITownRepository>().Use<TownRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetFactory<TownFactory>().Create();
            _viewModel = _viewModelFactory.Build<CreateTown, Town>( _entity);
            _vmTester = new ViewModelTester<CreateTown, Town>(_viewModel, _entity);
        }
        
        #endregion

        #region Mapping

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ShortName);
            _vmTester.CanMapBothWays(x => x.Address);
            _vmTester.CanMapBothWays(x => x.ContactName);
            _vmTester.CanMapBothWays(x => x.DistrictId);
            _vmTester.CanMapBothWays(x => x.EmergencyContact);
            _vmTester.CanMapBothWays(x => x.EmergencyFax);
            _vmTester.CanMapBothWays(x => x.EmergencyPhone);
            _vmTester.CanMapBothWays(x => x.Fax);
            _vmTester.CanMapBothWays(x => x.FD1Contact);
            _vmTester.CanMapBothWays(x => x.FD1Fax);
            _vmTester.CanMapBothWays(x => x.FD1Phone);
            _vmTester.CanMapBothWays(x => x.Phone);
            _vmTester.CanMapBothWays(x => x.FullName);
            _vmTester.CanMapBothWays(x => x.Zip);
            _vmTester.CanMapBothWays(x => x.Lat);
            _vmTester.CanMapBothWays(x => x.Lon);
            _vmTester.CanMapBothWays(x => x.DBA);
        }

        [TestMethod]
        public void TestAbbreviationTypeCanMapBothWays()
        {
            var at = GetEntityFactory<AbbreviationType>().Create(new {Description = "Foo"});
            _entity.AbbreviationType = at;

            _vmTester.MapToViewModel();

            Assert.AreEqual(at.Id, _viewModel.AbbreviationType);

            _entity.AbbreviationType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(at, _entity.AbbreviationType);
        }

        [TestMethod]
        public void TestStateCanMapBothWays()
        {
            var state = GetEntityFactory<State>().Create(new { Name = "Foo"});
            _entity.State = state;

            _vmTester.MapToViewModel();

            Assert.AreEqual(state.Id, _viewModel.State);

            _entity.State = null;
            _vmTester.MapToEntity();

            Assert.AreSame(state, _entity.State);
        }

        [TestMethod]
        public void TestCountyCanMapBothWays()
        {
            var county = GetEntityFactory<County>().Create(new {Name = "Foo"});
            _entity.County = county;

            _vmTester.MapToViewModel();

            Assert.AreEqual(county.Id, _viewModel.County);

            _entity.County = null;
            _vmTester.MapToEntity();

            Assert.AreSame(county, _entity.County);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ShortName, "The Township Name field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FullName);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.State);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AbbreviationType);
        }

        #endregion
    }

    [TestClass]
    public class EditTownTest : MapCallMvcInMemoryDatabaseTestBase<Town>
    {
        #region Private Members

        private ViewModelTester<EditTown, Town> _vmTester;
        private EditTown _viewModel;
        private Town _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<ITownRepository>().Use<TownRepository>(); 
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetFactory<TownFactory>().Create();
            _viewModel = _viewModelFactory.Build<EditTown, Town>( _entity);
            _vmTester = new ViewModelTester<EditTown, Town>(_viewModel, _entity);
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ShortName);
            _vmTester.CanMapBothWays(x => x.Address);
            _vmTester.CanMapBothWays(x => x.ContactName);
            _vmTester.CanMapBothWays(x => x.DistrictId);
            _vmTester.CanMapBothWays(x => x.EmergencyContact);
            _vmTester.CanMapBothWays(x => x.EmergencyFax);
            _vmTester.CanMapBothWays(x => x.EmergencyPhone);
            _vmTester.CanMapBothWays(x => x.Fax);
            _vmTester.CanMapBothWays(x => x.FD1Contact);
            _vmTester.CanMapBothWays(x => x.FD1Fax);
            _vmTester.CanMapBothWays(x => x.FD1Phone);
            _vmTester.CanMapBothWays(x => x.Phone);
            _vmTester.CanMapBothWays(x => x.FullName);
            _vmTester.CanMapBothWays(x => x.Zip);
            _vmTester.CanMapBothWays(x => x.Lat);
            _vmTester.CanMapBothWays(x => x.Lon);
            _vmTester.CanMapBothWays(x => x.DBA);
        }

        [TestMethod]
        public void TestAbbreviationTypeCanMapBothWays()
        {
            var at = GetEntityFactory<AbbreviationType>().Create(new { Description = "Foo" });
            _entity.AbbreviationType = at;

            _vmTester.MapToViewModel();

            Assert.AreEqual(at.Id, _viewModel.AbbreviationType);

            _entity.AbbreviationType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(at, _entity.AbbreviationType);
        }

        [TestMethod]
        public void TestStateCanMapBothWays()
        {
            var state = GetEntityFactory<State>().Create(new { Name = "Foo" });
            _entity.State = state;

            _vmTester.MapToViewModel();

            Assert.AreEqual(state.Id, _viewModel.State);

            _entity.State = null;
            _vmTester.MapToEntity();

            Assert.AreSame(state, _entity.State);
        }

        [TestMethod]
        public void TestCountyCanMapBothWays()
        {
            var county = GetEntityFactory<County>().Create(new { Name = "Foo" });
            _entity.County = county;

            _vmTester.MapToViewModel();

            Assert.AreEqual(county.Id, _viewModel.County);

            _entity.County = null;
            _vmTester.MapToEntity();

            Assert.AreSame(county, _entity.County);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ShortName, "The Township Name field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FullName);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.State);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AbbreviationType);
        }

        #endregion        
    }
}