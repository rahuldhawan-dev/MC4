﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC.Controls
{
    public interface IGridView : IListControl, IDataBoundControl
    {
        #region Properties

        bool AllowPaging { get; set; }
        bool AllowSorting { get; set; }
        bool AutoGenerateColumns { get; set; }
        bool AutoGenerateDeleteButton { get; set; }
        bool AutoGenerateEditButton { get; set; }
        bool AutoGenerateSelectButton { get; set; }

        string OnClientDelete { get; set; }
        string CssClass { set; get; }

        DataKeyArray DataKeys { get; }
        int EditIndex { get; }
        IGridViewRow IFooterRow { get; }
        IGridViewRow IHeaderRow { get; }
        IGridViewRowCollection IRows { get; }
        string OnClientSelect { get; set; }

        event GridViewSortEventHandler Sorting;
        event EventHandler SelectedIndexChanged;

        [Obsolete("Use the wrapped (testable) property IHeaderRow instead.")]
        GridViewRow HeaderRow { get; }

        [Obsolete("Use the wrapped (testable) property IFooterRow instead.")]
        GridViewRow FooterRow { get; }

        [Obsolete("Use the wrapped (testable) property IRows instead.")]
        GridViewRowCollection Rows { get; }

        DataControlFieldCollection Columns { get; }

        #endregion

        #region Methods

        void RenderControl(HtmlTextWriter writer);

        #endregion
    }
}