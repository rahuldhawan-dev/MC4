using System;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;

namespace MMSINC.Presenter
{
    /// <summary>
    /// Be careful when overloading any of these Event Handlers
    /// If you don't call this ChildResourceView.OnChildEvent(e) the
    /// resourceview that has this in it will not update its DataKey.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class ChildResourcePresenter<TEntity> : ResourcePresenter<TEntity>,
        IChildResourcePresenter<TEntity>, IChildResourcePresenter
        where TEntity : class, new()
    {
        #region Properties

        //e.g. TestDetailViewPropertyReturnsDetailView in 271
        public virtual IChildDetailView<TEntity> ChildDetailView
        {
            get { return (IChildDetailView<TEntity>)DetailView; }
        }

        public virtual IChildListView<TEntity> ChildListView
        {
            get { return (IChildListView<TEntity>)ListView; }
        }

        public virtual IChildResourceView<TEntity> ChildResourceView
        {
            get { return (IChildResourceView<TEntity>)View; }
        }

        public object ParentEntity { get; set; }

        #endregion

        #region Constructors

        public ChildResourcePresenter(IChildResourceView<TEntity> view, IRepository<TEntity> repository)
            : base(view, repository) { }

        #endregion

        #region Event Passthroughs

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            if (DetailView != null)
            {
                DetailView.Inserting += DetailView_Inserting;
            }
        }

        #endregion

        #region Event Handlers

        protected override void View_BackToListClicked(object sender, EventArgs e)
        {
            ChildResourceView.OnChildEvent(e);
            base.View_BackToListClicked(sender, e);
        }

        #region ListView

        protected override void ListView_CreateClicked(object sender, EventArgs e)
        {
            ChildResourceView.OnChildEvent(e);
            if (DetailView != null)
            {
                DetailView.ShowEntity(GetNewEntity());
                DetailView.SetViewMode(DetailViewMode.Insert);
                DetailView.DataBind();
            }

            if (View != null)
                View.SetViewMode(ResourceViewMode.Detail);
        }

        protected override void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChildResourceView.OnChildEvent(e);
            base.ListView_SelectedIndexChanged(sender, e);
        }

        #endregion

        #region DetailView

        protected override void DetailView_DeleteClicked(object sender, EventArgs e)
        {
            ChildResourceView.OnChildEvent(e);
            base.DetailView_DeleteClicked(sender, e);
        }

        protected override void DetailView_DiscardChangesClicked(object sender, EventArgs e)
        {
            ChildResourceView.OnChildEvent(e);
            base.DetailView_DiscardChangesClicked(sender, e);
        }

        protected override void DetailView_EditClicked(object sender, EventArgs e)
        {
            ChildResourceView.OnChildEvent(e);
            base.DetailView_EditClicked(sender, e);
        }

        protected virtual void DetailView_Inserting(object sender, EntityEventArgs<TEntity> e)
        {
            ChildResourceView.OnChildEvent(e);
        }

        protected override void DetailView_Updating(object sender, EventArgs e)
        {
            ChildResourceView.OnChildEvent(e);
            base.DetailView_Updating(sender, e);
        }

        #endregion

        #endregion

        #region Exposed Methods

        public abstract void FilterListViews();

        public virtual TEntity GetNewEntity()
        {
            return new TEntity();
        }

        #endregion
    }
}
