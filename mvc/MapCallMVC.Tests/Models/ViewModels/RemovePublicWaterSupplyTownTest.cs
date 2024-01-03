using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class RemovePublicWaterSupplyTownTest : InMemoryDatabaseTest<Town>
    {
        #region Init/Cleanup

        private IViewModelFactory _viewModelFactory;

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Mock();
            e.For<IViewModelFactory>().Use<ViewModelFactory>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModelFactory = _container.GetInstance<IViewModelFactory>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestRemovePublicWaterSupplyTownMapToEntityRemovesPublicWaterSupply()
        {
            var pws = GetFactory<PublicWaterSupplyFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            town.PublicWaterSupplies.Add(pws);
            var target = _viewModelFactory.BuildWithOverrides<RemovePublicWaterSupplyTown, Town>(town, new { PublicWaterSupplyId = pws.Id });
            var entity = new Town();

            target.MapToEntity(entity);

            Assert.AreEqual(0, entity.PublicWaterSupplies.Count);
        }

        #endregion
    }
}