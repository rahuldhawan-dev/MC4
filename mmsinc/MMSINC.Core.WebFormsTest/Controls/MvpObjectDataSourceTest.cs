using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Controls
{
    /// <summary>
    /// Summary description for MvpObjectDataSourceTest.
    /// </summary>
    [TestClass]
    public class MvpObjectDataSourceTest : EventFiringTestClass
    {
        #region Private Members

        private TestMvpObjectDataSource _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void MvpObjectDataSourceTestInitialize()
        {
            _target = new TestMvpObjectDataSourceBuilder();
        }

        #endregion

        [TestMethod]
        public void TestSetDefaultSelectParameterValueSetsSelectParameterToDefaultValue()
        {
            var parameterName = "foo";
            var expected = "bar";
            _target.SelectParameters.Add(new Parameter(parameterName));
            using (_mocks.Record())
            {
                _target.SelectParameters[parameterName].DefaultValue = expected;
            }

            using (_mocks.Playback())
            {
                _target.SetDefaultSelectParameterValue(parameterName, expected);
                Assert.AreEqual(expected, _target.SelectParameters[parameterName].DefaultValue);
            }
        }
    }

    internal class TestMvpObjectDataSourceBuilder : TestDataBuilder<TestMvpObjectDataSource>
    {
        #region Exposed Methods

        public override TestMvpObjectDataSource Build()
        {
            var obj = new TestMvpObjectDataSource();
            return obj;
        }

        #endregion
    }

    internal class TestMvpObjectDataSource : MvpObjectDataSource { }
}
