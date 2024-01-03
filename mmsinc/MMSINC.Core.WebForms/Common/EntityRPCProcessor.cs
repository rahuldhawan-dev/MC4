using System;
using MMSINC.Interface;
using MMSINC.Presenter;

namespace MMSINC.Common
{
    public class EntityRPCProcessor<TEntity>
        where TEntity : class
    {
        #region Properties

        public IResourceRPCPresenter<TEntity> Presenter { get; protected set; }

        #endregion

        #region Constructors

        public EntityRPCProcessor(IResourceRPCPresenter<TEntity> presenter)
        {
            Presenter = presenter;
        }

        #endregion

        #region Exposed Methods

        public void Process()
        {
            ResourceViewMode mode;
            switch (Presenter.RPCView.Command)
            {
                case RPCCommandNames.UPDATE:
                case RPCCommandNames.VIEW:
                    Presenter.Repository.RestoreFromPersistedState(Int32.Parse(Presenter.RPCView.Argument));
                    goto case RPCCommandNames.CREATE;
                case RPCCommandNames.CREATE:
                    mode = ResourceViewMode.Detail;
                    break;
                // case CommandNames.LIST:
                default:
                    mode = ResourceViewMode.List;
                    break;
            }

            Presenter.RPCView.SetViewMode(mode);
        }

        #endregion
    }
}
