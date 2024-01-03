using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest
{
    [TestClass]
    public class ResourceRegistryDictionaryTest
    {
        #region Fields

        private ResourceRegistryDictionary _target;

        #endregion

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new ResourceRegistryDictionary();
        }

        [TestMethod]
        public void TestRegistryKeysAreCaseInsensitive()
        {
            Assert.AreSame(StringComparer.InvariantCultureIgnoreCase, _target.Comparer);
        }

        [TestMethod]
        public void TestToMvcHtmlStringReturnsCombinedResultOfAllValuesInTheOrderTheyWereAdded()
        {
            _target.Add("A", new MvcHtmlString("A value"));
            _target.Add("B", new MvcHtmlString("B value"));
            _target.Add("C", new MvcHtmlString("C value"));

            // There's no line breaks, so this example looks weird.
            Assert.AreEqual("A valueB valueC value", _target.ToMvcHtmlString().ToString());
        }

        [TestMethod]
        public void TestToMvcHtmlStringDoesntUseTheHtmlEncodedStringToCreateItsResult()
        {
            var expected = @"<html></html><stuff attr=""value""></stuff>";
            _target.Add("A", new MvcHtmlString(@"<html></html>"));
            _target.Add("B", new MvcHtmlString(@"<stuff attr=""value""></stuff>"));
            Assert.AreEqual(expected, _target.ToMvcHtmlString().ToString());
        }

        [TestMethod]
        public void TestToMvcHtmlStringReturnsEmptyIfThereAreNoEntries()
        {
            Assert.IsFalse(_target.Any());
            Assert.AreSame(MvcHtmlString.Empty, _target.ToMvcHtmlString());
        }

        [TestMethod]
        public void TestToMvcHtmlStringReturnsSameMvcStringInstanceIfTheresOnlyOneEntry()
        {
            var expected = new MvcHtmlString("hey HEY!");
            _target.Add("A", expected);
            Assert.IsTrue(_target.Count == 1);
            Assert.AreSame(expected, _target.ToMvcHtmlString());
        }
    }
}
