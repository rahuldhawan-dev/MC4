using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Interface;
using StructureMap;
using StructureMap.Pipeline;

namespace MMSINC.View
{
    public abstract class ListView<TEntity> : MvpUserControl, IListView<TEntity>
        where TEntity : class
    {
        #region Constants

        public struct ViewStateKeys
        {
            public const string SORT_EXPRESSION = "sortExpression";
            public const string SORT_DIRECTION = "sortDirection";
        }

        public const string DESCENDING = "DESC";

        #endregion

        #region Private Members

        private IListPresenter<TEntity> _presenter;

        #endregion

        #region Properties

        public virtual IListPresenter<TEntity> Presenter
        {
            get
            {
                if (_presenter == null)
                    _presenter =
                        DependencyResolver.Current.GetService<IContainer>().GetInstance<IListPresenter<TEntity>>(
                            new ExplicitArguments(new Dictionary<string, object> {{"view", this}}));
                return _presenter;
            }
        }

        public abstract IListControl ListControl { get; }

        public virtual int SelectedIndex
        {
            get { return ListControl.SelectedIndex; }
            set { ListControl.SelectedIndex = value; }
        }

        public virtual object SelectedDataKey
        {
            get
            {
                return (ListControl == null ||
                        ListControl.SelectedDataKey == null)
                    ? null
                    : ((DataKey)ListControl.SelectedDataKey).Value;

                //if (ListControl == null || ListControl.SelectedDataKey == null)
                //    return null;
                //return ((DataKey)ListControl.SelectedDataKey).Value;
            }
        }

        public virtual string SqlSortExpression =>
            PreviousSortDirection ==
            SortDirection.Descending.ToString()
                ? SortExpression + " " + DESCENDING
                : SortExpression;

        /// <summary>
        /// This is silly, but we need to track the DESC/ASC with the
        /// SortExpress or another ViewState variable 
        /// </summary>
        public virtual string SortExpression
        {
            get
            {
                var sortExpression = ViewState[ViewStateKeys.SORT_EXPRESSION];
                return sortExpression != null
                    ? sortExpression.ToString()
                    : string.Empty;
            }
            set { ViewState[ViewStateKeys.SORT_EXPRESSION] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string PreviousSortDirection
        {
            get
            {
                var sortDirection = ViewState[ViewStateKeys.SORT_DIRECTION];
                return sortDirection != null
                    ? sortDirection.ToString()
                    : String.Empty;
            }
            set { ViewState[ViewStateKeys.SORT_DIRECTION] = value; }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                OnVisibilityChanged(value);
            }
        }

        #endregion

        #region Private Methods

        protected virtual void DataBindListControl()
        {
            ListControl.DataBind();
        }

        #endregion

        #region Events

        public event EventHandler SelectedIndexChanged;
        public event EventHandler UserControlLoaded;
        public event EventHandler CreateClicked;
        public event EventHandler LoadComplete;
        public event EventHandler<ObjectDataSourceEventArgs> DataSourceCreating;
        public event EventHandler<GridViewSortEventArgs> Sorting;
        public event EventHandler<GridViewPageEventArgs> PageIndexChanging;
        public event EventHandler<VisibilityChangeEventArgs> VisibilityChanged;

        #endregion

        #region Event Handlers

        protected virtual void Page_Init(object sender, EventArgs e)
        {
            Presenter.OnViewInit();
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if (!IsMvpPostBack)
                Presenter.OnViewInitialized();

            OnUserControlLoaded(e);

            Presenter.OnViewLoaded();
        }

        protected virtual void Page_LoadComplete(object sender, EventArgs e)
        {
            OnLoadComplete(e);
        }

        protected virtual void ListControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedIndexChanged(e);
        }

        protected virtual void ods_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            OnDataSourceCreating(e);
        }

        protected virtual void btnCreate_Click(object sender, EventArgs e)
        {
            OnCreateClicked(e);
        }

        protected virtual void ListControl_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Haven't been able to figure out how to set these through the list presenter. 
            //They need to be set for the Resource Presenter, and the List Presenter
            //event fires after that, so they ended up here.

            SortExpression = e.SortExpression;
            //if (!SortExpression.ToUpper().EndsWith("ASC") || !SortExpression.EndsWith("DESC"))
            //    SortExpression = SortExpression + " ASC";
            PreviousSortDirection =
                e.SortDirection.ToString() == PreviousSortDirection
                    ? SortDirection.Descending.ToString()
                    : SortDirection.Ascending.ToString();
            OnSorting(e);
        }

        protected virtual void ListControl_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            OnPageIndexChanging(e);
        }

        #endregion

        #region Event Passthroughs

        protected virtual void OnUserControlLoaded(EventArgs e)
        {
            UserControlLoaded?.Invoke(this, e);
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            SelectedIndexChanged?.Invoke(this, e);
        }

        protected virtual void OnDataSourceCreating(ObjectDataSourceEventArgs e)
        {
            DataSourceCreating?.Invoke(this, e);
        }

        protected virtual void OnCreateClicked(EventArgs e)
        {
            CreateClicked?.Invoke(this, e);
        }

        protected virtual void OnLoadComplete(EventArgs e)
        {
            LoadComplete?.Invoke(this, e);
        }

        protected virtual void OnSorting(GridViewSortEventArgs e)
        {
            Sorting?.Invoke(this, e);
        }

        protected virtual void OnPageIndexChanging(GridViewPageEventArgs e)
        {
            PageIndexChanging?.Invoke(this, e);
        }

        protected virtual void OnVisibilityChanged(bool value)
        {
            VisibilityChanged?.Invoke(this, new VisibilityChangeEventArgs(value));
        }

        #endregion

        #region Exposed Methods

        public virtual void SetListData(IEnumerable<TEntity> data)
        {
            ListControl.DataSource = data;
            ListControl.DataSourceID = null;
            GetAndDisplayCount(data);
        }

        public override void DataBind()
        {
            base.DataBind();
            DataBindListControl();
        }

        /// <summary>
        /// Override this method to get a count
        /// e.g. lblCount.Text = data.Count().ToString();
        /// </summary>
        /// <param name="data"></param>
        protected virtual void GetAndDisplayCount(IEnumerable<TEntity> data) { }

        #endregion

        #region Abstract Methods

        public abstract void SetViewControlsVisible(bool visible);

        #endregion
    }
}
