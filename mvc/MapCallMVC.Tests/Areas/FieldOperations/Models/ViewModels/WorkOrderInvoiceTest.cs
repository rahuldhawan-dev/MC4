using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using Permits.Data.Client.Entities;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Testing.Utilities;
using StructureMap;
using Permits.Data.Client.Repositories;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class CreateWorkOrderInvoiceTest : MapCallMvcInMemoryDatabaseTestBase<WorkOrderInvoice>
    {
        #region Fields

        private ViewModelTester<CreateWorkOrderInvoice, WorkOrderInvoice> _vmTester;
        private CreateWorkOrderInvoice _viewModel;
        private WorkOrderInvoice _entity;
        private Mock<IAuthenticationService<User>> _authenticationService;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
            _authenticationService = new Mock<IAuthenticationService<User>>();
            e.For<IAuthenticationService<User>>().Use(_authenticationService.Object);
            e.For<IWorkOrderRepository>().Use<WorkOrderRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateWorkOrderInvoice(_container);
            _entity = new WorkOrderInvoice();
            _vmTester = new ViewModelTester<CreateWorkOrderInvoice, WorkOrderInvoice>(_viewModel, _entity);

            _user = GetFactory<AdminUserFactory>().Create();
            _authenticationService.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.InvoiceDate);
            _vmTester.CanMapBothWays(x => x.IncludeMaterials);
        }

        [TestMethod]
        public void TestWorkOrderCanMapBothWays()
        {
            var wo = GetEntityFactory<WorkOrder>().Create();
            _entity.WorkOrder = wo;

            _vmTester.MapToViewModel();

            Assert.AreEqual(wo.Id, _viewModel.WorkOrder);

            _entity.WorkOrder = null;
            _vmTester.MapToEntity();

            Assert.AreSame(wo, _entity.WorkOrder);
        }

        [TestMethod]
        public void TestScheduleOfValueTypeCanMapBothWays()
        {
            var sovt = GetEntityFactory<ScheduleOfValueType>().Create(new {Description = "Foo"});
            _entity.ScheduleOfValueType = sovt;

            _vmTester.MapToViewModel();

            Assert.AreEqual(sovt.Id, _viewModel.ScheduleOfValueType);

            _entity.ScheduleOfValueType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(sovt, _entity.ScheduleOfValueType);
        }

        [TestMethod]
        public void TestMapToEntityCopiesWorkOrderScheduleOfValuesToWorkOrderInvoicesScheduleOfValues()
        {
            var wo = GetEntityFactory<WorkOrder>().Create();
            var sovt1 = GetEntityFactory<ScheduleOfValueType>().Create(new { Description = "Time & Materials"});
            var sovt2 = GetEntityFactory<ScheduleOfValueType>().Create(new { Description = "Unit Cost" });
            var sovc1 = GetEntityFactory<ScheduleOfValueCategory>().Create(new { ScheduleOfValueType = sovt1 });
            var sovc2 = GetEntityFactory<ScheduleOfValueCategory>().Create(new { ScheduleOfValueType = sovt2 });
            var sov = GetFactory<ScheduleOfValueFactory>().Create(new { Description = "Foo", ScheduleOfValueCategory = sovc1, LaborUnitOvertimeCost = 10m, LaborUnitCost = 5m });
            var sov2 = GetFactory<ScheduleOfValueFactory>().Create(new { Description = "FooBah", ScheduleOfValueCategory = sovc2 });
            var sov3 = GetFactory<ScheduleOfValueFactory>().Create(new { Description = "FooBaBaz", ScheduleOfValueCategory = sovc1, LaborUnitOvertimeCost = 100m, LaborUnitCost = 75m });

            var include = GetEntityFactory<WorkOrderScheduleOfValue>().Create(new {
                WorkOrder = wo,
                ScheduleOfValue = sov, 
                LaborUnitCost = sov.LaborUnitCost
            });
            var alsoInclude = GetEntityFactory<WorkOrderScheduleOfValue>().Create(new
            {
                WorkOrder = wo,
                ScheduleOfValue = sov3,
                IsOvertime = true
            });
            var doNotInclude = GetEntityFactory<WorkOrderScheduleOfValue>().Create(new
            {
                WorkOrder = wo,
                ScheduleOfValue = sov2
            });

            _viewModel.ScheduleOfValueType = sovt1.Id;
            _viewModel.WorkOrder = wo.Id;
            Session.Clear();
            Session.Flush();

            _vmTester.MapToEntity();

            Assert.AreEqual(2, _entity.WorkOrderInvoicesScheduleOfValues.Count);
            Assert.AreEqual(sov.Id, _entity.WorkOrderInvoicesScheduleOfValues.First().ScheduleOfValue.Id);
            Assert.AreEqual(sov.LaborUnitCost, _entity.WorkOrderInvoicesScheduleOfValues.First().LaborUnitCost);
            Assert.AreEqual(sov3.LaborUnitOvertimeCost, _entity.WorkOrderInvoicesScheduleOfValues.Last().LaborUnitCost);
        }

        [TestMethod]
        public void TestMiscAndMaterialCostArePopulated()
        {
            var wo = GetEntityFactory<WorkOrder>().Create();
            var sovt1 = GetEntityFactory<ScheduleOfValueType>().Create(new { Description = "argh" });
            var sovt2 = GetEntityFactory<ScheduleOfValueType>().Create(new { Description = "ugh" });
            var sovc1 = GetEntityFactory<ScheduleOfValueCategory>().Create(new { ScheduleOfValueType = sovt1 });
            var sovc2 = GetEntityFactory<ScheduleOfValueCategory>().Create(new { ScheduleOfValueType = sovt2 });
            var sov = GetFactory<ScheduleOfValueFactory>().Create(new { Description = "Foo", ScheduleOfValueCategory = sovc1, MaterialCost = 5m, MiscCost = 10m });
            var sov2 = GetFactory<ScheduleOfValueFactory>().Create(new { Description = "FooBah", ScheduleOfValueCategory = sovc2, MaterialCost = 10m, MiscCost = 20m });

            var include = GetEntityFactory<WorkOrderScheduleOfValue>().Create(new
            {
                WorkOrder = wo,
                ScheduleOfValue = sov
            });

            _viewModel.ScheduleOfValueType = sovt1.Id;
            _viewModel.WorkOrder = wo.Id;
            Session.Clear();
            Session.Flush();

            _vmTester.MapToEntity();

            Assert.AreEqual(1, _entity.WorkOrderInvoicesScheduleOfValues.Count);
            Assert.AreEqual(sov.Id, ((WorkOrderInvoiceScheduleOfValue)_entity.WorkOrderInvoicesScheduleOfValues.First()).ScheduleOfValue.Id);
            Assert.AreEqual(sov.MaterialCost, _entity.ScheduleOfValues.First().MaterialCost);
            Assert.AreEqual(sov.MiscCost, _entity.ScheduleOfValues.Last().MiscCost);
        }

        #endregion
    }

    [TestClass]
    public class EditWorkOrderInvoiceTest : MapCallMvcInMemoryDatabaseTestBase<WorkOrderInvoice>
    {
        #region Fields

        private ViewModelTester<EditWorkOrderInvoice, WorkOrderInvoice> _vmTester;
        private EditWorkOrderInvoice _viewModel;
        private WorkOrderInvoice _entity;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new EditWorkOrderInvoice(_container);
            _entity = new WorkOrderInvoice();
            _vmTester = new ViewModelTester<EditWorkOrderInvoice, WorkOrderInvoice>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.InvoiceDate);
            _vmTester.CanMapBothWays(x => x.IncludeMaterials);
        }

        //[TestMethod]
        //public void TestRequiredFields()
        //{
        //    // ValidationAssert.PropertyIsRequired(_viewModel, x => x.);
        //    Assert.Fail("Fill out this test.");
        //}

        [TestMethod]
        public void TestWorkOrderCanMapBothWays()
        {
            var wo = GetEntityFactory<WorkOrder>().Create();
            _entity.WorkOrder = wo;

            _vmTester.MapToViewModel();

            Assert.AreEqual(wo.Id, _viewModel.WorkOrder);

            _entity.WorkOrder = null;
            _vmTester.MapToEntity();

            Assert.AreSame(wo, _entity.WorkOrder);
        }

        [TestMethod]
        public void TestSettingSubmittedDateSetsSendSubmittedNotificationOnSaveProperly()
        {
            var invoice = GetEntityFactory<WorkOrderInvoice>().Create();
            var model = _viewModelFactory.BuildWithOverrides<EditWorkOrderInvoice, WorkOrderInvoice>(invoice, new { SubmittedDate = DateTime.Now});
            _vmTester = new ViewModelTester<EditWorkOrderInvoice, WorkOrderInvoice>(model, invoice);

            _vmTester.MapToEntity();

            Assert.IsTrue(model.SendSubmittedNotificationOnSave);

            invoice = GetEntityFactory<WorkOrderInvoice>().Create(new { SubmittedDate = DateTime.Now });
            var tomorrow = DateTime.Now.AddDays(1);
            model = _viewModelFactory.BuildWithOverrides<EditWorkOrderInvoice, WorkOrderInvoice>(invoice, new { SubmittedDate = tomorrow });
            _vmTester = new ViewModelTester<EditWorkOrderInvoice, WorkOrderInvoice>(model, invoice);

            _vmTester.MapToEntity();

            Assert.IsTrue(model.SendSubmittedNotificationOnSave);

            invoice = GetEntityFactory<WorkOrderInvoice>().Create(new { SubmittedDate = tomorrow });
            model = _viewModelFactory.BuildWithOverrides<EditWorkOrderInvoice, WorkOrderInvoice>(invoice, new { SubmittedDate = tomorrow });
            _vmTester = new ViewModelTester<EditWorkOrderInvoice, WorkOrderInvoice>(model, invoice);

            _vmTester.MapToEntity();

            Assert.IsFalse(model.SendSubmittedNotificationOnSave);

            invoice = GetEntityFactory<WorkOrderInvoice>().Create();
            model = _viewModelFactory.Build<EditWorkOrderInvoice, WorkOrderInvoice>( invoice);
            _vmTester = new ViewModelTester<EditWorkOrderInvoice, WorkOrderInvoice>(model, invoice);

            _vmTester.MapToEntity();

            Assert.IsFalse(model.SendSubmittedNotificationOnSave);
        }

        [TestMethod]
        public void TestSettingCanceledDateSetsSendCanceledNotificationOnSaveProperly()
        {
            var invoice = GetEntityFactory<WorkOrderInvoice>().Create();
            var model = _viewModelFactory.BuildWithOverrides<EditWorkOrderInvoice, WorkOrderInvoice>(invoice, new { CanceledDate = DateTime.Now });
            _vmTester = new ViewModelTester<EditWorkOrderInvoice, WorkOrderInvoice>(model, invoice);

            _vmTester.MapToEntity();

            Assert.IsTrue(model.SendCanceledNotificationOnSave);

            invoice = GetEntityFactory<WorkOrderInvoice>().Create(new { CanceledDate = DateTime.Now });
            var tomorrow = DateTime.Now.AddDays(1);
            model = _viewModelFactory.BuildWithOverrides<EditWorkOrderInvoice, WorkOrderInvoice>(invoice, new { CanceledDate = tomorrow });
            _vmTester = new ViewModelTester<EditWorkOrderInvoice, WorkOrderInvoice>(model, invoice);

            _vmTester.MapToEntity();

            Assert.IsTrue(model.SendCanceledNotificationOnSave);

            invoice = GetEntityFactory<WorkOrderInvoice>().Create(new { CanceledDate = tomorrow });
            model = _viewModelFactory.BuildWithOverrides<EditWorkOrderInvoice, WorkOrderInvoice>(invoice, new { CanceledDate = tomorrow });
            _vmTester = new ViewModelTester<EditWorkOrderInvoice, WorkOrderInvoice>(model, invoice);

            _vmTester.MapToEntity();

            Assert.IsFalse(model.SendCanceledNotificationOnSave);

            invoice = GetEntityFactory<WorkOrderInvoice>().Create();
            model = _viewModelFactory.Build<EditWorkOrderInvoice, WorkOrderInvoice>( invoice);
            _vmTester = new ViewModelTester<EditWorkOrderInvoice, WorkOrderInvoice>(model, invoice);

            _vmTester.MapToEntity();

            Assert.IsFalse(model.SendCanceledNotificationOnSave);
        }

        #endregion
    }

    [TestClass]
    public class WorkOrderInvoicePdfTest : MapCallMvcInMemoryDatabaseTestBase<WorkOrderInvoice>
    {
        #region Fields

        private Mock<MMSINC.Data.WebApi.IRepository<Permit>> _permitRepository;
        
        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _permitRepository = new Mock<MMSINC.Data.WebApi.IRepository<Permit>>();
            _container.Inject(_permitRepository.Object);
            _container.Inject(new Mock<IPermitsRepositoryFactory>().Object);
        }
        
        #endregion

        [TestMethod]
        public void TestPermitsReturnsPermits()
        {
            var workOrder = GetEntityFactory<WorkOrder>().Create();
            var workOrderInvoice = GetEntityFactory<WorkOrderInvoice>().Create(new {WorkOrder = workOrder});
            var target = _viewModelFactory.Build<WorkOrderInvoicePdf, WorkOrderInvoice>(workOrderInvoice);
            target.PermitRepository = _permitRepository.Object;
            var permit = new Permit();
            var permits = new[] {permit} as IEnumerable<Permit>;
            NameValueCollection foo = new NameValueCollection { { "ArbitraryIdentifier", workOrder.Id.ToString() } };

            _permitRepository.Setup(x => x.Search(foo)).Returns(permits);

            Assert.AreEqual(1, target.Permits.Count);
        }
    }
}
