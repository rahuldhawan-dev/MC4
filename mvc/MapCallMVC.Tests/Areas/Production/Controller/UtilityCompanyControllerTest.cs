using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Production.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class UtilityCompanyControllerTest : MapCallMvcControllerTestBase<UtilityCompanyController, UtilityCompany>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresLoggedInUserOnly("~/Production/UtilityCompany/ByStateId/");
            });
        }

        [TestMethod]
        public void TestByStateIdReturnsCascadingActionResult()
        {
            var stateValid = GetEntityFactory<State>().Create(new { Abbreviation = "NJ" });
            var stateInvalid = GetEntityFactory<State>().Create(new { Abbreviation = "QQ" });
            var utilityCompanyValid = GetEntityFactory<UtilityCompany>().Create(new {State = stateValid, Description = "Foo"});
            var utilityCompanyInvalid = GetEntityFactory<UtilityCompany>().Create(new {State = stateInvalid, Description = "Bar"});

            var results = (CascadingActionResult)_target.ByStateId(stateValid.Id);
            var data = results.GetSelectListItems().ToArray();

            Assert.AreEqual(2, data.Count());
            Assert.AreEqual(utilityCompanyValid.Description, data.Last().Text);
            Assert.AreEqual(utilityCompanyValid.Id.ToString(), data.Last().Value);
        }
    }
}
