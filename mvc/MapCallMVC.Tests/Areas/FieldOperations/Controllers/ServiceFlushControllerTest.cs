using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities.Pdf;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class ServiceFlushControllerTest : MapCallMvcControllerTestBase<ServiceFlushController, ServiceFlush>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IImageToPdfConverter>().Mock();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (ServiceFlushViewModelBase)vm;
                var serviceId = Repository.Find(model.Id).Service.Id;
                return new { action = "Show", controller = "Service", id = serviceId };
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesAssets;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/FieldOperations/ServiceFlush/Edit/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/ServiceFlush/Update/", role, RoleActions.UserAdministrator);
            });
        }

        #endregion
    }
}
