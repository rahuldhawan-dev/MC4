using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace MMSINC.Testing.Utilities
{
    public static class RegressionTestConfiguration
    {
        #region Constants

        public struct ConfigKeys
        {
            #region Constants

            public const string ROOT_URL = "RootUrl",
                                WEB_PROJECT_PATH = "WebProjectPath";

            #endregion
        }

        #endregion

        #region Private Methods

        private static string EnsureConfigValue(string key)
        {
            var path = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrEmpty(path))
            {
                throw new KeyNotFoundException(
                    $"'{key}' is not set in the RegressionTests assembly's App.config '{AppDomain.CurrentDomain.SetupInformation.ConfigurationFile}'.");
            }

            return path;
        }

        #endregion

        #region Exposed Methods

        public static string GetDevSiteUrl()
        {
            return EnsureConfigValue(ConfigKeys.ROOT_URL);
        }

        public static string GetDevSiteHostName()
        {
            return GetDevSiteUri().Host;
        }

        public static Uri GetDevSiteUri(string path)
        {
            return new Uri(GetDevSiteUri(), path);
        }

        public static Uri GetDevSiteUri()
        {
            return new Uri(GetDevSiteUrl());
        }

        public static int GetDevSitePort()
        {
            return GetDevSiteUri().Port;
        }

        /// <summary>
        /// Gets the full path to the root of the web project from the current regression tests project.
        /// </summary>
        /// <returns></returns>
        public static string GetWebProjectPath()
        {
            var path = EnsureConfigValue(ConfigKeys.WEB_PROJECT_PATH);

            path = Path.IsPathRooted(path)
                ? path
                : Path.GetFullPath(path).ToLower().Replace("\\regressiontests\\bin\\debug", "");

            return path;
        }

        #endregion
    }
}
