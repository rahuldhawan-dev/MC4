using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using MMSINC.Interface;
using MMSINC.Presenter;
using StructureMap;
using StructureMap.Pipeline;

namespace MMSINC.View
{
    public abstract class ResourceRPCView<TEntity> : ResourceView<TEntity>, IResourceRPCView<TEntity>
        where TEntity : class
    {
        #region Constants

        private const string ARGUMENT_REGEX_BASE = ":([^;]+);?";

        #endregion

        #region Private Members

        protected IResourceRPCPresenter<TEntity> _rpcPresenter;

        #endregion

        #region Properties

        public virtual string Command
        {
            get
            {
                return
                    HttpContext.Current.Request.QueryString[
                        RPCQueryStringValues.COMMAND];
            }
        }

        public virtual string Argument
        {
            get
            {
                return
                    HttpContext.Current.Request.QueryString[
                        RPCQueryStringValues.ARGUMENT];
            }
        }

        public override IResourcePresenter<TEntity> Presenter
        {
            get { return null; }
        }

        public virtual IResourceRPCPresenter<TEntity> RPCPresenter
        {
            get
            {
                if (_rpcPresenter == null)
                    _rpcPresenter =
                        DependencyResolver.Current.GetService<IContainer>().GetInstance<IResourceRPCPresenter<TEntity>>(
                            new ExplicitArguments(new Dictionary<string, object> {
                                {"view", this},
                                {"repository", Repository}
                            }));
                return _rpcPresenter;
            }
        }

        public virtual RPCCommands RPCCommand
        {
            get
            {
                if (!String.IsNullOrEmpty(Command))
                    return Command.StringToRPCCommand();
                throw new ArgumentNullException();
            }
        }

        #endregion

        #region Event Handlers

        protected override void Page_Init(object sender, EventArgs e)
        {
            RPCPresenter.OnViewInit(IUser);
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            // Normally we'd get the presenter and repository from
            // Ioc right here, but in this class we need the presenter
            // in Page_Init, which happens before this does.  So both
            // Presenter and Repository have been moved to lazy-evaluating
            // properties that do that.
            if (ListView != null)
                RPCPresenter.ListView = ListView;
            if (DetailView != null)
                RPCPresenter.DetailView = DetailView;
            if (SearchView != null)
                RPCPresenter.SearchView = SearchView;

            if (!IsMvpPostBack)
                RPCPresenter.OnViewInitialized();

            RPCPresenter.OnViewLoaded();
        }

        #endregion

        #region Exposed Methods

        public virtual void ShowEntityOnDetailView(TEntity entity)
        {
            DetailView.ShowEntity(entity);
        }

        public virtual void ShowDetailViewControls(bool show)
        {
            DetailView.SetViewControlsVisible(show);
        }

        public virtual string GetArgumentValue(string key)
        {
            return
                new Regex(key + ARGUMENT_REGEX_BASE).Match(Argument).Groups[1].Value;
        }

        public virtual TReturn GetArgumentValue<TReturn>(string key, Func<string, TReturn> transform)
        {
            return transform(GetArgumentValue(key));
        }

        #endregion

        #region Abstract Methods

        public abstract Expression<Func<TEntity, bool>> GenerateExpression();

        #endregion
    }
}
