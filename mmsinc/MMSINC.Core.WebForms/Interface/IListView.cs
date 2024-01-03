using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using MMSINC.Common;
using MMSINC.Controls;

namespace MMSINC.Interface
{
    public interface IListView<TEntity> : IView
        where TEntity : class
    {
        #region Properties

        IListPresenter<TEntity> Presenter { get; }
        int SelectedIndex { get; set; }
        object SelectedDataKey { get; }
        IListControl ListControl { get; }

        string SqlSortExpression { get; }
        string SortExpression { get; set; }
        string PreviousSortDirection { get; set; }

        #endregion

        #region Events

        event EventHandler SelectedIndexChanged,
                           UserControlLoaded,
                           CreateClicked,
                           LoadComplete;

        event EventHandler<ObjectDataSourceEventArgs> DataSourceCreating;
        event EventHandler<GridViewSortEventArgs> Sorting;
        event EventHandler<GridViewPageEventArgs> PageIndexChanging;
        event EventHandler<VisibilityChangeEventArgs> VisibilityChanged;

        #endregion

        #region Methods

        void SetViewControlsVisible(bool visible);
        void SetListData(IEnumerable<TEntity> data);

        #endregion
    }
}
