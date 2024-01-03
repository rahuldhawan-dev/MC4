using System;
using System.Collections.Specialized;
using System.IO;
using System.Web.UI;
using MapCall.Common.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Controls
{
    /// <summary>
    /// Summary description for DateTimePickerTest
    /// </summary>
    [TestClass]
    public class DateTimePickerTest
    {
        private TestDateTimePicker InitializeBuilder()
        {
            // Control needs to be on a Page for stuff like ClientID
            // to work properly. So I'm just gonna pop it in a page here.
            var p = new Page();
            var target = new TestDateTimePicker();
            p.Controls.Add(target);
            return target;
        }

        #region Property Tests

        #region ClientID

        [TestMethod]
        public void TestClientIDReturnsSomeValueWhenIDHasNotBeenSet()
        {
            var target = InitializeBuilder();
            Assert.IsNull(target.ID);
            var result = target.ClientID;
            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        [TestMethod]
        public void TestClientIDReturnsSomeValueWhenEffectiveClientIDModeIsStatic()
        {
            var target = InitializeBuilder();
            target.ClientIDMode = ClientIDMode.Static;
            var result = target.ClientID;
            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }

        #endregion

        #region CurrentDateFormat

        [TestMethod]
        public void TestCurrentDateFormatDoesNotReturnTimeWhenShowTimePickerIsFalse()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = false;

            var expected = DateTimePicker.DefaultValues.NET_DATE_FORMAT;
            var result = target.CurrentDateFormat;

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestCurrentDateFormatReturnsTimeWhenShowTimePickerIsTrue()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = true;

            var expected = DateTimePicker.DefaultValues.NET_DATE_FORMAT + " " +
                           DateTimePicker.DefaultValues.NET_TIME_FORMAT;
            var result = target.CurrentDateFormat;

            Assert.AreEqual(expected, result);
        }

        #endregion

        #region SelectedDate

        [TestMethod]
        public void TestSelectedDateIsStoredToViewState()
        {
            var target = InitializeBuilder();
            var expected = new DateTime(1984, 4, 24);
            target.SelectedDate = expected;

            var result =
                target.ViewStateTest[DateTimePicker.SELECTED_DATE_VIEWSTATE_KEY];

            Assert.AreEqual(expected, result);
        }

        #endregion

        #region SelectedDateAsString

        [TestMethod]
        public void TestSelectedDateAsStringOnlyReturnsDateFormatIfShowTimePickerIsFalse()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = false;
            target.SelectedDate = new DateTime(1984, 4, 24, 1, 1, 1);

            var expected = "04/24/1984";
            var result = target.SelectedDateAsString;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestSelectedDateAsStringReturnsDateAndTimeFormatIfShowTimePickerIsTrue()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = true;
            target.SelectedDate = new DateTime(1984, 4, 24, 4, 4, 1);

            var expected = "04/24/1984 04:04 AM";
            var result = target.SelectedDateAsString;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestSelectedDateAsStringIsInAMPMFormat()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = true;
            target.SelectedDate = new DateTime(1984, 4, 24, 23, 1, 1);

            var expected = "04/24/1984 11:01 PM";
            var result = target.SelectedDateAsString;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestSelectedDateAsStringReturnsNullWhenSelectedDateIsNull()
        {
            var target = InitializeBuilder();
            target.SelectedDate = null;
            Assert.IsNull(target.SelectedDateAsString);
        }

        [TestMethod]
        public void TestSelectedDateAsStringReturnsExpectedValueForDoubleDigitNumbers()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = true;
            target.SelectedDate = new DateTime(1984, 12, 24, 23, 10, 23);

            var expected = "12/24/1984 11:10 PM";
            var result = target.SelectedDateAsString;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestSelectedDateAsStringDoesNotReturnSeconds()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = true;
            target.SelectedDate = new DateTime(1984, 4, 24, 23, 1, 1);

            var expected = "04/24/1984 11:01 PM";
            var result = target.SelectedDateAsString;
            Assert.AreEqual(expected, result);
        }

        #endregion

        #region ShowTimePicker

        [TestMethod]
        public void TestShowTimePickerIsTrueWhenSetToTrue()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = true;
            Assert.IsTrue(target.ShowTimePicker);
        }

        [TestMethod]
        public void TestShowTimePickerIsFalseWhenSetToFalse()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = false;
            Assert.IsFalse(target.ShowTimePicker);
        }

        #endregion

        #endregion

        #region Method Tests

        #region Constructor

        [TestMethod]
        public void TestShowCalendarButtonIsTrueByDefault()
        {
            var target = InitializeBuilder();
            Assert.IsTrue(target.ShowCalendarButton);
        }

        #endregion

        #region BuildInitScript

        [TestMethod]
        public void TestJqueryAndNotDollarSignUsed()
        {
            var target = InitializeBuilder();
            var result = target.BuildInitScriptTest();
            Assert.IsTrue(result.StartsWith("jQuery"));
        }

        [TestMethod]
        public void TestJQueryDatePickerUsedWhenShowTimePickerIsFalse()
        {
            var target = InitializeBuilder();
            target.ID = "target";
            target.ShowTimePicker = false;

            var expected = "jQuery(\"#target\").datepicker({\"showOn\":\"both\"});";
            var result = target.BuildInitScriptTest();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestJQueryTimePickerUsedWhenShowTimePickerIsTrue()
        {
            var target = InitializeBuilder();
            target.ID = "target";
            target.ShowTimePicker = true;

            var expected = "jQuery(\"#target\").datetimepicker({\"showOn\":\"both\",\"ampm\":true});";
            var result = target.BuildInitScriptTest();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestChangeMonthTrueIsInJQueryArgumentsWhenShowMonthChangeDropDownIsTrue()
        {
            var target = InitializeBuilder();
            target.ID = "target";
            target.ShowMonthChangeDropDown = true;
            var expected = "jQuery(\"#target\").datepicker({\"showOn\":\"both\",\"changeMonth\":true});";
            var result = target.BuildInitScriptTest();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestChangeMonthTrueIsNotInJQueryArgumentsWhenShowMonthChangeDropDownIsTrue()
        {
            var target = InitializeBuilder();
            target.ID = "target";
            target.ShowTimePicker = false;
            target.ShowMonthChangeDropDown = false;

            var expected = "jQuery(\"#target\").datepicker({\"showOn\":\"both\"});";
            var result = target.BuildInitScriptTest();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestChangeYearTrueIsInJQueryArgumentsWhenShowYearChangeDropDownIsTrue()
        {
            var target = InitializeBuilder();
            target.ID = "target";
            target.ShowTimePicker = false;
            target.ShowYearChangeDropDown = true;
            var expected = "jQuery(\"#target\").datepicker({\"showOn\":\"both\",\"changeYear\":true});";
            var result = target.BuildInitScriptTest();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestChangeYearTrueIsNotInJQueryArgumentsWhenShowYearChangeDropDownIsTrue()
        {
            var target = InitializeBuilder();
            target.ID = "target";
            target.ShowYearChangeDropDown = false;

            var expected = "jQuery(\"#target\").datepicker({\"showOn\":\"both\"});";
            var result = target.BuildInitScriptTest();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestShowOnBothIsNotInJQueryArgumentsWhenShowCalendarButtonIsFalse()
        {
            var target = InitializeBuilder();
            target.ID = "target";
            target.ShowCalendarButton = false;

            var expected = "jQuery(\"#target\").datepicker({});";
            var result = target.BuildInitScriptTest();
            Assert.AreEqual(expected, result);
        }

        #endregion

        #region LoadPostData

        [TestMethod]
        public void TestLoadPostDataReturnsFalseIfEnabledIsFalse()
        {
            var target = InitializeBuilder();
            target.Enabled = false;
            Assert.IsFalse(target.LoadPostData(null, null));
        }

        [TestMethod]
        public void TestLoadPostDataReturnsFalseIfSelectedDateIsSameAsPostBackValue()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = false;
            var postBackData = new NameValueCollection();
            postBackData["target"] = "04/24/1984";
            target.SelectedDate = new DateTime(1984, 4, 24);
            Assert.IsFalse(target.LoadPostData("target", postBackData));
        }

        [TestMethod]
        public void TestLoadPostDataReturnsTrueIfSelectedDateIsNotEqualToPostBackValue()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = false;
            var postBackData = new NameValueCollection();
            postBackData["target"] = "04/24/2084";
            target.SelectedDate = new DateTime(1984, 4, 24);
            Assert.IsTrue(target.LoadPostData("target", postBackData));
        }

        [TestMethod]
        public void TestLoadPostDataSetsSelectedDateWhenItReturnsTrue()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = false;
            var postBackData = new NameValueCollection();
            postBackData["target"] = "04/24/2084";
            target.SelectedDate = new DateTime(1984, 4, 24);
            Assert.IsTrue(target.LoadPostData("target", postBackData));
            Assert.AreEqual(new DateTime(2084, 4, 24), target.SelectedDate);
        }

        #endregion

        #region OnSelectedDateChanged

        [TestMethod]
        public void TestOnSelectedDateChangedRaiseEvent()
        {
            var target = InitializeBuilder();
            var ivebeenhit = false;
            target.SelectedDateChanged += (sender, e) => { ivebeenhit = true; };

            target.OnSelectedDateChangedTest();
            Assert.IsTrue(ivebeenhit);
        }

        #endregion

        #region ParseDateFromString

        [TestMethod]
        public void TestParseDateFromStringReturnsNullDateForNullString()
        {
            var target = InitializeBuilder();
            var result = target.ParseDateFromString(null);
            Assert.IsFalse(result.HasValue);
        }

        [TestMethod]
        public void TestParseDateFromStringReturnsNullDateForEmptyString()
        {
            var target = InitializeBuilder();
            var result = target.ParseDateFromString(string.Empty);
            Assert.IsFalse(result.HasValue);
        }

        [TestMethod]
        public void TestParseDateFromStringReturnsNullDateForWhiteSpaceString()
        {
            var target = InitializeBuilder();
            var result = target.ParseDateFromString("    ");
            Assert.IsFalse(result.HasValue);
        }

        [TestMethod]
        public void TestParseDateFromStringReturnsExpectedDateWhenShowTimePickerIsFalse()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = false;

            var expected = new DateTime(1984, 4, 24);
            var result = target.ParseDateFromString("04/24/1984");
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(expected, result.Value);
        }

        [TestMethod]
        public void TestParseDateFromStringReturnsExpectedDateWhenShowTimePickerIsFalseAndValueHasNoLeadingZeros()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = false;

            var expected = new DateTime(1984, 4, 4);
            var result = target.ParseDateFromString("4/4/1984");
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(expected, result.Value);
        }

        [TestMethod]
        public void TestParseDateFromStringReturnsExpectedDateWhenTwoDigitYearIsInString()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = false;

            var expected = new DateTime(2011, 4, 4);
            var result = target.ParseDateFromString("4/4/11");
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(expected, result.Value);
        }

        [TestMethod]
        public void TestParseDateFromStringIgnoresTimeIfShowTimePickerIsFalse()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = false;

            var expected = new DateTime(1984, 4, 24);
            var result = target.ParseDateFromString("04/24/1984 1:01 PM");
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(expected, result.Value);
        }

        [TestMethod]
        public void TestParseDateFromStringIncludesTimeWhenShowTimePickerIsTrue()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = true;

            var expected = new DateTime(1984, 4, 24, 13, 1, 0);
            var result = target.ParseDateFromString("04/24/1984 1:01 PM");
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(expected, result.Value);
        }

        [TestMethod]
        public void TestParseDateFroMStringCanDealWithMissingSpaceBetweenTimeAndAMPM()
        {
            var target = InitializeBuilder();
            target.ShowTimePicker = true;

            var expected = new DateTime(1984, 4, 24, 13, 1, 0);
            var result = target.ParseDateFromString("04/24/1984 1:01PM");
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(expected, result.Value);
        }

        #endregion

        #region RaisePostDataChangedEvent

        [TestMethod]
        public void TestRaisePostDataChangedEventRaisesSelectedDateChangedEvent()
        {
            var target = InitializeBuilder();
            var ivebeenhit = false;
            target.SelectedDateChanged += (sender, e) => { ivebeenhit = true; };

            target.RaisePostDataChangedEvent();
            Assert.IsTrue(ivebeenhit);
        }

        #endregion

        #region Render

        [TestMethod]
        public void TestRenderWritesProperControl()
        {
            var target = InitializeBuilder();
            target.ID = "yarr";
            var expected = "<input id=\"yarr\" name=\"yarr\" class=\"dateTimePicker\" type=\"text\" />";

            using (var tw = new StringWriter())
            using (var html = new HtmlTextWriter(tw))
            {
                target.RenderTest(html);
                var result = tw.ToString();
                Assert.AreEqual(expected, result);
            }
        }

        [TestMethod]
        public void TestRenderWritesProperControlWhenSelectedDateHasValue()
        {
            var target = InitializeBuilder();
            target.ID = "yarr";
            target.SelectedDate = new DateTime(1984, 4, 24);
            var expected =
                "<input id=\"yarr\" name=\"yarr\" class=\"dateTimePicker\" type=\"text\" value=\"04/24/1984\" />";

            using (var tw = new StringWriter())
            using (var html = new HtmlTextWriter(tw))
            {
                target.RenderTest(html);
                var result = tw.ToString();
                Assert.AreEqual(expected, result);
            }
        }

        #endregion

        #endregion

        #region Misc Tests

        [TestMethod]
        public void TestDateTimePickerClassHasValidationPropertyAttribute()
        {
            var target =
                typeof(DateTimePicker).GetCustomAttributes(
                    typeof(ValidationPropertyAttribute), false);

            Assert.IsNotNull(target);
            Assert.IsTrue(target.Length == 1);
        }

        [TestMethod]
        public void TestDateTimePickerValidationPropertyAttributeIsSetToSelectedDate()
        {
            var target =
                typeof(DateTimePicker).GetCustomAttributes(
                    typeof(ValidationPropertyAttribute), false);

            var vpa = (ValidationPropertyAttribute)target[0];
            Assert.AreEqual("SelectedDate", vpa.Name);
        }

        #endregion
    }

    public class TestDateTimePicker : DateTimePicker
    {
        #region Test Properties

        public StateBag ViewStateTest
        {
            get { return ViewState; }
        }

        #endregion

        public string BuildInitScriptTest()
        {
            return BuildInitScript();
        }

        public void OnSelectedDateChangedTest()
        {
            OnSelectedDateChanged();
        }

        public void OnPreRenderTest()
        {
            OnPreRender(EventArgs.Empty);
        }

        public void RenderTest(HtmlTextWriter writer)
        {
            Render(writer);
        }
    }
}
