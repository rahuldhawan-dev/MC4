using System;
using System.IO;
using System.Web;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class ServerWrapper : IServer
    {
        #region Private Members

        private HttpServerUtility _innerServer;
        private HttpServerUtilityBase _innerServerBase;

        #endregion

        #region Properties

        protected dynamic InnerServer
        {
            get { return (object)_innerServer ?? _innerServerBase; }
        }

        #endregion

        #region Constructors

        public ServerWrapper(HttpServerUtility server)
        {
            _innerServer = server;
        }

        public ServerWrapper(HttpServerUtilityBase server)
        {
            _innerServerBase = server;
        }

        #endregion

        #region Exposed Methods

        public string UrlEncode(string source)
        {
            return InnerServer.UrlEncode(source);
        }

        public string MapPath(string path)
        {
            return InnerServer.MapPath(path);
        }

        public Exception GetLastError()
        {
            return InnerServer.GetLastError();
        }

        public void Execute(IHttpHandler handler, TextWriter writer, bool preserveForm)
        {
            InnerServer.Execute(handler, writer, preserveForm);
        }

        #endregion
    }
}
