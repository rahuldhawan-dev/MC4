using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.StreetOpeningPermits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class StreetOpeningPermitControllerTest
        : MapCallMvcControllerTestBase<StreetOpeningPermitController, StreetOpeningPermit>
    {
        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);

            options.CreateValidEntity = () => {
                var workOrder = GetEntityFactory<WorkOrder>().Create(new {
                    StreetOpeningPermitRequired = true
                });

                return GetEntityFactory<StreetOpeningPermit>().Create(new {
                    WorkOrder = workOrder
                });
            };

            options.CreateReturnsPartialShowViewOnSuccess = true;
            options.DestroyReturnsHttpStatusCodeNoContentOnSuccess = true;
            options.EditReturnsPartialView = true;
            options.ExpectedEditViewName = "_Edit";
            options.ExpectedNewViewName = "_New";
            options.ExpectedShowViewName = "_Show";
            options.NewReturnsPartialView = true;
            options.ShowReturnsPartialView = true;
            options.UpdateReturnsPartialShowViewOnSuccess = true;
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesWorkManagement;
            var urlBase = "~/StreetOpeningPermit/";

            Authorization.Assert(a => {
                a.RequiresRole(urlBase + "Show", role);
                a.RequiresRole(urlBase + "Destroy", role, RoleActions.Edit);
                a.RequiresRole(urlBase + "New", role, RoleActions.Edit);
                a.RequiresRole(urlBase + "Create", role, RoleActions.Edit);
                a.RequiresRole(urlBase + "Edit", role, RoleActions.Edit);
                a.RequiresRole(urlBase + "Update", role, RoleActions.Edit);
            });
        }

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            var order = ((StreetOpeningPermit)CreateEntityForAutomatedTests()).WorkOrder;

            var result = _target.New(order.Id);

            MvcAssert.IsViewNamed(result, "_New");

            var resultModel = ((ViewResultBase)result).Model;

            Assert.IsNotNull(resultModel);
            Assert.IsInstanceOfType(resultModel, typeof(CreateStreetOpeningPermit));
        }
    }
}
