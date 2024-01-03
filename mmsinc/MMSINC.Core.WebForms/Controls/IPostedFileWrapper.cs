namespace MMSINC.Controls
{
    public interface IPostedFileWrapper
    {
        int ContentLength { get; }
        string FileName { get; }
    }
}
