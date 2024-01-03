using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class AddFacilitySystemDeliveryEntryTypeTest : MapCallMvcInMemoryDatabaseTestBase<Facility>
    {
        #region Private Members

        private AddFacilitySystemDeliveryEntryType _target;
        private Mock<IFacilityRepository> _mockFacilityRepo;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new AddFacilitySystemDeliveryEntryType(_container);
            _mockFacilityRepo = new Mock<IFacilityRepository>();
            _container.Inject(_mockFacilityRepo.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntityAddsNewEntryType()
        {
            var systemDeliveryEntryType = GetEntityFactory<SystemDeliveryEntryType>().Create(new { Description = "Purchase Point"});
            var facility = GetEntityFactory<Facility>().Create();
            _target.IsEnabled = true;
            _target.MaximumValue = 3.141m;
            _target.MinimumValue = 1.618m;
            _target.SystemDeliveryEntryType = systemDeliveryEntryType.Id;
            _target.IsInjectionSite = true;

            _target.MapToEntity(facility);

            Assert.AreEqual(1, facility.FacilitySystemDeliveryEntryTypes.Count);
            var entryType = facility.FacilitySystemDeliveryEntryTypes.Single();
            Assert.AreEqual(systemDeliveryEntryType, entryType.SystemDeliveryEntryType);
            Assert.AreEqual(_target.MaximumValue, entryType.MaximumValue);
            Assert.AreEqual(_target.MinimumValue, entryType.MinimumValue);
            Assert.IsTrue(entryType.IsEnabled);
            Assert.IsTrue(entryType.IsInjectionSite);
            Assert.IsFalse(entryType.IsAutomationEnabled);

            _target.IsAutomationEnabled = true;
            _target.MapToEntity(facility);
            entryType = facility.FacilitySystemDeliveryEntryTypes.First(x => x.IsAutomationEnabled == true);
            Assert.IsTrue(entryType.IsAutomationEnabled);
        }

        [TestMethod]
        public void TestMapToEntityAddsEntryWithSupplierFacilityWhenTranserFrom()
        {
            var systemDeliveryEntryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(4);
            var systemDeliveryTranserFrom = systemDeliveryEntryTypes.Single(x => x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_FROM);
            var facility = GetEntityFactory<Facility>().Create();
            var supplierFacility = GetEntityFactory<Facility>().Create();
            _mockFacilityRepo.Setup(x => x.Find(supplierFacility.Id)).Returns(supplierFacility);
            _target.IsEnabled = true;
            _target.MaximumValue = 3.141m;
            _target.MinimumValue = 1.618m;
            _target.SystemDeliveryEntryType = systemDeliveryTranserFrom.Id;
            _target.SupplierFacility = supplierFacility.Id;

            _target.MapToEntity(facility);

            Assert.AreEqual(1, facility.FacilitySystemDeliveryEntryTypes.Count);
            var entryType = facility.FacilitySystemDeliveryEntryTypes.Single();
            Assert.AreEqual(systemDeliveryTranserFrom, entryType.SystemDeliveryEntryType);
            Assert.AreEqual(_target.MaximumValue, entryType.MaximumValue);
            Assert.AreEqual(_target.MinimumValue, entryType.MinimumValue);
            Assert.AreEqual(supplierFacility, entryType.SupplierFacility);
            Assert.IsTrue(entryType.IsEnabled);
        }

        [TestMethod]
        public void TestMapToEntityAddsEntryWithSupplierFacilityWhenTranserTo()
        {
            var systemDeliveryEntryTypes = GetEntityFactory<SystemDeliveryEntryType>().CreateList(4);
            var systemDeliveryTranserTo = systemDeliveryEntryTypes.Single(x => x.Id == SystemDeliveryEntryType.Indices.TRANSFERRED_TO);
            var facility = GetEntityFactory<Facility>().Create();
            var supplierFacility = GetEntityFactory<Facility>().Create();
            _mockFacilityRepo.Setup(x => x.Find(supplierFacility.Id)).Returns(supplierFacility);
            _target.IsEnabled = true;
            _target.MaximumValue = 3.141m;
            _target.MinimumValue = 1.618m;
            _target.SystemDeliveryEntryType = systemDeliveryTranserTo.Id;
            _target.SupplierFacility = supplierFacility.Id;

            _target.MapToEntity(facility);

            Assert.AreEqual(1, facility.FacilitySystemDeliveryEntryTypes.Count);
            var entryType = facility.FacilitySystemDeliveryEntryTypes.Single();
            Assert.AreEqual(systemDeliveryTranserTo, entryType.SystemDeliveryEntryType);
            Assert.AreEqual(_target.MaximumValue, entryType.MaximumValue);
            Assert.AreEqual(_target.MinimumValue, entryType.MinimumValue);
            Assert.AreEqual(supplierFacility, entryType.SupplierFacility);
            Assert.IsTrue(entryType.IsEnabled);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequiredWhen(_target, x => x.MaximumValue, 3.14m, x => x.IsEnabled, true);
            ValidationAssert.PropertyIsRequiredWhen(_target, x => x.MinimumValue, 3.14m, x => x.IsEnabled, true);
            ValidationAssert.PropertyIsRequiredWhen(_target, x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create().Id, x => x.SystemDeliveryEntryType, SystemDeliveryEntryType.Indices.TRANSFERRED_FROM);
            ValidationAssert.PropertyIsRequiredWhen(_target, x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create().Id, x => x.SystemDeliveryEntryType, SystemDeliveryEntryType.Indices.TRANSFERRED_TO);
            ValidationAssert.PropertyIsRequiredWhen(_target, x => x.SupplierFacility, GetEntityFactory<Facility>().Create().Id, x => x.SystemDeliveryEntryType, SystemDeliveryEntryType.Indices.TRANSFERRED_FROM);
            ValidationAssert.PropertyIsRequiredWhen(_target, x => x.SupplierFacility, GetEntityFactory<Facility>().Create().Id, x => x.SystemDeliveryEntryType, SystemDeliveryEntryType.Indices.TRANSFERRED_TO);
            ValidationAssert.PropertyIsRequiredWhen(_target, x => x.PurchaseSupplier, "You know the sound a fork makes in the garbage disposal? That’s the sound my brain makes", x => x.SystemDeliveryEntryType, SystemDeliveryEntryType.Indices.PURCHASED_WATER);
            ValidationAssert.PropertyIsRequired(_target, x => x.IsEnabled);
            ValidationAssert.PropertyIsRequired(_target, x => x.SystemDeliveryEntryType);
            ValidationAssert.PropertyIsRequired(_target, x => x.IsInjectionSite);
        }

        #endregion
    }
}
