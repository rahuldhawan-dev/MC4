using System;
using System.Linq;
using System.Web.Mvc;
using StructureMap;
using _271ObjectTests.Tests.Unit.Model;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Repositories;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Utilities;
using MMSINC.Utilities.StructureMap;
using Moq;
using Rhino.Mocks;
using Rhino.Mocks.Impl;
using Rhino.Mocks.Interfaces;
using Subtext.TestLibrary;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using WorkOrders.Presenters.WorkOrders;
using WorkOrders.Views.WorkOrders;
using AssetTypeRepository = WorkOrders.Model.AssetTypeRepository;
using Contact = WorkOrders.Model.Contact;
using ContactType = WorkOrders.Model.ContactType;
using IEmployeeRepository = WorkOrders.Model.IEmployeeRepository;
using Town = WorkOrders.Model.Town;
using TownContact = WorkOrders.Model.TownContact;
using WorkDescriptionRepository = WorkOrders.Model.WorkDescriptionRepository;
using WorkOrder = WorkOrders.Model.WorkOrder;
using WorkDescription = WorkOrders.Model.WorkDescription;
using OperatingCenter = WorkOrders.Model.OperatingCenter;
using Premise = WorkOrders.Model.Premise;
using SampleSite = WorkOrders.Model.SampleSite;
using Service = MapCall.Common.Model.Entities.Service;
using WorkOrderRequester = MapCall.Common.Model.Entities.WorkOrderRequester;

namespace _271ObjectTests.Tests.Unit.Presenters.WorkOrders
{
    /// <summary>
    /// Summary description for WorkOrderInputDetailPresenterTest
    /// </summary>
    [TestClass]
    public class WorkOrderDetailPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IRepository<WorkOrder> _repository;
        private IRepository<WorkDescription> _workDescriptionRepository;
        private IEmployeeRepository _employeeRepository;
        private ISampleSiteRepository _sampleSiteRepository;
        private IWorkOrderDetailView _view;
        private TestWorkOrderDetailPresenter _target;
        private ISecurityService _securityService;
        private IDateTimeProvider _dateTimeProvider;
        private Mock<IGeneralWorkOrderRepository> _iGeneralWorkOrderRepositoryMock;
        private Mock<ISampleSiteRepository> _ISampleSiteRepositoryMock;
        private Mock<ISAPWorkOrderRepository> _iSapWorkOrderRepository;
        private Mock<IWorkOrdersWorkOrderRepository> _iWorkOrdersWorkOrderRepository;
        private Mock<INotificationService> _notificationService;
        private Mock<IServiceRepository> _serviceRepository;
        private MapCall.Common.Model.Entities.WorkOrder _lastCreatedGeneralWorkOrder;
        private MapCall.Common.Model.Entities.SampleSite _lastCreatedGeneralSampleSite;
        private IContainer _container;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _container = new Container();
            _mocks
               .DynamicMock(out _view)
               .DynamicMock(out _repository)
               .DynamicMock(out _workDescriptionRepository)
               .DynamicMock(out _employeeRepository)
               .DynamicMock(out _securityService)
               .DynamicMock(out _dateTimeProvider)
               .DynamicMock(out _sampleSiteRepository);

            _target = new TestWorkOrderDetailPresenterBuilder(_view)
               .WithRepository(_repository);

            ReflectionHelper.SetStaticFieldValue("_instance",
                typeof(SecurityService), _securityService);
            _container.Inject(_workDescriptionRepository);
            _container.Inject(_sampleSiteRepository);
            _container.Inject(_employeeRepository);
            _container.Inject(_dateTimeProvider);

            _iGeneralWorkOrderRepositoryMock = new Mock<IGeneralWorkOrderRepository>();
            _container.Inject(_iGeneralWorkOrderRepositoryMock.Object);
            _ISampleSiteRepositoryMock = new Mock<ISampleSiteRepository>();
            _container.Inject(_ISampleSiteRepositoryMock.Object);
            _iSapWorkOrderRepository = new Mock<ISAPWorkOrderRepository>();
            _container.Inject(_iSapWorkOrderRepository.Object);
            _iWorkOrdersWorkOrderRepository = new Mock<IWorkOrdersWorkOrderRepository>();
            _container.Inject(_iWorkOrdersWorkOrderRepository.Object);
            _notificationService = new Mock<INotificationService>();
            _container.Inject(_notificationService.Object);
            _serviceRepository = new Mock<IServiceRepository>();
            _container.Inject(_serviceRepository.Object);

            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            ReflectionHelper.SetStaticFieldValue("_instance",
                typeof(SecurityService), (ISecurityService)null);

            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Private Methods

        private void SetupGeneralWorkOrderFrom271WorkOrder(WorkOrder workOrder, bool enableSap = false, Service service=null)
        {
            if (enableSap)
            {
                workOrder.SAPWorkOrderNumber = 123;
                workOrder.WorkOrderID = 1;
                workOrder.OperatingCenter.SAPEnabled = true;
                workOrder.OperatingCenter.SAPWorkOrdersEnabled = true;
            }

            _lastCreatedGeneralWorkOrder = new MapCall.Common.Model.Entities.WorkOrder {
                SAPWorkOrderNumber = workOrder.SAPWorkOrderNumber
            };
            _lastCreatedGeneralSampleSite = new MapCall.Common.Model.Entities.SampleSite {
                Id = 5, 
                Premise = new MapCall.Common.Model.Entities.Premise {
                    PremiseNumber = "1234"
                },
                CommonSiteName = "Office Guy Music Steakhouse"
            }; 
            _lastCreatedGeneralWorkOrder.OperatingCenter = new MapCall.Common.Model.Entities.OperatingCenter {
                SAPEnabled = workOrder.OperatingCenter.SAPEnabled,
                SAPWorkOrdersEnabled = workOrder.OperatingCenter.SAPWorkOrdersEnabled
            };
            if (workOrder.WorkDescriptionID > 0)
            {
                _lastCreatedGeneralWorkOrder.WorkDescription = new MapCall.Common.Model.Entities.WorkDescription();
                _lastCreatedGeneralWorkOrder.WorkDescription.SetPropertyValueByName("Id", workOrder.WorkDescriptionID);
            }
            if (workOrder.DateCompleted.HasValue)
                _lastCreatedGeneralWorkOrder.DateCompleted = workOrder.DateCompleted;
            if (workOrder.CancelledAt.HasValue)
                _lastCreatedGeneralWorkOrder.CancelledAt = workOrder.CancelledAt;
            if (service != null)
            {
                _lastCreatedGeneralWorkOrder.Service = new Service {
                    PreviousServiceCustomerMaterial = service.PreviousServiceCustomerMaterial,
                    PreviousServiceSize = service.PreviousServiceSize,
                    CustomerSideMaterial = service.CustomerSideMaterial,
                    CustomerSideSize = service.CustomerSideSize
                };
            }

            _iGeneralWorkOrderRepositoryMock.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(_lastCreatedGeneralWorkOrder);
            if (workOrder.IsPremiseLinkedToSampleSite)
            {
                _ISampleSiteRepositoryMock
                   .Setup(x => x.Find(workOrder.SampleSite.Id))
                   .Returns(_lastCreatedGeneralSampleSite);
            }
        }

        #endregion

        #region Updating

        [TestMethod]
        public void TestFinalizationCompletionSetsPreviousAndCustomerServiceMaterialsAndSizesOnTheServiceRecordUponCompletion()
        {
            var orderID = 123;
            // order to be sent in EventArgs
            var order = new TestWorkOrderBuilder()
                       .WithDateCompleted(null)
                       .WithCompletedByID(null)
                       .WithApprovedOn(DateTime.Today)
                       .WithApprovedByID(321)
                       .WithPremiseNumber("9180569554")
                       .WithAccountCharged("123456789")
                       .Build();

            // existing order, to be updated with the data from EventArgs
            var oldOrder = new TestWorkOrderBuilder()
                          .WithDateCompleted(DateTime.Today)
                          .WithCompletedByID(123)
                          .Build();
            order.WorkOrderID = oldOrder.WorkOrderID = orderID;
            order.IsPremiseLinkedToSampleSite = false;
            SetupGeneralWorkOrderFrom271WorkOrder(order);

            var expectedPreviousServiceMaterial = new MapCall.Common.Model.Entities.ServiceMaterial();
            var expectedPreviousServiceSize = new MapCall.Common.Model.Entities.ServiceSize();
            var expectedCustomerServiceMaterial = new MapCall.Common.Model.Entities.ServiceMaterial();
            var expectedCustomerServiceSize = new MapCall.Common.Model.Entities.ServiceSize();
            var expectedServiceMaterial = new MapCall.Common.Model.Entities.ServiceMaterial();
            var expectedServiceSize = new MapCall.Common.Model.Entities.ServiceSize();
            var service = new MapCall.Common.Model.Entities.Service();

            _lastCreatedGeneralWorkOrder.Service = service;
            _lastCreatedGeneralWorkOrder.PreviousServiceLineMaterial = expectedPreviousServiceMaterial;
            _lastCreatedGeneralWorkOrder.PreviousServiceLineSize = expectedPreviousServiceSize;
            _lastCreatedGeneralWorkOrder.CustomerServiceLineMaterial = expectedCustomerServiceMaterial;
            _lastCreatedGeneralWorkOrder.CustomerServiceLineSize = expectedCustomerServiceSize;
            _lastCreatedGeneralWorkOrder.CompanyServiceLineMaterial = expectedServiceMaterial;
            _lastCreatedGeneralWorkOrder.CompanyServiceLineSize = expectedServiceSize;

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Finalization);
                SetupResult.For(_repository.Get(orderID)).Return(oldOrder);
                SetupResult.For(_repository.SelectedDataKey).Return(order.WorkOrderID);
                //_repository.UpdateCurrentEntityLiterally(oldOrder);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                new EventRaiser((IMockedObject)_view, "Updating").Raise(
                    null,
                    new EntityEventArgs<WorkOrder>(order));
                _serviceRepository.Verify(x => x.Save(service));

                Assert.AreSame(expectedPreviousServiceMaterial,
                    service.PreviousServiceMaterial);
                Assert.AreSame(expectedPreviousServiceSize,
                    service.PreviousServiceSize);
                Assert.AreSame(expectedCustomerServiceMaterial,
                    service.CustomerSideMaterial);
                Assert.AreSame(expectedCustomerServiceSize,
                    service.CustomerSideSize);
                Assert.AreSame(expectedServiceMaterial,
                    service.ServiceMaterial);
                Assert.AreSame(expectedServiceSize,
                    service.ServiceSize);
            }
        }

        [TestMethod]
        public void TestFinalizationCompletionSetsDigitalAsBuiltRequiredIfWorkDescriptionCallsForIt()
        {
            var orderID = 123;
            // order to be sent in EventArgs
            var order = new TestWorkOrderBuilder()
                       .WithDateCompleted(null)
                       .WithCompletedByID(null)
                       .WithApprovedOn(DateTime.Today)
                       .WithApprovedByID(321)
                       .WithAccountCharged("123456789")
                       .Build();

            SetupGeneralWorkOrderFrom271WorkOrder(order);

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Finalization);
                SetupResult.For(_repository.SelectedDataKey).Return(order.WorkOrderID);
                SetupResult.For(_workDescriptionRepository.Get(order.WorkDescription.WorkDescriptionID))
                           .Return(new WorkDescription {
                                DigitalAsBuiltRequired = true
                            });
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                new EventRaiser((IMockedObject)_view, "Updating").Raise(
                    null,
                    new EntityEventArgs<WorkOrder>(order));

                Assert.IsTrue(order.DigitalAsBuiltRequired);
            }
        }

        [TestMethod]
        public void TestFinalizationCompletionDoesNotOverwritePreviousAndCustomerServiceMaterialsAndSizesOnTheServiceRecordUponCompletion()
        {
            var orderID = 123;
            // order to be sent in EventArgs
            var order = new TestWorkOrderBuilder()
                       .WithDateCompleted(null)
                       .WithCompletedByID(null)
                       .WithApprovedOn(DateTime.Today)
                       .WithApprovedByID(321)
                       .WithAccountCharged("123456789")
                       .Build();

            // existing order, to be updated with the data from EventArgs
            var oldOrder = new TestWorkOrderBuilder()
                          .WithDateCompleted(DateTime.Today)
                          .WithCompletedByID(123)
                          .Build();
            order.IsPremiseLinkedToSampleSite = false;
            order.WorkOrderID = oldOrder.WorkOrderID = orderID;
            var expectedPreviousServiceMaterial = new MapCall.Common.Model.Entities.ServiceMaterial();
            var expectedPreviousServiceSize = new MapCall.Common.Model.Entities.ServiceSize();
            var expectedCustomerServiceMaterial = new MapCall.Common.Model.Entities.ServiceMaterial();
            var expectedCustomerServiceSize = new MapCall.Common.Model.Entities.ServiceSize();
            var service = new Service {
                PreviousServiceMaterial = expectedPreviousServiceMaterial,
                PreviousServiceSize = expectedPreviousServiceSize,
                CustomerSideMaterial = expectedCustomerServiceMaterial,
                CustomerSideSize = expectedCustomerServiceSize
            };

            SetupGeneralWorkOrderFrom271WorkOrder(order, false, service);

            _lastCreatedGeneralWorkOrder.Service = service;

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Finalization);
                SetupResult.For(_repository.Get(orderID)).Return(oldOrder);
                SetupResult.For(_repository.SelectedDataKey).Return(order.WorkOrderID);
                //_repository.UpdateCurrentEntityLiterally(oldOrder);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                new EventRaiser((IMockedObject)_view, "Updating").Raise(
                    null,
                    new EntityEventArgs<WorkOrder>(order));
                _serviceRepository.Verify(x => x.Save(service));

                Assert.AreSame(expectedPreviousServiceMaterial, service.PreviousServiceMaterial);
                Assert.AreSame(expectedPreviousServiceSize, service.PreviousServiceSize);
                Assert.AreSame(expectedCustomerServiceMaterial, service.CustomerSideMaterial);
                Assert.AreSame(expectedCustomerServiceSize, service.CustomerSideSize);
            }
        }

        [TestMethod]
        public void TestUpdateForApprovalDoesNotSetsPreviousAndCustomerServiceMaterialsAndSizesOnTheServiceRecordUponRejection()
        {
            var orderID = 123;
            // order to be sent in EventArgs
            var order = new TestWorkOrderBuilder()
                       .WithDateCompleted(null)
                       .WithCompletedByID(null)
                       .WithApprovedOn(DateTime.Today)
                       .WithApprovedByID(null)
                       .WithAccountCharged("123456789")
                       .Build();

            // existing order, to be updated with the data from EventArgs
            var oldOrder = new TestWorkOrderBuilder()
                          .WithDateCompleted(DateTime.Today)
                          .WithCompletedByID(123)
                          .Build();
            order.WorkOrderID = oldOrder.WorkOrderID = orderID;
            order.IsPremiseLinkedToSampleSite = false;
            var mat1 = new MapCall.Common.Model.Entities.ServiceMaterial();
            var size1 = new MapCall.Common.Model.Entities.ServiceSize();
            var service = new Service {
                PreviousServiceMaterial = mat1, 
                PreviousServiceSize = size1,
            };

            SetupGeneralWorkOrderFrom271WorkOrder(order, false, service);

            var expectedPreviousServiceMaterial = new MapCall.Common.Model.Entities.ServiceMaterial();
            var expectedPreviousServiceSize = new MapCall.Common.Model.Entities.ServiceSize();
            var expectedCustomerServiceMaterial = new MapCall.Common.Model.Entities.ServiceMaterial();
            var expectedCustomerServiceSize = new MapCall.Common.Model.Entities.ServiceSize();
            var expectedServiceMaterial = new MapCall.Common.Model.Entities.ServiceMaterial();
            var expectedServiceSize = new MapCall.Common.Model.Entities.ServiceSize();

            _lastCreatedGeneralWorkOrder.Service = service;
            _lastCreatedGeneralWorkOrder.PreviousServiceLineMaterial = expectedPreviousServiceMaterial;
            _lastCreatedGeneralWorkOrder.PreviousServiceLineSize = expectedPreviousServiceSize;
            _lastCreatedGeneralWorkOrder.CustomerServiceLineMaterial = expectedCustomerServiceMaterial;
            _lastCreatedGeneralWorkOrder.CustomerServiceLineSize = expectedCustomerServiceSize;
            _lastCreatedGeneralWorkOrder.CompanyServiceLineMaterial = expectedServiceMaterial;
            _lastCreatedGeneralWorkOrder.CompanyServiceLineSize = expectedServiceSize;

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Approval);
                SetupResult.For(_repository.Get(orderID)).Return(oldOrder);
                _repository.UpdateCurrentEntityLiterally(oldOrder);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                new EventRaiser((IMockedObject)_view, "Updating").Raise(
                    null,
                    new EntityEventArgs<WorkOrder>(order));

                Assert.IsNull(oldOrder.DateCompleted);
                Assert.IsNull(oldOrder.CompletedByID);
                Assert.AreEqual(order.ApprovedByID, oldOrder.ApprovedByID);
                Assert.AreEqual(order.ApprovedOn, oldOrder.ApprovedOn);
                Assert.AreEqual(order.AccountCharged, oldOrder.AccountCharged);
            }

            _serviceRepository.Verify(x => x.Save(service), Times.Never);

            //make sure these aren't overwritten with values
            Assert.AreSame(mat1, service.PreviousServiceMaterial);
            Assert.AreSame(size1, service.PreviousServiceSize);
            //make sure these aren't ovewritten when they were null
            Assert.IsNull(service.CustomerSideMaterial);
            Assert.IsNull(service.CustomerSideSize);
            Assert.IsNull(service.ServiceMaterial);
            Assert.IsNull(service.ServiceSize);
        }

        [TestMethod]
        public void TestUpdateForApprovalDoesNotTrySettingAnythingOnServiceWhenServiceIsNullUponApproval()
        {
            var orderID = 123;
            // order to be sent in EventArgs
            var order = new TestWorkOrderBuilder()
                       .WithDateCompleted(null)
                       .WithCompletedByID(null)
                       .WithApprovedOn(DateTime.Today)
                       .WithApprovedByID(32)
                       .WithAccountCharged("123456789")
                       .Build();

            // existing order, to be updated with the data from EventArgs
            var oldOrder = new TestWorkOrderBuilder()
                          .WithDateCompleted(DateTime.Today)
                          .WithCompletedByID(123)
                          .Build();
            order.WorkOrderID = oldOrder.WorkOrderID = orderID;
            order.IsPremiseLinkedToSampleSite = false;

            SetupGeneralWorkOrderFrom271WorkOrder(order);

            _lastCreatedGeneralWorkOrder.Service = null;

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Approval);
                SetupResult.For(_repository.Get(orderID)).Return(oldOrder);
                _repository.UpdateCurrentEntityLiterally(oldOrder);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                new EventRaiser((IMockedObject)_view, "Updating").Raise(
                    null,
                    new EntityEventArgs<WorkOrder>(order));

                Assert.IsNull(oldOrder.DateCompleted);
                Assert.IsNull(oldOrder.CompletedByID);
                Assert.AreEqual(order.ApprovedByID, oldOrder.ApprovedByID);
                Assert.AreEqual(order.ApprovedOn, oldOrder.ApprovedOn);
                Assert.AreEqual(order.AccountCharged, oldOrder.AccountCharged);
            }

            // This is the only way to really test this. By all rights the code should throw exceptions if 
            // the null Service check is removed.
            _serviceRepository.Verify(x => x.Save((MapCall.Common.Model.Entities.Service)null), Times.Never);
        }

        [TestMethod]
        public void TestUpdatingCommandUpdatesEntityNormallyWhenPhaseIsNotApprovalOrPhaseIsGeneralAndAssetTypeHasNotChanged()
        {
            foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                if (phase == WorkOrderPhase.Approval) continue;

                var orderID = 123;
                var order = new TestWorkOrderBuilder()
                           .WithWorkOrderID(orderID)
                           .Build();
                order.IsPremiseLinkedToSampleSite = false;

                SetupGeneralWorkOrderFrom271WorkOrder(order);


                using (_mocks.Record())
                {
                    SetupResult.For(_view.Phase).Return(phase);
                    _repository.UpdateCurrentEntity(order);

                    if (phase == WorkOrderPhase.General)
                    {
                        SetupResult.For(_repository.Get(orderID)).Return(order);
                    }
                    if (phase == WorkOrderPhase.Finalization)
                    {
                        SetupResult.For(_repository.SelectedDataKey).Return(order.WorkOrderID);
                        //_repository.SetSelectedDataKey(order.WorkOrderID);
                    }
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();

                    new EventRaiser((IMockedObject)_view, "Updating").Raise(
                        null,
                        new EntityEventArgs<WorkOrder>(order));
                }

                _iWorkOrdersWorkOrderRepository.Verify(x => x.UpdateSAPWorkOrder(It.IsAny<WorkOrder>(), It.IsAny<MapCall.Common.Model.Entities.WorkOrder>()), Times.AtLeastOnce);

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestUpdatingCommandMergesDataFromOldOrderAndUpdatesLiterallyWhenPhaseIsApproval()
        {
            var orderID = 123;
            // order to be sent in EventArgs
            var order = new TestWorkOrderBuilder()
                       .WithDateCompleted(null)
                       .WithCompletedByID(null)
                       .WithApprovedOn(DateTime.Today)
                       .WithApprovedByID(321)
                       .WithAccountCharged("123456789")
                       .Build();
            // existing order, to be updated with the data from EventArgs
            var oldOrder = new TestWorkOrderBuilder()
                          .WithDateCompleted(DateTime.Today)
                          .WithCompletedByID(123)
                          .Build();
            order.IsPremiseLinkedToSampleSite = false;
            order.WorkOrderID = oldOrder.WorkOrderID = orderID;

            SetupGeneralWorkOrderFrom271WorkOrder(order);

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Approval);
                SetupResult.For(_repository.Get(orderID)).Return(oldOrder);
                _repository.UpdateCurrentEntityLiterally(oldOrder);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                new EventRaiser((IMockedObject)_view, "Updating").Raise(
                    null,
                    new EntityEventArgs<WorkOrder>(order));

                Assert.IsNull(oldOrder.DateCompleted);
                Assert.IsNull(oldOrder.CompletedByID);
                Assert.AreEqual(order.ApprovedByID, oldOrder.ApprovedByID);
                Assert.AreEqual(order.ApprovedOn, oldOrder.ApprovedOn);
                Assert.AreEqual(order.AccountCharged, oldOrder.AccountCharged);
            }

            _iWorkOrdersWorkOrderRepository.Verify(x => x.UpdateSAPWorkOrder(It.IsAny<WorkOrder>(), It.IsAny<MapCall.Common.Model.Entities.WorkOrder>()), Times.Once);
        }

        // TODO: this test is tough to pull off
        //[TestMethod]
        //public void TestUpdatingCOmmandMergesDataFromOldOrderAndUpdatesLiterallyWhenPhaseIsGeneralAndAssetTypeHasChanged()
        //{
        //    var orderID = 123;
        //    var newOrder = new TestWorkOrderBuilder()
        //        .WithWorkOrderID(orderID)
        //        .WithAssetType(new TestAssetTypeBuilder<Valve>())
        //        .WithValve(new TestValveBuilder().WithValveID(456))
        //        .Build();
        //    var oldOrder = new TestWorkOrderBuilder()
        //        .WithWorkOrderID(orderID)
        //        .WithAssetType(new TestAssetTypeBuilder<Hydrant>())
        //        .WithHydrant(new TestHydrantBuilder().WithHydrantID(456))
        //        .Build();

        //    using (_mocks.Record())
        //    {
        //        SetupResult.For(_view.Phase).Return(WorkOrderPhase.General);
        //        SetupResult.For(_repository.Get(orderID)).Return(oldOrder);
        //        _repository.UpdateCurrentEntityLiterally(oldOrder);
        //    }

        //    using (_mocks.Playback())
        //    {
        //        _target.OnViewLoaded();

        //        new EventRaiser((IMockedObject)_view, "Updating").Raise(
        //            null,
        //            new EntityEventArgs<WorkOrder>(newOrder));

        //        Assert.IsNull(oldOrder.HydrantID);
        //        Assert.AreEqual(oldOrder.AssetTypeID, newOrder.AssetTypeID);
        //        Assert.AreEqual(oldOrder.ValveID, newOrder.ValveID);
        //    }
        //}

        [TestMethod]
        public void TestUpdatingCommandMakesViewShowEntityReadOnlyWhenViewPhaseIsNotFinalization()
        {
            var orderID = 123;
            var order = new TestWorkOrderBuilder().Build();
            var oldOrder = new TestWorkOrderBuilder().Build();
            order.WorkOrderID = oldOrder.WorkOrderID = orderID;
            order.IsPremiseLinkedToSampleSite = false;

            SetupGeneralWorkOrderFrom271WorkOrder(order);

            foreach (
                WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                if (phase == WorkOrderPhase.Finalization ||
                    phase == WorkOrderPhase.Approval) continue;

                using (_mocks.Record())
                {
                    SetupResult.For(_view.Phase).Return(phase);
                    SetupResult.For(_repository.CurrentEntity).Return(order);
                    _view.ShowEntity(order);
                    _view.SetViewMode(DetailViewMode.ReadOnly);

                    // This is for the newer stuff involved with the ability for supervisors to reject orders
                    if (phase == WorkOrderPhase.Approval ||
                        phase == WorkOrderPhase.General)
                    {
                        SetupResult.For(_repository.Get(orderID))
                                   .Return(oldOrder);
                    }
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();

                    new EventRaiser((IMockedObject)_view, "Updating").Raise(
                        null, new EntityEventArgs<WorkOrder>(order));
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestUpdatingCommandMakesViewShowEntityReadOnlyWhenPhaseIsFinalizationAndOrderIsComplete()
        {
            var workOrderID = 666;
            var order = new TestWorkOrderBuilder().BuildCompleteOrder().WithWorkOrderID(workOrderID);
            order.WithPremiseNumber("9180610000");
            order.WithIsPremiseLinkedToSampleSite(false);

            SetupGeneralWorkOrderFrom271WorkOrder(order);
            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Finalization);
                SetupResult.For(_repository.CurrentEntity).Return(order);
                _view.ShowEntity(order);
                _view.SetViewMode(DetailViewMode.ReadOnly);
                SetupResult.For(_repository.SelectedDataKey).Return(workOrderID);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                new EventRaiser((IMockedObject)_view, "Updating").Raise(
                    null, new EntityEventArgs<WorkOrder>(order));
            }
        }

        [TestMethod]
        public void TestUpdatingCommandMakesViewShowEntityInEditModeWhenPhaseIsFinalizationAndOrderIsNotComplete()
        {
            var workOrderID = 666;
            var order = new TestWorkOrderBuilder().BuildIncompleteOrder().WithWorkOrderID(workOrderID).WithIsPremiseLinkedToSampleSite(false);
                
            SetupGeneralWorkOrderFrom271WorkOrder(order);

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Finalization);
                SetupResult.For(_repository.CurrentEntity).Return(order);
                _view.ShowEntity(order);
                _view.SetViewMode(DetailViewMode.Edit);
                SetupResult.For(_repository.SelectedDataKey).Return(workOrderID);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                new EventRaiser((IMockedObject)_view, "Updating").Raise(
                    null, new EntityEventArgs<WorkOrder>(order));
            }
        }

        #endregion

        #region Repository_CurrentEntityChanged

        [TestMethod]
        public void TestRepositoryCurrentEntityChangedEventSetsViewModeToEditWhenViewPhaseIsFinalizationOrApproval()
        {
            var phases = new[] { 
                WorkOrderPhase.Finalization, WorkOrderPhase.Approval, WorkOrderPhase.StockApproval
            };

            foreach (var phase in phases)
            {
                using (_mocks.Record())
                {
                    SetupResult.For(_view.Phase).Return(phase);
                    _view.SetViewMode(DetailViewMode.Edit);
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    var currentEntityChanged =
                        new EventRaiser((IMockedObject)_repository,
                            "CurrentEntityChanged");
                    currentEntityChanged.Raise(null, EntityEventArgs<WorkOrder>.Empty);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestRepositoryCurrentEntityChangedEventSetsViewModeToReadOnlyWhenPhaseIsNotFinalizationOrApproval()
        {
            var phasesToSkip = new[] {
                WorkOrderPhase.Finalization, WorkOrderPhase.Approval, WorkOrderPhase.StockApproval
            };

            foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                if (phasesToSkip.Contains(phase))
                    continue;

                using (_mocks.Record())
                {
                    SetupResult.For(_view.Phase).Return(phase);
                    _view.SetViewMode(DetailViewMode.ReadOnly);
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    var currentEntityChanged =
                        new EventRaiser((IMockedObject)_repository,
                            "CurrentEntityChanged");
                    currentEntityChanged.Raise(null, EntityEventArgs<WorkOrder>.Empty);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        #endregion

        #region OnViewInitialized

        [TestMethod]
        public void TestOnViewInitializedDoesNotSetViewModeWhenViewModeIsSet()
        {
            foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                using (_mocks.Record())
                {
                    SetupResult.For(_view.ModeSet).Return(true);
                    SetupResult.For(_view.Phase).Return(phase);
                    DoNotExpect.Call(
                        () => _view.SetViewMode(DetailViewMode.Insert));
                    LastCall.IgnoreArguments();
                }

                using (_mocks.Playback())
                {
                    _target.OnViewInitialized();
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestOnViewInitializedSetsModeToInsertByDefaultWhenViewIsInputAndViewModeNotSet()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_view.ModeSet).Return(false);
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Input);
                _view.SetViewMode(DetailViewMode.Insert);
            }

            using (_mocks.Playback())
            {
                _target.OnViewInitialized();
            }
        }

        [TestMethod]
        public void TestOnViewInitializedSetsModeToEditByDefaultWhenViewIsFinalizationAndViewModeNotSet()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_view.ModeSet).Return(false);
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Finalization);
                _view.SetViewMode(DetailViewMode.Edit);
            }

            using (_mocks.Playback())
            {
                _target.OnViewInitialized();
            }
        }

        #endregion

        #region OnViewLoaded

        [TestMethod]
        public void TestOnViewLoadedWiresRepositoryEntityInsertedAndEntityUpdatedEvents()
        {
            using (_mocks.Record())
            {
                _repository.EntityUpdated += null;
                LastCall.IgnoreArguments();

                _repository.EntityInserted += null;
                LastCall.IgnoreArguments();
            }
            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        #endregion

        #region Repository_EntityUpdated

        /// <summary>
        ///  If the record did not have a completed date and now has one
        ///  If it's a main break
        ///  Send an email to the people signed up
        ///  Send an email to the town contacts
        /// </summary>
        [TestMethod]
        public void TestRepositoryEntityUpdatedEventNotifiesIfOrderIsCompletedForMainBreaks()
        {
            var workOrderId = 5;
            var notifier = _mocks.CreateMock<INotificationService>();
            var town = new Town();
            _container.Inject(notifier);

            foreach (var workDescriptionID in WorkDescriptionRepository.MAIN_BREAKS)
            {
                var workOrder = new WorkOrder {
                    Town = town,
                    OperatingCenterID = 123,
                    WorkDescriptionID = workDescriptionID,
                    DateCompleted = DateTime.Now,
                    WorkOrderID = workOrderId,
                    PremiseNumber = "9180617000",
                    IsPremiseLinkedToSampleSite = false,
                    OperatingCenter = new OperatingCenter { SAPEnabled = false, OperatingCenterID = 12 }
                };
                SetupGeneralWorkOrderFrom271WorkOrder(workOrder);
                var args = new EntityEventArgs<WorkOrder>(workOrder);
                using (_mocks.Record())
                {
                    SetupResult.For(_repository.Get(workOrderId)).Return(workOrder);
                    SetupResult.For(_view.Phase).Return(WorkOrderPhase.Finalization);
                    notifier.Notify(workOrder.OperatingCenterID.Value,
                        RoleModules.FieldServicesWorkManagement,
                        WorkOrderDetailPresenter.MAIN_BREAK_COMPLETED_NOTIFICATION,
                        workOrder);
                    SetupResult.For(_repository.SelectedDataKey).Return(workOrder.WorkOrderID);
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    var currentEntityChanged = new EventRaiser((IMockedObject)_view, "Updating");
                    currentEntityChanged.Raise(null, args);
                    _notificationService.Verify(x => x.Notify(
                        It.IsAny<int>(),
                        It.IsAny<RoleModules>(),
                        It.IsAny<string>(),
                        It.IsAny<MapCall.Common.Model.Entities.WorkOrder>(),
                        It.IsAny<string>(),
                        null,
                        null), Times.Never);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestRepositoryEntityUpdateEventNotifiesIfWorkOrderDescriptionChangedToSewerOverFlow()
        {
            var town = new Town();
            var contact = new Contact();
            contact.SetHiddenFieldValueByName("_email", "a@a.com");
            var contactType = new ContactType();
            contactType.SetHiddenFieldValueByName("_contactTypeID", 8);

            foreach (var description in WorkDescriptionRepository.SEWER_OVERFLOW)
            {
                var workOrder = new WorkOrder 
                {
                    Town = town,
                    OperatingCenterID = 123,
                    WorkDescriptionID = description,
                    DateCompleted = DateTime.Now,
                    WorkOrderID = 100,
                    PremiseNumber = "9180617000",
                    IsPremiseLinkedToSampleSite = false,
                    OperatingCenter = new OperatingCenter
                    {
                        SAPEnabled = false,
                        OperatingCenterID = 123
                    }
                };
                SetupGeneralWorkOrderFrom271WorkOrder(workOrder);
                var args = new EntityEventArgs<WorkOrder>(workOrder);
                using (_mocks.Record())
                {
                    SetupResult.For(_repository.Get(100)).Return(workOrder);
                    SetupResult.For(_view.Phase).Return(WorkOrderPhase.Finalization);
                    SetupResult.For(_repository.SelectedDataKey).Return(workOrder.WorkOrderID);
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    var currentEntityChanged = new EventRaiser((IMockedObject)_view, "Updating");
                    currentEntityChanged.Raise(null, args);
                    _notificationService.Verify(x => x.Notify(
                        It.IsAny<int>(),
                        It.IsAny<RoleModules>(),
                        It.IsAny<string>(),
                        It.IsAny<MapCall.Common.Model.Entities.WorkOrder>(),
                        It.IsAny<string>(),
                        null,
                        null), Times.Never);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestRepositoryEntityUpdateEventNotifiesIfRequestedByChangedToAcousticMonitoring()
        {
            var notifier = _mocks.CreateMock<INotificationService>();
            
            var entity = new WorkOrder {
                Town = new Town(),
                OperatingCenterID = 123,
                WorkDescriptionID = 5,
                CancelledAt = DateTime.Now,
                WorkOrderID = 5,
                RequesterID = 6,
                SampleSite = new SampleSite {
                    Premise = new Premise {
                        PremiseNumber = "9180617886"
                    }
                },
                PremiseNumber = "9180617886",
                IsPremiseLinkedToSampleSite = true,
                OperatingCenter = new OperatingCenter { SAPEnabled = false, OperatingCenterID = 12 },
            };

            SetupGeneralWorkOrderFrom271WorkOrder(entity);

            //Original Data
            var workOrder = new MapCall.Common.Model.Entities.WorkOrder {
                RequestedBy = new WorkOrderRequester {
                    Id = 5
                }
            };
                
            _container.Inject(notifier);

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.General);
                SetupResult.For(_repository.Get(entity.WorkOrderID)).Return(entity);

                notifier.Notify(entity.OperatingCenter.OperatingCenterID,
                    RoleModules.FieldServicesWorkManagement,
                    WorkOrderDetailPresenter.ACOUSTIC_MONITORING_CREATED,
                    entity);
            }

            using (_mocks.Playback())
            {
                _target.CallTrySendAcousticMonitoringNotification(workOrder, entity);
            }
        }
        

        [TestMethod]
        public void TestRepositoryEntityUpdatedEventTriesToNotifyIfViewPhaseIsApprovalAndOrderIsApproved()
        {
            var tryNotifyCalledCorrectly = false;
            var workOrder = new WorkOrder {
                WorkOrderID = 1,
                OperatingCenterID = 123,
                ApprovedOn = DateTime.Today,
                WorkDescriptionID = (int)MapCall.Common.Model.Entities.
                                                 WorkDescription.Indices.HYDRANT_INSTALLATION
            };
            var args = new EntityEventArgs<WorkOrder>(workOrder);
            _target.MockTryApprovalNotification =
                order => tryNotifyCalledCorrectly = (order == workOrder);

            using (_mocks.Record())
            {
                SetupResult.For(_repository.Get(workOrder.WorkOrderID)).Return(workOrder);
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Approval);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                var currentEntityChanged =
                    new EventRaiser((IMockedObject)_repository,
                        "EntityUpdated");
                currentEntityChanged.Raise(null, args);
            }

            Assert.IsTrue(tryNotifyCalledCorrectly);
        }

        [TestMethod]
        public void TestRepositoryEntityUpdatedEventTriesToNotifyIfViewPhaseIsGeneralAndOrderIsApproved()
        {
            var tryNotifyCalledCorrectly = false;
            var workOrder = new WorkOrder {
                WorkOrderID = 1,
                OperatingCenterID = 123,
                ApprovedOn = DateTime.Today,
                WorkDescriptionID = (int)MapCall.Common.Model.Entities.
                                                 WorkDescription.Indices.HYDRANT_INSTALLATION
            };
            var args = new EntityEventArgs<WorkOrder>(workOrder);
            _target.MockTryApprovalNotification =
                order => tryNotifyCalledCorrectly = (order == workOrder);

            using (_mocks.Record())
            {
                SetupResult.For(_repository.Get(workOrder.WorkOrderID)).Return(workOrder);
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.General);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                var currentEntityChanged =
                    new EventRaiser((IMockedObject)_repository,
                        "EntityUpdated");
                currentEntityChanged.Raise(null, args);
            }

            Assert.IsTrue(tryNotifyCalledCorrectly);
        }

        [TestMethod]
        public void TestRepositoryEntityUpdatedEventDoesNotTryToNotifyIfViewPhaseIsApprovalButOrderIsNotApproved()
        {
            var tryNotifyCalledCorrectly = false;
            var workOrder = new WorkOrder
            {
                OperatingCenterID = 123
            };
            var args = new EntityEventArgs<WorkOrder>(workOrder);

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Approval);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                var currentEntityChanged =
                    new EventRaiser((IMockedObject)_repository,
                        "EntityUpdated");
                currentEntityChanged.Raise(null, args);
            }
        }

        [TestMethod]
        public void TestRepositoryEntityUpdatedEventDoesNotNotifyIfPhaseIsNotApprovalOrInput()
        {
            var tryNotifyCalled = false;
            _target.MockTryApprovalNotification = order => tryNotifyCalled = true;

            foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                if (phase == WorkOrderPhase.Approval)
                {
                    continue;
                }

                using (_mocks.Record())
                {
                    SetupResult.For(_view.Phase).Return(phase);
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    var currentEntityChanged =
                        new EventRaiser((IMockedObject)_repository,
                            "EntityUpdated");
                    currentEntityChanged.Raise(null,
                        EntityEventArgs<WorkOrder>.Empty);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
                Assert.IsFalse(tryNotifyCalled);
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestRepositoryEntityUpdatedNotifiesAllWhenChangingToMainBreakDescription()
        {
            var workOrderId = 5;
            var town = new Town();
            var contact = new Contact();
            contact.SetHiddenFieldValueByName("_email", "a@a.com");
            var contactType = new ContactType();
            contactType.SetHiddenFieldValueByName("_contactTypeID", 8);
            var townContact = new TownContact();
            townContact.Contact = contact;
            townContact.ContactType = contactType;
            town.TownContacts.Add(townContact);
            //foreach (var contact in workorder.Town.TownContacts.Where(x =>x.ContactType.ContactTypeID == 8)) 
            foreach (var workdescriptionID in WorkDescriptionRepository.MAIN_BREAKS)
            {
                var workOrder = new WorkOrder {
                    Town = town,
                    OperatingCenterID = 123,
                    WorkDescriptionID = 5,
                    PremiseNumber = "9180617000",
                    IsPremiseLinkedToSampleSite = false,
                    WorkOrderID = workOrderId,
                    OperatingCenter = new OperatingCenter {
                        SAPEnabled = false, 
                        OperatingCenterID = 12
                    }
                };
                SetupGeneralWorkOrderFrom271WorkOrder(workOrder);
                workOrder.WorkDescriptionID = workdescriptionID;
                var args = new EntityEventArgs<WorkOrder>(workOrder);
                using (_mocks.Record())
                {
                    SetupResult.For(_repository.Get(workOrderId)).Return(workOrder);
                    SetupResult.For(_repository.SelectedDataKey).Return(workOrder.WorkOrderID);
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    var currentEntityChanged =
                        new EventRaiser((IMockedObject)_view, "Updating");
                    currentEntityChanged.Raise(null, args);
                    _notificationService.Verify(x => x.Notify(
                        12,
                        RoleModules.FieldServicesWorkManagement,
                        WorkOrderDetailPresenter.MAIN_BREAK_NOTIFICATION,
                        It.IsAny<WorkOrder>(),
                        WorkOrderDetailPresenter.WORK_DESCRIPTION_CHANGED,
                        null,
                        null), Times.AtLeastOnce);
                    _notificationService.Verify(x => x.Notify(
                        12,
                        RoleModules.FieldServicesWorkManagement,
                        WorkOrderDetailPresenter.MAIN_BREAK_NOTIFICATION,
                        It.IsAny<WorkOrder>(),
                        null,
                        townContact.Contact.Email,
                        null), Times.AtLeastOnce);
                    //INotificationService.Notify(12, FieldServicesWorkManagement, "Main Break Entered", WorkOrders.Model.WorkOrder, null, "a@a.com")
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestRepositoryEntityUpdatedDoesNotNotifyAllWhenChangingToMainBreakDescriptionFromMainBreakDescription()
        {
            var workOrderId = 5;
            var town = new Town();

            foreach (var workdescriptionID in WorkDescriptionRepository.MAIN_BREAKS)
            {
                var workOrder = new WorkOrder 
                {
                    Town = town,
                    OperatingCenterID = 123,
                    WorkDescriptionID = (workdescriptionID == WorkDescriptionRepository.MAIN_BREAKS[0]) ? WorkDescriptionRepository.MAIN_BREAKS[1] : WorkDescriptionRepository.MAIN_BREAKS[0],
                    WorkOrderID = workOrderId,
                    PremiseNumber = "9180617000",
                    IsPremiseLinkedToSampleSite = false,
                    OperatingCenter = new OperatingCenter 
                    {
                        SAPEnabled = false, 
                        OperatingCenterID = 12
                    }
                };
                SetupGeneralWorkOrderFrom271WorkOrder(workOrder);
                workOrder.WorkDescriptionID = workdescriptionID;
                var args = new EntityEventArgs<WorkOrder>(workOrder);
                using (_mocks.Record())
                {
                    SetupResult.For(_repository.Get(workOrderId)).Return(workOrder);
                    SetupResult.For(_repository.SelectedDataKey).Return(workOrder.WorkOrderID);
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    var currentEntityChanged =
                        new EventRaiser((IMockedObject)_view, "Updating");
                    currentEntityChanged.Raise(null, args);
                    _notificationService.Verify(x => x.Notify(
                        workOrder.OperatingCenterID.Value,
                        RoleModules.FieldServicesWorkManagement,
                        WorkOrderDetailPresenter.MAIN_BREAK_NOTIFICATION,
                        It.IsAny<MapCall.Common.Model.Entities.WorkOrder>(),
                        WorkOrderDetailPresenter.WORK_DESCRIPTION_CHANGED,
                        null,
                        null), Times.Never);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestRepositoryEntityUpdatedDoesNotNotifyAllWhenChangingToMainBreakDescriptionWhenCancelled()
        {
            var workOrderId = 5;
            var town = new Town();

            foreach (var workdescriptionID in WorkDescriptionRepository.MAIN_BREAKS)
            {
                var workOrder = new WorkOrder 
                {
                    Town = town,
                    OperatingCenterID = 123,
                    WorkDescriptionID = 5,
                    IsPremiseLinkedToSampleSite = false,
                    CancelledAt = DateTime.Now,
                    WorkOrderID = workOrderId,
                    OperatingCenter = new OperatingCenter
                    {
                        SAPEnabled = false, 
                        OperatingCenterID = 12
                    }
                };
                SetupGeneralWorkOrderFrom271WorkOrder(workOrder);
                workOrder.WorkDescriptionID = workdescriptionID;
                var args = new EntityEventArgs<WorkOrder>(workOrder);
                using (_mocks.Record())
                {
                    SetupResult.For(_repository.Get(workOrderId)).Return(workOrder);
                    SetupResult.For(_repository.SelectedDataKey).Return(workOrder.WorkOrderID);
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    var currentEntityChanged =
                        new EventRaiser((IMockedObject)_view, "Updating");
                    currentEntityChanged.Raise(null, args);
                    _notificationService.Verify(x => x.Notify(
                        workOrder.OperatingCenterID.Value,
                        RoleModules.FieldServicesWorkManagement,
                        WorkOrderDetailPresenter.MAIN_BREAK_NOTIFICATION,
                        It.IsAny<MapCall.Common.Model.Entities.WorkOrder>(),
                        WorkOrderDetailPresenter.WORK_DESCRIPTION_CHANGED,
                        null,
                        null), Times.Never);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        #endregion

        #region TryApprovalNotification

        [TestMethod]
        public void TestTryApprovalNotificationNotifiesForCurbPitComplianceUnderCorrectConditions()
        {
            var notifier = _mocks.CreateMock<INotificationService>();
            var workOrder = new WorkOrder 
            {
                OperatingCenterID = 123,
                PurposeID = WorkOrderPurposeRepository.Indices.COMPLIANCE
            };

            _container.Inject(notifier);

            foreach (var workDescriptionID in WorkDescriptionRepository.CURB_PIT)
            {
                workOrder.WorkDescriptionID = workDescriptionID;

                using (_mocks.Record())
                {
                    notifier.Notify(workOrder.OperatingCenterID.Value,
                        RoleModules.FieldServicesWorkManagement,
                        WorkOrderDetailPresenter.SupervisorApprovalNotifications
                                                .CURB_PIT_COMPLIANCE, workOrder);
                }

                using (_mocks.Playback())
                {
                    _target.CallTryApprovalNotification(workOrder);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestTryApprovalNotificationNotifiesForCurbPitRevenueUnderCorrectConditions()
        {
            var notifier = _mocks.CreateMock<INotificationService>();
            var workOrder = new WorkOrder {
                OperatingCenterID = 123
            };

            _container.Inject(notifier);

            foreach (var purposeID in WorkOrderPurposeRepository.REVENUE)
            foreach (var workDescriptionID in WorkDescriptionRepository.CURB_PIT)
            {
                workOrder.PurposeID = purposeID;
                workOrder.WorkDescriptionID = workDescriptionID;

                using (_mocks.Record())
                {
                    notifier.Notify(workOrder.OperatingCenterID.Value,
                        RoleModules.FieldServicesWorkManagement,
                        WorkOrderDetailPresenter.SupervisorApprovalNotifications
                                                .CURB_PIT_REVENUE, workOrder);
                }

                using (_mocks.Playback())
                {
                    _target.CallTryApprovalNotification(workOrder);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestTryApprovalNotificationNotifiesForCurbPitEstimateUnderCorrectConditions()
        {
            var notifier = _mocks.CreateMock<INotificationService>();
            var workOrder = new WorkOrder 
            {
                OperatingCenterID = 123,
                PurposeID = WorkOrderPurposeRepository.Indices.ESTIMATES
            };

            _container.Inject(notifier);

            foreach (var workDescriptionID in WorkDescriptionRepository.CURB_PIT)
            {
                workOrder.WorkDescriptionID = workDescriptionID;

                using (_mocks.Record())
                {
                    notifier.Notify(workOrder.OperatingCenterID.Value,
                        RoleModules.FieldServicesWorkManagement,
                        WorkOrderDetailPresenter.SupervisorApprovalNotifications
                                                .CURB_PIT_ESTIMATE, workOrder);
                }

                using (_mocks.Playback())
                {
                    _target.CallTryApprovalNotification(workOrder);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestTryApprovalNotificationNotifiesForAssetsUnderCorrectConditions()
        {
            var notifier = _mocks.CreateMock<INotificationService>();
            var workOrder = new WorkOrder 
            {
                OperatingCenterID = 123,
                PurposeID = WorkOrderPurposeRepository.Indices.ESTIMATES
            };

            _container.Inject(notifier);

            foreach (var workDescriptionID in WorkDescriptionRepository.ASSET_COMPLETION)
            {
                workOrder.WorkDescriptionID = workDescriptionID;

                using (_mocks.Record())
                {
                    notifier.Notify(workOrder.OperatingCenterID.Value,
                        RoleModules.FieldServicesWorkManagement,
                        WorkOrderDetailPresenter.SupervisorApprovalNotifications
                                                .ASSET_ORDER_COMPLETED, workOrder);
                }

                using (_mocks.Playback())
                {
                    _target.CallTryApprovalNotification(workOrder);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        #endregion

        #region TryCompletedNotification

        [TestMethod]
        public void TestTryCompletedNotificationNotifiesForServiceLineRenewalCompleted()
        {
            var notifier = _mocks.CreateMock<INotificationService>();
            var workOrderId = 31;
            var workOrder = new WorkOrder 
            {
                WorkOrderID = workOrderId,
                OperatingCenterID = 123,
                PurposeID = WorkOrderPurposeRepository.Indices.ESTIMATES
            };
            _container.Inject(notifier);

            foreach (var workDescriptionID in WorkDescriptionRepository.SERVICE_LINE_RENEWALS)
            {
                workOrder.WorkDescriptionID = workOrder.WorkDescriptionID = workDescriptionID;
                workOrder.DateCompleted = DateTime.Now;
                SetupResult.For(_repository.Get(workOrderId)).Return(workOrder);

                using (_mocks.Record())
                {
                    if (workDescriptionID == WorkDescription.SERVICE_LINE_RENEWAL_LEAD)
                    {
                        notifier.Notify(
                            workOrder.OperatingCenterID.Value,
                            RoleModules.FieldServicesWorkManagement,
                            WorkOrderDetailPresenter.SERVICE_LINE_RENEWAL_LEAD_COMPLETED,
                            workOrder);
                    }
                    else
                    {
                        notifier.Notify(
                            workOrder.OperatingCenterID.Value,
                            RoleModules.FieldServicesWorkManagement,
                            WorkOrderDetailPresenter.SERVICE_LINE_RENEWAL_COMPLETED,
                            workOrder);
                    }
                }

                using (_mocks.Playback())
                {
                    _target.CallTryCompletedNotification(workOrder);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestTryCompletedNotificationNotifiesFRCCIfRequestedByFRCC()
        {
            var notifier = _mocks.CreateMock<INotificationService>();
            _container.Inject(notifier);
            var workOrderId = 31;
            var workOrder = new WorkOrder 
            {
                WorkOrderID = workOrderId,
                OperatingCenterID = 123,
                PurposeID = WorkOrderPurposeRepository.Indices.ESTIMATES,
                PriorityID = WorkOrderPriorityRepository.Indices.EMERGENCY,
                RequesterID = WorkOrderRequesterRepository.Indices.FRCC,
                DateCompleted = DateTime.Now
            };

            var args = new EntityEventArgs<WorkOrder>(workOrder);

            using (_mocks.Record())
            {
                SetupResult.For(_repository.Get(workOrderId)).Return(workOrder);
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Finalization);
                notifier.Notify(workOrder.OperatingCenterID.Value, RoleModules.FieldServicesWorkManagement,
                    WorkOrderDetailPresenter.FRCC_EMERGENCY_COMPLETED_NOTIFICATION,
                    workOrder);
            }

            using (_mocks.Playback())
            {
                _target.CallTryCompletedNotification(workOrder);
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            _mocks.ReplayAll();
        }

        #endregion

        #region Repository_EntityInserted

        [TestMethod]
        public void TestRepositoryEntityInsertedEventNotifiesIfPhaseIsInputAndOrderIsForMainBreak()
        {
            var notifier = _mocks.CreateMock<INotificationService>();
            var town = new Town();
            
            _container.Inject(notifier);

            foreach (var workDescriptionID in WorkDescriptionRepository
               .MAIN_BREAKS)
            {
                var workOrder = new WorkOrder {
                    Town = town,
                    OperatingCenterID = 123,
                    WorkDescriptionID = workDescriptionID,
                    WorkOrderID = 1,
                    PremiseNumber = "9180617886",
                    IsPremiseLinkedToSampleSite = false,
                    OperatingCenter = new OperatingCenter {
                        SAPEnabled = false, OperatingCenterID = 12
                    }
                };
                SetupGeneralWorkOrderFrom271WorkOrder(workOrder);

                var args = new EntityEventArgs<WorkOrder>(workOrder);

                using (_mocks.Record())
                {
                    SetupResult.For(_view.Phase).Return(WorkOrderPhase.Input);
                    notifier.Notify(workOrder.OperatingCenterID.Value,
                        RoleModules.FieldServicesWorkManagement,
                        WorkOrderDetailPresenter.
                            MAIN_BREAK_NOTIFICATION,
                        workOrder);
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    var currentEntityChanged =
                        new EventRaiser((IMockedObject)_repository,
                            "EntityInserted");
                    currentEntityChanged.Raise(null, args);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestRepositoryEntityInsertedEventNotifiesIfPhaseIsInputAndOrderIsForMainBreakAndTownContactLinked()
        {
            var notifier = _mocks.CreateMock<INotificationService>();
            var contact = new Contact();
            var contactType = new ContactType();
            contactType.SetHiddenFieldValueByName("_contactTypeID", 8);

            var town = new Town();
            town.TownContacts.Add(new TownContact 
            {
                Town = town, 
                ContactType = contactType, 
                Contact = contact
            });

            _container.Inject(notifier);

            foreach (var workDescriptionID in WorkDescriptionRepository.MAIN_BREAKS)
            {
                var workOrder = new WorkOrder {
                    Town = town,
                    OperatingCenterID = 123,
                    WorkDescriptionID = workDescriptionID,
                    WorkOrderID = 1,
                    PremiseNumber = "9180617886",
                    IsPremiseLinkedToSampleSite = false,
                    OperatingCenter = new OperatingCenter { SAPEnabled = false, OperatingCenterID = 12 }
                };
                SetupGeneralWorkOrderFrom271WorkOrder(workOrder);

                var args = new EntityEventArgs<WorkOrder>(workOrder);

                using (_mocks.Record())
                {
                    SetupResult.For(_view.Phase).Return(WorkOrderPhase.Input);
                    notifier.Notify(workOrder.OperatingCenterID.Value,
                        RoleModules.FieldServicesWorkManagement,
                        WorkOrderDetailPresenter.
                            MAIN_BREAK_NOTIFICATION,
                        workOrder);
                    notifier.Notify(workOrder.OperatingCenterID.Value,
                        RoleModules.FieldServicesWorkManagement,
                        WorkOrderDetailPresenter.
                            MAIN_BREAK_NOTIFICATION,
                        workOrder);
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    var currentEntityChanged =
                        new EventRaiser((IMockedObject)_repository,
                            "EntityInserted");
                    currentEntityChanged.Raise(null, args);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestRepositoryEntityInsertedEventNotifiesIfPhaseIsInputAndAssetTypeIsEquipment()
        {
            var notifier = _mocks.CreateMock<INotificationService>();

            _container.Inject(notifier);

            var workOrder = new WorkOrder {
                OperatingCenterID = 123,
                AssetTypeID = AssetTypeRepository.Indices.EQUIPMENT,
                WorkOrderID = 1,
                PremiseNumber = "9180617000",
                IsPremiseLinkedToSampleSite = false,
                OperatingCenter = new OperatingCenter { SAPEnabled = false, OperatingCenterID = 12 }
            };
            SetupGeneralWorkOrderFrom271WorkOrder(workOrder);

            var args = new EntityEventArgs<WorkOrder>(workOrder);

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Input);
                notifier.Notify(workOrder.OperatingCenterID.Value,
                    RoleModules.FieldServicesWorkManagement,
                    WorkOrderDetailPresenter.
                        EQUIPMENT_REPAIR,
                    workOrder);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                var currentEntityChanged =
                    new EventRaiser((IMockedObject)_repository,
                        "EntityInserted");
                currentEntityChanged.Raise(null, args);
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestRepositoryEntityInsertedEventDoesNotNotifyIfPhaseIsInputButWorkDescriptionDoesNotHaveMatchingPurpose()
        {
            var expected = WorkDescriptionRepository.MAIN_BREAKS
                                                    .Union(WorkDescriptionRepository.SERVICE_LINE_INSTALLATIONS)
                                                    .Union(WorkDescriptionRepository.SERVICE_LINE_RENEWALS)
                                                    .Union(WorkDescriptionRepository.SEWER_OVERFLOW).ToArray();

            var notifier = _mocks.CreateMock<INotifier>();

            _container.Inject(notifier);

            for (var workDescriptionID = 1; workDescriptionID < 500; ++workDescriptionID)
            {
                if (expected.Contains(workDescriptionID))
                    continue;
                var workOrder = new WorkOrder {
                    Town = new Town(),
                    OperatingCenterID = 12,
                    WorkDescriptionID = workDescriptionID,
                    WorkOrderID = 1,
                    PremiseNumber = "9180617000",
                    IsPremiseLinkedToSampleSite = false,
                    OperatingCenter = new OperatingCenter { SAPEnabled = false, OperatingCenterID = 12 }
                };

                SetupGeneralWorkOrderFrom271WorkOrder(workOrder);

                var args = new EntityEventArgs<WorkOrder>(workOrder);

                using (_mocks.Record())
                {
                    SetupResult.For(_view.Phase).Return(WorkOrderPhase.Input);
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    var currentEntityChanged =
                        new EventRaiser((IMockedObject)_repository,
                            "EntityInserted");
                    currentEntityChanged.Raise(null, args);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestRepositoryEntityInsertedEventDoesNotNotifyIfPhaseIsNotInput()
        {
            foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                if (phase == WorkOrderPhase.Input)
                {
                    continue;
                }

                using (_mocks.Record())
                {
                    SetupResult.For(_view.Phase).Return(phase);
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    var currentEntityChanged =
                        new EventRaiser((IMockedObject)_repository,
                            "EntityInserted");
                    currentEntityChanged.Raise(null,
                        EntityEventArgs<WorkOrder>.Empty);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();

                _iWorkOrdersWorkOrderRepository.Verify(x => x.UpdateSAPWorkOrder(It.IsAny<WorkOrder>(), It.IsAny<MapCall.Common.Model.Entities.WorkOrder>()), Times.Never);
            }

            _mocks.ReplayAll();
        }

        #endregion

        #region TrySampleSiteNotification

        [TestMethod]
        public void TestTrySampleSiteNotificationNotifies()
        {
            var notifier = _mocks.CreateMock<INotificationService>();

            var workOrder = new WorkOrder {
                Town = new Town(),
                OperatingCenterID = 123,
                WorkDescriptionID = 5,
                CancelledAt = DateTime.Now,
                WorkOrderID = 5,
                SampleSite = new SampleSite {
                    Premise = new Premise {
                        PremiseNumber = "9180617886"
                    }
                },
                PremiseNumber = "9180617886",
                IsPremiseLinkedToSampleSite = true,
                OperatingCenter = new OperatingCenter { SAPEnabled = false, OperatingCenterID = 12 }
            };

            SetupGeneralWorkOrderFrom271WorkOrder(workOrder);
            
            _container.Inject(notifier);

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.General);
                SetupResult.For(_repository.CurrentEntity).Return(workOrder);
                _repository.UpdateCurrentEntity(workOrder);
                SetupResult.For(_target);

                notifier.Notify(_lastCreatedGeneralWorkOrder.OperatingCenter.Id,
                    RoleModules.FieldServicesWorkManagement,
                    WorkOrderDetailPresenter.SAMPLE_SITE_NOTIFICATION,
                    _lastCreatedGeneralWorkOrder, WorkOrderDetailPresenter.SAMPLE_SITE_NOTIFICATION, null);
            }

            using (_mocks.Playback())
            {
                _target.CallTrySendSampleSiteNotification(workOrder);
            }
        }

        #endregion

        #region View_DeleteClicked

        [TestMethod]
        public void TestView_DeleteClickedSetsCancelledFieldsIfViewPhaseIsGeneral()
        {
            // order to be sent in EventArgs
            var order = new TestWorkOrderBuilder().Build();
            
            order.WorkOrderCancellationReasonID = 1;
            var now = DateTime.Now;
            var employeeId = 123;
            order.SampleSite = new SampleSite {
                Premise = new Premise {
                    PremiseNumber = "9180617886"
                }
            };
            order.IsPremiseLinkedToSampleSite = false;

            SetupGeneralWorkOrderFrom271WorkOrder(order);

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.General);
                SetupResult.For(_repository.CurrentEntity).Return(order);
                SetupResult.For(_dateTimeProvider.GetCurrentDate()).Return(now);
                SetupResult.For(_securityService.GetEmployeeID())
                    .Return(employeeId);
                _repository.UpdateCurrentEntity(order);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                new EventRaiser((IMockedObject)_view, "DeleteClicked").Raise(
                    null,
                    new EntityEventArgs<WorkOrder>(order));

                Assert.AreEqual(employeeId, order.CancelledByID);
                Assert.AreEqual(now, order.CancelledAt);
            }
            _iWorkOrdersWorkOrderRepository.Verify(x => x.UpdateSAPWorkOrder(It.IsAny<WorkOrder>(), It.IsAny<MapCall.Common.Model.Entities.WorkOrder>()), Times.Once);

        }

        #endregion
    }

    internal class TestWorkOrderDetailPresenterBuilder : TestDataBuilder<TestWorkOrderDetailPresenter>
    {
        #region Private Members

        private readonly IWorkOrderDetailView _view;
        private IRepository<WorkOrder> _repository;

        #endregion

        #region Constructors

        private TestWorkOrderDetailPresenterBuilder()
        {
        }

        public TestWorkOrderDetailPresenterBuilder(IWorkOrderDetailView view)
        {
            _view = view;
        }

        #endregion

        #region Exposed Methods

        public override TestWorkOrderDetailPresenter Build()
        {
            var presenter = new TestWorkOrderDetailPresenter(_view);
            if (_repository != null)
                presenter.Repository = _repository;
            return presenter;
        }

        public TestWorkOrderDetailPresenterBuilder WithRepository(IRepository<WorkOrder> repository)
        {
            _repository = repository;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderDetailPresenter : WorkOrderDetailPresenter
    {
        #region Properties

        public Action<WorkOrder> MockTryApprovalNotification;
        public Action<WorkOrder> MockTrySendSampleSiteNotification;
        public Action<MapCall.Common.Model.Entities.WorkOrder, WorkOrder> MockTrySendAcousticMonitoringNotification;

        #endregion

        #region Constructors

        public TestWorkOrderDetailPresenter(IWorkOrderDetailView view)
            : base(view)
        {
        }

        #endregion

        #region Exposed Methods

        public void CallTryApprovalNotification(WorkOrder order)
        {
            TryApprovalNotification(order);
        }

        public void CallTryCompletedNotification(WorkOrder order)
        {
            TryCompletedNotification(order);
        }

        public void CallTrySendSampleSiteNotification(WorkOrder order)
        {
            TrySendSampleSiteNotification(order);
        }

        public void CallTrySendAcousticMonitoringNotification(MapCall.Common.Model.Entities.WorkOrder order, WorkOrder entity)
        {
            TrySendAcousticMonitoringNotification(order, entity);
        }

    #endregion

    #region Private Methods

    protected override void TryApprovalNotification(WorkOrder order)
        {
            if (MockTryApprovalNotification != null)
            {
                MockTryApprovalNotification(order);
            }
            else
            {
                base.TryApprovalNotification(order);
            }
        }

    protected override void TrySendSampleSiteNotification(WorkOrder order)
    {
        if (MockTrySendSampleSiteNotification != null)
        {
            MockTrySendSampleSiteNotification(order);
        }
        else
        {
            base.TrySendSampleSiteNotification(order);
        }
    }

    protected override void TrySendAcousticMonitoringNotification(MapCall.Common.Model.Entities.WorkOrder order, WorkOrder entity)
    {
        if (MockTrySendAcousticMonitoringNotification != null)
        {
            MockTrySendAcousticMonitoringNotification(order, entity);
        }
        else
        {
            base.TrySendAcousticMonitoringNotification(order, entity);
        }
    }

        #endregion
    }
}
