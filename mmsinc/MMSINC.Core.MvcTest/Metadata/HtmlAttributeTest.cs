using System.Collections.Generic;
using System.Web.Mvc;
using MMSINC.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class HtmlAttributeTest
    {
        #region Fields

        private MockHtmlAttribute _target;
        private ModelMetadata _metadata;
        private const string KEY = "KEY";
        private const string VALUE = "VALUE";

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new MockHtmlAttribute();
            _target.TestKey = KEY;
            _target.TestValue = VALUE;
            _target.AddAttributesWhenISaySo = true;
            _metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(MockViewModel));
        }

        #endregion

        #region Tests

        #region Process

        [TestMethod]
        public void TestProcessDoesNotSetMetadataAdditionalValuesIfAddAttributesDoesNotAddAnyAttributes()
        {
            _target.AddAttributesWhenISaySo = false;

            Assert.IsFalse(_metadata.AdditionalValues.ContainsKey(HtmlAttribute.HTML_ATTRIBUTES_KEY));
            _target.Process(_metadata);
            Assert.IsFalse(_metadata.AdditionalValues.ContainsKey(HtmlAttribute.HTML_ATTRIBUTES_KEY));

            // Testing to ensure if there's a dictionary already in there that it doesn't get replaced.
            var expected = new Dictionary<string, object>();
            _metadata.AdditionalValues[HtmlAttribute.HTML_ATTRIBUTES_KEY] = expected;
            _target.Process(_metadata);
            Assert.AreSame(expected, _metadata.AdditionalValues[HtmlAttribute.HTML_ATTRIBUTES_KEY]);
        }

        [TestMethod]
        public void TestProcessMergesExistingValuesWithNewValuesFromAddAttributes()
        {
            var expected = new Dictionary<string, object>();
            expected.Add("Some key", "Some value");
            _metadata.AdditionalValues[HtmlAttribute.HTML_ATTRIBUTES_KEY] = expected;
            _target.Process(_metadata);

            var result = HtmlAttribute.GetHtmlAttributesFromMetadata(_metadata);
            Assert.IsTrue(result.ContainsKey("Some key"));
            Assert.AreEqual("Some value", result["Some key"]);
            Assert.IsTrue(result.ContainsKey(KEY));
            Assert.AreEqual(VALUE, result[KEY]);
        }

        #endregion

        #region GetHtmlAttributesFromMetadata

        [TestMethod]
        public void TestGetHtmlAttributesFromMetadataReturnsNullIfKeyNotInAdditionalValues()
        {
            Assert.IsFalse(_metadata.AdditionalValues.ContainsKey(HtmlAttribute.HTML_ATTRIBUTES_KEY));
            Assert.IsNull(HtmlAttribute.GetHtmlAttributesFromMetadata(_metadata));
        }

        [TestMethod]
        public void TestGetHtmlAttributesFromMetadataReturnsSameInstanceInAdditionalValues()
        {
            var expected = new Dictionary<string, object>();
            _metadata.AdditionalValues[HtmlAttribute.HTML_ATTRIBUTES_KEY] = expected;
            Assert.AreSame(expected, HtmlAttribute.GetHtmlAttributesFromMetadata(_metadata));
        }

        #endregion

        #endregion

        #region Helper classes

        public class MockViewModel
        {
            public int IntProp { get; set; }
        }

        private class MockHtmlAttribute : HtmlAttribute
        {
            public string TestKey { get; set; }
            public string TestValue { get; set; }
            public bool AddAttributesWhenISaySo { get; set; }

            protected override void AddAttributes(ModelMetadata modelMetadata, IDictionary<string, object> attrDict)
            {
                if (AddAttributesWhenISaySo)
                {
                    attrDict.Add(TestKey, TestValue);
                }
            }
        }

        #endregion
    }
}
