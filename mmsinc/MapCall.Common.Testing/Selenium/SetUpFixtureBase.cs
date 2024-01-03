using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using AllAboard;
using MapCall.Common.Testing.Selenium.TestParts;
using NUnit.Framework;
using Config = MMSINC.Testing.Utilities.RegressionTestConfiguration;

namespace MapCall.Common.Testing.Selenium
{
    public class SetUpFixtureBase
    {
        #region Constants

        public const string DEFAULT_HOST = "localhost";
        public const int DEFAULT_PORT = 4444;
        public const string DEFAULT_TARGET = "*chrome";

        #endregion

        #region Static Properties

        public static string UserName { get; protected set; }
        public static IExtendedSelenium Selenium { get; protected set; }

        #endregion

        #region Properties

        public IIISExpressServer Current { get; protected set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Override as necessary in inheriting classes to specify the url that should be loaded to test if the webserver is running at all.
        /// </summary>
        protected virtual Uri GetFirstPageUri()
        {
            var devSiteRootUri = Config.GetDevSiteUrl() + Login.LOGIN_URL;

            return new Uri(devSiteRootUri);
        }

        private void FailIfServerIsNotRunning()
        {
            Uri firstPageUri;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                // this works around a race condition, ensuring that the server is
                // up and running by the time we proceed
                // ReSharper disable AccessToStaticMemberViaDerivedType
                HttpWebRequest.Create(firstPageUri = GetFirstPageUri()).GetResponse();
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

                if (ex is WebException)
                {
                    var webEx = ex as WebException;
                    using (var rep = webEx.Response)
                    {
                        if (rep == null)
                        {
                            // Just rethrow the exception in this case. 
                            // And yes this happened to me one whole time. -Ross 5/2/2013
                            throw;
                        }

                        using (var stream = rep.GetResponseStream())
                        using (var sr = new StreamReader(stream))
                        {
                            var msg = sr.ReadToEnd();
                            throw new Exception(ex.Message + "\n\n" + msg, ex);
                        }
                    }
                }

                throw;
            }
        }

        private void Close()
        {
            Current.Stop();
        }

        #endregion

        [SetUp]
        public virtual void NamespaceSetUp()
        {
            Selenium = new SeleniumWrapper(DEFAULT_HOST, DEFAULT_PORT,
                DEFAULT_TARGET, Config.GetDevSiteUrl());
            Selenium.Start();
            //Lets make the window tiny, so esri doesn't have to work so hard.
            Selenium.GetEval("window.resizeTo(500,500);");

            var activeConnection = IPGlobalProperties
                                  .GetIPGlobalProperties()
                                  .GetActiveTcpListeners()
                                  .FirstOrDefault(x => x.Port == Config.GetDevSitePort());

            // sick of error messages about this.  look if there's any
            // processes with an active listening socket on our port, and
            // exterminate.
            // TODO: this also kills our dev server, which we don't want, but deleporter doesn't like running on both servers at the same time yet anyway
            if (activeConnection != null)
            {
                foreach (
                    var process in Process.GetProcesses().Where(x => x.ProcessName.ToLower().Contains("iisexpress")))
                {
                    process.Kill();
                }
            }

            Current = IISExpressServerFactory.ByAppConfig(false, true);
            Current.Run();
            FailIfServerIsNotRunning();
            UserName = Login.AsAdmin(Selenium);
        }

        [TearDown]
        public virtual void NamespaceTeardown()
        {
            try
            {
                Close();
                Selenium.Stop();
            }
            // ignore errors if unable to close the browser
            catch { }
        }
    }
}
