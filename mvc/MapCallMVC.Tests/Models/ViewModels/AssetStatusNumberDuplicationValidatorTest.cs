using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class AssetStatusNumberDuplicationValidatorTest : InMemoryDatabaseTest<Hydrant>
    {
        private AssetStatusNumberDuplicationValidator _target;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _target = _container.GetInstance<AssetStatusNumberDuplicationValidator>();
        }

        #endregion

        private void DoTest(int assetAStatus, int assetBStatus, bool expectedResult)
        {
            var result = _target.ShouldAllowDuplicate(assetAStatus, assetBStatus);
            if (expectedResult != result)
            {
                Assert.Fail($"Expected '{expectedResult}' but received '{result}' when passing '{assetAStatus}' and '{assetBStatus}' to ShouldAllowDuplicate.");
            }
        }

        [TestMethod]
        public void TestTruthTable()
        {
            foreach ((int assetAStatus, int assetBStatus, bool expectedResult) in AssetStatusNumberDuplicationValidator.TRUTH_TABLE)
            {
                DoTest(assetAStatus, assetBStatus, expectedResult);
            }
        }
    }
}
