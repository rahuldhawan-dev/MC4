using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Utilities;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class EntityLookupControllerBaseTest : MapCallMvcControllerTestBase<EntityLookupController<MMSINC.Data.NHibernate.IRepository<EquipmentCategory>, EquipmentCategory>, EquipmentCategory>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject<MMSINC.Data.NHibernate.IRepository<EquipmentCategory>>(Repository);
            Request = Application.CreateRequestHandler("~/EquipmentCategory");
            _target =
                Request
                    .CreateAndInitializeController<EntityLookupController<MMSINC.Data.NHibernate.IRepository<EquipmentCategory>, EquipmentCategory>>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // Needed because controller is EntityLookupControllerBase
            options.ExpectedEditViewName = "~/Views/EntityLookup/Edit.cshtml";
            options.ExpectedNewViewName = "~/Views/EntityLookup/New.cshtml";
            options.ExpectedShowViewName = "~/Views/EntityLookup/Show.cshtml";
            options.ExpectedCreateRedirectControllerName = "EquipmentCategory";
            options.ExpectedUpdateRedirectControllerName = "EquipmentCategory";
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            // We need to use a url for an actual EntityLookup here, can't just use EntityLookup since it's not valid.
            Authorization.Assert(a =>
            {
                var module = RoleModules.FieldServicesDataLookups;
                a.RequiresRole("~/IncidentType/Show/", module, RoleActions.Read);
                a.RequiresRole("~/IncidentType/Index/", module, RoleActions.Read);
                a.RequiresRole("~/IncidentType/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/IncidentType/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/IncidentType/Create/", module, RoleActions.Add);
                a.RequiresRole("~/IncidentType/New/", module, RoleActions.Add);
            });
        }

        [TestMethod]
        public void TestClassIsSetupCorrectlyForRouteContext()
        {
            Assert.IsTrue(typeof(EntityLookup).IsAssignableFrom(typeof(FuelType)), "This test needs a new EntityLookup type!");
            var request = Application.CreateRequestHandler("~/FuelType/");
            var route = new RouteContext(request.RequestContext);
            Assert.AreEqual("FuelType", route.RouteControllerName);
            Assert.AreEqual("FuelType", route.ControllerName);
            Assert.AreEqual(typeof(EntityLookupController<MMSINC.Data.NHibernate.IRepository<FuelType>, FuelType>), route.ControllerDescriptor.ControllerType);
        }

        #endregion
    }
}
