using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MMSINC.Data;
using MMSINC.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities;
using IContainer = StructureMap.IContainer;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class CustomModelMetadataProviderTest
    {
        #region Fields

        private TestMetadataProvider _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new TestMetadataProvider();
        }

        #endregion

        [TestMethod]
        public void TestCreateMetadataCallsProcessOnAttributesDerivedFromCustommModelMetadataAttribute()
        {
            var attr = new MockAttribute();
            var attributes = new[] {attr};
            _target.TestCreateMetadata(attributes, typeof(MockViewModel), null,
                typeof(MockViewModel), "Property");

            Assert.IsTrue(attr.WasProcessed);
        }

        [TestMethod]
        public void
            TestCreateMetadataUsesEntityDisplayNameAttributeIfModelIsViewModelAndViewModelDoesNotHaveThatAttribute()
        {
            var result = _target.GetMetadataForProperty(null, typeof(MockEntityViewModel), "DisplayName");
            Assert.AreEqual("Entity DisplayName", result.DisplayName,
                "The entity's DisplayName should have been used.");

            result = _target.GetMetadataForProperty(null, typeof(MockEntityViewModel), "DisplayNameViewModel");
            Assert.AreEqual("ViewModel DisplayNameViewModel", result.DisplayName,
                "The view model's DisplayName should override the entity DisplayName.");
        }

        [TestMethod]
        public void
            TestCreateMetadataUsesEntityDisplayFormatAttributeIfModelIsViewModelAndViewModelDoesNotHaveThatAttribute()
        {
            var result = _target.GetMetadataForProperty(null, typeof(MockEntityViewModel), "DisplayFormat");
            Assert.AreEqual("Entity DisplayFormat", result.DisplayFormatString,
                "The entity's DisplayFormat should have been used.");

            result = _target.GetMetadataForProperty(null, typeof(MockEntityViewModel), "DisplayFormatViewModel");
            Assert.AreEqual("ViewModel DisplayFormatViewModel", result.DisplayFormatString,
                "The view model's DisplayFormat should override the entity DisplayName.");
        }

        [TestMethod]
        public void TestCreateMetadataUsesEntityDisplayAttributeIfModelIsViewModelAndViewModelDoesNotHaveThatAttribute()
        {
            var result = _target.GetMetadataForProperty(null, typeof(MockEntityViewModel), "Display");
            Assert.AreEqual("Entity Display", result.DisplayName,
                "The entity's DisplayAttribute should have been used.");

            result = _target.GetMetadataForProperty(null, typeof(MockEntityViewModel), "DisplayViewModel");
            Assert.AreEqual("ViewModel DisplayViewModel", result.DisplayName,
                "The view model's DisplayAttribute should override the entity DisplayName.");
        }

        [TestMethod]
        public void TestCreateMetadataCorrectlyHandlesViewAttributeForDisplayFormatAttributeWithEntities()
        {
            var result = _target.GetMetadataForProperty(null, typeof(MockEntity), "DisplayFormatView");
            Assert.AreEqual(CommonStringFormats.DECIMAL_MAX_TWO_DECIMAL_PLACES, result.DisplayFormatString,
                "The entity's DisplayName should have been used.");
        }

        [TestMethod]
        public void TestCreateMetadataCorrectlHandlesViewAttributeOverrides()
        {
            var result = _target.GetMetadataForProperty(null, typeof(MockEntityViewModel),
                "DisplayFormatViewAttributeViewModel");
            Assert.AreEqual(CommonStringFormats.CURRENCY, result.DisplayFormatString,
                "The ViewModel's attribute should have overridden the inherited entity attribute.");
        }

        #region Helpers

        private class TestMetadataProvider : CustomModelMetadataProvider
        {
            public ModelMetadata TestCreateMetadata(IEnumerable<Attribute> attributes, Type containerType,
                Func<object> modelAccessor, Type modelType, string propertyName)
            {
                return CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            }
        }

        private class MockEntity
        {
            [DisplayName("Entity DisplayName")]
            public string DisplayName { get; set; }

            [DisplayName("Entity DisplayNameViewModel")]
            public string DisplayNameViewModel { get; set; }

            [DisplayFormat(DataFormatString = "Entity DisplayFormat")]
            public string DisplayFormat { get; set; }

            [DisplayFormat(DataFormatString = "Entity DisplayFormatViewModel")]
            public string DisplayFormatViewModel { get; set; }

            [Display(Name = "Entity Display")]
            public string Display { get; set; }

            [Display(Name = "Entity DisplayViewModel")]
            public string DisplayViewModel { get; set; }

            [View("Display this format view!", FormatStyle.DecimalMaxTwoDecimalPlaces)]
            public decimal DisplayFormatView { get; set; }

            [View("Override me!", FormatStyle.DecimalMaxTwoDecimalPlaces)]
            public decimal DisplayFormatViewAttributeViewModel { get; set; }
        }

        private class MockEntityViewModel : ViewModel<MockEntity>
        {
            public string DisplayName { get; set; }

            [DisplayName("ViewModel DisplayNameViewModel")]
            public string DisplayNameViewModel { get; set; }

            public string DisplayFormat { get; set; }

            [DisplayFormat(DataFormatString = "ViewModel DisplayFormatViewModel")]
            public string DisplayFormatViewModel { get; set; }

            public string Display { get; set; }

            [Display(Name = "ViewModel DisplayViewModel")]
            public string DisplayViewModel { get; set; }

            [View("I am an override!", FormatStyle.Currency)]
            public decimal DisplayFormatViewAttributeViewModel { get; set; }

            public MockEntityViewModel(IContainer container) : base(container) { }

            public MockEntityViewModel(IContainer container, MockEntity entity) : this(container)
            {
                if (entity != null)
                {
                    Map(entity);
                }
            }
        }

        private class MockViewModel
        {
            [Mock]
            public object Property { get; set; }
        }

        private class MockAttribute : Attribute, ICustomModelMetadataAttribute
        {
            public bool WasProcessed { get; set; }

            public void Process(ModelMetadata modelMetaData)
            {
                WasProcessed = true;
            }
        }

        #endregion
    }
}
