using System.Web.UI;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;

namespace MapCallMVC.Tests.Areas.Facilities.Models.ViewModels
{
    [TestClass]
    public class EditFacilitySystemDeliveryEntryTypeTest : MapCallMvcInMemoryDatabaseTestBase<FacilitySystemDeliveryEntryType>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<EditFacilitySystemDeliveryEntryType>();
            _entity = GetEntityFactory<FacilitySystemDeliveryEntryType>().Create();
            _vmTester = new ViewModelTester<EditFacilitySystemDeliveryEntryType, FacilitySystemDeliveryEntryType>(_viewModel, _entity, _container.GetInstance<ITestDataFactoryService>());
            _entity.SupplierFacility = GetEntityFactory<Facility>().Create();
        }

        #endregion

        #region Fields

        protected ViewModelTester<EditFacilitySystemDeliveryEntryType, FacilitySystemDeliveryEntryType> _vmTester;
        protected EditFacilitySystemDeliveryEntryType _viewModel;
        protected FacilitySystemDeliveryEntryType _entity;

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.IsEnabled);
            _vmTester.CanMapBothWays(x => x.MaximumValue);
            _vmTester.CanMapBothWays(x => x.MinimumValue);
            _vmTester.CanMapBothWays(x => x.BusinessUnit);
            _vmTester.CanMapBothWays(x => x.IsInjectionSite);
            _vmTester.CanMapBothWays(x => x.IsAutomationEnabled);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SystemDeliveryEntryType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsEnabled);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.BusinessUnit);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsInjectionSite);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsAutomationEnabled);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.MaximumValue, (decimal)1.2, x => x.IsEnabled, true, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.MinimumValue, (decimal)1.2, x => x.IsEnabled, true, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create().Id, x => x.SystemDeliveryEntryType, SystemDeliveryEntryType.Indices.TRANSFERRED_FROM);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create().Id, x => x.SystemDeliveryEntryType, SystemDeliveryEntryType.Indices.TRANSFERRED_TO);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.SupplierFacility, GetEntityFactory<Facility>().Create().Id, x => x.SystemDeliveryEntryType, SystemDeliveryEntryType.Indices.TRANSFERRED_FROM);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.SupplierFacility, GetEntityFactory<Facility>().Create().Id, x => x.SystemDeliveryEntryType, SystemDeliveryEntryType.Indices.TRANSFERRED_TO);
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.SystemDeliveryEntryType, _entity.SystemDeliveryEntryType);
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.SupplierFacility, GetEntityFactory<Facility>().Create());
        }

        #endregion
    }
}
