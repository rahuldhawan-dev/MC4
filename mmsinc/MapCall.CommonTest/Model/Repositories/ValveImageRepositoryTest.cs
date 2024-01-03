using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
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
    public class ValveImageRepositoryTest : InMemoryDatabaseTest<ValveImage, ValveImageRepository>
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
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            ConfigurationManager.AppSettings["ImageUploadRootDirectory"] = _tempDir;
            ConfigurationManager.AppSettings["ImageUploadValveDirectory"] = @"Some\Dir\For\Valves";
        }

        [TestCleanup]
        public void CleanupTest()
        {
            ConfigurationManager.AppSettings["ImageUploadRootDirectory"] = null;
            ConfigurationManager.AppSettings["ImageUploadValveDirectory"] = null;
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSaveSetsIsDefaultToFalseOnOtherValveImagesForSameValveIfIsDefaultIsSetToTrue()
        {
            var valve = GetFactory<ValveFactory>().Create();
            var image1 = GetFactory<ValveImageFactory>().Create(new {
                Valve = valve,
                IsDefaultImageForValve = true,
            });
            var image2 = GetFactory<ValveImageFactory>().Create(new {
                Valve = valve,
                IsDefaultImageForValve = false
            });

            image2.IsDefaultImageForValve = true;
            Repository.Save(image2);

            // Evict to ensure saving actually saves to the database.
            Session.Evict(image1);
            Session.Evict(image2);

            Assert.IsFalse(Session.Query<ValveImage>().Single(x => x.Id == image1.Id).IsDefaultImageForValve);
            Assert.IsTrue(Session.Query<ValveImage>().Single(x => x.Id == image2.Id).IsDefaultImageForValve);
        }

        [TestMethod]
        public void TestSaveImageToExpectedDirectory()
        {
            var expectedPath = _tempDir + @"NJ\Some\Dir\For\Valves\42\this is a file.tif";
            var model = new ValveImage();
            model.FileName = "this is a file.tif";
            model.ImageData = new byte[] {1, 2, 3, 4, 5};
            model.Town = GetFactory<TownFactory>().Create(new {DistrictId = 42f});
            model.OperatingCenter = GetFactory<OperatingCenterFactory>().Create();
            Assert.AreEqual("NJ", model.Town.County.State.Abbreviation, "Sanity check");
            Repository.Save(model);
            var result = File.ReadAllBytes(expectedPath);
            MyAssert.AreEqual(model.ImageData, result);
        }

        [TestMethod]
        public void TestSaveFilePutsSlashAtEndOfDirectoryString()
        {
            var expectedPath = _tempDir + @"NJ\Some\Dir\For\Valves\42\this is a file.tif";
            FileIO.DeleteIfFileExists(expectedPath);
            var model = new ValveImage();
            model.FileName = "this is a file.tif";
            model.ImageData = new byte[] {1, 2, 3, 4, 5};
            model.Town = GetFactory<TownFactory>().Create(new {DistrictId = 42f});
            model.OperatingCenter = GetFactory<OperatingCenterFactory>().Create();
            Repository.Save(model);
            Assert.AreEqual(@"NJ/Some/Dir/For/Valves/42/", model.Directory);
        }

        [TestMethod]
        public void TestFileExistsReturnsTrueIfAFileExists()
        {
            var expectedPath = _tempDir + @"NJ\Some\Dir\For\Valves\42\i will exist at some point.tif";
            // Ensure this file does not exist.
            FileIO.DeleteIfFileExists(expectedPath);

            var model = new ValveImage();
            model.FileName = "i will exist at some point.tif";
            model.ImageData = new byte[] {1, 2, 3, 4, 5};
            model.Town = GetFactory<TownFactory>().Create(new {DistrictId = 42f});
            model.OperatingCenter = GetFactory<OperatingCenterFactory>().Create();

            Assert.IsFalse(Repository.FileExists(expectedPath, model.Town));
            Repository.Save(model);
            Assert.IsTrue(Repository.FileExists(expectedPath, model.Town));
        }

        [TestMethod]
        public void TestDeleteDeletesTheFile()
        {
            var expectedPath = _tempDir + @"NJ\Some\Dir\For\Valves\42\file.tif";
            FileIO.DeleteIfFileExists(expectedPath);
            var town = GetFactory<TownFactory>().Create(new {DistrictId = 42f});
            var model = GetFactory<ValveImageFactory>().Create(new {
                FileName = "file.tif",
                Directory = @"NJ\Some\Dir\For\Valves\42\",
                Town = town
            });

            FileIO.WriteAllBytes(expectedPath, new byte[] {1, 2, 3});

            Repository.Delete(model);

            Assert.IsFalse(File.Exists(expectedPath));
        }

        [TestMethod]
        public void TestDeleteDoesNotDeleteTheFileIfThereAreMultipleRecordsReferencingTheSameFile()
        {
            var expectedPath = _tempDir + @"NJ\Some\Dir\For\Valves\42\file.tif";
            FileIO.DeleteIfFileExists(expectedPath);
            var town = GetFactory<TownFactory>().Create(new {DistrictId = 42f});
            var model = GetFactory<ValveImageFactory>().Create(new {
                FileName = "file.tif",
                Directory = @"NJ\Some\Dir\For\Valves\42\",
                Town = town
            });
            var modelWithSameFile = GetFactory<ValveImageFactory>().Create(new {
                FileName = "file.tif",
                Directory = @"NJ\Some\Dir\For\Valves\42\",
                Town = town
            });

            FileIO.WriteAllBytes(expectedPath, new byte[] {1, 2, 3});
            Repository.Delete(model);

            Assert.IsTrue(File.Exists(expectedPath));
        }

        [TestMethod]
        public void TestGetImageDataAsPdfThrowsNotSupportedExceptionWhenFileNameHasUnknownExtension()
        {
            var model = new ValveImage();
            model.FileName = "something.mp3";
            MyAssert.Throws<NotSupportedException>(() => Repository.GetImageDataAsPdf(model));
        }

        [TestMethod]
        public void TestGetImageDataAsPdfReturnsFileDataAsIsIfFileNameEndsWithPdfRegardlessOfCase()
        {
            var exts = new[] {"pdf", "PDF"};
            var expectedData = new byte[] {1, 2, 3, 4, 5};
            var expectedPath = _tempDir + @"NJ\Some\Dir\For\Valves\42\this is a file.pdf";
            FileIO.DeleteIfFileExists(expectedPath);
            FileIO.WriteAllBytes(expectedPath, expectedData);

            foreach (var ext in exts)
            {
                var model = new ValveImage();
                model.FileName = "this is a file." + ext;
                model.Directory = "NJ/Some/Dir/For/Taps/42/";

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
                var expectedPath = _tempDir + @"NJ\Some\Dir\For\Valves\42\this is a file." + ext;
                FileIO.DeleteIfFileExists(expectedPath);
                FileIO.WriteAllBytes(expectedPath, expectedData);

                var model = new ValveImage();
                model.FileName = "this is a file." + ext;
                model.Directory = "NJ/Some/Dir/For/Valves/42/";

                // Set this to null to ensure the repository isn't trying to read it
                // from the ImageData property.
                model.ImageData = null;

                _converter.Setup(x => x.Render(It.IsAny<IEnumerable<byte[]>>()))
                          .Callback<IEnumerable<byte[]>>(x => { MyAssert.AreEqual(expectedData, x.Single()); });

                Repository.GetImageDataAsPdf(model);
                _converter.Verify(x => x.Render(It.IsAny<IEnumerable<byte[]>>()));
            }
        }

        [TestMethod]
        public void TestGetImageDataReturnsPdfMadeUpOfMultipleFilesWhenFileNameIsABunchOfNumbersWithoutAFileExtension()
        {
            // Must be 8 characters long, no file extensions.
            var fileName1 = "11111111";
            var fileName2 = "22222222";
            var filePath1 = _tempDir + @"NJ\Some\Dir\For\Valves\42\" + fileName1 + ".tif";
            var filePath2 = _tempDir + @"NJ\Some\Dir\For\Valves\42\" + fileName2 + ".tif";
            var expectedFileData1 = new byte[] {1, 2, 3, 4, 5};
            var expectedFileData2 = new byte[] {4, 5, 6, 7, 8};

            FileIO.DeleteIfFileExists(filePath1);
            FileIO.DeleteIfFileExists(filePath2);

            FileIO.WriteAllBytes(filePath1, expectedFileData1);
            FileIO.WriteAllBytes(filePath2, expectedFileData2);

            var model = new ValveImage();
            model.FileName = fileName1 + fileName2;
            model.Directory = "NJ/Some/Dir/For/Valves/42/";

            _converter.Setup(x => x.Render(It.IsAny<IEnumerable<byte[]>>()))
                      .Callback<IEnumerable<byte[]>>(x => {
                           var results = x.ToArray();
                           MyAssert.AreEqual(expectedFileData1, results[0]);
                           MyAssert.AreEqual(expectedFileData2, results[1]);
                       });

            Repository.GetImageDataAsPdf(model);
            _converter.Verify(x => x.Render(It.IsAny<IEnumerable<byte[]>>()));
        }

        #region Reports

        #region GetValveImageLinks

        [TestMethod]
        public void TestValveImageLinksReturnsCorrectValveImageLinks()
        {
            var valve = GetFactory<ValveFactory>().Create();
            var image1 = GetFactory<ValveImageFactory>().Create(new {
                Valve = valve,
                IsDefaultImageForValve = true
            });
            var image2 = GetFactory<ValveImageFactory>().Create(new {
                Valve = valve,
                IsDefaultImageForValve = false
            });
            var image3 = GetFactory<ValveImageFactory>().Create(new {
                Valve = GetFactory<ValveFactory>().Create(new {SAPEquipmentId = 0}),
                IsDefaultImageForValve = false
            });

            var args = new TestValveImageLinks();

            var result = Repository.GetValveImageLinks(args).ToList();

            Assert.AreEqual(2, result.Count);
        }

        private class TestValveImageLinks : SearchSet<ValveImageLinkReportItem>
        {
            [SearchAlias("OperatingCenter", "opc", "Id")]
            public int? OperatingCenter { get; set; }
        }

        #endregion

        #endregion

        #endregion
    }
}
