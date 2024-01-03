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
    public class EditTapImageTest : MapCallMvcInMemoryDatabaseTestBase<TapImage>
    {
        #region Fields

        private ViewModelTester<EditTapImage, TapImage> _vmTester;
        private EditTapImage _viewModel;
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
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IServiceRepository>().Use<ServiceRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
            _mockImageRepo = e.For<ITapImageRepository>().Mock();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _viewModel = new EditTapImage(_container);
            _entity = new TapImage();
            _vmTester = new ViewModelTester<EditTapImage, TapImage>(_viewModel, _entity);
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true }); 
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
            _vmTester.CanMapBothWays(x => x.OfficeReviewRequired);
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
            var matl = GetEntityFactory<ServiceMaterial>().Create(new { Description = "Foo" });
            _entity.PreviousServiceMaterial = matl;

            _vmTester.MapToViewModel();

            Assert.AreEqual(matl.Id, _viewModel.PreviousServiceMaterial);

            _entity.PreviousServiceMaterial = null;
            _vmTester.MapToEntity();

            Assert.AreSame(matl, _entity.PreviousServiceMaterial);
        }

        [TestMethod]
        public void TestMapSetStreetIdentifyingIntegerFromServiceIfServiceAndServiceStreetAreNotNull()
        {
            var street = GetFactory<StreetFactory>().Create();
            var service = GetFactory<ServiceFactory>().Create();

            _entity.Service = null;
            _viewModel.StreetIdentifyingInteger = null;
            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.StreetIdentifyingInteger);

            service.Street = null;
            _entity.Service = service;
            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.StreetIdentifyingInteger);

            service.Street = street;
            _vmTester.MapToViewModel();
            Assert.AreEqual(street.Id, _viewModel.StreetIdentifyingInteger);
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

            var s = GetFactory<StreetFactory>().Create(new {
                Name = "SomeStreet",
                Prefix = GetEntityFactory<StreetPrefix>().Create(new{Description =  "Z"}),
                Suffix = GetEntityFactory<StreetSuffix>().Create(new {Description = "St"})
            });

            _viewModel.StreetIdentifyingInteger = s.Id;

            _vmTester.MapToEntity();

            Assert.AreEqual("SomeStreet", _entity.Street);
            Assert.AreEqual("Z", _entity.StreetPrefix);
            Assert.AreEqual("St", _entity.StreetSuffix);
        }

        [TestMethod]
        public void TestMapSetCrossStreetIdentifyingIntegerFromServiceIfServiceAndServiceStreetAreNotNull()
        {
            var street = GetFactory<StreetFactory>().Create();
            var service = GetFactory<ServiceFactory>().Create();

            _entity.Service = null;
            _viewModel.CrossStreetIdentifyingInteger = null;
            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.CrossStreetIdentifyingInteger);

            service.CrossStreet = null;
            _entity.Service = service;
            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.CrossStreetIdentifyingInteger);

            service.CrossStreet = street;
            _vmTester.MapToViewModel();
            Assert.AreEqual(street.Id, _viewModel.CrossStreetIdentifyingInteger);
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

        #endregion

    }
}
