using System;
using System.IO;
using System.Web;

namespace MMSINC.Interface
{
    public interface IServer
    {
        #region Exposed Methods

        string UrlEncode(string source);
        string MapPath(string path);
        Exception GetLastError();
        void Execute(IHttpHandler handler, TextWriter writer, bool preserveForm);

        #endregion
    }
}
