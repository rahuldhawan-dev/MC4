using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class ModelFormatterProvidersTest
    {
        #region Tests

        [TestMethod]
        public void TestCurrentReturnsNewModelFormatterProviderIntanceIfCurrentIsSetToNull()
        {
            var previous = ModelFormatterProviders.Current;
            ModelFormatterProviders.Current = null;
            var result = ModelFormatterProviders.Current;
            Assert.IsNotNull(result);
            Assert.AreNotSame(previous, result);
        }

        [TestMethod]
        public void TestCurrentReturnsSetValue()
        {
            var expected = new ModelFormatterProvider();
            ModelFormatterProviders.Current = expected;
            Assert.AreSame(expected, ModelFormatterProviders.Current);
        }

        #endregion
    }
}
