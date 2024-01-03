using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class HydrantModelControllerTest : MapCallMvcControllerTestBase<HydrantModelController, HydrantModel, HydrantModelRepository>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresLoggedInUserOnly("~/FieldOperations/HydrantModel/ByManufacturerId");
            });
        }

        #endregion
    }
}
