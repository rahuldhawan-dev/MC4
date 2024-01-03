using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;

namespace MMSINC.Testing.SeleniumMvc
{
    /// <summary>
    /// Additional By constraints for selenium tests.
    /// </summary>
    public static class ByHelper
    {
        #region Public Methods

        /// <summary>
        /// Returns the nearest ancestor with the given css class.
        /// </summary>
        /// <param name="cssClass"></param>
        /// <returns></returns>
        public static By AncestorCssClass(string cssClass)
        {
            return By.XPath($"ancestor::*[contains(@class, \"{cssClass}\")]");
        }

        /// <summary>
        /// Returns the nearest ancestor with the given tag.
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static By AncestorTagName(string tagName)
        {
            return By.XPath($"ancestor::{tagName}");
        }

        /// <summary>
        /// Returns the FIRST element that matches *any* of the constraints given in the
        /// order that the constraints are passed.
        /// </summary>
        /// <param name="constraints"></param>
        /// <returns></returns>
        public static By Any(params By[] constraints)
        {
            return new ByAny(constraints);
        }

        /// <summary>
        /// This returns elements that match have an attribute value that matches *exactly*. Trying to match
        /// the href of a link will not work if it's a partial url and you're trying to find an absolute url.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value">Optional value. If this isn't set, then elements that have the attribute with
        /// any value will be matched</param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static By Attribute(string attribute, string value = null, string tagName = "*")
        {
            var selector = value == null ? $"{tagName}[{attribute}]" : $"{tagName}[{attribute}=\"{value}\"]";
            return By.CssSelector(selector);
            // The XPath one works, but the css selector one is every so slightly faster.
            // return new ByXpathCorrected { XPath = $"//{tagName}[@{attribute}=\"{value}\"]" };
        }

        /// <summary>
        /// Returns elements with the matching href value. Use this as opposed to XPath by attribute as that
        /// does not work when trying to match absolute urls to partial urls.
        /// </summary>
        /// <param name="href"></param>
        /// <returns></returns>
        public static By Href(string href)
        {
            return new ByHref {Href = href};
        }

        /// <summary>
        /// Returns elements with the matching href value to the given regular expression. 
        /// Use this as opposed to XPath by attribute as that does not work when trying to
        /// match absolute urls to partial urls.
        /// </summary>
        /// <param name="hrefRegex"></param>
        /// <returns></returns>
        public static By Href(Regex hrefRegex)
        {
            return new ByHref {HrefRegex = hrefRegex};
        }

        /// <summary>
        /// Constraint for returning elements that have a partial text match.
        /// </summary>
        /// <returns></returns>
        public static By PartialTextInTag(string partialText, string tag = "*")
        {
            // NOTE: XPath does not support escaping characters!
            // NOTE 2: This finds the inner text node that actually contains the text, then returns
            //         the nearest ancestor that matches the given tag.
            // NOTE 3: This does not work properly when trying to find text that is split up into different
            //         tags. ex: You can't match "this string" if you do <div>this<span>string</span></div>

            return XPath($"//{tag}/text()[contains(., \"{partialText}\")]/..");
        }

        /// <summary>
        /// Constraint for returning elements that have an exact text match.
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// The tag is necessary to prevent returning multiple elements when an element
        /// contains text and is wrapped by another element that does not add any additional text.
        /// 
        /// ex: This will return two matches for <li><span>blah</span></li> because both tags contain
        /// the same text, technically.
        /// </summary>
        public static By TextInTag(string text, string tag = "*")
        {
            // NOTE: XPath does not support escaping characters!
            // NOTE: This finds the inner text node that actually contains the text, then returns
            //       the nearest ancestor that matches the given tag.
            return XPath($"//{tag}/text()[.= \"{text}\"]/..");
        }

        /// <summary>
        /// Use this for xpath as it correctly scopes the XPath to either the entire DOM
        /// or a specific web element depending on if WebDriver or WebElement is being used
        /// for searching.
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static By XPath(string xpath)
        {
            return new ByXpathCorrected {XPath = xpath};
        }

        #endregion

        #region Helper Classes

        private class ByHref : By
        {
            public string Href { get; set; }
            public Regex HrefRegex { get; set; }

            public override IWebElement FindElement(ISearchContext context)
            {
                try
                {
                    return FindElements(context).Single();
                }
                catch (InvalidOperationException e)
                {
                    // FindElement must throw an exception when it can not find an element. These exceptions are
                    // here to help debug the problem, cause otherwise it just says there's no match but not what it's
                    // looking for.
                    if (HrefRegex != null)
                    {
                        throw new InvalidOperationException($"Unable to find single element that matches the HrefRegex: {HrefRegex}", e);
                    }
                    throw new InvalidOperationException($"Unable to find single element that matches the Href: {Href}", e);
                }
            }

            public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
            {
                var allTags = context.FindElements(By.TagName("a"));
                if (HrefRegex != null)
                {
                    var tags = allTags.Where(x => {
                        var href = x.GetAttribute("href");
                        return !string.IsNullOrWhiteSpace(href) && HrefRegex.IsMatch(href);
                    });
                    return new ReadOnlyCollection<IWebElement>(tags.ToList());
                }
                else
                {
                    // For some reason, somewhere, we started testing urls that were being generated(either on the site itself, or in the regression tests)
                    // with improper casing. 
                    var hrefForComparison = Href.ToLowerInvariant();
                    // NOTE: IWebElement.GetAttribute returns null when the object does not contain an attribute.
                    var tags = allTags.Where(x => x.GetAttribute("href")?.ToLowerInvariant() == hrefForComparison);
                    return new ReadOnlyCollection<IWebElement>(tags.ToList());
                }
            }
        }

        /// <summary>
        /// This is a replacement for most usages of By.Xpath. The By.Xpath method does not correctly
        /// scope the XPath to a specific element when IWebElement.FindElement(s) is called. It instead
        /// continues to XPath the entire DOM instead.
        /// </summary>
        private class ByXpathCorrected : By
        {
            public string XPath { get; set; }

            private By GetXPathConstraint(ISearchContext context)
            {
                if (context is IWebElement)
                {
                    // The . is needed to force the xpath to be in the context of the current element.
                    return By.XPath("." + XPath);
                }

                return By.XPath(XPath);
            }

            public override IWebElement FindElement(ISearchContext context)
            {
                return context.FindElement(GetXPathConstraint(context));
            }

            public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
            {
                return context.FindElements(GetXPathConstraint(context));
            }
        }

        /// <summary>
        /// Returns the first element that matches any of the constraints.
        /// </summary>
        private class ByAny : By
        {
            private readonly IList<By> _constraints;

            public ByAny(params By[] constraints)
            {
                _constraints = constraints;
                Description = string.Join(" or ", constraints.Select(x => x.ToString()));
            }

            public override IWebElement FindElement(ISearchContext context)
            {
                // To keep this in sync with how FindElement works everywhere else, we want this
                // to throw an exception if one of the constraints returns multiple elements, but
                // only as we hit the constraint. We also only want this to throw an exception if
                // none of them return an element.

                foreach (var c in _constraints)
                {
                    try
                    {
                        return c.FindElement(context);
                    }
                    catch (NoSuchElementException)
                    {
                        // noop
                    }
                }

                throw new NoSuchElementException(Description);
            }

            public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
            {
                var results = _constraints.SelectMany(x => x.FindElements(context)).Distinct();
                return new ReadOnlyCollection<IWebElement>(results.ToList());
            }
        }

        #endregion
    }
}
