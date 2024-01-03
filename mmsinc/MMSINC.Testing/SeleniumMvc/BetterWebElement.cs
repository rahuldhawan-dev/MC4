using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MMSINC.Testing.SeleniumMvc
{
    public enum ElementTypes
    {
        Unknown,
        CheckBox,
        CheckBoxList,
        File,
        Select,
        TextArea,
        TextBox,
    }

    public interface IBetterWebElement : IFindElements
    {
        #region Properties

        // NOTE: I am using properties for things that are read-only and unlikely to ever change.
        string Id { get; }
        IWebElement InternalElement { get; }
        bool IsChecked { get; }
        bool IsDisplayed { get; }
        bool IsEnabled { get; }
        string TagName { get; }
        string Text { get; }
        ElementTypes Type { get; }

        #endregion

        #region Methods

        void Check(bool check);
        void Click();
        void DragTo(IBetterWebElement otherElement);
        void ForceClick();
        string GetAttribute(string attributeName);
        IEnumerable<IBetterWebElement> GetCheckBoxListItems();
        IEnumerable<IBetterWebElement> GetCheckedCheckBoxListItems();
        IEnumerable<string> GetCssClasses();
        string GetCssValue(string propertyName);
        IEnumerable<string> GetAllSelectedOptionValues();
        System.Drawing.Point GetLocation();
        T GetProperty<T>(string propertyName);
        IEnumerable<IBetterWebElement> GetSelectOptions();
        string GetSelectedOptionText();
        string GetSelectedOptionValue();
        System.Drawing.Size GetSize();
        string GetValue();
        void SetSelectedOptionByText(string optionText);
        void SetAttribute(string attributeName, object value);
        void SetFileUploadPath(string path);
        void SetValue(object value);
        void TriggerJQueryEvent(JQueryEventType eventType);
        void TriggerEvent(JavascriptEventType eventType);
        void TypeValue(string text, bool clearPreviousText = true);

        #endregion
    }

    /// <summary>
    /// Wrapper class for Selenium's IWebElement. 
    /// </summary>
    public class BetterWebElement : IBetterWebElement
    {
        #region Consts

        private const string MC_CHECKBOXLISTITEM = "mc-checkboxlistitem";

        #endregion

        #region Fields

        private readonly IWebElement _element;
        private readonly IBetterWebDriver _driver;
        private ElementTypes? _type;

        #endregion

        #region Properties

        public string Id => GetAttribute("id");

        /// <summary>
        /// Returns the internal IWebElement instance used for this wrapper. I recommend that
        /// this does *not* get used outside of the BetterWebElement class, but it is needed
        /// for use with interactions.
        /// </summary>
        public IWebElement InternalElement => _element;

        /// <summary>
        /// Returns true if this is a checkbox element that's checked.
        /// </summary>
        public bool IsChecked => GetProperty<bool>("checked");

        /// <summary>
        /// Returns true if the element is currently displayed. If this is returning false 
        /// even though the element is visible(not css hidden, but may need to scroll to see) then this
        /// isn't the right property for you.
        /// </summary>
        public bool IsDisplayed
        {
            get
            {
                // NOTE: _element.IsDisplayed will return false when an element is visible but might be pushed
                // off the screen. the jQuery method does not have this problem.
                return (bool)_driver.ExecuteScript($"return jQuery(arguments[0]).is(\":visible\")", _element);
            }
        }

        /// <summary>
        /// Returns true if the element is currently enabled.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                // Selenium's WebElement.Enabled check calls to some internal WebDriver
                // helper command. I have no idea how it works, but I do know that it is
                // not doing a basic check to see if an element has an "enabled" property.
                // So we need to special-case this for web components. We also would probably
                // have to special-case this for anything that isn't a form input.
                if (TagName == MC_CHECKBOXLISTITEM)
                {
                    return (bool)_driver.ExecuteScript("return arguments[0].enabled", _element);
                }

                return _element.Enabled;
            }
        }

        /// <summary>
        /// Returns the tag name of this element.
        /// </summary>
        public string TagName => _element.TagName;

        /// <summary>
        /// Returns the innerText value of this element. If you're trying to get the value of a textbox, use GetValue().
        /// </summary>
        /// <remarks>
        /// the textContent attribute returns the actual text value and not one that may have
        /// been altered by css text-transforms.
        /// </remarks>
        public string Text
        {
            get
            {
                // This removes excess white space that might exist inside an element but 
                // is otherwise not actually visible to a user.
                var baseText = _element.GetAttribute("textContent");
                return Regex.Replace(baseText, @"\s+", " ").Trim();
            }
        }

        /// <summary>
        /// Returns the type of element this thing is.
        /// </summary>
        public ElementTypes Type
        {
            get
            {
                if (_type == null)
                {
                    _type = GetElementType();
                }

                return _type.Value;
            }
        }

        #endregion

        #region Constructors

        public BetterWebElement(IWebElement internalElement, IBetterWebDriver driver)
        {
            _element = internalElement;
            _driver = driver;
        }

        #endregion

        #region Private Methods

        private void EnsureElementType(params ElementTypes[] types)
        {
            if (!types.Contains(Type))
            {
                var expected = string.Join(" ", types);
                throw new InvalidOperationException(
                    $"This element is of type {Type}(tag: {TagName}) and not one of the expected types: {expected}");
            }
        }

        private ElementTypes GetElementType()
        {
            switch (TagName)
            {
                case "input":
                    switch (GetAttribute("type"))
                    {
                        case "checkbox":
                            return ElementTypes.CheckBox;
                        case "file":
                            return ElementTypes.File;
                        case "text":
                            return ElementTypes.TextBox;
                    }

                    break;
                // At the time of writing this, interacting with mc-checkboxlistitem
                // seems to work the same as a regular checkbox element without any
                // additional workarounds. -Ross 8/28/2019
                case MC_CHECKBOXLISTITEM:
                    return ElementTypes.CheckBox;
                case "select":
                    return ElementTypes.Select;
                case "textarea":
                    return ElementTypes.TextArea;
                case "div":
                    if (GetCssClasses().Contains("checkbox-list"))
                    {
                        return ElementTypes.CheckBoxList;
                    }

                    break;
            }

            return ElementTypes.Unknown;
        }

        /// <summary>
        /// Scrolls the element into view. This is sometimes necessary because something selenium
        /// is about to do will throw an ElementNotVisibleException, like when clicking is involved.
        /// </summary>
        private void ScrollIntoView()
        {
            _driver.ExecuteScript("arguments[0].scrollIntoView()", _element);
        }

        #endregion

        #region Public Methods

        public void Check(bool check)
        {
            EnsureElementType(ElementTypes.CheckBox);

            var isCurrentlyChecked = IsChecked;

            if ((check && !isCurrentlyChecked) || (!check && isCurrentlyChecked))
            {
                // We need to actually click the element, rather than setting the checked attribute
                Click();
            }
        }

        /// <summary>
        /// Clicks this element. If the element is not visible, an exception is thrown.
        /// </summary>
        public void Click()
        {
            // There's a bug that's either related to Chrome or possibly validation stuff,
            // but sometimes you have to click a button twice to get it to actually do clicking
            // things. Rather than click twice, focusing the button first and then clicking it
            // seems to solve the problem.

            if (_element.TagName == "button")
            {
                _driver.ExecuteScript("arguments[0].focus()", _element);
            }

            // NOTE: If this is throwing an ElementNotVisibleException then you need to ensure
            // the element is actually *visible*. ie: If you're trying to click something inside 
            // of a tab, but the tab hasn't been selected, then nothing in that tab is currently
            // visible.

            try
            {
                _element.Click();
            }
            catch (InvalidOperationException invalid) when (invalid.Message.Contains("is not clickable at point"))
            {
                // This means the element is visible but it's hidden underneath something and needs to be
                // scrolled into view. ex: button hiding under a sticky footer.
                ScrollIntoView();
                _element.Click();
            }
            catch (ElementNotInteractableException notInteractable) when (notInteractable.Message.Contains(
                "element not interactable"))
            {
                // This exception is thrown when the element is not scrolled into view.
                ScrollIntoView();
                _element.Click();
            }
            catch (ElementNotVisibleException)
            {
                if (!_element.Displayed && IsDisplayed)
                {
                    // If the base element Displayed property is false, that means that it isn't
                    // visible in the viewport. If the IsDisplayed property is true, that means that
                    // it could still be visible but not in the viewport. If this happens, we can try
                    // clicking the element via javascript. NOTE: Don't use jQuery click, it doesn't work
                    // like you'd expect it to.
                    ForceClick();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Drags the current element to the same position as the given element.
        /// </summary>
        /// <param name="otherElement"></param>
        public void DragTo(IBetterWebElement otherElement)
        {
            var act = WebDriverHelper.Current.CreateInteraction();
            act.DragAndDrop(InternalElement, otherElement.InternalElement).Build().Perform();
        }

        public override bool Equals(object obj)
        {
            if (obj is BetterWebElement)
            {
                return _element.Equals(((BetterWebElement)obj)._element);
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Returns elements that match the given contraint.
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        public IEnumerable<IBetterWebElement> FindElements(By constraint)
        {
            // NOTE: If you change anything in this method, you may very well need to change it in BetterWebDriver as well.
            foreach (var el in _element.FindElements(constraint))
            {
                yield return new BetterWebElement(el, _driver);
            }
        }

        /// <summary>
        /// Returns the single element that matches the given constraint. An error is thrown if no element is found 
        /// or if there is more than one match.
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        public IBetterWebElement FindElement(By constraint)
        {
            // NOTE: If you change anything in this method, you may very well need to change it in BetterWebDriver as well.
            var result = _element.FindElement(constraint);
            return new BetterWebElement(result, _driver);
        }

        /// <summary>
        /// You should absolutely not be using this except in an instance where the element
        /// being clicked on is impossible to scroll into view(ex: A button inside a modal dialog
        /// where the button itself can be scrolled into view but the dialog can not be scrolled
        /// into view, ex2: the authorize.net popups when the chrome window is too small)
        /// </summary>
        public void ForceClick()
        {
            _driver.ExecuteScript("arguments[0].click()", _element);
        }

        /// <summary>
        /// Returns the value for a given attribute. Returns null if the element does not have the attribute.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public string GetAttribute(string attributeName)
        {
            return _element.GetAttribute(attributeName);
        }

        /// <summary>
        /// Returns all of the mc-checkboxlistitems in this element if the
        /// element is a CheckBoxList.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IBetterWebElement> GetCheckBoxListItems()
        {
            EnsureElementType(ElementTypes.CheckBoxList);
            return FindElements(By.TagName(MC_CHECKBOXLISTITEM));
        }

        /// <summary>
        /// Returns the mc-checkboxlistitems that are checked.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IBetterWebElement> GetCheckedCheckBoxListItems()
        {
            EnsureElementType(ElementTypes.CheckBoxList);
            // NOTE: Can't do an attribute search on this because the "checked" attribute
            // isn't required or in sync with the actual checkbox.
            return FindElements(By.TagName(MC_CHECKBOXLISTITEM)).Where(x => x.IsChecked);
        }

        /// <summary>
        /// Returns all of the css classes attached to this element.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetCssClasses()
        {
            // NOTE: Do not do a yield return in here as it may result
            //       in unintended stale element references. 

            var classAttrVal = GetAttribute("class");
            if (string.IsNullOrWhiteSpace(classAttrVal))
            {
                return Enumerable.Empty<string>();
            }

            return classAttrVal.Split(' ');
        }

        /// <summary>
        /// Returns the current css value for the given property.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string GetCssValue(string propertyName)
        {
            return _element.GetCssValue(propertyName);
        }

        public override int GetHashCode()
        {
            return _element.GetHashCode();
        }

        public System.Drawing.Point GetLocation()
        {
            return _element.Location;
        }

        /// <summary>
        /// Similar to GetAttribute except it returns a property value instead.
        /// Not all properties have cooresponding attributes, or the attributes
        /// are only there for initialization but do not stay in sync with the
        /// property(ex: regular checkboxes and the checked attribute).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public T GetProperty<T>(string propertyName)
        {
            return (T)_driver.ExecuteScript($"return arguments[0].{propertyName}", _element);
        }

        /// <summary>
        /// Returns all the options in a select tag.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IBetterWebElement> GetSelectOptions()
        {
            EnsureElementType(ElementTypes.Select);
            var select = new OpenQA.Selenium.Support.UI.SelectElement(_element);
            foreach (var opt in select.Options)
            {
                yield return new BetterWebElement(opt, _driver);
            }
        }

        /// <summary>
        /// Returns the display text of the selected option in a dropdown.
        /// </summary>
        /// <returns></returns>
        public string GetSelectedOptionText()
        {
            EnsureElementType(ElementTypes.Select);

            var select = new OpenQA.Selenium.Support.UI.SelectElement(_element);
            // If this is acting funny, we might need to wrap it in BetterWebElement.
            try
            {
                return select.SelectedOption.Text;
            }
            catch (NoSuchElementException)
            {
                // NoSuchElementException is thrown when no item is currently selected.
                return null;
            }
        }

        /// <summary>
        /// Returns the value of the selected option in a dropdown.
        /// </summary>
        /// <returns></returns>
        public string GetSelectedOptionValue()
        {
            EnsureElementType(ElementTypes.Select);

            var select = new OpenQA.Selenium.Support.UI.SelectElement(_element);
            // If this is acting funny, we might need to wrap it in BetterWebElement.
            try
            {
                return select.SelectedOption.GetAttribute("value");
            }
            catch (NoSuchElementException)
            {
                // NoSuchElementException is thrown when no item is currently selected.
                return null;
            }
        }

        /// <summary>
        /// REturns all of the selected values for a multi select element.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="GenericSeleniumException"></exception>
        public IEnumerable<string> GetAllSelectedOptionValues()
        {
            EnsureElementType(ElementTypes.Select);
            if (!GetProperty<bool>("multiple"))
            {
                throw new GenericSeleniumException("This select element does not have a multiple attribute on it.");
            }
            
            var select = new OpenQA.Selenium.Support.UI.SelectElement(_element);
            return select.AllSelectedOptions.Select(x => x.GetAttribute("value")).ToList();
        }

        /// <summary>
        /// Returns the size of this element.
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Size GetSize()
        {
            return _element.Size;
        }

        /// <summary>
        /// Returns the current value of the value attribute.
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            return GetAttribute("value");
        }

        /// <summary>
        /// Sets the attribute of an element to a specific value.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="value"></param>
        public void SetAttribute(string attributeName, object value)
        {
            _driver.ExecuteScript($"arguments[0].setAttribute(\"{attributeName}\", \"{value}\")", _element);
        }

        /// <summary>
        /// Sets the file upload path for a file upload input.
        /// </summary>
        /// <param name="path"></param>
        public void SetFileUploadPath(string path)
        {
            EnsureElementType(ElementTypes.File);

            // Ensure the file exists before uploading as chromedriver doesn't throw any sort of error when trying to
            // upload a file that doesn't exist.
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The file '{path}' does not exist.");
            }

            // This differs from TypeValue because firing events
            // breaks things with the file uploader.
            _element.SendKeys(path);
        }

        /// <summary>
        /// If this is a select tag, this will select the option with the specified display text.
        /// </summary>
        /// <param name="text"></param>
        public void SetSelectedOptionByText(string text)
        {
            EnsureElementType(ElementTypes.Select);
            text = text.Trim();

            // NOTE: The script version works a lot faster(20ms vs 100ms) than using selenium's
            //       select stuff and then firing off two events. If that starts breaking, though,
            //       then go back to using this.

            //var select = new OpenQA.Selenium.Support.UI.SelectElement(_element);
            //select.SelectByText(text);

            // NOTE: The blur event isn't needed here since no actual clicking/focusing is occurring.
            const string script = @"
    var dropdown = arguments[0];
    var text = arguments[1];
    for (var i = 0; i < dropdown.options.length; i++) {
        var opt = dropdown.options[i];
        if (opt.text === text) {
            opt.selected = true;     
            return true;
        }
    }

    return false;";

            var result = (bool)_driver.ExecuteScript(script, this.InternalElement, text);

            if (!result)
            {
                throw new GenericSeleniumException($"Unable to find select option with '{text}'");
            }

            TriggerEvent(JavascriptEventType.Change);

            // The blur event needs to be fired so the dropdown is closed before moving on 
            // to other actions, as sometimes the dropdown being open causes elements to be
            // unclickable. ex: If you have a DisplayFor with a dropdown, and underneath that
            // is a DisplayFor with a checkbox, selecting an item and then trying to click the
            // checkbox will not work. JobSiteCheckList.UserCanCreateAChecklist will fail when
            // trying to check the HasFlagPersonForTrafficControl checkbox after selecting
            // something from the HasTrafficControl dropdown right above it. Annoyingly, WebDriver
            // does not throw an exception when the click fails to actually do something uesful.
            // TriggerEvent(JQueryEventType.Blur);
        }

        /// <summary>
        /// Directly sets the value of the element.
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(object value)
        {
            // NOTE: Is this failing because of unexpected/illegal characters? Then you probably
            // have text with a line break in it which is causing invalid javascript. Use TypeValue
            // instead if you're having this problem. 
            _driver.ExecuteScript($"arguments[0].value = \"{value}\"", _element);

            // Need to fire the changed event when this happens.
            TriggerJQueryEvent(JQueryEventType.Change);

            // Need to fire the keyup event as well for jQuery validation. No clue 
            // why jQuery validation doesn't listen to the changed event. Hopefully
            // this doesn't screw up stuff.
            TriggerJQueryEvent(JQueryEventType.Keyup);
        }

        public override string ToString()
        {
            // I dunno that this returns anything useful. Maybe we can get it to display the 
            // html of the tag instead?
            return _element.ToString();
        }

        /// <summary>
        /// Triggers an event using jQuery.
        /// </summary>
        /// <param name="eventType"></param>
        [Obsolete("Should prefer TriggerEvent instead.")]
        public void TriggerJQueryEvent(JQueryEventType eventType)
        {
            var e = new JQueryEvent(_driver) {Type = eventType};
            e.Fire(this);
        }

        public void TriggerEvent(JavascriptEventType eventType)
        {
            var e = new JavascriptEvent(_driver);
            e.Fire(this, eventType);
        }

        /// <summary>
        /// Simulates typing the value.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="clearPreviousText">If true, the text currently in the element will be cleared before typing begins. Default is true.</param>
        public void TypeValue(string text, bool clearPreviousText = true)
        {
            // If Clear isn't called then the value gets appended to what's already in the textbox.
            if (clearPreviousText)
            {
                _element.Clear();
            }

            _element.SendKeys(text);

            // If you're typing something here and then checking for a validation message, make sure
            // that the step following this also does something to trigger the blur event on the
            // textbox, like hitting the save button in the form.
        }

        #endregion
    }
}
