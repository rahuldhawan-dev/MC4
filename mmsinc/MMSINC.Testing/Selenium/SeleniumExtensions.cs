using JetBrains.Annotations;
using NUnit.Framework;
using Selenium;
using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace MMSINC.Testing.Selenium
{
    public static class SeleniumExtensions
    {
        #region Public Extension Methods

        #region Helper Methods

        /// <summary>
        /// Selects the option in the select with the specified id with the specified label.
        /// </summary>
        /// <param name="selenium"></param>
        /// <param name="id">ID of the select to work with.</param>
        /// <param name="label">Option label to select.</param>
        public static void SelectLabel(this ISelenium selenium, string id, string label)
        {
            try
            {
                selenium.Select(id, "label=" + label);
            }
            catch (Exception ex)
            {
                throw new Exception("Option was not found. These are the available options: \r\n" +
                                    string.Join("\r\n", selenium.GetSelectOptions(id)));
            }

            // Changing a dropdown's selected value through javascript does not fire the
            // blur or change events. It's just how the html spec is. So we need to fire
            // the events ourselves in order for any dependent scripts to work correctly.
            // Also if there's any WebForms events depending on blur, it needs to be fired.
            selenium.FireEvent(id, "blur");
            // Firing an event is a no-wait scenario, we need to sleep for a second to
            // let any scripts do their thing, then fire the change event. This can probably
            // be adjusted but 1000ms seems to be the sweet spot. 
            Thread.Sleep(1000);
            // And for whatever reason, firing this event doesn't seem to need a sleep. 
            selenium.FireEvent(id, "change");
            //Thread.Sleep(500);
        }

        /// <summary>
        /// Selects the option in the select with the specified id with the specified value.
        /// </summary>
        /// <param name="selenium"></param>
        /// <param name="id">ID of the select to work with.</param>
        /// <param name="value">Option value to select.</param>
        public static void SelectValue(this ISelenium selenium, string id, string value)
        {
            selenium.Select(id, "value=" + value);
        }

        public static string GetLastLinkIdInTable(this ISelenium selenium, string table, string link)
        {
            var pattern = string.Format("_{0}_{1}_\\d+$", table, link);
            var ids = selenium.GetAllLinks();
            string ret = string.Empty;
            foreach (string id in ids)
            {
                if (Regex.IsMatch(id, pattern))
                    ret = id;
            }

            return ret;
        }

        [StringFormatMethod("scriptFormat")]
        public static void RunScript(this ISelenium selenium, string scriptFormat, params object[] args)
        {
            selenium.RunScript(String.Format(scriptFormat, args));
        }

        #endregion

        #region Wait Methods

        public static void WaitForEditable(this ISelenium selenium, string id)
        {
            selenium.WaitForEditable(id, Globals.DEFAULT_TIMEOUT_SECONDS);
        }

        public static void WaitForEditable(this ISelenium selenium, string id, int timeoutSeconds)
        {
            WaitForTrue(() => selenium.IsEditable(id), timeoutSeconds, onFail: () => {
                var message = String.Format("Element with the id '{0}' never became editable as expected.", id);
                Assert.Fail(
                    selenium.IsElementPresent(id) ? message : message + "  Element does not seem to be present.");
            });
        }

        public static void WaitForNotEditable(this ISelenium selenium, string id)
        {
            selenium.WaitForNotEditable(id, Globals.DEFAULT_TIMEOUT_SECONDS);
        }

        public static void WaitForNotEditable(this ISelenium selenium, string id, int timeoutSeconds)
        {
            WaitForFalse(() => selenium.IsEditable(id), timeoutSeconds);
        }

        public static void WaitForElementPresent(this ISelenium selenium, string id)
        {
            selenium.WaitForElementPresent(id, Globals.DEFAULT_TIMEOUT_SECONDS);
        }

        public static void WaitForElementPresent(this ISelenium selenium, string id, int timeoutSeconds)
        {
            WaitForTrue(() => selenium.IsElementPresent(id), timeoutSeconds,
                String.Format("Timeout exceeded waiting for element '{0}' to be present.", id));
        }

        public static void WaitForElementNotPresent(this ISelenium selenium, string id)
        {
            selenium.WaitForElementNotPresent(id, Globals.DEFAULT_TIMEOUT_SECONDS);
        }

        public static void WaitForElementNotPresent(this ISelenium selenium, string id, int timeoutSeconds)
        {
            WaitForTrue(() => !selenium.IsElementPresent(id), timeoutSeconds,
                String.Format("Timeout exceeded waiting for element '{0}' to be present.", id));
        }

        public static void WaitForNotValue(this ISelenium selenium, string id, string value)
        {
            selenium.WaitForNotValue(id, value, Globals.DEFAULT_TIMEOUT_SECONDS);
        }

        public static void WaitForNotValue(this ISelenium selenium, string id, string value, int timeoutSeconds)
        {
            WaitForFalse(() => selenium.GetValue(id) == value, timeoutSeconds);
        }

        public static void WaitForTextPresent(this ISelenium selenium, string text)
        {
            selenium.WaitForTextPresent(text, Globals.DEFAULT_TIMEOUT_SECONDS);
        }

        public static void WaitForTextPresent(this ISelenium selenium, string text, int timeoutSeconds)
        {
            WaitForTrue(() => selenium.IsTextPresent(text), timeoutSeconds);
        }

        public static void WaitForText(this ISelenium selenium, string id, string value)
        {
            selenium.WaitForText(id, value, Globals.DEFAULT_TIMEOUT_SECONDS);
        }

        public static void WaitForText(this ISelenium selenium, string id, string value, int timeoutSeconds)
        {
            WaitForTrue(() => selenium.GetText(id) == value, timeoutSeconds);
        }

        public static void WaitForText(this ISelenium selenium, string id, Regex expected)
        {
            selenium.WaitForText(id, expected, Globals.DEFAULT_TIMEOUT_SECONDS);
        }

        public static void WaitForText(this ISelenium selenium, string id, Regex expected, int timeoutSeconds)
        {
            WaitForTrue(() => expected.IsMatch(selenium.GetText(id)), timeoutSeconds);
        }

        public static void WaitForNotText(this ISelenium selenium, string id, string value)
        {
            selenium.WaitForNotText(id, value, Globals.DEFAULT_TIMEOUT_SECONDS);
        }

        public static void WaitForNotText(this ISelenium selenium, string id, string value, int timeoutSeconds)
        {
            WaitForFalse(() => selenium.GetText(id) == value, timeoutSeconds);
        }

        public static void WaitForNotSelectedLabel(this ISelenium selenium, string id, string label)
        {
            selenium.WaitForNotSelectedLabel(id, label,
                Globals.DEFAULT_TIMEOUT_SECONDS);
        }

        public static void WaitForNotSelectedLabel(this ISelenium selenium, string id, string label, int timeoutSeconds)
        {
            WaitForFalse(
                () => selenium.GetSelectedLabel(id).CompareTo(label) == 0,
                timeoutSeconds);
        }

        public static void WaitThenSelectLabel(this ISelenium selenium, string id, string label)
        {
            //            selenium.WaitForEditable(id);
            WaitForTrue(() => {
                    var options = selenium.GetSelectOptions(id);
                    foreach (var option in options)
                    {
                        if (option == label)
                        {
                            return true;
                        }
                    }

                    return false;
                }, Globals.DEFAULT_TIMEOUT_SECONDS,
                String.Format("Timed out attempting to select label \"{0}\" in element \"{1}\".", label, id));
            selenium.SelectLabel(id, label);
        }

        public static void ClickAndWaitForPageToLoad(this ISelenium selenium, string id)
        {
            selenium.ClickAndWaitForPageToLoad(id, Globals.DEFAULT_TIMEOUT);
        }

        public static void ClickAndWaitForPageToLoad(this ISelenium selenium, string id, string timeout)
        {
            selenium.Click(id);
            selenium.WaitForPageToLoad(timeout);
        }

        public static void SelectAndWaitForPageToLoad(this ISelenium selenium, string id, string value)
        {
            selenium.SelectAndWaitForPageToLoad(id, value, Globals.DEFAULT_TIMEOUT);
        }

        public static void SelectAndWaitForPageToLoad(this ISelenium selenium, string id, string value, string timeout)
        {
            selenium.SelectValue(id, value);
            selenium.WaitForPageToLoad(timeout);
        }

        public static void ClickAndWait(this ISelenium selenium, string id)
        {
            selenium.ClickAndWait(id, Globals.DEFAULT_TIMEOUT_SECONDS);
        }

        public static void ClickAndWait(this ISelenium selenium, string id, int timeoutSeconds)
        {
            selenium.Click(id);
            Wait(timeoutSeconds);
        }

        #endregion

        #region Assertion Methods

        public static void AssertAlert(this ISelenium selenium, string expected)
        {
            var actual = selenium.GetAlert();
            Assert.IsNotNull(actual,
                String.Format("Alert with message '{0}' was not found.",
                    expected));
            Assert.IsTrue(actual.CompareTo(expected) == 0,
                String.Format(
                    "Alert was expected to have the message '{0}', but had the message '{1}' instead.",
                    expected, actual));
        }

        public static void AssertText(this ISelenium selenium, string id, string expected)
        {
            var actual = selenium.GetText(id);
            Assert.IsTrue(actual.CompareTo(expected) == 0,
                String.Format(
                    "Element identified by '{0}' was expected to have the text '{1}', but had the text '{2}' instead.",
                    id, expected, actual));
        }

        public static void AssertText(this ISelenium selenium, string id, string expected, string message)
        {
            Assert.IsTrue(selenium.GetValue(id).CompareTo(expected) == 0, message);
        }

        public static void AssertText(this ISelenium selenium, string id, Regex expected)
        {
            var actual = selenium.GetText(id);
            Assert.IsTrue(expected.IsMatch(actual),
                String.Format(
                    "Element identified by '{0}' was expected to have text to match the Regex '{1}', but had the text '{2}' instead.",
                    id, expected, actual));
        }

        public static void AssertText(this ISelenium selenium, string id, Regex expected, string message)
        {
            Assert.IsTrue(expected.IsMatch(selenium.GetText(id)), message);
        }

        public static void AssertValue(this ISelenium selenium, string id, string expected)
        {
            var actual = selenium.GetValue(id);
            Assert.IsTrue(actual.CompareTo(expected) == 0,
                String.Format(
                    "Element identified by '{0}' was expected to have the value '{1}', but had the value '{2}' instead.",
                    id, expected, actual));
        }

        public static void AssertValue(this ISelenium selenium, string id, string expected, string message)
        {
            Assert.IsTrue(selenium.GetValue(id).CompareTo(expected) == 0, message);
        }

        public static void AssertTextPresent(this ISelenium selenium, string expected)
        {
            selenium.AssertTextPresent(expected,
                String.Format("Expected value '{0}' was not found. {1}", expected, selenium.GetHtmlSource()));
        }

        public static void AssertTextPresent(this ISelenium selenium, string expected, string message)
        {
            Assert.IsTrue(selenium.IsTextPresent(expected), message);
        }

        public static void AssertTextNotPresent(this ISelenium selenium, string notExpected)
        {
            selenium.AssertTextNotPresent(notExpected,
                String.Format("Unexpected value '{0}' was found.", notExpected));
        }

        public static void AssertTextNotPresent(this ISelenium selenium, string notExpected, string message)
        {
            Assert.IsFalse(selenium.IsTextPresent(notExpected), message);
        }

        public static void AssertElementPresent(this ISelenium selenium, string id)
        {
            selenium.AssertElementPresent(id,
                String.Format(
                    "Element identified by '{0}' was not found as expected.", id));
        }

        public static void AssertElementPresent(this ISelenium selenium, string id, string message)
        {
            Assert.IsTrue(selenium.IsElementPresent(id), message);
        }

        public static void AssertElementIsNotPresent(this ISelenium selenium, string id)
        {
            selenium.AssertElementIsNotPresent(id,
                String.Format(
                    "Element identified by '{0}' WAS found and was NOT expected.", id));
        }

        public static void AssertElementIsNotPresent(this ISelenium selenium, string id, string message)
        {
            Assert.IsFalse(selenium.IsElementPresent(id), message);
        }

        public static void AssertConfirmation(this ISelenium selenium, string confirmationMessage)
        {
            var actual = selenium.GetConfirmation();
            Assert.IsTrue(actual.CompareTo(confirmationMessage) == 0,
                String.Format(
                    "Confirmation with message '{0}' was expected, but message '{1}' was received instead.",
                    confirmationMessage, actual));
        }

        public static void AssertConfirmation(this ISelenium selenium, string confirmationMessage,
            string failureMessage)
        {
            Assert.IsTrue(
                selenium.GetConfirmation().CompareTo(confirmationMessage) == 0,
                failureMessage);
            // Do not use KeyPressNative("10") here. It will send keys to the browser
            // as well as the currently active window on the machine that's running the
            // tests. Text files will get random line breaks, things like that. Don't do it.
            // If you need to hit ok/cancel call selenium.ChoseOkOnNextConfirmation() prior
            // to triggering the action that will bring the confirmation dialog up.
        }

        public static void AssertSelectedLabel(this ISelenium selenium, string id, string label)
        {
            var actual = selenium.GetSelectedLabel(id);
            Assert.IsTrue(selenium.GetSelectedLabel(id).CompareTo(label) == 0,
                String.Format(
                    "Element identified by '{0}' was expected to have the label '{1}' selected, but instead had '{2}' selected.",
                    id, label, actual));
        }

        public static void AssertSelectedLabel(this ISelenium selenium, string id, string label, string message)
        {
            Assert.IsTrue(selenium.GetSelectedLabel(id).CompareTo(label) == 0,
                message);
        }

        public static void AssertAttribute(this ISelenium selenium, string id, string attributeID, string attribute)
        {
            var actual =
                selenium.GetAttribute(String.Format("{0}@{1}", id, attributeID));
            Assert.IsTrue(actual.CompareTo(attribute) == 0,
                String.Format(
                    "The '{0}' attribute of the element identified by '{1}' was expected to have the value '{2}', but had '{3}' instead.",
                    attributeID, id, attribute, actual));
        }

        public static void AssertAttribute(this ISelenium selenium, string id, string attributeID, string attribute,
            string message)
        {
            Assert.IsTrue(
                selenium.GetAttribute(String.Format("{0}@{1}", id, attributeID))
                        .CompareTo(attribute) == 0, message);
        }

        public static void AssertEditable(this ISelenium selenium, string id)
        {
            selenium.AssertEditable(id,
                String.Format(
                    "Element found by locator '{0}' was not found to be editable.", id));
        }

        public static void AssertEditable(this ISelenium selenium, string id, string message)
        {
            Assert.IsTrue(selenium.IsEditable(id));
        }

        public static void AssertNotEditable(this ISelenium selenium, string id)
        {
            selenium.AssertNotEditable(id,
                String.Format(
                    "Element found by locator '{0}' was found to be editable.", id));
        }

        public static void AssertNotEditable(this ISelenium selenium, string id, string message)
        {
            Assert.IsFalse(selenium.IsEditable(id));
        }

        #endregion

        #endregion

        #region Public Methods

        public static void Wait(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }

        #endregion

        #region Private Methods

        private static void WaitForTrue(Func<bool> lambda, int timeoutSeconds, string message = null,
            Action onFail = null)
        {
            onFail = onFail ?? (() => Assert.Fail(message ?? "timeout"));
            for (var second = 0; second < timeoutSeconds; ++second)
            {
                try
                {
                    if (lambda()) return;
                }
                catch { }

                Wait(5);
            }

            onFail();
        }

        private static void WaitForFalse(Func<bool> lambda, int timeoutSeconds, string message = null)
        {
            WaitForTrue(() => !lambda(), timeoutSeconds);
        }

        #endregion
    }
}
