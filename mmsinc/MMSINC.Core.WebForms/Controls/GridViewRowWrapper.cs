using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;

namespace MMSINC.Controls
{
    public class GridViewRowWrapper : IGridViewRow
    {
        #region Private Members

        private readonly GridViewRow _innerRow;

        #endregion

        #region Properties

        public bool Visible
        {
            get { return _innerRow.Visible; }
            set { _innerRow.Visible = value; }
        }

        public string ClientID
        {
            get { return _innerRow.ClientID; }
        }

        public bool EnableViewState
        {
            get { return _innerRow.EnableViewState; }
            set { _innerRow.EnableViewState = value; }
        }

        public string ID
        {
            get { return _innerRow.ID; }
            set { _innerRow.ID = value; }
        }

        #endregion

        #region Constructors

        public GridViewRowWrapper(GridViewRow row)
        {
            _innerRow = row;
        }

        #endregion

        #region Exposed Methods

        public void DataBind()
        {
            _innerRow.DataBind();
        }

        public string ResolveClientUrl(string url)
        {
            return _innerRow.ResolveClientUrl(url);
        }

        public Control FindControl(string id)
        {
            return _innerRow.FindControl(id);
        }

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            return
                ControlExtensions.FindControl
                    <TControl>(_innerRow, id);
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            return
                ControlExtensions.FindIControl
                    <TIControl>(_innerRow, id);
        }

        #endregion
    }

    public interface IGridViewRow : IControl { }
}
