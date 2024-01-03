using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;

namespace MMSINC.Core.MvcTest.Helpers.ControlBuilders
{
    [TestClass]
    public class ListBuilderTest
    {
        #region Fields

        private TestListBuilder _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new TestListBuilder();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestItemsReturnsEmptyList()
        {
            var target = new TestListBuilder();
            Assert.AreEqual(0, target.Items.Count);
        }

        [TestMethod]
        public void TestSelectedValuesReturnsEmptyList()
        {
            var target = new TestListBuilder();
            Assert.AreEqual(0, target.SelectedValues.Count);
        }

        [TestMethod]
        public void TestDefaultEmptyTextIsNull()
        {
            var target = new TestListBuilder();
            Assert.IsNull(target.EmptyText);
        }

        [TestMethod]
        public void TestGetItemsReturnsNewSelectListItemInstancesWithTheSameTextAndValue()
        {
            var expected = new SelectListItem {Selected = true, Text = "Some Text", Value = "Some Value"};
            _target.Items.Add(expected);
            var result = _target.GetItems().Single();
            Assert.AreNotSame(expected, result);
            Assert.AreEqual(expected.Text, result.Text);
            Assert.AreEqual(expected.Value, result.Value);
            Assert.AreNotEqual(expected.Selected, result.Selected,
                "The Selected value should be ignored unless SelectedValues includes the same value.");
        }

        [TestMethod]
        public void TestGetItemsReturnsItemsWithSelectedValuesWhenTheSelectedValuesAreStrings()
        {
            var expectedSelected = new SelectListItem {Text = "Some Text", Value = "Some Value"};
            var expectedUnselected = new SelectListItem {Text = "No!", Value = "Don't select me"};
            _target.Items.Add(expectedSelected);
            _target.Items.Add(expectedUnselected);
            _target.SelectedValues.Add(expectedSelected.Value);

            var result = _target.GetItems();
            Assert.IsTrue(result.Single(x => x.Value == expectedSelected.Value).Selected);
            Assert.IsFalse(result.Single(x => x.Value == expectedUnselected.Value).Selected);
        }

        [TestMethod]
        public void TestGetItemsDoesNotSelectItemsWhenNullIsASelectedValue()
        {
            var dontSelectNulls = new SelectListItem {Value = null};
            var dontSelectEmpties = new SelectListItem {Value = string.Empty};
            _target.Items.Add(dontSelectNulls);
            _target.Items.Add(dontSelectEmpties);
            _target.SelectedValues.Add(null);

            var result = _target.GetItems();
            Assert.IsFalse(result.Any(x => x.Selected == true));
        }

        [TestMethod]
        public void TestGetItemsDoesNotSelectItemsWhenAnEmptyStringIsASelectedValue()
        {
            var dontSelectNulls = new SelectListItem {Value = null};
            var dontSelectEmpties = new SelectListItem {Value = string.Empty};
            _target.Items.Add(dontSelectNulls);
            _target.Items.Add(dontSelectEmpties);
            _target.SelectedValues.Add(string.Empty);

            var result = _target.GetItems();
            Assert.IsFalse(result.Any(x => x.Selected == true));
        }

        [TestMethod]
        public void TestGetItemsCorrectlySelectsItemsWhenSelectedValueIsAnInteger()
        {
            var expectedSelected = new SelectListItem {Text = "Some Text", Value = "1"};
            var expectedUnselected = new SelectListItem {Text = "No!", Value = "2"};
            _target.Items.Add(expectedSelected);
            _target.Items.Add(expectedUnselected);
            _target.SelectedValues.Add(1);

            var result = _target.GetItems();
            Assert.IsTrue(result.Single(x => x.Value == expectedSelected.Value).Selected);
            Assert.IsFalse(result.Single(x => x.Value == expectedUnselected.Value).Selected);
        }

        [TestMethod]
        public void TestGetItemsSelectsItemsWhenTheSelectedValueIsAnEnumNumericalValue()
        {
            var expectedSelected = new SelectListItem {Text = "Some Text", Value = "1"};
            var expectedUnselected = new SelectListItem {Text = "No!", Value = "2"};
            _target.Items.Add(expectedSelected);
            _target.Items.Add(expectedUnselected);
            _target.SelectedValues.Add(IntEnum.One);

            var result = _target.GetItems();
            Assert.IsTrue(result.Single(x => x.Value == expectedSelected.Value).Selected);
            Assert.IsFalse(result.Single(x => x.Value == expectedUnselected.Value).Selected);
        }

        [TestMethod]
        public void TestGetItemsSelectedItemsWhenTheSelectedValueIsAnEnumThatIsATypeOtherThanInteger()
        {
            var expectedSelected = new SelectListItem {Text = "Some Text", Value = "1"};
            var expectedUnselected = new SelectListItem {Text = "No!", Value = "2"};
            _target.Items.Add(expectedSelected);
            _target.Items.Add(expectedUnselected);
            _target.SelectedValues.Add(ByteEnum.One);

            var result = _target.GetItems();
            Assert.IsTrue(result.Single(x => x.Value == expectedSelected.Value).Selected);
            Assert.IsFalse(result.Single(x => x.Value == expectedUnselected.Value).Selected);
        }

        [TestMethod]
        public void TestGetItemsAddsEmptyTextItemAsFirstItem()
        {
            var expectedSecond = new SelectListItem {Text = "Some Text", Value = "1"};
            _target.Items.Add(expectedSecond);
            _target.EmptyText = "Empty text";

            var result = _target.GetItems().ToArray();
            Assert.AreEqual("Empty text", result[0].Text);
            Assert.AreEqual(string.Empty, result[0].Value);
            Assert.AreEqual("Some Text", result[1].Text);
            Assert.AreEqual("1", result[1].Value);
        }

        [TestMethod]
        public void TestGetItemsDoesNotAddEmptyTextItemWhenEmptyTextIsNull()
        {
            var expectedSecond = new SelectListItem {Text = "Some Text", Value = "1"};
            _target.Items.Add(expectedSecond);
            _target.EmptyText = null;

            var result = _target.GetItems().ToArray();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Some Text", result[0].Text);
            Assert.AreEqual("1", result[0].Value);
        }

        [TestMethod]
        public void TestGetItemsDoesAddItemIfEmptyTextIsEmptyString()
        {
            _target.EmptyText = string.Empty;

            var result = _target.GetItems().ToArray();
            Assert.AreEqual(string.Empty, result[0].Text);
            Assert.AreEqual(string.Empty, result[0].Value);
        }

        [TestMethod]
        public void TestWithItemsReplacesItemsCollection()
        {
            var expected = new SelectListItem {Text = "Some Text", Value = "1"};
            var unexpected = new SelectListItem {Text = "No!", Value = "2"};
            _target.Items.Add(unexpected);
            _target.WithItems(new[] {expected});
            Assert.IsTrue(_target.Items.Contains(expected));
            Assert.IsFalse(_target.Items.Contains(unexpected));
        }

        [TestMethod]
        public void TestWithItemsSetsItemsToEmptyCollectionWhenValueIsNull()
        {
            _target.WithItems(null);
            Assert.IsFalse(_target.Items.Any());
        }

        [TestMethod]
        public void TestWithSelectedValuesReplacesSelectedValuesCollection()
        {
            var expected = new object();
            var unexpected = new object();
            _target.SelectedValues.Add(unexpected);
            _target.WithSelectedValues(new[] {expected});
            Assert.IsTrue(_target.SelectedValues.Contains(expected));
            Assert.IsFalse(_target.SelectedValues.Contains(unexpected));
        }

        [TestMethod]
        public void TestWithSelectedValuesSetsSelectedValuesToEmptyCollectionWhenValueIsNull()
        {
            _target.SelectedValues.Add("Some existing value");
            _target.WithSelectedValues(null);
            Assert.IsFalse(_target.SelectedValues.Any());
        }

        [TestMethod]
        public void TestWithEmptyTextSetsEmptyTextProperty()
        {
            _target.WithEmptyText("sup");
            Assert.AreEqual("sup", _target.EmptyText);

            _target.WithEmptyText(null);
            Assert.IsNull(_target.EmptyText);
        }

        #endregion

        #region Test classes

        private class TestListBuilder : ListBuilder<TestListBuilder>
        {
            protected override string CreateHtmlString()
            {
                throw new System.NotImplementedException();
            }

            public new SelectListItem[] GetItems()
            {
                return base.GetItems().ToArray();
            }

            public TestListBuilder WithEmptyText(string emptyText)
            {
                DoWithEmptyText(emptyText);
                return this;
            }

            public TestListBuilder WithItems(IEnumerable<SelectListItem> items)
            {
                DoWithItems(items);
                return this;
            }

            public TestListBuilder WithSelectedValues(IEnumerable<object> selectedValues)
            {
                DoWithSelectedValues(selectedValues);
                return this;
            }
        }

        private enum IntEnum
        {
            One = 1,
            Two = 2
        }

        private enum ByteEnum : byte
        {
            One = 1,
            Two = 2
        }

        #endregion
    }
}
