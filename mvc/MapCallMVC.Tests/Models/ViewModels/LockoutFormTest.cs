using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class LockoutFormTest : MapCallMvcInMemoryDatabaseTestBase<LockoutForm>
    {
        #region Fields

        private LockoutFormViewModel _viewModel;
        private LockoutForm _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private ViewModelTester<LockoutFormViewModel, LockoutForm> _vmTester;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private DateTime _now;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<IEquipmentRepository>().Use<EquipmentRepository>();
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<ISensorRepository>().Use<SensorRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _authServ = new Mock<IAuthenticationService<User>>();
            _user = new User { UserName = "rystroaconstrictor"};
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _now = DateTime.Now;
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now);

            _container.Inject(_dateTimeProvider.Object);
            _container.Inject(_authServ.Object);

            _entity = GetEntityFactory<LockoutForm>().Create();
            _viewModel = _viewModelFactory.Build<LockoutFormViewModel, LockoutForm>(_entity);
            _vmTester = new ViewModelTester<LockoutFormViewModel, LockoutForm>(_viewModel, _entity);
        }

        #endregion

        #region Mapping
        
        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            var factoryService = _container.GetInstance<ITestDataFactoryService>();

            _vmTester.CanMapBothWays(x => x.AdditionalLockoutNotes);
            _vmTester.CanMapBothWays(x => x.LocationOfLockoutNotes);
            _vmTester.CanMapBothWays(x => x.LockoutDateTime);
            _vmTester.CanMapBothWays(x => x.OutOfServiceDateTime);
            _vmTester.CanMapBothWays(x => x.ReasonForLockout);
            _vmTester.CanMapBothWays(x => x.OutOfServiceAuthorizedEmployee, factoryService);
            _vmTester.CanMapBothWays(x => x.OperatingCenter, factoryService);
            _vmTester.CanMapBothWays(x => x.Facility, factoryService);
            _vmTester.CanMapBothWays(x => x.Equipment, factoryService);
            _vmTester.CanMapBothWays(x => x.LockoutReason, factoryService);
            _vmTester.CanMapBothWays(x => x.LockoutDevice, factoryService);
            _vmTester.CanMapBothWays(x => x.IsolationPoint, factoryService);
            _vmTester.CanMapBothWays(x => x.ProductionWorkOrder, factoryService);
        }
        
        #endregion

        #region MapToEntity

        [TestMethod]
        public void TestMapToEntityCreatesContractorIfItDoesNotExist()
        {
            var lockoutForm = GetEntityFactory<LockoutForm>().Create();
            var target = _viewModelFactory.BuildWithOverrides<LockoutFormViewModel, LockoutForm>(lockoutForm, new { ContractorName = "bah" });

            target.MapToEntity(lockoutForm);

            Assert.IsNotNull(target.Contractor);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotCreateContractorIfSelected()
        {
            var contractorName = "bah";
            var contractor = GetEntityFactory<Contractor>().Create(new {Name  = contractorName});
            var lockoutForm = GetEntityFactory<LockoutForm>().Create();
            var target = _viewModelFactory.BuildWithOverrides<LockoutFormViewModel, LockoutForm>(lockoutForm, new { Contractor = contractor.Id });

            target.MapToEntity(lockoutForm);

            Assert.AreEqual(contractor.Id, target.Contractor);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotCreateContractorIfItAlreadyExists()
        {
            var contractorName = "bah";
            var contractor = GetEntityFactory<Contractor>().Create(new { Name = contractorName });
            var lockoutForm = GetEntityFactory<LockoutForm>().Create();
            var target = _viewModelFactory.BuildWithOverrides<LockoutFormViewModel, LockoutForm>(lockoutForm, new { ContractorName = contractorName });

            target.MapToEntity(lockoutForm);

            Assert.AreEqual(contractor.Id, target.Contractor);
        }

        #endregion
    }
}
