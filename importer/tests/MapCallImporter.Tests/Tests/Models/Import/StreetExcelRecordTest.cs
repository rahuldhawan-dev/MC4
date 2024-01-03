using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallImporter.Tests.Models.Import
{
    [TestClass]
     public class StreetExcelRecordTest : ExcelRecordTestBase<Street, MyCreateStreet, StreetExcelRecord>
    {
        protected override StreetExcelRecord CreateTarget()
        {
            return new StreetExcelRecord {
                TownID = SampleValues.AberdeenMonmouthNJTown.ID,
                Street = "Streety"
            };
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<Street, MyCreateStreet, StreetExcelRecord> test)
        {
            test.RequiredString(s => s.Street, s => s.Name);
            test.RequiredEntityRef(
                s => s.TownID,
                s => s.Town,
                SampleValues.AberdeenMonmouthNJTown.ID);

            test.TestedElsewhere(x => x.StreetPrefix);
            test.TestedElsewhere(x => x.StreetSuffix);
            test.NotMapped(s => s.Town);
            test.NotMapped(s => s.StateID);
            test.NotMapped(s => s.CountyID);
        }

        #region Prefix/Suffix

        [TestMethod]
        public void TestPrefixIsMappedFromStreetPrefix()
        {
            var prefix = GetEntityFactory<StreetPrefix>().Create();
            _target.StreetPrefix = prefix.Description;

            WithUnitOfWork(uow => {
                Assert.AreEqual(prefix.Description, _target.MapToEntity(uow, 1, _mappingHelper).Prefix.Description);
            });
        }

        [TestMethod]
        public void TestThrowsWhenPrefixNotFoundInDatabase()
        {
            _target.StreetPrefix = "whatever man";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, _mappingHelper));
            });
        }

        [TestMethod]
        public void TestSuffixIsMappedFromStreetSuffix()
        {
            var suffix = GetEntityFactory<StreetSuffix>().Create();
            _target.StreetSuffix = suffix.Description;

            WithUnitOfWork(uow => {
                Assert.AreEqual(suffix.Description, _target.MapToEntity(uow, 1, _mappingHelper).Suffix.Description);
            });
        }

        [TestMethod]
        public void TestThrowsWhenSuffixNotFoundInDatabase()
        {
            _target.StreetSuffix = "whatever man";

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, _mappingHelper));
            });
        }

        #endregion

        #region FullStName

        [TestMethod]
        public void TestFullStNameIsSetWithAllThreePartsIfProvided()
        {
            var prefix = GetEntityFactory<StreetPrefix>().Create(new {Description = "foo"});
            var suffix = GetEntityFactory<StreetSuffix>().Create(new {Description = "baz"});

            _target.StreetPrefix = prefix.Description;
            _target.Street = "bar";
            _target.StreetSuffix = suffix.Description;

            WithUnitOfWork(uow => {
                Assert.AreEqual("foo bar baz", _target.MapToEntity(uow, 1, _mappingHelper).FullStName);
            });
        }

        [TestMethod]
        public void TestFullStNameIsSetWithJustStreetIfOnlyStreetProvided()
        {
            _target.Street = "bar";

            WithUnitOfWork(uow => {
                Assert.AreEqual("bar", _target.MapToEntity(uow, 1, _mappingHelper).FullStName);
            });
        }

        [TestMethod]
        public void TestFullStNameIsSetWithPrefixAndStreetIfProvided()
        {
            var prefix = GetEntityFactory<StreetPrefix>().Create(new { Description = "foo" });
            _target.StreetPrefix = prefix.Description;
            _target.Street = "bar";

            WithUnitOfWork(uow => {
                Assert.AreEqual("foo bar", _target.MapToEntity(uow, 1, _mappingHelper).FullStName);
            });
        }

        [TestMethod]
        public void TestFullStNameIsSetWithStreetAndSuffixIfProvided()
        {
            var suffix = GetEntityFactory<StreetSuffix>().Create(new { Description = "baz" });
            _target.Street = "bar";
            _target.StreetSuffix = suffix.Description;

            WithUnitOfWork(uow => {
                Assert.AreEqual("bar baz", _target.MapToEntity(uow, 1, _mappingHelper).FullStName);
            });
        }

        #endregion

        #region IsActive

        [TestMethod]
        public void TestIsActiveIsSetToTrue()
        {
            WithUnitOfWork(uow => {
                Assert.IsTrue(_target.MapToEntity(uow, 1, _mappingHelper).IsActive);
            });
        }

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateAberdeenNJWithCountyAndStateAndSomeStreets(_container);
        }

        #endregion
    }
}
