using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Controllers
{
    [TestClass]
    public class IncidentInvestigationControllerTest : MapCallMvcControllerTestBase<IncidentInvestigationController, IncidentInvestigation>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "Incident", area = string.Empty, id = vm.Incident };
            options.NewReturnsPartialView = true;
            options.ExpectedNewViewName = "New";
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "Incident", area = string.Empty, id = vm.Incident };

            options.CreateValidEntity = () => {
                var oshaRecordableIncident = GetEntityFactory<Incident>().Create(new { IsOSHARecordable = true });
                return GetEntityFactory<IncidentInvestigation>().Create(new { Incident = oshaRecordableIncident });
            };

            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateIncidentInvestigation)vm;
                model.RootCauseFindingPerformedByUsers = new[] { GetEntityFactory<User>().Create().Id };
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditIncidentInvestigation)vm;
                model.RootCauseFindingPerformedByUsers = new[] { GetEntityFactory<User>().Create().Id };
            };
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.OperationsIncidents;

            Authorization.Assert(a => {
                a.RequiresRole("~/HealthAndSafety/IncidentInvestigation/New/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/IncidentInvestigation/Create/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/IncidentInvestigation/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/IncidentInvestigation/Update/", role, RoleActions.Edit);
            });
        }				

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var entity = GetEntityFactory<IncidentInvestigation>().Create();
            var findingType = GetEntityFactory<IncidentInvestigationRootCauseFindingType>().Create();

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditIncidentInvestigation, IncidentInvestigation>(entity, new {
                IncidentInvestigationRootCauseFindingType = findingType.Id
            }));

            Assert.AreSame(findingType, Session.Get<IncidentInvestigation>(entity.Id).IncidentInvestigationRootCauseFindingType);
        }

        #endregion
    }
}
