using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HumanResources.Controllers;
using MapCallMVC.Areas.HumanResources.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.HumanResources.Controllers
{
    [TestClass]
    public class CovidIssueControllerTest : MapCallMvcControllerTestBase<CovidIssueController, CovidIssue, CovidIssueRepository>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => GetEntityFactory<CovidIssue>().Create(new {
                Employee = GetEntityFactory<Employee>().Create(new {
                    PersonnelArea = typeof(PersonnelAreaFactory)
                })
            });
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateCovidIssue)vm;
                model.State = GetEntityFactory<State>().Create().Id;
            };
            options.InitializeSearchTester = (tester) => {
                // State and OperatingCenter are both readonly string properties on
                // the entity. The search tester blows up because it tries to create
                // a String entity for these. So they need to be made manually for the test.
                tester.TestPropertyValues[nameof(SearchCovidIssue.State)] = GetEntityFactory<State>().Create().Id;
                tester.TestPropertyValues[nameof(SearchCovidIssue.OperatingCenter)] = GetEntityFactory<OperatingCenter>().Create().Id;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.HumanResourcesCovid;
                const string path = "~/HumanResources/CovidIssue/";
                a.RequiresRole(path + "Search", role, RoleActions.Read);
                a.RequiresRole(path + "Show", role, RoleActions.Read);
                a.RequiresRole(path + "Index", role, RoleActions.Read);
                a.RequiresRole(path + "Create", role, RoleActions.Add);
                a.RequiresRole(path + "New", role, RoleActions.Add);
                a.RequiresRole(path + "Edit", role, RoleActions.Edit);
                a.RequiresRole(path + "Update", role, RoleActions.Edit);
                a.RequiresRole(path + "Destroy", role, RoleActions.Delete);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee0 = GetEntityFactory<Employee>().Create(new {OperatingCenter = operatingCenter});
            var employee1 = GetEntityFactory<Employee>().Create(new {OperatingCenter = operatingCenter});
            var entity0 = GetEntityFactory<CovidIssue>().Create(new {
                Employee = employee0,
                SupervisorsCell = "SupervisorsCell 0"
            });
            var entity1 = GetEntityFactory<CovidIssue>().Create(new {
                Employee = employee1,
                SupervisorsCell = "SupervisorsCell 1"
            });

            var search = new SearchCovidIssue();

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(operatingCenter.Description, "OperatingCenter");
                helper.AreEqual(operatingCenter.Description, "OperatingCenter", 1);
                helper.AreEqual(entity0.SupervisorsCell, "SupervisorsCell");
                helper.AreEqual(entity1.SupervisorsCell, "SupervisorsCell", 1);
            }
        }

        #endregion

        #region New

        [TestMethod]
        public void TestNewSetsDropDownsToTBD()
        {
            var result = _target.New();
            var Resultmodel = ((ViewResultBase)result).Model;
            var covidModel = (CreateCovidIssue)Resultmodel;
            Assert.AreEqual(covidModel.FaceCoveringWorn, CovidAnswerType.Indices.TBD);
            Assert.AreEqual(covidModel.WorkExposure, CovidAnswerType.Indices.TBD);
            Assert.AreEqual(covidModel.AvoidableCloseContact, CovidAnswerType.Indices.TBD);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<CovidIssue>().Create();
            var expected = "SupervisorsCell field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditCovidIssue, CovidIssue>(eq, new {
                SupervisorsCell = expected
            }));

            Assert.AreEqual(expected, Session.Get<CovidIssue>(eq.Id).SupervisorsCell);
        }

        #endregion
    }
}
