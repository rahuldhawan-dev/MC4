using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MapCall.Common.Testing.Selenium;
using RegressionTests.Lib.TestParts;
using RegressionTests.Tests;
using Config = MMSINC.Testing.Utilities.RegressionTestConfiguration;

namespace RegressionTests.Lib
{
    public abstract class BaseTest
    {
        #region Properties

        public virtual IExtendedSelenium Selenium
        {
            get { return SetUpFixtureBase.Selenium; }
        }

        #endregion
    }

    public abstract class BaseSearchTest<T> : BaseTest where T : TestPage 
    {
        public void RunTests(IEnumerable<T> testPages, params string[] butReallyOnlyTest)
        {
            RunTests(testPages, false, butReallyOnlyTest);
        }

        public void RunTests(IEnumerable<T> testPages, bool stopOnError = false, params string[] butReallyOnlyTest)
        {
            for (var i = butReallyOnlyTest.Length - 1; i >= 0; --i)
            {
                butReallyOnlyTest[i] = Config.GetDevSiteUrl() + butReallyOnlyTest[i];
            }
            var sb = new StringBuilder();

            var results = new List<long>();
            var sw = Stopwatch.StartNew();
            var errors = 0;

            foreach (var testPage in testPages)
            {
                sw.Restart();

                try
                {
                    try
                    {
                        if (butReallyOnlyTest.Any() && !butReallyOnlyTest.Contains(testPage.Url))
                        {
                            continue;
                        }
                        // Try once, it's possible a fluke 
                        // will cause it to throw.
                        TestTheTestPage(testPage);
                    }
                    catch (Exception)
                    {
                        if (stopOnError)
                        {
                            throw;
                        }

                        // So if a fluke shows up, try it a second time.
                        // This will get caught by the other catch 
                        // so it throws properly.
                        TestTheTestPage(testPage);
                    }
                }
                catch (Exception e)
                {
                    if (stopOnError)
                    {
                        throw;
                    }

                    errors++;
                    var msg = string.Format("Url: {0}, Type: {1}, Message: {2}\n", testPage.Url, e.GetType(), e.Message);
                    Debug.Print(msg); // Print is here so we can see the error before the test finishes.
                    sb.Append(msg);
                }

                sw.Stop();
                results.Add(sw.ElapsedMilliseconds);

                var avg = Math.Round(results.Average());
                var sum = TimeSpan.FromMilliseconds(results.Sum());
                Debug.Print("Page: " + testPage.Url + ", time: " + sw.ElapsedMilliseconds + ", avg:" + avg + "total: " + sum);
            }

            Debug.Print("Total test time: " + TimeSpan.FromMilliseconds(results.Sum()));

            if (sb.Length > 0)
            {
                // Throwing a SuccessException here will cause the tests to still pass even if they
                // fail. Don't do that. 
                throw new Exception(String.Format("{0} Failures Discovered:\n{1}", errors, sb));
            }
        }

        public abstract void TestTheTestPage(T testPage);
    }

    /// <summary>
    /// Base test class. Inheritors need to add the [TestFixture] attribute!!!!!!!!!!!!!!!!!!!!!!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseTest<T> : BaseTest
    {
        #region Exposed Methods

        /// <summary>
        /// Inherits need to add the [Test] attribute
        /// </summary>
        public abstract void TestCreate();

        /// <summary>
        /// Inherits need to add the [Test] attribute
        /// </summary>
        public virtual void TestSearch()
        {
            
        }

        #endregion

        #region Private Methods

        private void NavigateToHomePage()
        {
            Selenium.SelectFrame(Globals.NecessaryIDs.RELATIVE_UP_FRAME);
            Selenium.Open(Config.GetDevSiteUrl() +  Globals.HOME_URL);
        }

        protected virtual void NavigateToBasePage()
        {
            NavigateToHomePage();
        }

        protected virtual void NavigateToCreatePage()
        {
            NavigateToBasePage();
        }
        
        protected virtual void NavigateToSearchPage()
        {
            NavigateToBasePage();
        }

        protected abstract T Create();
        protected abstract void VerifyWasCreated(T item);


        #endregion
    }

}
