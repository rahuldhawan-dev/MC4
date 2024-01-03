using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public interface IListControl : IControl, IDataBoundControl
    {
        #region Properties

        int SelectedIndex { get; set; }
        object SelectedDataKey { get; }
        SortDirection SortDirection { get; }
        string SortExpression { get; }
        int PageSize { get; set; }
        int PageIndex { get; set; }

        #endregion

        #region Methods

        void SetSortDirection(SortDirection direction);

        #endregion
    }
}
