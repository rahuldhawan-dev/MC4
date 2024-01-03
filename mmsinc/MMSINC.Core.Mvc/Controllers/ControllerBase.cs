using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JetBrains.Annotations;
using MMSINC.Common;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Exceptions;
using MMSINC.Interface;
using StructureMap;

namespace MMSINC.Controllers
{
    /// <summary>
    /// Base controller class with some extra built-in functionality.
    /// </summary>
    public abstract class ControllerBase : Controller
    {
        #region Constants

        public const string ERROR_MESSAGE_KEY = "ErrorMessage",
                            NOTIFICATION_MESSAGE_KEY = "NotificationMessage",
                            SUCCESS_MESSAGE_KEY = "SuccessMessage";

        #endregion

        #region Private Members

        protected readonly IContainer _container;

        #endregion

        #region Properties

        protected override bool DisableAsyncSupport
        {
            get
            {
                if (MvcApplication.IsInTestMode)
                {
                    return true;
                }

                return base.DisableAsyncSupport;
            }
        }

        public IContainer Container => _container;

        #endregion

        #region Constructors

        public ControllerBase(ControllerBaseArguments args)
        {
            _container = args.Container;
        }

        #endregion

        #region Private Methods

        #region Notifications/Error/Success Messages

        // As of 7/12/2017, you are doing it wrong if you try to set these notifications
        // in TempData directly through the controller or views. Only use these methods
        // for dealing with it. If we need to return to adding errors/notifications from
        // outside of a controller action, we can revisit how this is done. 

        private void AppendMessage(string key, string message)
        {
            var messages = (List<string>)TempData.Peek(key);
            if (messages == null)
            {
                messages = new List<string>();
                TempData[key] = messages;
            }

            messages.Add(message);
        }

        protected internal void DisplayErrorMessage(string message)
        {
            AppendMessage(ERROR_MESSAGE_KEY, message);
        }

        protected internal void DisplayModelStateErrors()
        {
            var errors = (from x in ModelState
                          from y in x.Value.Errors
                          select y.ErrorMessage);

            foreach (var e in errors)
            {
                DisplayErrorMessage(e);
            }
        }

        // TODO: This shouldn't be public. This only public because a single
        // static method on EmployeeAssigmentController is using it to
        // send notifications from the ProductionWorkOrderController. 
        [NonAction]
        public void DisplayNotification(string message)
        {
            AppendMessage(NOTIFICATION_MESSAGE_KEY, message);
        }

        [StringFormatMethod("message")]
        protected void DisplaySuccessMessage(string message)
        {
            AppendMessage(SUCCESS_MESSAGE_KEY, message);
        }

        #endregion

        protected ActionResult RedirectToReferrerOr([AspMvcAction] string action, [AspMvcController] string controller)
        {
            return RedirectToReferrerOr(action, controller, String.Empty);
        }

        protected ActionResult RedirectToReferrerOr([AspMvcAction] string action, [AspMvcController] string controller,
            string fragmentId)
        {
            return RedirectToReferrerOr(action, controller, null, fragmentId);
        }

        protected ActionResult RedirectToReferrerOr([AspMvcAction] string action, [AspMvcController] string controller,
            object routeValues, string fragmentId)
        {
            if (Request.UrlReferrer != null)
            {
                var str = Request.UrlReferrer.OriginalString;
                str = str.EndsWith(fragmentId) ? str : str + fragmentId;
                return Redirect(str);
            }

            return RedirectToAction(action, controller, routeValues);
        }

        #endregion

        #region Exposed Methods

        [NonAction]
        public string GetUrlForModel(dynamic model, string action, string controller, string area = "")
        {
            var url = Url.Action(action, controller, new {area, id = model.Id});
            return Request.Url.Scheme + "://" + Request.Url.Authority + url;
        }

        [NonAction]
        public string GetMapUrlForModel(dynamic model, string action, string controller, string area = "")
        {
            var url = "/Modules/mvc/Map?ControllerName=" + controller +
                      "&ActionName=Show&AreaName=" + area + "&Search%5Bid%5D=" + model.Id;

            return Request.Url.Scheme + "://" + Request.Url.Authority + url;
        }

        /// <summary>
        /// Redirect to the area/action/controller with the specified fragmentId
        /// </summary>
        /// <param name="routeValues">Include at least an Action and Controller in this collection, passed to Url.RouteUrl</param>
        /// <param name="fragmentId">Anchor defined for the tab, including the # is not necesary.</param>
        /// <returns></returns>
        [NonAction]
        public ActionResult RedirectToRouteWithTabSelected(object routeValues, string fragmentId)
        {
            var url = Url.RouteUrl(routeValues) + (fragmentId.StartsWith("#") ? fragmentId : $"#{fragmentId}");
            return Redirect(url);
        }

        #endregion
    }

    /// <summary>
    /// Base controller class with built-in repository.
    /// </summary>
    /// <typeparam name="TRepository">
    /// Type of repository to provide.
    /// </typeparam>
    /// <typeparam name="TEntity">
    /// Type of entity represented by the controller.
    /// </typeparam>
    public abstract class ControllerBase<TRepository, TEntity> : ControllerBase
        where TRepository : class, IRepository<TEntity>
    {
        #region Private Members

        protected readonly TRepository _repository;
        protected readonly IViewModelFactory _viewModelFactory;

        #endregion

        #region Properties

        public virtual TRepository Repository => _repository;

        public virtual IViewModelFactory ViewModelFactory => _viewModelFactory;

        #endregion

        #region Constructors

        public ControllerBase(ControllerBaseArguments<TRepository, TEntity> args) : base(args)
        {
            _repository = args.Repository;
            _viewModelFactory = args.ViewModelFactory;
        }

        #endregion
    }

    /// <summary>
    /// Base controller class with built-in repsoitory,
    /// instantiated as a RepositoryBase of type TEntity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class ControllerBase<TEntity> : ControllerBase<IRepository<TEntity>, TEntity>
    {
        #region Properties

        public override IRepository<TEntity> Repository
        {
            get
            {
                if (_repository == null && typeof(IRepository).IsAssignableFrom(typeof(TEntity)))
                {
                    throw new TypeParameterException(
                        "Controller is improperly typed.  TEntity must be an entity, not a repository.");
                }

                return base.Repository;
            }
        }

        #endregion

        #region Constructors

        public ControllerBase(ControllerBaseArguments<IRepository<TEntity>, TEntity> args) : base(args) { }

        #endregion
    }

    public class ControllerBaseArguments
    {
        public IContainer Container { get; }

        public ControllerBaseArguments(IContainer container)
        {
            Container = container;
        }
    }

    public class ControllerBaseArguments<TRepository, TEntity> : ControllerBaseArguments
        where TRepository : class, IRepository<TEntity>
    {
        #region Properties

        public TRepository Repository { get; }
        public IViewModelFactory ViewModelFactory { get; }

        #endregion

        #region Constructors

        public ControllerBaseArguments(TRepository repository, IContainer container, IViewModelFactory viewModelFactory)
            : base(container)
        {
            Repository = repository;
            ViewModelFactory = viewModelFactory;
        }

        #endregion
    }

    public class ControllerBaseArguments<TEntity> : ControllerBaseArguments<IRepository<TEntity>, TEntity>
    {
        #region Constructors

        public ControllerBaseArguments(IRepository<TEntity> repository, IContainer container,
            IViewModelFactory viewModelFactory) : base(repository, container, viewModelFactory) { }

        #endregion
    }
}
