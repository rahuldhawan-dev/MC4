using System.Web.Mvc;
using MMSINC.Metadata;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable Mvc.ActionNotResolved, Mvc.ControllerNotResolved, Mvc.AreaNotResolved

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class AutoCompleteAttributeTest
    {
        #region Fields

        private AutoCompleteAttribute _target;
        private ModelMetadataProvider _originalProvider;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _originalProvider = ModelMetadataProviders.Current;
            ModelMetadataProviders.Current = new CustomModelMetadataProvider();
            _target = new AutoCompleteAttribute("controller", "action");
        }

        [TestCleanup]
        public void CleanupTest()
        {
            ModelMetadataProviders.Current = _originalProvider;
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestConstructorSetsControllerName()
        {
            var expected = "ControllerDoodad";
            var result = new AutoCompleteAttribute(expected, "action").Controller;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConstructorSetsActionName()
        {
            var expected = "ActionDoodad";
            var result = new AutoCompleteAttribute("controller", expected).Action;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConstructorSetsAreaName()
        {
            var expected = "AreaName";
            var result = new AutoCompleteAttribute(expected, "controller", "action").Area;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestConstructorThrowsForNullControllerParameter()
        {
            MyAssert.Throws(() => new AutoCompleteAttribute(null, "action"));
            MyAssert.Throws(() => new AutoCompleteAttribute("area", null, "action"));
            MyAssert.Throws(() => new AutoCompleteAttribute(string.Empty, "action"));
            MyAssert.Throws(() => new AutoCompleteAttribute("area", string.Empty, "action"));
            MyAssert.Throws(() => new AutoCompleteAttribute("   ", "action"));
            MyAssert.Throws(() => new AutoCompleteAttribute("area", "   ", "action"));
        }

        [TestMethod]
        public void TestConstructorThrowsForNullActionParameter()
        {
            MyAssert.Throws(() => new AutoCompleteAttribute("controller", null));
            MyAssert.Throws(() => new AutoCompleteAttribute("area", "controller", null));
            MyAssert.Throws(() => new AutoCompleteAttribute("controller", string.Empty));
            MyAssert.Throws(() => new AutoCompleteAttribute("area", "controller", string.Empty));
            MyAssert.Throws(() => new AutoCompleteAttribute("controller", "   "));
            MyAssert.Throws(() => new AutoCompleteAttribute("area", "controller", "   "));
        }

        [TestMethod]
        public void TestConstructorConvertsNullOrWhiteSpaceAreaToEmptyString()
        {
            Assert.AreEqual(string.Empty, new AutoCompleteAttribute(null, "controller", "action").Area);
            Assert.AreEqual(string.Empty, new AutoCompleteAttribute(string.Empty, "controller", "action").Area);
            Assert.AreEqual(string.Empty, new AutoCompleteAttribute("   ", "controller", "action").Area);
        }

        #endregion

        #region Properties

        [TestMethod]
        public void TestHttpMethodGetsAndSets()
        {
            _target.HttpMethod = "Methody";
            Assert.AreEqual("Methody", _target.HttpMethod);
        }

        [TestMethod]
        public void TestHttpMethodReturnsGetIfNullOrEmptyOrWhiteSpace()
        {
            _target.HttpMethod = null;
            Assert.AreEqual("GET", _target.HttpMethod);
            _target.HttpMethod = string.Empty;
            Assert.AreEqual("GET", _target.HttpMethod);
            _target.HttpMethod = "   ";
            Assert.AreEqual("GET", _target.HttpMethod);

            _target.HttpMethod = "POST";
            Assert.AreEqual("POST", _target.HttpMethod);
        }

        [TestMethod]
        public void TestDependsOnGetsAndSets()
        {
            _target.DependsOn = "Why So Serious?";
            Assert.AreEqual("Why So Serious?", _target.DependsOn);
        }

        [TestMethod]
        public void TestPlaceHolderGetsAndSets()
        {
            _target.PlaceHolder = "We have place holders now";
            Assert.AreEqual("We have place holders now", _target.PlaceHolder);
        }

        #endregion

        #region GetAttributeForModel

        [TestMethod]
        public void TestGetAttributeForModelReturnsSameInstanceAddedDuringProcess()
        {
            var md = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(ViewModel),
                "AutoCorrectProperty");
            _target.Process(md);
            Assert.AreSame(_target, AutoCompleteAttribute.GetAttributeForModel(md));
        }

        [TestMethod]
        public void TestGetAttributeForModelReturnsNullIfNoAttributeIsFound()
        {
            var md = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(ViewModel),
                "NoAutoCorrectProperty");
            Assert.IsNull(AutoCompleteAttribute.GetAttributeForModel(md));
        }

        #endregion

        #endregion

        #region Helper classes

        private class ViewModel
        {
            public object NoAutoCorrectProperty { get; set; }
            public object AutoCorrectProperty { get; set; }
        }

        #endregion
    }
}
