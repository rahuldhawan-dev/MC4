using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Utilities;
using AdminUserFactory = MapCall.Common.Testing.Data.AdminUserFactory;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class GrievanceControllerTest : MapCallMvcControllerTestBase<GrievanceController, Grievance>
    {
        #region Setup/Teardown

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject<IDateTimeProvider>(new DateTimeProvider());
            _target = Request.CreateAndInitializeController<GrievanceController>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                ((CreateGrievance)vm).LaborRelationsBusinessPartner = GetEntityFactory<Employee>().Create().Id;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.HumanResourcesUnion;
                a.RequiresRole("~/Grievance/Search", module);
                a.RequiresRole("~/Grievance/Show", module);
                a.RequiresRole("~/Grievance/Index", module);
                a.RequiresRole("~/Grievance/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/Grievance/Update", module, RoleActions.Edit);
                a.RequiresRole("~/Grievance/New", module, RoleActions.Add);
                a.RequiresRole("~/Grievance/Create", module, RoleActions.Add);
                a.RequiresLoggedInUserOnly("~/Grievance/ByEmployeeId/");
            });
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var grievance = GetFactory<GrievanceFactory>().Create();

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditGrievance, Grievance>(grievance, new {
                Description = "new description"
            }));

            Assert.AreEqual("new description", Repository.Find(grievance.Id).Description);
        }

        #region ByEmployeeId

        [TestMethod]
        public void TestByEmployeeIdReturnsCascadingResultForMatchingEmployeeIds()
        {
            _currentUser.IsAdmin = true;
            var emp = GetFactory<EmployeeFactory>().Create();
            var gr = GetFactory<GrievanceFactory>().Create();

            if (gr.EntityType != null)
            {
                var ge = GetEntityFactory<GrievanceEmployee>().Create();

                ge.Grievance = gr;
                ge.Employee = emp;

                var result = (CascadingActionResult)_target.ByEmployeeId(emp.Id);
                var actual = result.GetSelectListItems().ToArray();

                Assert.AreEqual(1, actual.Count() - 1);
            }
        }

        #endregion
    }
}