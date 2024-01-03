using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class
        ServiceMaterialControllerTest : MapCallMvcControllerTestBase<ServiceMaterialController, ServiceMaterial>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // Needed because controller inherits from EntityLookupControllerBase
            options.ExpectedEditViewName = "~/Views/EntityLookup/Edit.cshtml";
            options.ExpectedNewViewName = "~/Views/EntityLookup/New.cshtml";
            options.ExpectedShowViewName = "~/Views/ServiceMaterial/Show.cshtml";
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var role = RoleModules.FieldServicesDataLookups;
                a.RequiresRole("~/ServiceMaterial/Show", role);
                a.RequiresRole("~/ServiceMaterial/Index", role);
                a.RequiresRole("~/ServiceMaterial/New", role, RoleActions.Add);
                a.RequiresRole("~/ServiceMaterial/Create", role, RoleActions.Add);
                a.RequiresRole("~/ServiceMaterial/Edit", role, RoleActions.Edit);
                a.RequiresRole("~/ServiceMaterial/Update", role, RoleActions.Edit);
                a.RequiresLoggedInUserOnly("~/ServiceMaterial/ByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/ServiceMaterial/ByOperatingCenterIdNewServices/");
            });
        }

        [TestMethod]
        public void TestByOperatingCenterIdReturnsMaterialsForOperatingCenter()
        {
            var opcntr1 = GetEntityFactory<OperatingCenter>().Create();
            var opcntr2 = GetEntityFactory<OperatingCenter>().Create(new { OperatingCenterCode = "NJ4" });
            var validMaterial = GetEntityFactory<ServiceMaterial>().Create(new { Description = "valid" });
            var invalidMaterial = GetEntityFactory<ServiceMaterial>().Create(new { Description = "Invalid" });
            validMaterial.OperatingCentersServiceMaterials.Add(new OperatingCenterServiceMaterial
                { OperatingCenter = opcntr1, ServiceMaterial = validMaterial });
            invalidMaterial.OperatingCentersServiceMaterials.Add(new OperatingCenterServiceMaterial
                { OperatingCenter = opcntr2, ServiceMaterial = invalidMaterial });
            Session.Save(validMaterial);
            Session.Save(invalidMaterial);
            Session.Flush();

            var result = (CascadingActionResult)_target.ByOperatingCenterId(opcntr1.Id);
            var actual = result.GetSelectListItems();
            Assert.AreEqual(1, actual.Count() - 1); // because --select here--
            foreach (var selectListItem in actual)
            {
                Assert.AreNotEqual(invalidMaterial.Id.ToString(), selectListItem.Value);
            }
        }

        [TestMethod]
        public void TestByOperatingCenterIdNewServicesReturnsMaterialsForOperatingCenter()
        {
            var opcntr1 = GetEntityFactory<OperatingCenter>().Create();
            var opcntr2 = GetEntityFactory<OperatingCenter>().Create(new { OperatingCenterCode = "NJ4" });
            var validMaterial = GetEntityFactory<ServiceMaterial>().Create(new { Description = "valid" });
            var invalidMaterial = GetEntityFactory<ServiceMaterial>().Create(new { Description = "Invalid" });
            validMaterial.OperatingCentersServiceMaterials.Add(new OperatingCenterServiceMaterial
                { OperatingCenter = opcntr1, ServiceMaterial = validMaterial, NewServiceRecord = true });
            invalidMaterial.OperatingCentersServiceMaterials.Add(new OperatingCenterServiceMaterial
                { OperatingCenter = opcntr1, ServiceMaterial = invalidMaterial, NewServiceRecord = false });
            invalidMaterial.OperatingCentersServiceMaterials.Add(new OperatingCenterServiceMaterial
                { OperatingCenter = opcntr2, ServiceMaterial = invalidMaterial, NewServiceRecord = true });
            Session.Save(validMaterial);
            Session.Save(invalidMaterial);
            Session.Flush();

            var result = (CascadingActionResult)_target.ByOperatingCenterIdNewServices(opcntr1.Id);
            var actual = result.GetSelectListItems();
            Assert.AreEqual(1, actual.Count() - 1); // because --select here--
            foreach (var selectListItem in actual)
            {
                Assert.AreNotEqual(invalidMaterial.Id.ToString(), selectListItem.Value);
            }
        }

        [TestMethod]
        public void TestIndexReturnsIndexViewWithServiceMaterialDetails()
        {
            var epaCode1 = GetFactory<EPACodeFactory>().Create(new { Description = "LEAD" });
            var epaCode2 = GetFactory<EPACodeFactory>().Create(new { Description = "NON_LEAD" });
            var serviceMaterial1 = GetFactory<ServiceMaterialFactory>().Create(new {
                Description = "LEAD", IsEditEnabled = true, CompanyEPACode = epaCode1, CustomerEPACode = epaCode1
            });
            var serviceMaterial2 = GetFactory<ServiceMaterialFactory>().Create(new {
                Description = "AC", IsEditEnabled = true, CompanyEPACode = epaCode2, CustomerEPACode = epaCode2
            });

            var result = _target.Index();

            MvcAssert.IsViewNamed(result, "~/Views/ServiceMaterial/Index.cshtml");
            MvcAssert.IsViewWithEnumerableModel(result, new[] { serviceMaterial1, serviceMaterial2 });
        }
    }
}
