using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class TrainingContactHoursProgramCoordinatorControllerTest : MapCallMvcControllerTestBase<TrainingContactHoursProgramCoordinatorController, TrainingContactHoursProgramCoordinator>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<ITrainingContactHoursProgramCoordinatorRepository>().Use<TrainingContactHoursProgramCoordinatorRepository>();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.OperationsTrainingRecords;
                const string path = "~/TrainingContactHoursProgramCoordinator/";
                a.RequiresRole(path + "Search", role, RoleActions.Read);
                a.RequiresRole(path + "Show", role, RoleActions.Read);
                a.RequiresRole(path + "Index", role, RoleActions.Read);
                a.RequiresRole(path + "Create", role, RoleActions.Add);
                a.RequiresRole(path + "New", role, RoleActions.Add);
                a.RequiresRole(path + "Edit", role, RoleActions.Edit);
                a.RequiresRole(path + "Update", role, RoleActions.Edit);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<TrainingContactHoursProgramCoordinator>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<TrainingContactHoursProgramCoordinator>().Create(new {Description = "description 1"});
            var search = new SearchTrainingContactHoursProgramCoordinator();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Description, "Description");
                helper.AreEqual(entity1.Description, "Description", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<TrainingContactHoursProgramCoordinator>().Create();
            var emp = GetEntityFactory<Employee>().Create();

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditTrainingContactHoursProgramCoordinator, TrainingContactHoursProgramCoordinator>(eq, new {
                ProgramCoordinator = emp.Id
            }));

            Assert.AreEqual(emp.ToString(), Session.Get<TrainingContactHoursProgramCoordinator>(eq.Id).Description);
        }

        #endregion
    }
}
