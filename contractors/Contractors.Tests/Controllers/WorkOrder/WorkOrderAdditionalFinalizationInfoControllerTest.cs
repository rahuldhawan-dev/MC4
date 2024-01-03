using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contractors.Controllers.WorkOrder;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.Utilities;
using Moq;
using StructureMap;
using IWorkDescriptionRepository = Contractors.Data.Models.Repositories.IWorkDescriptionRepository;
using IWorkOrderRepository = Contractors.Data.Models.Repositories.IWorkOrderRepository;
using WorkDescriptionRepository = Contractors.Data.Models.Repositories.WorkDescriptionRepository;
using WorkOrderRepository = Contractors.Data.Models.Repositories.WorkOrderRepository;
using MapCall.Common.Utility.Notifications;
using MMSINC.Data.NHibernate;

namespace Contractors.Tests.Controllers.WorkOrder
{
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    [TestClass]
    public class WorkOrderAdditionalFinalizationInfoControllerTest : ContractorControllerTestBase<WorkOrderAdditionalFinalizationInfoController, MapCall.Common.Model.Entities.WorkOrder, IRepository<MapCall.Common.Model.Entities.WorkOrder>>
    {
        #region Private Members

        private DateTimeProvider _dateTimeProvider;
        private Mock<INotificationService> _noteServ;
        private Mock<IWorkOrderRepository> _mockedRepository;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _noteServ = new Mock<INotificationService>();
            _container.Inject(_noteServ.Object);
            _mockedRepository = new Mock<IWorkOrderRepository>();
            _container.Inject(_mockedRepository.Object);
        }

        protected override void InitializeObjectFactory(
            ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
            e.For<IWorkOrderRepository>().Use<WorkOrderRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IWorkDescriptionRepository>()
             .Use<WorkDescriptionRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () =>
                GetFactory<FinalizationWorkOrderFactory>().Create(new {
                    AssignedContractor = _currentUser.Contractor
                });
            options.ExpectedEditViewName = "_Edit";
        }

        #endregion

        #region Authorization   

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/WorkOrderAdditionalFinalizationInfo/Update");
            });
        }

        #endregion

        #region Update

        [TestMethod]
        public void TestUpdateReturnsEditViewWithModelLoadedAndViewDataPopulatedIfModelStateIsNotValid()
        {
            var order = GetFactory<FinalizationWorkOrderFactory>().Create(new
            {
                AssignedContractor = _currentUser.Contractor
            });
            var model = _container.GetInstance<WorkOrderAdditionalFinalizationInfo>();
            model.Map(order);
            var expectedDescriptions = _container
                .GetInstance<IWorkDescriptionRepository>()
                .GetByAssetTypeId(order.AssetType.Id).ToArray();

            _target.ModelState.AddModelError("nope", "nuh uh");

            var result = _target.Update(model) as PartialViewResult;

            Assert.AreEqual("_Edit", result.ViewName);
            Assert.AreSame(model, result.Model);
            MyAssert.AreEqual(expectedDescriptions[0].Id.ToString(),
                ((IEnumerable<SelectListItem>)
                    result.ViewData["WorkDescription"]).ToArray()[0].Value);
        }

        [TestMethod]
        public override void TestUpdateRedirectsToShowActionAfterSuccessfulSave()
        {
            // noop override: returns empty json result.
        }

        [TestMethod]
        public void TestUpdateUpdatesTheNecessaryFieldsAndReturnsNothingIfModelStateIsValid()
        {
            var order = new MapCall.Common.Model.Entities.WorkOrder() {
                OperatingCenter = new OperatingCenter { SAPEnabled = false },
                Id = 666,
                WorkDescription = new WorkDescription() { Id = 666 }
            };
            var model = new WorkOrderAdditionalFinalizationInfo(_container) {
                Id = 666,
                AppendNotes = "these are new notes",
                DistanceFromCrossStreet = 10,
                LostWater = 20,
                WorkDescription = 666
            };
            _mockedRepository.Setup(x => x.Find(666)).Returns(order);
            _mockedRepository.Setup(x => x.Save(It.IsAny<MapCall.Common.Model.Entities.WorkOrder>())).Returns(order);
            _target = _container.GetInstance<WorkOrderAdditionalFinalizationInfoController>();

            var result = _target.Update(model) as JsonResult;

            _mockedRepository.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.Data);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationWhenSewerOverflowChanged()
        {
            var workDescription1 = GetEntityFactory<WorkDescription>().Create(new {
                Id = (int)WorkDescription.Indices.SEWER_MAIN_CLEANING
            });
            var workDescription2 = GetEntityFactory<WorkDescription>().Create(new {
                Id = (int)WorkDescription.Indices.SEWER_MAIN_OVERFLOW
            });
            var town = GetFactory<TownFactory>().Create();
            var order = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                Town = town,
                WorkDescription = workDescription1
            });
            var model = new WorkOrderAdditionalFinalizationInfo(_container) {
                Id = order.Id,
                WorkDescription = workDescription2.Id
            };
            _mockedRepository.Setup(x => x.Find(model.Id)).Returns(order);
            _mockedRepository.Setup(x => x.Save(It.IsAny<MapCall.Common.Model.Entities.WorkOrder>())).Returns(order);

            _target.Update(model);

            _noteServ.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }
        
        [TestMethod]
        public void TestUpdateSendsNotificationWhenPitcherFilterNotDelivered()
        {
            var fact = GetEntityFactory<WorkDescription>();

            var workDescription = GetEntityFactory<WorkDescription>().Create(new {
                Id = (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL
            });
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create(new {Description = ServiceMaterial.Descriptions.GALVANIZED});
            var state = GetFactory<StateFactory>().Create();
            var town = GetFactory<TownFactory>().Create( new {State = state});
            var order = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                Town = town,
                WorkDescription = workDescription,
                HasPitcherFilterBeenProvidedToCustomer = false,
                PreviousServiceLineMaterial = serviceMaterial
            });
            var model = new WorkOrderAdditionalFinalizationInfo(_container) {
                Id = order.Id,
                WorkDescription = workDescription.Id
            };
            _mockedRepository.Setup(x => x.Find(model.Id)).Returns(order);
            _mockedRepository.Setup(x => x.Save(It.IsAny<MapCall.Common.Model.Entities.WorkOrder>())).Returns(order);

            _target.Update(model);

            _noteServ.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }
        
        [TestMethod]
        public void TestUpdateDoesNotSendNotificationWhenPitcherFilterDelivered()
        {
            var workDescription = GetEntityFactory<WorkDescription>().Create(new {
                Id = (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL
            });
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create(new {Description = ServiceMaterial.Descriptions.LEAD});
            var state = GetFactory<StateFactory>().Create();
            var town = GetFactory<TownFactory>().Create( new {State = state});
            var order = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                Town = town,
                WorkDescription = workDescription,
                HasPitcherFilterBeenProvidedToCustomer = true,
                PreviousServiceLineMaterial = serviceMaterial
            });
            var model = new WorkOrderAdditionalFinalizationInfo(_container) {
                Id = order.Id,
                WorkDescription = workDescription.Id
            };
            
            NotifierArgs resultArgs = null;
            
            _mockedRepository.Setup(x => x.Find(model.Id)).Returns(order);
            _mockedRepository.Setup(x => x.Save(It.IsAny<MapCall.Common.Model.Entities.WorkOrder>())).Returns(order);
            _noteServ.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            _target.Update(model);
            _noteServ.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never());

            Assert.IsNull(resultArgs);
        }

        #endregion
    }
}
