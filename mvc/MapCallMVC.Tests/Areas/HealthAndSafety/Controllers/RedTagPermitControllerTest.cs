using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels.RedTagPermits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Controllers
{
    [TestClass]
    public class RedTagPermitControllerTest : MapCallMvcControllerTestBase<RedTagPermitController, RedTagPermit>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var facility = GetEntityFactory<Facility>().Create();
                var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {
                    State = GetEntityFactory<State>().Create()
                });

                var equipment = GetEntityFactory<Equipment>().Create(new {
                    OperatingCenter = operatingCenter,
                    Facility = facility,
                    EquipmentType = GetFactory<EquipmentTypeFireSuppressionFactory>().Create()
                });

                var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create(new {
                    Equipment = equipment,
                    OperatingCenter = operatingCenter,
                    Facility = facility
                });

                return GetEntityFactory<RedTagPermit>().Create(new {
                    PersonResponsible = GetEntityFactory<Employee>().Create(),
                    ProductionWorkOrder = productionWorkOrder,
                    Equipment = equipment,
                    HotWorkProhibited = true
                });
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditRedTagPermitViewModel)vm;
                model.EquipmentRestoredOn = DateTime.Now;
                model.EquipmentRestoredOnChangeReason = "Yeah";
            };
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<INotificationService>().Use((_notificationService = new Mock<INotificationService>()).Object);
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const string urlPathPart = "~/HealthAndSafety/RedTagPermit";

                a.RequiresRole($"{urlPathPart}/Search/", RoleModules.ProductionWorkManagement);
                a.RequiresRole($"{urlPathPart}/Show/", RoleModules.ProductionWorkManagement);
                a.RequiresRole($"{urlPathPart}/Index/", RoleModules.ProductionWorkManagement);
                a.RequiresRole($"{urlPathPart}/New/", RoleModules.ProductionWorkManagement, RoleActions.Add);
                a.RequiresRole($"{urlPathPart}/Create/", RoleModules.ProductionWorkManagement, RoleActions.Add);
                a.RequiresRole($"{urlPathPart}/Edit/", RoleModules.ProductionWorkManagement, RoleActions.Edit);
                a.RequiresRole($"{urlPathPart}/Update/", RoleModules.ProductionWorkManagement, RoleActions.Edit);
            });
        }

        #endregion

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // other tests needed to cover this.
        }

        [TestMethod]
        public void TestNewReturnsViewWithCreateViewModelWhenParametersAreValid()
        {
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var equipment = GetEntityFactory<Equipment>().Create();

            var newViewModel = _viewModelFactory.BuildWithOverrides<NewRedTagPermitViewModel>(new {
                ProductionWorkOrder = productionWorkOrder.Id,
                OperatingCenter = operatingCenter.Id,
                Equipment = equipment.Id
            });

            var actionResult = (ViewResult)_target.New(newViewModel);
            var viewModel = (CreateRedTagPermitViewModel)actionResult.Model;

            Assert.AreEqual(productionWorkOrder.Id, viewModel.ProductionWorkOrder);
            Assert.AreEqual(operatingCenter.Id, viewModel.OperatingCenter);
            Assert.AreEqual(equipment.Id, viewModel.Equipment);

            MvcAssert.IsViewNamed(actionResult, "New");
        }

        [TestMethod]
        public void TestCreateSendsNotification()
        {
            var notificationPurpose = RedTagPermitController.CREATE_NOTIFICATION;
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create();
            // ARRANGE
            var existing = GetEntityFactory<RedTagPermit>().Create(new {
                OperatingCenter = operatingCenter,
                ProductionWorkOrder = productionWorkOrder,
                SmokingProhibited = true
            });
            var entity = _viewModelFactory.Build<CreateRedTagPermitViewModel, RedTagPermit>(existing);

            // ACT
            var result = _target.Create(entity);
            
            // ASSERT
            _notificationService.Verify(
                x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose)), Times.Once);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            const int numberOfTurnsToClose = 10;

            var entity = GetEntityFactory<RedTagPermit>().Create();

            _target.Update(_viewModelFactory.BuildWithOverrides<EditRedTagPermitViewModel, RedTagPermit>(entity, new {
                NumberOfTurnsToClose = numberOfTurnsToClose,
                EquipmentRestoredOn = DateTime.Now
            }));

            var updatedEntity = Session.Get<RedTagPermit>(entity.Id);

            Assert.AreEqual(numberOfTurnsToClose, updatedEntity.NumberOfTurnsToClose);
        }

        [TestMethod]
        public void TestUpdateSendsNotification()
        {
            var notificationPurpose = RedTagPermitController.EDIT_NOTIFICATION;
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create();
            
            var existing = GetEntityFactory<RedTagPermit>().Create(new {
                OperatingCenter = operatingCenter,
                ProductionWorkOrder = productionWorkOrder,
                EquipmentRestoredOn = DateTime.Now,
                SmokingProhibited = true
            });
            var entity = _viewModelFactory.Build<EditRedTagPermitViewModel, RedTagPermit>(existing);

            _target.Update(entity);
            
            _notificationService.Verify(
                x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose)), Times.Once);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationDoesNotSendWhenEquipmentRestoredOnIsNull()
        {
            var notificationPurpose = RedTagPermitController.EDIT_NOTIFICATION;
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create();
            
            var existing = GetEntityFactory<RedTagPermit>().Create(new {
                OperatingCenter = operatingCenter,
                ProductionWorkOrder = productionWorkOrder,
                SmokingProhibited = true
            });
            existing.EquipmentRestoredOn = null;
            var entity = _viewModelFactory.Build<EditRedTagPermitViewModel, RedTagPermit>(existing);
            
            _target.Update(entity);
            
            _notificationService.Verify(
                x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose)), Times.Never);
        }

        #endregion

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var state = GetEntityFactory<State>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new { State = state });
            var facility = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter });
            var equipment = GetEntityFactory<Equipment>().Create(new { Facility = facility });
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create(new {
                Facility = facility,
                Equipment = equipment,
                OperatingCenter = operatingCenter
            });

            var personResponsible = GetEntityFactory<Employee>().Create();
            
            GetEntityFactory<RedTagPermit>().Create(new {
                Equipment = equipment,
                PersonResponsible = personResponsible,
                ProductionWorkOrder = productionWorkOrder,
                NumberOfTurnsToClose = 5
            });

            GetEntityFactory<RedTagPermit>().Create(new {
                Equipment = equipment,
                PersonResponsible = personResponsible,
                ProductionWorkOrder = productionWorkOrder,
                NumberOfTurnsToClose = 6
            });

            var search = new SearchRedTagPermitViewModel {
                State = state.Id,
                OperatingCenter = operatingCenter.Id,
                Facility = facility.Id,
                Equipment = equipment.Id
            };

            _target.Index(search);
            Assert.AreEqual(2, search.Count);
        }

        #endregion

        #region Search

        [TestMethod]
        public void TestIndexXlsExportsExcel()
        {
            const int expectedResultsCount = 4;

            var state = GetEntityFactory<State>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new { State = state });
            var companySubsidiary = GetEntityFactory<CompanySubsidiary>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {
                OperatingCenter = operatingCenter, 
                CompanySubsidiary = companySubsidiary
            });
            var equipment = GetEntityFactory<Equipment>().Create(new { Facility = facility });
            var personResponsible = GetEntityFactory<Employee>().Create();
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create(new {
                Facility = facility,
                Equipment = equipment,
                OperatingCenter = operatingCenter
            });

            var redTagPermitFactory = GetFactory<RedTagPermitFactory>();

            var forms = new List<RedTagPermit>(expectedResultsCount);
            for (var a = 0; a < expectedResultsCount; a++)
            {
                forms.Add(redTagPermitFactory.Create(new {
                    Equipment = equipment,
                    ProductionWorkOrder = productionWorkOrder,
                    PersonResponsible = personResponsible,
                    NumberOfTurnsToClose = 5
                }));
            }

            var searchViewModel = new SearchRedTagPermitViewModel {
                State = state.Id,
                OperatingCenter = operatingCenter.Id,
                Facility = facility.Id,
                Equipment = equipment.Id
            };

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var searchViewResult = _target.Index(searchViewModel) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, searchViewResult, true))
            {
                for (var rowIndex = 0; rowIndex < forms.Count(); rowIndex++)
                {
                    var form = forms[rowIndex];

                    helper.AreEqual(form.Id, "Id", rowIndex);
                    helper.AreEqual(form.NumberOfTurnsToClose, nameof(form.NumberOfTurnsToClose), rowIndex);
                }

                Assert.AreEqual(expectedResultsCount, helper.GetRowCount(helper.GetSheetNames().First()));
            }
        }

        #endregion

        #region Pdf

        [TestMethod]
        public void TestShowRespondsToPdf()
        {
            var entity = GetEntityFactory<RedTagPermit>().Create();
            InitializeControllerAndRequest("~/HealthAndSafety/RedTagPermit/Show/" + entity.Id + ".pdf");
            var result = _target.Show(entity.Id);
            Assert.IsInstanceOfType(result, typeof(PdfResult));
        }

        #endregion
    }
}