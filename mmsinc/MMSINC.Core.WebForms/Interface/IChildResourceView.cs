using System;

namespace MMSINC.Interface
{
    public interface IChildResourceView<TEntity> : IChildResourceView, IResourceView<TEntity>
        where TEntity : class
    {
        #region Exposed Methods

        void ShowEntityOnDetailView(TEntity entity);

        #endregion
    }

    public interface IChildResourceView : IResourceView
    {
        #region Properties

        IChildResourcePresenter Presenter { get; }

        #endregion

        #region Events

        event EventHandler ChildEvent;

        #endregion

        #region Exposed Methods

        void OnChildEvent(EventArgs e);

        #endregion
    }
}
