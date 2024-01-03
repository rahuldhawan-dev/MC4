using System;
using System.Web.Optimization;
using dotless.Core.Loggers;
using dotless.Core.configuration;
using JetBrains.Annotations;

namespace MMSINC.Bundling
{
    public class DotLessTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            var config = new DotlessConfiguration();
            config.Logger = typeof(DotLessErrorThrowingLogger);

            response.Content = dotless.Core.Less.Parse(response.Content, config);
            response.ContentType = "text/css";
        }

        private class DotLessErrorThrowingLogger : ILogger
        {
            private static void PrintError(string message)
            {
                Console.WriteLine(message);
            }

            public void Log(LogLevel level, string message)
            {
                PrintError("DotLessLogger Log: " + level + " " + message);
            }

            public void Info(string message)
            {
                PrintError("DotLessLogger Info: " + message);
            }

            [StringFormatMethod("message")]
            public void Info(string message, params object[] args)
            {
                PrintError("DotLessLogger Info: " + string.Format(message, args));
            }

            public void Debug(string message)
            {
                PrintError("DotLessLogger Debug: " + message);
            }

            [StringFormatMethod("message")]
            public void Debug(string message, params object[] args)
            {
                PrintError("DotLessLogger Debug: " + string.Format(message, args));
            }

            public void Warn(string message)
            {
                PrintError("DotLessLogger Warn: " + message);
            }

            [StringFormatMethod("message")]
            public void Warn(string message, params object[] args)
            {
                PrintError("DotLessLogger Warn: " + string.Format(message, args));
            }

            public void Error(string message)
            {
                throw new Exception("DotLessTransform blew up. Here's why: " + message);
            }

            [StringFormatMethod("message")]
            public void Error(string message, params object[] args)
            {
                var errorMessage = string.Format(message, args);
                throw new Exception("DotLessTransform blew up. Here's why: " + errorMessage);
            }
        }
    }
}
