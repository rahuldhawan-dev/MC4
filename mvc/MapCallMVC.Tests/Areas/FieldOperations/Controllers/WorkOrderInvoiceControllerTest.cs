using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using Moq;
using Permits.Data.Client.Repositories;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class WorkOrderInvoiceControllerTest : MapCallMvcControllerTestBase<WorkOrderInvoiceController, WorkOrderInvoice>
    {
        #region Fields

        private Mock<INotificationService> _notifier;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotificationService>();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _container.Inject(_notifier.Object);
            _container.Inject(_dateTimeProvider.Object);
            _container.Inject(new Mock<IPermitsRepositoryFactory>().Object);
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = WorkOrderInvoiceController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/WorkOrderInvoice/Search/", role);
                a.RequiresRole("~/FieldOperations/WorkOrderInvoice/Show/", role);
                a.RequiresRole("~/FieldOperations/WorkOrderInvoice/Index/", role);
                a.RequiresRole("~/FieldOperations/WorkOrderInvoice/New/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/WorkOrderInvoice/Create/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/WorkOrderInvoice/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/WorkOrderInvoice/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/WorkOrderInvoice/AddScheduleOfValue/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/WorkOrderInvoice/RemoveScheduleOfValue/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/WorkOrderInvoice/Destroy/", role, RoleActions.Delete);
			});
		}				

        #endregion

        #region Show

        [TestMethod]
        public void TestShowDisplaysNotificationIfMatchingScheduleOfValuesNotFound()
        {
            var workOrder = GetEntityFactory<WorkOrder>().Create();
            var entity = GetEntityFactory<WorkOrderInvoice>().Create(new { WorkOrder = workOrder});
            
            var result = _target.Show(entity.Id) as ViewResult;

            Assert.AreEqual(WorkOrderInvoiceController.DOES_NOT_MATCH_WORKORDER, ((List<string>)result.TempData[MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY]).Single());
        }

        [TestMethod]
        public void TestShowDoesNotDisplayNotificationIfMatchingScheduleOfValuesNotFound()
        {
            var entity = GetEntityFactory<WorkOrderInvoice>().Create();

            var result = _target.Show(entity.Id) as ViewResult;

            Assert.IsNull(result.TempData[MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY]);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var date = new DateTime(2016, 10, 12, 12, 0, 0);
            var entity0 = GetEntityFactory<WorkOrderInvoice>().Create(new { InvoiceDate = date});
            var entity1 = GetEntityFactory<WorkOrderInvoice>().Create(new { InvoiceDate = date});
            var search = new SearchWorkOrderInvoice();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(date.ToOADate(), "InvoiceDate");   //FRAGILE, EXCEL LIBRARY DEPENDENT
                helper.AreEqual(date.ToOADate(), "InvoiceDate", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<WorkOrderInvoice>().Create();
            var expected = DateTime.Now;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditWorkOrderInvoice, WorkOrderInvoice>(eq, new {
                InvoiceDate = expected
            }));

            Assert.AreEqual(expected, Session.Get<WorkOrderInvoice>(eq.Id).InvoiceDate);
        }

        [TestMethod]
        public void TestSendsNotificationIfSubmitted()
        {
            var expectedBytes = new byte[] {1, 2, 3};
            var pdfRenderer = new Mock<IHtmlToPdfConverter>();
            _container.Inject(pdfRenderer.Object);
            pdfRenderer.Setup(x => x.RenderHtmlToPdfBytes(It.IsAny<string>())).Returns(expectedBytes);
            ((FakeViewEngine)Application.ViewEngine).Views["Pdf"] = new Mock<IView>().Object;
            var workOrder = GetEntityFactory<WorkOrder>().Create();
            var invoice = GetEntityFactory<WorkOrderInvoice>().Create(new { WorkOrder = workOrder});
            var model = _viewModelFactory.BuildWithOverrides<EditWorkOrderInvoice, WorkOrderInvoice>(invoice, new { SubmittedDate = DateTime.Now });
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.Update(model);

            MvcAssert.RedirectsToRoute(result, new { action = "Show", controller = "WorkOrderInvoice", id = invoice.Id});
            Assert.AreEqual(invoice.WorkOrder.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.FieldServicesWorkOrderInvoice, resultArgs.Module);
            Assert.AreEqual(WorkOrderInvoiceController.SUBMITTED_NOTIFICATION_PURPOSE, resultArgs.Purpose);
            Assert.AreSame(invoice, resultArgs.Data);
            Assert.IsNull(resultArgs.Subject);
            Assert.IsTrue(expectedBytes.ByteArrayEquals(resultArgs.Attachments.Single().BinaryData));
        }

        [TestMethod]
        public void TestSendsNotificationIfCanceled()
        {
            var expectedBytes = new byte[] { 1, 2, 3 };
            var pdfRenderer = new Mock<IHtmlToPdfConverter>();
            _container.Inject(pdfRenderer.Object);
            pdfRenderer.Setup(x => x.RenderHtmlToPdfBytes(It.IsAny<string>())).Returns(expectedBytes);
            ((FakeViewEngine)Application.ViewEngine).Views["Pdf"] = new Mock<IView>().Object;
            var workOrder = GetEntityFactory<WorkOrder>().Create();
            var invoice = GetEntityFactory<WorkOrderInvoice>().Create(new { WorkOrder = workOrder });
            var model = _viewModelFactory.BuildWithOverrides<EditWorkOrderInvoice, WorkOrderInvoice>(invoice, new { CanceledDate = DateTime.Now });
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.Update(model);

            MvcAssert.RedirectsToRoute(result, new { action = "Show", controller = "WorkOrderInvoice", id = invoice.Id });
            Assert.AreEqual(invoice.WorkOrder.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.FieldServicesWorkOrderInvoice, resultArgs.Module);
            Assert.AreEqual(WorkOrderInvoiceController.CANCELED_NOTIFICATION_PURPOSE, resultArgs.Purpose);
            Assert.AreSame(invoice, resultArgs.Data);
            Assert.IsNull(resultArgs.Subject);
            Assert.IsTrue(expectedBytes.ByteArrayEquals(resultArgs.Attachments.Single().BinaryData));
        }

        #endregion

        #region Add/Remove Schedule Of Values

        [TestMethod]
        public void TestAddScheduleOfValueCreatesAndAddsScheduleOfValue()
        {
            var workOrderInvoice = GetEntityFactory<WorkOrderInvoice>().Create();
            var scheduleOfValue = GetEntityFactory<ScheduleOfValue>().Create();

            MyAssert.CausesIncrease(() => 
                _target.AddScheduleOfValue(new AddWorkOrderInvoiceScheduleOfValue(_container) { 
                   Id = workOrderInvoice.Id, 
                   Total = 5, 
                   ScheduleOfValue = scheduleOfValue.Id,
                   IncludeWithInvoice = true
                }),
                _container.GetInstance<RepositoryBase<WorkOrderInvoiceScheduleOfValue>>().GetAll().Count);
        }

        [TestMethod]
        public void TestRemoveScheduleOfValueRemovesScheduleOfValue()
        {
            var workOrderInvoice = GetEntityFactory<WorkOrderInvoice>().Create();
            var scheduleOfValue = GetEntityFactory<ScheduleOfValue>().Create();

            _target.AddScheduleOfValue(new AddWorkOrderInvoiceScheduleOfValue(_container) { 
                Id = workOrderInvoice.Id,
                Total = 5,
                ScheduleOfValue = scheduleOfValue.Id,
                IncludeWithInvoice = true
            });

            workOrderInvoice = Session.Load<WorkOrderInvoice>(workOrderInvoice.Id);

            MyAssert.CausesDecrease(() => _target.RemoveScheduleOfValue(
                _viewModelFactory.BuildWithOverrides<RemoveWorkOrderInvoiceScheduleOfValue, WorkOrderInvoice>(
                    workOrderInvoice, new {
                        ScheduleOfValueId = workOrderInvoice.ScheduleOfValues.First().Id
                    })), _container.GetInstance<RepositoryBase<WorkOrderInvoiceScheduleOfValue>>().GetAll().Count);

            workOrderInvoice = Session.Load<WorkOrderInvoice>(workOrderInvoice.Id);
            Assert.AreEqual(0, workOrderInvoice.WorkOrderInvoicesScheduleOfValues.Count);
        }

        #endregion
    }
}
