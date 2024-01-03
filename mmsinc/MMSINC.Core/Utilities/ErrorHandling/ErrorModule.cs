using System;
using System.Web;
using System.Web.Mvc;
using MMSINC.Common;
using MMSINC.Interface;
using StructureMap;

namespace MMSINC.Utilities.ErrorHandling
{
    public class ErrorModule : IHttpModule
    {
        #region Constants

        public const string SERVER_NAME_KEY = "Server_Name",
                            LOCALHOST = "localhost";

        #endregion

        #region Private Members

        protected IHttpApplicationWrapper _httpApplication;
        protected IErrorEmailer _errorEmailer;

        #endregion

        #region Constructors

        public ErrorModule()
        {
            _httpApplication = DependencyResolver.Current.GetService<HttpApplicationWrapper>();
            _errorEmailer = DependencyResolver.Current.GetService<ErrorEmailer>();
        }

        #endregion

        #region Event Handlers

        protected virtual void Application_Error(object sender, EventArgs e)
        {
            var context = _httpApplication.CurrentContext;
            if (context.Request.ServerVariables[SERVER_NAME_KEY] != LOCALHOST)
            {
                _errorEmailer.SendEmail(_httpApplication.CurrentContext);
            }
        }

        #endregion

        #region Exposed Methods

        public void Init(HttpApplication context)
        {
            _httpApplication.Application = context;
            _httpApplication.Error += Application_Error;
        }

        public void Dispose()
        {
            _httpApplication.Dispose();
        }

        #endregion
    }
}
