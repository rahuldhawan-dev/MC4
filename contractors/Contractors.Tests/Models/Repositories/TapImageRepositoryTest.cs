using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using Moq;
using StructureMap;
using TapImageRepository = Contractors.Data.Models.Repositories.TapImageRepository;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class TapImageRepositoryTest : ContractorsControllerTestBase<TapImage, TapImageRepository>
    {
        #region Fields

        private string _tempDir = Path.GetTempPath();
        private Mock<IImageToPdfConverter> _converter;

        #endregion

        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            _converter = i.For<IImageToPdfConverter>().Mock();
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            ConfigurationManager.AppSettings["ImageUploadRootDirectory"] = _tempDir;
            ConfigurationManager.AppSettings["ImageUploadTapDirectory"] = @"Some\Dir\For\Taps";
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ConfigurationManager.AppSettings["ImageUploadRootDirectory"] = null;
            ConfigurationManager.AppSettings["ImageUploadTapDirectory"] = null;
        }

        #endregion

        [TestMethod]
        public void TestDeleteThrowsNotSupportedException()
        {
            var model = new TapImage();
            MyAssert.Throws<NotSupportedException>(() => Repository.Delete(model));
        }

        [TestMethod]
        public void TestGetImageDataAsPdfThrowsNotSupportedExceptionWhenFileNameHasUnknownExtension()
        {
            var model = new TapImage();
            model.FileName = "something.mp3";
            MyAssert.Throws<NotSupportedException>(() => Repository.GetImageDataAsPdf(model));
        }

        [TestMethod]
        public void TestGetImageDataAsPdfReturnsFileDataAsIsIfFileNameEndsWithPdfRegardlessOfCase()
        {
            var exts = new[] { "pdf", "PDF" };
            var expectedData = new byte[] { 1, 2, 3, 4, 5 };
            var expectedPath = _tempDir + @"NJ\Some\Dir\For\Taps\42\this is a file.pdf";
            FileIO.DeleteIfFileExists(expectedPath);
            FileIO.WriteAllBytes(expectedPath, expectedData);

            foreach (var ext in exts)
            {
                var model = new TapImage();
                model.FileName = "this is a file." + ext;
                model.Directory = "NJ/Some/Dir/For/Taps/42/";

                var result = Repository.GetImageDataAsPdf(model);
                MyAssert.AreEqual(expectedData, result);
            }
        }

        [TestMethod]
        public void TestGetImageDataAsPdfReturnsSingleTifFileConvertedToPdfIfFileNameEndsInTifOrTiffRegardlesOfCase()
        {
            var exts = new[] { "tif", "TIF", "tiff", "TIFF" };
            var expectedData = new byte[] { 1, 2, 3, 4, 5 };
            foreach (var ext in exts)
            {
                var expectedPath = _tempDir + @"NJ\Some\Dir\For\Taps\42\this is a file." + ext;
                FileIO.DeleteIfFileExists(expectedPath);
                FileIO.WriteAllBytes(expectedPath, expectedData);

                var model = new TapImage();
                model.FileName = "this is a file." + ext;
                model.Directory = "NJ/Some/Dir/For/Taps/42/";

                // Set this to null to ensure the repository isn't trying to read it
                // from the ImageData property.
                model.ImageData = null;

                _converter.Setup(x => x.Render(It.IsAny<IEnumerable<byte[]>>()))
                    .Callback<IEnumerable<byte[]>>(x =>
                    {
                        MyAssert.AreEqual(expectedData, x.Single());
                    });

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
            var filePath1 = _tempDir + @"NJ\Some\Dir\For\Taps\42\" + fileName1 + ".tif";
            var filePath2 = _tempDir + @"NJ\Some\Dir\For\Taps\42\" + fileName2 + ".tif";
            var expectedFileData1 = new byte[] { 1, 2, 3, 4, 5 };
            var expectedFileData2 = new byte[] { 4, 5, 6, 7, 8 };

            FileIO.DeleteIfFileExists(filePath1);
            FileIO.DeleteIfFileExists(filePath2);

            FileIO.WriteAllBytes(filePath1, expectedFileData1);
            FileIO.WriteAllBytes(filePath2, expectedFileData2);

            var model = new TapImage();
            model.FileName = fileName1 + fileName2;
            model.Directory = "NJ/Some/Dir/For/Taps/42/";

            _converter.Setup(x => x.Render(It.IsAny<IEnumerable<byte[]>>()))
                .Callback<IEnumerable<byte[]>>(x =>
                {
                    var results = x.ToArray();
                    MyAssert.AreEqual(expectedFileData1, results[0]);
                    MyAssert.AreEqual(expectedFileData2, results[1]);
                });

            Repository.GetImageDataAsPdf(model);
            _converter.Verify(x => x.Render(It.IsAny<IEnumerable<byte[]>>()));
        }

        [TestMethod]
        public void GetTapImagesForWorkOrderReturnsTapImagesForWorkOrder()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var premNum = "87654321";
            var servNum = "987654321";
            var workOrder = GetFactory<WorkOrderFactory>().Create(new
            {
                AssignedContractor = _currentUser.Contractor,
                PremiseNumber = premNum,
                ServiceNumber = servNum
            });

            var expected = GetFactory<TapImageFactory>().CreateArray(3, new {PremiseNumber = premNum, ServiceNumber = servNum, Town = town});
            var notExpected = GetFactory<TapImageFactory>().CreateArray(3, new { PremiseNumber = "12", ServiceNumber = "34", Town = town });

            _currentUser.Contractor.OperatingCenters.Add(operatingCenter);
            operatingCenter.Towns.Add(town);
            Session.Flush();

            var actual = Repository.GetTapImagesForWorkOrder(workOrder).ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].PremiseNumber, actual[i].PremiseNumber);
                Assert.AreEqual(expected[i].ServiceNumber, actual[i].ServiceNumber);
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }
    }
}
