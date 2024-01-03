using System;
using System.Web;

namespace MMSINC.Interface
{
    public interface IHttpApplicationWrapper : IDisposable
    {
        #region Properties

        HttpApplication Application { get; set; }
        IHttpContext CurrentContext { get; }

        #endregion

        #region Events

        event EventHandler Error;

        #endregion
    }
}
