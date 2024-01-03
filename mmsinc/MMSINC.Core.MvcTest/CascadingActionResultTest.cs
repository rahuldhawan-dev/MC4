using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MMSINC.Core.MvcTest
{
    // [DeploymentItem(@"x86\SQLite.Interop.dll", "x86")]
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    [TestClass]
    public class CascadingActionResultTest
    {
        #region Fields

        private TestCascadingActionResult _target;

        #endregion

        #region Init/cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new TestCascadingActionResult();
            _target.TextField = "Text";
            _target.ValueField = "Value";
        }

        #endregion

        #region Tests

        #region Constructors

        [TestMethod]
        public void TestConstructorSetsDataInDataOverload()
        {
            var expected = new[] {"string"};
            var target = new CascadingActionResult(expected);
            Assert.AreSame(expected, target.Data);
        }

        [TestMethod]
        public void TestConstructorSetsDataTextValue()
        {
            var expected = new[] {"string"};
            var target = new CascadingActionResult(expected, "Text", "Value");
            Assert.AreSame(expected, target.Data);
            Assert.AreSame("Text", target.TextField);
            Assert.AreSame("Value", target.ValueField);
        }

        [TestMethod]
        public void TestConstructorSetsSortItemsByTextFieldToTrue()
        {
            var target = new CascadingActionResult();
            Assert.IsTrue(target.SortItemsByTextField);
        }

        #endregion

        #region Properties

        [TestMethod]
        public void TestDataPropertyGetsAndSets()
        {
            var expected = new[] {"some string"};
            _target.Data = expected;
            Assert.AreSame(expected, _target.Data);
        }

        [TestMethod]
        public void TestJsonRequestBehaviorPropertyGetsAndSets()
        {
            _target.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            Assert.AreEqual(JsonRequestBehavior.AllowGet, _target.JsonRequestBehavior);
        }

        [TestMethod]
        public void TestSelectedValuePropertyGetsAndSets()
        {
            var expected = new object();
            _target.SelectedValue = expected;
            Assert.AreSame(expected, _target.SelectedValue);
        }

        [TestMethod]
        public void TestTextFieldPropertyGetsAndSets()
        {
            _target.TextField = "Text";
            Assert.AreEqual("Text", _target.TextField);
        }

        [TestMethod]
        public void TestValueFieldPropertyGetsAndSets()
        {
            _target.ValueField = "Value";
            Assert.AreEqual("Value", _target.ValueField);
        }

        #endregion

        #region CreateJsonResult

        [TestMethod]
        public void TestCreateJsonResultReturnsJsonResultObject()
        {
            _target.Data = new List<object>();
            MyAssert.IsInstanceOfType<JsonResult>(_target.CallCreateJsonResult());
        }

        [TestMethod]
        public void TestCreateJsonResultSetsJsonBehaviorOnJsonResultObject()
        {
            _target.Data = new List<object>();
            _target.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            var result = (JsonResult)_target.CallCreateJsonResult();
            Assert.AreEqual(_target.JsonRequestBehavior, result.JsonRequestBehavior);
        }

        [TestMethod]
        public void TestCreateJsonResultSetsDataToItemsFromGetJsonItemsMethod()
        {
            _target.SelectedValue = 2;
            _target.Data = new[] {new Item {Value = 2, Text = "Text"}};
            var expected = _target.GetJsonItems().ToArray();
            var result = (JsonResult)_target.CallCreateJsonResult();
            var resultData = (IEnumerable)result.Data;

            for (var i = 0; i < expected.Count(); i++)
            {
                Assert.AreEqual(expected.ToList()[i], resultData.OfType<object>().ToList()[i]);
            }
        }

        #endregion

        #region Execute

        [TestMethod]
        public void TestExecuteExecutesTheResultFromCreateJsonResult()
        {
            var actionResult = new Mock<ActionResult>();
            var context = new Mock<ControllerContext>();
            _target.TestJsonResult = actionResult.Object;
            _target.ExecuteResult(context.Object);

            actionResult.Verify(x => x.ExecuteResult(context.Object));
        }

        #endregion

        #region GetSelectListItems

        [TestMethod]
        public void TestGetSelectListItemsThrowsNullExceptionIfDataIsNull()
        {
            Assert.IsNull(_target.Data);
            // GetSelectListItems uses yield returns, so we need to explicitly call ToArray here.
            MyAssert.Throws<ArgumentNullException>(() => _target.GetSelectListItems().ToArray());
        }

        [TestMethod]
        public void TestGetSelectListItemsReturnsNoItemsIfThereAreNoItemsInData()
        {
            _target.Data = new List<string>();
            var result = _target.GetSelectListItems();
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void TestGetSelectListItemsAddsEmptyValueSelectItemIfThereAreAnyItems()
        {
            _target.Data = new[] {new Item {Value = 2, Text = "Text"}};

            var items = _target.GetSelectListItems().ToArray();

            Assert.AreEqual(2, items.Count());
            Assert.AreEqual("", items[0].Value);
        }

        [TestMethod]
        public void TestGetSelectListItemsSetsEmptyValueToSelectedIfSelectedValueDoesNotExistInData()
        {
            _target.SelectedValue = null;
            _target.Data = new[] {new Item {Value = 2, Text = "Text"}};

            var items = _target.GetSelectListItems().ToArray();

            Assert.IsTrue(items[0].Selected);
        }

        [TestMethod]
        public void TestGetSelectListItemsDoesNotSetEmptySelectValueToSelectedIfDataContainsSelectedValue()
        {
            _target.SelectedValue = 2;
            _target.Data = new[] {new Item {Value = 2, Text = "Text"}};
            var items = _target.GetSelectListItems().ToArray();
            Assert.IsFalse(items[0].Selected);
        }

        [TestMethod]
        public void TestGetSelectListItemsSetsSelectedToTrueOnDataItemThatMatchesSelectedValue()
        {
            _target.SelectedValue = 2;
            _target.Data = new[] {new Item {Value = 2, Text = "Text"}};

            var items = _target.GetSelectListItems().ToArray();

            Assert.IsTrue(items[1].Selected);
        }

        [TestMethod]
        public void TestGetSelectListItemsReturnsItemsSortedByTextValueIfSortItemsByTextFieldIsTrue()
        {
            var item1 = new Item {Value = 1, Text = "Z"};
            var item2 = new Item {Value = 2, Text = "A"};
            _target.Data = new[] {item1, item2};

            // Test sorted
            _target.SortItemsByTextField = true;
            var items = _target.GetSelectListItems().ToArray();
            Assert.AreEqual(item2.Text, items[1].Text);
            Assert.AreEqual(item1.Text, items[2].Text);

            // Test unsorted
            _target.SortItemsByTextField = false;
            items = _target.GetSelectListItems().ToArray();
            Assert.AreEqual(item1.Text, items[1].Text);
            Assert.AreEqual(item2.Text, items[2].Text);
        }

        #endregion

        #region GetJsonItems

        private void AssertSelectItemAndJsonItemEquality(SelectListItem selectItem, dynamic jsonItem)
        {
            Assert.AreEqual(selectItem.Text, jsonItem.text);
            Assert.AreEqual(selectItem.Value, jsonItem.value);
            if (selectItem.Selected)
            {
                Assert.AreEqual(selectItem.Selected, jsonItem.selected);
            }
        }

        [TestMethod]
        public void TestGetJsonItemsReturnsMatchingAnonymousObjectsFromGetSelectListItems()
        {
            _target.SelectedValue = 2;
            _target.Data = new[] {new Item {Value = 2, Text = "Text"}};
            var selectItems = _target.GetSelectListItems().ToArray();
            var jsonItems = _target.GetJsonItems().ToArray();

            for (var i = 0; i < selectItems.Count(); i++)
            {
                AssertSelectItemAndJsonItemEquality(selectItems[i], jsonItems[i]);
            }
        }

        #endregion

        #endregion

        #region Helper classes

        private class Item
        {
            public int Value { get; set; }
            public string Text { get; set; }
        }

        private class TestCascadingActionResult : CascadingActionResult
        {
            public ActionResult TestJsonResult { get; set; }

            protected internal override ActionResult CreateJsonResult()
            {
                return TestJsonResult ?? base.CreateJsonResult();
            }

            public ActionResult CallCreateJsonResult()
            {
                return CreateJsonResult();
            }
        }

        #endregion
    }
}
