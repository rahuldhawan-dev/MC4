using System.Linq;
using Contractors.Controllers;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class TownControllerTest : ContractorControllerTestBase<TownController, Town, TownRepository>
    {
        #region Tests

        [TestMethod]
        public void TestByOperatingCenterIDReturnsTownsForOperatingCenter()
        {
            var invalidTown = GetFactory<TownFactory>().Build();
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var towns = GetFactory<TownFactory>().CreateList(3);
            towns.Each(x => {
                x.OperatingCentersTowns.Add(new OperatingCenterTown {
                    OperatingCenter = operatingCenter,
                    Town = x
                });
                Session.SaveOrUpdate(x);
            });
            _currentUser.Contractor.OperatingCenters.Add(operatingCenter);
           // Session.SaveOrUpdate(_currentUser.Contractor);
            Session.Flush();
            var result =
                (CascadingActionResult)
                    _target.ByOperatingCenterId(
                        operatingCenter.Id);

            var actual = result.GetSelectListItems();

            Assert.AreEqual(towns.Count(), actual.Count() - 1); // -1 accounts for the select here
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Town/ByOperatingCenterId");
                a.RequiresLoggedInUserOnly("~/Town/ByCountyId");
            });
        }

        #endregion
    }
}
