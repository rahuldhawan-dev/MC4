using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class RestorationProcessingControllerTest : MapCallMvcControllerTestBase<RestorationProcessingController, WorkOrder>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var workOrder = GetFactory<WorkOrderFactory>().Create();
                GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, workOrder.OperatingCenter, _currentUser, RoleActions.UserAdministrator);
                return workOrder;
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/RestorationProcessing/Show", role, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/RestorationProcessing/Search", role, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/RestorationProcessing/Index", role, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/RestorationProcessing/Edit", role, RoleActions.Edit);
            });
        }

        [TestMethod]
        public override void TestEditReturns404IfMatchingRecordCanNotBeFound()
        {
            // noop, Edit action only needed so users with edit permissions can add/edit documents
        }

        [TestMethod]
        public override void TestEditReturnsEditViewWithEditViewModel()
        {
            // noop, Edit action only needed so users with edit permissions can add/edit documents
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            //var restoration = GetEntityFactory<Restoration>().Create();
            //var restList = new List<Restoration> { restoration };
            //var wo = GetEntityFactory<WorkOrder>().Create(new
            //    { WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory), Restorations = restList });

            var wo = GetEntityFactory<WorkOrder>().Create(new { WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory) });
            var restoration = GetEntityFactory<Restoration>().Create(new { WorkOrder = wo });

            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, wo.OperatingCenter, _currentUser, RoleActions.UserAdministrator);
            var searchViewModel = new SearchRestorationProcessing();

            var result = _target.Index(searchViewModel);

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, searchViewModel.Count);
            Assert.AreSame(wo, searchViewModel.Results.Single());
        }

        #endregion
    }
}
