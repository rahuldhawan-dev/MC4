using System;

namespace MMSINC.Interface
{
    public interface IHttpCachePolicy
    {
        #region Methods

        void SetExpires(DateTime date);

        #endregion
    }
}
