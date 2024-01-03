using System;
using System.Linq.Expressions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
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
    public class EditEquipmentTest : MapCallMvcInMemoryDatabaseTestBase<Equipment>
    {
        #region Private Members

        private Equipment _entity;
        private EditEquipment _target;
        private ViewModelTester<EditEquipment, Equipment> _vmTester;
        private Mock<IAuthenticationService<User>> _authServ;
        private Mock<IEquipmentRepository> _repository;
        private Mock<IFacilityRepository> _facilityRepository;
        private User _user;
        
        #endregion
        
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IEquipmentRepository>().Use((_repository = new Mock<IEquipmentRepository>()).Object);
            e.For<IFacilityRepository>().Use((_facilityRepository = new Mock<IFacilityRepository>()).Object);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetFactory<EquipmentFactory>().Create();
            _target = _viewModelFactory.Build<EditEquipment, Equipment>(_entity);
            _vmTester = new ViewModelTester<EditEquipment, Equipment>(_target, _entity);
            _user = new User { UserName = "mcadmin" };
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        [TestMethod]
        public void TestDateRetired()
        {
            //Date Retired is set to Null when the equipment status is not retired
            _target.EquipmentStatus = EquipmentStatus.Indices.OUT_OF_SERVICE;
            _target.DateRetired = DateTime.Today;
            _target.MapToEntity(_entity);
            Assert.IsNull(_target.DateRetired);

            //Date Retired is persisted when the equipment status is retired
            _target.EquipmentStatus = EquipmentStatus.Indices.RETIRED;
            _target.DateRetired = DateTime.Today;
            _target.MapToEntity(_entity);
            Assert.IsNotNull(_target.DateRetired);
            Assert.AreEqual(_entity.DateRetired, _target.DateRetired);

            //Date Retired is persisted when the equipment status is pending retirement
            _target.EquipmentStatus = EquipmentStatus.Indices.PENDING_RETIREMENT;
            _target.DateRetired = DateTime.Today;
            _target.MapToEntity(_entity);
            Assert.IsNotNull(_target.DateRetired);
            Assert.AreEqual(_entity.DateRetired, _target.DateRetired);
        }

        [TestMethod]
        public void TestSAPEquipmentIdEditableWhenSAPEquipmentIdIsNull()
        {
            _target.SAPEquipmentId = 3;

            Assert.IsFalse(_target.SAPEquipmentIdEditable);

            _target.SAPEquipmentId = null;

            Assert.IsTrue(_target.SAPEquipmentIdEditable);

            _target.SAPEquipmentId = 0;

            Assert.IsTrue(_target.SAPEquipmentIdEditable);
        }

        [TestMethod]
        public void TestSAPEquipmentIdEditableWhenSAPEErrorCodeStartsWithErrorCode()
        {
            _target.SAPEquipmentId = 3;
                
            Assert.IsFalse(_target.SAPEquipmentIdEditable);

            _target.SAPErrorCode = EditEquipment.SAP_ERROR_CODE;

            Assert.IsTrue(_target.SAPEquipmentIdEditable);
        }

        [TestMethod]
        public void TestMapToEntityCancelsAnyAttachedWorkOrders()
        {
            var now = DateTime.Now;
            var reason = GetEntityFactory<WorkOrderCancellationReason>()
                .Create(new {Status = "ARET", Description = "Asset Retired"});
            _entity.WorkOrders = GetFactory<WorkOrderFactory>().CreateList(2);
            _target.DateRetired = now;

            _vmTester.MapToEntity();

            foreach (var workOrder in _entity.WorkOrders)
            {
                Assert.AreEqual(now, workOrder.CancelledAt);
                Assert.AreEqual(reason, workOrder.WorkOrderCancellationReason);
            }
        }

        [TestMethod]
        public void TestCannotSetToExistingSAPEquipmentId()
        {
            var department = GetEntityFactory<Department>().Create();
            var facility = GetEntityFactory<Facility>().Create(new { Department = department });
            _facilityRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(facility);
            var eq1 = GetEntityFactory<Equipment>().Create(new {SAPEquipmentId = 123 });
            var eq2 = GetEntityFactory<Equipment>().Create(new {SAPEquipmentId = 234 });
            _repository.Setup(x => x.Any(It.IsAny<Expression<Func<Equipment,bool>>>())).Returns(true);
            _target = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(eq2, new {eq1.SAPEquipmentId});
            
            ValidationAssert.ModelStateHasError(_target, x => x.SAPEquipmentId, EquipmentViewModel.SAP_EQUIPMENTID_ALREADY_IN_USE);
        }

        [TestMethod]
        public void TestCanSetToExistingSAPEquipmentId()
        {
            var department = GetEntityFactory<Department>().Create();
            var facility = GetEntityFactory<Facility>().Create(new { Department = department });
            _facilityRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(facility);
            var eq1 = GetEntityFactory<Equipment>().Create(new { SAPEquipmentId = 123 });
            _repository.Setup(x => x.Any(It.IsAny<Expression<Func<Equipment, bool>>>())).Returns(false);
            _target = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(eq1, new { SAPEquipmentId = 123 });

            ValidationAssert.ModelStateIsValid(_target, x => x.SAPEquipmentId);
        }

        [TestMethod]
        public void TestCanSetToAnotherSAPEquipmentIdNotInUse()
        {
            var department = GetEntityFactory<Department>().Create();
            var facility = GetEntityFactory<Facility>().Create(new { Department = department });
            _facilityRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(facility);
            var eq1 = GetEntityFactory<Equipment>().Create(new { SAPEquipmentId = 123 });
            _repository.Setup(x => x.Any(It.IsAny<Expression<Func<Equipment, bool>>>())).Returns(false);
            _target = _viewModelFactory.BuildWithOverrides<EditEquipment, Equipment>(eq1, new { SAPEquipmentId = 1234 });

            ValidationAssert.ModelStateIsValid(_target, x => x.SAPEquipmentId);
        }

        [TestMethod]
        public void TestDateInstalledIsNotRequired()
        {
            _target.DateInstalled = null;

            ValidationAssert.ModelStateIsValid(_target, e => e.DateInstalled);
        }

        [TestMethod]
        public void TestMapToEntityModifiesIdentifierIfFacilityChanges()
        {
            var facility1 = GetEntityFactory<Facility>().Create();
            var facility2 = GetEntityFactory<Facility>().Create();

            _facilityRepository.Setup(x => x.Find(facility1.Id)).Returns(facility1);
            _facilityRepository.Setup(x => x.Find(facility2.Id)).Returns(facility2);

            _entity = GetEntityFactory<Equipment>().Create(new {
                Facility = facility1
            });

            var originalId = _entity.Identifier;

            _target = new EditEquipment(_container);
            _target.Map(_entity);

            _target.Facility = facility2.Id;

            _entity = _target.MapToEntity(_entity);

            Assert.AreNotEqual(originalId, _entity.Identifier);
        }

        [TestMethod]
        public void TestEquipmentPurposeDoesMapToEntity()
        {
            var department = GetEntityFactory<Department>().Create();
            var facility = GetEntityFactory<Facility>().Create(new { Department = department });
            _facilityRepository.Setup(x => x.Find(It.IsAny<int>())).Returns(facility);
            var eq1 = GetEntityFactory<Equipment>().Create(new { SAPEquipmentId = 123 });
            _repository.Setup(x => x.Any(It.IsAny<Expression<Func<Equipment, bool>>>())).Returns(false);
            var equipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create();
            _target.EquipmentPurpose = equipmentPurpose.Id;
            _entity = _target.MapToEntity(_entity);

            Assert.AreEqual(equipmentPurpose.Id, _entity.EquipmentPurpose.Id);
        }
    }
}
