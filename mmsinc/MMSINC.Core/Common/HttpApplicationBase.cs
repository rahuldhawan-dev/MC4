using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MMSINC.Common
{
    /// <summary>
    /// All of our site's HttpApplications shouhld inherit from this in some way.
    /// </summary>
    public abstract class HttpApplicationBase : HttpApplication
    {
        #region Constants

        internal const string IS_PRODUCTION_KEY = "IsProduction";
        internal const string IS_STAGING_KEY = "IsStaging";
        internal const string IS_TRAINING_KEY = "IsTraining";

        #endregion

        #region Fields

        private static bool? _isProduction, _isStaging, _isTraining;

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether this code is running in production mode or not. 
        /// 
        /// You must set IsProduction = "true" in the web.config's AppSettings
        /// for this to be true.
        /// </summary>
        public static bool IsProduction
        {
            get
            {
                if (!_isProduction.HasValue)
                {
                    bool boo;
                    Boolean.TryParse(ConfigurationManager.AppSettings[IS_PRODUCTION_KEY], out boo);
                    _isProduction = boo;
                }

                return _isProduction.Value;
            }
            internal set { _isProduction = value; }
        }

        public static bool IsStaging
        {
            get
            {
                if (!_isStaging.HasValue)
                {
                    bool boo;
                    Boolean.TryParse(ConfigurationManager.AppSettings[IS_STAGING_KEY], out boo);
                    _isStaging = boo;
                }

                return _isStaging.Value;
            }
            internal set { _isStaging = value; }
        }

        public static bool IsTraining
        {
            get
            {
                if (!_isTraining.HasValue)
                {
                    bool boo;
                    Boolean.TryParse(ConfigurationManager.AppSettings[IS_TRAINING_KEY], out boo);
                    _isTraining = boo;
                }

                return _isTraining.Value;
            }
            internal set { _isTraining = value; }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// DO NOT CALL THIS METHOD. This method exists solely for the ASP.NET runtime
        /// when the application is first started up. Use OnApplicationStart for any
        /// overrides or additional functionality.
        /// </summary>
        protected void Application_Start()
        {
            OnApplication_Start();
        }

        /// <summary>
        /// This method is called ONE TIME. Any instances created after this method
        /// has been fired by the ASP.NET runtime will not have the method called.
        /// Use the Init method if you need that sort of thing.
        /// </summary>
        protected virtual void OnApplication_Start()
        {
            // noop, for inheritors.
        }

        /// <summary>
        /// DO NOT CALL THIS METHOD. Use OnBeginRequest instead. This has to
        /// exist for the ASP.NET runtime to bind to something.
        /// </summary>
        protected void Application_BeginRequest()
        {
            OnBeginRequest();
        }

        protected virtual void OnBeginRequest()
        {
            // This is a massive massive hack to ensure that we can access the forms authentication
            // cookie sent in the request when the FormsAuthenticationModule decides "Hey, I'm just gonna
            // remove the whole value from the request because it expired. No one would ever want to access
            // that when it's expired!".

            var cookieName = FormsAuthentication.FormsCookieName;
            if (Request.Cookies.AllKeys.Contains(cookieName))
            {
                // ReSharper disable once PossibleNullReferenceException
                // It's not a null reference, shut up Resharper.
                HttpContext.Current.Items.Add(cookieName, Request.Cookies[cookieName].Value);
            }
        }

        /// <summary>
        /// Nulls ths IsProduction value so it's re-read from app settings again. This is only here
        /// for testing purposes.
        /// </summary>
        internal static void ResetIsProduction()
        {
            _isProduction = null;
        }

        internal static void ResetIsStaging()
        {
            _isStaging = null;
        }

        internal static void ResetIsTraining()
        {
            _isTraining = null;
        }

        #endregion
    }
}
