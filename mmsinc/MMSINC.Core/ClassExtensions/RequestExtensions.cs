using System;
using System.Web;

namespace MMSINC.ClassExtensions
{
    public static class RequestExtensions
    {
        #region Extension Methods

        public static string GetRootUrl(this HttpRequestBase request)
        {
            var uri = request.Url;
            return String.Format("{0}{1}{2}:{3}", uri.Scheme,
                Uri.SchemeDelimiter, uri.Host, uri.Port);
        }

        #endregion
    }
}
