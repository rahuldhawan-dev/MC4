using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class HydrantWorkOrdersByDescriptionControllerTest : MapCallMvcControllerTestBase<HydrantWorkOrdersByDescriptionController, Hydrant, HydrantRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Reports/HydrantWorkOrdersByDescription/Search");
                a.RequiresLoggedInUserOnly("~/Reports/HydrantWorkOrdersByDescription/Index");
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var hydrant1 = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc1 });
            var hydrant2 = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc1 });

            var search = new SearchHydrantWorkOrdersByDescription { OperatingCenter = opc1.Id };
            var result = _target.Index(search);
            MvcAssert.IsViewNamed(result, "Index");

            Assert.AreEqual(2, search.Count);
        }
    }
}