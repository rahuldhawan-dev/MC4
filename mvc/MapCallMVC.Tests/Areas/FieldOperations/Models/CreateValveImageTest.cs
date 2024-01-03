using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
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
    public class CreateValveImageTest : MapCallMvcInMemoryDatabaseTestBase<ValveImage>
    {
        #region Fields

        private ViewModelTester<CreateValveImage, ValveImage> _vmTester;
        private CreateValveImage _viewModel;
        private ValveImage _entity;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IValveImageRepository> _mockImageRepo;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            _mockImageRepo = e.For<IValveImageRepository>().Mock();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IValveRepository>().Use<ValveRepository>();
            e.For<IValveNormalPositionRepository>().Use<ValveNormalPositionRepository>();
            e.For<IValveOpenDirectionRepository>().Use<ValveOpenDirectionRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _viewModel = new CreateValveImage(_container);
            _viewModel.FileUpload = new AjaxFileUpload
            {
                FileName = "Some file.tif",
                BinaryData = new byte[] { 1, 2, 3 }
            };

            _entity = new ValveImage();
            _container.BuildUp(_entity);
            _vmTester = new ViewModelTester<CreateValveImage, ValveImage>(_viewModel, _entity);
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true});
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestPropertiesThatMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.CrossStreet);
            _vmTester.CanMapBothWays(x => x.CrossStreetPrefix);
            _vmTester.CanMapBothWays(x => x.CrossStreetSuffix);
            _vmTester.CanMapBothWays(x => x.DateCompleted);
            _vmTester.CanMapBothWays(x => x.IsDefaultImageForValve);
            _vmTester.CanMapBothWays(x => x.Location);
            _vmTester.CanMapBothWays(x => x.ValveSize);
            _vmTester.CanMapBothWays(x => x.NumberOfTurns);
            _vmTester.CanMapBothWays(x => x.Street);
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.StreetPrefix);
            _vmTester.CanMapBothWays(x => x.StreetSuffix);
            _vmTester.CanMapBothWays(x => x.TownSection);
            _vmTester.CanMapBothWays(x => x.ValveNumber);
        }

        [TestMethod]
        public void TestOpenDirectionCanMapBothWays()
        {
            var opc = GetFactory<ValveOpenDirectionFactory>().Create();
            _entity.OpenDirection = opc;
            _vmTester.MapToViewModel();
            Assert.AreEqual(opc.Id, _viewModel.OpenDirection);

            _entity.OpenDirection = null;
            _vmTester.MapToEntity();
            Assert.AreSame(opc, _entity.OpenDirection);
        }

        [TestMethod]
        public void TestNormalPositionCanMapBothWays()
        {
            var opc = GetFactory<ValveNormalPositionFactory>().Create();
            _entity.NormalPosition = opc;
            _vmTester.MapToViewModel();
            Assert.AreEqual(opc.Id, _viewModel.NormalPosition);

            _entity.NormalPosition = null;
            _vmTester.MapToEntity();
            Assert.AreSame(opc, _entity.NormalPosition);
        }
        
        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            _entity.OperatingCenter = opc;
            _vmTester.MapToViewModel();
            Assert.AreEqual(opc.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();
            Assert.AreSame(opc, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestTownCanMapBothWays()
        {
            var t = GetFactory<TownFactory>().Create();
            _entity.Town = t;
            _vmTester.MapToViewModel();
            Assert.AreEqual(t.Id, _viewModel.Town);

            _entity.Town = null;
            _vmTester.MapToEntity();
            Assert.AreSame(t, _entity.Town);
        }

        [TestMethod]
        public void TestValveCanMapBothWays()
        {
            var v = GetFactory<ValveFactory>().Create();
            _entity.Valve = v;
            _vmTester.MapToViewModel();
            Assert.AreEqual(v.Id, _viewModel.Valve);

            _entity.Valve = null;
            _vmTester.MapToEntity();
            Assert.AreSame(v, _entity.Valve);
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

            var s = GetFactory<StreetFactory>().Create(new {
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
        

        #endregion

        #region Validation

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.CrossStreet);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsDefaultImageForValve, BaseValveImageViewModel.ErrorMessages.IS_DEFAULT_IMAGE);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ValveSize);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FileUpload);
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
        public void TestValveEntityMustExist()
        {
            var v = GetFactory<ValveFactory>().Create();
            ValidationAssert.EntityMustExist(_viewModel, x => x.Valve, v);
        }

        [TestMethod]
        public void TestValveOpenDirectionEntityMustExist()
        {
            var v = GetFactory<ValveOpenDirectionFactory>().Create();
            ValidationAssert.EntityMustExist(_viewModel, x => x.OpenDirection, v);
        }

        [TestMethod]
        public void TestValveNormalPositionEntityMustExist()
        {
            var v = GetFactory<ValveNormalPositionFactory>().Create();
            ValidationAssert.EntityMustExist(_viewModel, x => x.NormalPosition, v);
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

        #endregion

        #region SetDefaults

        [TestMethod]
        public void TestSetDefaultsSetsDefaults()
        {
            var town = GetEntityFactory<Town>().Create();
            var townSection = GetFactory<TownSectionFactory>().Create(new {
                Town = town
            });
            var s = GetEntityFactory<StreetPrefix>().Create(new {Description = "S"});
            var street = GetEntityFactory<Street>().Create(new {
                Suffix = GetEntityFactory<StreetSuffix>().Create(new {Description = "BLVD"}),
                Prefix = s,
                Town = town, FullStName = "S HOLLYWOOD BLVD", Name = "HOLLYWOOD"
            });
            var crossStreet = GetEntityFactory<Street>().Create(new {
                Suffix = GetEntityFactory<StreetSuffix>().Create(new {Description = "St"}),
                Prefix = s,
                Town = town, FullStName = "S VINE ST", Name = "VINE"
            });
            var valveOpensDirection = GetEntityFactory<ValveOpenDirection>().Create();
            var valveSize = GetEntityFactory<ValveSize>().Create();
            var normalPosition = GetEntityFactory<ValveNormalPosition>().Create();
            var valve = GetFactory<ValveFactory>().Create(new {
                Turns = 5m,
                TownSection = townSection,
                Town = town, 
                Street = street,
                StreetNumber = "123",
                ValveLocation = "corner",
                DateInstalled = DateTime.Now.AddHours(-1),
                CrossStreet = crossStreet,
                OpenDirection = valveOpensDirection,
                ValveSize = valveSize,
                NormalPosition = normalPosition
            });
            
            _viewModel.Valve = valve.Id;

            _viewModel.SetDefaults();

            Assert.AreEqual(valve.OperatingCenter.Id, _viewModel.OperatingCenter);
            Assert.AreEqual(valve.Town.Id, _viewModel.Town);
            Assert.AreEqual(valve.StreetNumber, _viewModel.StreetNumber);
            Assert.AreEqual(valve.ValveNumber, _viewModel.ValveNumber);
            Assert.AreEqual(valve.ValveLocation, _viewModel.Location);
            Assert.AreEqual(valve.Turns.ToString(), _viewModel.NumberOfTurns);
            Assert.AreEqual(String.Format(CommonStringFormats.DATE, valve.DateInstalled), _viewModel.DateCompleted);
            Assert.AreEqual(valve.Street.Id, _viewModel.StreetIdentifyingInteger);
            Assert.AreEqual(street.Name, _viewModel.Street);
            Assert.AreEqual(street.Prefix.Description, _viewModel.StreetPrefix);
            Assert.AreEqual(street.Suffix.Description, _viewModel.StreetSuffix);
            Assert.AreEqual(townSection.Description, _viewModel.TownSection);
            Assert.AreEqual(crossStreet.Name, _viewModel.CrossStreet);
            Assert.AreEqual(crossStreet.Prefix.Description, _viewModel.CrossStreetPrefix);
            Assert.AreEqual(crossStreet.Suffix.Description, _viewModel.CrossStreetSuffix);
            Assert.AreEqual(normalPosition.Id, _viewModel.NormalPosition);
            Assert.AreEqual(valveSize.Description, _viewModel.ValveSize);
            Assert.AreEqual(valveOpensDirection.Id, _viewModel.OpenDirection);
        }

        #endregion

        #endregion
    }
}
