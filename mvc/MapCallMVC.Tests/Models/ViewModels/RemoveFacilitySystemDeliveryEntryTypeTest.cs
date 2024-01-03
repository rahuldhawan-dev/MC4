using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class RemoveFacilitySystemDeliveryEntryTypeTest : MapCallMvcInMemoryDatabaseTestBase<Facility>
    {
        #region Private Members

        private RemoveFacilitySystemDeliveryEntryType _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new RemoveFacilitySystemDeliveryEntryType(_container);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntityRemovesEntryType()
        {
            var systemDeliveryType = GetEntityFactory<SystemDeliveryType>().Create();
            var facility = GetEntityFactory<Facility>().Create(new { SystemDeliveryType = systemDeliveryType });
            var systemDeliveryEntryType = GetEntityFactory<SystemDeliveryEntryType>().Create(new { Description = "Purchase Point", SystemDeliveryType = systemDeliveryType });
            var systemDeliveryEntryTypeOther = GetEntityFactory<SystemDeliveryEntryType>().Create(new { Description = "Delivered Water", SystemDeliveryType = systemDeliveryType });
            var entryType1 = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {
                Facility = facility, MaximumValue = 3.14m, MinimumValue = 1.618m, IsEnabled = true,
                SystemDeliveryEntryType = systemDeliveryEntryType
            });
            var entryType2 = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create(new {
                Facility = facility, MaximumValue = 3.14m, MinimumValue = 1.618m, IsEnabled = true,
                SystemDeliveryEntryType = systemDeliveryEntryTypeOther
            });
            facility.FacilitySystemDeliveryEntryTypes.Add(entryType1);
            facility.FacilitySystemDeliveryEntryTypes.Add(entryType2);
            _target.FacilitySystemDeliveryEntryTypeId = entryType1.Id;

            _target.MapToEntity(facility);
            
            Assert.AreEqual(1, facility.FacilitySystemDeliveryEntryTypes.Count);
            Assert.AreEqual(entryType2, facility.FacilitySystemDeliveryEntryTypes.FirstOrDefault());
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_target, x => x.FacilitySystemDeliveryEntryTypeId);
        }

        #endregion
    }
}
