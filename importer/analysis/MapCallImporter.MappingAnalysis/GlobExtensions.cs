using System.Collections.Generic;
using System.IO;
using System.Linq;
using Glob;

namespace MapCallImporter.MappingAnalysis
{
    public static class GlobExtensions
    {
        public static IEnumerable<FileInfo> GlobFiles(this DirectoryInfo di, string pattern, SearchOption options)
        {
            var glob = new Glob.Glob(pattern, GlobOptions.Compiled);

            return di.EnumerateFiles("*", options).Where(directory => glob.IsMatch(directory.FullName));
        }
    }
}