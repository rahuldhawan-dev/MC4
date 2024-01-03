using MapCall.Common.Model.Entities;
using MapCallImporter.Common;
using MapCallImporter.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using System;
using System.Linq;
using MapCall.Common.Model.Repositories;
using MapCallImporter.Library.Testing;
using MapCallImporter.SampleValues;
using NHibernate;
using NHibernate.Linq;
using StructureMap;

namespace MapCallImporter.Tests.Validation
{
    [TestClass]
    public class ExcelFileValidationServiceTest : MapCallImporterInMemoryDatabaseTestBase<ExcelFileValidationService>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        #endregion

        #region Private Methods

        private void TestInvalidFile(Action<IContainer> dataSeedFn, string filePath, params string[] expectedIssues)
        {
            dataSeedFn(_container);

            Assert.AreEqual(ExcelFileProcessingResult.InvalidFileContents,
                _target.Handle(GetRelativePath(filePath)).Result);

            MyAssert.AreEqual(expectedIssues, _target.LastIssues.ToArray());
        }

        private void TestInvalidFile(Func<IContainer, int> dataSeedFn, string filePath, params string[] expectedIssues)
        {
            TestInvalidFile(new Action<IContainer>((c) => dataSeedFn(c)), filePath, expectedIssues);
        }

        #endregion

        // NOTE: The tests in this class all depend on failures that come up during MapToEntity.
        // Because MapToEntity is no longer called if LastIssues has any values, issues which
        // arise in MapToEntity (mostly EntityMustExist type things) will only be caught for
        // the first record in a given file.  This is an unfortunate side-effect, but being able
        // to present the user with usable error messages was deemed more important.

        #region General

        [TestMethod]
        public void TestHandleReturnsCouldNotDetermineContentTypeWhenContentTypeIsIndeterminable()
        {
            Assert.AreEqual(ExcelFileProcessingResult.CouldNotDetermineContentType,
                _target.Handle(GetRelativePath(SampleFiles.SampleFiles.INDETERMINABLE_CONTENT_TYPE)).Result);
        }

        [TestMethod]
        public void TestHandleReturnsInvalidFileTypeWhenFileTypeIsInvalid()
        {
            Assert.AreEqual(ExcelFileProcessingResult.InvalidFileType,
                _target.Handle(GetRelativePath(SampleFiles.SampleFiles.INVALID_FILE_TYPE)).Result);
        }

        [TestMethod]
        public void TestHandleReturnsFileAlreadyOpenWhenFileIsAlreadyOpen()
        {
            WithOpenExcelFile(GetRelativePath(SampleFiles.SampleFiles.INDETERMINABLE_CONTENT_TYPE), p => {
                Assert.AreEqual(ExcelFileProcessingResult.FileAlreadyOpen, _target.Handle(p).Result);
            });
        }

        #endregion

        #region Streets

        [TestMethod]
        public void TestHandleValidatesStreetsFileWithMissingCountyAndSetsLastIssues()
        {
            TestInvalidFile(TestDataHelper.CreateAberdeenNJWithCountyAndStateAndSomeStreets,
                SampleFiles.SampleFiles.Import.Streets.MISSING_COUNTY,
                "Street at row 2 has CountyID 666 which was not found in the database."
                //"Street at row 3 has CountyID 666 which was not found in the database.",
                //"Street at row 4 has CountyID 666 which was not found in the database.",
                //"Street at row 5 has CountyID 666 which was not found in the database."
                );
        }

        [TestMethod]
        public void TestHandleValidatesStreetsFileWithMissingStateAndSetsLastIssues()
        {
            TestInvalidFile(TestDataHelper.CreateAberdeenNJWithCountyAndStateAndSomeStreets,
                SampleFiles.SampleFiles.Import.Streets.MISSING_STATE,
                "Street at row 2 has StateID 666 which was not found in the database.",
                "Street at row 2 has Town 41 and State 666, but according to the database that town is in State 1",
                "Street at row 2 has County 13 and State 666, but according to the database that county is in State 1"
                //"Street at row 3 has StateID 666 which was not found in the database.",
                //"Street at row 3 has Town 41 and State 666, but according to the database that town is in State 1",
                //"Street at row 3 has County 13 and State 666, but according to the database that county is in State 1",
                //"Street at row 4 has StateID 666 which was not found in the database.",
                //"Street at row 4 has Town 41 and State 666, but according to the database that town is in State 1",
                //"Street at row 4 has County 13 and State 666, but according to the database that county is in State 1",
                //"Street at row 5 has StateID 666 which was not found in the database.",
                //"Street at row 5 has Town 41 and State 666, but according to the database that town is in State 1",
                //"Street at row 5 has County 13 and State 666, but according to the database that county is in State 1"
            );
        }

        [TestMethod]
        public void TestHandleValidatesStreetsFileWithMissingTownAndSetsLastIssues()
        {
            TestInvalidFile(TestDataHelper.CreateAberdeenNJWithCountyAndStateAndSomeStreets,
                SampleFiles.SampleFiles.Import.Streets.MISSING_TOWN,
                "Row 2: Town's value does not match an existing object.",
                "Row 2: A town is required."
            );
        }

        [TestMethod]
        public void TestHandleValidatesStreetsFileWithStreetsAlreadyInDatabaseAndSetsLastIssues()
        {
            TestDataHelper.CreateAberdeenNJWithCountyAndStateAndSomeStreets(_container);
            var aberdeen = _container.GetInstance<ISession>().Get<Town>(SampleValues.AberdeenMonmouthNJTown.ID);

            void CreateStreet(string name, string suffix = null, string prefix = null)
            {
                var streetSuffix = string.IsNullOrWhiteSpace(suffix)
                    ? null
                    : Session.Query<StreetSuffix>().Single(ss => ss.Description == suffix);
                var streetPrefix = string.IsNullOrWhiteSpace(prefix)
                    ? null
                    : Session.Query<StreetPrefix>().Single(sp => sp.Description == prefix);
                var s = GetEntityFactory<Street>().Create(new {
                    Name = name,
                    Suffix = streetSuffix,
                    Prefix = streetPrefix,
                    Town = aberdeen,
                    FullStName = ""
                });
                s.Town.Streets.Add(s);
            }

            CreateStreet("ELECTRIC", "AVE");
            CreateStreet("ABBEY", "RD");
            CreateStreet(prefix: "POSITIVELY", name: "4th", suffix: "ST");
            CreateStreet("TOBACCO", "RD");

            Assert.AreEqual(ExcelFileProcessingResult.InvalidFileContents,
                _target.Handle(GetRelativePath(SampleFiles.SampleFiles.Import.Streets.VALID)).Result);

            MyAssert.AreEqual(new[] {
                "Row 2: A record already exists for this street for this town.",
            }, _target.LastIssues.ToArray());
        }

        #endregion

        #region Valves

        [TestMethod]
        public void TestHandleValidatesValvesFileWithMissingStreetAndSetsLastIssues()
        {
            TestInvalidFile(TestDataHelper.CreateStuffForValvesInAberdeenNJ, SampleFiles.SampleFiles.Import.Valves.MISSING_STREET,
                "Row 2: Street Name's value does not match an existing object."
                //"Row 3: Street Name's value does not match an existing object.",
                //"Row 4: Street Name's value does not match an existing object.",
                //"Row 5: Street Name's value does not match an existing object."
                );
        }

        [TestMethod]
        public void TestHandleValidatesValvesFileWithMissingTownAndSetsLastIssues()
        {
            TestInvalidFile(TestDataHelper.CreateStuffForValvesInAberdeenNJ,
                SampleFiles.SampleFiles.Import.Valves.MISSING_TOWN,
                "Row 2: Town 666 does not exist within operating center 10.  Please adjust the value in the file or add an OperatingCenterTown record through MapCall.",
                //"Row 2: Town's value does not match an existing object.",
                "Row 3: Town 666 does not exist within operating center 10.  Please adjust the value in the file or add an OperatingCenterTown record through MapCall.",
                //"Row 3: Town's value does not match an existing object."
                "Row 4: Town 666 does not exist within operating center 10.  Please adjust the value in the file or add an OperatingCenterTown record through MapCall.",
                //"Row 4: Town's value does not match an existing object.",
                "Row 5: Town 666 does not exist within operating center 10.  Please adjust the value in the file or add an OperatingCenterTown record through MapCall."
                //"Row 5: Town's value does not match an existing object."
            );
        }

        [TestMethod]
        public void TestHandleValidatesValvesFileWithValvesAlreadyInDatabaseAndSetsLastIssues()
        {
            TestDataHelper.CreateStuffForValvesInAberdeenNJ(_container);
            TestDataHelper.CreateValidValvesLikeTheValidValvesInTheValidValvesFile(_container);
            
            Assert.AreEqual(ExcelFileProcessingResult.InvalidFileContents,
                _target.Handle(GetRelativePath(SampleFiles.SampleFiles.Import.Valves.VALID)).Result);

            MyAssert.AreEqual(new[] {
                "Row 2: The valve number 'VAB-6666' is not unique to the operating center 'NJ7 - Shrewsbury'",
                //"Row 3: The valve number 'VAB-6667' is not unique to the operating center 'NJ7 - Shrewsbury'",
                //"Row 4: The valve number 'VAB-6668' is not unique to the operating center 'NJ7 - Shrewsbury'",
                //"Row 5: The valve number 'VAB-6669' is not unique to the operating center 'NJ7 - Shrewsbury'"
            }, _target.LastIssues.ToArray());
        }

        #endregion

        #region Hydrants

        [TestMethod]
        public void TestHandleValidatesHydrantsFileWithHydrantsAlreadyInDatabaseAndSetsLastIssues()
        {
            TestDataHelper.CreateStuffForHydrantsInAberdeenNJ(_container);
            TestDataHelper.CreateValidHydrantsLikeTheValidHydrantsInTheValidHydrantsFile(_container);

            var result = _target.Handle(GetRelativePath(SampleFiles.SampleFiles.Import.Hydrants.VALID));
            Assert.AreEqual(ExcelFileProcessingResult.InvalidFileContents,
                result.Result, string.Join(", ", result.Issues));

            MyAssert.AreEqual(new[] {
                    "Row 2: The hydrant number 'HAB-6666' is not unique to the operating center 'NJ7 - Shrewsbury'",
                    //"Row 3: The hydrant number 'HAB-6667' is not unique to the operating center 'NJ7 - Shrewsbury'",
                    //"Row 4: The hydrant number 'HAB-6668' is not unique to the operating center 'NJ7 - Shrewsbury'",
                    //"Row 5: The hydrant number 'HAB-6669' is not unique to the operating center 'NJ7 - Shrewsbury'",
                },
                _target.LastIssues.ToArray());
        }

        #endregion
    }
}
