namespace MMSINC.Controls
{
    public interface IFileUpload : IControl
    {
        IPostedFileWrapper WrappedPostedFile { get; }
    }
}
