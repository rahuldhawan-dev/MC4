using System;
using System.Collections.Specialized;
using System.Web;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class RequestWrapper : IRequest
    {
        #region Constants

        public struct ServerVariableKeys
        {
            public const string URL = "URL",
                                SERVER_NAME = "SERVER_NAME";
        }

        #endregion

        #region Private Members

        private HttpRequest _innerRequest;
        private HttpRequestBase _innerRequestBase;

        #endregion

        #region Properties

        protected dynamic InnerRequest
        {
            get { return (object)_innerRequest ?? _innerRequestBase; }
        }

        public Uri Uri
        {
            get { return InnerRequest.Url; }
        }

        public NameValueCollection Form
        {
            get { return InnerRequest.Form; }
        }

        public string Url
        {
            get { return ServerVariables[ServerVariableKeys.URL]; }
        }

        public string ServerName
        {
            get { return ServerVariables[ServerVariableKeys.SERVER_NAME]; }
        }

        public IQueryString IQueryString
        {
            get { return new QueryStringWrapper(InnerRequest.QueryString); }
        }

        public string Querystring
        {
            get { return InnerRequest.QueryString.ToString(); }
        }

        public string RelativeUrl
        {
            get { return Url + "?" + Querystring; }
        }

        public string RawUrl
        {
            get { return InnerRequest.RawUrl; }
        }

        public NameValueCollection ServerVariables
        {
            get { return InnerRequest.ServerVariables; }
        }

        public NameValueCollection Params
        {
            get { return InnerRequest.Params; }
        }

        public string HttpMethod
        {
            get { return InnerRequest.HttpMethod; }
        }

        #endregion

        #region Constructors

        public RequestWrapper(HttpRequest innerRequest)
        {
            _innerRequest = innerRequest;
        }

        public RequestWrapper(HttpRequestBase innerRequest)
        {
            _innerRequestBase = innerRequest;
        }

        #endregion
    }
}
