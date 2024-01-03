using System;
using System.IO;

namespace MMSINC.Utilities
{
    /// <summary>
    /// Encapsulates binary data into a temporary file saved to the file system. The file is
    /// then deleted when the TemporaryFile object is disposed.
    /// </summary>
    public class TemporaryFile : IDisposable
    {
        #region Properties

        /// <summary>
        /// Returns the location of the temporary file.
        /// </summary>
        public string FilePath { get; protected set; }

        public FileInfo FileInfo
        {
            get { return new FileInfo(FilePath); }
        }

        #endregion

        #region Fields

        private bool _isDisposed;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a temporary file on the file system with the given binary data.
        /// </summary>
        /// <param name="binaryData">The data for the file</param>
        /// <param name="fileExt">If the file path requires a specific extension(like when OleDB reads excel files), set this. Otherwise leave it null.</param>
        public TemporaryFile(byte[] binaryData, string fileExt = null)
        {
            if (binaryData == null)
            {
                throw new InvalidOperationException("BinaryData may not be null.");
            }

            FilePath = GetFilePath(fileExt);

            File.WriteAllBytes(FilePath, binaryData);
        }

        public TemporaryFile(Stream inputStream, string fileExt = null)
        {
            if (inputStream == null)
            {
                throw new InvalidOperationException("inputStream may not be null.");
            }

            FilePath = GetFilePath(fileExt);

            using (var file = File.Create(FilePath))
            {
                inputStream.CopyTo(file);

                file.Close();
            }
        }

        private static string GetFilePath(string fileExt)
        {
            var tempDir = Path.GetTempPath();
            var randomFile = Path.GetRandomFileName();

            if (!string.IsNullOrWhiteSpace(fileExt))
            {
                randomFile += "." + fileExt;
            }

            return Path.Combine(tempDir, randomFile);
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            try
            {
                if (!_isDisposed)
                {
                    File.Delete(FilePath);
                }
            }
            finally
            {
                _isDisposed = true;
            }
        }

        private static TReturn WithTemporaryFile<TReturn>(TemporaryFile tmp, Func<TemporaryFile, TReturn> fn)
        {
            TReturn ret;

            using (tmp)
            {
                ret = fn(tmp);
            }

            return ret;
        }

        public static TReturn WithTemporaryFile<TReturn>(byte[] binaryData, Func<TemporaryFile, TReturn> fn,
            string fileExt = null)
        {
            return WithTemporaryFile(new TemporaryFile(binaryData, fileExt), fn);
        }

        public static TReturn WithTemporaryFile<TReturn>(Stream inputStream, Func<TemporaryFile, TReturn> fn,
            string fileExt = null)
        {
            return WithTemporaryFile(new TemporaryFile(inputStream, fileExt), fn);
        }

        #endregion
    }
}
