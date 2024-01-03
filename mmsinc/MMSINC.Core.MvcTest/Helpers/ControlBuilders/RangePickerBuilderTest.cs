using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.EnumExtensions;
using MMSINC.Data;
using MMSINC.Helpers;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MMSINC.Core.MvcTest.Helpers.ControlBuilders
{
    [TestClass]
    public class RangePickerBuilderTest
    {
        #region Fields

        private RangePickerBuilder _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new RangePickerBuilder();
            _target.StartBuilder = new TextBoxBuilder();
            _target.EndBuilder = new TextBoxBuilder();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestStartBuilderIsNullByDefault()
        {
            Assert.IsNull(new RangePickerBuilder().StartBuilder);
        }

        [TestMethod]
        public void TestEndBuilderIsNullByDefault()
        {
            Assert.IsNull(new RangePickerBuilder().EndBuilder);
        }

        [TestMethod]
        public void TestConstructorCreatesDropDownSelectListBuilderForOperatorBuilderWithOperatorItems()
        {
            var target = new RangePickerBuilder();
            Assert.IsNotNull(target.OperatorBuilder);
            Assert.AreEqual(SelectListType.DropDown, target.OperatorBuilder.Type);

            var operators = (RangeOperator[])Enum.GetValues(typeof(RangeOperator));
            foreach (var op in operators)
            {
                Assert.IsTrue(
                    target.OperatorBuilder.Items.Any(x =>
                        x.Text == op.DescriptionAttr() && x.Value == ((int)op).ToString()));
            }
        }

        [TestMethod]
        public void TestToHtmlStringThrowsExceptionIfEndBuilderIsNull()
        {
            _target.EndBuilder = null;
            MyAssert.Throws<InvalidOperationException>(() => _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringThrowsExceptionIfStartBuilderIsNull()
        {
            _target.StartBuilder = null;
            MyAssert.Throws<InvalidOperationException>(() => _target.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringRendersExpectedOutput()
        {
            var expected = @"<div class=""range""><input class=""range-start"" type=""text"" value="""" /> " +
                           @"<select class=""range-operator""><option value=""0"">Between</option><option value=""1"">=</option><option value=""2"">&gt;</option><option value=""3"">&gt;=</option><option value=""4"">&lt;</option><option value=""5"">&lt;=</option></select> " +
                           @"<input class=""range-end"" type=""text"" value="""" /></div>";
            var result = _target.ToHtmlString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestToHtmlStringRemovesNameAttributeFromWrapperDiv()
        {
            _target.Name = "blah blah blah";
            var result = _target.ToHtmlString();
            StringAssert.StartsWith(result, @"<div class=""range"">");
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
