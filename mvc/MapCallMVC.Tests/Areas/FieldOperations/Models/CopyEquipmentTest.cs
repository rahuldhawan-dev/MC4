using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class CopyEquipmentTest : MapCallMvcInMemoryDatabaseTestBase<Equipment>
    {
        #region Private Members

        private ViewModelTester<CopyEquipment, Equipment> _vmTester;
        private CopyEquipment _viewModel;
        private Equipment _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IEquipmentRepository>().Use<EquipmentRepository>();
            e.For<IDateTimeProvider>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            _user.IsAdmin = true;

            _entity = GetEntityFactory<Equipment>().Create();
            _viewModel = _viewModelFactory.Build<CopyEquipment, Equipment>(_entity);
            _vmTester = new ViewModelTester<CopyEquipment, Equipment>(_viewModel, _entity);
        }

        #endregion

        [TestMethod]
        public void TestPropertiesAreExcludedInCopy()
        {
            var entity = GetEntityFactory<Equipment>().Create(new {
                SAPEquipmentId = 1234,
                ReplacedEquipment = GetEntityFactory<Equipment>().Create(),
                EquipmentStatus = GetEntityFactory<EquipmentStatus>().Create(),
                RequestedBy = GetEntityFactory<Employee>().Create(),
                AssetControlSignOffBy = GetEntityFactory<Employee>().Create(),
                AssetControlSignOffDate = DateTime.Now,
                DateRetired = DateTime.Now,
                ABCIndicator = GetEntityFactory<ABCIndicator>().Create(),
                DateInstalled = DateTime.Now,
                SAPEquipmentIdBeingReplaced = 12345,
                ScadaTagName = GetEntityFactory<ScadaTagName>().Create(),
                ArcFlashHierarchy = 1.2m,
                ArcFlashRating = "1",
                IsReplacement = true
            });
            _viewModel = _viewModelFactory.Build<CopyEquipment, Equipment>(entity);

            _viewModel.Map(entity);

            Assert.IsNull(_viewModel.SAPEquipmentId);
            Assert.IsNull(_viewModel.ReplacedEquipment);
            Assert.IsNull(_viewModel.RequestedBy);
            Assert.IsNull(_viewModel.AssetControlSignOffBy);
            Assert.IsNull(_viewModel.AssetControlSignOffDate);
            Assert.IsNull(_viewModel.DateRetired);
            Assert.IsNull(_viewModel.DateInstalled);
            Assert.IsNull(_viewModel.SAPEquipmentIdBeingReplaced);
            Assert.IsNull(_viewModel.ScadaTagName);
            Assert.IsNull(_viewModel.ArcFlashHierarchy);
            Assert.IsNull(_viewModel.ArcFlashRating);
            Assert.IsNull(_viewModel.IsReplacement);
        }

        [TestMethod]
        public void TestEquipmentStatusSetToPendingOnCopy()
        {
            var cancelledEquipmentStatus = GetFactory<CancelledEquipmentStatusFactory>().Create();
            var pendingEquipmentStatus = GetFactory<PendingEquipmentStatusFactory>().Create();

            var entity = GetEntityFactory<Equipment>().Create(new
            {
                SAPEquipmentId = 1234,
                ReplacedEquipment = GetEntityFactory<Equipment>().Create(),
                EquipmentStatus = cancelledEquipmentStatus,
                RequestedBy = GetEntityFactory<Employee>().Create(),
                AssetControlSignOffBy = GetEntityFactory<Employee>().Create(),
                AssetControlSignOffDate = DateTime.Now,
                DateRetired = DateTime.Now,
                ABCIndicator = GetEntityFactory<ABCIndicator>().Create(),
                DateInstalled = DateTime.Now,
                SAPEquipmentIdBeingReplaced = 12345,
                ScadaTagName = GetEntityFactory<ScadaTagName>().Create(),
                ArcFlashHierarchy = 1.2m,
                ArcFlashRating = "1",
                IsReplacement = true
            });
            _viewModel = _viewModelFactory.Build<CopyEquipment, Equipment>(entity);

            _viewModel.MapToEntity(entity);

            Assert.AreEqual(pendingEquipmentStatus, entity.EquipmentStatus);
        }
    }
}