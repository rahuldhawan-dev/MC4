using MapCall.Common.Testing;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels.SystemDeliveryEntries;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class CreateSystemDeliveryFacilityEntryTest : ViewModelTestBase<SystemDeliveryFacilityEntry, CreateSystemDeliveryFacilityEntry>
    {
        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.EntryValue);
            _vmTester.CanMapBothWays(x => x.EntryDate);
            _vmTester.CanMapBothWays(x => x.IsInjection);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.EntryValue);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.SystemDeliveryEntry, GetEntityFactory<SystemDeliveryEntry>().Create());
            ValidationAssert.EntityMustExist(x => x.SystemDeliveryEntryType, GetEntityFactory<SystemDeliveryEntryType>().Create());
            ValidationAssert.EntityMustExist(x => x.EnteredBy, GetEntityFactory<Employee>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // No-op
        }

        [TestMethod]
        public void EntryValuesAreZeroByDefault()
        {
            var vm = new CreateSystemDeliveryFacilityEntry(Container);
            Assert.AreEqual(vm.EntryValue, decimal.Zero);
        }

        #endregion
    }
}
