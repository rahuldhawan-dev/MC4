using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class GrievanceCategorizationControllerTest : MapCallMvcControllerTestBase<GrievanceCategorizationController, GrievanceCategorization>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/GrievanceCategorization/ByCategoryIdOrAll/");
            });
        }

        #region ByOperatingCenterId

        [TestMethod]
        public void TestByCategoryIdReturnsGrievanceCategorizationsForCategory()
        {
            var gory = GetFactory<GrievanceCategoryFactory>().Create();
            var invalidGory = GetFactory<GrievanceCategoryFactory>().Create(new { Description = "Fl[]ppy" });
            var categorization1 = GetFactory<GrievanceCategorizationFactory>().Create(new { GrievanceCategory = gory });
            var categorization2 = GetFactory<GrievanceCategorizationFactory>().Create(new { GrievanceCategory = gory });
            var invalid = GetFactory<GrievanceCategorizationFactory>().Create(new { GrievanceCategory = invalidGory});

            Session.Flush();

            var result = (CascadingActionResult)_target.ByCategoryIdOrAll(gory.Id);

            var actual = result.GetSelectListItems();

            Assert.AreEqual(2, actual.Count() - 1); // --select here--
            foreach (var selectListItem in actual)
            {
                Assert.AreNotEqual(invalid.Id.ToString(), selectListItem.Value);
            }
        }

        [TestMethod]
        public void TestByCategoryIdReturnsAllGrievanceCategoriesWhenNull()
        {
            var gory = GetFactory<GrievanceCategoryFactory>().Create();
            var gory2 = GetFactory<GrievanceCategoryFactory>().Create(new { Description = "Fl[]ppy" });
            var categorization1 = GetFactory<GrievanceCategorizationFactory>().Create(new { GrievanceCategory = gory });
            var categorization2 = GetFactory<GrievanceCategorizationFactory>().Create(new { GrievanceCategory = gory });
            var categorization3 = GetFactory<GrievanceCategorizationFactory>().Create(new { GrievanceCategory = gory2 });

            Session.Flush();

            var result = (CascadingActionResult)_target.ByCategoryIdOrAll(null);

            var actual = result.GetSelectListItems();

            Assert.AreEqual(3, actual.Count() - 1); // --select here--
        }

        #endregion
    }
}