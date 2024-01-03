using System;
using System.Collections.Generic;
using MMSINC.Common;

namespace MMSINC.Interface
{
    public interface IDetailView<TEntity> : IView
        where TEntity : class
    {
        #region Properties

        IDetailPresenter<TEntity> Presenter { get; }
        DetailViewMode CurrentMode { get; }
        bool Visible { get; set; }
        IEnumerable<IChildResourceView> ChildResourceViews { get; }
        object CurrentDataKey { get; }

        #endregion

        #region Events

        event EventHandler EditClicked,
                           DiscardChangesClicked,
                           UserControlLoaded,
                           EntityLoaded;

        event EventHandler<EntityEventArgs<TEntity>> Inserting, Updating, DeleteClicked;

        #endregion

        #region Methods

        void SetViewControlsVisible(bool visible);
        void SetViewMode(DetailViewMode mode);
        void ShowEntity(TEntity entity);
        void DataBind();

        #endregion
    }

    public enum DetailViewMode
    {
        Insert,
        Edit,
        ReadOnly
    }
}
