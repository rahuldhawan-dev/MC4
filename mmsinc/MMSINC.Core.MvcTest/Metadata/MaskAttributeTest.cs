using System.Web.Mvc;
using MMSINC.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class MaskAttributeTest
    {
        #region Fields

        private MaskAttribute _target;
        private ModelMetadata _metadata;
        private const string MASK = "Mask";

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new MaskAttribute(MASK);
            _metadata = ModelMetadataProviders.Current.GetMetadataForType(null,
                typeof(HtmlAttributeTest.MockViewModel));
        }

        #endregion

        [TestMethod]
        public void TestConstructorSetsMask()
        {
            Assert.AreEqual(MASK, _target.Mask);
        }

        [TestMethod]
        public void TestMaskGetsAndSets()
        {
            var expected = "I am a mask";
            _target.Mask = expected;
            Assert.AreEqual(expected, _target.Mask);
        }

        [TestMethod]
        public void TestDataMaskAttributeGetsAddedToHtmlAttributesWhenMaskIsSet()
        {
            _target.Process(_metadata);
            Assert.IsTrue(HtmlAttribute.GetHtmlAttributesFromMetadata(_metadata)
                                       .ContainsKey(MaskAttribute.DATA_MASK_ATTRIBUTE_KEY));
        }

        [TestMethod]
        public void TestNothingHappensWhenMaskIsNotSet()
        {
            _target.Mask = null;
            _target.Process(_metadata);
            Assert.IsNull(HtmlAttribute.GetHtmlAttributesFromMetadata(_metadata));
        }
    }
}
