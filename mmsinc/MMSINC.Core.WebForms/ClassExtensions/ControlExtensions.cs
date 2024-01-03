using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Interface;
using MenuItem = System.Web.UI.WebControls.MenuItem;

namespace MMSINC.ClassExtensions
{
    public static class ControlExtensions
    {
        #region Constants

        private struct ValueStrings
        {
            public static readonly string[] TRUE_VALUES = {
                "yes",
                "true"
            };
        }

        #endregion

        #region System.Web.UI.WebControls.DetailsViewMode

        public static DetailViewMode ToMVPDetailViewMode(this DetailsViewMode mode)
        {
            switch (mode)
            {
                case DetailsViewMode.Insert:
                    return DetailViewMode.Insert;
                case DetailsViewMode.Edit:
                    return DetailViewMode.Edit;
                default:
                    return DetailViewMode.ReadOnly;
            }
        }

        #endregion

        #region System.Web.UI.WebControls.FormViewMode

        public static DetailViewMode ToMVPDetailViewMode(this FormViewMode mode)
        {
            switch (mode)
            {
                case FormViewMode.Insert:
                    return DetailViewMode.Insert;
                case FormViewMode.Edit:
                    return DetailViewMode.Edit;
                default:
                    return DetailViewMode.ReadOnly;
            }
        }

        #endregion

        #region MMSINC.DesignPatterns.MVP.DetailViewMode

        public static DetailsViewMode ToDetailsViewMode(this DetailViewMode mode)
        {
            switch (mode)
            {
                case DetailViewMode.Insert:
                    return DetailsViewMode.Insert;
                case DetailViewMode.Edit:
                    return DetailsViewMode.Edit;
                default:
                    return DetailsViewMode.ReadOnly;
            }
        }

        public static FormViewMode ToFormViewMode(this DetailViewMode mode)
        {
            switch (mode)
            {
                case DetailViewMode.Insert:
                    return FormViewMode.Insert;
                case DetailViewMode.Edit:
                    return FormViewMode.Edit;
                default:
                    return FormViewMode.ReadOnly;
            }
        }

        #endregion

        #region System.Web.UI.WebControls.DropDownList

        public static TReturn GetSelectedValue<TReturn>(this DropDownList ddl, Func<ListItem, TReturn> fn)
        {
            if (ddl.SelectedIndex < 0) return default(TReturn);

            return fn(ddl.SelectedItem);
        }

        public static int? GetSelectedValue(this DropDownList ddl)
        {
            return ddl.GetSelectedValue<int?>(li => {
                int value;
                if (!Int32.TryParse(li.Value, out value))
                    return null;
                return value;
            });
        }

        public static bool? GetBooleanValue(this DropDownList ddl)
        {
            return
                ddl.GetSelectedValue(
                    li =>
                        (li == null || String.IsNullOrEmpty(li.Value))
                            ? (bool?)null
                            : ((IList<string>)ValueStrings.TRUE_VALUES).Contains(
                                li.Value.ToLower()));
        }

        public static string GetStringValue(this DropDownList ddl)
        {
            return ddl.GetSelectedValue(li => (li == null) ? null : li.Value);
        }

        #endregion

        #region System.Web.UI.WebControls.ListBox

        public static List<TReturn> GetSelectedValues<TReturn>(this ListBox listBox, Func<ListItem, TReturn> fn)
        {
            var indices = listBox.GetSelectedIndices();
            var len = indices.Length;

            if (len == 0) return null;

            var ret = new List<TReturn>();
            for (var i = 0; i < len; ++i)
            {
                ret.Add(fn(listBox.Items[indices[i]]));
            }

            return ret;
        }

        public static List<int> GetSelectedValues(this ListBox listBox)
        {
            return listBox.GetSelectedValues(li => Int32.Parse(li.Value));
        }

        #endregion

        #region System.Web.UI.WebControls.TextBox

        public static TReturn GetValue<TReturn>(this TextBox txt, Func<String, TReturn> fn)
        {
            return fn(txt.Text);
        }

        public static int GetIntValue(this TextBox txt)
        {
            int ret;
            return (Int32.TryParse(txt.Text, out ret)) ? ret : 0;
        }

        public static int? TryGetIntValue(this TextBox txt)
        {
            int ret;
            return (Int32.TryParse(txt.Text, out ret)) ? ret : (int?)null;
        }

        public static double GetDoubleValue(this TextBox txt)
        {
            double ret;
            return (Double.TryParse(txt.Text, out ret)) ? ret : 0;
        }

        public static double? TryGetDoubleValue(this TextBox txt)
        {
            double ret;
            return (Double.TryParse(txt.Text, out ret)) ? ret : (double?)null;
        }

        public static DateTime GetDateTimeValue(this TextBox txt)
        {
            DateTime ret;
            return (DateTime.TryParse(txt.Text, out ret))
                ? ret
                : DateTime.MinValue;
        }

        public static DateTime? TryGetDateTimeValue(this TextBox txt)
        {
            DateTime ret;
            return (DateTime.TryParse(txt.Text, out ret))
                ? ret
                : (DateTime?)null;
        }

        #endregion

        #region System.Web.UI.WebControls.MenuItemCollection

        public static MenuItem GetItemWithValue(this MenuItemCollection coll, string value)
        {
            for (int i = 0, len = coll.Count; i < len; ++i)
            {
                if (coll[i].Value == value)
                    return coll[i];
            }

            return null;
        }

        #endregion

        #region System.Web.UI.Control

        public static TControl FindControl<TControl>(this Control ctrl, string id) where TControl : Control
        {
            return (TControl)ctrl.FindControl(id);
        }

        public static TIControl FindIControl<TIControl>(this Control ctrl, string id) where TIControl : IControl
        {
            return (TIControl)(object)ctrl.FindControl(id);
        }

        public static IUserControl FindIParent(this Control ctrl)
        {
            var iParent = ctrl as IUserControl;

            return iParent ??
                   ((ctrl.Parent == null) ? null : FindIParent(ctrl.Parent));
        }

        #endregion

        #region MMSINC.Controls.IControl

        public static TIControl FindIControl<TIControl>(this IControl ctrl, string id) where TIControl : IControl
        {
            return (TIControl)(object)ctrl.FindControl(id);
        }

        #endregion
    }
}
