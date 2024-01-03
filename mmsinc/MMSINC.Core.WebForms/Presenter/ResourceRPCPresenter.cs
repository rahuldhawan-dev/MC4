using System;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;

namespace MMSINC.Presenter
{
    public abstract class ResourceRPCPresenter<TEntity> : ResourcePresenter<TEntity>, IResourceRPCPresenter<TEntity>
        where TEntity : class, new()
    {
        #region Private Members

        private readonly IResourceRPCView<TEntity> _view;
        protected RPCProcessor<TEntity> _rpcProcessor;

        #endregion

        #region Properties

        public IResourceRPCView<TEntity> RPCView
        {
            get { return _view; }
        }

        protected virtual RPCProcessor<TEntity> RPCProcessor
        {
            get
            {
                if (_rpcProcessor == null)
                    _rpcProcessor = new RPCProcessor<TEntity>(this);
                return _rpcProcessor;
            }
        }

        #endregion

        #region Constructors

        public ResourceRPCPresenter(IResourceRPCView<TEntity> view, IRepository<TEntity> repository)
            : base(view, repository)
        {
            _view = view;
        }

        #endregion

        #region Private Methods

        protected virtual void ProcessCommandAndArgument()
        {
            RPCProcessor.Process();
        }

        #endregion

        #region Exposed Methods

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            ProcessCommandAndArgument();
            if (ListView != null)
            {
                ListView.SelectedIndexChanged += ListView_SelectedIndexChanged;
            }

            RPCView.BackToListClicked += BackToList_Clicked;
        }

        public void ChangeViewCommand(RPCCommands command)
        {
            View.Redirect(
                View.RelativeUrl.Replace(
                    RPCProcessor<TEntity>.GenerateRPCCommandString(
                        RPCView.Command),
                    RPCProcessor<TEntity>.GenerateRPCCommandString(
                        command)));
        }

        #endregion

        #region Event Handlers

        protected override void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            RPCView.SetViewMode(ResourceViewMode.Detail);
        }

        protected virtual void BackToList_Clicked(object sender, EventArgs e)
        {
            RPCView.SetViewMode(ResourceViewMode.List);
        }

        #endregion
    }

    public static class RPCCommandExtensions
    {
        #region Extension Methods

        public static string RPCCommandToString(this RPCCommands command)
        {
            switch (command)
            {
                case RPCCommands.Create:
                    return RPCCommandNames.CREATE;
                case RPCCommands.Delete:
                    return RPCCommandNames.DELETE;
                case RPCCommands.List:
                    return RPCCommandNames.LIST;
                case RPCCommands.Update:
                    return RPCCommandNames.UPDATE;
                default:
                    return RPCCommandNames.VIEW;
            }
        }

        public static RPCCommands StringToRPCCommand(this string command)
        {
            switch (command)
            {
                case RPCCommandNames.CREATE:
                    return RPCCommands.Create;
                case RPCCommandNames.DELETE:
                    return RPCCommands.Delete;
                case RPCCommandNames.LIST:
                    return RPCCommands.List;
                case RPCCommandNames.UPDATE:
                    return RPCCommands.Update;
                case RPCCommandNames.VIEW:
                    return RPCCommands.View;
                default:
                    throw new ArgumentOutOfRangeException("command", command,
                        "String value could not be converted to an RPCCommand.");
            }
        }

        #endregion
    }

    public struct RPCQueryStringValues
    {
        public const string COMMAND = "cmd",
                            ARGUMENT = "arg";
    }

    public struct RPCCommandNames
    {
        public const string CREATE = "create",
                            UPDATE = "update",
                            LIST = "list",
                            VIEW = "view",
                            DELETE = "delete";
    }

    public enum RPCCommands
    {
        Create,
        Update,
        List,
        View,
        Delete
    }
}
