using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using StructureMap;
using IWorkOrderRepository = Contractors.Data.Models.Repositories.IWorkOrderRepository;
using WorkOrderRepository = Contractors.Data.Models.Repositories.WorkOrderRepository;

namespace Contractors.Tests.Models.ViewModels
{
    [TestClass]
    public class WorkOrderFinalizationDetailsTest : MapCallMvcInMemoryDatabaseTestBase<WorkOrder>
    {
        #region Members

        private ContractorUser _currentUser;
        private WorkOrder _order;
        private WorkOrderFinalizationDetails _target;
        private Mock<IAuthenticationService<ContractorUser>>
            _authenticationService;

        #endregion

        #region Setup/Teardown

        protected override void InitializeObjectFactory(
            ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IWorkOrderRepository>().Use<WorkOrderRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            _authenticationService =
                e.For<IAuthenticationService<ContractorUser>>().Mock();
            e.For<ITapImageRepository>().Mock();
            e.For<IServiceRepository>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _currentUser = GetFactory<ContractorUserFactory>().Create();
            _authenticationService
               .Setup(x => x.CurrentUser).Returns(_currentUser);
            _order = GetFactory<FinalizationWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor
            });
            _target = _container.GetInstance<WorkOrderFinalizationDetails>();
            _target.Map(_order);
        }

        #endregion

        [TestMethod]
        public void TestValidateWontAllowMainBreakOrdersWithNoMainBreakInfo()
        {
            // repair
            _order = GetFactory<FinalizationWorkOrderFactory>().Create(new {
                AssetType = typeof(MainAssetTypeFactory),
                WorkDescription = typeof(MainBreakRepairWorkDescriptionFactory),
                AssignedContractor = _currentUser.Contractor
            });
            _target = _container.GetInstance<WorkOrderFinalizationDetails>();
            _target.Map(_order);

            var results = _target
                .Validate(new ValidationContext(_target, null, null))
                .ToArray();

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(WorkOrderFinalizationDetails.NO_MAIN_BREAK_INFO,
                results[0].ErrorMessage);

            // this should make it valid
            GetFactory<MainBreakFactory>().Create(new {WorkOrder = _order});
            Session.Clear();

            results = _target
                .Validate(new ValidationContext(_target, null, null))
                .ToArray();

            Assert.AreEqual(0, results.Count());

            // replace
            _order = GetFactory<FinalizationWorkOrderFactory>().Create(new {
                AssetType = typeof(MainAssetTypeFactory),
                WorkDescription = typeof(WaterMainBreakReplaceWorkDescriptionFactory),
                AssignedContractor = _currentUser.Contractor
            });
            _target = _container.GetInstance<WorkOrderFinalizationDetails>();
            _target.Map(_order);

            results = _target
                .Validate(new ValidationContext(_target, null, null))
                .ToArray();

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(WorkOrderFinalizationDetails.NO_MAIN_BREAK_INFO,
                results[0].ErrorMessage);

            // this should make it valid
            GetFactory<MainBreakFactory>().Create(new {WorkOrder = _order});
            Session.Clear();

            results = _target
                .Validate(new ValidationContext(_target, null, null))
                .ToArray();

            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestValidateWontAllowOrdersWithOpenCrewAssignments()
        {
            var assignment = GetFactory<OpenCrewAssignmentFactory>()
                .Create(new { WorkOrder = _order });
            _target.DateCompleted = DateTime.Now;
            Session.Clear();
            _order = Session.Get<WorkOrder>(_order.Id);

            var results = _target
                .Validate(new ValidationContext(_target, null, null))
                .ToArray();

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(WorkOrderFinalizationDetails.OPEN_CREW_ASSIGNMENTS,
                results[0].ErrorMessage);

            _order = GetFactory<FinalizationWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor
            });
            _target = _container.GetInstance<WorkOrderFinalizationDetails>();
            _target.Map(_order);
            _target.DateCompleted = DateTime.Now;

            results = _target
                .Validate(new ValidationContext(_target, null, null))
                .ToArray();

            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void TestValidateWontAllowUnknownMeterLocation()
        {
            var order = GetFactory<FinalizationWorkOrderFactory>().Create(new {
                AssetType = typeof(ServiceAssetTypeFactory), 
                AssignedContractor = _currentUser.Contractor
            });
            _target.Map(order);
            _target.DateCompleted = DateTime.Now;
            _target.MeterLocation = MeterLocation.Indices.UNKNOWN;
            Session.Clear();

            var results = _target
               .Validate(new ValidationContext(_target, null, null))
               .ToArray();

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(WorkOrderFinalizationDetails.ERROR_METER_LOCATION_REQUIRED, results[0].ErrorMessage);
         }

        [TestMethod]
        public void TestMapToEntitySetServiceFields()
        {
            var service = GetFactory<ServiceFactory>().Create();
            var previousServiceMaterial = GetFactory<ServiceMaterialFactory>().Create();
            var customerServiceMaterial = GetFactory<ServiceMaterialFactory>().Create();
            var companyServiceMaterial = GetFactory<ServiceMaterialFactory>().Create();
            var previousServiceSize = GetFactory<ServiceSizeFactory>().Create();
            var customerServiceSize = GetFactory<ServiceSizeFactory>().Create();
            var companyServiceSize = GetFactory<ServiceSizeFactory>().Create();

            _order.Service = service;
            _target.PreviousServiceLineMaterial = previousServiceMaterial.Id;
            _target.CompanyServiceLineMaterial = companyServiceMaterial.Id;
            _target.CustomerServiceLineMaterial = customerServiceMaterial.Id;
            _target.PreviousServiceLineSize = previousServiceSize.Id;
            _target.CompanyServiceLineSize = companyServiceSize.Id;
            _target.CustomerServiceLineSize = customerServiceSize.Id;

            _target.MapToEntity(_order);
            
            Session.Clear();
            _order = Session.Get<WorkOrder>(_order.Id);
            var serviceEntity = Session.Get<Service>(service.Id);

            Assert.AreEqual(previousServiceMaterial.Id, serviceEntity.PreviousServiceMaterial?.Id);
            Assert.AreEqual(customerServiceMaterial.Id, serviceEntity.CustomerSideMaterial?.Id);
            Assert.AreEqual(companyServiceMaterial.Id, serviceEntity.ServiceMaterial?.Id);
            Assert.AreEqual(previousServiceSize.Id, serviceEntity.PreviousServiceSize?.Id);
            Assert.AreEqual(customerServiceSize.Id, serviceEntity.CustomerSideSize?.Id);
            Assert.AreEqual(companyServiceSize.Id, serviceEntity.ServiceSize?.Id);
        }
    }
}
