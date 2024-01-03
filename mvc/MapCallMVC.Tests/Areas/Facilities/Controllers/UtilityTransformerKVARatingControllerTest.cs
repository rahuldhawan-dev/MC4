using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Facilities.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class UtilityTransformerKVARatingControllerTest : MapCallMvcControllerTestBase<UtilityTransformerKVARatingController, UtilityTransformerKVARating>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresLoggedInUserOnly("~/Facilities/UtilityTransformerKVARating/ByVoltage/");
            });
        }

        [TestMethod]
        public void TestByVoltageReturnsRecordsByVoltage()
        {
            var v1 = GetEntityFactory<Voltage>().Create(new{ Description = "120" });
            var v2 = GetEntityFactory<Voltage>().Create(new{ Description = "240" });

            var ukvr1 = GetEntityFactory<UtilityTransformerKVARating>().Create(new {Description = "For 120"});
            var ukvr2 = GetEntityFactory<UtilityTransformerKVARating>().Create(new {Description = "For 120 and 240"});
            var ukvr3 = GetEntityFactory<UtilityTransformerKVARating>().Create(new {Description = "For 240"});

            v1.UtilityTransformerKVARatings.Add(ukvr1);
            v1.UtilityTransformerKVARatings.Add(ukvr2);
            v2.UtilityTransformerKVARatings.Add(ukvr2);
            v2.UtilityTransformerKVARatings.Add(ukvr3);

            Session.Save(v1);
            Session.Save(v2);
            Session.Flush();

            // Using GetSelectListItems here seems weird to me but it's currently the only
            // way to test these things when the repository is returning IQueryable<dynamic generated type>
            var result = ((CascadingActionResult)_target.ByVoltage(v1.Id)).GetSelectListItems(false);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(x => x.Value == ukvr1.Id.ToString() && x.Text == ukvr1.Description));
            Assert.IsTrue(result.Any(x => x.Value == ukvr2.Id.ToString() && x.Text == ukvr2.Description));

            result = ((CascadingActionResult)_target.ByVoltage(v2.Id)).GetSelectListItems(false);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(x => x.Value == ukvr2.Id.ToString() && x.Text == ukvr2.Description));
            Assert.IsTrue(result.Any(x => x.Value == ukvr3.Id.ToString() && x.Text == ukvr3.Description));
        }

        #endregion
    }
}
