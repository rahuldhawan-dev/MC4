using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace MMSINC.Testing.SeleniumMvc
{
    public interface IBetterWebDriver : IFindElements, IDisposable
    {
        #region Properties

        Uri CurrentUri { get; }

        [Obsolete("This is here to make it easier to test out how to wrap things up.")]
        IWebDriver InternalDriver { get; }

        string PageSource { get; }
        string Text { get; }

        #endregion

        #region Methods

        void ConfirmDialog();
        Actions CreateInteraction();
        void DismissDialog();
        object ExecuteScript(string script, params object[] args);
        string GetAlertText();
        void GoBack();
        void LeaveFrame();
        void GoToUrl(Uri uri);
        void Refresh();
        void SwitchToFrame(string frame);
        void SwitchToFirstTab();
        void SwitchToLastTab();
        void SwitchToTab(int index);
        void CloseLastTab();

        void CloseFirstTab();
        void WaitForAnimationsToComplete();
        void WaitForAjaxToComplete();
        TResult WaitUntilNoExceptionThrown<TResult>(Func<TResult> callback, int timeoutInSeconds = 30);
        void WaitUntilNoExceptionThrown(Action callback, int timeoutInSeconds = 30);
        void WaitUntil(Func<bool> callback, int timeoutInSections = 30);

        IBetterWebElement WaitUntilSingleElementExists(By constraint,
            int timeoutInSeconds = 30);

        IBetterWebElement WaitUntilSingleElementExistsByIdOrName(string element, int timeoutInSeconds);
        void CaptureScreenshot();
        bool SingleElementExists(By constraint);

        void PerformTerribleHackToClearUpChromeMemoryLeak();

        int GetNumberOfWindowHandles();
        
        #endregion
    }

    public class BetterWebDriver : IBetterWebDriver
    {
        #region Fields

        private readonly IWebDriver _driver;
        private readonly IJavaScriptExecutor _jsDriver;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the current url.
        /// </summary>
        public Uri CurrentUri => new Uri(_driver.Url);

        public IWebDriver InternalDriver => _driver;

        /// <summary>
        /// Returns the page's html source.
        /// </summary>
        public string PageSource => _driver.PageSource;

        /// <summary>
        /// Returns the current page's text content only.
        /// </summary>
        public string Text => FindElement(By.TagName("html")).Text;

        #endregion

        #region Constructor

        public BetterWebDriver(IWebDriver actualDriver)
        {
            _driver = actualDriver;
            _jsDriver = (IJavaScriptExecutor)_driver;
        }

        #endregion

        #region Public Methods

        public void PerformTerribleHackToClearUpChromeMemoryLeak()
        {
            if (WebDriverHelper.INeedToSeeTheBrowser)
            {
                return;
            }
            // This is a gross GROSS **GROSS** hack to deal with
            // Chrome/ChromeDriver having a memory leak as of v74. Eventually the browser
            // ends up using too much ram and crashes.

            // Using this method *WILL* cause the browser to come to the front
            // every time a new window/tab is created.

            // First get all currently open windows
            var allAvailableWindowHandles = _driver.WindowHandles.ToList();
            // Them open a new window
            ExecuteScript("window.open()");
            // Compare the previous list of windows to the new list to find the handle of our new window.
            // window.open(), while it does make it the active tab *in Chrome*, it does not make it
            // the active window that selenium is following.
            var newWindow = _driver.WindowHandles.Except(allAvailableWindowHandles).Single();

            // Close the previous window as that's the one selenium is currently following.
            _driver.Close();

            // Switch to our new window so selenium knows which one to look at.
            _driver.SwitchTo().Window(newWindow);
        }

        public int GetNumberOfWindowHandles()
        {
            return _driver.WindowHandles.Count;
        }

        /// <summary>
        /// Selenium is weird, but this can either be used to hit okay on a confirm dialog, or to acknowledge an alert dialog.
        /// </summary>
        public void ConfirmDialog()
        {
            _driver.SwitchTo().Alert().Accept();
        }

        /// <summary>
        /// Returns an Actions object for the internal WebDriver instance. This is useful for doing
        /// tricky mouse things like dragging and dropping in the permits designer.
        /// </summary>
        /// <returns></returns>
        public Actions CreateInteraction()
        {
            return new Actions(_driver);
        }

        /// <summary>
        /// Selenium is weird, but this can either be used to hit cancel on a confirm dialog, or to acknowledge an alert dialog.
        /// </summary>
        public void DismissDialog()
        {
            _driver.SwitchTo().Alert().Dismiss();
        }

        /// <summary>
        /// Executes javascript with the given arguments and then returns the value.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object ExecuteScript(string script, params object[] args)
        {
            // Hopefully this passes the params around correctly
            //Console.WriteLine();
            //Console.WriteLine(script);
            //Console.WriteLine();
            return _jsDriver.ExecuteScript(script, args);
        }

        /// <summary>
        /// Disposes the current driver/browser instance. You absolutely want to use this to clean up 
        /// the temp files created by the browser. ex: Chrome profiles are created, they need to be killed.
        /// </summary>
        public void Dispose()
        {
            // Always make sure to dispose the driver as it will otherwise leave temp files hanging around everytime it runs.
            _driver.Quit();
        }

        /// <summary>
        /// Returns elements that match the given contraint.
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        public IEnumerable<IBetterWebElement> FindElements(By constraint)
        {
            // NOTE: If you change anything in this method, you may very well need to change it in BetterWebElement as well.
            foreach (var el in _driver.FindElements(constraint))
            {
                yield return new BetterWebElement(el, this);
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
            // NOTE: If you change anything in this method, you may very well need to change it in BetterWebElement as well.
            var result = _driver.FindElement(constraint);
            return new BetterWebElement(result, this);
        }

        /// <summary>
        /// Returns the text of the current alert dialog.
        /// </summary>
        /// <returns></returns>
        public string GetAlertText()
        {
            // Might want something like this if waiting is involved http://stackoverflow.com/a/12639803/152168
            return _driver.SwitchTo().Alert().Text;
        }

        /// <summary>
        /// Navigates the browser backwards one step in the browser history.
        /// </summary>
        public void GoBack()
        {
            _driver.Navigate().Back();
        }

        /// <summary>
        /// Navigates the current browser window(and possibly frame if you're inside one) to 
        /// the given url.
        /// </summary>
        /// <param name="uri"></param>
        public void GoToUrl(Uri uri)
        {
            _driver.Navigate().GoToUrl(uri);
        }

        /// <summary>
        /// Refreshes the current page.
        /// </summary>
        public void Refresh()
        {
            _driver.Navigate().Refresh();
        }

        /// <summary>
        /// Changes the internal driver back to the main browser window. Or if you're dealing
        /// with multiple frames inside of frames(frameception) then this will bring you to the
        /// parent frame. We don't actually do this anywhere.
        /// </summary>
        public void LeaveFrame()
        {
            _driver.SwitchTo().ParentFrame();
        }

        /// <summary>
        /// Changes the internal driver to use the expected iframe.
        /// </summary>
        /// <param name="frame"></param>
        public void SwitchToFrame(string frame)
        {
            _driver.SwitchTo().Frame(frame);
        }

        /// <summary>
        /// Instructs the internal driver to switch to the first tab.
        /// </summary>
        public void SwitchToFirstTab()
        {
            _driver.SwitchTo().Window(_driver.WindowHandles.First());
        }

        /// <summary>
        /// Instructs the internal driver to switch to the last tab.
        /// </summary>
        public void SwitchToLastTab()
        {
            _driver.SwitchTo().Window(_driver.WindowHandles.Last());
        }

        public void SwitchToTab(int index)
        {
            _driver.SwitchTo().Window(_driver.WindowHandles[index]);
        }

        /// <summary>
        /// Closes the current tab/window
        /// </summary>
        public void CloseLastTab()
        {
            _driver.SwitchTo().Window(_driver.WindowHandles.Last()).Close();
        }

        /// <summary>
        /// Closes the first tab/window
        /// </summary>
        public void CloseFirstTab()
        {
            _driver.SwitchTo().Window(_driver.WindowHandles.First()).Close();
        }

        /// <summary>
        /// Waits for all jQuery animations to complete before resuming.
        /// </summary>
        public void WaitForAnimationsToComplete()
        {
            // we used to wait until no running animations could be detected, but it makes more sense to
            // just stop them now and put all animated elements in the state they'd be after the animations ran
            // https://api.jquery.com/stop/
            // NOTE: Don't return the result, you will receive a "Object reference chain is too long" error as of Chrome v77.
            ExecuteScript("jQuery('*:animated').stop(false, true)");
        }

        /// <summary>
        /// Waits for all ajax requests to complete before resuming.
        /// </summary>
        public void WaitForAjaxToComplete()
        {
            // SUPER NOTE: Are you getting a timeout that is seemingly random? Make sure any previous steps that trigger
            //             an ajax call have been completed successfully. If your callback js code throws an error(ie accessing undefined arrays),
            //             then jQuery will not update the jQuery.active property and this will timeout.

            WaitUntil(() => ((long)ExecuteScript("return jQuery.active")) == 0);
        }

        /// <summary>
        /// This will attempt to get the expected result for the given amount of time.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="callback"></param>
        /// <param name="timeoutInSeconds">Defaults to 30 seconds</param>
        /// <returns></returns>
        public TResult WaitUntilNoExceptionThrown<TResult>(Func<TResult> callback, int timeoutInSeconds = 30)
        {
            // This is currently gross. The WebDriverWait class is exception driven, so any method
            // that is passed in needs to throw exceptions if it's failing. Throw GenericSeleniumException
            // for this.
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));

            // The default for this is 500ms which seems a bit long. It's worth noting that
            // there is no waiting period before the first time the callback runs. The interval
            // only starts after the first time the callback fails. 
            wait.PollingInterval = TimeSpan.FromMilliseconds(100);

            // Do not just add random exceptions to this, like InvalidOperationException should not be in here.
            wait.IgnoreExceptionTypes(typeof(GenericSeleniumException));
            wait.IgnoreExceptionTypes(typeof(NotFoundException));
            return wait.Until(x => callback());
        }

        /// <summary>
        /// This will wait a given amount of time for something to occur, but does not 
        /// return a value.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="timeoutInSeconds"></param>
        public void WaitUntilNoExceptionThrown(Action callback, int timeoutInSeconds = 30)
        {
            WaitUntilNoExceptionThrown(() => {
                callback();
                return true;
            }, timeoutInSeconds);
        }

        public void WaitUntil(Func<bool> callback, int timeoutInSeconds = 30)
        {
            var wait = new WebDriverWait(_driver,
                TimeSpan.FromSeconds(timeoutInSeconds)) {
                PollingInterval = TimeSpan.FromMilliseconds(100)
            };

            wait.Until(_ => callback());
        }

        public IBetterWebElement WaitUntilSingleElementExists(By constraint, int timeoutInSeconds = 30)
        {
            IBetterWebElement ret = null;

            WaitUntil(() => {
                var elements = FindElements(constraint);

                switch (elements.Count())
                {
                    case 0:
                        return false;
                    case 1:
                        ret = elements.Single();
                        return true;
                    default:
                        throw new GenericSeleniumException(
                            $"Found {elements.Count()} elements matching constraint '{constraint}'.");
                }
            }, timeoutInSeconds);

            return ret;
        }

        public bool SingleElementExists(By constraint)
        {
            return FindElements(constraint).Count() == 1;
        }

        public IBetterWebElement WaitUntilSingleElementExistsByIdOrName(string element, int timeoutInSeconds = 30)
        {
            return WaitUntilSingleElementExists(ByHelper.Any(By.Id(element),
                By.Name(element)));
        }

        public void CaptureScreenshot()
        {
            var screenshot = _driver.TakeScreenshot();
            var dirPath = Path.GetFullPath("..\\..\\..\\screenshots");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var filePath = Path.Combine(dirPath, $"screenshot-{DateTime.Now:yyyy-MM-dd HHmmss}.png");
            Console.WriteLine($"Saving screenshot as {filePath}...");
            screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
        }

        #endregion
    }
}
