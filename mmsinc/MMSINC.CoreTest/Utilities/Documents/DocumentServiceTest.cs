using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Documents;

namespace MMSINC.CoreTest.Utilities.Documents
{
    [TestClass]
    public class DocumentServiceTest
    {
        #region Fields

        private DocumentService _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new DocumentService();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            try
            {
                Directory.Delete(_target.RootDirectory, true);
            }
            catch (DirectoryNotFoundException)
            {
                // Some tests don't write to the drive so the directory
                // is never created. No need to do anything here.
            }
        }

        #endregion

        #region Private Methods

        private string GetFileLocation(string hash)
        {
            return Path.Combine(_target.RootDirectory, hash.Substring(0, 2), hash) + ".file";
        }

        #endregion

        #region Tests

        #region GetFileHash

        [TestMethod]
        public void TestGetFileHashReturnsSHA1Hash()
        {
            var expectedBytes = new byte[] {1, 2, 3};
            string expectedHash;
            using (var sh1 = SHA1.Create())
            {
                var hash = sh1.ComputeHash(expectedBytes);
                var strHash = BitConverter.ToString(hash);
                expectedHash = strHash.Replace("-", "");
            }

            var result = _target.GetFileHash(expectedBytes);
            Assert.AreEqual(expectedHash, result);
        }

        [TestMethod]
        public void TestGetFileHashAlwaysReturnsTheSameHashForBinaryData()
        {
            var expectedBytes = new byte[] {1, 2, 3};
            // A new array to make sure the hash isn't being based on instance instead of value
            var expectedSimilarBytes = new byte[] {1, 2, 3};
            var otherBytes = new byte[] {4, 5, 6};

            Assert.AreEqual(_target.GetFileHash(expectedBytes), _target.GetFileHash(expectedBytes));
            Assert.AreEqual(_target.GetFileHash(expectedBytes), _target.GetFileHash(expectedSimilarBytes));
            Assert.AreNotEqual(_target.GetFileHash(expectedBytes), _target.GetFileHash(otherBytes));
        }

        #endregion

        #region Save

        [TestMethod]
        public void TestSaveSavesToExpectedLocation()
        {
            var file = new byte[] {1, 2, 3};
            var hash = _target.GetFileHash(file);
            var expected = GetFileLocation(hash);
            Assert.IsFalse(File.Exists(expected),
                "This file should not exist. If it exists then test cleanup failed somewhere.");

            _target.Save(file);
            Assert.IsTrue(File.Exists(expected));
        }

        [TestMethod]
        public void TestSaveDoesNotThrowExceptionWhenCalledForAFileThatAlreadyExists()
        {
            var file = new byte[] {1, 2, 3};

            // Save once
            _target.Save(file);

            // Save again
            MyAssert.DoesNotThrow(() => _target.Save(file));
        }

        [TestMethod]
        public void TestSaveDoesNotOverwriteExistingFile()
        {
            var file = new byte[] {1, 2, 3};
            var hash = _target.Save(file);
            var fileInfo = new FileInfo(GetFileLocation(hash));
            var expectedCreationTime = fileInfo.CreationTime;

            System.Threading.Thread.Sleep(500);

            _target.Save(file);
            fileInfo = new FileInfo(GetFileLocation(hash));
            Assert.AreEqual(expectedCreationTime, fileInfo.CreationTime);
        }

        #endregion

        #region Open

        [TestMethod]
        public void TestOpenReturnsTheSavedFile()
        {
            var file = new byte[] {1, 2, 3};
            var hash = _target.Save(file);

            var result = _target.Open(hash);
            MyAssert.AreEqual(file, result);
        }

        [TestMethod]
        public void TestOpenAlwaysReturnsANewByteArrayForIdenticalFiles()
        {
            var expectedFile = new byte[] {1, 2, 3};
            var hash = _target.Save(expectedFile);

            var firstResult = _target.Open(hash);
            var secondResult = _target.Open(hash);

            Assert.AreNotSame(expectedFile, firstResult);
            Assert.AreNotSame(expectedFile, secondResult);
            Assert.AreNotSame(firstResult, secondResult);

            MyAssert.AreEqual(expectedFile, firstResult);
            MyAssert.AreEqual(expectedFile, secondResult);
        }

        [TestMethod]
        public void TestOpenThrowsExceptionIfDirectoryDoesNotExist()
        {
            var file = new byte[] {1, 2, 3};
            var hash = _target.GetFileHash(file);
            MyAssert.Throws<DirectoryNotFoundException>(() => _target.Open(hash));
        }

        [TestMethod]
        public void TestOpenThrowsExceptionIfFileDoesNotExist()
        {
            var file = new byte[] {1, 2, 3};
            var hash = _target.GetFileHash(file);
            var subDir = Path.Combine(_target.RootDirectory, hash.Substring(0, 2));
            Directory.CreateDirectory(subDir);
            MyAssert.Throws<FileNotFoundException>(() => _target.Open(hash));
        }

        [TestMethod]
        public void TestOpenThrowsInvalidOperationExceptionForNullOrEmptyOrWhiteSpaceHashes()
        {
            MyAssert.Throws<InvalidOperationException>(() => _target.Open(null));
            MyAssert.Throws<InvalidOperationException>(() => _target.Open(string.Empty));
            MyAssert.Throws<InvalidOperationException>(() => _target.Open("     "));
        }

        [TestMethod]
        public void TestOpenThrowsInvalidOperationExceptionIfHashIsNotExactly40CharactersLong()
        {
            MyAssert.Throws<InvalidOperationException>(() => _target.Open("I am not 40 characters."));
        }

        #endregion

        #endregion
    }
}
