using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using NHibernate.Linq;
using System.Linq;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class StateControllerTest : MapCallMvcControllerTestBase<StateController, State>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject<IRepository<State>>(_container.GetInstance<RepositoryBase<State>>());
        }

        #endregion

        #region Tests

        #region Index

        [TestMethod]
        public void TestIndexReturnsIndexViewWithCorrectStates()
        {
            var state1 = GetFactory<StateFactory>().Create(new {Abbreviation = "NJ"});
            var state2 = GetFactory<StateFactory>().Create(new {Abbreviation = "NY"});

            var result = _target.Index();

            MvcAssert.IsViewNamed(result, "Index");
            MvcAssert.IsViewWithEnumerableModel(result, new[] { state1, state2 });
        }

        #endregion


        #region Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var state = GetFactory<StateFactory>().Create();
            var viewModel = _viewModelFactory.Build<EditState, State>( state);
            viewModel.Abbreviation = "XX";
            viewModel.Name = "X X";
            viewModel.ScadaTable = "scada yo";

            _target.Update(viewModel);

            Session.Evict(state);

            var stateAgain = Session.Query<State>().Single(x => x.Id == state.Id);

            Assert.AreEqual(viewModel.Abbreviation, stateAgain.Abbreviation);
            Assert.AreEqual(viewModel.Name, stateAgain.Name);
            Assert.AreEqual(viewModel.ScadaTable, stateAgain.ScadaTable);
        }

        #endregion

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesDataLookups;
                a.RequiresRole("~/State/Index/", module);
                a.RequiresRole("~/State/Show/", module);
                a.RequiresSiteAdminUser("~/State/Edit/");
                a.RequiresSiteAdminUser("~/State/Update/");
                a.RequiresLoggedInUserOnly("~/State/ByOperatingCenterId/");
            });
        }
    }
}
