using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class SelectAttributeTest
    {
        #region Fields

        private SelectAttribute _target;
        private ModelMetadata _metadata;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new SelectAttribute();
            _metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(MockViewModel));
        }

        #endregion

        #region TestMethods

        #region Constructors

        [TestMethod]
        public void TestDefaultConstructorDoesNotSetAnyPropertyValues()
        {
            var target = new SelectAttribute();
            Assert.IsNull(target.ControllerViewDataKey);
            Assert.AreEqual(SelectType.DropDown, target.Type);
        }

        [TestMethod]
        public void TestOverloadConstructorSetsExpectedParameters()
        {
            var target = new SelectAttribute(SelectType.CheckBoxList, "controller key");
            Assert.AreEqual(SelectType.CheckBoxList, target.Type);
            Assert.AreEqual("controller key", target.ControllerViewDataKey);
        }

        [TestMethod]
        public void TestConstructorSetsControllerName()
        {
            var expected = "ControllerDoodad";
            var result = new SelectAttribute(SelectType.CheckBoxList, expected, null).Controller;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConstructorSetsActionName()
        {
            var expected = "ActionDoodad";
            var result = new SelectAttribute(SelectType.DropDown, null, expected).Action;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConstructorSetsAreaName()
        {
            var expected = "AreaName";
            var result = new SelectAttribute(SelectType.MultiSelect, expected, null, null).Area;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConstructorSetsDependentsRequiredToAllByDefault()
        {
            var result = new SelectAttribute();
            Assert.AreEqual(DependentRequirement.All, result.DependentsRequired);
        }

        #endregion

        #region GetSelectList

        [TestMethod]
        public void TestGetSelectListReturnsSelectListWithItemsFoundInViewDataWhenControllerViewDataKeyIsSet()
        {
            var expectedItems = new List<object> {"Oh hi"};
            var target = new SelectAttribute(SelectType.DropDown, "controller key");
            var vd = new ViewDataDictionary {
                {"controller key", expectedItems}
            };

            var selectList = target.GetSelectList(vd, null);

            Assert.AreSame(expectedItems, ((SelectList)selectList).Items);
        }

        [TestMethod]
        public void TestGetSelectListReturnsSelectListWithItemsFoundInViewDataWhenViewDataKeyDefaultsToPropertyName()
        {
            var expectedItems = new List<object>();
            expectedItems.Add("Oh hi");
            var target = new SelectAttribute(SelectType.DropDown, null);
            var model = new MockViewModel();
            var vd = new ViewDataDictionary(model);
            // Need to override the metadata cause otherwise it gives metadata for an entire class
            // instead of just a property on it.
            vd.ModelMetadata =
                ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(MockViewModel), "IntProp");
            vd["IntProp"] = expectedItems;
            var selectList = target.GetSelectList(vd, null);
            Assert.AreSame(expectedItems, ((SelectList)selectList).Items);
        }

        [TestMethod]
        public void TestGetSelectListReturnsSelectListWithValueAsDataValueFieldWhenModelTypeHasPublicGetterForId()
        {
            var expectedItems = new List<MockViewModelWithPublicId> {
                new MockViewModelWithPublicId()
            };
            var target = new SelectAttribute();
            var model = new MockViewModel();
            var vd = new ViewDataDictionary(model) {
                ModelMetadata =
                    ModelMetadataProviders.Current.GetMetadataForProperty(null,
                        typeof(MockViewModelWithPublicId), "IntProp"),
            };
            // Need to override the metadata cause otherwise it gives metadata for an entire class
            // instead of just a property on it.
            vd["IntProp"] = expectedItems;

            Assert.AreEqual("Value", ((SelectList)target.GetSelectList(vd, null)).DataValueField);
        }

        [TestMethod]
        public void TestGetSelectListSetsSelectedItem()
        {
            var expectedItems = new List<object>();
            expectedItems.Add("Oh hi");
            var target = new SelectAttribute(SelectType.DropDown, "controller key");
            var vd = new ViewDataDictionary();
            vd["controller key"] = expectedItems;

            var selectList = target.GetSelectList(vd, "Oh hi");
            Assert.AreEqual("Oh hi", ((SelectList)selectList).SelectedValue);
        }

        [TestMethod]
        public void TestGetSelectListCastsEnumSelectedValuesToIntsBecauseMvcsDropDownListDoesNotKnowHowToMatchEnums()
        {
            var expectedItems = new List<object>();
            expectedItems.Add((int)SomeEnum.Value);
            expectedItems.Add((int)SomeEnum.OtherValue);
            var target = new SelectAttribute(SelectType.DropDown, "controller key");
            var vd = new ViewDataDictionary();
            vd["controller key"] = expectedItems;

            var selectList = target.GetSelectList(vd, SomeEnum.OtherValue);
            Assert.AreEqual((int)SomeEnum.OtherValue, ((SelectList)selectList).SelectedValue);
        }

        [TestMethod]
        public void TestGetSelectListSetsMultipleSelectedValuesForMultiSelectTypes()
        {
            var expectedItems = new List<object>();
            expectedItems.Add("Selected 1");
            expectedItems.Add("Not Selected");
            expectedItems.Add("Selected 2");
            var target = new SelectAttribute(SelectType.MultiSelect, "key");
            var vd = new ViewDataDictionary();
            vd["key"] = expectedItems;

            // TODO: Need string check for IEnumerable 
            var selectList = (MultiSelectList)target.GetSelectList(vd, new[] {"Selected 1", "Selected 2"});
            var selectedValues = selectList.SelectedValues.Cast<object>();
            Assert.IsTrue(selectedValues.Contains("Selected 1"));
            Assert.IsFalse(selectedValues.Contains("Not Selected"));
            Assert.IsTrue(selectedValues.Contains("Selected 2"));
        }

        [TestMethod]
        public void TestGetSelectListThrowsForMultiSelectIfModelIsNotIEnumerable()
        {
            var expectedItems = new List<object>();
            var target = new SelectAttribute(SelectType.MultiSelect, "key");
            var vd = new ViewDataDictionary();
            vd["key"] = expectedItems;

            MyAssert.Throws<InvalidOperationException>(() => target.GetSelectList(vd, new object()));
        }

        [TestMethod]
        public void TestGetSelectListThrowsForMultiSelectIfModelIsString()
        {
            var expectedItems = new List<object>();
            var target = new SelectAttribute(SelectType.MultiSelect, "key");
            var vd = new ViewDataDictionary();
            vd["key"] = expectedItems;

            MyAssert.Throws<InvalidOperationException>(() => target.GetSelectList(vd, "i am a string"));
        }

        [TestMethod]
        public void TestGetSelectListDoesNotThrowForMultiSelectIfModelIsNull()
        {
            var expectedItems = new List<object>();
            var target = new SelectAttribute(SelectType.MultiSelect, "key");
            var vd = new ViewDataDictionary();
            vd["key"] = expectedItems;

            MyAssert.DoesNotThrow(() => target.GetSelectList(vd, null));
        }

        [TestMethod]
        public void TestGetSelectListRemovesItemsKeyFromViewData()
        {
            var controllerKeyName = "controller key";
            var expectedItems = new List<object>();
            expectedItems.Add("Oh hi");
            var target = new SelectAttribute(SelectType.DropDown, controllerKeyName);
            var vd = new ViewDataDictionary();
            vd[controllerKeyName] = expectedItems;

            target.GetSelectList(vd, "Oh hi");
            Assert.IsFalse(vd.ContainsKey(controllerKeyName));
        }

        #endregion

        #region Properties

        [TestMethod]
        public void TestActionGetsAndSets()
        {
            _target.Action = "Heyo";
            Assert.AreEqual("Heyo", _target.Action);
        }

        [TestMethod]
        public void TestDependsOnGetsAndSets()
        {
            _target.DependsOn = "Some dependable thing";
            Assert.AreEqual("Some dependable thing", _target.DependsOn);
        }

        [TestMethod]
        public void TestErrorTextGetsAndSets()
        {
            _target.ErrorText = "Errory";
            Assert.AreEqual("Errory", _target.ErrorText);
        }

        [TestMethod]
        public void TestHttpMethodGetsAndSets()
        {
            _target.HttpMethod = "Methody";
            Assert.AreEqual("Methody", _target.HttpMethod);
        }

        [TestMethod]
        public void TestLoadingTextGetsAndSets()
        {
            _target.LoadingText = "Loadingy";
            Assert.AreEqual("Loadingy", _target.LoadingText);
        }

        [TestMethod]
        public void TestPromptTextGetsAndSets()
        {
            _target.PromptText = "Prompty";
            Assert.AreEqual("Prompty", _target.PromptText);
        }

        [TestMethod]
        public void TestSettingTypeSetsType()
        {
            Assert.AreEqual(SelectType.DropDown, _target.Type);
            _target.Type = SelectType.MultiSelect;
            Assert.AreEqual(SelectType.MultiSelect, _target.Type);
        }

        [TestMethod]
        public void TestSettingControllerViewDataKeySetsControllerViewDataKey()
        {
            Assert.IsNull(_target.ControllerViewDataKey);
            _target.ControllerViewDataKey = "blah";
            Assert.AreEqual("blah", _target.ControllerViewDataKey);
        }

        #endregion

        #region Process

        [TestMethod]
        public void TestProcessAddsSelfToMetadataAdditionalValues()
        {
            var target = new SelectAttribute(SelectType.MultiSelect, "controller key");
            target.Process(_metadata);
            Assert.AreEqual(target,
                _metadata.AdditionalValues[
                    SelectAttribute.ADDITIONAL_VALUES_KEY]);
        }

        [TestMethod]
        public void TestProcessSetsTemplateHintToSelect()
        {
            var target = new SelectAttribute(SelectType.CheckBoxList, "controller key");
            target.Process(_metadata);
            Assert.AreEqual(SelectAttribute.DROPDOWN_TEMPLATE_HINT, _metadata.TemplateHint);
        }

        [TestMethod]
        public void TestProcessThrowsIfDependsOnIsNullWhenIsCascadingIsTrue()
        {
            var md = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(CascadeViewModel),
                "CascadingProperty");
            // Setting Action, because if it's null this will still throw an exception.
            _target.Action = "action";
            _target.Controller = "controller";
            Assert.IsNull(_target.DependsOn);
            Assert.IsTrue(_target.IsCascading);
            MyAssert.Throws<ArgumentNullException>(() => _target.Process(md));
        }

        [TestMethod]
        public void TestOnMetadataCreatedDoesNotThrowIfAreaIsNullOrEmpty()
        {
            // Setting DependsOn and Action, because if they're null this will still throw an exception.
            var md = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(CascadeViewModel),
                "CascadingProperty");
            _target.Action = "Action";
            _target.Controller = "Controller";
            _target.DependsOn = "ParentProperty";
            Assert.IsNull(_target.Area);
            Assert.IsTrue(_target.IsCascading);
            MyAssert.DoesNotThrow(() => _target.Process(md));
        }

        #endregion

        #endregion

        #region Nested Classes

        private class CascadeViewModel
        {
            public object ParentProperty { get; set; }
            public object CascadingProperty { get; set; }
        }

        private class MockViewModel
        {
            public int IntProp { get; set; }
            public SomeEnum EnumProp { get; set; }
        }

        private class MockViewModelWithProtectedId : MockViewModel
        {
            protected int Id { get; set; }
            public int MockViewModelWithProtectedIdId { get; set; }
        }

        private class MockViewModelWithPublicId : MockViewModel
        {
            public int Id { get; set; }
            public int MockViewModelWithPublicIdId { get; set; }
        }

        private enum SomeEnum
        {
            Value = 1,
            OtherValue = 2
        }

        #endregion
    }
}
