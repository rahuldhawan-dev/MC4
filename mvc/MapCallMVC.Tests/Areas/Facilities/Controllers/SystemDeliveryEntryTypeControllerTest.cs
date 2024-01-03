using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class SystemDeliveryEntryTypeControllerTest : MapCallMvcControllerTestBase<SystemDeliveryEntryTypeController, SystemDeliveryEntryType>
    {
        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }
        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/SystemDeliveryEntryType/BySystemDeliveryTypeId");
                a.RequiresLoggedInUserOnly("~/SystemDeliveryEntryType/ByFacilitiesSystemDeliveryTypeId");
            });
        }

        #endregion

        #region Lookups

        [TestMethod]
        public void BySystemDeliveryTypeIdReturnsCorrectSystemDeliveryType()
        {
            var systemDeliveryTypes = GetFactory<SystemDeliveryTypeFactory>().CreateAll();
            var entryType = GetEntityFactory<SystemDeliveryEntryType>().Create(new {SystemDeliveryType = systemDeliveryTypes[0]});
            var otherEntryType = GetEntityFactory<SystemDeliveryEntryType>().Create(new {SystemDeliveryType = systemDeliveryTypes[1], Description = "Other"});

            var results = (CascadingActionResult)_target.BySystemDeliveryTypeId(entryType.Id);
            var actual = (IEnumerable<dynamic>)results.Data;

            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(entryType.Id, actual.First().Id);
        }

        [TestMethod]
        public void ByFacilitiesSystemDeliveryTypeIdReturnsCorrectTypes()
        {
            var systemDeliveryTypes = GetFactory<SystemDeliveryTypeFactory>().CreateAll();
            var entryType = GetEntityFactory<SystemDeliveryEntryType>().Create(new {SystemDeliveryType = systemDeliveryTypes[0]});
            var otherEntryType = GetEntityFactory<SystemDeliveryEntryType>().Create(new {SystemDeliveryType = systemDeliveryTypes[1], Description = "Other"});
            var facility = GetEntityFactory<Facility>().Create(new {SystemDeliveryType = systemDeliveryTypes[0]});
            var otherFacility = GetEntityFactory<Facility>().Create(new {SystemDeliveryType = systemDeliveryTypes[1]});

            Session.SaveOrUpdate(facility);
            Session.SaveOrUpdate(otherFacility);
            Session.Flush();

            var results = (CascadingActionResult)_target.ByFacilitiesSystemDeliveryTypeId(new[]{facility.Id, otherFacility.Id});
            var actual = (IEnumerable<dynamic>)results.Data;

            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(entryType.Id, actual.First().Id);
            Assert.AreEqual(otherEntryType.Id, actual.Last().Id);
            
            results = (CascadingActionResult)_target.ByFacilitiesSystemDeliveryTypeId(new[]{facility.Id});
            actual = (IEnumerable<dynamic>)results.Data;

            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(entryType.Id, actual.First().Id);

            results = (CascadingActionResult)_target.ByFacilitiesSystemDeliveryTypeId(new[]{otherFacility.Id});
            actual = (IEnumerable<dynamic>)results.Data;

            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(otherEntryType.Id, actual.First().Id);
        }

        #endregion
    }
}
