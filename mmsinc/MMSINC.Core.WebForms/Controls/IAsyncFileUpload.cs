using System.IO;

namespace MMSINC.Controls
{
    public interface IAsyncFileUpload : IControl
    {
        byte[] FileBytes { get; }
        Stream FileContent { get; }
        IPostedFileWrapper IPostedFile { get; }
    }
}
