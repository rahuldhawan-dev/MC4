using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class EditStateTest : InMemoryDatabaseTest<State>
    {
        #region Fields

        private State _entity;
        private EditState _viewModel;
        private ViewModelTester<EditState, State> _vmTester;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetFactory<StateFactory>().Create();
            _viewModel = new EditState(_container);
            _vmTester = new ViewModelTester<EditState, State>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestMappingAbbreviation()
        {
            _vmTester.CanMapBothWays(x => x.Abbreviation, "ZZ", "AA");
        }

        [TestMethod]
        public void TestMappingId()
        {
            _vmTester.CanMapToViewModel(x => x.Id, 12);
        }

        [TestMethod]
        public void TestMappingName()
        {
            _vmTester.CanMapBothWays(x => x.Name, "My Name", "My Other Name");
        }

        [TestMethod]
        public void TestMappingScadaTable()
        {
            _vmTester.CanMapBothWays(x => x.ScadaTable, "Whats a scada table?", "I don't know.");
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestMaxLengthOnTheStringProperties()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Abbreviation, State.MaxLengths.ABBREVIATION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Name, State.MaxLengths.NAME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ScadaTable, State.MaxLengths.SCADA_TBL);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Abbreviation);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Name);
        }

        #endregion

        #endregion
    }
}
