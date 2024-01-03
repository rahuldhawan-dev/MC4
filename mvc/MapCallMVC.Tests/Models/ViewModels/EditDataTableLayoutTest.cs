using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Utilities;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class EditDataTableLayoutTest : MapCallMvcInMemoryDatabaseTestBase<DataTableLayout>
    {
        #region Fields

        private DataTableLayout _entity;
        private EditDataTableLayout _target;
        private ViewModelTester<EditDataTableLayout, DataTableLayout> _vmTester;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetFactory<DataTableLayoutFactory>().Create();
            _target = _container.GetInstance<EditDataTableLayout>();
            _vmTester = new ViewModelTester<EditDataTableLayout, DataTableLayout>(_target, _entity);
            _target.TypeGuid = TypeCache.RegisterType(typeof(TestViewModel));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TypeCache.Clear();
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestMapToEntityRemovesPropertiesFromEntityThatWereRemovedFromViewModel()
        {
            var expectedToStay = new DataTableLayoutProperty { PropertyName = "IShouldStay" };
            var expectedToLeave = new DataTableLayoutProperty { PropertyName = "IShouldLeave" };
            _entity.Properties.Add(expectedToStay);
            _entity.Properties.Add(expectedToLeave);
            _target.Properties = new [] { "IShouldStay" };

            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.Properties.Contains(expectedToStay));
            Assert.IsFalse(_entity.Properties.Contains(expectedToLeave));
        }

        [TestMethod]
        public void TestMapToEntityAddsNewPropertiesToEntity()
        {
            _target.Properties = new [] { "AddMe" };

            _vmTester.MapToEntity();

            Assert.AreEqual(1, _entity.Properties.Count(x => x.PropertyName == "AddMe"));
        }

        [TestMethod]
        public void TestMapToEntityLeavesExistingPropertiesThatHaveNotBeenRemovedOrAdded()
        {
            var expectedToStay = new DataTableLayoutProperty { PropertyName = "IShouldStay" };
            _entity.Properties.Add(expectedToStay);
            _target.Properties = new [] { "IShouldStay" };

            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.Properties.Contains(expectedToStay));
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_target, x => x.Properties);
        }

        [TestMethod]
        public void TestValidationFailsIfPropertiesCollectionIsEmpty()
        {
            _target.Properties = new string[] { };
            ValidationAssert.ModelStateHasError(_target, x => x.Properties, "The Properties field is required.");
        }

        [TestMethod]
        public void TestValidationFailsIfPropertiesDoNotExistOnTheType()
        {
            _target.Properties = new [] { nameof(TestViewModel.Property), "NotRealProperty" };
            ValidationAssert.ModelStateHasError(_target, x => x.Properties, "The property 'NotRealProperty' does not exist.");

            _target.Properties = new [] { nameof(TestViewModel.Property) };
            ValidationAssert.ModelStateIsValid(_target, x => x.Properties);
        }

        [TestMethod]
        public void TestValidationFailsIfTypeCanNotBeFound()
        {
            _target.Properties = new string[] { }; // needs to be not-null so the validation doesn't cut out early.
            _target.TypeGuid = Guid.Empty;
            ValidationAssert.ModelStateHasError(_target, x => x.TypeGuid, "Unable to find type.");
        }

        #endregion

        #endregion

        #region Helper classes

        private class TestViewModel
        {
            public string Property { get; set; }
        }

        #endregion
    }
}
