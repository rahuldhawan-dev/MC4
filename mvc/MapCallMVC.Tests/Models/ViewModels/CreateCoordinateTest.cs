using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CreateCoordinateTest : MapCallMvcInMemoryDatabaseTestBase<Coordinate>
    {
        #region Private Members

        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private CreateCoordinate _viewModel;
        private Coordinate _entity;
        private ViewModelTester<CreateCoordinate, Coordinate> _vmTester;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _viewModel = new CreateCoordinate(_container);
            _entity = new Coordinate();
            _vmTester = new ViewModelTester<CreateCoordinate, Coordinate>(_viewModel, _entity);
            
            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);
        }
        
        #endregion

        [TestMethod]
        public void TestSetDefaultsSetsDefaultCoordinatesForUsersDefaultOperatingCenter()
        {
            var coordinate = GetEntityFactory<Coordinate>().Create(new {Latitude = 4m, Longitude = 5m});
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { MapId = "1231231212", Coordinate = coordinate});
            _user = GetFactory<UserFactory>().Create(new { DefaultOperatingCenter = operatingCenter });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _viewModel.SetDefaults();

            Assert.AreNotEqual(0, _viewModel.Latitude);
            Assert.AreEqual(coordinate.Latitude, _viewModel.Latitude);
            Assert.AreEqual(coordinate.Longitude, _viewModel.Longitude);
        }

        [TestMethod]
        public void TestMapSetsIconId()
        {
            var icon = GetFactory<MapIconFactory>().Create();
            var coordinate = new Coordinate();
            coordinate.Icon = icon;

            var target = _viewModelFactory.Build<CreateCoordinate, Coordinate>(coordinate);
            Assert.AreEqual(icon.Id, target.IconId);
        }

        [TestMethod]
        public void TestMapToEntitySetsIcon()
        {
            var icon = GetFactory<MapIconFactory>().Create();
            var coordinate = new Coordinate();

            var target = new CreateCoordinate(_container);
            target.IconId = icon.Id;

            target.MapToEntity(coordinate);
            Assert.AreSame(icon, coordinate.Icon);
        }
    }
}