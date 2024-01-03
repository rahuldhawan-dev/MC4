using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class ClassLocationControllerTest : MapCallMvcControllerTestBase<ClassLocationController, ClassLocation>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // Needed because the controller inherits from EntityLookupControllerBase.
            options.ExpectedEditViewName = "~/Views/ClassLocation/Edit.cshtml";
            options.ExpectedNewViewName = "~/Views/ClassLocation/New.cshtml";
            options.ExpectedShowViewName = "~/Views/ClassLocation/Show.cshtml";
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            // We need to use a url for an actual EntityLookup here, can't just use EntityLookup since it's not valid.
            Authorization.Assert(a =>
            {
                var module = RoleModules.FieldServicesDataLookups;
                a.RequiresRole("~/ClassLocation/Show/", module, RoleActions.Read);
                a.RequiresRole("~/ClassLocation/Index/", module, RoleActions.Read);
                a.RequiresRole("~/ClassLocation/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/ClassLocation/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/ClassLocation/Create/", module, RoleActions.Add);
                a.RequiresRole("~/ClassLocation/New/", module, RoleActions.Add);
            });
        }
    }
}
