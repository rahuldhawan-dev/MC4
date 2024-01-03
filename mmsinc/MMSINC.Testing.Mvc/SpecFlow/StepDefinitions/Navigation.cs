using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using JetBrains.Annotations;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Testing.SeleniumMvc;
using MMSINC.Testing.SpecFlow.Library;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Config = MMSINC.Testing.Utilities.RegressionTestConfiguration;

namespace MMSINC.Testing.SpecFlow.StepDefinitions
{
    [Binding]
    public static class Navigation
    {
        #region Properties

        // TODO: This should really be a Dictionary<string,string> since NameValueCollection
        //       allows for multiple values for the same key.
        public static NameValueCollection PageStrings { get; private set; }

        #endregion

        #region Steps

        [Given("I am at the (.+) page")]
        [When("^I (?:try to )?(?:access|visit|go to) the (.+) page$")]
        public static void GivenIAmAtAPage(string page)
        {
            page = GetPageString(page);
            if (!page.StartsWith("/"))
            {
                page = "/" + page;
            }

            GivenIAmAtUrl(page, false);
        }

        [Given("^I am at the (.+) page expecting an error$")]
        [When("^I (?:try to )?(?:access|visit|go to) the (.+) page expecting an error$")]
        public static void GivenIAmAtAPageExpectingAnError(string page)
        {
            page = GetPageString(page);
            if (!page.StartsWith("/"))
            {
                page = "/" + page;
            }

            GivenIAmAtUrl(page, false, true);
        }

        [Given("^I am at the ([^\" ]+) (page|frag) for ([^\":]+):? \"([^\"]+)\"")]
        [When("^I (?:access|visit|go to) the ([^\" ]+) (page|frag) for ([^\":]+):? \"([^\"]+)\"")]
        public static void GivenIBeAtThePageFor(string action, string pageFrag, string className, string entity)
        {
            GivenIAmAtUrl(GetUrlFor(action, className, entity, (pageFrag == "frag")), true);
        }

        [When("^I (?:access|visit|go to) the ([^\" ]+) page for ([^\":]+):? \"([^\"]+)\" to see the ([^\" ]+) tab")]
        public static void GivenIBeAtThePageForWithATab(string action, string className, string entity, string tab)
        {
            GivenIAmAtUrl(GetUrlFor(action, className, entity, false, tab), true);
        }

        [When("^I (?:access|visit|go to) the ([^\" ]+) page for ([^\"]+): \"([^\"]+)\" expecting an error$")]
        public static void GivenIBeAtThePageForExpectingAnError(string action, string className, string entity)
        {
            GivenIAmAtUrl(GetUrlFor(action, className, entity), false, true);
        }

        [Given("^I am at the ([^\"]+) page for ([^\"]+): \"([^\"]+)\" with (.+)")]
        [When("^I (?:access|visit|go to) the ([^\"]+) page for ([^\"]+): \"([^\"]+)\" with (.+)")]
        public static void GivenIBeAtThePageForThingWithQueryString(string action, string className, string entity,
            string queryString)
        {
            GivenIAmAtUrl(
                GetUrlFor(action, className, entity, queryString), true);
        }

        [When("^I try to (?:access|visit|go to) the ([^\"]+) page for ([^\"]+): \"([^\"]+)\"")]
        public static void WhenITryToAccessThePageFor(string action, string className, string entity)
        {
            GivenIAmAtUrl(GetUrlFor(action, className, entity), false);
        }

        [When("^I try to (?:access|visit|go to) the ([^\"]+) page for ([^\"]+): \"([^\"]+)\" expecting an error")]
        public static void WhenITryToAccessThePageForExpectingAnError(string action, string className, string entity)
        {
            GivenIAmAtUrl(GetUrlFor(action, className, entity), false, true);
        }

        [When("^I try to (?:access|visit|go to) the ([^\"]+) page for ([^\"]+): \"([^\"]+)\" with (.+)")]
        public static void WhenITryToAccessThePageForThingWithQueryString(string action, string className,
            string entity, string queryString)
        {
            GivenIAmAtUrl(
                GetUrlFor(action, className, entity, queryString), false);
        }

        [When("I go back")]
        public static void IGoBack()
        {
            WebDriverHelper.Current.GoBack();
        }

        #endregion

        #region Helper Methods

        public static void SetPageStringDictionary(NameValueCollection nvc)
        {
            // Validate that the collection is correct before setting it.
            var duplicateKeys = new List<string>();

            foreach (var key in nvc.AllKeys)
            {
                var vals = nvc.GetValues(key);
                if (vals == null || vals.Length > 1)
                {
                    duplicateKeys.Add(key);
                }
            }

            if (duplicateKeys.Any())
            {
                throw new InvalidOperationException(
                    "There are PageStrings keys that are either duplicated or have null values: " +
                    string.Join(", ", duplicateKeys));
            }

            PageStrings = nvc;
        }

        public static void GivenIAmAtUrl(string relativePathUrl, bool verifyCorrectUrl, bool expectError = false)
        {
            VisitRelativePath(relativePathUrl, expectError);
            if (verifyCorrectUrl)
            {
                VerifyCurrentUrl(relativePathUrl);
            }
        }

        public static void VisitRelativePath(string path, bool expectError = false)
        {
            var url = GetAbsoluteUriFromRelativePath(path);
            WebDriverHelper.Current.GoToUrl(url);
            if (!expectError)
            {
                State.AssertThereIsNoYSOD($"Unexpected YSOD visiting url '{path}'!");
            }
        }

        public static Uri GetAbsoluteUriFromRelativePath(string relativePath)
        {
            return new Uri(Config.GetDevSiteUri(), relativePath);
        }

        public static void VerifyCurrentUrl(string relativePathUrl)
        {
            UrlsAreEqual(GetAbsoluteUriFromRelativePath(relativePathUrl).ToString(),
                WebDriverHelper.Current.CurrentUri.ToString());
        }

        public static string ParseQueryString(string queryString)
        {
            if (String.IsNullOrWhiteSpace(queryString))
            {
                return String.Empty;
            }

            var sb = new StringBuilder("?");
            var qs = Data.ConvertToNameValueCollection(queryString);
            foreach (var kv in qs.AllKeys)
            {
                ParseQueryStringKeyValue(kv, qs[kv], sb);
            }

            return sb.ToString();
        }

        public static void ParseQueryStringKeyValue(string key, string value, StringBuilder sb)
        {
            // I apologize for this hackish method. If we actually need more querystring
            // parsing in the future, we can obliterate this. -Ross 2/1/2012
            if (key.Equals("date", StringComparison.InvariantCultureIgnoreCase))
            {
                value = value.ToDateTime().ToShortDateString();
            }

            sb.AppendFormat("{0}={1}", key, value);
        }

        public static Uri GetUriFor(string action, string className, string entity)
        {
            return GetUriForPage(GetUrlFor(action, className, entity));
        }

        public static Uri GetUriFor(string action, object obj, string objTypeName = null)
        {
            return GetUriForPage(
                string.Format(
                    "{0}/{1}/{2}",
                    GetPageString(objTypeName ?? obj.GetType().Name.ToLowerSpaceCase()),
                    action,
                    Data.GetEntityId(obj)));
        }

        public static Uri GetUriForPage(string page)
        {
            return new Uri(new Uri(Config.GetDevSiteUrl()),
                GetPageString(page));
        }

        public static string GetUrlFor(string action, string className, string entity, bool frag = false,
            string tab = null)
        {
            var urlFormatString = (frag) ? "{0}/{1}.frag" : "{0}/{1}";
            var entityId = Data.GetEntityId(TestObjectCache.Instance[className][entity]);
            var url = String.Format(urlFormatString, action, entityId);
            if (!action.Contains("/"))
            {
                url = String.Format("{0}/{1}", GetPageString(className), url);
            }

            if (!string.IsNullOrWhiteSpace(tab))
            {
                url = url + "#" + tab + "Tab";
            }

            return url;
        }

        public static string GetUrlFor(string action, string className, string entity, string queryString)
        {
            return GetUrlFor(action, className, entity) + ParseQueryString(queryString);
        }

        /// <summary>
        /// Returns the controller path for a thing. If a thing is not registered with PageStrings,
        /// it will return the value passed in. If the value passed in has any spaces, the spaces
        /// will be removed.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string GetPageString(string page)
        {
            page = GetProperControllerName(page);
            // need to eat the root url, otherwise it tries to check for "//"
            page = (page == "/" ? String.Empty : page);
            return page;
        }

        private static string GetProperControllerName(string page)
        {
            if (PageStrings != null && PageStrings[page] != null)
            {
                //var possibleNames = PageStrings.GetValues(page);
                //if (possibleNames.Length > 1)
                //{
                //    throw new InvalidOperationException("The PageStrings collection has duplicate entries for '" + page +"'. My diagnosis? Bad babysitting. Or a bad merge.");
                //}
                // Since there's a registered path, we don't need to do anything besides return the registered value.
                return PageStrings[page];
            }

            return page.Replace(" ", String.Empty);
        }

        public static void UrlsAreEqual(string expected, string result)
        {
            StringAssert.AreEqualIgnoringCase(expected, HttpUtility.UrlDecode(result));
        }

        public static void UrlsAreEqual(Uri expected, Uri result)
        {
            // Uri's Equals overload ignores case
            Assert.AreEqual(expected, result);
        }

        public static void UrlsAreNotEqual(string expected, string result)
        {
            StringAssert.AreNotEqualIgnoringCase(expected, result);
        }

        public static void UrlsAreNotEqual(Uri expected, Uri result)
        {
            // Uri's Equals overload ignores case
            Assert.AreNotEqual(expected, result);
        }

        [StringFormatMethod("message")]
        public static void UrlStartsWith(string expected, string result, string message, params object[] args)
        {
            if (!result.StartsWith(expected, StringComparison.InvariantCultureIgnoreCase))
            {
                Assert.Fail(message, args);
            }
        }

        #endregion
    }
}
