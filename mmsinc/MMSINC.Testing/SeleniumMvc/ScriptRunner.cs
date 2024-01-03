using System;

namespace MMSINC.Testing.SeleniumMvc
{
    /// <summary>
    /// Simple class meant to make it a little nicer to run random scripts.
    /// This came from the WatiN testing, I don't know if it's still useful for seleniums really.
    /// </summary>
    public class ScriptRunner
    {
        #region Consts

        /// <summary>
        /// Script format for creating a self-executing function and returning its result. Also, it looks like a weird fish.
        /// </summary>
        public const string SELF_EXECUTING_SCRIPT_FORMAT = "(function() {{ {0} }})();",
                            NO_WAIT_FORMAT = "var callback = function() {{ {0} }}; setTimeout(callback, 10);";

        #endregion

        #region Properties

        /// <summary>
        /// The javascript to be executed.
        /// </summary>
        public string Script { get; set; }

        #endregion

        #region Constructors

        public ScriptRunner() : this(null) { }

        public ScriptRunner(string script)
        {
            Script = script;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Override if the Script property requires additional processing before being evaluated.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetScript()
        {
            return Script;
        }

        private string GetAndEnsureScript()
        {
            var s = GetScript();
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new InvalidOperationException("Script can't be null or empty");
            }

            return s;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Runs an arbitrary script and returns its result.
        /// </summary>
        /// <returns></returns>
        public static string Run(string script)
        {
            var runner = new ScriptRunner(script);
            return runner.Run();
        }

        /// <summary>
        /// Runs an arbitrary script but doesn't wait for it to return.
        /// </summary>
        /// <param name="script"></param>
        public static void RunNoWait(string script)
        {
            var runner = new ScriptRunner(script);
            runner.RunNoWait();
        }

        /// <summary>
        /// Runs whatever script is currently set on the Script property.
        /// </summary>
        /// <returns></returns>
        public virtual string Run()
        {
            var s = GetAndEnsureScript();
            var result = WebDriverHelper.Current.ExecuteScript(s);
            try
            {
                // I dunno what should be done with nulls here at the moment.
                return result.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SCRIPT PROBLEM:");
                Console.WriteLine(s);
                Console.WriteLine();

                throw;
            }
        }

        /// <summary>
        /// Runs whatever script is currently set on the Script property but doesn't wait for it to return.
        /// Useful for running scripts that trigger confirm/alert dialogs.
        /// </summary>
        public virtual void RunNoWait()
        {
            var s = string.Format(NO_WAIT_FORMAT, GetAndEnsureScript());
            WebDriverHelper.Current.ExecuteScript(s);
        }

        #endregion
    }
}
