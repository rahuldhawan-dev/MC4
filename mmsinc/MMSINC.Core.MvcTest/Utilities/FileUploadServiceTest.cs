using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Metadata;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Web.Mvc;
using IDateTimeProvider = MMSINC.Utilities.IDateTimeProvider;

namespace MMSINC.Core.MvcTest.Utilities
{
    [TestClass]
    public class FileUploadServiceTest
    {
        #region Fields

        private FileUploadService _target;
        private TempDataDictionary _tempData;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container(e => e.For<IDateTimeProvider>().Use(new Mock<IDateTimeProvider>().Object));
            _tempData = new TempDataDictionary();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _container.Inject(_dateTimeProvider.Object);
            _target = _container.With(_tempData).GetInstance<FileUploadService>();
        }

        [TestCleanup]
        public void CleanupTest() { }

        #endregion

        #region Tests

        [TestMethod]
        public void TestConstructorThrowsForNullTempDataArgument()
        {
            MyAssert.Throws<ArgumentNullException>(() => new FileUploadService(null, null));
        }

        [TestMethod]
        public void TestStoreFileAndSetKeyThrowsForNullFile()
        {
            MyAssert.Throws<ArgumentNullException>(() => _target.StoreFileAndSetKey(null));
        }

        [TestMethod]
        public void TestStoreFileAndSetKeySetsKeyToANewGuid()
        {
            var file = new AjaxFileUpload();
            Assert.AreEqual(Guid.Empty, file.Key);
            _target.StoreFileAndSetKey(file);
            Assert.AreNotEqual(Guid.Empty, file.Key);

            var file2 = new AjaxFileUpload();
            Assert.AreEqual(Guid.Empty, file2.Key);
            _target.StoreFileAndSetKey(file2);
            Assert.AreNotEqual(Guid.Empty, file2.Key);
            Assert.AreNotEqual(file.Key, file2.Key);
        }

        [TestMethod]
        public void TestStoreFileAndSetKeyThrowsExceptionIfFileAlreadyHasKey()
        {
            var file = new AjaxFileUpload();
            _target.StoreFileAndSetKey(file);
            MyAssert.Throws<InvalidOperationException>(() => _target.StoreFileAndSetKey(file));
        }

        [TestMethod]
        public void TestStoreFileAndSetKeyClearsExpiredFiles()
        {
            var uploadTime = new DateTime(2013, 11, 11, 12, 0, 0);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(uploadTime);
            var file = new AjaxFileUpload();
            _target.StoreFileAndSetKey(file);
            Assert.IsTrue(_target.HasFile(file.Key));

            _dateTimeProvider.Setup(x => x.GetCurrentDate())
                             .Returns(uploadTime + FileUploadService.UPLOAD_EXPIRATION_TIMEOUT);

            _target = _container.With(_tempData).GetInstance<FileUploadService>();
            Assert.IsFalse(_target.HasFile(file.Key), "The first file should now be removed from tempdata.");
        }

        [TestMethod]
        public void TestHasFileReturnsFalseIfNoKeyExistsInTempData()
        {
            Assert.IsFalse(_target.HasFile(Guid.NewGuid()));
        }

        [TestMethod]
        public void TestHasFileReturnsFalseIfKeyExistInTempDataButIsNull()
        {
            var key = Guid.NewGuid();
            _tempData[key.ToString()] = null;
            Assert.IsFalse(_target.HasFile(key));
        }

        [TestMethod]
        public void TestHasFileReturnsFalseIfKeyExistsInTempDataAndIsNotAjaxFileUploadType()
        {
            var key = Guid.NewGuid();
            _tempData[key.ToString()] = "I am a string and not an AjaxFileUpload instance";
            Assert.IsFalse(_target.HasFile(key));
        }

        [TestMethod]
        public void TestHasFileReturnsFalseIfKeyExistsButUploadHasExpired()
        {
            var uploadTime = new DateTime(2013, 11, 11, 12, 0, 0);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(uploadTime);
            var file = new AjaxFileUpload();
            _target.StoreFileAndSetKey(file);

            _dateTimeProvider.Setup(x => x.GetCurrentDate())
                             .Returns(uploadTime + FileUploadService.UPLOAD_EXPIRATION_TIMEOUT);

            // Create a new target so that the cleaner can run.
            _target = _container.With(_tempData).GetInstance<FileUploadService>();
            Assert.IsFalse(_target.HasFile(file.Key), "The first file should now be removed from tempdata.");
        }

        [TestMethod]
        public void TestHasFileReturnsTrueIfKeyExistsAndIsAjaxFileUpload()
        {
            var file = new AjaxFileUpload();
            _target.StoreFileAndSetKey(file);
            Assert.IsTrue(_target.HasFile(file.Key));
        }

        [TestMethod]
        public void TestGetFileThrowsExceptionIfNoKeyExistsInTempData()
        {
            MyAssert.Throws<InvalidOperationException>(() => _target.GetFile(Guid.NewGuid()));
        }

        [TestMethod]
        public void TestGetFileThrowsExceptionIfKeyExistsInTempDataButIsNull()
        {
            var key = Guid.NewGuid();
            _tempData[key.ToString()] = null;
            MyAssert.Throws<InvalidOperationException>(() => _target.GetFile(key));
        }

        [TestMethod]
        public void TestGetFileThrowsExceptionIfKeyExistInTempDataButObjectIsNotAjaxFileUploadInstance()
        {
            var key = Guid.NewGuid();
            _tempData[key.ToString()] = "I am a string and not an AjaxFileUpload instance";
            MyAssert.Throws<InvalidOperationException>(() => _target.GetFile(key));
        }

        [TestMethod]
        public void TestGetFileThrowsExceptionIfKeyExistsButUploadIsExpired()
        {
            var uploadTime = new DateTime(2013, 11, 11, 12, 0, 0);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(uploadTime);
            var file = new AjaxFileUpload();
            _target.StoreFileAndSetKey(file);

            _dateTimeProvider.Setup(x => x.GetCurrentDate())
                             .Returns(uploadTime + FileUploadService.UPLOAD_EXPIRATION_TIMEOUT);

            // Create a new target so that the cleaner can run.
            _target = _container.With(_tempData).GetInstance<FileUploadService>();
            MyAssert.Throws<InvalidOperationException>(() => _target.GetFile(file.Key));
        }

        [TestMethod]
        public void TestGetFileReturnsStoredFileIfKeyExistsInTempDataAndObjectIsAjaxFileUploadInstance()
        {
            var file = new AjaxFileUpload();
            _target.StoreFileAndSetKey(file);
            var result = _target.GetFile(file.Key);
            Assert.AreSame(file, result);
        }

        #endregion
    }
}
