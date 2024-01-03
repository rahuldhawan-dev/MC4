using System;
using System.Collections.Generic;

namespace MapCall.Common.Utility
{
    // The ASP libraries have this nice VirtualPath class that would normally do this for us.
    // Except it's internal. So we can't use it.
    // Bastards.

    public static class PathHelper
    {
        private static string Combine(params string[] pathPieces)
        {
            var pieces = new List<string>();

            foreach (var p in pathPieces)
            {
                // This is done to split up a parameter that already has nested directories in it. Ie "dir/dir/dir/dir"
                pieces.AddRange(p.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries));
            }

            return "/" + string.Join("/", pieces.ToArray());
        }

        public static string CombineDirectory(params string[] pathPieces)
        {
            return Combine(pathPieces) + "/";
        }

        public static string CombineFile(params string[] pathPieces)
        {
            return Combine(pathPieces);
        }
    }
}
