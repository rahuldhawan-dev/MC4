using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HumanResources.Controllers;
using MapCallMVC.Areas.HumanResources.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.HumanResources.Controllers
{
    [TestClass]
    public class EmployeeAccountabilityActionControllerTest : MapCallMvcControllerTestBase<EmployeeAccountabilityActionController, EmployeeAccountabilityAction, IRepository<EmployeeAccountabilityAction>>
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
                var incident = GetEntityFactory<Incident>().Create(new {
                    Employee = GetEntityFactory<Employee>().Create()
                });

                return GetEntityFactory<EmployeeAccountabilityAction>().Create(new {
                    Employee = incident.Employee,
                    DateAdministered = Lambdas.GetNow,
                    Incident = incident,
                    OperatingCenter = GetEntityFactory<OperatingCenter>().Create(),
                    AccountabilityActionTakenType = GetEntityFactory<AccountabilityActionTakenType>().Create(),
                    DisciplineAdministeredBy = GetEntityFactory<Employee>().Create(),
                    AccountabilityActionTakenDescription = "wearing the world's largest shoes"
                });
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.HumanResourcesAccountabilityAction;
                const string path = "~/HumanResources/EmployeeAccountabilityAction/";
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
            var descrip = "it happen at midnight";
            var accountabilityActionTakenType = GetEntityFactory<AccountabilityActionTakenType>().Create();
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var employee = GetFactory<EmployeeFactory>().Create();
            var entity0 = GetEntityFactory<EmployeeAccountabilityAction>().Create(new
            {
                AccountabilityActionTakenType = accountabilityActionTakenType,
                OperatingCenter = operatingCenter,
                Employee = employee,
                DisciplineAdministeredBy = employee,
                AccountabilityActionTakenDescription = descrip
            });
            var entity1 = GetEntityFactory<EmployeeAccountabilityAction>().Create(new
            {
                AccountabilityActionTakenType = accountabilityActionTakenType,
                OperatingCenter = operatingCenter,
                Employee = employee,
                DisciplineAdministeredBy = employee,
                AccountabilityActionTakenDescription = descrip
            });
            var search = new SearchEmployeeAccountabilityAction();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Employee, "Employee");
                helper.AreEqual(entity1.Employee, "Employee");
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var accountabilityActionTakenType = GetEntityFactory<AccountabilityActionTakenType>().Create();
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var employee = GetFactory<EmployeeFactory>().Create();
            var eq = GetEntityFactory<EmployeeAccountabilityAction>().Create();
            var expected = employee.FullName;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEmployeeAccountabilityAction, EmployeeAccountabilityAction>(eq, new
            {
                OperatingCenter = operatingCenter.Id,
                DisciplineAdministeredBy = employee.Id,
                Employee = employee.Id,
                AccountabilityActionTakenDescription = "the thing"
            }));

            Assert.AreEqual(expected, Session.Get<EmployeeAccountabilityAction>(eq.Id).Employee.ToString());
        }
        
        #endregion

    }
}