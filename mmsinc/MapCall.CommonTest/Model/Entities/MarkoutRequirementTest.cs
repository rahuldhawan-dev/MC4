using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class MarkoutRequirementTest
    {
        #region Private Members

        private MarkoutRequirement _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void AssetTypeTestInitialize()
        {
            _target = new MarkoutRequirement();
        }

        #endregion

        [TestMethod]
        public void TestToStringReturnsDescription()
        {
            var description = "this is the description";
            _target.Description = description;

            Assert.AreEqual(description, _target.ToString());
        }

        [TestMethod]
        public void TestMarkoutRequirementEnumDerivesFromMarkoutRequirementID()
        {
            foreach (var value in Enum.GetValues(typeof(MarkoutRequirementEnum)))
            {
                var id = (int)value;
                _target.SetPropertyValueByName("Id", id);

                Assert.AreEqual(value, _target.MarkoutRequirementEnum);
            }
        }
    }
}
