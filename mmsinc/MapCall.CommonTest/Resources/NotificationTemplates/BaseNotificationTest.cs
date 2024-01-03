using System;
using System.IO;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorEngine;

namespace MapCall.CommonTest.Resources.NotificationTemplates
{
    public abstract class BaseNotificationTest
    {
        #region Private Methods

        protected static string RenderTemplate(string nameFormat, string name, object model)
        {
            var streamPath = string.Format(nameFormat, name);
            return RenderTemplate(streamPath, model);
        }

        protected static string RenderTemplate(string streamPath, object model)
        {
            using (var stream = typeof(RazorNotifier).Assembly.GetManifestResourceStream(streamPath))
            {
                if (stream == null)
                {
                    Assert.Fail("Could not stream template at location {0}", streamPath);
                }

                using (var reader = new StreamReader(stream))
                {
                    var template = reader.ReadToEnd();
                    return Razor.Parse(template, model);
                }
            }
        }

        #endregion
    }
}
