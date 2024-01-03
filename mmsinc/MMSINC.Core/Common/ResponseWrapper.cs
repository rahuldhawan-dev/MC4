using System.Web;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class ResponseWrapper : IResponse
    {
        #region Private Members

        private readonly HttpResponse _innerResponse;

        private HttpCachePolicyWrapper _cacheWrapper;
        private HttpResponseBase _innerResponseBase;

        #endregion

        #region Properties

        protected dynamic InnerResponse
        {
            get { return (object)_innerResponse ?? _innerResponseBase; }
        }

        public IHttpCachePolicy Cache
        {
            get
            {
                if (_cacheWrapper == null)
                {
                    _cacheWrapper = new HttpCachePolicyWrapper(InnerResponse.Cache);
                }

                return _cacheWrapper;
            }
        }

        public string Charset
        {
            get { return InnerResponse.Charset; }
            set { InnerResponse.Charset = value; }
        }

        public string ContentType
        {
            get { return InnerResponse.ContentType; }
            set { InnerResponse.ContentType = value; }
        }

        public bool IsClientConnected
        {
            get { return InnerResponse.IsClientConnected; }
        }

        public bool IsRequestBeingRedirected
        {
            get { return InnerResponse.IsRequestBeingRedirected; }
        }

        #endregion

        #region Constructors

        public ResponseWrapper(HttpResponse response)
        {
            _innerResponse = response;
        }

        public ResponseWrapper(HttpResponseBase response)
        {
            _innerResponseBase = response;
        }

        #endregion

        #region Exposed Methods

        public void Redirect(string url)
        {
            InnerResponse.Redirect(url);
        }

        public void Redirect(string url, bool endResponse)
        {
            InnerResponse.Redirect(url, endResponse);
        }

        public void End()
        {
            InnerResponse.End();
        }

        public void BinaryWrite(byte[] bytes)
        {
            InnerResponse.BinaryWrite(bytes);
        }

        public void Write(string text)
        {
            InnerResponse.Write(text);
        }

        public void AddHeader(string headerName, string headerValue)
        {
            InnerResponse.AddHeader(headerName, headerValue);
        }

        public void Clear()
        {
            InnerResponse.Clear();
        }

        #endregion
    }
}
