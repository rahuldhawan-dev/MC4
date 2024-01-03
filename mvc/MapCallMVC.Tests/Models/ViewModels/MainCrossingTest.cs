using System;
using System.Linq;
using System.Linq.Expressions;
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
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using StructureMap;

/*
 * TODO:
 * Add ConstructionType, CrossingType
 */
namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CreateMainCrossingTest : MapCallMvcInMemoryDatabaseTestBase<MainCrossing>
    {
        #region Fields

        private MainCrossing _entity;
        private CreateMainCrossing _target;
        private ViewModelTester<CreateMainCrossing, MainCrossing> _vmTester;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
            e.For<IPublicWaterSupplyRepository>().Use<PublicWaterSupplyRepository>();
            e.For<IRecurringFrequencyUnitRepository>().Use<RecurringFrequencyUnitRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetFactory<MainCrossingFactory>().Create();
            _target = new CreateMainCrossing(_container);
            _vmTester = new ViewModelTester<CreateMainCrossing, MainCrossing>(_target, _entity);
            _user = GetFactory<UserFactory>().Create(new {IsAdmin = true});
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_target, x => x.Street);
            ValidationAssert.PropertyIsRequired(_target, x => x.ClosestCrossStreet);
        }
    }

    [TestClass]
    public class MainCrossingTest : MapCallMvcInMemoryDatabaseTestBase<MainCrossing>
    {
        #region Fields

        private MainCrossing _entity;
        private MainCrossingViewModel _target;
        private ViewModelTester<MainCrossingViewModel, MainCrossing> _vmTester;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
            e.For<IPublicWaterSupplyRepository>().Use<PublicWaterSupplyRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetFactory<MainCrossingFactory>().Create();
            _target = new MainCrossingViewModel(_container);
            _vmTester = new ViewModelTester<MainCrossingViewModel, MainCrossing>(_target, _entity);
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestMappings()
        {
            _vmTester.CanMapToViewModel(x => x.Id, 13);
            _vmTester.CanMapBothWays(x => x.IsCompanyOwned);
            _vmTester.CanMapBothWays(x => x.LengthOfSegment);
            _vmTester.CanMapBothWays(x => x.IsCriticalAsset);
            _vmTester.CanMapBothWays(x => x.MaximumDailyFlow);
            _vmTester.CanMapBothWays(x => x.Comments);
            _vmTester.CanMapBothWays(x => x.PressureSurgePotentialType, GetEntityFactory<PressureSurgePotentialType>().Create());
            _vmTester.CanMapBothWays(x => x.TypicalOperatingPressureRange, GetEntityFactory<TypicalOperatingPressureRange>().Create());
        }

        [TestMethod]
        public void TestViewModelMapSetsPropertiesAndIds()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var street = GetFactory<StreetFactory>().Create();
            var coordinate = GetFactory<CoordinateFactory>().Create();
            var bodyOfWater = _container.GetInstance<TestDataFactory<BodyOfWater>>().Create();
            var crossingCategory = _container.GetInstance<TestDataFactory<CrossingCategory>>().Create(new { Description = "Foo"});
            var pressureZone = _container.GetInstance<TestDataFactory<PressureZone>>().Create(new { Description = "Bar" });
            var customerRange = _container.GetInstance<TestDataFactory<CustomerRange>>().Create(new { Description = "Baz" });
            var publicWaterSupply = GetFactory<PublicWaterSupplyFactory>().Create();
            var pipeDiameter = GetFactory<PipeDiameterFactory>().Create();
            var pipeMaterial = GetFactory<PipeMaterialFactory>().Create();
            var supportStructure = _container.GetInstance<TestDataFactory<SupportStructure>>().Create(new { Description = "Bah" });
            var crossingType = _container.GetInstance<TestDataFactory<CrossingType>>().Create(new {Description = "blergh"});
            var constructionType = _container.GetInstance<TestDataFactory<ConstructionType>>().Create(new { Description = "flergh" });
            var mainCrossingStatus = GetFactory<MainCrossingStatusFactory>().Create();
            var mainCrossing = GetFactory<MainCrossingFactory>().Create(new {
                OperatingCenter = operatingCenter,
                Town = town,
                ClosestCrossStreet = street,
                Coordinate = coordinate,
                BodyOfWater = bodyOfWater,
                CrossingCategory = crossingCategory,
                PressureZone = pressureZone,
                CustomerRange = customerRange,
                PWSID = publicWaterSupply,
                MainMaterial = pipeMaterial,
                MainDiameter = pipeDiameter,
                SupportStructure = supportStructure,
                ConstructionType = constructionType,
                CrossingType = crossingType,
                MainCrossingStatus = mainCrossingStatus
            });

            var target = new MainCrossingViewModel(_container);
            target.Map(mainCrossing);

            Assert.AreEqual(operatingCenter.Id, target.OperatingCenter);
            Assert.AreEqual(town.Id, target.Town);
            Assert.AreEqual(coordinate.Id, target.CoordinateId);
            Assert.AreEqual(bodyOfWater.Id, target.BodyOfWater);
            Assert.AreEqual(crossingCategory.Id, target.CrossingCategory);
            Assert.AreEqual(pressureZone.Id, target.PressureZone);
            Assert.AreEqual(customerRange.Id, target.CustomerRange);
            Assert.AreEqual(publicWaterSupply.Id, target.PWSID);
            Assert.AreEqual(pipeMaterial.Id, target.MainMaterial);
            Assert.AreEqual(pipeDiameter.Id, target.MainDiameter);
            Assert.AreEqual(supportStructure.Id, target.SupportStructure);
            Assert.AreEqual(crossingType.Id, target.CrossingType);
            Assert.AreEqual(constructionType.Id, target.ConstructionType);
            Assert.AreEqual(mainCrossingStatus.Id, target.MainCrossingStatus);
        }

        [TestMethod]
        public void TestViewModelMapToEntitySetsProperties()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var street = GetFactory<StreetFactory>().Create();
            var coordinate = GetFactory<CoordinateFactory>().Create();
            var bodyOfWater = _container.GetInstance<TestDataFactory<BodyOfWater>>().Create();
            var crossingCategory = _container.GetInstance<TestDataFactory<CrossingCategory>>().Create(new { Description = "Foo" });
            var pressureZone = _container.GetInstance<TestDataFactory<PressureZone>>().Create(new { Description = "Bar" });
            var customerRange = _container.GetInstance<TestDataFactory<CustomerRange>>().Create(new { Description = "Baz" });
            var publicWaterSupply = GetFactory<PublicWaterSupplyFactory>().Create();
            var pipeDiameter = GetFactory<PipeDiameterFactory>().Create();
            var pipeMaterial = GetFactory<PipeMaterialFactory>().Create();
            var supportStructure = _container.GetInstance<TestDataFactory<SupportStructure>>().Create(new { Description = "Bah" });
            var crossingType = _container.GetInstance<TestDataFactory<CrossingType>>().Create(new { Description = "blergh" });
            var constructionType = _container.GetInstance<TestDataFactory<ConstructionType>>().Create(new { Description = "flergh" });
            var mainCrossingStatus = GetFactory<MainCrossingStatusFactory>().Create();

            var target = new MainCrossingViewModel(_container) {
                OperatingCenter = operatingCenter.Id,
                Town = town.Id,
                CoordinateId = coordinate.Id,
                BodyOfWater = bodyOfWater.Id,
                CrossingCategory = crossingCategory.Id,
                PressureZone = pressureZone.Id,
                CustomerRange = customerRange.Id,
                PWSID = publicWaterSupply.Id,
                MainDiameter = pipeDiameter.Id,
                MainMaterial = pipeMaterial.Id,
                SupportStructure = supportStructure.Id,
                ConstructionType = constructionType.Id,
                CrossingType = crossingType.Id,
                MainCrossingStatus = mainCrossingStatus.Id
            };

            var entity = new MainCrossing();

            target.MapToEntity(entity);

            Assert.AreEqual(operatingCenter.Id, entity.OperatingCenter.Id);
            Assert.AreEqual(town.Id, entity.Town.Id);
            Assert.AreEqual(coordinate.Id, entity.Coordinate.Id);
            Assert.AreEqual(bodyOfWater.Id, entity.BodyOfWater.Id);
            Assert.AreEqual(crossingCategory.Id, entity.CrossingCategory.Id);
            Assert.AreEqual(pressureZone.Id, entity.PressureZone.Id);
            Assert.AreEqual(customerRange.Id, entity.CustomerRange.Id);
            Assert.AreEqual(publicWaterSupply.Id, entity.PWSID.Id);
            Assert.AreEqual(pipeDiameter.Id, entity.MainDiameter.Id);
            Assert.AreEqual(pipeMaterial.Id, entity.MainMaterial.Id);
            Assert.AreEqual(supportStructure.Id, entity.SupportStructure.Id);
            Assert.AreEqual(crossingType.Id, entity.CrossingType.Id);
            Assert.AreEqual(constructionType.Id, entity.ConstructionType.Id);
            Assert.AreEqual(mainCrossingStatus.Id, entity.MainCrossingStatus.Id);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            var mockRepo = new Mock<IRepository<CrossingCategory>>();
            mockRepo.Setup(x => x.Where(It.IsAny<Expression<Func<CrossingCategory, bool>>>()))
                .Returns(new[] {new CrossingCategory {Id = 1}}.AsQueryable());
            _container.Inject(mockRepo.Object);

            ValidationAssert.PropertyIsRequired(_target, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_target, x => x.CrossingCategory);
            ValidationAssert.PropertyIsRequired(_target, x => x.InspectionFrequency);
            ValidationAssert.PropertyIsRequired(_target, x => x.InspectionFrequencyUnit);//, "The Inspection Frequestn Unit field is required.");
            ValidationAssert.PropertyIsRequired(_target, x => x.MainCrossingStatus);
        }
        

        #endregion
    }
}
