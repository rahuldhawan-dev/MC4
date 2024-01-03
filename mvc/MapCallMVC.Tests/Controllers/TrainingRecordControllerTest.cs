using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;
using ControllerBase = MMSINC.Controllers.ControllerBase;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class TrainingRecordControllerTest : MapCallMvcControllerTestBase<TrainingRecordController, TrainingRecord>
    {
        #region Init/Cleanup

        private Mock<INotificationService> _notifier;

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _notifier = e.For<INotificationService>().Mock();
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<ITrainingModuleRepository>().Use<TrainingModuleRepository>();
            e.For<ITrainingRecordRepository>().Use<TrainingRecordRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                return GetEntityFactory<TrainingRecord>().Create(new {
                    ScheduledDate = DateTime.Now,
                    MaximumClassSize = 10,
                });
            };
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role_module = RoleModules.OperationsTrainingRecords;
            Authorization.Assert(a => {
                a.RequiresRole("~/TrainingRecord/Search", role_module, RoleActions.Read);
                a.RequiresRole("~/TrainingRecord/Show", role_module, RoleActions.Read);
                a.RequiresRole("~/TrainingRecord/Index", role_module, RoleActions.Read);
                a.RequiresRole("~/TrainingRecord/Edit", role_module, RoleActions.Edit);
                a.RequiresRole("~/TrainingRecord/Update", role_module, RoleActions.Edit);
                a.RequiresRole("~/TrainingRecord/Create", role_module, RoleActions.Add);
                a.RequiresRole("~/TrainingRecord/New", role_module, RoleActions.Add);
                a.RequiresRole("~/TrainingRecord/Destroy", role_module, RoleActions.Delete);
                a.RequiresRole("~/TrainingRecord/Finalize", role_module, RoleActions.Edit);
                a.RequiresRole("~/TrainingRecord/AddTrainingSession", role_module, RoleActions.Edit);
                a.RequiresRole("~/TrainingRecord/RemoveTrainingSession", role_module, RoleActions.Edit);
            });
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowDisplaysNotificationIfTrainingModuleTrainingHoursHaveNotBeenMet()
        {
            var trainingModule = GetEntityFactory<TrainingModule>().Create(new { TotalHours = (float)40 });
            var entity = GetEntityFactory<TrainingRecord>().Create(new { TrainingModule = trainingModule });
            
            var result = _target.Show(entity.Id) as ViewResult;

            Assert.AreEqual(TrainingRecordController.TRAINING_SESSIONS_INCOMPLETE, ((List<string>)result.TempData[ControllerBase.NOTIFICATION_MESSAGE_KEY]).Single());
        }

        [TestMethod]
        public void TestShowRespondsToPdfAndSetsEmployeeId()
        {
            var entity = GetEntityFactory<TrainingRecord>().Create();
            var dataType = GetEntityFactory<DataType>().Create(new {
                Name = TrainingRecord.DataTypeNames.EMPLOYEES_ATTENDED, TableName = TrainingRecordMap.TABLE_NAME
            });
            var employee = GetEntityFactory<Employee>().Create(new { TLicense = "Foo", IsActive = true });
            var employeeLink = GetEntityFactory<EmployeeLink>().Create(new { Employee = employee, DataType = dataType, LinkedId = entity.Id });
            Session.Flush();
            Session.Clear();

            InitializeControllerAndRequest("~/TrainingRecord/Show/" + entity.Id + ".pdf?employeeId=" + employee.Id);

            var result = _target.Show(entity.Id, employee.Id);

            Assert.IsInstanceOfType(result, typeof(PdfResult));
        }
        
        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<TrainingRecord>().Create(new { CourseLocation = "location 0"});
            var entity1 = GetEntityFactory<TrainingRecord>().Create(new { CourseLocation = "location 1" });
            var search = new SearchTrainingRecord();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.CourseLocation, "Class Location Details");
                helper.AreEqual(entity1.CourseLocation, "Class Location Details", 1);
            }
        }

        [TestMethod]
        public void TestIndexReturnsFragmentWithProperViewDataAndResults()
        {
            var entity0 = GetEntityFactory<TrainingRecord>().Create(new { CourseLocation = "location 0" });
            var entity1 = GetEntityFactory<TrainingRecord>().Create(new { CourseLocation = "location 1" });
            var entity2 = GetEntityFactory<TrainingRecord>().Create(new { CourseLocation = "location 2" });
            // sneaky way to double check/test the Ids search property
            var search = new SearchTrainingRecord{ Ids = new[] {entity0.Id, entity1.Id }}; 
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.FRAGMENT;

            var result = _target.Index(search) as PartialViewResult;
            var resultModel = ((SearchTrainingRecord)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "_AjaxIndexLimited");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreSame(entity0, resultModel[0]);
            Assert.AreSame(entity1, resultModel[1]);

        }

        #endregion

        #region Search

        [TestMethod]
        public void TestSearchingForStartOrEndWorksCorrectly()
        {
            // Start and End are used to detect if a TrainingRecord has 
            // either a MinSessionDate *or* MaxSessionDate that falls in 
            // the the search range.
            var expectedStart = new DateTime(1984, 4, 24);
            var expectedEnd = new DateTime(1984, 4, 30);

            Func<DateTime?, DateTime?, TrainingRecord> createRecord = (min, max) => {
                var training = GetEntityFactory<TrainingRecord>().Create();
                var sess = GetEntityFactory<TrainingSession>().Create(new {
                    TrainingRecord = training, 
                    StartDateTime = min,
                    EndDateTime = max
                });
                
                return training;
            };

            var goodMinRecord = createRecord(expectedStart, null);
            var goodMaxRecord = createRecord(null, expectedEnd);
            var badMinRecord = createRecord(expectedStart.AddDays(-1), null);
            var badMaxRecord = createRecord(null, expectedEnd.AddDays(1));
            var goodEverythingInRange = createRecord(expectedStart.AddDays(1), expectedEnd.AddDays(-1));

            var model = new SearchTrainingRecord();
            model.Start = expectedStart;
            model.End = expectedEnd;

            _target.Index(model);

            Assert.AreEqual(3, model.Count);
            Assert.IsTrue(model.Results.Contains(goodMinRecord));
            Assert.IsTrue(model.Results.Contains(goodMaxRecord));
            Assert.IsTrue(model.Results.Contains(goodEverythingInRange));
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateSendsNotificationOnCreate()
        {
            var opCntr = GetEntityFactory<OperatingCenter>().Create();
            var classLocation = GetEntityFactory<ClassLocation>().Create(new { OperatingCenter = opCntr});
            var ent = GetEntityFactory<TrainingRecord>().Create(new { MaximumClassSize=20, ScheduledDate = DateTime.Now, ClassLocation = classLocation });
            var model = _viewModelFactory.Build<CreateTrainingRecord, TrainingRecord>( ent);
            model.Id = 0;
            ValidationAssert.ModelStateIsValid(model);
            NotifierArgs resultsArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultsArgs = x);

            var result = _target.Create(model);
            var entity = Repository.Find(model.Id);

            Assert.AreSame(entity, resultsArgs.Data);
            Assert.AreEqual(classLocation.OperatingCenter.Id, resultsArgs.OperatingCenterId);
            Assert.AreEqual("http://localhost/TrainingRecord/Show/" + entity.Id, entity.RecordUrl);

        }
        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<TrainingRecord>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditTrainingRecord, TrainingRecord>(eq, new {
                CourseLocation = expected
            }));

            Assert.AreEqual(expected, Session.Get<TrainingRecord>(eq.Id).CourseLocation);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationIfTrainingRecordIsBeingCanceled()
        {
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var dataType = GetEntityFactory<DataType>().Create(new {
                TableName = TrainingRecordMap.TABLE_NAME,
                Name = TrainingRecord.DataTypeNames.EMPLOYEES_SCHEDULED
            });
            var employee = GetEntityFactory<Employee>().Create();
            var tr = GetEntityFactory<TrainingRecord>().Create();
            var el = GetEntityFactory<EmployeeLink>().Create(new {
                Employee = employee,
                LinkedId = tr.Id,
                DataType = dataType
            });
            Session.Clear();
            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditTrainingRecord, TrainingRecord>(tr, new {
                Canceled = true
            }));

            Assert.IsNotNull(resultArgs, "notifier may not have been called");
            Assert.AreEqual(resultArgs.Address, employee.EmailAddress);
            Assert.AreEqual(TrainingRecordController.ROLE_MODULE, resultArgs.Module);
            Assert.AreEqual(TrainingRecordController.CANCELED_NOTIFICATION, resultArgs.Purpose);
            
        }

        [TestMethod]
        public void TestUpdateSendsNotificationToSupervisorIfTrainingRecordIsBeingCanceledAndEmployeeHasSupervisorWithEmailAddress()
        {
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var dataType = GetEntityFactory<DataType>().Create(new
            {
                TableName = TrainingRecordMap.TABLE_NAME,
                Name = TrainingRecord.DataTypeNames.EMPLOYEES_SCHEDULED
            });
            var supervisor = GetEntityFactory<Employee>().Create();
            var employee = GetEntityFactory<Employee>().Create(new { ReportsTo = supervisor });
            var tr = GetEntityFactory<TrainingRecord>().Create();
            var el = GetEntityFactory<EmployeeLink>().Create(new
            {
                Employee = employee,
                LinkedId = tr.Id,
                DataType = dataType
            });
            Session.Clear();
            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditTrainingRecord, TrainingRecord>(tr, new {
                Canceled = true
            }));

            Assert.IsNotNull(resultArgs, "notifier may not have been called");
            // these two lines are fragile/weak. if the position of these move in the
            // updatenotification method this could break.
            Assert.AreNotEqual(resultArgs.Address, employee.EmailAddress);
            Assert.AreEqual(resultArgs.Address, supervisor.EmailAddress);
            Assert.AreEqual(TrainingRecordController.ROLE_MODULE, resultArgs.Module);
            Assert.AreEqual(TrainingRecordController.CANCELED_NOTIFICATION, resultArgs.Purpose);
        }

        [TestMethod]
        public void TestUpdateDoesNotSendNotificationIfTrainingRecordWasAlreadyCanceled()
        {
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var tr = GetEntityFactory<TrainingRecord>().Create(new { Canceled = true });

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditTrainingRecord, TrainingRecord>(tr, new {
                Canceled = true
            }));

            Assert.IsNull(resultArgs, "notifier was called when it should not have been");
        }

        #endregion
    }
}