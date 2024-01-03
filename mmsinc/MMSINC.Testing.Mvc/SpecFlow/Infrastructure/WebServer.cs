using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using AllAboard;
using DeleporterCore.Client;
using MMSINC.Testing.SpecFlow.Library;
using TechTalk.SpecFlow;
using Config = MMSINC.Testing.Utilities.RegressionTestConfiguration;

namespace MMSINC.Testing.SpecFlow.Infrastructure
{
    [Binding]
    public static class WebServer
    {
        #region Constants

        public const string SERVER_KEY = "server";

        //public const string WEB_DEV_SERVER_PATH = @"{0}\microsoft shared\DevServer\10.0\WebDev.WebServer40.EXE";
        public const string WEB_DEV_SERVER_PATH = @"{0}\IIS Express\iisexpress.exe";

        #endregion

        #region Exposed Static Members

        public static string ServerPath
        {
            get
            {
                string tryFirst = String.Format(WEB_DEV_SERVER_PATH,
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.ProgramFilesX86));
                return File.Exists(tryFirst)
                    ? tryFirst
                    : String.Format(WEB_DEV_SERVER_PATH,
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.ProgramFiles));
            }
        }

        public static IIISExpressServer Current { get; private set; }

        public static bool IsInitialized { get; private set; }

        #endregion

        #region Event-Driven Methods

        [BeforeTestRun]
        public static void Open()
        {
            // prevent re-initialization
            if (IsInitialized) return;

            Current = IISExpressServerFactory.ByAppConfig();

            Current.Run();

            //var activeConnection = IPGlobalProperties
            //    .GetIPGlobalProperties()
            //    .GetActiveTcpListeners()
            //    .FirstOrDefault(x => x.Port == Config.GetDevSitePort());

            //// sick of error messages about this.  look if there's any
            //// processes with an active listening socket on our port, and
            //// exterminate.
            //// TODO: this also kills our dev server, which we don't want, but deleporter doesn't like running on both servers at the same time yet anyway
            //if (activeConnection != null)
            //{
            //    foreach (
            //        var process in Process.GetProcesses().Where(x => x.ProcessName.Contains("iisexpress")))
            //    {
            //        process.Kill();
            //    }
            //}

            //Current = new Process {
            //    StartInfo = new ProcessStartInfo(
            //        ServerPath,
            //        string.Format("/port:{0} /path:\"{1}\"", Config.GetDevSitePort(), Config.GetWebProjectPath())) {
            //            CreateNoWindow = true,
            //            UseShellExecute = false
            //        }
            //};
            //Current.Start();
            FailIfServerIsNotRunning();
            IsInitialized = true;
        }

        private static void FailIfServerIsNotRunning()
        {
            try
            {
                // okta needs this one
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // hit up a url which only ever returns an empty 200 OK result to ensure we're not forcing
                // a connection to real services such as SQL or SAP prior to Application.IsInTestMode being
                // set to true
                var uri = Config.GetDevSiteUri("/Home/Blank");

                // this works around a race condition, ensuring that the server is
                // up and running by the time we proceed
                // ReSharper disable AccessToStaticMemberViaDerivedType
                HttpWebRequest.Create(uri).GetResponse();
                // ReSharper restore AccessToStaticMemberViaDerivedType
            }
            catch (Exception ex)
            {
                // If GetResponse throws an exception, it leaves the NUnit testing thing
                // hanging and doesn't actually "complete", so we have to force the web
                // server to shut down manually. 

                try
                {
                    Close();
                }
                catch (Exception)
                {
                    // Temporarily eating this exception to see what the web exception is.                       
                }

                if (!(ex is WebException webEx) || webEx.Response == null)
                {
                    throw;
                }

                using (var rep = webEx.Response)
                {
                    using (var stream = rep.GetResponseStream())
                    using (var sr = new StreamReader(stream))
                    {
                        var msg = sr.ReadToEnd();
                        throw new Exception(webEx.Message + "\n\n" + msg, webEx);
                    }
                }
            }
        }

        [AfterTestRun]
        public static void Close()
        {
            Current.Stop();
            IsInitialized = false;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
