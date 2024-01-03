using System.Linq;
using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class TownSectionControllerTest : ContractorControllerTestBase<TownSectionController, TownSection, TownSectionRepository>
    {
        #region Tests

        [TestMethod]
        public void TestByTownIDReturnsTownSectionsForTown()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            // TODO: Use a sequence here on the town section name
            var townSections = GetFactory<TownSectionFactory>().CreateList(2, new { Town = town }).ToArray();
            var extraTownSection = GetFactory<TownSectionFactory>().Create(new { Town = GetFactory<TownFactory>().Create(), Name="Not Me" });

            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                OperatingCenter = operatingCenter,
                Town = town
            });
            _currentUser.Contractor.OperatingCenters.Add(operatingCenter);
            //Session.SaveOrUpdate(town);
            //Session.SaveOrUpdate(_currentUser.Contractor);
            Session.Flush();
            var result = (CascadingActionResult)_target.ByTownId(town.Id);

            // This will return the actual select items, minus any sort of
            // extra empty "-- Select Here --" items. So the Count should be correct
            // without having to do any other checks.
            var yeah = (from ts in townSections
                        from json in result.GetSelectListItems()
                        where ts.Id.ToString() == json.Value 
                        select new { ts, json }).ToList();

            Assert.AreEqual(townSections.Count(), yeah.Count());
        }

        [TestMethod]
        public void TestByTownIdIsHttpGetOnly()
        {
            MyAssert.MethodHasAttribute<HttpGetAttribute>(_target, "ByTownId", new[] {typeof (int)});
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/TownSection/ByTownId");
            });
        }

        #endregion
    }
}
