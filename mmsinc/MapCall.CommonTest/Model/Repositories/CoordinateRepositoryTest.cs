using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    /// <summary>
    /// Summary description for CustomerCoordinateRepositoryTest
    /// </summary>
    [TestClass]
    public class CoordinateRepositoryTest : InMemoryDatabaseTest<Coordinate, CoordinateRepository>
    {
        #region Fields

        #endregion

        #region Tests

        [TestMethod]
        public void TestCloneAndSaveClonesAndSaves()
        {
            var original = GetFactory<CoordinateFactory>().Create();

            var result = Repository.CloneAndSave(original);
            Assert.AreNotEqual(original.Id, result.Id,
                "The cloned coordinate should not have the same Id as the original.");
            Assert.AreNotEqual(0, result.Id, "The cloned coordinate should have been saved.");
            Assert.AreEqual(original.Latitude, result.Latitude);
            Assert.AreEqual(original.Longitude, result.Longitude);

            Assert.IsNotNull(original.Icon, "Sanity");
            Assert.AreSame(original.Icon, result.Icon);
        }

        [TestMethod]
        public void TestCloneAndSaveSetsIconToDefaultIconSetIconIfOriginalDoesNotHaveIcon()
        {
            var defaultIcon = GetFactory<DefaultMapIconFactory>().Create();

            var original = GetFactory<CoordinateFactory>().Create();
            original.Icon = null;
            // Need to evict because Session.Save will think the Coordinate instance is dirty and try to save
            // the original with a null icon/
            Session.Evict(original);

            var result = Repository.CloneAndSave(original);
            Assert.AreNotEqual(original.Id, result.Id,
                "The cloned coordinate should not have the same Id as the original.");
            Assert.AreNotEqual(0, result.Id, "The cloned coordinate should have been saved.");
            Assert.AreEqual(original.Latitude, result.Latitude);
            Assert.AreEqual(original.Longitude, result.Longitude);
            Assert.AreSame(defaultIcon, result.Icon);
        }

        [TestMethod]
        public void TestFindByValuesReturnsNullIfNoMatchingRecordsAreFound()
        {
            // Make sure it doesn't bring back some random coordinate
            var someCoordinate = GetFactory<CoordinateFactory>().Create();

            var result = Repository.FindByValues(0, 0, 0);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestFindByValuesReturnsFirstValueWhenMultipleCoordinatesExistWithSameLatLonAndIcon()
        {
            var defaultIcon = GetFactory<DefaultMapIconFactory>().Create();
            var coord1 = GetFactory<CoordinateFactory>().Create(new { Latitude = 0m, Longitude = 0m, Icon = defaultIcon });
            var coord2 = GetFactory<CoordinateFactory>().Create(new { Latitude = 0m, Longitude = 0m, Icon = defaultIcon });
            Assert.AreNotSame(coord1, coord2, "Sanity test");

            var result = Repository.FindByValues(0, 0, defaultIcon.Id);

            Assert.AreSame(coord1, result);
        }

        [TestMethod]
        public void TestFindByValuesMatchesOnlyLatAndLonWithCoordinatesThatDoNotHaveAnIconWhenIconParameterEquals0()
        {
            var defaultIcon = GetFactory<DefaultMapIconFactory>().Create();
            var coordWithIcon = GetFactory<CoordinateFactory>().Create(new { Latitude = 0m, Longitude = 0m, Icon = defaultIcon });
            var coordWithoutIcon = GetFactory<CoordinateFactory>().Create(new { Latitude = 0m, Longitude = 0m, Icon = (MapIcon)null });

            Assert.AreSame(coordWithoutIcon, Repository.FindByValues(0, 0, 0));
            Assert.AreSame(coordWithIcon, Repository.FindByValues(0, 0, defaultIcon.Id));
        }

        #endregion
    }
}
