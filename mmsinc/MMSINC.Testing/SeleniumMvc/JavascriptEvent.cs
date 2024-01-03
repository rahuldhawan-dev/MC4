namespace MMSINC.Testing.SeleniumMvc
{
    public enum JavascriptEventType
    {
        // NOTE: Add events to this as needed.
        Change,
    }

    /// <summary>
    /// Class for firing off completely vanilla javascript events in selenium.
    /// </summary>
    public class JavascriptEvent
    {
        #region Fields

        private IBetterWebDriver _driver;

        #endregion

        #region Constructors

        public JavascriptEvent(IBetterWebDriver driver)
        {
            _driver = driver;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Fires an event on a given element.
        /// </summary>
        public virtual void Fire(IBetterWebElement el, JavascriptEventType eventType)
        {
            var eventFireScript = $"arguments[0].dispatchEvent(new Event('{eventType.ToString().ToLower()}'));";
            _driver.ExecuteScript(eventFireScript, el.InternalElement);
        }

        #endregion
    }
}
