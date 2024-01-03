using System;
using System.Collections.Specialized;

namespace MMSINC.Interface
{
    public interface IRequest
    {
        #region Properties

        NameValueCollection ServerVariables { get; }
        NameValueCollection Params { get; }
        NameValueCollection Form { get; }
        string Url { get; }
        Uri Uri { get; }
        string ServerName { get; }
        string Querystring { get; }
        IQueryString IQueryString { get; }
        string RelativeUrl { get; }
        string RawUrl { get; }
        string HttpMethod { get; }

        #endregion
    }
}
