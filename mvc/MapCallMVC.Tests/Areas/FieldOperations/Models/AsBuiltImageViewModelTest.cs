using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using FluentNHibernate.Utils;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    public abstract class AsBuiltImageViewModelTest<TViewModel> : ViewModelTestBase<AsBuiltImage, TViewModel>
        where TViewModel : BaseAsBuiltImageViewModel
    {
        #region Fields

        public Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
        }

        [TestInitialize]
        public void AsBuiltImageViewModelTestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(DateTime.Now);
            _authServ = new Mock<IAuthenticationService<User>>();
            _user = GetEntityFactory<User>().Create(new { UserName = "IAMAUSER"});
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);

            _entity.CoordinatesModifiedOn = null;
        }

        #region Validations

         [TestMethod]
         public override void TestEntityMustExistValidation()
         {
             ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
             ValidationAssert.EntityMustExist(x => x.Town, GetEntityFactory<Town>().Create());
         }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Comments);
            _vmTester.CanMapBothWays(x => x.DateInstalled);
            _vmTester.CanMapBothWays(x => x.PhysicalInService);
            _vmTester.CanMapBothWays(x => x.StreetPrefix);
            _vmTester.CanMapBothWays(x => x.ApartmentNumber);
            _vmTester.CanMapBothWays(x => x.Street);
            _vmTester.CanMapBothWays(x => x.StreetSuffix);
            _vmTester.CanMapBothWays(x => x.CrossStreetPrefix);
            _vmTester.CanMapBothWays(x => x.CrossStreet);
            _vmTester.CanMapBothWays(x => x.CrossStreetSuffix);
            _vmTester.CanMapBothWays(x => x.ProjectName);
            _vmTester.CanMapBothWays(x => x.MapPage);
            _vmTester.CanMapBothWays(x => x.TaskNumber);
            _vmTester.CanMapBothWays(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            _vmTester.CanMapBothWays(x => x.Town, GetEntityFactory<Town>().Create());
            _vmTester.CanMapBothWays(x => x.Coordinate, GetEntityFactory<Coordinate>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.DateInstalled);
            ValidationAssert.PropertyIsRequired(x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(x => x.Town);
            ValidationAssert.PropertyIsRequired(x => x.Coordinate);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.Comments, AsBuiltImage.StringLengths.COMMENTS);
            ValidationAssert.PropertyHasMaxStringLength(x => x.CrossStreet, AsBuiltImage.StringLengths.CROSS_STREET);
            ValidationAssert.PropertyHasMaxStringLength(x => x.Street, AsBuiltImage.StringLengths.STREET);
            ValidationAssert.PropertyHasMaxStringLength(x => x.StreetPrefix, AsBuiltImage.StringLengths.STREET_PREFIX);
            ValidationAssert.PropertyHasMaxStringLength(x => x.StreetSuffix, AsBuiltImage.StringLengths.STREET_SUFFIX);
            ValidationAssert.PropertyHasMaxStringLength(x => x.ProjectName, AsBuiltImage.StringLengths.PROJECT_NAME);
            ValidationAssert.PropertyHasMaxStringLength(x => x.TaskNumber, AsBuiltImage.StringLengths.TASK_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(x => x.CrossStreetPrefix, AsBuiltImage.StringLengths.XSTREET_PREFIX);
            ValidationAssert.PropertyHasMaxStringLength(x => x.CrossStreetSuffix, AsBuiltImage.StringLengths.XSTREET_SUFFIX);
            ValidationAssert.PropertyHasMaxStringLength(x => x.ApartmentNumber, AsBuiltImage.StringLengths.APARTMENT_NUMBER);
        }

        #endregion

        #region MapToEntity

        [TestMethod]
        public void TestMapToEntitySetsCoordinatesModifiedOnToNowWhenCoordinateIsSet()
        {
            var now = _dateTimeProvider.Object.GetCurrentDate();
             
            var c = GetFactory<CoordinateFactory>().Create();
            _viewModel.Coordinate = c.Id;
            Assert.IsNull(_entity.CoordinatesModifiedOn);
            Assert.IsFalse(_viewModel.CoordinateChanged);
            Assert.IsNull(_entity.CoordinatesModifiedBy);
            _vmTester.MapToEntity();
            Assert.AreEqual(now, _entity.CoordinatesModifiedOn);
            Assert.IsTrue(_viewModel.CoordinateChanged);
            Assert.AreEqual(_user.UserName, _entity.CoordinatesModifiedBy);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotSetCoordinatesModifiedOnIfCoordinateIsNotSet()
        {
            _viewModel.Coordinate = null;
            _entity.Coordinate = null; // needs to be nulled out because the factory is setting this by default.
            _entity.CoordinatesModifiedOn = null;
            Assert.IsNull(_entity.CoordinatesModifiedOn);
            Assert.IsFalse(_viewModel.CoordinateChanged);
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.CoordinatesModifiedOn);
            Assert.IsFalse(_viewModel.CoordinateChanged);
        }

        #endregion
    }

    [TestClass]
    public class CreateAsBuiltImageTest : AsBuiltImageViewModelTest<CreateAsBuiltImage>
    {
        #region Fields

        private Mock<IAsBuiltImageRepository> _mockAsBuiltRepoForFileValidation;

        #endregion

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _mockAsBuiltRepoForFileValidation = e.For<IAsBuiltImageRepository>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel.FileUpload = new AjaxFileUpload {
                FileName = "Some file.tif",
                BinaryData = new byte[] { 1, 2, 3 }
            };
            _mockAsBuiltRepoForFileValidation = new Mock<IAsBuiltImageRepository>();
            _container.Inject(_mockAsBuiltRepoForFileValidation.Object);
        }

        #region Tests

        [TestMethod]
        public void TestMapToEntitySetsFileName()
        {
            _viewModel.FileUpload.FileName = "NEATO!";
            _vmTester.MapToEntity();
            Assert.AreEqual("NEATO!", _entity.FileName);
        }

        [TestMethod]
        public void TestMapToEntitySetsImageData()
        {
            var expected = new byte[] { 1, 2, 3 };
            _viewModel.FileUpload.BinaryData = expected;
            _vmTester.MapToEntity();
            Assert.AreSame(expected, _entity.ImageData);
        }

        [TestMethod]
        public void TestValidationFailsIfUploadedFileAlreadyExists()
        {
            var town = GetFactory<TownFactory>().Create();
            _viewModel.Town = town.Id;
            _viewModel.FileUpload = new AjaxFileUpload
            {
                FileName = "Some file.tif",
                BinaryData = new byte[] { 1, 2, 3 }
            };

            _mockAsBuiltRepoForFileValidation.Setup(x => x.FileExists(_viewModel.FileUpload.FileName, town)).Returns(true);

            ValidationAssert.ModelStateHasError(_viewModel, "FileUpload.Key", "The file uploaded already exists. Please rename and upload again.");
        }

        #endregion
    }

    [TestClass]
    public class EditAsBuiltImageTest : AsBuiltImageViewModelTest<EditAsBuiltImage> { }
}
