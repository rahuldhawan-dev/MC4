using System;
using System.Web;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class HttpCachePolicyWrapper : IHttpCachePolicy
    {
        #region Fields

        protected HttpCachePolicy _innerCachePolicy;

        #endregion

        #region Constructors

        public HttpCachePolicyWrapper(HttpCachePolicy cachePolicy)
        {
            _innerCachePolicy = cachePolicy;
        }

        #endregion

        #region Public Methods

        public void SetExpires(DateTime date)
        {
            _innerCachePolicy.SetExpires(date);
        }

        #endregion
    }
}
