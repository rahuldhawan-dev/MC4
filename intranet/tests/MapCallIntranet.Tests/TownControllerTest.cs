using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallIntranet.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;

namespace MapCallIntranet.Tests
{
    [TestClass]
    public class TownControllerTest : MapCallIntranetControllerTestBase<TownController, Town>
    {
        [TestMethod]
        public override void TestControllerAuthorization() {}

        #region ByOperatingCenterId

        [TestMethod]
        public void TestByOperatingCenterIdReturnsTownsForOperatingCenter()
        {
            var opcntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opcntr2 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ4" });
            var validTowns = GetFactory<TownFactory>().CreateList(2);
            var invalid = GetFactory<TownFactory>().Create();
            validTowns[0].OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opcntr1, Town = validTowns[0], Abbreviation = "XX" });
            validTowns[1].OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opcntr1, Town = validTowns[1], Abbreviation = "XX" });
            invalid.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opcntr2, Town = invalid, Abbreviation = "XX" });

            Session.Flush();

            var result = (CascadingActionResult)_target.ByOperatingCenterId(opcntr1.Id);

            var actual = result.GetSelectListItems();

            Assert.AreEqual(validTowns.Count(), actual.Count() - 1); // --select here--
            foreach (var selectListItem in actual)
            {
                Assert.AreNotEqual(invalid.Id.ToString(), selectListItem.Value);
            }
        }

        #endregion
    }
}
