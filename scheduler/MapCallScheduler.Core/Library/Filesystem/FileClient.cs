using System.Collections.Generic;
using System.IO;
using System.Text;
using MapCallScheduler.Library.Common;
using MMSINC.ClassExtensions;

namespace MapCallScheduler.Library.Filesystem
{
    public class FileClient : IFileClient
    {
        public IEnumerable<FileInfo> GetListing(string path, string searchPattern = null)
        {
            foreach (var file in (searchPattern == null
                ? Directory.GetFiles(path)
                : Directory.GetFiles(path, searchPattern)))
            {
                yield return new FileInfo(file);
            }
        }

        private static Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }

        public FileData DownloadFile(string path)
        {
            // not sure why, but the whole stream seems to get cut off if we try to go at it directly
            var streamCopy = new MemoryStream();
            using (var stream = OpenRead(path))
            {
                stream.CopyToAndReset(streamCopy);
            }

            var bytes = streamCopy.ToByteArray();

            return new FileData(path, Encoding.UTF8.GetString(bytes), bytes);
        }

        public void DeleteFile(string file)
        {
            File.Delete(file);
        }

        public void WriteFile(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }
    }

    public interface IFileClient
    {
        IEnumerable<FileInfo> GetListing(string path, string searchPattern = null);
        FileData DownloadFile(string path);
        void DeleteFile(string file);
        void WriteFile(string path, string contents);
    }
}