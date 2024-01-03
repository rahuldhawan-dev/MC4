using System;
using System.IO;

namespace MMSINC.Utilities
{
    /// <summary>
    /// Utility class for File/Directory IO stuff.
    /// </summary>
    public static class FileIO
    {
        /// <summary>
        /// Deletes a single file if the file exists.
        /// </summary>
        /// <param name="filePath"></param>
        /// <summary>
        /// 
        /// This is here because File.Delete() is not consistent.
        /// File.Delete throws an exception if any directory in
        /// the path does not exist, but it does not throw if the 
        /// file itself does not exist.
        /// 
        /// </summary>
        public static void DeleteIfFileExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Will create a directory and all subdirectories in a path if
        /// they do not currently exist. If the path includes a file name,
        /// the file name will be removed. Returns a DirectoryInfo object
        /// for the created or existing directory.
        /// </summary>
        /// <param name="path"></param>
        public static DirectoryInfo EnsureDirectoryExists(string path)
        {
            // We need to use GetDirectoryName because Directory.CreateDirectory
            // will consider the file name at the end of the path as another 
            // directory, creating a directory with the name of the file.
            var dir = Path.GetDirectoryName(path);
            // ReSharper disable once AssignNullToNotNullAttribute
            return Directory.CreateDirectory(dir);
        }

        /// <summary>
        /// Returns a randomized temporary file name in the current user's temp directory.
        /// This does NOT create the file itself.
        /// </summary>
        /// <returns></returns>
        public static string GetRandomTemporaryFileName()
        {
            // This uses GetRandomFileName instead of GetTempFileName because GetTempFileName
            // seems to sequentially create the file names and causes issues in multi-threaded
            // situations where the temp file gets deleted/renamed.
            return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }

        /// <summary>
        /// Writes all of the bytes to a file for the given path. If the directory or
        /// sub directories do not exist, they will be created. If a file already
        /// exists, it will be overwritten.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="bytes"></param>
        public static void WriteAllBytes(string path, byte[] bytes)
        {
            EnsureDirectoryExists(path);
            File.WriteAllBytes(path, bytes);
        }
    }
}
