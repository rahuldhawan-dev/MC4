using System;
using System.Web;
using MMSINC.Interface;
using StructureMap;

namespace MMSINC.Common
{
    public class HttpApplicationWrapper : IHttpApplicationWrapper
    {
        #region Private Members

        private HttpApplication _application;
        private IHttpContext _context;
        private readonly IContainer _container;

        #endregion

        #region Properties

        public virtual HttpApplication Application
        {
            get { return _application; }
            set { _application = value; }
        }

        public virtual IHttpContext CurrentContext
        {
            get
            {
                if (_context == null)
                    _context = _container.With(_application.Context).GetInstance<HttpContextWrapper>();
                return _context;
            }
        }

        #endregion

        #region Constructors

        public HttpApplicationWrapper(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Events

        public event EventHandler Error
        {
            add { _application.Error += value; }
            remove { _application.Error -= value; }
        }

        #endregion

        #region Exposed Methods

        public void Dispose()
        {
            _application.Dispose();
        }

        #endregion
    }
}
