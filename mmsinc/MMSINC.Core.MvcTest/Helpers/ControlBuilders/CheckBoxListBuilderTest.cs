using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;

namespace MMSINC.Core.MvcTest.Helpers.ControlBuilders
{
    [TestClass]
    public class CheckBoxListBuilderTest
    {
        #region Fields

        private CheckBoxListBuilder _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new CheckBoxListBuilder();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestToHtmlStringRendersCheckBoxesWithExpectedCheckedValues()
        {
            var expected = "<div class=\"checkbox-list\" id=\"Id\">" +
                           "<input class=\"dummy-check-box-list-input\" name=\"Id_CheckBoxList\" type=\"hidden\" />" +
                           "<mc-checkboxlistitem checked=\"\" name=\"Id\" text=\"First\" value=\"1\"></mc-checkboxlistitem>" +
                           "<mc-checkboxlistitem name=\"Id\" text=\"Second\" value=\"2\"></mc-checkboxlistitem>" +
                           "<mc-checkboxlistitem checked=\"\" name=\"Id\" text=\"Third\" value=\"3\"></mc-checkboxlistitem>" +
                           "</div>";

            var items = new List<SelectListItem>();
            items.Add(new SelectListItem {Text = "First", Value = "1"});
            items.Add(new SelectListItem {Text = "Second", Value = "2"});
            items.Add(new SelectListItem {Text = "Third", Value = "3"});
            _target.WithItems(items);
            _target.WithSelectedValues(new object[] {1, 3});

            _target.Id = "Id";
            _target.Name = "Id";
            Assert.AreEqual(expected, _target.ToHtmlString());
        }

        [TestMethod]
        public void TestWithAddsAnonymousObjectAsHtmlAttributes()
        {
            var expected = new {
                mrs_dash = "yeah"
            };

            _target.With(expected);
            Assert.AreSame(expected.mrs_dash, _target.HtmlAttributes["mrs-dash"]);
        }

        [TestMethod]
        public void TestWithAddsDictionaryAsHtmlAttributes()
        {
            var expected = new Dictionary<string, object>();
            expected["something"] = "yay";

            _target.With(expected);
            Assert.AreSame("yay", _target.HtmlAttributes["something"]);
        }

        [TestMethod]
        public void TestWithAdds()
        {
            _target.With("neat", "cool");
            Assert.AreEqual("cool", _target.HtmlAttributes["neat"]);
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
        public void TestWithEmptyTextSetsEmptyTextProperty()
        {
            _target.WithEmptyText("sup");
            Assert.AreEqual("sup", _target.EmptyText);

            _target.WithEmptyText(null);
            Assert.IsNull(_target.EmptyText);
        }

        [TestMethod]
        public void TestWithAddsAndReturnsSelf()
        {
            var result = _target.With("neat", "cool");
            Assert.AreEqual("cool", _target.HtmlAttributes["neat"]);
            Assert.AreSame(_target, result);
        }

        [TestMethod]
        public void TestWithIdSetsIdAndReturnsSelf()
        {
            var result = _target.WithId("Id");
            Assert.AreEqual("Id", _target.Id);
            Assert.AreSame(_target, result);
        }

        [TestMethod]
        public void TestWithNameSetsNameAndReturnsSelf()
        {
            var result = _target.WithName("Name");
            Assert.AreEqual("Name", _target.Name);
            Assert.AreSame(_target, result);
        }

        #endregion
    }
}
