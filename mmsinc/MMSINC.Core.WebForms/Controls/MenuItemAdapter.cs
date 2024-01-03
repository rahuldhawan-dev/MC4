using System;

namespace MMSINC.Controls
{
    public class MenuItemAdapter : IMenuItemAdapter
    {
        #region Private Members

        private IMenu _parent;

        #endregion

        #region Constructors

        public MenuItemAdapter(IMenu parent)
        {
            _parent = parent;
        }

        #endregion

        #region Private Methods

        private void Noop(object sender, EventArgs e) { }

        #endregion

        #region Exposed Methods

        public System.Web.UI.WebControls.MenuItem ToDotNetMenuItem(MenuItem item)
        {
            _parent.AddKeyAndMethod(item.Value, item.OnClickHandler ?? Noop);
            return new System.Web.UI.WebControls.MenuItem {
                Value = item.Value,
                Text = item.TextFormat
            };
        }

        #endregion
    }
}
