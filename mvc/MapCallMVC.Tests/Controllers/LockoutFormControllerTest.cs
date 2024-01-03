using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class LockoutFormControllerTest : MapCallMvcControllerTestBase<LockoutFormController, LockoutForm>
    {
        #region Constants

        public const RoleModules ROLE = LockoutFormController.ROLE;

        #endregion

        #region Fields

        private OperatingCenter _opCenter;
        private Mock<INotificationService> _notifier;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private DateTime _now;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            _opCenter = GetFactory<OperatingCenterFactory>().Create();
            var user = GetEntityFactory<User>().Create(new {DefaultOperatingCenter = _opCenter, FullName = "Full Name"});

            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations}),
                Module = GetFactory<ModuleFactory>().Create(new {Id = ROLE}),
                Action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read}),
                OperatingCenter = _opCenter,
                User = user
            });

            Session.Save(user);

            return user;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotificationService>();
            _container.Inject(_notifier.Object);
            _now = DateTime.Now;
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now);
            _container.Inject(_dateTimeProvider.Object);

            _target = Request.CreateAndInitializeController<LockoutFormController>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = LockoutFormController.ROLE;
                a.RequiresRole("~/HealthAndSafety/LockoutForm/Index/", module);
                a.RequiresRole("~/HealthAndSafety/LockoutForm/Search/", module);
                a.RequiresRole("~/HealthAndSafety/LockoutForm/Show/", module);
                a.RequiresRole("~/HealthAndSafety/LockoutForm/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/LockoutForm/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/LockoutForm/New/", module, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/LockoutForm/NewFromProductionWorkOrder/", module, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/LockoutForm/Create/", module, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/LockoutForm/Copy/", module, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/LockoutForm/Destroy/", module, RoleActions.Delete);
            });
        }
        
        #region Show

        [TestMethod]
        public void TestShowRespondsToFragment()
        {
            var good = GetEntityFactory<LockoutForm>().Create();
            InitializeControllerAndRequest("~/HealthAndSafety/LockoutForm/Show" + good.Id + ".frag");

            var result = (PartialViewResult)_target.Show(good.Id);

            MvcAssert.IsViewNamed(result, "_ShowPopup");
            Assert.AreSame(good, result.Model);
        }

        [TestMethod]
        public void TestShowRespondsToMap()
        {
            var good = GetEntityFactory<LockoutForm>().Create();
            var bad = GetEntityFactory<LockoutForm>().Create();
            InitializeControllerAndRequest("~/HealthAndSafety/LockoutForm/Show" + good.Id + ".map");

            var result = (MapResult)_target.Show(good.Id);
            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();

            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
            Assert.IsTrue(result.ModelStateIsValid);
        }

        [TestMethod]
        public void TestShowRespondsToPdf()
        {
            var entity = GetEntityFactory<LockoutForm>().Create();
            InitializeControllerAndRequest("~/HealthAndSafety/LockoutForm/Show/" + entity.Id + ".pdf");
            var result = _target.Show(entity.Id);
            Assert.IsInstanceOfType(result, typeof(PdfResult));
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexReturnsIndexViewWithArrayOfLockoutForms()
        {
            var eq1 = GetEntityFactory<LockoutForm>().Create();
            var eq2 = GetEntityFactory<LockoutForm>().Create();
            var search = new SearchLockoutForm();
            _target.ControllerContext = new ControllerContext();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchLockoutForm)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreSame(eq1, resultModel[0]);
            Assert.AreSame(eq2, resultModel[1]);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var lockoutQuestionCategory = GetEntityFactory<LockoutFormQuestionCategory>().Create(new {Description = "Foo"});
            var lockoutQuestion = GetEntityFactory<LockoutFormQuestion>().Create(new { Question = "Are you there?", IsActive = true, DisplayOrder = 1, Category = lockoutQuestionCategory });
            var entity0 = GetEntityFactory<LockoutForm>().Create(new { LocationOfLockoutNotes = "description 0" });
            var lockoutAnswer = GetEntityFactory<LockoutFormAnswer>().Create(new { LockoutForm = entity0, Answer = true, LockoutFormQuestion = lockoutQuestion, Comments = "Aw hell no" });
            var entity1 = GetEntityFactory<LockoutForm>().Create(new { LocationOfLockoutNotes = "description 1" });
            Session.Clear(); // clear things out so when it runs we get the fresh form entity with the answer
            var search = new SearchLockoutForm();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;
            
            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.LocationOfLockoutNotes, "LocationOfLockoutNotes");
                helper.AreEqual(entity1.LocationOfLockoutNotes, "LocationOfLockoutNotes", 1);
                Assert.AreEqual(lockoutQuestion.Question, helper.GetValue<string>(LockoutFormController.SHEET_QUESTIONS_ANSWERS,"Question",0));
                Assert.IsTrue(helper.GetValue<bool>(LockoutFormController.SHEET_QUESTIONS_ANSWERS,"Answer",0));
                Assert.AreEqual(lockoutAnswer.Comments, helper.GetValue<string>(LockoutFormController.SHEET_QUESTIONS_ANSWERS,"Comments",0));
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateSendsNotificationEmail()
        {
            _currentUser.IsAdmin = true;
            var ent = GetEntityFactory<LockoutForm>().Create();
            var model = _viewModelFactory.Build<CreateLockoutForm, LockoutForm>( ent);
            model.Id = 0;
            
            ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Create(model);
            var entity = Repository.Find(model.Id);

            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(LockoutFormController.ROLE, resultArgs.Module);
            Assert.AreEqual(LockoutFormController.CREATE_NOTIFICATION_PURPOSE, resultArgs.Purpose);
            Assert.IsNull(resultArgs.Subject);
        }

        [TestMethod]
        public void TestCreateUpdatesProductionWorkOrderPreReqSatisifedDate()
        {
            var preReq = GetFactory<HasLockoutRequirementProductionPrerequisiteFactory>().Create();
            var order = GetEntityFactory<ProductionWorkOrder>().Create();
            var workOrderPreReq = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>()
               .Create(new {ProductionWorkOrder = order, ProductionPrerequisite = preReq});
            var lockoutForm = GetEntityFactory<LockoutForm>().Create();
            var model = _viewModelFactory.Build<CreateLockoutForm, LockoutForm>(lockoutForm);

            order.ProductionWorkOrderProductionPrerequisites.Add(workOrderPreReq);
            model.ProductionWorkOrder = order.Id;

            Session.Clear();

            var result = _target.Create(model);
            

            //Need to requery to test that ProductionWorkOrderProductionPrerequisite.SatisifedOn is being saved from CreateLockOutForm.MapToEntity()
            var entity = Repository.Find(model.Id);
            var lockoutFormPwoPreReq =
                entity.ProductionWorkOrder.ProductionWorkOrderProductionPrerequisites.FirstOrDefault();

            Assert.AreEqual(lockoutFormPwoPreReq.SatisfiedOn, _dateTimeProvider.Object.GetCurrentDate());
            Assert.AreNotSame(workOrderPreReq, lockoutFormPwoPreReq);
        }

        [TestMethod]
        public void TestNewFromProductionWorkOrderLoadsNewFormWithValuesFromOrder()
        {
            _currentUser.IsAdmin = true;
            var order = GetEntityFactory<ProductionWorkOrder>().Create();
            var pwoe = GetEntityFactory<ProductionWorkOrderEquipment>().Create(new {
                ProductionWorkOrder = order,
                IsParent = true
            });
            order.Equipments.Add(pwoe);

            var result = (ViewResult)_target.NewFromProductionWorkOrder(order.Id);

            MyAssert.IsInstanceOfType<ActionResult>(result);
            MyAssert.IsInstanceOfType<CreateLockoutForm>(result.Model);
            var foo = (CreateLockoutForm)result.Model;

            Assert.AreEqual(order.Id, foo.ProductionWorkOrder);
            Assert.AreEqual(order.OperatingCenter.Id, foo.OperatingCenter);
            Assert.AreEqual(order.Facility.Id, foo.Facility);
            Assert.AreEqual(order.EquipmentType.Id, foo.EquipmentType);
            Assert.AreEqual(order.Equipment.Id, foo.Equipment);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public void TestUpdateDoesNotSendNotificationEmailWhenReturnedToServiceIsNotEntered()
        {
            _currentUser.IsAdmin = true;
            var ent = GetEntityFactory<LockoutForm>().Create(new { OutOfServiceDateTime = DateTime.Now.AddHours(-6) });
            var model = _viewModelFactory.Build<EditLockoutForm, LockoutForm>( ent);
            // verified this will totally fail if you set its returnedToService info. -arr

            ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never());
            Assert.IsNull(resultArgs);
        }

        [TestMethod]
        public void TestUpdateDoesNotSendNotificationEmailWhenReturnedToServiceIsWasAlreadyEntered()
        {
            _currentUser.IsAdmin = true;
            var employee = GetEntityFactory<Employee>().Create();
            var ent = GetEntityFactory<LockoutForm>().Create(new {
                OutOfServiceDateTime = DateTime.Now.AddHours(-6),
                ReturnedToServiceDateTime = DateTime.Now,
                ReturnToServiceAuthorizedEmployee = employee,
                ReturnedToServiceNotes = "these notes are required"
            });
            var model = _viewModelFactory.Build<EditLockoutForm, LockoutForm>( ent);
            
            ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never());
            Assert.IsNull(resultArgs);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenReturnedToServiceIsEntered  ()
        {
            _currentUser.IsAdmin = true;
            var employee = GetFactory<EmployeeFactory>().Create();

            var ent = GetEntityFactory<LockoutForm>().Create(new { OutOfServiceDateTime = DateTime.Now.AddHours(-6)});
            var model = _viewModelFactory.BuildWithOverrides<EditLockoutForm, LockoutForm>(ent, new {
                ReturnedToServiceDateTime = DateTime.Now,
                ReturnToServiceAuthorizedEmployee = employee.Id,
                ReturnedToServiceNotes = "these notes are required"
            });

            ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);
            var entity = Repository.Find(model.Id);

            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(LockoutFormController.ROLE, resultArgs.Module);
            Assert.AreEqual(LockoutFormController.UPDATE_NOTIFICATION_PURPOSE, resultArgs.Purpose);
            Assert.IsNull(resultArgs.Subject);
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<LockoutForm>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditLockoutForm, LockoutForm>(eq, new {
                AdditionalLockoutNotes = expected
            }));

            Assert.AreEqual(expected, Session.Get<LockoutForm>(eq.Id).AdditionalLockoutNotes);
        }

        #endregion

        #region Copy

        [TestMethod]
        public void TestCopyReturnsHttpNotFoundWhenEntityDoesNotExist()
        {
            Assert.IsNotNull(_target.Copy(0) as HttpNotFoundResult);
        }

        [TestMethod]
        public void TestCopyReturnsNewFormWithCreateLockoutFormModel()
        {
            var form = GetEntityFactory<LockoutForm>().Create();

            Assert.IsNotNull(form.LocationOfLockoutNotes); // sanity check

            var result = _target.Copy(form.Id) as ViewResult;
            var model = result.Model as CreateLockoutForm;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);

            Assert.IsNull(model.LocationOfLockoutNotes); // sanity check, nulled by model map method
        }

        #endregion

        #endregion
    }
}
