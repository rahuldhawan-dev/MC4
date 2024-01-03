using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;

namespace MMSINC.Core.MvcTest.Helpers.ControlBuilders
{
    [TestClass]
    public class SelectListBuilderTest
    {
        #region Fields

        private SelectListBuilder _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new SelectListBuilder();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestDefaultTypeIsDropDown()
        {
            var target = new SelectListBuilder();
            Assert.AreEqual(SelectListType.DropDown, target.Type);
        }

        [TestMethod]
        public void TestToHtmlStringRendersSelectTagWithMultipleAttributeIfTypeIsListBox()
        {
            _target.Type = SelectListType.ListBox;
            var result = _target.ToHtmlString();
            Assert.AreEqual("<select multiple=\"multiple\"></select>", result);
        }

        [TestMethod]
        public void TestToHtmlStringRendersSelectTagOptions()
        {
            var expected = "<select><option value=\"Hi Value\">Hi</option>" +
                           "<option value=\"Bye Value\">Bye</option></select>";
            _target.Items.Add(new SelectListItem {Text = "Hi", Value = "Hi Value"});
            _target.Items.Add(new SelectListItem {Text = "Bye", Value = "Bye Value"});

            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestToHtmlStringRenderSelectTagOptionsWithSelectedValues()
        {
            var expected = "<select>" +
                           "<option value=\"First Value\">First</option>" +
                           "<option selected=\"selected\" value=\"Second Value\">Second</option>" +
                           "<option selected=\"selected\" value=\"Third Value\">Third</option>" +
                           "</select>";
            _target.Items.Add(new SelectListItem {Text = "First", Value = "First Value"});
            _target.Items.Add(new SelectListItem {Text = "Second", Value = "Second Value"});
            _target.Items.Add(new SelectListItem {Text = "Third", Value = "Third Value"});

            _target.SelectedValues.Add("Second Value");
            _target.SelectedValues.Add("Third Value");

            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestToHtmlStringIgnoresTheSelectedPropertyOfTheItemsObjectsAndOnlyUsesTheSelectedValuesProperty()
        {
            var expected = "<select>" +
                           "<option value=\"First Value\">First</option>" +
                           "<option selected=\"selected\" value=\"Second Value\">Second</option>" +
                           "<option value=\"Third Value\">Third</option>" +
                           "</select>";
            _target.Items.Add(new SelectListItem {Text = "First", Value = "First Value"});
            _target.Items.Add(new SelectListItem {Text = "Second", Value = "Second Value"});

            // This Selected = true should be completely ignored
            _target.Items.Add(new SelectListItem {Text = "Third", Value = "Third Value", Selected = true});

            _target.SelectedValues.Add("Second Value");

            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestToHtmlStringHtmlEscapesTheTextValuesForOptionTags()
        {
            var expected = "<select><option value=\"0\">&gt;</option></select>";
            _target.Items.Add(new SelectListItem {Text = ">", Value = "0"});

            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
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
