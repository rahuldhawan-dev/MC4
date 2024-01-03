using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class ServiceSizeControllerTest : MapCallMvcControllerTestBase<ServiceSizeController, ServiceSize>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules module = RoleModules.FieldServicesDataLookups;
                a.RequiresRole("~/FieldOperations/ServiceSize/Index/", module);
                a.RequiresRole("~/FieldOperations/ServiceSize/Show/", module);
                a.RequiresRole("~/FieldOperations/ServiceSize/New/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/ServiceSize/Create/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/ServiceSize/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/ServiceSize/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/ServiceSize/Destroy/", module, RoleActions.Delete);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetFactory<ServiceSizeFactory>().Create();
            var expected = "3/4";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<ServiceSizeViewModel, ServiceSize>(eq, new {
                ServiceSizeDescription = expected
            }));

            Assert.AreEqual(expected, Session.Get<ServiceSize>(eq.Id).ServiceSizeDescription);
        }

        #endregion
    }
}