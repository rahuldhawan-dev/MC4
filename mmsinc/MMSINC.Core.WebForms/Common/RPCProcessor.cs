using System;
using MMSINC.Interface;
using MMSINC.Presenter;

namespace MMSINC.Common
{
    public class RPCProcessor<TEntity>
        where TEntity : class
    {
        #region Private Members

        private readonly IResourceRPCPresenter<TEntity> _presenter;

        #endregion

        #region Properties

        protected IResourceRPCPresenter<TEntity> Presenter
        {
            get { return _presenter; }
        }

        #endregion

        #region Constructors

        public RPCProcessor(IResourceRPCPresenter<TEntity> presenter)
        {
            _presenter = presenter;
        }

        #endregion

        #region Private Methods

        private void SetRepositoryIndexToViewArgumentAndShowEntity()
        {
            // WE NEED TO SET THE FILTER EXPRESSION HERE. OTHERWISE WE
            // GET BACK EVERY WORK ORDER AT THIS POINT.
            Presenter.Repository.SetSelectedDataKeyForRPC(
                Presenter.RPCView.Argument,
                Presenter.RPCView.GenerateExpression());
            Presenter.RPCView.ShowEntityOnDetailView(
                Presenter.Repository.CurrentEntity);
        }

        private void SetDetailViewMode(DetailViewMode mode)
        {
            Presenter.RPCView.SetDetailMode(mode);
        }

        private void ShowDetailsViewControls(bool show)
        {
            Presenter.RPCView.ShowDetailViewControls(show);
        }

        private void SetViewMode(ResourceViewMode mode)
        {
            Presenter.RPCView.SetViewMode(mode);
        }

        #endregion

        #region Command Handler Functions

        private void HandleCreateCommand()
        {
            SetViewMode(ResourceViewMode.Detail);
            SetDetailViewMode(DetailViewMode.Insert);
            ShowDetailsViewControls(true);
        }

        private void HandleViewCommand()
        {
            SetRepositoryIndexToViewArgumentAndShowEntity();
            SetViewMode(ResourceViewMode.Detail);
            SetDetailViewMode(DetailViewMode.ReadOnly);
            ShowDetailsViewControls(true);
        }

        private void HandleListCommand()
        {
            SetViewMode(ResourceViewMode.List);
        }

        private void HandleUpdateCommand()
        {
            SetRepositoryIndexToViewArgumentAndShowEntity();
            SetViewMode(ResourceViewMode.Detail);
            SetDetailViewMode(DetailViewMode.Edit);
            ShowDetailsViewControls(true);
        }

        #endregion

        #region Exposed Methods

        public void Process()
        {
            switch (Presenter.RPCView.Command)
            {
                case RPCCommandNames.CREATE:
                    HandleCreateCommand();
                    break;
                case RPCCommandNames.VIEW:
                    HandleViewCommand();
                    break;
                case RPCCommandNames.LIST:
                    HandleListCommand();
                    break;
                case RPCCommandNames.UPDATE:
                    HandleUpdateCommand();
                    break;
            }
        }

        #endregion

        #region Exposed Static Methods

        public static string GenerateRPCCommandString(RPCCommands command)
        {
            return GenerateRPCCommandString(command.RPCCommandToString());
        }

        public static string GenerateRPCCommandString(string command)
        {
            return RPCQueryStringValues.COMMAND + "=" + command;
        }

        #endregion
    }
}
