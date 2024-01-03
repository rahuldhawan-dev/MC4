using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSINC.Testing.SeleniumMvc
{
    /// <summary>
    /// Use this exception for controlling the annoying WebDriver.WaitUntil workflow.
    /// BetterWebDriver.WaitUntil is setup to ignore this exception.
    /// </summary>
    public class GenericSeleniumException : Exception
    {
        #region Constructors

        public GenericSeleniumException(string message) : base(message) { }
        public GenericSeleniumException(string message, Exception inner) : base(message, inner) { }

        #endregion
    }
}
