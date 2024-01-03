using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Internal;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Common;

namespace MMSINC.CoreTest.Common
{
    [TestClass]
    public class HttpApplicationBaseTest
    {
        #region Private Members

        private IInternalConfigSystem _originalInternalConfigThing;
        private FieldInfo s_configSystem;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            SetupTestConfigSystem();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            // Make sure to set this back to the original field. 
            // Other tests will break if you don't.
            s_configSystem.SetValue(null, _originalInternalConfigThing);
        }

        private void SetupTestConfigSystem()
        {
            // We need to do this to get around the readonlyness of
            // ConfigurationManager. Once you set a value, you can't
            // change it to something else. This should get us around it.
            // You should still use ConfigurationManager as you would
            // expect to.
            s_configSystem =
                typeof(ConfigurationManager).GetField("s_configSystem", BindingFlags.Static | BindingFlags.NonPublic);

            if (s_configSystem == null)
            {
                throw new Exception("Can't find s_configSystem field on ConfigurationManager");
            }

            _originalInternalConfigThing = (IInternalConfigSystem)s_configSystem.GetValue(null);
            s_configSystem.SetValue(null, new TestInternalConfigSystem());

            //s_configSystem
        }

        #endregion

        #region IsProduction

        private static void SetIsProduction(bool val)
        {
            HttpApplicationBase.ResetIsProduction();
            ConfigurationManager.AppSettings[HttpApplicationBase.IS_PRODUCTION_KEY] = val.ToString();
        }

        [TestMethod]
        public void TestIsProductionReturnsTrueIfAppSettingIsProductionIsTrue()
        {
            SetIsProduction(true);
            Assert.IsTrue(HttpApplicationBase.IsProduction);
        }

        [TestMethod]
        public void TestIsProductionReturnsFalseIfAppSettingIsProductionIsFalse()
        {
            SetIsProduction(false);
            Assert.IsFalse(HttpApplicationBase.IsProduction);
        }

        [TestMethod]
        public void TestIsProductionReturnsFalseIfAppSettingIsProductionIsMissing()
        {
            HttpApplicationBase.ResetIsProduction();
            ConfigurationManager.AppSettings.Remove(HttpApplicationBase.IS_PRODUCTION_KEY);
            Assert.IsFalse(HttpApplicationBase.IsProduction);
        }

        #endregion

        #region IsStaging

        private static void SetIsStaging(bool val)
        {
            HttpApplicationBase.ResetIsStaging();
            ConfigurationManager.AppSettings[HttpApplicationBase.IS_STAGING_KEY] = val.ToString();
        }

        [TestMethod]
        public void TestIsStagingReturnsTrueIfAppSettingIsStagingIsTrue()
        {
            SetIsStaging(true);
            Assert.IsTrue(HttpApplicationBase.IsStaging);
        }

        [TestMethod]
        public void TestIsStagingReturnsFalseIfAppSettingIsStagingIsFalse()
        {
            SetIsStaging(false);
            Assert.IsFalse(HttpApplicationBase.IsStaging);
        }

        [TestMethod]
        public void TestIsStagingReturnsFalseIfAppSettingIsStagingIsMissing()
        {
            HttpApplicationBase.ResetIsStaging();
            ConfigurationManager.AppSettings.Remove(HttpApplicationBase.IS_STAGING_KEY);
            Assert.IsFalse(HttpApplicationBase.IsStaging);
        }

        #endregion

        #region IsTraining

        private static void SetIsTraining(bool val)
        {
            HttpApplicationBase.ResetIsTraining();
            ConfigurationManager.AppSettings[HttpApplicationBase.IS_TRAINING_KEY] = val.ToString();
        }

        [TestMethod]
        public void TestIsTrainingReturnsTrueIfAppSettingIsStagingIsTrue()
        {
            SetIsTraining(true);
            Assert.IsTrue(HttpApplicationBase.IsTraining);
        }

        [TestMethod]
        public void TestIsTrainingReturnsFalseIfAppSettingIsStagingIsFalse()
        {
            SetIsTraining(false);
            Assert.IsFalse(HttpApplicationBase.IsTraining);
        }

        [TestMethod]
        public void TestIsTrainingReturnsFalseIfAppSettingIsStagingIsMissing()
        {
            HttpApplicationBase.ResetIsTraining();
            ConfigurationManager.AppSettings.Remove(HttpApplicationBase.IS_TRAINING_KEY);
            Assert.IsFalse(HttpApplicationBase.IsTraining);
        }

        #endregion

        #region Application_Start

        [TestMethod]
        public void TestApplication_StartCallsOnApplicationStart()
        {
            var target = new TestHttpApplication();
            Assert.IsTrue(target.Application_StartCallsOnApplicationStart());
        }

        #endregion

        #region Helper classes

        private class TestInternalConfigSystem : IInternalConfigSystem
        {
            private readonly NameValueCollection _values = new NameValueCollection();

            public object GetSection(string configKey)
            {
                return _values;
            }

            public void RefreshConfig(string sectionName)
            {
                throw new NotImplementedException();
            }

            public bool SupportsUserConfig
            {
                get { throw new NotImplementedException(); }
            }
        }

        private class TestHttpApplication : HttpApplicationBase
        {
            private bool _wasCalled;

            public bool Application_StartCallsOnApplicationStart()
            {
                _wasCalled = false;
                Application_Start();
                return _wasCalled;
            }

            protected override void OnApplication_Start()
            {
                base.OnApplication_Start();
                _wasCalled = true;
            }
        }

        #endregion
    }
}
