using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for RestorationMethodIntegrationTestTest
    /// </summary>
    [TestClass]
    public class RestorationMethodIntegrationTest
    {
        #region Constant

        public const int REFERENCE_ID = 1;

        #endregion

        #region Exposed Static Methods

        public static RestorationMethod GetValidRestorationMethod()
        {
            return RestorationMethodRepository.GetEntity(REFERENCE_ID);
        }

        #endregion

        #region Private Members

        private RestorationMethod _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void RestorationMethodIntegrationTestInitialize()
        {
            _target = new TestRestorationMethodBuilder();
        }

        #endregion
    }

    internal class TestRestorationMethodBuilder : TestDataBuilder<RestorationMethod>
    {
        #region Exposed Methods

        public override RestorationMethod Build()
        {
            var obj = new RestorationMethod();
            return obj;
        }

        #endregion
    }
}