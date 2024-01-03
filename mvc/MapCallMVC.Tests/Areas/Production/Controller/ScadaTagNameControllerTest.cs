using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Production.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class ScadaTagNameControllerTest : MapCallMvcControllerTestBase<ScadaTagNameController, ScadaTagName>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.ProductionWorkManagement;
            Authorization.Assert(auth => {
                auth.RequiresLoggedInUserOnly("~/Production/ScadaTagName/ByPartialMatchName");
            });
        }

        [TestMethod]
        public void TestByPartialMatchNameReturnsByPartialMatchName()
        {
            var stn = GetEntityFactory<ScadaTagName>().Create(new {Description = "foo", TagName = "bar" });
            
            var result = (AutoCompleteResult)_target.ByPartialMatchName("foo");
            var actual = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(1, actual.Count());
        }
    }
}
