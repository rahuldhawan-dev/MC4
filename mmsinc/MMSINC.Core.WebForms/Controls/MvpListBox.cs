using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;

namespace MMSINC.Controls
{
    public class MvpListBox : ListBox, IListBox
    {
        #region Properties

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

        public List<TReturn> GetSelectedValues<TReturn>(Func<ListItem, TReturn> fn)
        {
            var indices = GetSelectedIndices();
            var len = indices.Length;

            if (len == 0) return null;

            var ret = new List<TReturn>();
            for (var i = 0; i < len; ++i)
            {
                ret.Add(fn(Items[indices[i]]));
            }

            return ret;
        }

        public List<int> GetSelectedValues()
        {
            return GetSelectedValues(li => Int32.Parse(li.Value));
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
