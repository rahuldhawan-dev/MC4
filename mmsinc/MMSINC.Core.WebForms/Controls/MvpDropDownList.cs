using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;

namespace MMSINC.Controls
{
    public class MvpDropDownList : DropDownList, IDropDownList
    {
        #region Properties

        public bool ConvertEmptyStringToNull { get; set; }

        public override ListItem SelectedItem
        {
            get
            {
                var item = base.SelectedItem;
                if (ConvertEmptyStringToNull && item.Value == String.Empty)
                    item = new ListItem(item.Text, null);
                return item;
            }
        }

        public object DataItem
        {
            get
            {
                var dataItemContainer = (IDataItemContainer)DataItemContainer;
                return dataItemContainer != null ? dataItemContainer.DataItem : null;
            }
        }

        public override string SelectedValue
        {
            get
            {
                var val = base.SelectedValue;
                return (ConvertEmptyStringToNull && val == String.Empty)
                    ? null
                    : val;
            }
            set { base.SelectedValue = value; }
        }

        public object SelectedDataKey
        {
            get { return SelectedValue; }
        }

        public SortDirection SortDirection
        {
            get { return SortDirection.Ascending; }
        }

        public string SortExpression
        {
            get { return null; }
        }

        public int PageSize
        {
            get { return -1; }
            set { }
        }

        public int PageIndex
        {
            get { return -1; }
            set { }
        }

        public string[] DataKeyNames
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        #region Exposed Methods

        public TReturn GetSelectedValue<TReturn>(Func<ListItem, TReturn> fn)
        {
            return
                ControlExtensions.GetSelectedValue(this, fn);
        }

        public int? GetSelectedValue()
        {
            return
                ControlExtensions.GetSelectedValue(this);
        }

        public bool? GetBooleanValue()
        {
            return
                ControlExtensions.GetBooleanValue(this);
        }

        public string GetStringValue()
        {
            return
                ControlExtensions.GetStringValue(this);
        }

        public void SetSortDirection(SortDirection direction)
        {
            throw new InvalidOperationException();
        }

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            return
                ControlExtensions.FindControl
                    <TControl>(this, id);
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            return
                ControlExtensions.FindIControl
                    <TIControl>((Control)this, id);
        }

        #endregion
    }
}
