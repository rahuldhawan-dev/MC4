using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using System.Linq;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class GradientControllerTest : MapCallMvcControllerTestBase<GradientController, Gradient>
    {
        #region Tests

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<Gradient>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditGradient, Gradient>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<Gradient>(eq.Id).Description);
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresSiteAdminUser("~/Gradient/Search/");
                a.RequiresSiteAdminUser("~/Gradient/Show/");
                a.RequiresSiteAdminUser("~/Gradient/Index/");
                a.RequiresSiteAdminUser("~/Gradient/New/");
                a.RequiresSiteAdminUser("~/Gradient/Create/");
                a.RequiresSiteAdminUser("~/Gradient/Edit/");
                a.RequiresSiteAdminUser("~/Gradient/Update/");
                a.RequiresLoggedInUserOnly("~/Gradient/ByTownId");
            });
        }

        #region Ajaxie Actions

        [TestMethod]
        public void TestByTownIdReturnsGradientsByTownId()
        {
            var townA = GetEntityFactory<Town>().Create();
            var townB = GetEntityFactory<Town>().Create();
            var townC = GetEntityFactory<Town>().Create();

            var gradientAInTownA = GetEntityFactory<Gradient>().Create();
            var gradientBInTownA = GetEntityFactory<Gradient>().Create();
            var gradientCInTownB = GetEntityFactory<Gradient>().Create();
            var gradientDInTownC = GetEntityFactory<Gradient>().Create();

            gradientAInTownA.Towns.Add(townA);
            gradientBInTownA.Towns.Add(townA);
            gradientCInTownB.Towns.Add(townB);
            gradientDInTownC.Towns.Add(townC);

            Session.Flush();

            var result = (CascadingActionResult)_target.ByTownId(townA.Id, townB.Id);

            var items = result.GetSelectListItems().ToList();

            Assert.AreEqual(3, items.Count - 1);
            Assert.AreEqual(1, items.Count(x => x.Value == gradientAInTownA.Id.ToString()));
            Assert.AreEqual(1, items.Count(x => x.Value == gradientBInTownA.Id.ToString()));
            Assert.AreEqual(1, items.Count(x => x.Value == gradientCInTownB.Id.ToString()));
            Assert.AreEqual(0, items.Count(x => x.Value == gradientDInTownC.Id.ToString()));
            Assert.AreEqual(gradientAInTownA.Description, items.First(x => x.Value == gradientAInTownA.Id.ToString()).Text);
            Assert.AreEqual(gradientBInTownA.Description, items.First(x => x.Value == gradientBInTownA.Id.ToString()).Text);
            Assert.AreEqual(gradientCInTownB.Description, items.First(x => x.Value == gradientCInTownB.Id.ToString()).Text);
        }

        #endregion

        #endregion
    }
}
