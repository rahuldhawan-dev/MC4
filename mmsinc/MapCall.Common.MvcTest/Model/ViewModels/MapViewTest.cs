using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.Common.MvcTest.Model.ViewModels
{
    [TestClass]
    public class MapViewTest
    {
        #region Fields

        private MapView _target;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new MapView();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestEmptyConstructorSetsSearchPropertyToANewEmptyDictionary()
        {
            var result = new MapView();
            Assert.IsNotNull(result.Search);
            Assert.IsFalse(result.Search.Any());
        }

        [TestMethod]
        public void TestActionReturnsIndexIfActionNameIsNullOrStringEmpty()
        {
            _target.ActionName = null;
            Assert.AreEqual("Index", _target.ActionName);

            _target.ActionName = string.Empty;
            Assert.AreEqual("Index", _target.ActionName);
        }

        #region ToRouteValueDictionary

        [TestMethod]
        public void TestToRouteValueDictionarySetsAreaNameIfThePropertyIsSet()
        {
            _target.AreaName = "some area";
            Assert.AreEqual("some area", _target.ToRouteValueDictionary()["AreaName"]);
        }

        [TestMethod]
        public void TestToRouteValueDictionaryDoesNotAddAreaNameKeyIfAreaNameIsNullOrEmptyOrWhiteSpace()
        {
            _target.AreaName = null;
            Assert.IsFalse(_target.ToRouteValueDictionary().ContainsKey("AreaName"));
            _target.AreaName = string.Empty;
            Assert.IsFalse(_target.ToRouteValueDictionary().ContainsKey("AreaName"));
            _target.AreaName = "    ";
            Assert.IsFalse(_target.ToRouteValueDictionary().ContainsKey("AreaName"));
        }

        [TestMethod]
        public void TestToRouteValueDictionaryAddsActionNameKeyIfThePropertyIsSetAndDoesNotEqualTheDefaultValue()
        {
            _target.ActionName = "SomeAction";
            Assert.AreEqual("SomeAction", _target.ToRouteValueDictionary()["ActionName"]);

            _target.ActionName = "Index";
            Assert.IsFalse(_target.ToRouteValueDictionary().ContainsKey("ActionName"));
        }

        [TestMethod]
        public void TestToRouteValueDictionarySetsControllerNameIfThePropertyIsSet()
        {
            _target.ControllerName = "some controller";
            Assert.AreEqual("some controller", _target.ToRouteValueDictionary()["ControllerName"]);
        }

        [TestMethod]
        public void TestToRouteValueDictionaryDoesNotAddControllerNameKeyIfControllerNameIsNullOrEmptyOrWhiteSpace()
        {
            _target.ControllerName = null;
            Assert.IsFalse(_target.ToRouteValueDictionary().ContainsKey("ControllerName"));
            _target.ControllerName = string.Empty;
            Assert.IsFalse(_target.ToRouteValueDictionary().ContainsKey("ControllerName"));
            _target.ControllerName = "    ";
            Assert.IsFalse(_target.ToRouteValueDictionary().ContainsKey("ControllerName"));
        }

        [TestMethod]
        public void TestToRouteValueDictionaryDoesNotAddAnySearchKeysIfSearchIsNullOrEmpty()
        {
            _target.Search = null;
            var result = _target.ToRouteValueDictionary().Keys
                                .Where(x => x.StartsWith("Search", StringComparison.InvariantCultureIgnoreCase));
            Assert.IsFalse(result.Any());

            _target.Search = new Dictionary<string, string>();
            result = _target.ToRouteValueDictionary().Keys
                            .Where(x => x.StartsWith("Search", StringComparison.InvariantCultureIgnoreCase));
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void TestToRouteValueDictionaryCopiesAllKeysInSearchAndAppendsTheSearchPrefixForEachKey()
        {
            _target.Search = new Dictionary<string, string>();
            _target.Search.Add("SomeKey", "SomeValue");
            var result = _target.ToRouteValueDictionary();
            Assert.IsTrue(result.ContainsKey("Search[SomeKey]"));
            Assert.AreEqual(result["Search[SomeKey]"], "SomeValue");
        }

        #endregion

        #endregion
    }
}
