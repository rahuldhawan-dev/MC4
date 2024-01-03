using System;
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
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class CreateTapImageTest : MapCallMvcInMemoryDatabaseTestBase<TapImage>
    {
        #region Fields

        private ViewModelTester<CreateTapImage, TapImage> _vmTester;
        private CreateTapImage _viewModel;
        private TapImage _entity;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<ITapImageRepository> _mockImageRepo;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            _mockImageRepo = e.For<ITapImageRepository>().Mock();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IServiceRepository>().Use<ServiceRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _viewModel = new CreateTapImage(_container);
            _viewModel.FileUpload = new AjaxFileUpload
            {
                FileName = "Some file.tif",
                BinaryData = new byte[] { 1, 2, 3 }
            };
            _entity = new TapImage();
            _vmTester = new ViewModelTester<CreateTapImage, TapImage>(_viewModel, _entity);
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true}); 
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ApartmentNumber);
            _vmTester.CanMapBothWays(x => x.Block);
            _vmTester.CanMapBothWays(x => x.DateCompleted);
            _vmTester.CanMapBothWays(x => x.IsDefaultImageForService);
            _vmTester.CanMapBothWays(x => x.LengthOfService);
            _vmTester.CanMapBothWays(x => x.Lot);
            _vmTester.CanMapBothWays(x => x.MainSize);
            _vmTester.CanMapBothWays(x => x.PremiseNumber);
            _vmTester.CanMapBothWays(x => x.ServiceNumber);
            _vmTester.CanMapBothWays(x => x.ServiceType);
            _vmTester.CanMapBothWays(x => x.Street);
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.StreetPrefix);
            _vmTester.CanMapBothWays(x => x.StreetSuffix);
            _vmTester.CanMapBothWays(x => x.TownSection);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var fk = GetFactory<OperatingCenterFactory>().Create();
            _entity.OperatingCenter = fk;
            _vmTester.MapToViewModel();
            Assert.AreEqual(fk.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();
            Assert.AreSame(fk, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestTownCanMapBothWays()
        {
            var fk = GetFactory<TownFactory>().Create();
            _entity.Town = fk;
            _vmTester.MapToViewModel();
            Assert.AreEqual(fk.Id, _viewModel.Town);

            _entity.Town = null;
            _vmTester.MapToEntity();
            Assert.AreSame(fk, _entity.Town);
        }

        [TestMethod]
        public void TestServiceCanMapBothWays()
        {
            var fk = GetFactory<ServiceFactory>().Create();
            _entity.Service = fk;
            _vmTester.MapToViewModel();
            Assert.AreEqual(fk.Id, _viewModel.Service);

            _entity.Service = null;
            _vmTester.MapToEntity();
            Assert.AreSame(fk, _entity.Service);
        }

        [TestMethod]
        public void TestPreviousServiceSizeCanMapBothWays()
        {
            var size = GetEntityFactory<ServiceSize>().Create(new { ServiceSizeDescription = "3/5", Size = 0.75m });
            _entity.PreviousServiceSize = size;

            _vmTester.MapToViewModel();

            Assert.AreEqual(size.Id, _viewModel.PreviousServiceSize);

            _entity.PreviousServiceSize = null;
            _vmTester.MapToEntity();

            Assert.AreSame(size, _entity.PreviousServiceSize);
        }

        [TestMethod]
        public void TestPreviouServiceMaterialCanMapBothWays()
        {
            var matl = GetEntityFactory<ServiceMaterial>().Create(new {Description = "Foo"});
            _entity.PreviousServiceMaterial = matl;

            _vmTester.MapToViewModel();

            Assert.AreEqual(matl.Id, _viewModel.PreviousServiceMaterial);

            _entity.PreviousServiceMaterial = null;
            _vmTester.MapToEntity();

            Assert.AreSame(matl, _entity.PreviousServiceMaterial);
        }

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
        public void TestMapToEntitySetsStreetInformationIfStreetIdentifyingIntegerIsSet()
        {
            _viewModel.StreetIdentifyingInteger = null;
            _viewModel.Street = "NoStreetId";
            _viewModel.StreetPrefix = "Q";
            _viewModel.StreetSuffix = "Ave";

            _vmTester.MapToEntity();

            Assert.AreEqual("NoStreetId", _entity.Street);
            Assert.AreEqual("Q", _entity.StreetPrefix);
            Assert.AreEqual("Ave", _entity.StreetSuffix);

            var s = GetFactory<StreetFactory>().Create(new
            {
                Name = "SomeStreet",
                Prefix = GetEntityFactory<StreetPrefix>().Create(new { Description = "Z" }),
                Suffix = GetEntityFactory<StreetSuffix>().Create(new { Description = "St" })
            });

            _viewModel.StreetIdentifyingInteger = s.Id;

            _vmTester.MapToEntity();

            Assert.AreEqual("SomeStreet", _entity.Street);
            Assert.AreEqual("Z", _entity.StreetPrefix);
            Assert.AreEqual("St", _entity.StreetSuffix);
        }

        [TestMethod]
        public void TestMapToEntitySetsCrossStreetInformationIfCrossStreetIdentifyingIntegerIsSet()
        {
            _viewModel.CrossStreetIdentifyingInteger = null;
            _viewModel.CrossStreet = "NoCrossStreetId";

            _vmTester.MapToEntity();

            Assert.AreEqual("NoCrossStreetId", _entity.CrossStreet);

            var s = GetFactory<StreetFactory>().Create(new
            {
                Name = "SomeCrossStreet",
                Prefix = GetEntityFactory<StreetPrefix>().Create(new { Description = "Z" }),
                Suffix = GetEntityFactory<StreetSuffix>().Create(new { Description = "St" })
            });

            _viewModel.CrossStreetIdentifyingInteger = s.Id;

            _vmTester.MapToEntity();

            Assert.AreEqual("Z SomeCrossStreet St", _entity.CrossStreet);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsDefaultImageForService);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FileUpload);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ServiceType);
        }

        [TestMethod]
        public void TestOperatingCenterEntityMustExist()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, opc);
        }

        [TestMethod]
        public void TestTownEntityMustExist()
        {
            var t = GetFactory<TownFactory>().Create();
            ValidationAssert.EntityMustExist(_viewModel, x => x.Town, t);
        }

        [TestMethod]
        public void TestServiceEntityMustExist()
        {
            var v = GetFactory<ServiceFactory>().Create();
            ValidationAssert.EntityMustExist(_viewModel, x => x.Service, v);
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

            _mockImageRepo.Setup(x => x.FileExists(_viewModel.FileUpload.FileName, town))
                .Returns(true);

            ValidationAssert.ModelStateHasError(_viewModel, "FileUpload.Key", "The file uploaded already exists. Please rename and upload again.");
        }

        [TestMethod]
        public void TestSetDefaultsSetsDefaults()
        {
            var opcntr = GetEntityFactory<OperatingCenter>().Create();
            var serviceCategory = GetEntityFactory<ServiceCategory>().Create();
            var street = GetEntityFactory<Street>().Create();
            var crossStreet = GetEntityFactory<Street>().Create();
            var serviceType = GetEntityFactory<ServiceType>().Create(new { OperatingCenter = opcntr, ServiceCategory = serviceCategory, Description = "Blergh" });
            var serviceSize = GetEntityFactory<ServiceSize>().Create();
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create();
            var service = GetFactory<ServiceFactory>().Create(new {
                OperatingCenter = opcntr,
                Street = street,
                CrossStreet = crossStreet,
                ApartmentNumber = "Garbage",
                ServiceType = serviceType,
                LengthOfService = 1m,
                ServiceSize = serviceSize,
                ServiceMaterial = serviceMaterial, 
                PreviousServiceSize = serviceSize,
                PreviousServiceMaterial = serviceMaterial,
                CustomerSideMaterial = serviceMaterial,
                CustomerSideSize = serviceSize
            });
            _viewModel.Service = service.Id;

            _viewModel.SetDefaults();

            Assert.AreEqual(service.Id, _viewModel.Service);
            Assert.AreEqual(service.OperatingCenter.Id, _viewModel.OperatingCenter);
            Assert.AreEqual(service.Town.Id, _viewModel.Town);
            Assert.AreEqual(service.Street.Id, _viewModel.StreetIdentifyingInteger);
            Assert.AreEqual(service.CrossStreet.Id, _viewModel.CrossStreetIdentifyingInteger);
            Assert.AreEqual(service.ServiceType.Description, _viewModel.ServiceType);
            Assert.AreEqual(service.LengthOfService.ToString(), _viewModel.LengthOfService);
            Assert.AreEqual(service.ServiceSize.Id, _viewModel.ServiceSize);
            Assert.AreEqual(service.ServiceMaterial.Id, _viewModel.ServiceMaterial);
            Assert.AreEqual(service.ServiceSize.Id, _viewModel.PreviousServiceSize);
            Assert.AreEqual(service.ServiceMaterial.Id, _viewModel.PreviousServiceMaterial);
            Assert.AreEqual(service.StreetNumber, _viewModel.StreetNumber);
            Assert.AreEqual(service.PremiseNumber, _viewModel.PremiseNumber);
            Assert.AreEqual(service.ServiceNumber.ToString(), _viewModel.ServiceNumber);
            Assert.AreEqual(service.Lot, _viewModel.Lot);
            Assert.AreEqual(service.Block, _viewModel.Block);
            Assert.AreEqual(service.DateInstalled, _viewModel.DateCompleted);
            Assert.AreEqual(false, _viewModel.IsDefaultImageForService);
            Assert.AreEqual(serviceMaterial.Id, _viewModel.CustomerSideMaterial);
            Assert.AreEqual(serviceSize.Id, _viewModel.CustomerSideSize);
            Assert.AreEqual(service.ApartmentNumber, _viewModel.ApartmentNumber);
        }

        #endregion
    }
}
