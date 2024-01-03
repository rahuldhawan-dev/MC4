using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using Moq;
using NHibernate.Linq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class MapImageRepositoryTest : InMemoryDatabaseTest<MapImage, MapImageRepository>
    {
        #region Fields

        private string _tempDir = Path.GetTempPath();
        private Mock<IImageToPdfConverter> _converter;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IImageToPdfConverter>().Use((_converter = new Mock<IImageToPdfConverter>()).Object);
        }

        [TestInitialize]
        public void InitializeTest()
        {
            ConfigurationManager.AppSettings["ImageUploadRootDirectory"] = _tempDir;
        }

        [TestCleanup]
        public void CleanupTest()
        {
            ConfigurationManager.AppSettings["ImageUploadRootDirectory"] = null;
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSaveThrowsNotSupportedExceptionForBothOverloads()
        {
            MyAssert.Throws<NotSupportedException>(() => Repository.Save(new MapImage()));
            MyAssert.Throws<NotSupportedException>(() => Repository.Save(new[] {new MapImage(), new MapImage()}));
        }

        [TestMethod]
        public void TestDeleteThrowsNotSupportedException()
        {
            MyAssert.Throws<NotSupportedException>(() => Repository.Delete(new MapImage()));
        }

        [TestMethod]
        public void TestGetImageDataAsPdfThrowsNotSupportedExceptionWhenFileNameHasUnknownExtension()
        {
            var model = new MapImage();
            model.FileName = "something.mp3";
            MyAssert.Throws<NotSupportedException>(() => Repository.GetImageDataAsPdf(model));
        }

        [TestMethod]
        public void TestGetImageDataAsPdfReturnsFileDataAsIsIfFileNameEndsWithPdfRegardlessOfCase()
        {
            var exts = new[] {"pdf", "PDF"};
            var expectedData = new byte[] {1, 2, 3, 4, 5};
            var expectedPath = _tempDir + @"NJ\Some\Dir\For\Maps\42\this is a file.pdf";
            FileIO.DeleteIfFileExists(expectedPath);
            FileIO.WriteAllBytes(expectedPath, expectedData);

            foreach (var ext in exts)
            {
                var model = new MapImage();
                model.FileName = "this is a file." + ext;
                model.Directory = "NJ/Some/Dir/For/Maps/42/";

                var result = Repository.GetImageDataAsPdf(model);
                MyAssert.AreEqual(expectedData, result);
            }
        }

        [TestMethod]
        public void TestGetImageDataAsPdfReturnsSingleTifFileConvertedToPdfIfFileNameEndsInTifOrTiffRegardlesOfCase()
        {
            var exts = new[] {"tif", "TIF", "tiff", "TIFF"};
            var expectedData = new byte[] {1, 2, 3, 4, 5};
            foreach (var ext in exts)
            {
                var expectedPath = _tempDir + @"NJ\Some\Dir\For\Maps\42\this is a file." + ext;
                FileIO.DeleteIfFileExists(expectedPath);
                FileIO.WriteAllBytes(expectedPath, expectedData);

                var model = new MapImage();
                model.FileName = "this is a file." + ext;
                model.Directory = "NJ/Some/Dir/For/Maps/42/";

                _converter.Setup(x => x.Render(It.IsAny<IEnumerable<byte[]>>()))
                          .Callback<IEnumerable<byte[]>>(x => { MyAssert.AreEqual(expectedData, x.Single()); });

                Repository.GetImageDataAsPdf(model);
                _converter.Verify(x => x.Render(It.IsAny<IEnumerable<byte[]>>()));
            }
        }

        [TestMethod]
        public void
            TestGetImageDataAsPdfReturnsPdfMadeUpOfMultipleFilesWhenFileNameIsABunchOfNumbersWithoutAFileExtension()
        {
            // Must be 8 characters long, no file extensions.
            var fileName1 = "11111111";
            var fileName2 = "22222222";
            var filePath1 = _tempDir + @"NJ\Some\Dir\For\Maps\42\" + fileName1 + ".tif";
            var filePath2 = _tempDir + @"NJ\Some\Dir\For\Maps\42\" + fileName2 + ".tif";
            var expectedFileData1 = new byte[] {1, 2, 3, 4, 5};
            var expectedFileData2 = new byte[] {4, 5, 6, 7, 8};

            FileIO.DeleteIfFileExists(filePath1);
            FileIO.DeleteIfFileExists(filePath2);

            FileIO.WriteAllBytes(filePath1, expectedFileData1);
            FileIO.WriteAllBytes(filePath2, expectedFileData2);

            var model = new MapImage();
            model.FileName = fileName1 + fileName2;
            model.Directory = "NJ/Some/Dir/For/Maps/42/";

            _converter.Setup(x => x.Render(It.IsAny<IEnumerable<byte[]>>()))
                      .Callback<IEnumerable<byte[]>>(x => {
                           var results = x.ToArray();
                           MyAssert.AreEqual(expectedFileData1, results[0]);
                           MyAssert.AreEqual(expectedFileData2, results[1]);
                       });

            Repository.GetImageDataAsPdf(model);
            _converter.Verify(x => x.Render(It.IsAny<IEnumerable<byte[]>>()));
        }

        [TestMethod]
        public void TestGetImageDataAsPdfReturnsDataFromFileBasedOnMapPageIfMapPageHasValue()
        {
            var expectedData = new byte[] {1, 2, 3, 4, 5};
            var expectedPath = _tempDir + @"NJ\Some\Dir\For\Maps\42\1234.tif";
            FileIO.DeleteIfFileExists(expectedPath);
            FileIO.WriteAllBytes(expectedPath, expectedData);

            var model = new MapImage();
            model.FileName = "if you see this then MapPage didn't get used like it should have";
            model.MapPage = "1234";
            model.Directory = "NJ/Some/Dir/For/Maps/42/";

            _converter.Setup(x => x.Render(It.IsAny<IEnumerable<byte[]>>()))
                      .Callback<IEnumerable<byte[]>>(x => { MyAssert.AreEqual(expectedData, x.Single()); });

            Repository.GetImageDataAsPdf(model);
            _converter.Verify(x => x.Render(It.IsAny<IEnumerable<byte[]>>()));
        }

        [TestMethod]
        public void
            TestGetImageDateAsPdfReturnsFileNameWithLeadingZeroesRemovedForThatOneDirectoryThatHasPoorlyNamedFilesForSomeReason()
        {
            var expectedData = new byte[] {1, 2, 3, 4, 5};
            var expectedPath = _tempDir + @"NJ\00-Maps\00630001\00\1234.tif";
            FileIO.DeleteIfFileExists(expectedPath);
            FileIO.WriteAllBytes(expectedPath, expectedData);

            var model = new MapImage();
            model.FileName = "00001234";
            model.MapPage = "I exist but should not be used in this instance";
            model.Directory = "NJ/00-Maps/00630001/00/";

            _converter.Setup(x => x.Render(It.IsAny<IEnumerable<byte[]>>()))
                      .Callback<IEnumerable<byte[]>>(x => { MyAssert.AreEqual(expectedData, x.Single()); });

            Repository.GetImageDataAsPdf(model);
            _converter.Verify(x => x.Render(It.IsAny<IEnumerable<byte[]>>()));
        }

        [TestMethod]
        public void TestGetImageDataAsPdfReturnsFileNameWhenMapPageIsNotSet()
        {
            var expectedData = new byte[] {1, 2, 3, 4, 5};
            var expectedPath = _tempDir + @"NJ\Some\Dir\For\Maps\42\1234.tif";
            FileIO.DeleteIfFileExists(expectedPath);
            FileIO.WriteAllBytes(expectedPath, expectedData);

            var model = new MapImage();
            model.FileName = "1234.tif";
            model.MapPage = null;
            model.Directory = "NJ/Some/Dir/For/Maps/42/";

            _converter.Setup(x => x.Render(It.IsAny<IEnumerable<byte[]>>()))
                      .Callback<IEnumerable<byte[]>>(x => { MyAssert.AreEqual(expectedData, x.Single()); });

            Repository.GetImageDataAsPdf(model);
            _converter.Verify(x => x.Render(It.IsAny<IEnumerable<byte[]>>()));
        }

        [TestMethod]
        public void TestFindImageInDirectionCanFindsImagesInDirections()
        {
            var town = GetFactory<TownFactory>().Create();
            var current = GetFactory<MapImageFactory>().Create(new
                {North = "NORTH", South = "SOUTH", East = "EAST", West = "WEST", Town = town});
            var north = GetFactory<MapImageFactory>().Create(new {MapPage = "NORTH", Town = town});
            var south = GetFactory<MapImageFactory>().Create(new {MapPage = "SOUTH", Town = town});
            var east = GetFactory<MapImageFactory>().Create(new {MapPage = "EAST", Town = town});
            var west = GetFactory<MapImageFactory>().Create(new {MapPage = "WEST", Town = town});

            Assert.AreSame(north, Repository.FindImageInDirection(current, MapImageDirection.North));
            Assert.AreSame(south, Repository.FindImageInDirection(current, MapImageDirection.South));
            Assert.AreSame(east, Repository.FindImageInDirection(current, MapImageDirection.East));
            Assert.AreSame(west, Repository.FindImageInDirection(current, MapImageDirection.West));
        }

        [TestMethod]
        public void TestFindImageInDirectionCanFindsImagesInDirectionsWhenSeveralExist()
        {
            var town = GetFactory<TownFactory>().Create();
            var current = GetFactory<MapImageFactory>().Create(new
                {North = "NORTH", South = "SOUTH", East = "EAST", West = "WEST", Town = town});
            var north = GetFactory<MapImageFactory>().Create(new {MapPage = "NORTH", Town = town});
            var north2 = GetFactory<MapImageFactory>().Create(new {MapPage = "NORTH", Town = town});
            var south = GetFactory<MapImageFactory>().Create(new {MapPage = "SOUTH", Town = town});
            var east = GetFactory<MapImageFactory>().Create(new {MapPage = "EAST", Town = town});
            var west = GetFactory<MapImageFactory>().Create(new {MapPage = "WEST", Town = town});

            Assert.AreSame(north2, Repository.FindImageInDirection(current, MapImageDirection.North));
            Assert.AreSame(south, Repository.FindImageInDirection(current, MapImageDirection.South));
            Assert.AreSame(east, Repository.FindImageInDirection(current, MapImageDirection.East));
            Assert.AreSame(west, Repository.FindImageInDirection(current, MapImageDirection.West));
        }

        [TestMethod]
        public void TestFindImageInDirectionReturnsNullIfDirectionIsNONE()
        {
            var town = GetFactory<TownFactory>().Create();
            var current = GetFactory<MapImageFactory>().Create(new {North = "NONE", Town = town});
            // The repo should return null regardless of whether or not a MapPage manages to be set to "NONE".
            var dontFind = GetFactory<MapImageFactory>().Create(new {MapPage = "NONE", Town = town});

            Assert.IsNull(Repository.FindImageInDirection(current, MapImageDirection.North));
        }

        [TestMethod]
        public void TestFindImageInDirectionREturnsNullIfMatchingMapPageExistsButIsInDifferentDistrict()
        {
            var town = GetFactory<TownFactory>().Create(new {
                DistrictId = 12345f
            });
            var townDiffDistrict = GetFactory<TownFactory>().Create(new {
                DistrictId = 44444f
            });
            var current = GetFactory<MapImageFactory>().Create(new {North = "NORTH", Town = town});
            // The repo should return null regardless of whether or not a MapPage manages to be set to "NONE".
            var dontFind = GetFactory<MapImageFactory>().Create(new {MapPage = "NORTH", Town = townDiffDistrict});

            Assert.IsNull(Repository.FindImageInDirection(current, MapImageDirection.North));
        }

        #endregion
    }
}
