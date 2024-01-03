using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        FunctionalLocationRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<FunctionalLocation,
            FunctionalLocationRepository>
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<ISensorRepository>().Use<SensorRepository>();
        }

        #endregion

        [TestMethod]
        public void TestGetByAssetTypeIdReturnsByAssetTypeId()
        {
            var assetTypeValid = GetFactory<EquipmentAssetTypeFactory>().Create();
            var assetTypeInvalid = GetFactory<ValveAssetTypeFactory>().Create();

            var functionalLocation = GetFactory<EquipmentFunctionalLocationFactory>()
               .Create(new {AssetType = assetTypeValid});
            var functionalLocationInvalid =
                GetFactory<ValveFunctionalLocationFactory>().Create(new {AssetType = assetTypeInvalid});

            var target = Repository.GetByAssetTypeId(assetTypeValid.Id);

            Assert.AreEqual(1, target.Count());
            Assert.IsTrue(target.Contains(functionalLocation));
            Assert.IsFalse(target.Contains(functionalLocationInvalid));
        }

        [TestMethod]
        public void TestGetByTownIdAndAssetTypeReturnsByTownIdAndAssetTypeId()
        {
            var town = GetEntityFactory<Town>().Create();
            var invalid = GetEntityFactory<Town>().Create();
            var assetTypeValid = GetFactory<EquipmentAssetTypeFactory>().Create();
            var assetTypeInvalid = GetFactory<ValveAssetTypeFactory>().Create();

            var functionalLocation = GetFactory<EquipmentFunctionalLocationFactory>()
               .Create(new {Town = town, AssetType = assetTypeValid});
            var functionalLocationInvalid1 = GetFactory<ValveFunctionalLocationFactory>()
               .Create(new {Town = town, AssetType = assetTypeInvalid});
            var functionalLocationInvalid2 = GetFactory<ValveFunctionalLocationFactory>()
               .Create(new {Town = invalid, AssetType = assetTypeValid});
            var functionalLocationInvalid3 = GetFactory<ValveFunctionalLocationFactory>()
               .Create(new {Town = invalid, AssetType = assetTypeInvalid});

            var target = Repository.GetByTownIdAndAssetTypeId(town.Id, assetTypeValid.Id);

            Assert.AreEqual(1, target.Count());
            Assert.IsTrue(target.Contains(functionalLocation));
            Assert.IsFalse(target.Contains(functionalLocationInvalid1));
            Assert.IsFalse(target.Contains(functionalLocationInvalid2));
            Assert.IsFalse(target.Contains(functionalLocationInvalid3));
        }

        [TestMethod]
        public void TestGetActiveByTownIdAndAssetTypeReturnsByTownIdAndAssetTypeId()
        {
            var town = GetEntityFactory<Town>().Create();
            var invalid = GetEntityFactory<Town>().Create();
            var assetTypeValid = GetFactory<EquipmentAssetTypeFactory>().Create();
            var assetTypeInvalid = GetFactory<ValveAssetTypeFactory>().Create();

            var functionalLocation = GetFactory<EquipmentFunctionalLocationFactory>()
               .Create(new {Town = town, AssetType = assetTypeValid});
            var functionalLocationInvalid0 = GetFactory<EquipmentFunctionalLocationFactory>()
               .Create(new {Town = town, AssetType = assetTypeValid, IsActive = false});
            var functionalLocationInvalid1 = GetFactory<ValveFunctionalLocationFactory>()
               .Create(new {Town = town, AssetType = assetTypeInvalid});
            var functionalLocationInvalid2 = GetFactory<ValveFunctionalLocationFactory>()
               .Create(new {Town = invalid, AssetType = assetTypeValid});
            var functionalLocationInvalid3 = GetFactory<ValveFunctionalLocationFactory>()
               .Create(new {Town = invalid, AssetType = assetTypeInvalid});

            var target = Repository.GetActiveByTownIdAndAssetTypeId(town.Id, assetTypeValid.Id);

            Assert.AreEqual(1, target.Count());
            Assert.IsTrue(target.Contains(functionalLocation));
            Assert.IsFalse(target.Contains(functionalLocationInvalid0));
            Assert.IsFalse(target.Contains(functionalLocationInvalid1));
            Assert.IsFalse(target.Contains(functionalLocationInvalid2));
            Assert.IsFalse(target.Contains(functionalLocationInvalid3));
        }

        [TestMethod]
        public void TestGetByTownIdReturnsByTownId()
        {
            var town = GetEntityFactory<Town>().Create();
            var invalidTown = GetEntityFactory<Town>().Create();
            var assetType1 = GetFactory<EquipmentAssetTypeFactory>().Create();
            var assetType2 = GetFactory<ValveAssetTypeFactory>().Create();

            var functionalLocation = GetFactory<EquipmentFunctionalLocationFactory>()
               .Create(new {Town = town, AssetType = assetType1});
            var functionalLocation1 = GetFactory<ValveFunctionalLocationFactory>()
               .Create(new {Town = town, AssetType = assetType1});
            var functionalLocation2 = GetFactory<ValveFunctionalLocationFactory>()
               .Create(new {Town = town, AssetType = assetType2});
            var functionalLocationInvalid = GetFactory<ValveFunctionalLocationFactory>()
               .Create(new {Town = invalidTown, AssetType = assetType1});

            var target = Repository.GetByTownId(town.Id);

            Assert.AreEqual(3, target.Count());
            Assert.IsTrue(target.Contains(functionalLocation));
            Assert.IsTrue(target.Contains(functionalLocation1));
            Assert.IsTrue(target.Contains(functionalLocation2));
            Assert.IsFalse(target.Contains(functionalLocationInvalid));
        }

        [TestMethod]
        public void TestGetActiveByTownIdReturnsActiveRecordsByTownId()
        {
            var town = GetEntityFactory<Town>().Create();
            var assetType1 = GetFactory<EquipmentAssetTypeFactory>().Create();
            var assetType2 = GetFactory<ValveAssetTypeFactory>().Create();

            var functionalLocation = GetFactory<EquipmentFunctionalLocationFactory>()
               .Create(new {Town = town, AssetType = assetType1});
            var functionalLocation1 = GetFactory<ValveFunctionalLocationFactory>()
               .Create(new {Town = town, AssetType = assetType1});
            var functionalLocation2 = GetFactory<ValveFunctionalLocationFactory>()
               .Create(new {Town = town, AssetType = assetType2});
            var functionalLocationInvalid = GetFactory<ValveFunctionalLocationFactory>()
               .Create(new {Town = town, AssetType = assetType1, IsActive = false});

            var target = Repository.GetActiveByTownId(town.Id);

            Assert.AreEqual(3, target.Count());
            Assert.IsTrue(target.Contains(functionalLocation));
            Assert.IsTrue(target.Contains(functionalLocation1));
            Assert.IsTrue(target.Contains(functionalLocation2));
            Assert.IsFalse(target.Contains(functionalLocationInvalid));
        }

        [TestMethod]
        public void TestGetByFacilityIdReturnsByFacilityId()
        {
            var assetType = GetFactory<EquipmentAssetTypeFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            var christmasTown = GetEntityFactory<Town>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {Town = town});
            var functionalLocation = GetFactory<EquipmentFunctionalLocationFactory>()
               .Create(new {AssetType = assetType, Town = town});
            var functionalLocationPartDeux =
                GetFactory<EquipmentFunctionalLocationFactory>().Create(new {AssetType = assetType});
            var functionalLocationOther = GetFactory<EquipmentFunctionalLocationFactory>()
               .Create(new {AssetType = assetType, Town = christmasTown});

            var target = Repository.GetByFacilityId(facility.Id);

            Assert.AreEqual(2, target.Count());
            Assert.IsTrue(target.Contains(functionalLocation));
            Assert.IsFalse(target.Contains(functionalLocationOther));
            Assert.IsTrue(target.Contains(functionalLocationPartDeux));
        }
    }
}
