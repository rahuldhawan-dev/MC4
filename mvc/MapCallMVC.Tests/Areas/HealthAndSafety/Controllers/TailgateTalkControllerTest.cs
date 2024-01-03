using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controllers;
using MMSINC.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using MMSINC.Results;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Controllers
{
    [TestClass]
    public class TailgateTalkControllerTest : MapCallMvcControllerTestBase<TailgateTalkController, TailgateTalk, TailgateTalkRepository>
    {
        #region Roles

        public const RoleModules ROLE = RoleModules.OperationsHealthAndSafety;

        #endregion

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITailgateTalkRepository>().Use<TailgateTalkRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {

            // Needs to exist for Create tests.
            var dataType = GetFactory<DataTypeFactory>().Create(new {
                TableName = TailgateTalkMap.TABLE_NAME,
                Name = TailgateTalk.DATA_TYPE_NAME
            });
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.OperationsHealthAndSafety;
            Authorization.Assert(a => {
                a.RequiresRole("~/HealthAndSafety/TailgateTalk/Show/", module, RoleActions.Read);
                a.RequiresRole("~/HealthAndSafety/TailgateTalk/Search/", module, RoleActions.Read);
                a.RequiresRole("~/HealthAndSafety/TailgateTalk/Index/", module, RoleActions.Read);
                a.RequiresRole("~/HealthAndSafety/TailgateTalk/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/TailgateTalk/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/TailgateTalk/New/", module, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/TailgateTalk/Create/", module, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/TailgateTalk/Destroy/", module, RoleActions.Delete);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<TailgateTalk>().Create(new { TrainingTimeHours = 10m });
            var entity1 = GetEntityFactory<TailgateTalk>().Create(new { TrainingTimeHours = 11m });
            var search = new SearchTailgateTalk();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.TrainingTimeHours, "TrainingTimeHours");
                helper.AreEqual(entity1.TrainingTimeHours, "TrainingTimeHours", 1);
            }
        }

        [TestMethod]
        public void TestIndexJSONExportsJSON()
        {
            var now = DateTime.Now;
            var entity0 = GetEntityFactory<TailgateTalk>().Create(new {
                TrainingTimeHours = 10m,
                HeldOn = now.AddDays(-1)
            });
            var entity1 = GetEntityFactory<TailgateTalk>().Create(new {
                TrainingTimeHours = 11m,
                HeldOn = now.AddDays(-1)
            });
            var search = new SearchTailgateTalk {
                HeldOn = new DateRange {
                    Start = now.AddDays(-2),
                    End = now,
                    Operator = RangeOperator.Between
                }
            };
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.JSON;

            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result.Data);

            helper.AreEqual(entity0.Id, "Id");
            helper.AreEqual(entity1.Id, "Id", 1);
            helper.AreEqual(entity0.TrainingTimeHours, "TrainingTimeHours");
            helper.AreEqual(entity1.TrainingTimeHours, "TrainingTimeHours", 1);
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateCreatesNewTailgateTalkAndAddsPresentedByToLinkedEmployees()
        {
            var presentedBy = GetFactory<EmployeeFactory>().Create();

            var model = _viewModelFactory.Build<CreateTailgateTalk, TailgateTalk>(GetEntityFactory<TailgateTalk>().Build());
            model.PresentedBy = presentedBy.Id;

            Assert.AreEqual(0, model.Id);
            _target.Create(model);
            Assert.AreNotEqual(0, model.Id);

            Session.Clear();
            var entity = Session.Query<TailgateTalk>().Single(x => x.Id == model.Id);
            Assert.AreEqual(presentedBy.Id, entity.LinkedEmployees.Single().Employee.Id);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<TailgateTalk>().Create();
            var expected = 1.1m;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditTailgateTalk, TailgateTalk>(eq, new {
                TrainingTimeHours = expected
            }));

            Assert.AreEqual(expected, Session.Get<TailgateTalk>(eq.Id).TrainingTimeHours);
        }

        #endregion

        #region Lookup Data

        [TestMethod]
        public void TestSetLookupDataForNewOnlyIncludesActiveEmployees()
        {
            // arrange
            var activeEmployee =
                GetEntityFactory<Employee>().Create(new {
                    Status = GetFactory<ActiveEmployeeStatusFactory>().Create()
                });

            var inactiveEmployee =
                GetEntityFactory<Employee>().Create(new {
                    Status = GetFactory<InactiveEmployeeStatusFactory>().Create()
                });

            //act
            _target.SetLookupData(ControllerAction.New);
            var result = (IEnumerable<SelectListItem>)_target.ViewData["PresentedBy"];

            //assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void TestSetLookupDataForEditIncludesAllEmployees()
        {
            // arrange
            var inactiveEmployee = GetFactory<EmployeeFactory>().Create(new { IsActive = false });
            var activeEmployee = GetFactory<EmployeeFactory>().Create(new { IsActive = true });

            //act
            _target.SetLookupData(ControllerAction.Edit);
            var result = (IEnumerable<SelectListItem>)_target.ViewData["PresentedBy"];

            //assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void TestSetLookupDataForSearchIncludesAllEmployees()
        {
            // arrange
            var inactiveEmployee = GetFactory<EmployeeFactory>().Create(new { IsActive = false });
            var activeEmployee = GetFactory<EmployeeFactory>().Create(new { IsActive = true });

            //act
            _target.SetLookupData(ControllerAction.Search);
            var result = (IEnumerable<SelectListItem>)_target.ViewData["PresentedBy"];

            //assert
            Assert.AreEqual(2, result.Count());
        }

        #endregion
    }
}
