using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.ShortCycle.Controllers;
using MapCallMVC.Areas.ShortCycle.Models.ViewModels.ShortCycleCustomerMaterials;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.ShortCycle.Controllers
{
    [TestClass]
    public class ShortCycleCustomerMaterialControllerTest
        : MapCallMvcControllerTestBase<ShortCycleCustomerMaterialController, ShortCycleCustomerMaterial>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesShortCycle;
            Authorization.Assert(auth => {
                auth.RequiresRole("~/ShortCycle/ShortCycleCustomerMaterial/Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because Premise is required for the search, and only fragments are supported
            var cm1 = GetEntityFactory<ShortCycleCustomerMaterial>().Create();
            var cm2 = GetEntityFactory<ShortCycleCustomerMaterial>().Create();

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.FRAGMENT;
            
            var result = _target.Index(new SearchShortCycleCustomerMaterial {
                Premise = cm2.Premise.Id
            }) as PartialViewResult;
            
            var resultModel = ((SearchShortCycleCustomerMaterial)result.Model).Results.ToList();
            
            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Count);
            Assert.AreSame(cm2, resultModel.Single());
        }
    }
}
