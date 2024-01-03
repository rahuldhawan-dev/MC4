using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using SpoilRemovalEntity = MapCall.Common.Model.Entities.SpoilRemoval;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.SpoilRemoval
{
    [TestClass]
    public class SpoilRemovalViewModelTest<TViewModel> : ViewModelTestBase<SpoilRemovalEntity, TViewModel> where TViewModel : SpoilRemovalViewModel
    {
        #region Init/Cleanup

        protected override SpoilRemovalEntity CreateEntity()
        {
            return GetEntityFactory<SpoilRemovalEntity>().Build();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IStateRepository>().Use<StateRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.RemovedFrom, GetEntityFactory<SpoilStorageLocation>().Create());
            ValidationAssert.EntityMustExist(x => x.FinalDestination, GetEntityFactory<SpoilFinalProcessingLocation>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.RemovedFrom);
            _vmTester.CanMapBothWays(x => x.FinalDestination);
            _vmTester.CanMapBothWays(x => x.DateRemoved);
            _vmTester.CanMapBothWays(x => x.Quantity);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.RemovedFrom);
            ValidationAssert.PropertyIsRequired(x => x.DateRemoved);
        }

        [TestMethod]
        public override void TestStringLengthValidation() { }

        #endregion
    }
}
