using System;
using System.Collections.Specialized;
using System.Web;

namespace MMSINC.Testing
{
    public class StubHttpContextForRouting : HttpContextBase
    {
        #region Private Members

        private readonly StubHttpRequestForRouting _request;
        private readonly StubHttpResponseForRouting _response;

        #endregion

        #region Properties

        public override HttpRequestBase Request
        {
            get { return _request; }
        }

        public override HttpResponseBase Response
        {
            get { return _response; }
        }

        #endregion

        #region Constructors

        public StubHttpContextForRouting(string appPath = "/", string requestUrl = "~/")
        {
            _request = new StubHttpRequestForRouting(appPath, requestUrl);
            _response = new StubHttpResponseForRouting();
        }

        #endregion
    }

    public class StubHttpRequestForRouting : HttpRequestBase
    {
        #region Private Members

        private readonly string _appPath, _requestUrl;

        #endregion

        #region Properties

        public override string ApplicationPath
        {
            get { return _appPath; }
        }

        public override string AppRelativeCurrentExecutionFilePath
        {
            get { return _requestUrl; }
        }

        public override string PathInfo
        {
            get { return String.Empty; }
        }

        public override NameValueCollection ServerVariables
        {
            get { return new NameValueCollection(); }
        }

        #endregion

        #region Constructors

        public StubHttpRequestForRouting(string appPath, string requestUrl)
        {
            _appPath = appPath;
            _requestUrl = requestUrl;
        }

        #endregion
    }

    public class StubHttpResponseForRouting : HttpResponseBase
    {
        #region Exposed Methods

        public override string ApplyAppPathModifier(string virtualPath)
        {
            return virtualPath;
        }

        #endregion
    }
}
