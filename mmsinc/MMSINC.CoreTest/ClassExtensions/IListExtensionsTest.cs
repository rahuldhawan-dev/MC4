using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.IListExtensions;

namespace MMSINC.CoreTest.ClassExtensions
{
    #region Tests

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class IListExtensionsTest
    {
        [TestMethod]
        public void TestMapMapsListFromOneTypeToAnother()
        {
            var source = new List<int> {1, 2, 3};

            var result = source.Map(i => i.ToString());

            Assert.AreEqual("1", result.ToArray()[0]);
            Assert.AreEqual("2", result.ToArray()[1]);
            Assert.AreEqual("3", result.ToArray()[2]);
        }

        [TestMethod]
        public void TestIEnumerableVersionAlsoWorks()
        {
            var source = new List<int> {1, 2, 3};

            var result = IEnumerableExtensions.Map<int, string>(source, i => i.ToString());

            Assert.AreEqual("1", result.ToArray()[0]);
            Assert.AreEqual("2", result.ToArray()[1]);
            Assert.AreEqual("3", result.ToArray()[2]);
        }

        [TestMethod]
        public void TestMapToDictionaryWorksWithoutDupes()
        {
            var puppies = new[] {
                new {Id = 1, Name = "Spot"},
                new {Id = 2, Name = "Spike"}
            }.ToList();

            var duplicatePuppyCounter = 0;

            void DuplicateKeyLogger(string key) => ++duplicatePuppyCounter;

            var mappedPuppies = puppies.MapToDictionary(x => x.Name, DuplicateKeyLogger);

            Assert.AreEqual(0, duplicatePuppyCounter);
            Assert.AreEqual(2, mappedPuppies.Keys.Count());
            Assert.AreEqual(puppies.First(), mappedPuppies["Spot"]);
            Assert.AreEqual(puppies.Last(), mappedPuppies["Spike"]);
        }

        [TestMethod]
        public void TestMapToDictionaryWorksWithDupes()
        {
            var puppies = new[] {
                new {Id = 1, Name = "Sadie"},
                new {Id = 2, Name = "Ivy"},
                new {Id = 3, Name = "Ruby"},
                new {Id = 4, Name = "Ivy"},
                new {Id = 5, Name = "Shelby"}
            }.ToList();

            var duplicatePuppyCounter = 0;

            void DuplicateKeyLogger(string key) => ++duplicatePuppyCounter;

            var mappedPuppies = puppies.MapToDictionary(x => x.Name, DuplicateKeyLogger);

            Assert.AreEqual(1, duplicatePuppyCounter);
            Assert.AreEqual(4, mappedPuppies.Keys.Count());
            Assert.AreEqual(puppies[0], mappedPuppies["Sadie"]);
            Assert.AreEqual(puppies[1], mappedPuppies["Ivy"]);
            Assert.AreEqual(puppies[2], mappedPuppies["Ruby"]);
            Assert.AreEqual(puppies[4], mappedPuppies["Shelby"]);
        }
    }

    #endregion
}