using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class AllocationPermitTest : MapCallMvcInMemoryDatabaseTestBase<AllocationPermit>
    {
        #region Fields

        private AllocationPermit _entity;
        private AllocationPermitViewModel _target;
        private ViewModelTester<AllocationPermitViewModel, AllocationPermit> _vmTester;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IPublicWaterSupplyRepository>().Use<PublicWaterSupplyRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _entity = _container.GetInstance<TestDataFactory<AllocationPermit>>().Create();
            _target = _container.GetInstance<AllocationPermitViewModel>();
            _vmTester = new ViewModelTester<AllocationPermitViewModel, AllocationPermit>(_target, _entity);
        }

        #endregion
   
        [TestMethod]
        public void TestMapping()
        {
            _vmTester.CanMapToViewModel(x => x.Id, 626);
            _vmTester.DoesNotMapToEntity(x => x.Id, 262);

            _vmTester.CanMapBothWays(x => x.CreatedAt);
            _vmTester.CanMapBothWays(x => x.System);
            _vmTester.CanMapBothWays(x => x.SurfaceSupply);
            _vmTester.CanMapBothWays(x => x.GroundSupply);
            _vmTester.CanMapBothWays(x => x.GeologicalFormation);
            _vmTester.CanMapBothWays(x => x.ActivePermit);
            _vmTester.CanMapBothWays(x => x.EffectiveDateOfPermit);
            _vmTester.CanMapBothWays(x => x.RenewalApplicationDate);
            _vmTester.CanMapBothWays(x => x.ExpirationDate);
            _vmTester.CanMapBothWays(x => x.SubAllocationNumber);
            _vmTester.CanMapBothWays(x => x.Gpd);
            _vmTester.CanMapBothWays(x => x.Mgm);
            _vmTester.CanMapBothWays(x => x.Mgy);
            _vmTester.CanMapBothWays(x => x.PermitType);
            _vmTester.CanMapBothWays(x => x.PermitFee);
            _vmTester.CanMapBothWays(x => x.SourceDescription);
            _vmTester.CanMapBothWays(x => x.SourceRestrictions);
            _vmTester.CanMapBothWays(x => x.PermitNotes);
            _vmTester.CanMapBothWays(x => x.Gpm);
        }

        [TestMethod]
        public void TestViewModelMapSetsPropertiesAndIds()
        {
            var publicWaterSupply = _container.GetInstance<TestDataFactory<PublicWaterSupply>>().Create();
            var environmentalPermit = _container.GetInstance<TestDataFactory<EnvironmentalPermit>>().Create(new { Description = "all cat" });
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var allocationPermit = _container.GetInstance<TestDataFactory<AllocationPermit>>().Create(new
            {
                PublicWaterSupply = publicWaterSupply,
                EnvironmentalPermit = environmentalPermit,
                OperatingCenter = operatingCenter
            });

            var target = new AllocationPermitViewModel(_container);
            target.Map(allocationPermit);

            Assert.AreEqual(publicWaterSupply.Id, target.PublicWaterSupply);
            Assert.AreEqual(operatingCenter.Id, target.OperatingCenter);
            Assert.AreEqual(environmentalPermit.Id, target.EnvironmentalPermit);
        }

        [TestMethod]
        public void TestViewModelMapToEntitySetsProperties()
        {
            var publicWaterSupply = _container.GetInstance<TestDataFactory<PublicWaterSupply>>().Create();
            var environmentalPermit = _container.GetInstance<TestDataFactory<EnvironmentalPermit>>().Create(new { Description = "all cat" });
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var target = new AllocationPermitViewModel(_container)
            {
                PublicWaterSupply = publicWaterSupply.Id,
                EnvironmentalPermit = environmentalPermit.Id,
                OperatingCenter = operatingCenter.Id
            };

            var entity = new AllocationPermit();
            target.MapToEntity(entity);

            Assert.AreEqual(publicWaterSupply.Id, entity.PublicWaterSupply.Id);
            Assert.AreEqual(environmentalPermit.Id, entity.EnvironmentalPermit.Id);
            Assert.AreEqual(operatingCenter.Id, entity.OperatingCenter.Id);
        }
    }
}