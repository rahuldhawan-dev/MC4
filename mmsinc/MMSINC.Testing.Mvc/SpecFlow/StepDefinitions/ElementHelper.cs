using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.SeleniumMvc;
using OpenQA.Selenium;

namespace MMSINC.Testing.SpecFlow.StepDefinitions
{
    /// <summary>
    /// This for specific methods of trying to find elements that are annoyingly complicated. These methods
    /// are to reduce duplicate code between State and Input steps.
    /// </summary>
    /// <remarks>
    /// 
    /// All methods that return null elements must start with TryFind.
    /// All methods that throw an exception if the element can't be found must start with Find.
    /// 
    /// </remarks>
    public static class ElementHelper
    {
        #region Private methods

        private static By GetButtonByIdNameOrTextConstraints(string text)
        {
            // Find.ById and ByName work for any input/buttons.
            // Find.ByText is here because the value attr for <button></button> does not set its display text.
            // Note: If you have two buttons on the page that have the same name/text/id
            // then you're going to probably punch something when you find out.

            return ByHelper.Any(By.Id(text), By.Name(text), ByHelper.TextInTag(text, "button"));
        }

        #endregion

        #region Public Methods

        public static IBetterWebElement FindButtonByIdNameOrText(string text, IFindElements container = null)
        {
            container = container ?? WebDriverHelper.Current;
            return container.FindElement(GetButtonByIdNameOrTextConstraints(text));
        }

        public static IEnumerable<IBetterWebElement> FindButtonsByIdNameOrText(string text,
            IFindElements container = null)
        {
            container = container ?? WebDriverHelper.Current;
            return container.FindElements(GetButtonByIdNameOrTextConstraints(text));
        }

        public static IBetterWebElement FindElementByIdOrName(string finder, params object[] args)
        {
            var formatted = string.Format(finder, args);
            return WebDriverHelper.Current.FindElement(ByHelper.Any(By.Id(formatted), By.Name(formatted)));
        }

        public static IBetterWebElement FindElementByIdNameOrSelector(string finder, params object[] args)
        {
            var formatted = string.Format(finder, args);
            return WebDriverHelper.Current.FindElement(ByHelper.Any(By.Id(formatted), By.Name(formatted),
                By.CssSelector(formatted)));
        }

        /// <summary>
        /// Returns the content div for a given tab.
        /// </summary>
        /// <param name="tabText"></param>
        /// <returns></returns>
        public static IBetterWebElement FindTabContentContainerByTabText(string tabText)
        {
            var tab = TryFindTabByText(tabText);
            Assert.IsNotNull(tab, $"Tab with the text '{tabText}' was not found.");

            var tabLink = tab.FindElement(By.TagName("a"));
            var tabDivId = tabLink.GetAttribute("href").Split("#".ToCharArray(), 2)[1];
            var tabDiv = WebDriverHelper.Current.FindElement(By.Id(tabDivId));
            return tabDiv;
        }

        public static IBetterWebElement TryFindButtonByIdNameOrText(string text, IFindElements container = null)
        {
            container = container ?? WebDriverHelper.Current;
            return container.FindElements(GetButtonByIdNameOrTextConstraints(text)).SingleOrDefault();
        }

        /// <summary>
        /// Returns the tab that matches the text if the tab exists. This is *not* the a tag inside of the tab.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IBetterWebElement TryFindTabByText(string text)
        {
            var tabsContainer = WebDriverHelper.Current.FindElements(By.ClassName("tabs-container")).ToList();

            Assert.IsTrue(tabsContainer.Any(), "Tabs container was not found on the current page.");

            // NOTE: At the moment, you're doing something wrong if you have multiple tabs with the same label.
            var tab = tabsContainer.SelectMany(x => x.FindElements(ByHelper.Attribute("data-tab-text", text)));
            return tab.SingleOrDefault();
        }

        #endregion
    }
}
