using System.Collections;
using System.Web;

namespace MMSINC.Interface
{
    public interface IHttpContext
    {
        #region Properties

        IHttpHandler Handler { get; set; }
        IResponse Response { get; }
        IRequest Request { get; }
        IServer Server { get; }
        IUser User { get; }
        IDictionary Items { get; }

        #endregion
    }
}
