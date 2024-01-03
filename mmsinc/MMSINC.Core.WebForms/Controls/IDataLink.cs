namespace MMSINC.Controls
{
    /// <summary>
    /// Interface for controls likes Notes, Documents, and HyperLinks.
    /// </summary>
    public interface IDataLink
    {
        int DataLinkID { get; set; }
        int DataTypeID { get; set; }

        bool AllowAdd { get; set; }
        bool AllowEdit { get; set; }
        bool AllowDelete { get; set; }
        bool Visible { get; set; }
    }
}
