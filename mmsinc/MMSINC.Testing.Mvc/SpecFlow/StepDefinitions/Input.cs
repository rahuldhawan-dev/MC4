using DeleporterCore.Client;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Helpers;
using MMSINC.Testing.ClassExtensions.StringExtensions;
using MMSINC.Testing.SeleniumMvc;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc;
using TechTalk.SpecFlow;

namespace MMSINC.Testing.SpecFlow.StepDefinitions
{
    [Binding]
    public static class Input
    {
        public static readonly Regex FROM_NOW_REGEX = new Regex(@"(\d+) days from now", RegexOptions.Compiled);

        #region Steps

        #region Frames

        [When("I switch to the ([^ ]+) frame$")]
        public static void ISwitchToTheFrame(string frame)
        {
            // So switching the driver to a frame makes the whole driver use that frame.
            WebDriverHelper.Current.SwitchToFrame(frame);
        }
        
        [When("I leave the frame")]
        public static void ILeaveTheFrame()
        {
            WebDriverHelper.Current.LeaveFrame();
        }

        #endregion

        #region Value Elements (Inputs, Selects, etc.)

        [When("^I type \"([^\"]+)\" into the ([^ ]+) field in frame ([^ ]+) for authorizenet$")]
        public static void WhenISpecificallyTypeInAnAuthorizeNetField(string data, string field, string frame)
        {
            // So Authorize.net has some bizarre stuff going on when you type into their textboxes.
            // There's a lot of keypress or other events connected to their textboxes that changes
            // the formatting, validation, and automatically moving the textbox focus to another
            // textbox. Our usual "I type a value" step does not work for this as something with
            // authorize.net's scripting causes some of the stuff we type to get lost. Doing this
            // one letter at a time seems to solve that problem.

            WebDriverHelper.Current.SwitchToFrame(frame);
            try
            {
                var textBox = ElementHelper.FindElementByIdOrName(field);

                foreach (var c in data)
                {
                    textBox.TypeValue(c.ToString(), false);
                }
            }
            finally
            {
                WebDriverHelper.Current.LeaveFrame();
            }
        }

        [When("^I enter \"([^\"]+)\" into the ([^ ]+) field in frame ([^ ]+)$")]
        public static void WhenIEnterDataInAFieldInAFrame(string data, string field, string frame)
        {
            // So switching the driver to a frame makes the whole driver use that frame.
            // We need a "And I switch to frame" and "I leave the frame" type steps
            WebDriverHelper.Current.SwitchToFrame(frame);
            try
            {
                var textBox = ElementHelper.FindElementByIdOrName(field);
                textBox.SetValue(data);
            }
            finally
            {
                WebDriverHelper.Current.LeaveFrame();
            }
        }

        [When("I (enter|type) ([^\"]+) into the (.+) field in frame ([^ ]+)$")]
        public static void WhenIEnterSpecialDataInAFieldInAFrame(string action, string data, string field, string frame)
        {
            WhenIEnterDataInAFieldInAFrame(ParseSpecialString(data), field, frame);
        }

        [When(
            "I enter ([^\"]+) \"([^\"]+)\"'s ([^ ]+) and select ([^\"]+) \"([^\"]+)\"'s ([^ ]+) from the (.+) autocomplete(?: field)?")]
        public static void IEnterNamedValueIntoAutoCompleteAndSelectNamedDataFromList(string firstType,
            string firstName,
            string firstDataMemberName, string secondType, string secondName, string secondDataMemberName,
            string fieldName)
        {
            var data = Data.GetCachedEntityPropertyValue(firstType, firstName, firstDataMemberName);
            IEnterValueIntoAutoCompleteAndSelectNamedDataFromList(data, secondType, secondName, secondDataMemberName,
                fieldName);
        }

        [When("I enter \"([^\"]+)\" and select ([^\"]+) \"([^\"]+)\"'s ([^ ]+) from the (.+) autocomplete(?: field)?")]
        public static void IEnterValueIntoAutoCompleteAndSelectNamedDataFromList(string typedValue, string type,
            string name, string dataMemberName, string fieldName)
        {
            var data = Data.GetCachedEntityPropertyValue(type, name, dataMemberName);
            IEnterValueIntoAutoCompleteAndSelectFromList(typedValue, data, fieldName);
        }

        [Given("I have entered \"([^\"]*)\" and selected \"([^\"]*)\" from the (.+) autocomplete field")]
        [When("I enter \"([^\"]*)\" and select \"([^\"]*)\" from the (.+) autocomplete field")]
        public static void IEnterValueIntoAutoCompleteAndSelectFromList(string typedValue, string selectValue,
            string fieldName)
        {
            var autoCompleteFieldName = fieldName + "_" + AutoCompleteBuilder.AUTOCOMPLETE_FIELD_ADDITION;

            // This needs to be typed, not entered. Typing focuses the textbox and initializes the
            // unobtrusive initializer thingy for autocompletes.
            // Also this HAS to be an id lookup due to the script below.
            var autoCompleteField = WebDriverHelper.Current.FindElement(By.Id(autoCompleteFieldName));
            autoCompleteField.TypeValue(typedValue);

            // We need to pause for a second because there's an async 300ms delay between input and
            // when the jquery autocomplete fires off the search for results. Without the delay, there
            // ends up being a weird moment where, sometimes, the autocomplete fires off a change event
            // with a null selected value, even though just before it sent out a select event with the
            // expected value. The only place I saw this happening was the WorkOrder features that were
            // setting NearestCrossStreet.
            Thread.Sleep(1000);
            // This is so very gross. I hate you, jQuery UI.
            var script = new StringBuilder();
            script.Append("var keyEvent = $.Event('keydown');");
            script.Append("keyEvent.keyCode = $.ui.keyCode.DOWN;");
            script.Append("var input = $(arguments[0]);");
            script.Append("for(var i = 0; i < 100; i++) {");
            script.AppendFormat(" if (input.val() != '{0}') {{ input.trigger(keyEvent); }} else {{ break; }}",
                selectValue);
            script.Append("}");
            script.Append("keyEvent = $.Event('keydown');");
            script.Append("keyEvent.keyCode = $.ui.keyCode.ENTER;");
            script.Append("input.trigger(keyEvent);");

            WebDriverHelper.Current.ExecuteScript(script.ToString(), autoCompleteField.InternalElement);
            Assert.AreEqual(selectValue, autoCompleteField.GetValue());
        }

        [When("I enter \"([^\"]*)\" not expecting to see \"([^\"]*)\" in the (.+) autocomplete field")]
        public static void IEnterValueNotExpectingToSeeValueInAutocompleteField(string typedValue, string selectValue,
            string fieldName)
        {
            var autoCompleteFieldName = fieldName + "_" + AutoCompleteBuilder.AUTOCOMPLETE_FIELD_ADDITION;

            // AS ABOVE:
            // This needs to be typed, not entered. Typing focuses the textbox and initializes the
            // unobtrusive initializer thingy for autocompletes.
            // Also this HAS to be an id lookup due to the script below.
            var autoCompleteField = WebDriverHelper.Current.FindElement(By.Id(autoCompleteFieldName));

            if (!autoCompleteField.IsEnabled)
            {
                Assert.Fail(
                    $"Autocomplete field '{fieldName}' is currently not enabled, and thus cannot be typed into.");
            }

            autoCompleteField.TypeValue(typedValue);

            // This is also so very gross. Boo jQuery
            var script = new StringBuilder();
            script.Append("var keyEvent = $.Event('keydown');");
            script.Append("keyEvent.keyCode = $.ui.keyCode.DOWN;");
            script.AppendFormat("var input = $(arguments[0]);");
            script.Append("for(var i = 0; i < 100; i++) {");
            script.AppendFormat(" if (input.val() != '{0}') {{ input.trigger(keyEvent); }} else {{ return true; }}",
                selectValue);
            script.Append("}");
            script.Append("return false;");

            var valueSeen =
                (bool)WebDriverHelper.Current.ExecuteScript(script.ToString(), autoCompleteField.InternalElement);
            Assert.IsFalse(valueSeen, $"Value '{selectValue}' was seen unexpectedly");
        }

        [When(
            "I enter ([^\"]+) \"([^\"]+)\"'s ([^ ]+) and select ([^\"]+) \"([^\"]+)\"'s ([^ ]+) from the (.+) combo ?box")]
        public static void IEnterNamedValueIntoComboBoxAndSelectNamedDataFromList(string firstType, string firstName,
            string firstDataMemberName, string secondType, string secondName, string secondDataMemberName,
            string fieldName)
        {
            var data = Data.GetCachedEntityPropertyValue(firstType, firstName, firstDataMemberName);
            IEnterValueIntoComboBoxAndSelectNamedDataFromList(data, secondType, secondName, secondDataMemberName,
                fieldName);
        }

        [When("I enter \"([^\"]+)\" and select ([^\"]+) \"([^\"]+)\"'s ([^ ]+) from the (.+) combo ?box")]
        public static void IEnterValueIntoComboBoxAndSelectNamedDataFromList(string typedValue, string type,
            string name,
            string dataMemberName, string fieldName)
        {
            var data = Data.GetCachedEntityPropertyValue(type, name, dataMemberName);
            IEnterValueIntoComboBoxAndSelectFromList(typedValue, data, fieldName);
        }

        [Given("I have entered \"([^\"]*)\" and selected \"([^\"]*)\" from the (.+) combobox")]
        [When("I enter \"([^\"]*)\" and select \"([^\"]*)\" from the (.+) combobox")]
        public static void IEnterValueIntoComboBoxAndSelectFromList(string typedValue, string selectValue,
            string fieldName)
        {
            var outerDiv = WebDriverHelper.Current.FindElement(By.CssSelector("div#" + fieldName));
            var label = outerDiv.FindElement(By.CssSelector("div.inner a.label"));

            // Need a reference to this before the combobox is activated, because it's gonna
            // be detached and reattached elsewhere.
            var comboBoxShell = outerDiv.FindElement(By.ClassName("multilist-shell"));

            label.Click();

            // Need to wait for animation to complete before the textbox is focused.
            IWaitForAnimationsToComplete();

            var comboBoxSearchField = comboBoxShell.FindElement(By.CssSelector("input[type=text]"));
            comboBoxSearchField.TypeValue(typedValue);

            // The "entering" doesn't focus the textbox, which is what triggers the unobtrusive autocomplete
            // to actually initialize.
            comboBoxSearchField.TriggerJQueryEvent(JQueryEventType.Focus);

            // There's a delay in the script between the value being entered and the request
            // to the server. The ajax is done synchronously though so there shouldn't be race
            // conditions.
            WebDriverHelper.Current.WaitUntil(() => comboBoxShell.FindElements(By.TagName("ul")).Count() == 1);
            var comboBoxList = comboBoxShell.FindElement(By.TagName("ul"));

            // ByHelper.PartialTextInTag doesn't work well here if there's more than one item that matches.
            // ByHelper.TextInTag doesn't work here because it uses XPath and can't deal with the extra
            // whitespace that is showing up in these items for some reason.
            var listItemLink = comboBoxList.FindElements(By.TagName("a")).Single(x => x.Text == selectValue);
            if (!listItemLink.IsDisplayed)
            {
                Assert.Fail(
                    $"The item you're trying to select is not visible. You typed \"{typedValue}\" and tried to select \"{selectValue}\"");
            }

            listItemLink.Click();
            Assert.AreEqual(selectValue, label.FindElement(By.TagName("span")).Text.Trim());
        }

        [Given("I have (enter|type)e?d \"([^\"]*)\" into the (.+) field")]
        [Given("I have (select)ed \"([^\"]*)\" from the (.+) dropdown")]
        [Given("I have (select)ed \"([^\"]*)\" from the (.+) multiselect")]
        [Given("I have (check)ed \"([^\"]*)\" in the (.+) checkbox list")]
        [Given("I have (uncheck)ed \"([^\"]*)\" in the (.+) checkbox list")]
        [When("I (enter|type) \"([^\"]*)\" into the (.+) field")]
        [When("I (select) \"([^\"]*)\" from the (.+) dropdown")]
        [When("I (select) \"([^\"]*)\" from the (.+) multiselect")]
        [When("I (check) \"([^\"]*)\" in the (.+) checkbox list")]
        [When("I (uncheck) \"([^\"]*)\" in the (.+) checkbox list")]
        public static void WhenIEnterDataInAField(string action, string data, string field)
        {
            switch (action)
            {
                case "check":
                    CheckOrUncheckCheckboxInCheckboxListByTextOrValue(field, data, true);
                    break;
                case "uncheck":
                    CheckOrUncheckCheckboxInCheckboxListByTextOrValue(field, data, false);
                    break;
                case "enter":
                    data = ParseSpecialString(data);
                    EnterValueInField(field, data);
                    break;
                case "type":
                    TypeValueInField(field, data);
                    break;
                case "select":
                    SelectTextInField(field, data);
                    break;
                default:
                    throw new NotSupportedException(action);
            }

            IWaitForAjaxToFinishLoading();
        }

        [Given("I have (enter|type)e?d \"([^\"]*)\" into the (.+) field in the (.+) form")]
        [Given("I have (select)ed \"([^\"]*)\" from the (.+) dropdown in the (.+) form")]
        [Given("I have (select)ed \"([^\"]*)\" from the (.+) multiselect in the (.+) form")]
        [Given("I have (check)ed \"([^\"]*)\" in the (.+) checkbox list in the (.+) form")]
        [Given("I have (uncheck)ed \"([^\"]*)\" in the (.+) checkbox list in the (.+) form")]
        [When("I (enter|type) \"([^\"]*)\" into the (.+) field in the (.+) form")]
        [When("I (select) \"([^\"]*)\" from the (.+) dropdown in the (.+) form")]
        [When("I (select) \"([^\"]*)\" from the (.+) multiselect in the (.+) form")]
        [When("I (check) \"([^\"]*)\" in the (.+) checkbox list in the (.+) form")]
        [When("I (uncheck) \"([^\"]*)\" in the (.+) checkbox list in the (.+) form")]
        public static void WhenIEnterDataInAFieldInAForm(string action, string data, string field, string form)
        {
            switch (action)
            {
                case "check":
                    CheckOrUncheckCheckboxInCheckboxListByTextOrValue(field, data, true, form);
                    break;
                case "uncheck":
                    CheckOrUncheckCheckboxInCheckboxListByTextOrValue(field, data, false, form);
                    break;
                case "enter":
                    EnterValueInField(field, data, form);
                    break;
                case "type":
                    TypeValueInField(field, data, form);
                    break;
                case "select":
                    SelectTextInField(field, data, form);
                    break;
                default:
                    throw new NotSupportedException($"Do what now? {action}");
            }
        }

        [Given(
            "I have (enter|type)e?d (?!the current hostname)(?!the date)(?!the path)([^\"]+) \"([^\"]+)\" into the (.+) (?:field|text box)")]
        [Given("I have (select)ed ([^\"]+) \"([^\"]+)\" from the (.+) dropdown")]
        [Given("I have (select)ed ([^\"]+) \"([^\"]+)\" from the (.+) multiselect")]
        [Given("I have (check)ed ([^\"]+) \"([^\"]+)\" in the (.+) checkbox list")]
        [Given("I have (uncheck)ed ([^\"]+) \"([^\"]+)\" in the (.+) checkbox list")]
        [When(
            "I (enter|type) (?!the current hostname)(?!the date)(?!the path)([^\"]+) \"([^\"]+)\" into the (.+) (?:field|text box)")]
        [When("I (select) ([^\"]+) \"([^\"]+)\" from the (.+) dropdown")]
        [When("I (select) ([^\"]+) \"([^\"]+)\" from the (.+) multiselect")]
        [When("I (check) ([^\"]+) \"([^\"]+)\" in the (.+) checkbox list")]
        [When("I (uncheck) ([^\"]+) \"([^\"]+)\" in the (.+) checkbox list")]
        public static void WhenIEnterNamedDataInAField(string action, string type, string name, string fieldName)
        {
            WhenIEnterNamedDataInAField(action, type, name, "ToString", fieldName);
        }

        [When("^I (select) ([^\"]+) \"([^\"]+)\" from the (.+) dropdown in the (.+) form$")]
        [When("^I (select) ([^\"]+) \"([^\"]+)\" from the (.+) multiselect in the (.+) form$")]
        public static void WhenIEnterNamedDataInAFieldInAForm(string action, string type, string name, string fieldName,
            string form)
        {
            var data = Data.GetCachedEntityPropertyValue(type, name, "ToString");
            WhenIEnterDataInAFieldInAForm(action, data, fieldName, form);
        }

        [Given("I have (enter|type)e?d ([^\"]+) \"([^\"]+)\"'s? ([^ ]+) into the (.+) (?:field|text box)")]
        [Given("I have (select)ed ([^\"]+) \"([^\"]+)\"'s? ([^ ]+) from the (.+) dropdown")]
        [Given("I have (select)ed ([^\"]+) \"([^\"]+)\"'s? ([^ ]+) from the (.+) multiselect")]
        [Given("I have (check)ed ([^\"]+) \"([^\"]+)\"'s? ([^ ]+) in the (.+) checkbox list")]
        [Given("I have (uncheck)ed ([^\"]+) \"([^\"]+)\"'s? ([^ ]+) in the (.+) checkbox list")]
        [When("I (enter|type) ([^\"]+) \"([^\"]+)\"'s? ([^ ]+) into the (.+) (?:field|text box)")]
        [When("I (select) ([^\"]+) \"([^\"]+)\"'s? ([^ ]+) from the (.+) dropdown")]
        [When("I (select) ([^\"]+) \"([^\"]+)\"'s? ([^ ]+) from the (.+) multiselect")]
        [When("I (check) ([^\"]+) \"([^\"]+)\"'s? ([^ ]+) in the (.+) checkbox list")]
        [When("I (uncheck) ([^\"]+) \"([^\"]+)\"'s? ([^ ]+) in the (.+) checkbox list")]
        public static void WhenIEnterNamedDataInAField(string action, string type, string name, string dataMemberName,
            string fieldName)
        {
            var data = Data.GetCachedEntityPropertyValue(type, name, dataMemberName);
            WhenIEnterDataInAField(action, data, fieldName);
        }

        [Given("^I have (enter|type)e?d the current hostname and \"([^\"]+)\" into the (.+) (?:field|text box)$"),
         When("^I (enter|type) the current hostname and \"([^\"]+)\" into the (.+) (?:field|text box)$")]
        public static void WhenIEnterTheCurrentHostnameAndDataInAField(string action, string data, string field)
        {
            WhenIEnterDataInAField(action, data.PrependCurrentHostname(), field);
        }

        [When("I (enter|type) ([^\"]+) into the (.+) (?:field|text box)")]
        public static void WhenIEnterSpecialDataInAField(string action, string data, string field)
        {
            WhenIEnterDataInAField(action, ParseSpecialString(data), field);
        }

        [When("I (enter|type) ([^\"]+) into the (.+) (?:field|text box) in the (.+) form")]
        public static void WhenIEnterSpecialDataInAFieldInaForm(string action, string data, string field, string form)
        {
            WhenIEnterDataInAFieldInAForm(action, ParseSpecialString(data), field, form);
        }

        [When("I (enter|type) the date \"([^\"]*)\" into the (.+) (?:field|text box)")]
        public static void WhenIEnterADateInAField(string action, string data, string field)
        {
            WhenIEnterDataInAField(action, data.ToDateTime().ToShortDateString(), field);
        }

        [When("^I set (.+) to")]
        public static void WhenISetFieldToMultilineText(string field, string multilineText)
        {
            EnterValueInField(field, multilineText);
        }

        [When("^I (check|uncheck) the (.+) (?:field|checkbox)$")]
        public static void WhenICheckOrUncheckTheField(string check, string fieldName)
        {
            var chk = ElementHelper.FindElementByIdNameOrSelector(fieldName);
            chk.Check(check == "check");
        }

        [When("^I check the (.+) radio button with the value (.+)")]
        public static void WhenICheckTheRadioButton(string radio, string value)
        {
            var rdo = WebDriverHelper.Current.FindElement(By.CssSelector($"[name='{radio}'][value='{value}']"));

            rdo.Click();
        }

        [When("^I click the checkbox named ([^\\s]+) with the value \"([^\"]+)\"$")]
        public static void WhenIClickTheCheckbox(string name, string value)
        {
            var cb = WebDriverHelper.Current.FindElements(By.Name(name));
            Assert.IsTrue(cb.Any(), $"There are no checkboxes with the name '{name}'.");

            var cbWithValue = cb.Single(x => x.GetValue() == ParseSpecialString(value));
            cbWithValue.Check(true);
        }

        [When("^I click the checkbox named ([^\\s]+) with ([^\"]+) \"([^\"]+)\"'s ([^ ]+)$")]
        public static void WhenIClickTheCheckbox(string checkboxName, string type, string name, string dataMemberName)
        {
            var data = Data.GetCachedEntityPropertyValue(type, name, dataMemberName);
            WhenIClickTheCheckbox(checkboxName, data);
        }

        [When("^I click the checkbox named ([^\\s]+) with ([^\"]+) \"([^\"]+)\"'s ([^ ]+) under the \"([^\"]+)\" tab$")]
        public static void WhenIClickTheCheckboxUnderTheTab(string checkboxName, string type, string name,
            string dataMemberName, string tabText)
        {
            var tabDiv = ElementHelper.FindTabContentContainerByTabText(tabText);
            var value = Data.GetCachedEntityPropertyValue(type, name, dataMemberName);
            var box = tabDiv.FindElement(By.CssSelector($"[name='{checkboxName}'][value='{value}']"));
            box.Check(true);
        }

        [Given("^I have uploaded \"([^\"]+)\"$")]
        [When("^I upload \"([^\"]+)\"$")]
        public static void WhenIUploadAFile(string fileName)
        {
            WhenIUploadAFileToTheSpecificField(fileName, "file");
        }

        [Given("I have uploaded \"([^\"]+)\" to the ([^\"]+) field")]
        [When("I upload \"([^\"]+)\" to the ([^\"]+) field")]
        public static void WhenIUploadAFileToTheSpecificField(string fileName, string fieldName)
        {
            var input = WebDriverHelper.Current.FindElements(By.Name(fieldName)).Single();
            // var currentDirectory = Directory.GetCurrentDirectory().ToLower().Replace("\\bin\\debug", "");

            var currentDirectory = TestContext.CurrentContext.TestDirectory.ToLower()
                                              .ReplaceRegex(@"\\bin(?:\\x64)?\\debug", "");

            // This file needs to be local to the compiled project, so the image
            // is set to copy to the output dir.
            var path = Path.Combine(currentDirectory, fileName);
            // WebDriver/ChromeDriver doesn't support file paths that contain "../.." type stuff in it. 
            // Path.GetFullPath returns the resolved path for those.
            path = Path.GetFullPath(path);
            input.SetFileUploadPath(path);

            // This doesn't actually prove that the file uploaded, it just means 
            // that the file got selected to begin uploading.
            var successList = WebDriverHelper.Current.FindElements(By.ClassName("qq-upload-list")).Single();
            //            successList.WaitUntilExists(10);
            Assert.IsTrue(successList.Text.Contains(Path.GetFileName(Path.GetFileName(fileName))),
                "File name not found in successful uploads list.");

            // Last thing's last, make sure the animation is done because that's what actually means
            // the upload finished.

            // This stupid element does not get added until you click the button and actually
            // begin uploading, then it never goes away, it only hides.
            var spinner = WebDriverHelper.Current.FindElements(By.ClassName("qq-upload-spinner")).Single();

            WebDriverHelper.Current.WaitUntil(() => !spinner.IsDisplayed);
        }

        [Given("I have multi uploaded \"([^\"]+)\" to the ([^\"]+) field")]
        [When("I multi upload \"([^\"]+)\" to the ([^\"]+) field")]
        public static void WhenIMultiUploadAFileToTheSpecificField(string fileName, string fieldName)
        {
            var input = WebDriverHelper.Current.FindElements(By.Name(fieldName)).FirstOrDefault();
            // var currentDirectory = Directory.GetCurrentDirectory().ToLower().Replace("\\bin\\debug", "");

            var currentDirectory = TestContext.CurrentContext.TestDirectory.ToLower()
                                              .ReplaceRegex(@"\\bin(?:\\x64)?\\debug", "");

            // This file needs to be local to the compiled project, so the image
            // is set to copy to the output dir.
            var path = Path.Combine(currentDirectory, fileName);
            // WebDriver/ChromeDriver doesn't support file paths that contain "../.." type stuff in it. 
            // Path.GetFullPath returns the resolved path for those.
            path = Path.GetFullPath(path);
            input.SetFileUploadPath(path);

            // This doesn't actually prove that the file uploaded, it just means 
            // that the file got selected to begin uploading.
            var successList = WebDriverHelper.Current.FindElements(By.ClassName("qq-upload-list")).FirstOrDefault();
            //            successList.WaitUntilExists(10);
            Assert.IsTrue(successList.Text.Contains(Path.GetFileName(Path.GetFileName(fileName))),
                "File name not found in successful uploads list.");

            // Last thing's last, make sure the animation is done because that's what actually means
            // the upload finished.

            // This stupid element does not get added until you click the button and actually
            // begin uploading, then it never goes away, it only hides.
            var spinner = WebDriverHelper.Current.FindElements(By.ClassName("qq-upload-spinner")).FirstOrDefault();

            WebDriverHelper.Current.WaitUntil(() => !spinner.IsDisplayed);
        }
        #endregion

        #region Buttons, Links, and Tabs

        [Given("^I have pressed \"([^\"]+)\"$"),
         Given("^I have pressed ([^\"\\s]+)$"),
         When("^I press \"([^\"]+)\"$"),
         When("^I press ([^\"\\s]+)$")]
        public static void WhenIPressAButton(string buttonText)
        {
            buttonText = StripQuotationMarks(buttonText);
            PressButton(buttonText);

            State.AssertThereIsNoYSOD($"Clicking the button '{buttonText}' resulted in a YSOD!");
        }

        [Given("^I have pressed one of the \"([^\"]+)\" buttons$"),
         When("^I press one of the \"([^\"]+)\" buttons$")]
        public static void WhenIPressOneOfTheButtons(string buttonText)
        {
            buttonText = StripQuotationMarks(buttonText);
            PressOneOfTheButtons(buttonText);

            State.AssertThereIsNoYSOD($"Clicking the button '{buttonText}' resulted in a YSOD!");
        }

        [Given("^I have pressed ([^\\s]+) from tab ([^\\s]+)$"),
         Given("^I have pressed \"([^\"]+)\" from tab ([^\\s]+)$"),
         When("^I press ([^\\s]+) from tab ([^\\s]+)$"),
         When("^I press \"([^\"]+)\" from tab ([^\\s]+)$")]
        public static void WhenIPressAButtonFromATab(string buttonText, string tab)
        {
            try
            {
                var tabContainer = ElementHelper.FindTabContentContainerByTabText(tab);
                PressButton(buttonText, tabContainer);
            }
            finally
            {
                // Ensure that we leave the frame context!
                WebDriverHelper.Current.LeaveFrame();
            }
        }

        [Given("^I have pressed ([^\\s]+) in frame ([^\\s]+)$"),
         Given("^I have pressed \"([^\"]+)\" in frame ([^\\s]+)$"),
         When("^I press ([^\\s]+) in frame ([^\\s]+)$"),
         When("^I press \"([^\"]+)\" in frame ([^\\s]+)$")]
        public static void WhenIPressAButtonInAFrame(string buttonText, string frame)
        {
            WebDriverHelper.Current.SwitchToFrame(frame);
            try
            {
                PressButton(buttonText);
            }
            finally
            {
                // Ensure that we leave the frame context!
                WebDriverHelper.Current.LeaveFrame();
            }
        }

        [When("^I force press ([^\\s]+) in frame ([^\\s]+) because it is impossible to scroll into view"),
         When("^I force press \"([^\"]+)\" in frame ([^\\s]+) because it is impossible to scroll into view")]
        public static void WhenIForcePressAButtonInAFrame(string buttonText, string frame)
        {
            WebDriverHelper.Current.SwitchToFrame(frame);
            try
            {
                ForcePressButton(buttonText);
            }
            finally
            {
                // Ensure that we leave the frame context!
                WebDriverHelper.Current.LeaveFrame();
            }
        }

        [When("^I follow \"([^\"]*)\" in frame ([^\\s]+)$")]
        public static void WhenIFollowLinkInFrame(string linkText, string frame)
        {
            WebDriverHelper.Current.SwitchToFrame(frame);
            try
            {
                FindVisibleLinkByDisplayText(linkText).Click();
            }
            finally
            {
                // Ensure that we leave the frame context!
                WebDriverHelper.Current.LeaveFrame();
            }
        }

        [When("^I switch to the (first|last) browser tab")]
        public static void WhenISwitchToABrowserTab(string firstOrLastTabDesignator)
        {
            switch (firstOrLastTabDesignator)
            {
                case "first":
                    WebDriverHelper.Current.SwitchToFirstTab();
                    break;

                case "last": 
                    WebDriverHelper.Current.SwitchToLastTab();
                    break;
            }
        }

        [When("^I close the (first|last) browser tab")]
        public static void WhenICloseTheLastBrowserTab(string firstOrLastTabDesignator)
        {
            // get the number of window handles
            var handles = WebDriverHelper.Current.GetNumberOfWindowHandles();
            
            // close the last tab
            
            switch (firstOrLastTabDesignator)
            {
                case "first":
                    WebDriverHelper.Current.CloseFirstTab();
                    break;

                case "last":
                    WebDriverHelper.Current.CloseLastTab();
                    break;
            }

            // wait for the last tab to close
            WebDriverHelper.Current.WaitUntil(() => WebDriverHelper.Current.GetNumberOfWindowHandles() < handles);

            // switch to the previous tab
            WebDriverHelper.Current.SwitchToTab(WebDriverHelper.Current.GetNumberOfWindowHandles() - 1);
            
            // wait for it to reload
            WhenIWaitForThePageToReload();
        }

        // This step exists specifically because authorize.net changed one of their
        // link's text values and it messed up everything.
        [When("^I follow \"([^\"]*)\" by id in frame ([^\\s]+)$")]
        public static void WhenIFollowLinkByIdInFrame(string linkId, string frame)
        {
            WebDriverHelper.Current.SwitchToFrame(frame);
            try
            {
                FindVisibleLinksByConstraint(By.Id(linkId), "Can't find link with id: " + linkId).Single().Click();
            }
            finally
            {
                // Ensure that we leave the frame context!
                WebDriverHelper.Current.LeaveFrame();
            }
        }

        [Given("^I have followed \"([^\"]*)\"")]
        [When("^I follow \"([^\"]*)\"")]
        public static void WhenIFollowLink(string linkText)
        {
            FindVisibleLinkByDisplayText(linkText).Click();

            State.AssertThereIsNoYSOD($"Following the link '{linkText}' resulted in a YSOD!");
        }
        
        [When("^I follow the link \"([^\"]*)\" inside the success message")]
        public static void WhenIFollowLinInsideSuccessMessage(string linkText)
        {
            FindVisibleLinkInSuccessMessage(linkText).Click();

            State.AssertThereIsNoYSOD($"Following the link '{linkText}' resulted in a YSOD!");
        }
        
        [Given("^I have followed one of the \"([^\"]*)\" links")]
        [When("^I follow one of the \"([^\"]*)\" links")]
        public static void WhenIFollowOneOfTheLinks(string linkText)
        {
            FindVisibleLinksByDisplayText(linkText).First().Click();
        }

        [When("^I follow the ([^\"]+) link for ([^\"]+) \"([^\"]+)\"$")]
        public static void WhenIFollowTheLinkFor(string action, string type, string name)
        {
            var namedItem = Data.GetCachedEntity(type, name);
            var baseUrl = Navigation.GetUriFor(action, namedItem, type);
            var link = WebDriverHelper.Current.FindElement(ByHelper.Href(baseUrl.ToString()));
            link.Click();
        }

        [When("^I follow one of the ([^\"]+) links for ([^\"]+) \"([^\"]+)\"$")]
        public static void WhenIFollowOneOfTheLinksFor(string action, string type, string name)
        {
            var namedItem = Data.GetCachedEntity(type, name);
            var baseUrl = Navigation.GetUriFor(action, namedItem, type);
            var link = WebDriverHelper.Current.FindElements(ByHelper.Href(baseUrl.ToString()));
            link.First().Click();
        }

        // 	When I follow the "View" link to the Show page for one call markout ticket "one"
        [When("I follow the \"([^\"]+)\" link to the ([^\"]+) page for ([^\"]+) \"([^\"]+)\"")]
        public static void IFollowTheLinkWithTextToThePageForNamedItem(string linkText, string action, string type,
            string name)
        {
            var namedItem = Data.GetCachedEntity(type, name);
            var baseUrl = Navigation.GetUriFor(action, namedItem);
            var links = WebDriverHelper.Current.FindElements(ByHelper.Href(baseUrl.ToString()));
            if (!links.Any())
            {
                throw new NoSuchElementException($"Link with href '{baseUrl}' and text '{linkText}' not found");
            }
            links.Single(x => x.Text.Contains(linkText)).Click();
        }

        // 	When I follow the "View" link to the Show page for one call markout ticket "one"
        [When("I follow the \"([^\"]+)\" link to the ([^\"]+) page for ([^\"]+) \"([^\"]+)\" and fragment of \"(.+)\"")]
        public static void IFollowTheLinkWithTextToThePageForNamedItemAndFragmentOf(string linkText, string action, string type, string name, string fragment)
        {
            var namedItem = Data.GetCachedEntity(type, name);
            var baseUrl = Navigation.GetUriFor(action, namedItem) + fragment;
            var links = WebDriverHelper.Current.FindElements(ByHelper.Href(baseUrl));
            if (!links.Any())
            {
                throw new NoSuchElementException($"Link with href '{baseUrl}' and text '{linkText}' not found");
            }

            var link = links.Single(x => x.Text.Contains(linkText));

            link.Click();
        }

        [When("^I click ok in the alert after pressing (.+)$")]
        public static void WhenIClickOkInTheAlertAfterPressing(string buttonText)
        {
            IAnswerTheAlertDialog(() => PressButton(buttonText));

            State.AssertThereIsNoYSOD($"Clicking the button '{buttonText}' resulted in a YSOD!");
        }

        [When("^I click (ok|cancel) in the dialog after following \"(.+)\"$")]
        public static void WhenIClickOkInTheDialogAfterFollowing(string ok, string linkText)
        {
            EnsureOkCancel(ok);
            FindVisibleLinkByDisplayText(linkText).Click();
            if (ok == "cancel")
            {
                WebDriverHelper.Current.DismissDialog();
            }
            else
            {
                WebDriverHelper.Current.ConfirmDialog();
            }
        }
        
        [When("^I click (ok|cancel) in the dialog after following \"(.+)\" and wait for the new tab to load$")]
        public static void WhenIClickOKInTheDialogAfterFollowingAndWaitForTheNewTabToLoad(string ok, string linkText)
        {
            // get the number of window handles
            var handles = WebDriverHelper.Current.GetNumberOfWindowHandles();
            
            // go open the tab, the way we normally do
            WhenIClickOkInTheDialogAfterFollowing(ok, linkText);
            
            // wait for the number of window handles to increase meaning the tab was opened
            WebDriverHelper.Current.WaitUntil(() => WebDriverHelper.Current.GetNumberOfWindowHandles() > handles);
            
            // switch to the last tab, even if the browser already did, selenium might not have
            WhenISwitchToABrowserTab("last");
            
            // wait for the tab to load
            WhenIWaitForThePageToReload();
        }

        [When("^I click (ok|cancel) in the dialog after clicking \"(.+)\"$")]
        public static void WhenIClickOKInTheDialogAfterCheckingElement(string ok, string elementId)
        {
            IAnswerTheConfirmDialog(() => WhenICheckOrUncheckTheField("check", elementId), ok);
        }

        [When("^I click (ok|cancel) in the dialog after pressing \"(.+)\"$")]
        public static void WhenIClickOKInTheDialogAfterPressing(string ok, string buttonText)
        {
            IAnswerTheConfirmDialog(() => PressButton(buttonText), ok);

            // NOTE: This backfires if an alert or other confirmation dialog pops up after the
            // first one.
            State.AssertThereIsNoYSOD($"Clicking the button '{buttonText}' resulted in a YSOD!");
        }

        [When("^I click (ok|cancel) in the dialog after pressing \"(.+)\" for an ajax request$")]
        public static void WhenIClickOKInTheDialogAfterPressingAndAYSODWillNeverHappen(string ok, string buttonText)
        {
            IAnswerTheConfirmDialog(() => PressButton(buttonText), ok);
        }

        [When("^I click (ok|cancel) in the dialog after following the \"(.+)\" link for ([^\"]+): \"([^\"]+)\"$")]
        public static void WhenIClickOKInTheDialogAfterFollowingTheLinkFor(string ok, string action, string type,
            string name)
        {
            var namedItem = Data.GetCachedEntity(type, name);
            var baseUrl = Navigation.GetUriFor(action, namedItem);
            IAnswerTheConfirmDialog(() => { FindVisibleLinkByUrl(baseUrl).Click(); }, ok);
        }

        [When("^I click ok in the alert after typing \"([^\"]*)\" into the (.+) field")]
        public static void WhenIClickOkInTheAlertAfterEnteringValueIntoTheField(string data, string field)
        {
            IAnswerTheAlertDialog(() => {
                TypeValueInField(field, data);
                ElementHelper.FindElementByIdNameOrSelector(field).TriggerJQueryEvent(JQueryEventType.Blur);
            });
        }

        [When("^I click the ([0-9]+)(?:st|nd|rd|th) row of ([^\\s]+)$")]
        public static void ClickTheNumberedRowOfATable(int rowIndex,
            string tableId)
        {
            var row = FindTableRowByIndex(rowIndex, tableId);
            row.Click();
        }

        [When("^I click the \"([^\"]+)\" checkbox in the ([0-9]+)(?:st|nd|rd|th) row of ([^\\s]+)$")]
        public static void WhenIClickTheNamedCheckboxInANumberedRowOfAtATable(string checkboxName, int rowIndex,
            string tableId)
        {
            ClickTheNamedCheckboxInANumberedRowOfATable(checkboxName, rowIndex, tableId);
        }

        [When("^I click the \"([^\"]+)\" button in the ([0-9]+)(?:st|nd|rd|th) row of ([^\\s]+)$")]
        public static void WhenIClickTheNamedButtonInANumberedRowOfAtATable(string buttonText, int rowIndex,
            string tableId)
        {
            ClickTheNamedButtonInANumberedRowOfATable(buttonText, rowIndex, tableId);
        }

        [When("^I click the \"([^\"]+)\" link in the ([0-9]+)(?:st|nd|rd|th) row of ([^\\s]+)$")]
        public static void WhenIClickTheNamedLinkInANumberedRowOfAtATable(string linkText, int rowIndex, string tableId)
        {
            ClickTheNamedLinkInANumberedRowOfATable(linkText, rowIndex, tableId);
        }

        [When("^I click the \"([^\"]+)\" link under \"([^\"]+)\" in the ([0-9]+)(?:st|nd|rd|th) row of ([^\\s]+)$")]
        public static void WhenIClickTheNamedLinkInANumberedRowOfAtATable(string linkText, string header, int rowIndex,
            string tableId)
        {
            ClickLinkByTextInCellByHeaderAndRow(linkText, header, rowIndex, tableId);
        }

        [When(
            "^I click the \"([^\"]+)\" button in the ([0-9]+)(?:st|nd|rd|th) row of ([^\\s]+) and then click ([oO][kK]|Cancel) in the confirmation dialog$")]
        public static void WhenIClickTheNamedButtonInANumberedRowOfATableAndThenClickConfirm(string buttonText,
            int rowIndex, string tableId, string ok)
        {
            IAnswerTheConfirmDialog(
                () => ClickTheNamedButtonInANumberedRowOfATable(buttonText, rowIndex, tableId), ok);
        }

        [When(
            "^I click the \"([^\"]+)\" link in the ([0-9]+)(?:st|nd|rd|th) row of ([^\\s]+) and then click ok in the alert$")]
        public static void WhenIClickTheNamedLinkInANumberedRowOfAtATableAndThenClickOk(string linkText, int rowIndex,
            string tableId)
        {
            IAnswerTheAlertDialog(() => ClickTheNamedLinkInANumberedRowOfATable(linkText, rowIndex, tableId));
        }

        [When(
            "^I click the \"([^\"]+)\" link in the ([0-9]+)(?:st|nd|rd|th) row of ([^\\s]+) and then click ok in the confirmation dialog$")]
        public static void WhenIClickTheNamedLinkInANumberedRowOfAtATableAndThenClickConfirm(string linkText,
            int rowIndex, string tableId)
        {
            IAnswerTheConfirmDialog(() => ClickTheNamedLinkInANumberedRowOfATable(linkText, rowIndex, tableId), "ok");
        }

        [When("^I click ok in the alert$")]
        public static void WhenIClickOkInTheDialog()
        {
            WebDriverHelper.Current.ConfirmDialog();
        }

        [When("^I wait for ajax to finish loading$")]
        public static void IWaitForAjaxToFinishLoading()
        {
            WebDriverHelper.Current.WaitForAjaxToComplete();
        }

        [When("I wait for animations to complete")]
        public static void IWaitForAnimationsToComplete()
        {
            // You shouldn't want to use this step, but sometimes it's necessary if 
            // you're doing an element visibility test that involves animations.
            WebDriverHelper.Current.WaitForAnimationsToComplete();
        }

        [Given("^I have clicked the \"([^\"]+)\" tab")]
        [When("^I click the \"([^\"]+)\" tab")]
        public static void WhenIClickTheTab(string text)
        {
            var tab = ElementHelper.TryFindTabByText(text);
            Assert.IsNotNull(tab, $"Tab with the text '{text}' was not found.");
            tab.FindElement(By.TagName("a")).Click();
        }

        [Given("^I have waited for the dialog to (open|close)")]
        [When("^I wait for the dialog to (open|close)")]
        public static void WhenIWAitForDialog(string openClose)
        {
            // This could be a problem if we end up having more than one jQuery dialog running from
            // the ajaxtables script at the same time. We can deal with it when we get there. 
            const string JQUERY_DIALOG_WRAPPER_ID = "dialog-content-wrapper";

            var cur = WebDriverHelper.Current;
            switch (openClose)
            {
                case "open":
                    cur.WaitUntilSingleElementExists(By.Id(JQUERY_DIALOG_WRAPPER_ID));
                    break;

                case "close":
                    cur.WaitUntil(() =>
                        !cur.FindElements(By.Id(JQUERY_DIALOG_WRAPPER_ID)).Any());
                    break;

                default:
                    throw new InvalidOperationException($"What kind of argument is '{openClose}'");
            }
        }

        #endregion

        #region Block Level Elements

        /// <summary>
        /// This step is for draggable tables using jquery.tablednd.js
        /// </summary>
        [When("^I drag the ([0-9]+)(?:st|nd|rd|th) row in the \"(.+)\" table (up|down) ([0-9]+) rows?$")]
        public static void WhenIDragRowInTheTable(int count, string tableId, string direction, int rowsToMove)
        {
            // NOTE: This step/the specific script this drag and drop stuff in contractors uses does not play
            //       nice with Selenium's drag and drop stuff, which is why there's all this extra javascript
            //       stuff in here still.
            var directionMultiplier = (direction == "up" ? (-1) : 1);
            var rowIndex = count;
            var otherRowIndex = rowIndex + (rowsToMove * directionMultiplier);

            if (otherRowIndex < 0)
            {
                Assert.Fail("The other row would not exist in the table because its index would be less than zero");
            }

            var rowToDrag = FindTableRowByIndex(rowIndex, tableId);
            var dragToRow = FindTableRowByIndex(otherRowIndex, tableId);

            var dragRowPosition = rowToDrag.GetLocation();
            var otherRowPosition = dragToRow.GetLocation();

            WebDriverHelper.Current.ExecuteScript(
                String.Format(
                    "var row = $('#{0} tr').eq({1}); window.mouseTrap = {{ 'row' : row, 'cell' : row.find('td').eq(1) }};",
                    tableId, rowIndex));
            WebDriverHelper.Current.ExecuteScript(String.Format("window.mouseTrap.cell.trigger({0});",
                CreateMouseArgs("mousedown", dragRowPosition.X, dragRowPosition.Y)));
            WebDriverHelper.Current.ExecuteScript(String.Format("window.mouseTrap.row.trigger({0});",
                CreateMouseArgs("mousemove", dragRowPosition.X, dragRowPosition.Y)));
            WebDriverHelper.Current.ExecuteScript(String.Format("window.mouseTrap.row.trigger({0});",
                CreateMouseArgs("mousemove", otherRowPosition.X, otherRowPosition.Y)));
            WebDriverHelper.Current.ExecuteScript("window.mouseTrap.row.trigger('mouseup');");
        }

        #endregion

        #region Waiting

        [Given("I wait (\\d+) seconds?"),
         When("I wait (\\d+) seconds?"),
         Then("I wait (\\d+) seconds?")]
        public static void IWait(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }

        [When("^I wait for element ([^\\s]+) to be enabled$")]
        public static void WhenIWaitForElementToBeEnabled(string element)
        {
            var el = WebDriverHelper.Current.FindElement(By.Id(element));
            WebDriverHelper.Current.WaitUntil(() => el.GetAttribute("disabled") == null);
        }

        [Given("^I wait for element ([^\\s]+) to exist$")]
        [When("^I wait for element ([^\\s]+) to exist$")]
        public static void WhenIWaitForElementToExist(string element)
        {
            var constraint = ByHelper.Any(By.Id(element), By.Name(element));
            WebDriverHelper.Current.WaitUntil(() => WebDriverHelper.Current.FindElements(constraint).Any());
        }

        [When("I reload the page")]
        public static void WhenIReloadThePage()
        {
            WebDriverHelper.Current.Refresh();
            WhenIWaitForThePageToReload();
        }

        [When("^I wait for the page to reload$")]
        public static void WhenIWaitForThePageToReload()
        {
            // NOTE: I don't really know if this step works properly. Also don't use this
            //       as a way of waiting for ajax updates, it does not work that way.

            // http://stackoverflow.com/a/17897771/152168 might be the answer to this
            WebDriverHelper.Current.WaitUntil(() =>
                ((string)WebDriverHelper.Current
                                        .ExecuteScript("return document.readyState")) == "complete");
        }

        [When("^I wait for element ([^\\s]+) to exist in frame ([^\\s]+)$")]
        public static void WhenIWaitForElementToExistInTheFrame(string element, string frame)
        {
            WebDriverHelper.Current.SwitchToFrame(frame);
            try
            {
                WebDriverHelper.Current.WaitUntilSingleElementExistsByIdOrName(element, 30);
            }
            finally
            {
                // Ensure that we switch back to the main window, otherwise all other tests will start failing.
                WebDriverHelper.Current.LeaveFrame();
            }
        }

        #endregion

        [Given("I launch the query interface"),
         When("I launch the query interface"),
         Then("I launch the query interface")]
        public static void LaunchTheQueryInterface()
        {
            Deleporter.Run(() =>
                DependencyResolver.Current.GetService<NHibernateQueryInterface.NHibernateQueryInterface>()
                                  .ShowWindow());
        }

        #endregion

        #region Helper Methods

        #region Finding

        internal static IEnumerable<IBetterWebElement> FindVisibleLinksByConstraint(By con, string conDescriptForErrors)
        {
            var links = WebDriverHelper.Current.FindElements(con);
            Assert.IsTrue(links.Any(), $"There's no link with '{conDescriptForErrors}' on the current page.");

            var visibleLinks = links.Where(l => l.IsDisplayed).ToList();
            Assert.IsTrue(visibleLinks.Any(),
                $"There's at least one link with '{conDescriptForErrors}' on the current page, but it's not visible.");

            return visibleLinks;
        }

        public static IBetterWebElement FindVisibleLinkByDisplayText(string linkText)
        {
            var visibleLinks = FindVisibleLinksByDisplayText(linkText);

            if (visibleLinks.Count() != 1)
            {
                Assert.Fail($"There's more than one visible link with '{linkText}' on the current page.");
            }

            return visibleLinks.Single();
        }

        public static IEnumerable<IBetterWebElement> FindVisibleLinksByDisplayText(string linkText)
        {
            // TextInTag will not find link buttons.
            var constraint = ByHelper.Any(ByHelper.TextInTag(linkText, "a"), ByHelper.XPath($"//a[.='{linkText}']"));
            return FindVisibleLinksByConstraint(constraint, linkText);
        }

        public static IBetterWebElement FindVisibleLinkInSuccessMessage(string linkText)
        {
            var constraint = By.XPath($"//*[@id='success-messages']/div/div/a[.='{linkText}']");
            return FindVisibleLinksByConstraint(constraint, linkText).Single();
        }

        public static IBetterWebElement FindVisibleLinkByUrl(Uri uri)
        {
            var url = uri.ToString();
            return FindVisibleLinksByConstraint(ByHelper.Href(url), url).Single();
        }

        public static IBetterWebElement FindTableRowByIndex(int rowIndex, string tableId)
        {
            var table = WebDriverHelper.Current.FindElement(By.Id(tableId));
            return FindTableRowByIndex(rowIndex, table);
        }

        public static IBetterWebElement FindTableRowByIndex(int rowIndex, IBetterWebElement table)
        {
            rowIndex = rowIndex - 1;
            var tableRows = table.FindElements(By.CssSelector("tbody > tr")).ToList();
            if (tableRows.Count <= rowIndex)
            {
                Assert.Fail($"The table '{table.Id}' only has {tableRows.Count} rows.");
            }

            return tableRows[rowIndex];
        }

        #endregion

        #region Manipulation

        public static void ClickTheNamedLinkInANumberedRowOfATable(string linkText, int rowIndex, string tableId)
        {
            var row = FindTableRowByIndex(rowIndex, tableId);
            var link = row.FindElement(ByHelper.TextInTag(linkText, "a"));
            Assert.IsNotNull(link,
                $"A link with the text '{linkText}' in row {rowIndex} of the table with the id '{tableId}' could not be found.");
            link.Click();
        }

        public static void ClickLinkByTextInCellByHeaderAndRow(string linkText, string headerText, int rowIndex,
            string tableId)
        {
            var table = WebDriverHelper.Current.FindElement(By.Id(tableId));
            var header = table.FindElements(By.CssSelector("thead > tr > th")).ToList();
            var headerIndex = header.IndexOf(header.Single(x => x.Text == headerText));
            var row = FindTableRowByIndex(rowIndex, table);
            var cell = row.FindElements(By.TagName("td")).ToList()[headerIndex];
            var link = cell.FindElement(ByHelper.TextInTag(linkText, "a"));
            Assert.IsNotNull(link,
                $"A link with the text '{linkText}' in row {rowIndex} of the table with the id '{tableId}' could not be found.");
            link.Click();
        }

        public static void ClickTheNamedButtonInANumberedRowOfATable(string linkText, int rowIndex, string tableId)
        {
            var row = FindTableRowByIndex(rowIndex, tableId);
            var button = row.FindElement(ByHelper.TextInTag(linkText, "button"));
            Assert.IsNotNull(button,
                $"A button with the text '{linkText}' in row {rowIndex} of the table with the id '{tableId}' could not be found.");
            button.Click();
        }

        public static void ClickTheNamedCheckboxInANumberedRowOfATable(string linkText, int rowIndex, string tableId)
        {
            var row = FindTableRowByIndex(rowIndex, tableId);
            var checkbox = row.FindElement(By.CssSelector($"input[name=\"{linkText}\"]"));
            Assert.IsNotNull(checkbox,
                $"A checkbox with the text '{linkText}' in row {rowIndex} of the table with the id '{tableId}' could not be found.");
            checkbox.Check(!checkbox.IsChecked);
        }

        public static void PressButton(string buttonText, IFindElements container = null)
        {
            var btn = ElementHelper.FindButtonByIdNameOrText(buttonText, container);

            // Need to get the class before clicking, as clicking usually causes page navigation
            // and then throws a stale element error if you try to access the btn again.
            var buttonClass = btn.GetCssClasses();

            btn.Click();

            if (buttonClass.Contains("collapse-trigger"))
            {
                // Animations can cause flukey race conditions with element visibility, so we want to wait
                // for any animations to complete before continuing.
                IWaitForAnimationsToComplete();
            }
        }

        /// <summary>
        /// DO NOT USE THIS IF YOU DO NOT HAVE TO THIS IS A STUPID HACK FOR AUTHORIZE.NET POPUPS ONLY
        /// </summary>
        /// <param name="buttonText"></param>
        /// <param name="container"></param>
        public static void ForcePressButton(string buttonText, IFindElements container = null)
        {
            var btn = ElementHelper.FindButtonByIdNameOrText(buttonText);

            // Need to get the class before clicking, as clicking usually causes page navigation
            // and then throws a stale element error if you try to access the btn again.
            var buttonClass = btn.GetCssClasses();

            btn.ForceClick();

            if (buttonClass.Contains("collapse-trigger"))
            {
                // Animations can cause flukey race conditions with element visibility, so we want to wait
                // for any animations to complete before continuing.
                IWaitForAnimationsToComplete();
            }
        }

        public static void PressOneOfTheButtons(string buttonText, IFindElements container = null)
        {
            var butts = ElementHelper.FindButtonsByIdNameOrText(buttonText);
            var btn = butts.First();

            // Need to get the class before clicking, as clicking usually causes page navigation
            // and then throws a stale element error if you try to access the btn again.
            var buttonClass = btn.GetCssClasses();

            btn.Click();

            if (buttonClass.Contains("collapse-trigger"))
            {
                // Animations can cause flukey race conditions with element visibility, so we want to wait
                // for any animations to complete before continuing.
                IWaitForAnimationsToComplete();
            }
        }

        public static void EnterValueInField(string field, string data, string form = "")
        {
            if (!string.IsNullOrWhiteSpace(form))
            {
                field = $"#{form} input[id={field}]";
            }

            var textField = ElementHelper.FindElementByIdNameOrSelector(field);
            textField.SetValue(data);
        }

        public static void TypeValueInField(string field, string data, string form = "")
        {
            if (!string.IsNullOrWhiteSpace(form))
            {
                field = $"#{form} input[id={field}]";
            }

            var textField = ElementHelper.FindElementByIdNameOrSelector(field);
            textField.TypeValue(data);
        }

        public static void SelectTextInField(string field, string data, string form = "")
        {
            if (!string.IsNullOrWhiteSpace(form))
            {
                field = $"#{form} select[id={field}]";
            }

            var select = ElementHelper.FindElementByIdNameOrSelector(field);

            // the browser merges multiple spaces down to one, so we need to also
            data = new Regex("[ ]{2}").Replace(data, " ");
            // I switched this to a loop because select.Select(data) takes
            // forever to actually fail when an option is missing from the list.
            select.SetSelectedOptionByText(data);
        }

        public static void CheckOrUncheckCheckboxInCheckboxListByTextOrValue(string field, string data,
            bool shouldCheck, string form = "")
        {
            if (!string.IsNullOrWhiteSpace(form))
            {
                field = $"#{form} #{field}]";
            }

            var checkboxList = ElementHelper.FindElementByIdNameOrSelector(field);

            // the browser merges multiple spaces down to one, so we need to also
            data = new Regex("[ ]{2}").Replace(data, " ");

            var checkbox = checkboxList.GetCheckBoxListItems().Single(o =>
                o.GetProperty<string>("text") == data ||
                o.GetProperty<string>("value") == data);
            checkbox.Check(shouldCheck);
        }

        public static void IAnswerTheAlertDialog(Action confirmTrigger)
        {
            try
            {
                confirmTrigger();
            }
            catch (UnhandledAlertException) { }

            WebDriverHelper.Current.WaitUntilNoExceptionThrown(() => WebDriverHelper.Current.ConfirmDialog());
        }

        private static void EnsureOkCancel(string okCancel)
        {
            if (okCancel != "cancel" && okCancel != "ok")
            {
                throw new InvalidOperationException("It needs to be 'ok' or 'cancel'.");
            }
        }

        public static void IAnswerTheConfirmDialog(Action confirmTrigger, string okCancel)
        {
            EnsureOkCancel(okCancel);

            confirmTrigger();
            WebDriverHelper.Current.WaitUntilNoExceptionThrown(() => {
                if (okCancel == "cancel")
                {
                    WebDriverHelper.Current.DismissDialog();
                }
                else
                {
                    WebDriverHelper.Current.ConfirmDialog();
                }
            });
        }

        #endregion

        #region Utilities

        public static string ParseSpecialString(string data)
        {
            var match = FROM_NOW_REGEX.Match(data);
            if (match.Success)
            {
                var value = DateTime.Now.AddDays(int.Parse(match.Groups[1].Value)).Date.ToString("d");
                return FROM_NOW_REGEX.Replace(data, value);
            }

            switch (data)
            {
                case "yesterday's date":
                    return DateTime.Today.AddDays(-1).ToString();
                case "today's date":
                    return DateTime.Today.ToString("d");
                case "tomorrow":
                    return DateTime.Today.AddDays(1).ToString("d");
                case "now":
                    return DateTime.Now.ToString();
                case "this year":
                    return DateTime.Today.Year.ToString();
                default:
                    return data;
            }
        }

        // This whole mess is how events need to be fired in IE9 that works
        // with the jquery.tableDnD.js script. You break it you fix it. -Ross
        public static string CreateMouseArgs(string type, int pageX, int pageY)
        {
            return
                $"{{ 'type' : '{type}', 'pageX': {pageX}, 'clientX' : {pageX}, 'pageY' : {pageY}, 'clientY' : {pageY}, 'bubbles' : true }}";
        }

        public static string StripQuotationMarks(string str)
        {
            if (str == null)
            {
                return null;
            }

            return str.Replace("\"", string.Empty);
        }

        #endregion

        #endregion
    }
}
