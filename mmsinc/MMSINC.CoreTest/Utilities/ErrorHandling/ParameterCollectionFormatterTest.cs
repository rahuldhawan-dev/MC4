using System.Collections.Specialized;
using System.Linq;
using System.Text;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Utilities.ErrorHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace MMSINC.CoreTest.Utilities.ErrorHandling
{
    /// <summary>
    /// Summary description for ParameterCollectionFormatterTest.
    /// </summary>
    [TestClass, DoNotParallelize]
    public class ParameterCollectionFormatterTest : EventFiringTestClass
    {
        #region Private Members

        private TestParameterCollectionFormatter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void ParameterCollectionFormatterTestInitialize()
        {
            base.EventFiringTestClassInitialize();
            _target = new TestParameterCollectionFormatterBuilder();
        }

        [TestCleanup]
        public void ParameterCollectionFormatterTestCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestFormatParametersReturnsParamsAsString()
        {
            var request = _mocks.DynamicMock<IRequest>();
            var verb = "POST";
            var prms = new NameValueCollection {
                {"foo", "bar"},
                {"foobar", "baz"},
                {"SKIPME", "PLEASE"},
                {"METOO", "THANKS"}
            };
            var indentString = "&nbsp;&nbsp;";
            var lineSeparator = "<br/>";

            using (_mocks.Record())
            {
                SetupResult.For(request.Params).Return(prms);
                SetupResult.For(request.HttpMethod).Return(verb);
            }

            using (_mocks.Playback())
            {
                var sb = new StringBuilder();
                sb.AppendFormat("{0}Http Verb: {1}", indentString, verb);
                foreach (var key in prms.AllKeys)
                {
                    if (!(new[] {"SKIPME", "METOO"}).Contains(key))
                    {
                        sb.AppendFormat("{0}{1}{2}: {3}", lineSeparator,
                            indentString, key, prms[key]);
                    }
                }

                Assert.AreEqual(sb.ToString(),
                    _target.FormatParameters(request, indentString, lineSeparator, "SKIPME", "METOO"));
            }
        }
    }

    internal class TestParameterCollectionFormatterBuilder : TestDataBuilder<TestParameterCollectionFormatter>
    {
        #region Exposed Methods

        public override TestParameterCollectionFormatter Build()
        {
            var obj = new TestParameterCollectionFormatter();
            return obj;
        }

        #endregion
    }

    internal class TestParameterCollectionFormatter : ParameterCollectionFormatter { }
}
