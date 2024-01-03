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
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class CreateServiceRestorationTest : MapCallMvcInMemoryDatabaseTestBase<ServiceRestoration>
    {
        #region Fields

        private ViewModelTester<CreateServiceRestoration, ServiceRestoration> _vmTester;
        private CreateServiceRestoration _viewModel;
        private ServiceRestoration _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);

            _viewModel = new CreateServiceRestoration(_container);
            _entity = new ServiceRestoration();
            _vmTester = new ViewModelTester<CreateServiceRestoration, ServiceRestoration>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.EstimatedRestorationAmount);
            _vmTester.CanMapBothWays(x => x.EstimatedValue);
            _vmTester.CanMapBothWays(x => x.Cancel);
            _vmTester.CanMapBothWays(x => x.PartialRestorationInvoiceNumber);
            _vmTester.CanMapBothWays(x => x.PartialRestorationAmount);
            _vmTester.CanMapBothWays(x => x.PartialRestorationDate);
            _vmTester.CanMapBothWays(x => x.PartialRestorationCost);
            _vmTester.CanMapBothWays(x => x.PartialRestorationTrafficControlHours);
            _vmTester.CanMapBothWays(x => x.FinalRestorationInvoiceNumber);
            _vmTester.CanMapBothWays(x => x.FinalRestorationAmount);
            _vmTester.CanMapBothWays(x => x.FinalRestorationDate);
            _vmTester.CanMapBothWays(x => x.FinalRestorationCost);
            _vmTester.CanMapBothWays(x => x.FinalRestorationTrafficControlHours);
            _vmTester.CanMapBothWays(x => x.PurchaseOrderNumber);
        }

        [TestMethod]
        public void TestRestorationTypeCanMapBothWays()
        {
            var restorationType = GetEntityFactory<RestorationType>().Create(new {Description = "Foo"});
            _entity.RestorationType = restorationType;

            _vmTester.MapToViewModel();

            Assert.AreEqual(restorationType.Id, _viewModel.RestorationType);

            _entity.RestorationType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(restorationType, _entity.RestorationType);
        }

        [TestMethod]
        public void TestPartialRestorationMethodCanMapBothWays()
        {
            var restorationMethod = GetEntityFactory<RestorationMethod>().Create(new {Description = "Foo"});
            _entity.PartialRestorationMethod = restorationMethod;

            _vmTester.MapToViewModel();

            Assert.AreEqual(restorationMethod.Id, _viewModel.PartialRestorationMethod);

            _entity.PartialRestorationMethod = null;
            _vmTester.MapToEntity();

            Assert.AreSame(restorationMethod, _entity.PartialRestorationMethod);
        }

        [TestMethod]
        public void TestPartialRestorationCompletionByCanMapBothWays()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var serviceRestorationContractor = GetEntityFactory<ServiceRestorationContractor>().Create(new {Contractor = "Foo", OperatingCenter = operatingCenter});
            _entity.PartialRestorationCompletionBy = serviceRestorationContractor;

            _vmTester.MapToViewModel();

            Assert.AreEqual(serviceRestorationContractor.Id, _viewModel.PartialRestorationCompletionBy);

            _entity.PartialRestorationCompletionBy = null;
            _vmTester.MapToEntity();

            Assert.AreSame(serviceRestorationContractor, _entity.PartialRestorationCompletionBy);
        }

        [TestMethod]
        public void TestFinalRestorationMethodCanMapBothWays()
        {
            var restorationMethod = GetEntityFactory<RestorationMethod>().Create(new { Description = "Foo" });
            _entity.FinalRestorationMethod = restorationMethod;

            _vmTester.MapToViewModel();

            Assert.AreEqual(restorationMethod.Id, _viewModel.FinalRestorationMethod);

            _entity.FinalRestorationMethod = null;
            _vmTester.MapToEntity();

            Assert.AreSame(restorationMethod, _entity.FinalRestorationMethod);
        }

        [TestMethod]
        public void TestFinalRestorationCompletionByCanMapBothWays()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var serviceRestorationContractor = GetEntityFactory<ServiceRestorationContractor>().Create(new { Contractor = "Foo", OperatingCenter = operatingCenter });
            _entity.FinalRestorationCompletionBy = serviceRestorationContractor;

            _vmTester.MapToViewModel();

            Assert.AreEqual(serviceRestorationContractor.Id, _viewModel.FinalRestorationCompletionBy);

            _entity.FinalRestorationCompletionBy = null;
            _vmTester.MapToEntity();

            Assert.AreSame(serviceRestorationContractor, _entity.FinalRestorationCompletionBy);
        }

        [TestMethod]
        public void TestMapToEntitySetsInitiatedByToCurrentUser()
        {
            _entity.InitiatedBy = null;

            _vmTester.MapToEntity();

            Assert.AreSame(_user, _entity.InitiatedBy);
        }

        #endregion
    }

    [TestClass]
    public class EditServiceRestorationTest : MapCallMvcInMemoryDatabaseTestBase<ServiceRestoration>
    {
        #region Fields

        private ViewModelTester<EditServiceRestoration, ServiceRestoration> _vmTester;
        private EditServiceRestoration _viewModel;
        private ServiceRestoration _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditServiceRestoration(_container);
            _entity = new ServiceRestoration();
            _vmTester = new ViewModelTester<EditServiceRestoration, ServiceRestoration>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.EstimatedRestorationAmount);
            _vmTester.CanMapBothWays(x => x.EstimatedValue);
            _vmTester.CanMapBothWays(x => x.Cancel);
            _vmTester.CanMapBothWays(x => x.PartialRestorationInvoiceNumber);
            _vmTester.CanMapBothWays(x => x.PartialRestorationAmount);
            _vmTester.CanMapBothWays(x => x.PartialRestorationDate);
            _vmTester.CanMapBothWays(x => x.PartialRestorationCost);
            _vmTester.CanMapBothWays(x => x.PartialRestorationTrafficControlHours);
            _vmTester.CanMapBothWays(x => x.FinalRestorationInvoiceNumber);
            _vmTester.CanMapBothWays(x => x.FinalRestorationAmount);
            _vmTester.CanMapBothWays(x => x.FinalRestorationDate);
            _vmTester.CanMapBothWays(x => x.FinalRestorationCost);
            _vmTester.CanMapBothWays(x => x.FinalRestorationTrafficControlHours);
            _vmTester.CanMapBothWays(x => x.PurchaseOrderNumber);
        }

        [TestMethod]
        public void TestRestorationTypeCanMapBothWays()
        {
            var restorationType = GetEntityFactory<RestorationType>().Create(new { Description = "Foo" });
            _entity.RestorationType = restorationType;

            _vmTester.MapToViewModel();

            Assert.AreEqual(restorationType.Id, _viewModel.RestorationType);

            _entity.RestorationType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(restorationType, _entity.RestorationType);
        }

        [TestMethod]
        public void TestPartialRestorationMethodCanMapBothWays()
        {
            var restorationMethod = GetEntityFactory<RestorationMethod>().Create(new { Description = "Foo" });
            _entity.PartialRestorationMethod = restorationMethod;

            _vmTester.MapToViewModel();

            Assert.AreEqual(restorationMethod.Id, _viewModel.PartialRestorationMethod);

            _entity.PartialRestorationMethod = null;
            _vmTester.MapToEntity();

            Assert.AreSame(restorationMethod, _entity.PartialRestorationMethod);
        }

        [TestMethod]
        public void TestPartialRestorationCompletionByCanMapBothWays()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var serviceRestorationContractor = GetEntityFactory<ServiceRestorationContractor>().Create(new { Contractor = "Foo", OperatingCenter = operatingCenter });
            _entity.PartialRestorationCompletionBy = serviceRestorationContractor;

            _vmTester.MapToViewModel();

            Assert.AreEqual(serviceRestorationContractor.Id, _viewModel.PartialRestorationCompletionBy);

            _entity.PartialRestorationCompletionBy = null;
            _vmTester.MapToEntity();

            Assert.AreSame(serviceRestorationContractor, _entity.PartialRestorationCompletionBy);
        }

        [TestMethod]
        public void TestFinalRestorationMethodCanMapBothWays()
        {
            var restorationMethod = GetEntityFactory<RestorationMethod>().Create(new { Description = "Foo" });
            _entity.FinalRestorationMethod = restorationMethod;

            _vmTester.MapToViewModel();

            Assert.AreEqual(restorationMethod.Id, _viewModel.FinalRestorationMethod);

            _entity.FinalRestorationMethod = null;
            _vmTester.MapToEntity();

            Assert.AreSame(restorationMethod, _entity.FinalRestorationMethod);
        }

        [TestMethod]
        public void TestFinalRestorationCompletionByCanMapBothWays()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var serviceRestorationContractor = GetEntityFactory<ServiceRestorationContractor>().Create(new { Contractor = "Foo", OperatingCenter = operatingCenter });
            _entity.FinalRestorationCompletionBy = serviceRestorationContractor;

            _vmTester.MapToViewModel();

            Assert.AreEqual(serviceRestorationContractor.Id, _viewModel.FinalRestorationCompletionBy);

            _entity.FinalRestorationCompletionBy = null;
            _vmTester.MapToEntity();

            Assert.AreSame(serviceRestorationContractor, _entity.FinalRestorationCompletionBy);
        }

        #endregion
    }
}
