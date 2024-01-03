using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Helpers;
using MMSINC.Testing.ClassExtensions.StringExtensions;
using MMSINC.Testing.SeleniumMvc;
using MMSINC.Testing.SpecFlow.Library;
using MMSINC.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using TechTalk.SpecFlow;
using ObjectExtensions = MMSINC.ClassExtensions.ObjectExtensions.ObjectExtensions;

namespace MMSINC.Testing.SpecFlow.StepDefinitions
{
    [Binding]
    public static class State
    {
        #region Constants

        public static readonly FormatHelper FORMAT_HELPER = new FormatHelper {
            {"date", "{0:d}"},
            {"date without seconds", CommonStringFormats.DATETIME_WITHOUT_SECONDS},
            {"short date", "{0:MM/dd/yy}"},
            {"short date with full year", CommonStringFormats.DATE},
            {"decimal rounded to two places", "{0:0.00}"},
            {"currency", "{0:c}"}
        };

        public const string SORTING_TEXT = " ˄ ˅ ",
                            SORTING_ASCENDING_TEXT = SortHelper.SORT_ASC_SYMBOL,
                            SORTING_DESCENDING_TEXT = SortHelper.SORT_DESC_SYMBOL;

        public static readonly Regex NAMED_TYPE = new Regex("([^\"]+): \"([^\"]+)\""),
                                     NAMED_TYPE_PROPERTY = new Regex("([^\"]+): \"([^\"]+)\"'s ([^\"]+)");

        #endregion

        #region Private Methods

        public static void AssertThereIsNoYSOD(string errorMessage)
        {
            // Checking PageSource for this because the YSOD screen will not have jQuery.
            var pageSource = WebDriverHelper.Current.PageSource;
            if (pageSource.Contains("Server Error in"))
            {
                Assert.Fail(errorMessage + Environment.NewLine + pageSource);
            }
        }

        private static IBetterWebElement TryToFindSelectOptionByText(string select, string text)
        {
            var elem = WebDriverHelper.Current.FindElement(By.Id(select));
            // pair down consecutive whitespace, because the browser does it when rendering options
            text = new Regex("[ ]{2}").Replace(text, " ").Trim();
            return elem
                  .GetSelectOptions()
                  .SingleOrDefault(o => o.Text.Trim() == text || o.GetValue().Trim() == text);
        }

        #endregion

        #region Step Definitions

        [Given("I do not currently function")]
        public static void IDoNotCurrentlyWork()
        {
            // Use this step for regression tests that need to exist, do not work, but shouldn't fail.
            // Do not remove this from the app.config:  <runtime missingOrPendingStepsOutcome="Error" />
            // Otherwise tests will pass when steps are missing/refactored when they should actually be
            // notifying us that they're not working.
            Assert.Inconclusive("This test does not currently function correctly.");
        }

        #region Page Content

        [Then("^I should see \"([^\"]*)\"$")]
        public static void ThenIShouldSeeText(string text)
        {
            // NOTE: THIS STEP CAN BE SLOW DEPENDING ON THE NUMBER OF ELEMENTS ON A PAGE, YOU SHOULD PROBABLY NOT USE THIS.

            // We need to remove multiple spaces/line breaks because they're included when trying to get an element's
            // text, even though browsers do not display multiple spaces.

            var currentPageText = WebDriverHelper.Current.Text;

            if (!currentPageText.Contains(text))
            {
                Assert.Fail($"The text '{text}' was not found on the page as expected.  Page text: {currentPageText}");
            }
            else
            {
                var elementWithText = WebDriverHelper.Current.FindElements(By.XPath($"//*[contains(text(), '{text}')]"));

                if (!elementWithText.Any())
                {
                    // If you're experiencing this problem, use the "I should see "text" in the bluh element" step instead.
                    // This fails because the text is split up in multiple tags and ByHelper.PartialTextInTag does not work with that.
                    throw new InvalidOperationException(
                        $"Somehow the text \"{text}\" is on the page but not in a single element.");
                }

                var isDisplayed = elementWithText.Any(x => x.IsDisplayed);
                Assert.IsTrue(isDisplayed, $"The text '{text}' was found on the current page but is not visible.");
            }
        }

        [Obsolete("Don't use this. You should use 'I should (only|at least) see'")]
        [Then("^I should see \"([^\"]*)\" in the ([^\"]+) element$")]
        public static void ThenIShouldSeeTextInElement(string text, string elementId)
        {
            var element = WebDriverHelper.Current.FindElement(By.Id(elementId));
            Assert.IsTrue(element.Text.Contains(text));
            Assert.IsTrue(element.IsDisplayed, $"The {elementId} is not visible.");
        }

        [Then("^I should see \"([^\"]*)\" in the ([^\"]+) frame")]
        public static void ThenIShouldSeeTextInFrame(string text, string frame)
        {
            WebDriverHelper.Current.SwitchToFrame(frame);
            try
            {
                ThenIShouldSeeText(text);
            }
            finally
            {
                WebDriverHelper.Current.LeaveFrame();
            }

            //  var element = WebDriverHelper.Current.FindElement(By.Id(elementId));
            //   Assert.IsTrue(element.Text.Contains(text));
            //   Assert.IsTrue(element.IsDisplayed, $"The {elementId} is not visible.");
        }

        [Then("^I should see the current hostname and \"([^\"]*)\"$")]
        public static void ThenIShouldSeeTheCurrentHostnameAndText(string text)
        {
            ThenIShouldSeeText(text.PrependCurrentHostname());
        }

        #region See display for

        private static string GetDisplayForText(string field)
        {
            // NOTE: This is attempting to parse our the displayed value from our DisplayFor template.
            //       If the DisplayFor template's ever significantly modified, this might break.
            var el = WebDriverHelper.Current.FindElements(ByHelper.Attribute("for", field))
                                    .SingleOrDefault(x => x.IsDisplayed);

            if (el == null)
            {
                WebDriverHelper.Current.CaptureScreenshot();
                Assert.Fail(
                    $"A label for the field '{field}' could not be found, so a matching display could also not be found.");
            }

            var fieldPair = el.FindElement(ByHelper.AncestorCssClass("field-pair"));
            var fieldEl = fieldPair.FindElement(By.ClassName("field"));
            return fieldEl.Text;
        }

        [Obsolete("Don't use this. Use the same step but with quotes around today's date.")]
        [Then("^I should see a display for ([^\"]+) with today's date$")]
        public static void ThenIShouldSeeTodaysDateInt(string field)
        {
            var displayedText = GetDisplayForText(field);
            var displayedValue = DateTime.Parse(displayedText).BeginningOfDay();
            Assert.AreEqual(DateTime.Now.BeginningOfDay(), displayedValue);
        }

        [Then("^I should see a display for ([^\"]+) with a date time close to now")]
        public static void ThenIShouldSeeADisplayForWithATimeValueCloseToNow(string field)
        {
            var displayedText = GetDisplayForText(field);
            var displayedValue = DateTime.Parse(displayedText);
            
            MyAssert.AreClose(DateTime.Now, displayedValue);
        }

        [Then(
            "^I should see a display for ([^\"]+) with escaped text ((?<![\\\\])['\"])((?:.(?!(?<![\\\\])\\2))*.?)\\2$")]
        public static void ThenIShouldSeeADisplayForWithEscapedTextIn(string field, string thing, string text)
        {
            text = text.Replace("\\r\\n", Environment.NewLine).Replace("\\\"", "\"").Trim();
            ThenIShouldSeeADisplayForWith(field, text);
        }

        [Given("^I can see a display for ([^\"]+) containing \"([^\"]*)\"$")]
        [When("^I see a display for ([^\"]+) containing \"([^\"]*)\"$")]
        [Then("^I should see a display for ([^\"]+) containing \"([^\"]*)\"$")]
        public static void ThenIShouldSeeADisplayForContaining(string field, string text)
        {
            text = (text ?? string.Empty).Replace("\\r\\n", Environment.NewLine).Trim();
            text = Input.ParseSpecialString(text);
            var fieldText = GetDisplayForText(field);
            Assert.IsTrue(fieldText.Contains(text),
                $"A display for the field '{field}' CONTAINING the text '{text}' could not be found on the current page. The display text instead says '{fieldText}'.");
        }
        
        [Given("^I can see a display for ([^\"]+) with \"([^\"]*)\"$")]
        [When("^I see a display for ([^\"]+) with \"([^\"]*)\"$")]
        [Then("^I should see a display for ([^\"]+) with \"([^\"]*)\"$")]
        public static void ThenIShouldSeeADisplayForWith(string field, string text)
        {
            text = (text ?? string.Empty).Replace("\\r\\n", Environment.NewLine).Trim();
            text = Input.ParseSpecialString(text);
            var fieldText = GetDisplayForText(field);
            Assert.AreEqual(text, fieldText,
                $"A display for the field '{field}' with the EXACT text '{text}' could not be found on the current page. The display text instead says '{fieldText}'.");
        }

        [Then("^I should see a display for ([^\"]+): \"([^\"]+)\"'s ([^\\s]+)")]
        public static void ThenIShouldSeeADisplayForThingsPropertysValue(string type, string name, string value)
        {
            // NOTE: This step does *not* work when the display field is coming from a nested model. Use the
            //       'I should see a display for Nested_Property with type "type name"'s Property' step instead.
            var namedItem = Data.GetCachedEntity(type, name);
            var text = (namedItem.GetFormattedPropertyValueByName(value) ?? String.Empty).ToString();

            // Need to convert .'s to _'s in order to find the label for nested property access.
            value = value.Replace('.', '_');

            ThenIShouldSeeADisplayForWith(value, text);
        }

        [Then("^I should see a display for ([^\"]+) with ([^\"]+) \"([^\"]+)\"$")]
        public static void ThenIShouldSeeANamedDataInDisplayFor(string field, string type, string name)
        {
            var text = Data.GetCachedEntityPropertyValue(type, name, "ToString");
            ThenIShouldSeeADisplayForWith(field, text);
        }

        [Then("^I should see a display for ([^\"]+) with ([^\"]+) \"([^\"]+)\"'s ([^\\s]+)$")]
        public static void ThenIShouldSeeNamedDataInDisplayFor(string field, string type, string name, string property)
        {
            var namedItem = TestObjectCache.Instance.Lookup(type, name);
            var text = property == "ToString"
                ? namedItem.ToString()
                : (namedItem.GetFormattedPropertyValueByName(property) ?? String.Empty).ToString();

            ThenIShouldSeeADisplayForWith(field, text);
        }

        /// <summary>
        /// When you need to use the ToString
        /// </summary>
        /// <param name="label"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        [Then("^I should see a display for \"([^\"]+)\" with ([^\"]+): \"([^\"]+)\"")]
        public static void ThenIShouldSeeADisplayLabelThingy(string label, string type, string name)
        {
            var text = Data.GetCachedEntity(type, name).ToString();
            ThenIShouldSeeADisplayForWith(label, text);
        }

        [Then("^I should see a display for \"([^\"]+)\" with ([^\"]+): \"([^\"]+)\"'s ([^\\s]+)")]
        public static void ThenIShouldSeeADisplayLabelThingy(string label, string type, string name, string property)
        {
            var text = Data.GetCachedEntityPropertyValue(type, name, property);
            ThenIShouldSeeADisplayForWith(label, text);
        }

        #endregion

        [Then("^I (should|should not) see ([^\"]+): \"([^\"]+)\"'s ([^\\s]+) on the page$")]
        public static void ThenIShouldSeeText(string nopteraptor, string type, string name, string property)
        {
            var should = StepHelper.IsTruthy(nopteraptor);
            // get the object from the cache
            var propValue = Data.GetCachedEntityPropertyValue(type, name, property);
            Assert.IsNotNull(propValue, $"{type} \"{name}\"'s {property} property is null, so there's nothing to see.");
            var text = propValue;

            if (should != WebDriverHelper.Current.PageSource.Contains(text))
            {
                Assert.Fail(
                    should
                        ? "The text '{0}' was not found on the page as expected.  Page text: {1}"
                        : "The text '{0}' was found on the page and should not have been.  Page text: {1}", text,
                    WebDriverHelper.Current.Text);
            }
        }

        /// <summary>
        /// If this is failing, make sure you are attempting to look at the full text. 
        /// It does not work with partial text.
        /// </summary>
        /// <param name="text"></param>
        [Then("^I should not see \"([^\"]+)\"$")]
        public static void ThenIShouldNotSeeText(string text)
        {
            var el = WebDriverHelper.Current.FindElements(ByHelper.PartialTextInTag(text));
            if (el.Any(x => x.IsDisplayed))
            {
                Assert.Fail($"The text '{text}' was unexpectedly found on the current page.");
            }
        }

        #region Drop Hugh Downs

        /// <summary>
        ///  If you're having trouble trying to match this because of DisplayItems or other weirdness
        ///  use - Then I should see foo "one"'s Id in the Foo dropdown
        /// </summary>
        /// <param name="nopteraptor"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="dropdown"></param>
        [Then("^I (should|should not) see ([^\"]+) \"([^\"]+)\" in the ([^\\s]+) dropdown$")]
        public static void ThenIShouldSeeOptionInDropDown(string nopteraptor, string type, string name, string dropdown)
        {
            ThenIShouldSeeOptionInDropDown(nopteraptor, type, name, "ToString", dropdown);
        }

        [Then("^I (should|should not) see ([^\"]+) \"([^\"]+)\"'s ([^\\s]+) in the ([^\\s]+) dropdown$")]
        public static void ThenIShouldSeeOptionInDropDown(string nopteraptor, string type, string name,
            string propertyName, string dropdown)
        {
            var value = Data.GetCachedEntityPropertyValue(type, name, propertyName);
            ThenIShouldSeeTextInTheSelect(nopteraptor, value, dropdown);
        }

        #endregion

        [Then(
            "the td elements in the ([0-9]+)(?:st|nd|rd|th) row of the \"([^\"]*)\" table should have a \"([^\"]*)\" value of \"([^\"]*)\"")]
        public static void TheTDElementsInANumberedRowOfTheTableHasTheCSSPropertyWithValue(int rowIndex, string tableId,
            string cssProperty, string value)
        {
            var row = Input.FindTableRowByIndex(rowIndex, tableId);
            foreach (var td in row.FindElements(By.TagName("td")))
            {
                var actualParsedValue = td.GetCssValue(cssProperty);
                if (new Regex(RegularExpressions.HEX_COLOR_VALUE).IsMatch(value))
                { // Do we have a hex value - i.e. Color, e.g. #F0F0F0
                    var rgb = new Regex(RegularExpressions.RGB_COLOR_VALUE).Match(actualParsedValue).Groups;
                    if (rgb.Count >= 3)
                    {
                        Assert.AreEqual(value.ToUpper(),
                            $"#{int.Parse(rgb[1].Value).ToString("X")}{int.Parse(rgb[2].Value).ToString("X")}{int.Parse(rgb[3].Value).ToString("X")}",
                            $"{cssProperty} was not equal to {value} for td with text of {td.Text}");
                    }
                    else
                    {
                        Assert.Fail(
                            $"Unable to parse the correct color, there weren't 3 values for RGB: {actualParsedValue}");
                    }
                }
                else
                {
                    Assert.AreEqual(value, actualParsedValue,
                        $"{cssProperty} was not equal to {value} for td with text of {td.Text}");
                }
            }
        }

        [Then("The element ([^\\s]+) should have the attribute ([^\\s]+) with ([^\"]+) \"([^\"]+)\"'s ([^\\s]+)")]
        public static void ThenElementShouldHaveTheAttributeWithObjectsProperty(string elementId, string attribute,
            string type, string name, string propertyName)
        {
            var el = WebDriverHelper.Current.FindElement(By.Id(elementId));
            var attrValue = el.GetAttribute(attribute);
            Assert.IsNotNull(attrValue,
                $"The element '{elementId}' does not have the attribute '{attribute}' or the attribute does not have a value.");
            var value = Data.GetCachedEntityPropertyValue(type, name, propertyName);
            Assert.IsNotNull(value, $"The property '{propertyName}' is null.");
            Assert.AreEqual(value, attrValue);
        }

        [Given("^I can see \"([^\"]*)\" in the ([^\\s]+) field$")]
        [When("^I see \"([^\"]*)\" in the ([^\\s]+) field$")]
        [Then("^I should see \"([^\"]*)\" in the ([^\\s]+) field$")]
        public static void ThenIShouldSeeTextInControl(string text, string field)
        {
            var actual = GetValueInField(field);
            // if we're expecting an empty string, 'actual' will be null
            text = String.IsNullOrEmpty(text) ? string.Empty : text;

            if (text != string.Empty)
            {
                text = Input.ParseSpecialString(text);
            }

            Assert.AreEqual(text, actual, $"{text} != {actual}");
        }

        [Then("^I should(?:n't| not) see \"([^\"]*)\" in the ([^\\s]+) field$")]
        public static void ThenIShouldNotSeeTextInControl(string text, string field)
        {
            var actual = GetValueInField(field);
            // if we're expecting an empty string, 'actual' will be null
            text = String.IsNullOrEmpty(text) ? null : text;

            if (text != null)
            {
                text = Input.ParseSpecialString(text);
            }

            Assert.AreNotEqual(text, actual, $"{text} != {actual}");
        }

        [Given("^I can see ([^\"]+) \"([^\"]+)\"'s ([^\"]+) in the ([^\\s]+) field$")]
        [When("^I see ([^\"]+) \"([^\"]+)\"'s ([^\"]+)in the ([^\\s]+) field$")]
        [Then("^I should see ([^\"]+) \"([^\"]+)\"'s ([^\"]+) in the ([^\\s]+) field$")]
        public static void ISeeNamedValueInField(string type, string name, string property, string field)
        {
            var text = GetRawValue(type, name, property);
            ThenIShouldSeeTextInControl(Convert.ToString(text), field);
        }

        [Then("^I (should|should not) see \"([^\"]*)\" in the ([^\\s]+) (?:select|dropdown)$")]
        public static void ThenIShouldSeeTextInTheSelect(string nopteraptor, string text, string select)
        {
            WebDriverHelper.Current.WaitForAjaxToComplete();

            var should = StepHelper.IsTruthy(nopteraptor);
            IBetterWebElement option;

            try
            {
                option = TryToFindSelectOptionByText(select, text);
            }
            catch (StaleElementReferenceException)
            {
                // working around a fluke error where element is not attached to the page document
                option = TryToFindSelectOptionByText(select, text);
            }

            if ((option != null) != should)
            {
                Assert.Fail(
                    should
                        ? "Unable to find option '{0}' in list '{1}'"
                        : "Unexpectedly found option '{0}' in list '{1}'", text, @select);
            }
        }

        [Given("I (can|can not|can't) see the ([^\\s]+) (?:element|field)$")]
        [Then("^I (should|should not|shouldn't) see the ([^\\s]+) (?:element|field)$")]
        public static void ThenIMightSeeTheElement(string nopterator, string element)
        {
            var should = StepHelper.IsTruthy(nopterator);
            var actual = WebDriverHelper.Current.FindElements(By.Id(element)).SingleOrDefault();

            if (should && (actual == null || !actual.IsDisplayed))
            {
                Assert.Fail($"The element {element} should be visible but is not.");
            }
            else if (!should && (actual != null && actual.IsDisplayed))
            {
                Assert.Fail($"The element {element} should not be visible but is.");
            }
        }

        [Then("^I should (only|at least) see \"?([^\"]+)\"? in the ([^\\s]+) element$")]
        public static void ThenIShouldSeeTextInTheElement(string arg, string text, string element)
        {
            Assert.IsNotNull(text, $"You wanna find null text in the element '{element}'? You're silly.");
            text = text.Trim(); // I hate whitespace.
            text = Input.ParseSpecialString(text); // in case this is a special datetime value or w/e
            var actual = WebDriverHelper.Current.FindElement(By.Id(element));
            Assert.IsNotNull(actual.Text, $"Can not find text '{text}' in element '{element}'. Actual text: null");
            var actualText = actual.Text.Trim(); // I hate whitespace here, too.
            switch (arg)
            {
                case "only":
                    Assert.AreEqual(actualText, text,
                        $"Can not find text '{text}' in element '{element}'. Actual text: '{actual.Text}'");
                    break;
                case "at least":
                    Assert.IsTrue(actualText.Contains(text),
                        $"Can not find text '{text}' in element '{element}'. Actual text: '{actual.Text}'");
                    break;
            }
        }

        [Given("^I (can not|can't) see \"([^\"]+)\" in the ([^\\s]+) element$")]
        [When("^I (can not|can't) see \"([^\"]+)\" in the ([^\\s]+) element$")]
        [Then("^I (should not|shouldn't) see \"([^\"]+)\" in the ([^\\s]+) element$")]
        public static void ThenIShouldNotSeeTextInTheElement(string arg, string text, string element)
        {
            Assert.IsNotNull(text, $"You wanna find null text in the element '{element}'? You're silly.");
            text = text.Trim(); // I hate whitespace.
            var actual = WebDriverHelper.Current.FindElements(By.Id(element)).SingleOrDefault();
            Assert.IsTrue(actual != null, $"Can not find element with id '{element}'.");

            if (!string.IsNullOrWhiteSpace(actual.Text))
            {
                Assert.IsFalse(actual.Text.Contains(text),
                    $"Found text '{text}' in element '{element}' when you didn't want to. Actual text: '{actual.Text}'");
            }
        }

        [Then("^I should (only|at least) see ([^\"]+) \"([^\"]+)\"'s ([^\\s]+) in the ([^\\s]+) element$")]
        public static void ThenIShouldSeeSuchAndSuchesThingInTheElement(string arg, string type, string name,
            string propertyName, string element)
        {
            // get the object from the cache
            var value = Data.GetCachedEntityPropertyValue(type, name, propertyName);
            Assert.IsNotNull(value, $"{type}.{propertyName} is null. You should probably set a value on it.");
            ThenIShouldSeeTextInTheElement(arg, value.ToString(), element);
        }

        [Then(
            "^I should (only|at least) see ([^\"]+) \"([^\"]+)\"'s ([^\\s]+) in the ([^\\s]+) element as(?: a)? ((?!in t).*)$")]
        public static void ThenIShouldSeeSuchAndSuchesThingInTheElementFormattedAs(string arg, string type, string name,
            string propertyName, string element, string format)
        {
            var rawValue = GetRawValue(type, name, propertyName);
            rawValue.ThrowIfNull("rawValue");
            var formattedText = FORMAT_HELPER.FormatValue(rawValue, format);

            ThenIShouldSeeTextInTheElement(arg, formattedText, element);
        }

        [Given("^the (.+) field is (checked|unchecked)")]
        [When("^the (.+) field is (checked|unchecked)")]
        [Then("^the (.+) field should be (checked|unchecked)")]
        public static void ThenTheFieldShouldBeChecked(string fieldName, string isChecked)
        {
            var chk = ElementHelper.FindElementByIdNameOrSelector(fieldName);
            
            if (isChecked == "checked" && !chk.IsChecked)
            {
                Assert.Fail($"Field '{fieldName}' expected to be checked but was not checked.");
            }
            else if (isChecked == "unchecked" && chk.IsChecked)
            {
                Assert.Fail($"Field '{fieldName}' expected to be unchecked but was checked");
            }
        }

        [Then("^([^\"]+) \"([^\"]+)\" (should|should not) be selected in the ([^\\s]+) (dropdown|multiselect)")]
        public static void ThenNamedItemShouldBeSelectedInTheDropDown(string type, string name, string nopteraptor,
            string dropdown, string dropOrMulti)
        {
            var value = Data.GetCachedEntityPropertyValue(type, name, "Id");
            ThenOptionWithValueShouldMaybeBeSelectedForTheSelectElement(value, nopteraptor, dropdown, dropOrMulti);
        }

        [Then("^([^\"]+) \"([^\"]+)\"'s ([^\\s]+) (should|should not) be selected in the ([^\\s]+) dropdown$")]
        public static void ThenNamedDataShouldBeSelectedInTheDropDown(string type, string name, string propertyName,
            string nopteraptor, string dropdown)
        {
            var value = Data.GetCachedEntityPropertyValue(type, name, propertyName);
            ThenStringShouldBeSelectedInTheDropDown(value, nopteraptor, dropdown);
        }

        [Given("^\"([^\"]+)\" (is|isn't) selected in the ([^\\s]+) dropdown$")]
        [When("^\"([^\"]+)\" (is|isn't) selected in the ([^\\s]+) dropdown$")]
        [Then("^\"([^\"]+)\" (should|should not) be selected in the ([^\\s]+) dropdown$")]
        public static void ThenStringShouldBeSelectedInTheDropDown(string value, string nopteraptor, string dropdown)
        {
            // Chrome Driver or Chrome V89 has a vendetta, so now we have to trim the strings because its randomly putting a space after the value
            //IE "HAB-1" is now "HAB-1 "
            value = value?.Trim();
            var elem = WebDriverHelper.Current.FindElement(By.Id(dropdown));
            var selectedText = elem.GetSelectedOptionText()?.Trim();
            var should = StepHelper.IsTruthy(nopteraptor);

            if (should)
            {
                Assert.AreEqual(value, selectedText,
                    $"Option with the text '{value}' within the drop down '{dropdown}' was found, but is not currently selected.");
            }
            else
            {
                Assert.AreNotEqual(value, selectedText,
                    $"Option with the text '{value}' within the drop down '{dropdown}' was found and is currently selected.");
            }
        }

        [Then("^(\\d+) (should|should not) be selected in the ([^\\s]+) (dropdown|multiselect)$")]
        public static void ThenOptionWithValueShouldMaybeBeSelectedForTheSelectElement(string id, string nopteraptor, string selectElementId, string dropOrMulti)
        {
            var should = StepHelper.IsTruthy(nopteraptor);
            var elem = WebDriverHelper.Current.FindElement(By.Id(selectElementId));

            switch (dropOrMulti)
            {
                case "dropdown":
                    var selectedValue = elem.GetSelectedOptionValue();
                    if (should)
                    {
                        Assert.AreEqual(id, selectedValue,
                            $"Option with the value '{id}' within the drop down '{selectElementId}' was found, but is not currently selected.");
                    }
                    else
                    {
                        Assert.AreNotEqual(id, selectedValue,
                            $"Option with the text '{id}' within the drop down '{selectElementId}' was found and is currently selected.");
                    }

                    break;

                case "multiselect":
                    var selectedValues = elem.GetAllSelectedOptionValues();
                    var selectedValuesForError = string.Join(", ", selectedValues);
                    var hasValue = selectedValues.Contains(id);
                    
                    if (should)
                    {
                        Assert.IsTrue(hasValue,
                            $"Option with the value '{id}' within the multiselect '{selectElementId}' was found, but is not currently selected. Currently selected: {selectedValuesForError}");
                    }
                    else
                    {
                        Assert.IsFalse(hasValue,
                            $"Option with the text '{id}' within the drop down '{selectElementId}' was found and is currently selected. Currently selected: {selectedValuesForError}");
                    }
                    break;
            }
        }

        #region CheckBoxLists

        [Then("^I (should|should not) see \"([^\"]*)\" in the ([^\\s]+) checkbox list$")]
        public static void IMightSeeCheckBoxWithTextOrValueInCheckboxList(string nopteraptor, string text,
            string select)
        {
            WebDriverHelper.Current.WaitForAjaxToComplete();
            var elem = WebDriverHelper.Current.FindElement(By.Id(select));
            var should = StepHelper.IsTruthy(nopteraptor);
            // pair down consecutive whitespace, because the browser does it when rendering options
            text = new Regex("[ ]{2}").Replace(text, " ").Trim();

            var option = elem.GetCheckBoxListItems().SingleOrDefault(o =>
                o.GetProperty<string>("text") == text || o.GetProperty<string>("value") == text)?.IsDisplayed ?? false;

            if (option != should)
            {
                Assert.Fail(
                    should
                        ? "Unable to find checkbox '{0}' in list '{1}'"
                        : "Unexpectedly found checkbox '{0}' in list '{1}'", text, @select);
            }
        }

        [Then("^I (should|should not) see ([^\"]+) \"([^\"]+)\" in the ([^\\s]+) checkbox list")]
        public static void IMightSeeNamedItemInCheckBoxList(string nopteraptor, string type, string name,
            string dropdown)
        {
            IMightSeeNamedItemPropertyInCheckBoxList(nopteraptor, type, name, "ToString", dropdown);
        }

        [Then("^I (should|should not) see ([^\"]+) \"([^\"]+)\"'s ([^\\s]+) in the ([^\\s]+) checkbox list")]
        public static void IMightSeeNamedItemPropertyInCheckBoxList(string nopteraptor, string type, string name,
            string propertyName, string dropdown)
        {
            var value = Data.GetCachedEntityPropertyValue(type, name, propertyName);
            IMightSeeCheckBoxWithTextOrValueInCheckboxList(nopteraptor, value, dropdown);
        }

        [Given("^\"([^\"]+)\" (is|isn't) checked in the ([^\\s]+) checkbox list")]
        [When("^\"([^\"]+)\" (is|isn't) checked in the ([^\\s]+) checkbox list")]
        [Then("^\"([^\"]+)\" (should|should not) be checked in the ([^\\s]+) checkbox list")]
        public static void TextMightBeCheckedInTheCheckBoxList(string value, string nopteraptor, string dropdown)
        {
            var elem = WebDriverHelper.Current.FindElement(By.Id(dropdown));
            var hasMatch = elem.GetCheckedCheckBoxListItems().Any(x => x.GetAttribute("text") == value);
            var should = StepHelper.IsTruthy(nopteraptor);

            if (should)
            {
                Assert.IsTrue(hasMatch, $"There is not a checkbox with the label '{value}' that is currently checked.");
            }
            else
            {
                Assert.IsFalse(hasMatch,
                    $"There is a checkbox with the label '{value}' that is currently checked when it should not be.");
            }
        }

        [Then("^([^\"]+) \"([^\"]+)\"'s ([^\\s]+) (should|should not) be checked in the ([^\\s]+) checkbox list")]
        public static void NamedDataPropertyAsTextMightBeCheckedInTheCheckBoxList(string type, string name,
            string propertyName, string nopteraptor, string dropdown)
        {
            var value = Data.GetCachedEntityPropertyValue(type, name, propertyName);
            TextMightBeCheckedInTheCheckBoxList(value, nopteraptor, dropdown);
        }

        [Then("^([^\"]+) \"([^\"]+)\" (should|should not) be checked in the ([^\\s]+) checkbox list")]
        public static void NamedItemIdMightBeCheckedInTheCheckBoxList(string type, string name, string nopteraptor,
            string dropdown)
        {
            var value = Data.GetCachedEntityPropertyValue(type, name, "Id");
            var elem = WebDriverHelper.Current.FindElement(By.Id(dropdown));
            var hasMatch = elem.GetCheckedCheckBoxListItems().Any(x => x.GetAttribute("value") == value);
            var should = StepHelper.IsTruthy(nopteraptor);

            if (should)
            {
                Assert.IsTrue(hasMatch, $"There is not a checkbox with the label '{value}' that is currently checked.");
            }
            else
            {
                Assert.IsFalse(hasMatch,
                    $"There is a checkbox with the label '{value}' that is currently checked when it should not be.");
            }
        }

        [Then("^([^\"]+) \"([^\"]+)\" (should|should not) be (enabled|disabled) in the ([^\\s]+) checkbox list")]
        public static void TheCheckBoxListItemMightBeEnabled(string type, string name, string nopteraptor,
            string enabled, string dropdown)
        {
            var value = Data.GetCachedEntityPropertyValue(type, name, "Id");
            var elem = WebDriverHelper.Current.FindElement(By.Id(dropdown));
            var hasMatch = elem.GetCheckBoxListItems().Where(x => x.GetAttribute("value") == value).SingleOrDefault();
            var should = StepHelper.IsTruthy(nopteraptor);

            Assert.IsNotNull(hasMatch, "Checkbox items were not found in the checkbox list");
            Assert.AreEqual(should, hasMatch.IsEnabled);
        }

        [Then("the checkbox named ([^\\s]+) with ([^\"]+) \"([^\"]+)\"'s ([^ ]+) (should|should not) be (enabled|disabled)$")]
        public static void TheCheckboxValueItemMightBeEnabled(string checkboxName, string type, string id, string value, string nopteraptor, string disenabled)
        {
            var should = StepHelper.IsTruthy(nopteraptor);
            var enabled = (disenabled == "enabled");
            var box = WebDriverHelper.Current.FindElements(By.Name(checkboxName));
            if ((should && enabled) == (box != null))
            {
                Assert.Fail(
                    should && enabled 
                        ? "CheckBox with name '{0}' and value '{1}' was not found when expected."
                        : "CheckBox with name '{0}' and value '{1}' was found unexpectedly.", checkboxName, id);
            }
        }

        #endregion

        [Then("^I should not see the field ([^\\s]+)$")]
        public static void ThenFieldShouldNotBeVisible(string field)
        {
            ThenFieldShouldBeVisible(field, "should not");
        }

        [Then("^the ([^\\s]+) field (should|should not) be visible$")]
        public static void ThenFieldShouldBeVisible(string field, string nopterator)
        {
            var should = StepHelper.IsTruthy(nopterator);
            var elem = WebDriverHelper.Current.FindElement(By.Id(field));

            if (elem.IsDisplayed != should)
            {
                Assert.Fail(
                    (should)
                        ? "Field with the id '{0}' was not found to be visible as expected."
                        : "Field with the id '{0}' was found to be visible unexpectedly.",
                    field);
            }
        }

        [Given("^the ([^\\s]+) ((?!tab)[^\\s]+) is (enabled|disabled)$")]
        [Then("^the ([^\\s]+) ((?!tab)[^\\s]+) should be (enabled|disabled)$")]
        public static void TheElementIsEnabled(string element, string elementType, string disenabled)
        {
            var el = WebDriverHelper.Current.FindElement(By.Id(element));
            var enabled = (disenabled == "enabled");
            Assert.AreEqual(enabled, el.IsEnabled);
        }

        [Given("^the ([^\\s]+) tab (is|is not) active")]
        [Then("^the ([^\\s]+) tab (should|should not) be active")]
        public static void TheTabIsActive(string element, string toBeOrNotToBe)
        {
            var shouldBeActive = StepHelper.IsTruthy(toBeOrNotToBe);
            var el = WebDriverHelper.Current.FindElement(By.CssSelector($"[aria-controls='{element}']"));

            Assert.AreEqual(shouldBeActive ? "true" : "false", el.GetAttribute("aria-selected"));
        }

        [Given("^the ([^\\s]+) field (is|is not) readonly$")]
        [Then("^the ([^\\s]+) field (should|should not) be readonly$")]
        public static void TheElementIsEnabled(string element, string shouldOrShouldNot)
        {
            var el = WebDriverHelper.Current.FindElement(By.Id(element));
            var shouldBeReadOnly = StepHelper.IsTruthy(shouldOrShouldNot);
            Assert.AreEqual(shouldBeReadOnly, el.GetProperty<bool>("readOnly"));
        }

        [Given("^the ([^\\s]+) field's autocomplete is (enabled|disabled)$")]
        [When("^the ([^\\s]+) field's autocomplete is (enabled|disabled)$")]
        [Then("^the ([^\\s]+) field should have autocomplete (enabled|disabled)$")]
        public static void TheElementMightEnableAutocomplete(string element, string disenabled)
        {
            var enabled = (disenabled == "enabled");
            var el = WebDriverHelper.Current.FindElement(By.Id(element));
            if (enabled)
            {
                Assert.IsTrue(el.GetAttribute("autocomplete") != "off",
                    "The autocomplete attribute must be set to 'on' or otherwise not be set at all.");
            }
            else
            {
                Assert.IsTrue(el.GetAttribute("autocomplete") == "off",
                    "The autocomplete attribute must be set to 'off'.");
            }
        }

        [Then("I (should|should not) see a checkbox named ([^\\s]+) with the value \"([^\"]+)\"$")]
        public static void ThenIShouldSeeACheckbox(string nopteraptor, string name, string value)
        {
            var should = StepHelper.IsTruthy(nopteraptor);

            var box = WebDriverHelper.Current.FindElements(By.Name(name)).SingleOrDefault(x => x.GetValue() == value);

            if (should != (box != null))
            {
                Assert.Fail(
                    should
                        ? "CheckBox with name '{0}' and value '{1}' was not found when expected."
                        : "CheckBox with name '{0}' and value '{1}' was found unexpectedly.", name, value);
            }
        }

        [Then("the ([^\\s]+) checkbox for ([^\"]+) \"([^\"]+)\" (should|should not) be checked$")]
        public static void ToBeOrNotToBeCheckedThatIsTheQuestion(string checkBoxName, string type, string name,
            string nopteraptor)
        {
            var should = StepHelper.IsTruthy(nopteraptor);
            var data = Data.GetCachedEntityPropertyValue(type, name, "Id");
            var box = WebDriverHelper.Current.FindElements(By.Name(checkBoxName))
                                     .SingleOrDefault(x => x.GetValue() == data);
            if (box == null)
            {
                Assert.Fail($"Could not find checkbox with name '{checkBoxName}'.");
            }

            if (should && !box.IsChecked)
            {
                Assert.Fail($"The checkbox '{checkBoxName}' was not checked when it should have been.");
            }

            if (!should && box.IsChecked)
            {
                Assert.Fail($"The checkbox '{checkBoxName}' was not checked when it should have been.");
            }
        }

        [Then("I (should|should not) see a checkbox named ([^\\s]+) with ([^\"]+) \"([^\"]+)\"'s ([^ ]+)$")]
        public static void ThenIShouldSeeACheckbox(string nopteraptor, string checkboxName, string type, string name,
            string dataMemberName)
        {
            var data = Data.GetCachedEntityPropertyValue(type, name, dataMemberName);
            ThenIShouldSeeACheckbox(nopteraptor, checkboxName, data);
        }

        [Then("I (should|should not) see the table-caption \"([^\"]+)\"$")]
        public static void ThenIShouldSeeATableCaption(string nopteraptor, string text)
        {
            var should = StepHelper.IsTruthy(nopteraptor);
            var caption = WebDriverHelper.Current.FindElements(ByHelper.TextInTag(text, "caption"));

            if (should != (caption != null))
            {
                Assert.Fail(
                    should
                        ? "Table caption with text '{0}' was not found."
                        : "Table caption with text '{0}' was found unexpectedly.", text);
            }
        }

        [Then("I (should|should not|shouldn't) see the \"(.+)\" tab")]
        public static void ThenIShouldSeeTheTab(string nopteraptor, string text)
        {
            var should = StepHelper.IsTruthy(nopteraptor);
            var tab = ElementHelper.TryFindTabByText(text);

            if (should != (tab != null && tab.IsDisplayed))
            {
                Assert.Fail(
                    should
                        ? "Tab with link text '{0}' was not found."
                        : "Tab with link text '{0}' was found unexpectedly.", text);
            }
        }

        [Then("the \"(.+)\" tab should be (enabled|disabled)")]
        public static void ThenTheTabShouldBeDisabled(string text, string disenabled)
        {
            var shouldBeEnabled = disenabled == "enabled";
            var tab = ElementHelper.TryFindTabByText(text);

            if (tab == null)
            {
                Assert.Fail($"Tab with link text '{text}' was not found.");
            }

            var isEnabled = !tab.GetCssClasses().Contains("ui-state-disabled");

            if (shouldBeEnabled && !isEnabled)
            {
                Assert.Fail($"Tab with link text '{text}' is not enabled.");
            }

            else if (!shouldBeEnabled && isEnabled)
            {
                Assert.Fail($"Tab with link text '{text}' is not disabled.");
            }
        }

        #region Seeing stuff in columns

        [Then(
            "^I (should|shouldn't|should not|shant) see \"([^\"]+)\" in the table ([^\"\\s][^\"\\s]*)'s \"([^\"]+)\" column$")]
        public static IBetterWebElement ThenIShouldSeeTextInTheTablesNamedColumn(string nopteraptor, string text,
            string tableId, string columnName)
        {
            IBetterWebElement cell;
            if (StepHelper.IsTruthy(nopteraptor))
            {
                if (!TextExistsInColumn(columnName, text, tableId, out cell))
                {
                    Assert.Fail(
                        $"No row exists with '{text}' in the {columnName} column.  Page text: {WebDriverHelper.Current.PageSource}");
                }
            }
            else
            {
                if (TextExistsInColumn(columnName, text, tableId, out cell))
                {
                    Assert.Fail(
                        $"The row exists with '{text}' in the {columnName} column.  Page text: {WebDriverHelper.Current.PageSource}");
                }
            }

            return cell;
        }

        [Then(
            "^I (should|shouldn't|should not|shant) see ([^\"]+) in the table ([^\"\\s][^\"\\s]*)'s \"([^\"]+)\" column$")]
        public static IBetterWebElement ThenIShouldSeeSpecialValueInTheTablesNamedColumn(string nopteraptor,
            string text, string tableId, string columnName)
        {
            return ThenIShouldSeeTextInTheTablesNamedColumn(nopteraptor, Input.ParseSpecialString(text), tableId,
                columnName);
        }

        [Then("^I (should|shouldn't|should not|shant) see \"([^\"]+)\" in the \"([^\"]+)\" column$")]
        public static IBetterWebElement ThenIShouldSeeTextInTheNamedColumn(string nopteraptor, string text,
            string columnName)
        {
            IBetterWebElement cell;
            if (StepHelper.IsTruthy(nopteraptor))
            {
                if (!TextExistsInColumn(columnName, text, (string)null, out cell))
                {
                    Assert.Fail(
                        $"No row exists with '{text}' in the {columnName} column.  Page text: {WebDriverHelper.Current.PageSource}");
                }
            }
            else
            {
                if (TextExistsInColumn(columnName, text, (string)null, out cell))
                {
                    Assert.Fail(
                        $"The row exists with '{text}' in the {columnName} column.  Page text: {WebDriverHelper.Current.PageSource}");
                }
            }

            return cell;
        }

        [Then("^I (should|shouldn't|should not|shant) see the \"([^\"]+)\" column in the \"([^\"]+)\" table$")]
        public static void IShouldNotSeeTheTableColumnWithName(string yesNo, string columnName, string tableId)
        {
            var shouldBeVisible = StepHelper.IsTruthy(yesNo);
            var fullTable = GetSpecflowTableFromHtmlTable(tableId);
            var table = WebDriverHelper.Current.FindElement(By.Id(tableId));

            var headersWithoutSortingThings =
                fullTable.Header.Select(x => x.Replace("▾", string.Empty).Replace("▴", string.Empty));

            if (shouldBeVisible)
            {
                Assert.IsTrue(headersWithoutSortingThings.Contains(columnName),
                    $"A column with the header \"{columnName}\" was not found in the table \"{tableId}\". Columns found: {string.Join(",", fullTable.Header)}");
                var columnHeader = GetColumnIndexByName(table, columnName).HeaderCell;
                Assert.IsTrue(columnHeader.IsDisplayed, $"The {columnName} column is not displayed when it should be.");
            }
            else
            {
                if (headersWithoutSortingThings.Contains(columnName))
                {
                    var columnHeader = GetColumnIndexByName(table, columnName).HeaderCell;
                    Assert.IsFalse(columnHeader.IsDisplayed,
                        $"The {columnName} column is displayed when it should not be.");
                }
            }
        }

        /// <summary>
        /// Checks for an exact match of a cell based on a column header
        /// </summary>
        /// <param name="nopteraptor">should or shouldn't</param>
        /// <param name="type">object's class </param>
        /// <param name="name">name of the object</param>
        /// <param name="property">property whose value we want to see</param>
        /// <param name="columnName">column header it should be in</param>
        [Then(
            "^I (should|shouldn't|should not|shant) see ([^\"]+) \"([^\"]+)\"'s ([^\\s]+) in the \"([^\"]+)\" column$")]
        public static IBetterWebElement ThenIShouldSeeTextInTheNamedColumn(string nopteraptor, string objType,
            string instanceName, string propertyName, string columnName)
        {
            return AssertTextInNamedColumn(nopteraptor, objType, instanceName, propertyName, columnName, null,
                o => o.ToString());
        }

        [Then(
            "I (should|shouldn't|should not|shant) see ([^\"]+) \"([^\"]+)\"'s ([^\\s]+) as(?: a)? (.+) in the \"([^\"]+)\" column$")]
        public static IBetterWebElement ThenIShouldSeeFormattedTextInTheNamedColumn(string nopteraptor, string objType,
            string instanceName, string propertyName, string format, string columnName)
        {
            return AssertTextInNamedColumn(nopteraptor, objType, instanceName, propertyName, columnName, null,
                o => FORMAT_HELPER.FormatValue(o, format));
        }

        [Then(
            "^I (should|shouldn't|should not|shant) see ([^\"]+) \"([^\"]+)\"'s ([^\\s]+) in the table ([^\"\\s][^\"\\s]*)'s \"([^\"]+)\" column$")]
        public static IBetterWebElement ThenIShouldSeeNamedTypesPropertyInTheNamedTablesNamedColumn(string nopteraptor,
            string objType, string instanceName, string propertyName, string tableId, string columnName)
        {
            return AssertTextInNamedColumn(nopteraptor, objType, instanceName, propertyName, columnName, tableId,
                o => o.ToString());
        }

        [Then(
            "^I (should|shouldn't|should not|shant) see ([^\"]+) \"([^\"]+)\"'s ([^\\s]+) in the table ([^\"\\s][^\"\\s]*)'s \"([^\"]+)\" column under the \"([^\"]+)\" tab$")]
        public static IBetterWebElement ThenIShouldSeeNamedTypesPropertyInTheNamedTablesNamedColumnUnderTab(
            string nopteraptor, string objType, string instanceName, string propertyName, string tableId,
            string columnName, string tabText)
        {
            var tabDiv = ElementHelper.FindTabContentContainerByTabText(tabText);
            return AssertTextInNamedColumn(nopteraptor, objType, instanceName, propertyName, columnName, tableId,
                o => o.ToString(), parentObject: tabDiv);
        }

        [Then(
            "I (should|shouldn't|should not|shant) see ([^\"]+) \"([^\"]+)\"'s ([^\\s]+) as(?: a)? (.+) in the table ([^\"\\s][^\"\\s]*)'s \"([^\"]+)\" column$")]
        public static IBetterWebElement ThenIShouldSeeNamedTypesPropertyFormattedAsInTheNamedTablesNamedColumn(
            string nopteraptor, string objType, string instanceName, string propertyName, string format, string tableId,
            string columnName)
        {
            return AssertTextInNamedColumn(nopteraptor, objType, instanceName, propertyName, columnName, tableId,
                o => FORMAT_HELPER.FormatValue(o, format));
        }

        [Then(
            "I (should|shouldn't|should not|shant) see (-?[0-9]*\\.?[0-9]+) as(?: a)? (.+) in the table ([^\"\\s][^\"\\s]*)'s \"([^\"]+)\" column$")]
        public static IBetterWebElement ThenIShouldSeeNumberFormattedAsInTheNamedTablesNamedColumn(string nopteraptor,
            decimal value, string format, string tableId, string columnName)
        {
            return AssertTextInNamedColumn(nopteraptor, value, columnName, tableId,
                o => FORMAT_HELPER.FormatValue(o, format));
        }

        [Then("the ([^\"]+) table should be empty")]
        public static void TheNamedTableShouldBeEmpty(string tableId)
        {
            // There's no "should not" for this because you should be testing
            // the values that exist in the table, not whether or not there's
            // any value at all in there.

            // Table must exist, so this will throw.
            var table = WebDriverHelper.Current.FindElement(By.Id(tableId));

            if (table.FindElements(By.CssSelector("tbody tr")).Any())
            {
                Assert.Fail($"Table {tableId} has non-header/footer rows.");
            }
        }

        [Then("the ([^\"]+) table should have ([0-9]+) rows")]
        public static void TheNamedTableShouldHaveXRows(string tableId, int rows)
        {
            // There's no "should not" for this because you should be testing
            // the values that exist in the table, not whether or not there's
            // any value at all in there.

            // Table must exist, so this will throw.
            var table = WebDriverHelper.Current.FindElement(By.Id(tableId));
            var rowCount = table.FindElements(By.CssSelector("tbody > tr"));

            if (rowCount.Count() != rows)
            {
                Assert.Fail($"Table {tableId} does not have {rows} rows as expected.");
            }
        }

        private static string ParseExpectedTableCellText(string rawValue)
        {
            if (NAMED_TYPE_PROPERTY.IsMatch(rawValue))
            {
                var matches = NAMED_TYPE_PROPERTY.Match(rawValue);
                // I don't understand regex. -Ross
                var type = matches.Groups[1].Value;
                var name = matches.Groups[2].Value;
                var prop = matches.Groups[3].Value;
                return Convert.ToString(Data.GetCurrentEntityPropertyValue(type, name, prop));
            }

            if (NAMED_TYPE.IsMatch(rawValue))
            {
                var matches = NAMED_TYPE.Match(rawValue);
                // I don't understand regex. -Ross
                var type = matches.Groups[1].Value;
                var name = matches.Groups[2].Value;
                return Convert.ToString(Data.GetCurrentEntityPropertyValue(type, name, "ToString"));
            }

            return rawValue;
        }

        private static Table GetSpecflowTableFromHtmlTable(string tableId)
        {
            // NOTE: WebDriver has an occasional issue with socket exceptions/connection issues
            //       on rare occasions when there's a ton of commands being sent to the browser.
            //       The only time I actually saw this come up was when running all regressions.
            //       The ValveMonthlyReportPage.UserCanViewTheValvesOperatedByMonthReport would
            //       always fail. While I'm not sure what the actual problem is(the internet doesn't
            //       seem to know either), there were definitely a lot of repeated requests being
            //       made to the browser that don't need to be done. Also parsing more than a couple
            //       of rows got extremely slow. By reading the whole table into memory once, this
            //       speeds things up significantly. 
            var tableElement = WebDriverHelper.Current.FindElement(By.Id(tableId));

            var header = tableElement.FindElements(By.CssSelector("thead > tr > th"));
            Assert.IsTrue(header.Any(), $"No header row found in {tableId}");
            var headerCells = header.Select(x => x.Text).ToArray();
            var table = new Table(headerCells);

            var bodyRows = tableElement.FindElements(By.CssSelector("tbody > tr"));
            var footerRows = tableElement.FindElements(By.CssSelector("tfoot > tr"));
            var rows = bodyRows.Concat(footerRows);
            foreach (var row in rows)
            {
                var cells = row.FindElements(By.TagName("td")).Select(x => x.Text).ToArray();
                if (cells.Count() == headerCells.Count())
                {
                    table.AddRow(cells);
                }
                else
                {
                    // This is being done for the time being so we can ignore rows where colspan is being used.
                    // Those types of tables will not work with table parsing.
                    Console.WriteLine(
                        $"Ignored parsing row because the number of cells({cells.Count()}) does not match the number of header cells({headerCells.Count()}).");
                    Console.WriteLine($"Values of ignored row: {string.Join(",", cells)}");
                }
            }

            return table;
        }

        [Then("^I should see the following values in the ([^\\s]+) table")]
        public static void IShouldSeeThisTableWithValues(string tableId, TechTalk.SpecFlow.Table testTable)
        {
            // To make life easy, this does not care what order the columns are in.
            // However, this is meant to also test that rows exist in the order they're given.

            var htmlTable = GetSpecflowTableFromHtmlTable(tableId);

            foreach (var header in testTable.Header)
            {
                Assert.IsTrue(htmlTable.ContainsColumn(header),
                    $"Table '{tableId}' does not contain a column with header '{header}'.");
            }

            for (var i = 0; i < testTable.Rows.Count; i++)
            {
                var testRow = testTable.Rows[i];
                foreach (var testHeader in testTable.Header)
                {
                    var rawExpectedValue = testRow[testHeader];
                    var isPartialMatch = rawExpectedValue.StartsWith("*") && rawExpectedValue.EndsWith("*");
                    if (isPartialMatch)
                    {
                        rawExpectedValue = rawExpectedValue.Replace("*", "");
                    }
                    else if (rawExpectedValue.IsDateTime())
                    {
                        rawExpectedValue = rawExpectedValue.ToDateTime().ToShortDateString();
                    }

                    var expected = ParseExpectedTableCellText(rawExpectedValue);
                    // HUGE NOTE: The SpecFlow table is going to return invisible text. Be very aware of this
                    // if you're trying to match text in a cell that has hidden elements.
                    var actual = htmlTable.Rows[i][testHeader];
                    //Console.WriteLine($"Asserting that '{expected}' is equal to '{actual}'");

                    // NOTE: If you want to do partial text matching here, you need to put *'s around the value in the specflow table.
                    //       The previous way this was tested was to always use partial matches, which was causing things to pass
                    //       when they should have failed.
                    if (isPartialMatch)
                    {
                        Assert.IsTrue(actual.Contains(expected),
                            $"Expected '{actual}' to contain the text '{expected}'.");
                    }
                    else
                    {
                        Assert.AreEqual(expected, actual,
                            $"Mismatch at row {i}, column '{testHeader}'. If you're trying to do a partial match. put asterisks around the expected value.");
                    }
                }
            }
        }

        #endregion

        #region I should/n't see links

        private static void IMightSeeALinkWithUrl(string shouldernt, IBetterWebElement expectedLink,
            string urlForErrorMessage)
        {
            if (StepHelper.IsTruthy(shouldernt))
            {
                Assert.IsNotNull(expectedLink, $"Could not find a link with the url '{urlForErrorMessage}'");
            }
            else
            {
                Assert.IsNull(expectedLink, $"Link was found when it should not have been: '{urlForErrorMessage}'");
            }
        }

        [Then("^I should see the link \"([^\"]+)\"")]
        public static IBetterWebElement ThenIShouldSeeLink(string text)
        {
            // FindElement will fail if nothing is found.
            return WebDriverHelper.Current.FindElement(By.LinkText(text));
        }

        [Then("^I should see the link with href \"([^\"]+)\"")]
        public static IBetterWebElement ThenIShouldSeeLinkWithHref(string href)
        {
            // FindElement will fail if nothing is found.
            return WebDriverHelper.Current.FindElement(By.CssSelector($"[href^=\"{href}\"]"));
        }

        [Then("^I should not see the link \"([^\"]+)\"")]
        public static void ThenIShouldNotSeeLink(string text)
        {
            if (WebDriverHelper.Current.FindElements(ByHelper.TextInTag(text, "a")).Any())
            {
                Assert.Fail($"A link with the text '{text}' was found when not expected.");
            }
        }

        [Then("^I should see the link \"([^\"]+)\" with the url \"([^\"]+)\"")]
        public static void ThenIMightSeeLinkWithExactUrl(string text, string url)
        {
            var link = ThenIShouldSeeLink(text);
            var linkHref = link.GetAttribute("href");
            if (linkHref != url)
            {
                Assert.AreEqual(linkHref, url);
            }
        }

        [Then("^I should see the link \"([^\"]+)\" ends with \"([^\"]+)\"$")]
        public static void ThenIShouldSeeLink(string text, string url)
        {
            //var link = WebBrowser.Current.ILink(Find.ByText(text));
            var link = WebDriverHelper.Current.FindElements(ByHelper.TextInTag(text, "a"));
            if (!link.Any())
            {
                Assert.Fail($"A link with the text '{text}' and url was not found when expected.");
            }

            var href = link.SingleOrDefault(x => x.GetAttribute("href").EndsWith(url));
            if (href == null)
            {
                Assert.Fail($"The link url did not end with {url}. Actual: {href}");
            }
        }

        [Then(
            "^I (should|shouldn't|should not|shant) see a secure link to the ([^\"]+) page for ([^\":]+):? \"([^\"]+)\"")]
        public static void ThenIShouldSeeASecureLinkToThePageFor(string nopteraptor, string action, string className,
            string namedEntity)
        {
            //   var should = StepHelper.IsTruthy(nopteraptor);
            IBetterWebElement link;
            var baseUrl = GetLinkForNamedEntity(action, className, namedEntity, out link) + "\\?" +
                          FormBuilder.SECURE_FORM_HIDDEN_FIELD_NAME + "=";
            var baseUrlRegEx = new Regex(baseUrl + ".+", RegexOptions.IgnoreCase);
            link = WebDriverHelper.Current.FindElements(ByHelper.Href(baseUrlRegEx)).SingleOrDefault();
            IMightSeeALinkWithUrl(nopteraptor, link, baseUrl);
        }

        [Then("^I (should|shouldn't|should not|shant) see a link to \"([^\"]+)\"$")]
        public static void ThenIShouldSeeALink(string nopteraptor, string url)
        {
            ThenIShouldSeeTheLinkToAPage(nopteraptor, url);
        }

        [Then("^I (should|shouldn't|should not|shant) see a link to the ([^\"]+) page for ([^\":]+):? \"([^\"]+)\"")]
        public static void ThenIShouldSeeTheLinkToAPageFor(string nopteraptor, string action, string className,
            string namedEntity)
        {
            IBetterWebElement link;
            var baseUrl = GetLinkForNamedEntity(action, className, namedEntity, out link);
            IMightSeeALinkWithUrl(nopteraptor, link, baseUrl);
        }

        [Then("^I (should|shouldn't|should not|shant) see a link to the ([^\"]+) page")]
        public static void ThenIShouldSeeTheLinkToAPage(string nopteraptor, string action)
        {
            IBetterWebElement link;
            var baseUrl = GetLinkForPartialUrl(action, out link);
            IMightSeeALinkWithUrl(nopteraptor, link, baseUrl);
        }

        [Then("I (should|shouldn't|should not|shant) see a link to the ([^\"]+) page with querystring( .+)")]
        public static void ThenIShouldSeeTheLinkToAPageWithQueryString(string nopteraptor, string action,
            string queryMess)
        {
            IBetterWebElement link;
            var baseUrl = GetLinkForPartialUrl(action, queryMess, out link);
            IMightSeeALinkWithUrl(nopteraptor, link, baseUrl);
        }

        [Then("I (should|shouldn't|should not|shant) see a link to the ([^\"]+) page for ([^\":]+):? \"([^\"]+)\" and fragment of \"(.+)\"")]
        public static void ThenIShouldSeeTheLinkToAPageAndFragmentOf(string nopteraptor, string action, string className, string entity, string fragment)
        {
            var linkUrl = Navigation.GetUriFor(action, className, entity) + fragment;

            IBetterWebElement link = WebDriverHelper.Current.FindElements(ByHelper.Href(linkUrl)).SingleOrDefault();

            IMightSeeALinkWithUrl(nopteraptor, link, linkUrl);
        }

        #endregion

        #region I should/n't see the button

        [Then("^I (should|should not) see the button \"([^\"]+)\"$")]
        public static void ThenIShouldSeeButton(string shouldernt, string buttonText)
        {
            var button = ElementHelper.TryFindButtonByIdNameOrText(buttonText);
            var should = StepHelper.IsTruthy(shouldernt);
            if (should && (button == null || !button.IsDisplayed))
            {
                Assert.Fail($"A button with the name '{buttonText}' was not found.");
            }

            if (!should && (button != null && button.IsDisplayed))
            {
                Assert.Fail($"A button with the name '{buttonText}' was found when not expected.");
            }
        }

        [Then("^I (should|should not) see the button \"([^\"]+)\" under the \"([^\"]+)\" tab$")]
        public static void ThenIShouldSeeButtonUnderTab(string shouldernt, string buttonText, string tabText)
        {
            var tab = ElementHelper.FindTabContentContainerByTabText(tabText);
            var button = ElementHelper.TryFindButtonByIdNameOrText(buttonText, tab);
            var should = StepHelper.IsTruthy(shouldernt);

            if (should && (button == null || !button.IsDisplayed))
            {
                Assert.Fail($"A button with the name '{buttonText}' was not found.");
            }

            if (!should && (button != null && button.IsDisplayed))
            {
                Assert.Fail($"A button with the name '{buttonText}' was found when not expected.");
            }
        }

        #endregion

        #region I should/n't see the table

        [Then("^I (should|shouldn't|should not|shant) see the table (.+)$")]
        public static void ThenIShouldSeeTheTable(string nopteraptor, string tableId)
        {
            var table = WebDriverHelper.Current.FindElements(By.Id(tableId)).SingleOrDefault();

            if (table != null && table.TagName != "table")
            {
                Assert.Fail($"'{tableId}' is not a table.");
            }

            if (StepHelper.IsTruthy(nopteraptor))
            {
                Assert.IsNotNull(table, $"No table found with the id '{tableId}'.");
            }
            else
            {
                Assert.IsNull(table, $"Table with id '{tableId}' found unexpectedly.");
            }
        }

        #endregion

        #region Should/Shouldn't See Error/Validation Message

        // This step's for finding specific validation messages when multiple of the same message
        // might exist on a page(ie: Models with horribly long default Required messages that get replaced
        // with something shorter).
        [Then("I (should|should not) see a validation message for ([^\\s]+) with \"([^\"]+)\"$")]
        public static void IShouldSeeAValidationMessageForField(string shouldernt, string field, string message)
        {
            var should = StepHelper.IsTruthy(shouldernt);
            
            var byChain = By.CssSelector($"span.field-validation-error[data-valmsg-for='{field}']");
            IMightSeeMessage(byChain, message, should);
        }

        [Then(
            "^I (should|shouldn't|should not) see the (validation|notification|error) message with ([^\"]+): \"([^\"]+)\"'s ([^\\s]+)$")]
        public static void ThenIShouldSeeTheMessage(string nopteraptor, string type, string messageType, string name,
            string property)
        {
            //var namedItem = TestObjectCache.Instance.Lookup(type, name);
            //var text = ObjectExtensions.GetPropertyValueByName(namedItem, property).ToString();
            var text = Data.GetCachedEntityPropertyValue(messageType, name, property);
            ThenIShouldSeeTheMessage(nopteraptor, messageType, text);
        }

        // TODO: There's something here between this and Input.ParseSpecialStrings that i'm just not connecting yet
        // Could be made to work a bit cleaner
        private static string ReplaceSpecialStrings(this string data)
        {
            var dates = new Dictionary<string, DateTime> {
                { " today's date ", DateTime.Today },
                { " yesterday's date ", DateTime.Today.AddDays(-1) },
                { " tomorrow ", DateTime.Today.AddDays(1) }
            };

            return dates.Aggregate(data, (current, thing) =>
                current.Replace(thing.Key, $" {thing.Value} "));
        }

        [When("^I (can|can not) see the (validation|notification|error|success) message \"?([^\"]+?)\"?$")]
        [Then("^I (should|shouldn't|should not) see the (validation|notification|error|success) message \"?([^\"]+?)\"?$")]
        public static void ThenIShouldSeeTheMessage(string nopteraptor, string messageType, string message)
        {
            var iShouldSee = StepHelper.IsTruthy(nopteraptor);
            message = message.ReplaceSpecialStrings();

            switch (messageType)
            {
                case "error":
                    IMightSeeMessage(By.CssSelector(".notification-item-error .message"), message, iShouldSee);
                    break;
                case "validation":
                    IMightSeeMessage(By.ClassName("field-validation-error"), message, iShouldSee);
                    break;
                case "notification":
                    IMightSeeMessage(By.CssSelector("div.notification div.message"), message, iShouldSee);
                    break;
                case "success":
                    IMightSeeMessage(By.CssSelector(".notification-item-success .message"), message, iShouldSee);
                    break;
            }
        }

        private static void IMightSeeMessage(By baseConstraint, string message, bool iShouldSee = true)
        {
            var elements = WebDriverHelper.Current.FindElements(baseConstraint);

            if (iShouldSee != elements.Any(x => TrimmedStringsMatch(message, x.Text)))
            {
                Assert.Fail(
                    iShouldSee
                        ? "Expected message is not displayed.  Expected: \"{0}\""
                        : "Unexpected validation message: \"{0}\"", message);
            }
        }

        private static bool TrimmedStringsMatch(string left, string right)
        {
            return left.Trim() == right.Trim();
        }

        #endregion

        #endregion

        #region Current Location (url)

        // TODO: These steps should all be paired down. -Ross 5/23/2018

        [Then("I should be at the (.+) screen")]
        public static void ThenIShouldBeAtScreen(string page)
        {
            var expectedUrl = Navigation.GetUriForPage(page);
            Navigation.UrlsAreEqual(expectedUrl, WebDriverHelper.Current.CurrentUri);

            // This is here because there have been regression tests where this is the last step and
            // the page was failing to load properly.
            AssertThereIsNoYSOD($"Navigation to the url {expectedUrl} resulted in a YSOD.");
        }

        [Then("I should not be at the (.+) screen")]
        public static void ThenIShouldNotBeAtScreen(string page)
        {
            var expectedUrl = Navigation.GetUriForPage(page);
            Navigation.UrlsAreNotEqual(expectedUrl, WebDriverHelper.Current.CurrentUri);
            // This is here because there have been regression tests where this is the last step and
            // the page was failing to load properly.
            AssertThereIsNoYSOD($"Navigation to the url {expectedUrl} resulted in a YSOD.");
        }

        /// <summary>
        /// Used for when the browser's current url might have a query string
        /// that doesn't need to be accounted for.
        /// </summary>
        /// <param name="page"></param>
        [Then("I should be at the (.+) page")]
        public static void ThenIShouldBeAtPage(string page)
        {
            var pageString = "/" + Navigation.GetPageString(page);
            var result = WebDriverHelper.Current.CurrentUri.AbsolutePath;
            Navigation.UrlStartsWith(pageString, result, "Not at the expected page. Expected {0}, actual {1}",
                pageString, result);

            // This is here because there have been regression tests where this is the last step and
            // the page was failing to load properly.
            AssertThereIsNoYSOD($"Navigation to the url resulted in a YSOD.");
        }

        [Then("^I (should|should not) be at the ([^\"]+) page for ([^\":]+):? \"([^\"]+)\" on the ([^\"]+) tab")]
        public static void ThenIShouldBeAtTheTabOnThePageFor(string shouldernt, string action, string className,
            string entity, string tab)
        {
            // NOTE: This test doesn't actually assert that the tab is visible. This won't work in MapCallMVC, might work in Permits for that.
            var expected = String.Format("{0}#{1}Tab", Navigation.GetUriFor(action, className, entity), tab);
            var current = WebDriverHelper.Current.CurrentUri.ToString();
            try
            {
                if (StepHelper.IsTruthy(shouldernt))
                {
                    Navigation.UrlsAreEqual(expected, current);
                }
                else
                {
                    Navigation.UrlsAreNotEqual(expected, current);
                }

                // This is here because there have been regression tests where this is the last step and
                // the page was failing to load properly.
                AssertThereIsNoYSOD($"Navigation to the url {expected} resulted in a YSOD.");
            }
            catch (Exception)
            {
                WebDriverHelper.Current.CaptureScreenshot();
                throw;
            }
        }

        [Then("^I (should|should not) be at the ([^\"]+) page for ([^\":]+):? \"([^\"]+)\"")]
        public static void ThenIShouldBeAtThePageFor(string shouldernt, string action, string className, string entity)
        {
            var expected = Navigation.GetUriFor(action, className, entity).ToString();
            var current = WebDriverHelper.Current.CurrentUri.ToString()
                                         .RemoveQueryString()
                                         .RemoveFragmentString();
            if (StepHelper.IsTruthy(shouldernt))
            {
                Navigation.UrlsAreEqual(expected, current);
            }
            else
            {
                Navigation.UrlsAreNotEqual(expected, current);
            }

            // This is here because there have been regression tests where this is the last step and
            // the page was failing to load properly.
            AssertThereIsNoYSOD($"Navigation to the url {expected} resulted in a YSOD.");
        }

        [Then("^I should be at the ([^\"]+) page for ([^\":]+):? \"([^\"]+)\" with (.+)")]
        public static void ThenIShouldBeAtThePageForThingWithQueryString(string action, string className, string entity,
            string queryString)
        {
            var expectedUrl = Navigation.GetUrlFor(action, className, entity, queryString);
            Navigation.VerifyCurrentUrl(expectedUrl);

            // This is here because there have been regression tests where this is the last step and
            // the page was failing to load properly.
            AssertThereIsNoYSOD($"Navigation to the url {expectedUrl} resulted in a YSOD.");
        }

        [Then("^I should be at the ([^\"]+) page with querystring for ([^\":]+):? \"([^\"]+)\"")]
        public static void ThenIShouldBeAtTheActionPageWithIdQueryStringFromTheEntity(string action, string type,
            string name)
        {
            //get the id for the entity
            var id = GetRawValue(type, name, "Id");
            var expectedUrl = Navigation.GetUriForPage(String.Format("{0}?formId={1}", action, id)).ToString();

            Navigation.VerifyCurrentUrl(expectedUrl);

            // This is here because there have been regression tests where this is the last step and
            // the page was failing to load properly.
            AssertThereIsNoYSOD($"Navigation to the url {expectedUrl} resulted in a YSOD.");
        }

        [Then("^I (should|should not) be at the ([^\"]+) page for ([^\":]+):? \"([^\"]+)\" and fragment of \"(.+)\"")]
        public static void ThenIShouldBeAtThePageForAndFragmentOf(string shouldernt, string action, string className, string entity, string fragment)
        {
            var expected = Navigation.GetUriFor(action, className, entity).ToString() + fragment;
            var current = WebDriverHelper.Current.CurrentUri.ToString().RemoveQueryString();

            if (StepHelper.IsTruthy(shouldernt))
            {
                Navigation.UrlsAreEqual(expected, current);
            }
            else
            {
                Navigation.UrlsAreNotEqual(expected, current);
            }

            // This is here because there have been regression tests where this is the last step and
            // the page was failing to load properly.
            AssertThereIsNoYSOD($"Navigation to the url {expected} resulted in a YSOD.");
        }

        #endregion

        #region Data State

        [Then("^the ([^\" ]+) value for ([^\"]+) \"([^\"]+)\" should be ([^\"]+) \"([^\"]+)\"")]
        public static void ThenTheValueForSomeNamedThingShouldBeSomeOtherNamedThing(string property, string type,
            string name, string otherType, string otherName)
        {
            var thing = Data.GetCachedEntity(type, name);
            dynamic actualThing = Data.GetCurrentEntityPropertyValue(thing, thing.GetType(), property);
            dynamic expectedThing = Data.GetCachedEntity(otherType, otherName);

            Assert.AreEqual(expectedThing.Id, actualThing.Id);
        }

        [Then("^the ([^\" ]+) value for ([^\"]+) \"([^\"]+)\" should be \"([^\"]+)\"$")]
        public static void ThenTheValueForSomeNamedThingShouldBe(string property, string type, string name,
            string newValue)
        {
            var value = GetRawValue(type, name, property);
            Assert.IsNotNull(value, $"Value is null on property: {property}");
            Assert.AreEqual(newValue, Convert.ChangeType(value, typeof(string)));
        }

        [Then("^the ([^\" ]+) date value for ([^\"]+) \"([^\"]+)\" should be the special date value \"([^\"]+)\"$")]
        public static void ThenTheValueForSomeNamedThingShouldBeTheSpecialValue(string property, string type,
            string name, string specialValue)
        {
            var value = GetRawValue(type, name, property);
            Assert.IsNotNull(value, $"Value is null on property: {property}");
            var expected = specialValue.ToDateTime().Date;
            var actual = ((DateTime)Convert.ChangeType(value, typeof(DateTime))).Date;
            Assert.AreEqual(expected, actual);
        }

        [Then("^the ([^\" ]+) value for ([^\"]+) \"([^\"]+)\" (?:should not|shouldn't) be \"([^\"]+)\"$")]
        public static void ThenTheValueForSomeNamedThingShouldNotBe(string property, string type, string name,
            string unexpectedValue = null)
        {
            Assert.AreNotEqual(unexpectedValue, Convert.ChangeType(GetRawValue(type, name, property), typeof(string)));
        }

        [Then("^the ([^\" ]+) value for ([^\"]+) \"([^\"]+)\" (?:should not|shouldn't) be null$")]
        public static void ThenTheValueForSomeNamedThingShouldNotBeNull(string property, string type, string name)
        {
            ThenTheValueForSomeNamedThingShouldNotBe(property, type, name);
        }

        [Then(
            "^the ([^\" ]+) date value for ([^\"]+) \"([^\"]+)\" (?:should not|shouldn't) be the special date value \"([^\"]+)\"$")]
        public static void ThenTheValueForSomeNamedThingShouldNotBeTheSpecialValue(string property, string type,
            string name, string specialValue)
        {
            var value = GetRawValue(type, name, property);
            if (value == null)
            {
                // it certainly isn't whatever special date value
                return;
            }

            var expected = specialValue.ToDateTime().Date;
            var actual = ((DateTime)Convert.ChangeType(value, typeof(DateTime))).Date;
            Assert.AreNotEqual(expected, actual);
        }

        [Then("^([^\"]+) \"([^\"]+)\" should no longer exist$")]
        public static void ThenTheNamedThingShouldNoLongerExist(string type, string name)
        {
            var entity = (IEntity)Data.GetCachedEntity(type, name);
            Assert.IsFalse(Data.EntityExists(entity.Id, entity.GetType()));
        }

        #endregion

        #endregion

        #region Helper Methods

        public static string GetValueInField(string field)
        {
            return ElementHelper.FindElementByIdNameOrSelector(field).GetValue();
        }

        public static object GetRawValue(string objType, string instanceName, string propertyName, bool reload = true)
        {
            var entity = Data.GetCachedEntity(objType, instanceName);
            object propValue = null;

            if (reload && !Data.NoDataReload)
            {
                return Data.GetCurrentEntityPropertyValue(entity, Data.GetModelType(objType), propertyName);
            }
            else
            {
                return ObjectExtensions.GetPropertyValueByName(entity, propertyName);
            }
        }

        private static bool TextExistsInColumnInRow(string columnName, string text, IBetterWebElement tableRow,
            out IBetterWebElement cell)
        {
            var table = tableRow.FindElement(ByHelper.AncestorTagName("table"));
            var headerRows = table.FindElements(By.CssSelector("thead tr")).ToList();
            if (!headerRows.Any())
            {
                Assert.Fail("Table does not appear to have a header row, so no columns can be located.");
            }

            var headerRow = headerRows[0];
            var columns = headerRow.FindElements(By.TagName("th"));
            var columnIndex = columns.TakeWhile(x => x.Text != columnName &&
                                                     x.Text != columnName + SORTING_TEXT
                                                     && x.Text != columnName + SORTING_ASCENDING_TEXT &&
                                                     x.Text != columnName + SORTING_DESCENDING_TEXT).Count();

            if (columnIndex == columns.Count())
            {
                Assert.Fail(
                    $"Column Header: {columnName} does not exist in table {table}. Page text: {WebDriverHelper.Current.PageSource}");
            }

            var allCells = tableRow.FindElements(By.TagName("td")).ToList();
            if (allCells.Count() <= columnIndex)
            {
                cell = null;
                return false;
            }

            cell = allCells[columnIndex];
            return cell != null && !String.IsNullOrWhiteSpace(cell.Text) && text == cell.Text.Trim();
        }

        public static bool TextExistsInColumn(string columnName, string text, string tableId,
            out IBetterWebElement cell, bool matchWholeColumn = false, int rowIndex = -1,
            IFindElements parentObject = null)
        {
            parentObject = parentObject ?? WebDriverHelper.Current;
            var table = (tableId != null
                ? parentObject.FindElement(By.Id(tableId))
                : parentObject.FindElements(By.TagName("table")).First());
            return TextExistsInColumn(columnName, text, table, out cell, matchWholeColumn, rowIndex);
        }

        public static (int Index, IBetterWebElement HeaderCell) GetColumnIndexByName(IBetterWebElement table,
            string columnName)
        {
            // Use /thead rather than //thead. The double slash returns all descendants, while the 
            // single slash only returns direct descendants. This prevents tables-inside-tables from
            // causing problems.
            var rows = table.FindElements(ByHelper.XPath("/thead/tr"));
            if (!rows.Any())
            {
                Assert.Fail("Table does not appear to have a header row, so no columns can be located.");
            }

            var headerRow = rows.Single(); // header row should only ever have a single row.
            var columns = headerRow.FindElements(By.TagName("th"));

            var columnIndex = columns.TakeWhile(x => x.Text != columnName &&
                                                     x.Text != columnName + SORTING_TEXT
                                                     && x.Text != columnName + SORTING_ASCENDING_TEXT &&
                                                     x.Text != columnName + SORTING_DESCENDING_TEXT).Count();

            if (columnIndex == columns.Count())
            {
                Assert.Fail(
                    $"Column Header: {columnName} does not exist in table {table}. Page text: {WebDriverHelper.Current.PageSource}");
            }

            var columnHeader = columns.Skip(columnIndex).Take(1).Single();

            return (Index: columnIndex, HeaderCell: columnHeader);
        }

        public static bool TextExistsInColumn(string columnName, string text, IBetterWebElement table,
            out IBetterWebElement outCell, bool matchWholeColumn = false, int rowIndex = -1)
        {
            outCell = null;
            var columnIndex = GetColumnIndexByName(table, columnName).Index;

            Func<IBetterWebElement, IBetterWebElement> getCellIfTextMatches = (row) => {
                var allCells = row.FindElements(By.TagName("td")).ToList();
                if (allCells.Count > columnIndex)
                {
                    var cell = allCells[columnIndex];
                    // A cell's text could be empty and we could be checking that the cell is empty.
                    var cellText = (cell.Text ?? string.Empty).Trim();
                    if ((matchWholeColumn && cellText == text) || (!matchWholeColumn && cellText.Contains(text)))
                    {
                        return cell;
                    }
                }

                return null;
            };

            var tableRows = table.FindElements(By.TagName("tr")).ToList();
            if (rowIndex > -1)
            {
                if (tableRows.Count <= rowIndex)
                {
                    Assert.Fail($"Unable to find row index {rowIndex} for table {table}.");
                }

                var row = tableRows[rowIndex];
                outCell = getCellIfTextMatches(row);
            }
            else
            {
                // We're doing it this way because table.FindRow doesn't trim
                // whitespace when comparing values.
                foreach (var r in tableRows)
                {
                    outCell = getCellIfTextMatches(r);
                    if (outCell != null)
                    {
                        break;
                    }
                }
            }

            return (outCell != null);
        }

        public static IBetterWebElement AssertTextInNamedColumnInRow(string nopteraptor, string objectType,
            string instanceName, string propertyName, string columnName, IBetterWebElement tableRow,
            Func<object, string> formatValue)
        {
            IBetterWebElement cell;
            // only reload if nopteraptor is "should", because otherwise the
            // entity could have been deleted from the site's database
            var should = StepHelper.IsTruthy(nopteraptor);

            var rawValue = GetRawValue(objectType, instanceName, propertyName, should);
            rawValue.ThrowIfNull("rawValue",
                $"The '{propertyName}' property on the {objectType} instance '{instanceName}' did not return a value.");
            var rawText = rawValue.ToString();
            var formattedText = formatValue(rawValue);

            if (should)
            {
                if (!TextExistsInColumnInRow(columnName, formattedText, tableRow, out cell))
                {
                    Assert.Fail(
                        $"No row exists with '{formattedText}' in the {columnName} column.  Page text: {WebDriverHelper.Current.PageSource}");
                }

                if (rawText != formattedText && TextExistsInColumnInRow(columnName, rawText, tableRow, out cell))
                {
                    Assert.Fail(
                        $"Found a row with {objectType} \"{instanceName}\"'s {propertyName} in the {columnName} column, but it's formatted incorrectly.  Expected \"{formattedText}\"; actual \"{rawValue}\".");
                }
            }
            else
            {
                if (TextExistsInColumnInRow(columnName, formattedText, tableRow, out cell))
                {
                    Assert.Fail(
                        $"The row exists with '{formattedText}' in the {columnName} column.  Page text: {WebDriverHelper.Current.PageSource}");
                }
            }

            return cell;
        }

        public static IBetterWebElement AssertTextInNamedColumn(string nopteraptor, object value, string columnName,
            string tableId, Func<object, string> formatValue = null, int rowIndex = -1)
        {
            var matchWholeColumn = formatValue != null;
            formatValue = formatValue ?? (o => o.ToString());
            var rawValue = value.ToString();
            IBetterWebElement cell;
            var formattedText = formatValue(value);
            var should = StepHelper.IsTruthy(nopteraptor);
            if (should)
            {
                if (!TextExistsInColumn(columnName, formattedText, tableId, out cell, matchWholeColumn, rowIndex))
                {
                    Assert.Fail(
                        $"No row exists with '{formattedText}' in the {columnName} column.  Page text: {WebDriverHelper.Current.PageSource}");
                }

                if (rawValue != formattedText &&
                    TextExistsInColumn(columnName, rawValue, tableId, out cell, matchWholeColumn, rowIndex))
                {
                    Assert.Fail(
                        $"Found a row with the expected string in the {columnName} column, but it's formatted incorrectly.  Expected \"{formattedText}\"; actual \"{value}\".");
                }
            }
            else
            {
                if (TextExistsInColumn(columnName, formattedText, tableId, out cell, matchWholeColumn, rowIndex))
                {
                    Assert.Fail(
                        $"The row exists with '{formattedText}' in the {columnName} column.  Page text: {WebDriverHelper.Current.PageSource}");
                }
            }

            return cell;
        }

        public static IBetterWebElement AssertTextInNamedColumn(string nopteraptor, string objectType,
            string instanceName, string propertyName, string columnName, string tableId,
            Func<object, string> formatValue = null, int rowIndex = -1, dynamic parentObject = null)
        {
            var should = StepHelper.IsTruthy(nopteraptor);

            var matchWholeColumn = formatValue != null;
            formatValue = formatValue ?? (o => o.ToString());
            IBetterWebElement cell;
            // only reload if nopteraptor is "should", because otherwise the
            // entity could have been deleted from the site's database
            var rawValue = GetRawValue(objectType, instanceName, propertyName, should);
            rawValue.ThrowIfNull("rawValue",
                $"The '{propertyName}' property on the {objectType} instance '{instanceName}' did not return a value.");
            var rawText = rawValue.ToString();
            var formattedText = formatValue(rawValue);
            if (should)
            {
                if (!TextExistsInColumn(columnName, formattedText, tableId, out cell, matchWholeColumn, rowIndex,
                    parentObject))
                {
                    Assert.Fail(
                        $"No row exists with '{formattedText}' in the {columnName} column.  Page text: {WebDriverHelper.Current.PageSource}");
                }

                if (rawText != formattedText && TextExistsInColumn(columnName, rawText, tableId, out cell,
                    matchWholeColumn, rowIndex, parentObject))
                {
                    Assert.Fail(
                        $"Found a row with {objectType} \"{instanceName}\"'s {propertyName} in the {columnName} column, but it's formatted incorrectly.  Expected \"{formattedText}\"; actual \"{rawValue}\".");
                }
            }
            else
            {
                if (TextExistsInColumn(columnName, formattedText, tableId, out cell, matchWholeColumn, rowIndex,
                    parentObject))
                {
                    Assert.Fail(
                        $"The row exists with '{formattedText}' in the {columnName} column.  Page text: {WebDriverHelper.Current.PageSource}");
                }
            }

            return cell;
        }

        public static string GetLinkForNamedEntity(string action, string className,
            string namedEntity, out IBetterWebElement link)
        {
            var baseUrl = Navigation.GetUriFor(action, className, namedEntity).ToString();
            link = WebDriverHelper.Current.FindElements(ByHelper.Href(baseUrl)).FirstOrDefault();
            return baseUrl;
        }

        public static string GetLinkForPartialUrl(string action, out IBetterWebElement link)
        {
            var baseUrl = Navigation.GetUriForPage(action).ToString();
            link = WebDriverHelper.Current.FindElements(ByHelper.Href(baseUrl)).SingleOrDefault();
            return baseUrl;
        }

        public static string GetLinkForPartialUrl(string action, string queryMess, out IBetterWebElement link)
        {
            // I have literally no idea what this is doing. -Ross 5/15/2017

            var segmentRgx = new Regex("([^ ]+)=([^\" ]+: \"[^\"]+\"'s [^ ]+|[^ :]+)", RegexOptions.Compiled);
            var namedItemRgx = new Regex("([^\" ]+): \"([^\"]+)\"'s ([^ ]+)", RegexOptions.Compiled);
            var baseUrl = Navigation.GetUriForPage(action).ToString();
            var qs = new List<string>();
            foreach (var segment in queryMess.Split("&".ToCharArray()))
            {
                var match = segmentRgx.Match(segment.ToString());
                var key = match.Groups[1].Value;
                var value = match.Groups[2].Value;
                if (!namedItemRgx.IsMatch(value))
                {
                    qs.Add($"{key}={value}");
                }
                else
                {
                    var itemMatch = namedItemRgx.Match(value);
                    var type = itemMatch.Groups[1].Value;
                    var name = itemMatch.Groups[2].Value;
                    var propertyName = itemMatch.Groups[3].Value;
                    var namedItem = Data.GetCachedEntity(type, name);
                    qs.Add($"{key}={ObjectExtensions.GetPropertyValueByName(namedItem, propertyName)}");
                }
            }

            var expected = string.Format("{0}?{1}", baseUrl, string.Join("&", qs));
            link = WebDriverHelper.Current.FindElements(ByHelper.Href(expected)).SingleOrDefault();
            return expected;
        }

        #endregion
    }

    public class FormatHelper : NameValueCollection
    {
        #region Exposed Methods

        public string FormatValue(object value, string formatName)
        {
            return String.Format(this[formatName], value);
        }

        #endregion
    }
}
