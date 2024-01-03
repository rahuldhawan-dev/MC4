using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public interface IDropDownList : IListControl
    {
        #region Properties

        bool AppendDataBoundItems { get; set; }
        string SelectedValue { get; }
        string DataTextField { get; set; }
        string DataValueField { get; set; }
        ListItemCollection Items { get; }
        ListItem SelectedItem { get; }
        object DataItem { get; }

        #endregion

        #region Events

        event EventHandler DataBound, DataBinding;

        #endregion

        #region Methods

        TReturn GetSelectedValue<TReturn>(Func<ListItem, TReturn> fn);
        int? GetSelectedValue();
        bool? GetBooleanValue();
        string GetStringValue();

        #endregion
    }
}
