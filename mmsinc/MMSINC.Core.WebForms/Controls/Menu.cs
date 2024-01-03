using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions;

namespace MMSINC.Controls
{
    [ControlBuilder(typeof(GenericControlBuilder<MenuItem>)),
     ParseChildren(false)]
    public class Menu : DataBoundControl, IMenu //System.Web.UI.WebControls.Menu, IMenu
    {
        #region Private Members

        private IMenuItemAdapter _adapter;
        private Dictionary<string, EventHandler> _dictionary;
        private System.Web.UI.WebControls.Menu _innerMenu;

        #endregion

        #region Properties

        public MenuItemCollection Items
        {
            get { return _innerMenu.Items; }
        }

        #endregion

        #region Constructors

#if DEBUG
        public Menu(IMenuItemAdapter adapter)
        {
            _adapter = adapter;
            _dictionary = new Dictionary<string, EventHandler>();
            _innerMenu = new System.Web.UI.WebControls.Menu();
            _innerMenu.MenuItemClick += InnerMenu_MenuItemClick;
            Controls.Add(_innerMenu);
        }
#endif

        public Menu()
        {
            _adapter = new MenuItemAdapter(this);
            _dictionary = new Dictionary<string, EventHandler>();
            _innerMenu = new System.Web.UI.WebControls.Menu();
            _innerMenu.MenuItemClick += InnerMenu_MenuItemClick;
            Controls.Add(_innerMenu);
        }

        #endregion

        #region Private Methods

        protected void InnerMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            OnMenuItemClick(e);
        }

        protected void OnMenuItemClick(MenuEventArgs e)
        {
            if (MenuItemClick != null)
                MenuItemClick(this, e);

            _dictionary[e.Item.Value](this, e);
        }

        protected override void AddParsedSubObject(object obj)
        {
            var item = obj as MenuItem;
            if (item == null)
                base.AddParsedSubObject(obj);
            else
                Items.Add(_adapter.ToDotNetMenuItem(item));
        }

        protected override void PerformSelect()
        {
            if (!IsBoundUsingDataSourceID)
                OnDataBinding(EventArgs.Empty);

            GetData().Select(CreateDataSourceSelectArguments(), OnDataSourceViewSelectCallback);

            RequiresDataBinding = false;
            MarkAsDataBound();

            OnDataBound(EventArgs.Empty);
        }

        protected void OnDataSourceViewSelectCallback(IEnumerable retrievedData)
        {
            if (IsBoundUsingDataSourceID)
                OnDataBinding(EventArgs.Empty);

            PerformDataBinding(retrievedData);
        }

        protected override void PerformDataBinding(IEnumerable data)
        {
            // TODO: Pretty messy, eh?
            base.PerformDataBinding(data);

            Type dataType = null;

            if (data != null)
            {
                foreach (var item in data)
                {
                    if (dataType == null)
                        dataType = item.GetType();

                    foreach (var key in _dictionary.Keys)
                    {
                        var info = dataType.GetProperty(key, BindingFlags.Instance | BindingFlags.Public);
                        if (info == null)
                        {
                            throw new MissingMemberException(
                                String.Format(
                                    "Type {0} does not have an exposed property called {1}.",
                                    dataType.Name, key));
                        }

                        var obj = info.GetValue(item, null);

                        var countable = obj as IList;
                        var menuItem = Items.GetItemWithValue(key);
                        if (menuItem != null)
                            menuItem.Text = countable == null
                                ? String.Format(menuItem.Text, obj)
                                : String.Format(menuItem.Text,
                                    countable.Count);
                    }
                }
            }
        }

        #endregion

        #region Events

        public event EventHandler<MenuEventArgs> MenuItemClick;

        #endregion

        #region Exposed Methods

        public void AddKeyAndMethod(string value, EventHandler handler)
        {
            _dictionary.Add(value, handler);
        }

        #endregion
    }
}
