using System;

namespace MMSINC.ClassExtensions
{
    public static class UriExtensions
    {
        /// <summary>
        /// Returns the AbsoluteUri with the Query removed from it if it exists.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetAbsoluteUriWithoutQuery(this Uri uri)
        {
            if (uri == null)
            {
                throw new NullReferenceException("uri");
            }

            var query = uri.Query;
            var absUri = uri.AbsoluteUri;
            return ((String.IsNullOrEmpty(query)) ? absUri : absUri.Replace(query, string.Empty));
        }
    }
}
