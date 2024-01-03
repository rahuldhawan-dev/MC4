using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class HydrantModelRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<HydrantModel, HydrantModelRepository>
    {
        #region Tests

        [TestMethod]
        public void TestGetByManufacturerIdReturnsMatchingHydrantModels()
        {
            var model1 = GetEntityFactory<HydrantModel>().Create();
            var model2 = GetEntityFactory<HydrantModel>().Create();

            var result = Repository.GetByManufacturerId(model1.HydrantManufacturer.Id);
            Assert.AreSame(model1, result.Single());

            result = Repository.GetByManufacturerId(model2.HydrantManufacturer.Id);
            Assert.AreSame(model2, result.Single());
        }

        #endregion
    }
}
