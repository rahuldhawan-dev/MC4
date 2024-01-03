using System;
using System.IO;
using NUnit.Framework;
using Selenium;

namespace MapCall.Common.Testing.Selenium
{
    public class SeleniumWrapper : IExtendedSelenium
    {
        #region Private Members

        protected readonly ISelenium _innerSelenium;

        #endregion

        #region Constructors

        public SeleniumWrapper(string serverHost, int serverPort,
            string browserString, string browserURL)
        {
            _innerSelenium = new DefaultSelenium(serverHost, serverPort,
                browserString, browserURL);
        }

        public SeleniumWrapper(ICommandProcessor processor)
        {
            _innerSelenium = new DefaultSelenium(processor);
        }

        #endregion

        #region Private Methods

        private void Log(string str)
        {
            Console.WriteLine("    " + str);
        }

        #endregion

        #region Altered Methods

        public void Open(string url)
        {
            _innerSelenium.Open(url);
            DetectYSOD();
        }

        #endregion

        #region Added Methods

        public void OpenAndAllowErrors(string url)
        {
            _innerSelenium.Open(url);
        }

        public void DetectYSOD()
        {
            if (IsElementPresent("//html/body/font/b[text()=' Description: ']"))
            {
                Assert.Fail("YSOD Detected: {0}", GetBodyText());
            }
        }

        #endregion

        #region Wrapped Methods

        public void SetExtensionJs(string extensionJs)
        {
            _innerSelenium.SetExtensionJs(extensionJs);
        }

        public void Start()
        {
            _innerSelenium.Start();
        }

        public void Stop()
        {
            _innerSelenium.Stop();
        }

        public void Click(string locator)
        {
            Log($"Clicking element '{locator}'..");
            _innerSelenium.Click(locator);
        }

        public void DoubleClick(string locator)
        {
            _innerSelenium.DoubleClick(locator);
        }

        public void ContextMenu(string locator)
        {
            _innerSelenium.ContextMenu(locator);
        }

        public void ClickAt(string locator, string coordString)
        {
            _innerSelenium.ClickAt(locator, coordString);
        }

        public void DoubleClickAt(string locator, string coordString)
        {
            _innerSelenium.DoubleClickAt(locator, coordString);
        }

        public void ContextMenuAt(string locator, string coordString)
        {
            _innerSelenium.ContextMenuAt(locator, coordString);
        }

        public void FireEvent(string locator, string eventName)
        {
            _innerSelenium.FireEvent(locator, eventName);
        }

        public void Focus(string locator)
        {
            _innerSelenium.Focus(locator);
        }

        public void KeyPress(string locator, string keySequence)
        {
            _innerSelenium.KeyPress(locator, keySequence);
        }

        public void ShiftKeyDown()
        {
            _innerSelenium.ShiftKeyDown();
        }

        public void ShiftKeyUp()
        {
            _innerSelenium.ShiftKeyUp();
        }

        public void MetaKeyDown()
        {
            _innerSelenium.MetaKeyDown();
        }

        public void MetaKeyUp()
        {
            _innerSelenium.MetaKeyUp();
        }

        public void AltKeyDown()
        {
            _innerSelenium.AltKeyDown();
        }

        public void AltKeyUp()
        {
            _innerSelenium.AltKeyUp();
        }

        public void ControlKeyDown()
        {
            _innerSelenium.ControlKeyDown();
        }

        public void ControlKeyUp()
        {
            _innerSelenium.ControlKeyUp();
        }

        public void KeyDown(string locator, string keySequence)
        {
            _innerSelenium.KeyDown(locator, keySequence);
        }

        public void KeyUp(string locator, string keySequence)
        {
            _innerSelenium.KeyUp(locator, keySequence);
        }

        public void MouseOver(string locator)
        {
            _innerSelenium.MouseOver(locator);
        }

        public void MouseOut(string locator)
        {
            _innerSelenium.MouseOut(locator);
        }

        public void MouseDown(string locator)
        {
            _innerSelenium.MouseDown(locator);
        }

        public void MouseDownRight(string locator)
        {
            _innerSelenium.MouseDownRight(locator);
        }

        public void MouseDownAt(string locator, string coordString)
        {
            _innerSelenium.MouseDownAt(locator, coordString);
        }

        public void MouseDownRightAt(string locator, string coordString)
        {
            _innerSelenium.MouseDownRightAt(locator, coordString);
        }

        public void MouseUp(string locator)
        {
            _innerSelenium.MouseUp(locator);
        }

        public void MouseUpRight(string locator)
        {
            _innerSelenium.MouseUpRight(locator);
        }

        public void MouseUpAt(string locator, string coordString)
        {
            _innerSelenium.MouseUpAt(locator, coordString);
        }

        public void MouseUpRightAt(string locator, string coordString)
        {
            _innerSelenium.MouseUpRightAt(locator, coordString);
        }

        public void MouseMove(string locator)
        {
            _innerSelenium.MouseMove(locator);
        }

        public void MouseMoveAt(string locator, string coordString)
        {
            _innerSelenium.MouseMoveAt(locator, coordString);
        }

        public void Type(string locator, string value)
        {
            Log($"Typing value '{value}' into field '{locator}'...");
            _innerSelenium.Type(locator, value);
        }

        public void TypeKeys(string locator, string value)
        {
            _innerSelenium.TypeKeys(locator, value);
        }

        public void SetSpeed(string value)
        {
            _innerSelenium.SetSpeed(value);
        }

        public string GetSpeed()
        {
            return _innerSelenium.GetSpeed();
        }

        public void Check(string locator)
        {
            _innerSelenium.Check(locator);
        }

        public void Uncheck(string locator)
        {
            _innerSelenium.Uncheck(locator);
        }

        public void Select(string selectLocator, string optionLocator)
        {
            Log($"Selecting '{optionLocator}' in element '{selectLocator}'...");
            _innerSelenium.Select(selectLocator, optionLocator);
        }

        public void AddSelection(string locator, string optionLocator)
        {
            _innerSelenium.AddSelection(locator, optionLocator);
        }

        public void RemoveSelection(string locator, string optionLocator)
        {
            _innerSelenium.RemoveSelection(locator, optionLocator);
        }

        public void RemoveAllSelections(string locator)
        {
            _innerSelenium.RemoveAllSelections(locator);
        }

        public void Submit(string formLocator)
        {
            _innerSelenium.Submit(formLocator);
        }

        public void OpenWindow(string url, string windowID)
        {
            _innerSelenium.OpenWindow(url, windowID);
        }

        public void SelectWindow(string windowID)
        {
            _innerSelenium.SelectWindow(windowID);
        }

        public void SelectPopUp(string windowID)
        {
            _innerSelenium.SelectPopUp(windowID);
        }

        public void DeselectPopUp()
        {
            _innerSelenium.DeselectPopUp();
        }

        public void SelectFrame(string locator)
        {
            _innerSelenium.SelectFrame(locator);
        }

        public bool GetWhetherThisFrameMatchFrameExpression(
            string currentFrameString, string target)
        {
            return
                _innerSelenium.GetWhetherThisFrameMatchFrameExpression(
                    currentFrameString, target);
        }

        public bool GetWhetherThisWindowMatchWindowExpression(
            string currentWindowString, string target)
        {
            return
                _innerSelenium.GetWhetherThisWindowMatchWindowExpression(
                    currentWindowString, target);
        }

        public void WaitForPopUp(string windowID, string timeout)
        {
            _innerSelenium.WaitForPopUp(windowID, timeout);
        }

        public void ChooseCancelOnNextConfirmation()
        {
            _innerSelenium.ChooseCancelOnNextConfirmation();
        }

        public void ChooseOkOnNextConfirmation()
        {
            _innerSelenium.ChooseOkOnNextConfirmation();
        }

        public void AnswerOnNextPrompt(string answer)
        {
            _innerSelenium.AnswerOnNextPrompt(answer);
        }

        public void GoBack()
        {
            _innerSelenium.GoBack();
        }

        public void Refresh()
        {
            _innerSelenium.Refresh();
        }

        public void Close()
        {
            _innerSelenium.Close();
        }

        public bool IsAlertPresent()
        {
            return _innerSelenium.IsAlertPresent();
        }

        public bool IsPromptPresent()
        {
            return _innerSelenium.IsPromptPresent();
        }

        public bool IsConfirmationPresent()
        {
            return _innerSelenium.IsConfirmationPresent();
        }

        public string GetAlert()
        {
            return _innerSelenium.GetAlert();
        }

        public string GetConfirmation()
        {
            return _innerSelenium.GetConfirmation();
        }

        public string GetPrompt()
        {
            return _innerSelenium.GetPrompt();
        }

        public string GetLocation()
        {
            return _innerSelenium.GetLocation();
        }

        public string GetTitle()
        {
            return _innerSelenium.GetTitle();
        }

        public string GetBodyText()
        {
            return _innerSelenium.GetBodyText();
        }

        public string GetValue(string locator)
        {
            return _innerSelenium.GetValue(locator);
        }

        public string GetText(string locator)
        {
            try
            {
                return _innerSelenium.GetText(locator);
            }
            catch (SeleniumException e)
            {
                var now = DateTime.Now.ToString("yyyyMMddHHmmss");
                var screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), $"Error-{now}.png");
                Log(
                    $"{now} Error getting text for locator '{locator}', generating screenshot at '{screenshotPath}':{Environment.NewLine}{e}");
                _innerSelenium.CaptureEntirePageScreenshot(screenshotPath, null);
                throw;
            }
        }

        public void Highlight(string locator)
        {
            _innerSelenium.Highlight(locator);
        }

        public string GetEval(string script)
        {
            return _innerSelenium.GetEval(script);
        }

        public bool IsChecked(string locator)
        {
            return _innerSelenium.IsChecked(locator);
        }

        public string GetTable(string tableCellAddress)
        {
            return _innerSelenium.GetTable(tableCellAddress);
        }

        public string[] GetSelectedLabels(string selectLocator)
        {
            return _innerSelenium.GetSelectedLabels(selectLocator);
        }

        public string GetSelectedLabel(string selectLocator)
        {
            return _innerSelenium.GetSelectedLabel(selectLocator);
        }

        public string[] GetSelectedValues(string selectLocator)
        {
            return _innerSelenium.GetSelectedValues(selectLocator);
        }

        public string GetSelectedValue(string selectLocator)
        {
            return _innerSelenium.GetSelectedValue(selectLocator);
        }

        public string[] GetSelectedIndexes(string selectLocator)
        {
            return _innerSelenium.GetSelectedIndexes(selectLocator);
        }

        public string GetSelectedIndex(string selectLocator)
        {
            return _innerSelenium.GetSelectedIndex(selectLocator);
        }

        public string[] GetSelectedIds(string selectLocator)
        {
            return _innerSelenium.GetSelectedIds(selectLocator);
        }

        public string GetSelectedId(string selectLocator)
        {
            return _innerSelenium.GetSelectedId(selectLocator);
        }

        public bool IsSomethingSelected(string selectLocator)
        {
            return _innerSelenium.IsSomethingSelected(selectLocator);
        }

        public string[] GetSelectOptions(string selectLocator)
        {
            return _innerSelenium.GetSelectOptions(selectLocator);
        }

        public string GetAttribute(string attributeLocator)
        {
            return _innerSelenium.GetAttribute(attributeLocator);
        }

        public bool IsTextPresent(string pattern)
        {
            return _innerSelenium.IsTextPresent(pattern);
        }

        public bool IsElementPresent(string locator)
        {
            return _innerSelenium.IsElementPresent(locator);
        }

        public bool IsVisible(string locator)
        {
            return _innerSelenium.IsVisible(locator);
        }

        public bool IsEditable(string locator)
        {
            return _innerSelenium.IsEditable(locator);
        }

        public string[] GetAllButtons()
        {
            return _innerSelenium.GetAllButtons();
        }

        public string[] GetAllLinks()
        {
            return _innerSelenium.GetAllLinks();
        }

        public string[] GetAllFields()
        {
            return _innerSelenium.GetAllFields();
        }

        public string[] GetAttributeFromAllWindows(string attributeName)
        {
            return _innerSelenium.GetAttributeFromAllWindows(attributeName);
        }

        public void Dragdrop(string locator, string movementsString)
        {
            _innerSelenium.Dragdrop(locator, movementsString);
        }

        public void SetMouseSpeed(string pixels)
        {
            _innerSelenium.SetMouseSpeed(pixels);
        }

        public decimal GetMouseSpeed()
        {
            return _innerSelenium.GetMouseSpeed();
        }

        public void DragAndDrop(string locator, string movementsString)
        {
            _innerSelenium.DragAndDrop(locator, movementsString);
        }

        public void DragAndDropToObject(string locatorOfObjectToBeDragged,
            string locatorOfDragDestinationObject)
        {
            _innerSelenium.DragAndDropToObject(locatorOfObjectToBeDragged,
                locatorOfDragDestinationObject);
        }

        public void WindowFocus()
        {
            _innerSelenium.WindowFocus();
        }

        public void WindowMaximize()
        {
            _innerSelenium.WindowMaximize();
        }

        public string[] GetAllWindowIds()
        {
            return _innerSelenium.GetAllWindowIds();
        }

        public string[] GetAllWindowNames()
        {
            return _innerSelenium.GetAllWindowNames();
        }

        public string[] GetAllWindowTitles()
        {
            return _innerSelenium.GetAllWindowTitles();
        }

        public string GetHtmlSource()
        {
            return _innerSelenium.GetHtmlSource();
        }

        public void SetCursorPosition(string locator, string position)
        {
            _innerSelenium.SetCursorPosition(locator, position);
        }

        public decimal GetElementIndex(string locator)
        {
            return _innerSelenium.GetElementIndex(locator);
        }

        public bool IsOrdered(string locator1, string locator2)
        {
            return _innerSelenium.IsOrdered(locator1, locator2);
        }

        public decimal GetElementPositionLeft(string locator)
        {
            return _innerSelenium.GetElementPositionLeft(locator);
        }

        public decimal GetElementPositionTop(string locator)
        {
            return _innerSelenium.GetElementPositionTop(locator);
        }

        public decimal GetElementWidth(string locator)
        {
            return _innerSelenium.GetElementWidth(locator);
        }

        public decimal GetElementHeight(string locator)
        {
            return _innerSelenium.GetElementHeight(locator);
        }

        public decimal GetCursorPosition(string locator)
        {
            return _innerSelenium.GetCursorPosition(locator);
        }

        public string GetExpression(string expression)
        {
            return _innerSelenium.GetExpression(expression);
        }

        public decimal GetXpathCount(string xpath)
        {
            return _innerSelenium.GetXpathCount(xpath);
        }

        public decimal GetCSSCount(string cssLocator)
        {
            return _innerSelenium.GetCSSCount(cssLocator);
        }

        public void AssignId(string locator, string identifier)
        {
            _innerSelenium.AssignId(locator, identifier);
        }

        public void AllowNativeXpath(string allow)
        {
            _innerSelenium.AllowNativeXpath(allow);
        }

        public void IgnoreAttributesWithoutValue(string ignore)
        {
            _innerSelenium.IgnoreAttributesWithoutValue(ignore);
        }

        public void WaitForCondition(string script, string timeout)
        {
            _innerSelenium.WaitForCondition(script, timeout);
        }

        public void SetTimeout(string timeout)
        {
            _innerSelenium.SetTimeout(timeout);
        }

        public void WaitForPageToLoad(string timeout)
        {
            Log($"Waiting {timeout} ms for page to load...");
            _innerSelenium.WaitForPageToLoad(timeout);
        }

        public void WaitForFrameToLoad(string frameAddress, string timeout)
        {
            _innerSelenium.WaitForFrameToLoad(frameAddress, timeout);
        }

        public string GetCookie()
        {
            return _innerSelenium.GetCookie();
        }

        public string GetCookieByName(string name)
        {
            return _innerSelenium.GetCookieByName(name);
        }

        public bool IsCookiePresent(string name)
        {
            return _innerSelenium.IsCookiePresent(name);
        }

        public void CreateCookie(string nameValuePair, string optionsString)
        {
            _innerSelenium.CreateCookie(nameValuePair, optionsString);
        }

        public void DeleteCookie(string name, string optionsString)
        {
            _innerSelenium.DeleteCookie(name, optionsString);
        }

        public void DeleteAllVisibleCookies()
        {
            _innerSelenium.DeleteAllVisibleCookies();
        }

        public void SetBrowserLogLevel(string logLevel)
        {
            _innerSelenium.SetBrowserLogLevel(logLevel);
        }

        public void RunScript(string script)
        {
            _innerSelenium.RunScript(script);
        }

        public void AddLocationStrategy(string strategyName,
            string functionDefinition)
        {
            _innerSelenium.AddLocationStrategy(strategyName, functionDefinition);
        }

        public void CaptureEntirePageScreenshot(string filename, string kwargs)
        {
            _innerSelenium.CaptureEntirePageScreenshot(filename, kwargs);
        }

        public void Rollup(string rollupName, string kwargs)
        {
            _innerSelenium.Rollup(rollupName, kwargs);
        }

        public void AddScript(string scriptContent, string scriptTagId)
        {
            _innerSelenium.AddScript(scriptContent, scriptTagId);
        }

        public void RemoveScript(string scriptTagId)
        {
            _innerSelenium.RemoveScript(scriptTagId);
        }

        public void UseXpathLibrary(string libraryName)
        {
            _innerSelenium.UseXpathLibrary(libraryName);
        }

        public void SetContext(string context)
        {
            _innerSelenium.SetContext(context);
        }

        public void AttachFile(string fieldLocator, string fileLocator)
        {
            _innerSelenium.AttachFile(fieldLocator, fileLocator);
        }

        public void CaptureScreenshot(string filename)
        {
            _innerSelenium.CaptureScreenshot(filename);
        }

        public string CaptureScreenshotToString()
        {
            return _innerSelenium.CaptureScreenshotToString();
        }

        public string CaptureEntirePageScreenshotToString(string kwargs)
        {
            return _innerSelenium.CaptureEntirePageScreenshotToString(kwargs);
        }

        public void ShutDownSeleniumServer()
        {
            _innerSelenium.ShutDownSeleniumServer();
        }

        public string RetrieveLastRemoteControlLogs()
        {
            return _innerSelenium.RetrieveLastRemoteControlLogs();
        }

        public void KeyDownNative(string keycode)
        {
            _innerSelenium.KeyDownNative(keycode);
        }

        public void KeyUpNative(string keycode)
        {
            _innerSelenium.KeyUpNative(keycode);
        }

        public void KeyPressNative(string keycode)
        {
            _innerSelenium.KeyPressNative(keycode);
        }

        #endregion
    }

    public interface IExtendedSelenium : ISelenium
    {
        void OpenAndAllowErrors(string url);
        void DetectYSOD();
    }
}
