using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;

namespace MMSINC.Utilities.Documents
{
    public interface IDocumentService
    {
        /// <summary>
        /// Returns a unique key that identifies a file.
        /// </summary>
        /// <param name="binaryData"></param>
        /// <returns></returns>
        string GetFileHash(byte[] binaryData);

        /// <summary>
        /// Saves the binary data and returns a unique key for identifying the file.
        /// </summary>
        string Save(byte[] binaryData);

        byte[] Open(string hash);
    }

    // TODO: For testing purposes we probably want a CleanUp method that deletes the whole directory.

    /// <summary>
    /// DocumentService's purpose is solely for saving and retrieving document
    /// data that has been stored
    /// </summary>
    public class DocumentService : IDocumentService
    {
        #region Consts

        private const string DOCUMENT_DATA_DIRECTORY_CONFIGURATION_KEY = "DocumentDataDirectory";
        private const int HASH_LENGTH = 40;

        #endregion

        #region Fields

        private string _rootDir;

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the root directory where documents are stored.
        /// </summary>
        internal protected string RootDirectory
        {
            get
            {
                if (_rootDir == null)
                {
                    _rootDir = ConfigurationManager.AppSettings[DOCUMENT_DATA_DIRECTORY_CONFIGURATION_KEY];

                    if (string.IsNullOrWhiteSpace(_rootDir))
                    {
                        throw new InvalidOperationException("The configuration file is missing a value for the " +
                                                            DOCUMENT_DATA_DIRECTORY_CONFIGURATION_KEY + " key.");
                    }
                }

                return _rootDir;
            }
        }

        #endregion

        #region Private Methods

        private void EnsureDirectory(string hash)
        {
            var sub = GetSubDirectory(hash);
            Directory.CreateDirectory(sub);
        }

        private string GetSubDirectory(string hash)
        {
            var subDirName = hash.Substring(0, 2);
            return Path.Combine(RootDirectory, subDirName);
        }

        private string GetFileLocation(string hash)
        {
            var filePath = Path.Combine(GetSubDirectory(hash), hash) + ".file";
            return filePath;
        }

        protected void SaveCore(string hash, byte[] binaryData)
        {
            EnsureDirectory(hash);
            var filePath = GetFileLocation(hash);

            // Why's this in a try catch? Because there's a race condition
            // when calling File.Exists() and then File.Open. It's possible
            // for File.Exists to return false, another app creates the file,
            // and then File.Open throws cause the file was created before it
            // had the chance to. 
            try
            {
                using (var f = File.Open(filePath, FileMode.CreateNew))
                {
                    f.Write(binaryData, 0, binaryData.Length);
                }
            }
            catch (IOException ex)
            {
                // There's literally no other reasonably reliable way of checking
                // for a "File Already Exists" exception. Checking the message is 
                // flakey because we may one day switch to Lithuanian. Also this
                // HResult number is pretty ingrained into every other version of
                // Windows, it'll probably never change.
                const int fileAlreadyExistsHresult = -2147024816;
                var hresult = System.Runtime.InteropServices.Marshal.GetHRForException(ex);
                if (hresult != fileAlreadyExistsHresult)
                {
                    throw;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the SHA1 hash for a file's contents.
        /// </summary>
        public string GetFileHash(byte[] binaryData)
        {
            using (var sh1 = SHA1.Create())
            {
                var hash = sh1.ComputeHash(binaryData);
                var strHash = BitConverter.ToString(hash);
                strHash = strHash.Replace("-", "");
                return strHash;
            }
        }

        /// <summary>
        /// Saves the binary data to the file system and returns the file hash. If
        /// the file already exists locally, then no saving takes place.
        /// </summary>
        public string Save(byte[] binaryData)
        {
            var hash = GetFileHash(binaryData);
            SaveCore(hash, binaryData);
            return hash;
        }

        /// <summary>
        /// Returns the file that matches the given hash. This method will always return
        /// a new byte array instance.
        /// </summary>
        /// <param name="hash">the SH1 hash for a given a file</param>
        /// <returns></returns>
        public byte[] Open(string hash)
        {
            // This could potentially become painful if we're suddenly shooting out
            // tons of documents or the average file size goes way up(it's about 256kb
            // as of 12/6/2013). If that ends up happening, we'll want an OpenStream
            // type of solution so ASP can handle things without eating up memory.

            if (string.IsNullOrWhiteSpace(hash) || hash.Length != HASH_LENGTH)
            {
                throw new InvalidOperationException(
                    "The hash provided is null or otherwise does not meet the hash length requirements.");
            }

            return File.ReadAllBytes(GetFileLocation(hash));
        }

        #endregion
    }
}
