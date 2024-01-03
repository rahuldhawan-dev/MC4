using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace MMSINC.Testing.SeleniumMvc
{
    // IMPORTANT: If you're wondering why these bindings aren't running, make sure the app.config of the 
    //            RegressionTests project includes this assembly in the specflow/stepAssemblies section.

    /// <summary>
    /// Wrapper class around the Selenium WebDriver
    /// </summary>
    [Binding]
    public class WebDriverHelper
    {
        #region Fields

        public static bool INeedToSeeTheBrowser;
        private static IBetterWebDriver _current;

        #endregion

        #region Properties

        public static IBetterWebDriver Current
        {
            get { return _current ?? (_current = CreateWebDriver()); }
        }

        #endregion

        #region Private Methods

        private static IBetterWebDriver CreateWebDriver()
        {
            var options = new ChromeOptions();

            // This stops Chrome from constantly asking if you want to save your login info.
            options.AddUserProfilePreference("credentials_enable_service", false);

            // Headless mode doesn't offer us any performance advantages, but it does
            // keep the window out of your face. If you need to debug a test, you should
            // probably disable this. This is currently only enabled due to the Chrome memory
            // leak hack(PerformTerribleHackToClearUpChromeMemoryLeak) being enabled in Data.
            if (!INeedToSeeTheBrowser)
            {
                options.AddArgument("--headless");
                // By default, headless more runs an 800x600 window. This can sometimes cause
                // elements to give an "element not interactable" error if an element is scrolled
                // off the screen because we don't design for that size. We don't really design 
                // for any minimum size in mind, though. 
                options.AddArgument("--window-size=1920,1200");
            }

            // This tells Chrome not to display images. It makes no difference on performance.
            // options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            
            new DriverManager().SetUpDriver(new ChromeConfig());
            return new BetterWebDriver(new ChromeDriver(options));
        }

        #endregion

        #region Test Event Driven Methods

        // TODO: RE-ENABLE THIS WHEN CONVERSION IS FINISHED
        [AfterTestRun]
        public static void Close()
        {
            if (!INeedToSeeTheBrowser)
            {
                // not sure why we only dispose when we're headless, maybe so that the browser sticks
                // around after a failure?  it feels like this is going to change again soon.
                Current.Dispose();
            }

            _current = null;
        }

        [BeforeScenario("@headful")]
        public static void PreHeadless()
        {
            Close();
            INeedToSeeTheBrowser = true;
        }

        [AfterScenario("@headful")]
        public static void PostHeadless()
        {
            INeedToSeeTheBrowser = false;
            Close();
        }

        #endregion
    }
}
