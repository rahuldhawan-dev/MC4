using System;
using System.Linq.Expressions;
using MMSINC.Presenter;

namespace MMSINC.Interface
{
    public interface IResourceRPCView : IResourceView
    {
        #region Properties

        string Command { get; }
        string Argument { get; }
        RPCCommands RPCCommand { get; }

        #endregion

        #region Methods

        void ShowDetailViewControls(bool show);

        #endregion
    }

    public interface IResourceRPCView<TEntity> : IResourceRPCView, IResourceView<TEntity>
        where TEntity : class
    {
        #region Properties

        IResourceRPCPresenter<TEntity> RPCPresenter { get; }

        #endregion

        #region Methods

        void ShowEntityOnDetailView(TEntity entity);
        Expression<Func<TEntity, bool>> GenerateExpression();

        #endregion
    }
}
