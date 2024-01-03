using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ChemicalTest
    {
        #region Fields

        private Chemical _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new Chemical();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestDisplayChemicalFormsDisplaysTheRightThing()
        {
            var stateOfMatter = new StateOfMatter {Id = 1, Description = "Gas"};
            var stateOfMatterTwo = new StateOfMatter {Id = 2, Description = "Liquid"};
            var stateOfMatterThree = new StateOfMatter {Id = 3, Description = "Solid"};

            _target.ChemicalStates.Add(stateOfMatter);
            _target.ChemicalStates.Add(stateOfMatterTwo);
            _target.ChemicalStates.Add(stateOfMatterThree);

            Assert.AreEqual(String.Join(", ", stateOfMatter, stateOfMatterTwo, stateOfMatterThree), _target.DisplayChemicalStates);

            // Lets make sure this doesn't blow up if the list is empty
            _target.ChemicalStates.Clear();
            Assert.AreEqual(string.Empty, _target.DisplayChemicalStates);
        }

        #endregion
    }
}
