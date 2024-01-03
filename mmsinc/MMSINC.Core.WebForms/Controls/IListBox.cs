using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public interface IListBox : IListControl
    {
        #region Methods

        List<TReturn> GetSelectedValues<TReturn>(Func<ListItem, TReturn> fn);
        List<int> GetSelectedValues();

        #endregion
    }
}
