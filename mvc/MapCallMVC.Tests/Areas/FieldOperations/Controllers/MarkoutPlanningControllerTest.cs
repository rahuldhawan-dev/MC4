using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class MarkoutPlanningControllerTest : MapCallMvcControllerTestBase<MarkoutPlanningController, WorkOrder>
    {
        #region Setup
        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);

            options.EditReturnsPartialView = true;
            options.ExpectedEditViewName = "_Edit";
            options.UpdateReturnsPartialShowViewOnSuccess = true;
            options.ExpectedShowViewName = "_Show";
            options.CreateValidEntity = () => GetEntityFactory<WorkOrder>().Create(new {
                MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory),
                StreetOpeningPermitRequired = false
            });  
            options.CreateValidEntity = () => {
                var wo = GetEntityFactory<WorkOrder>().Create(new {
                    MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory),
                    StreetOpeningPermitRequired = false
                });
                AddWorkManagementRoleToCurrentUserForOperatingCenter(wo.OperatingCenter);
                return wo;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditMarkoutPlanning)vm;
                model.DateMarkoutNeeded = DateTime.Now;
                model.MarkoutTypeNeeded = GetEntityFactory<MarkoutType>().Create().Id;
            };
        }

        #endregion

        #region Private Methods

        private void AddWorkManagementRoleToCurrentUserForOperatingCenter(OperatingCenter opc)
        {
            var role = GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, opc, _currentUser, RoleActions.UserAdministrator);
        }        

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/MarkoutPlanning/Search", role, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/MarkoutPlanning/Index", role, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/MarkoutPlanning/Edit", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/MarkoutPlanning/Update", role, RoleActions.Edit);
            });
        }

        #endregion
    }
}
