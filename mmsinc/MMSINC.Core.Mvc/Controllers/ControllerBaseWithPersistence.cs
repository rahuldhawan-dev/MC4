using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using JetBrains.Annotations;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using StructureMap;
using StructureMap.Pipeline;

namespace MMSINC.Controllers
{
    public abstract class
        ControllerBaseWithPersistence<TRepository, TEntity, TUser> : ControllerBaseWithAuthentication<TRepository,
            TEntity, TUser>
        where TRepository : class, IRepository<TEntity>
        where TUser : IAdministratedUser
        where TEntity : class
    {
        #region Private Members

        protected
            IActionHelper<ControllerBaseWithPersistence<TRepository, TEntity, TUser>, TRepository, TEntity, TUser>
            _actionHelper;

        #endregion

        #region Properties
        
        public IActionHelper
            <ControllerBaseWithPersistence<TRepository, TEntity, TUser>, TRepository, TEntity, TUser> ActionHelper
        {
            get { return _actionHelper; }
        }

        #endregion

        #region Constructors

        public ControllerBaseWithPersistence(ControllerBaseWithPersistenceArguments<TRepository, TEntity, TUser> args)
            : base(args)
        {
            _actionHelper = args.Container
                                .GetInstance<ActionHelper<ControllerBaseWithPersistence<TRepository, TEntity, TUser>,
                                     TRepository, TEntity, TUser>>(
                                     new ExplicitArguments(new Dictionary<string, object> {{"controller", this}}));
        }

        #endregion

        #region Exposed Methods

        [NonAction]
        public virtual void SetLookupData(ControllerAction action)
        {
            //noop   
        }

        /// <summary>
        /// Gives ActionHelper access to RedirectToAction. Only use this in ActionHelper.
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        [NonAction]
        public RedirectToRouteResult DoRedirectionToAction([AspMvcAction] string actionName, object routeValues)
        {
            return RedirectToAction(actionName, routeValues);
        }

        /// <summary>
        /// Gives ActionHelper access to RedirectToAction. Only use this in ActionHelper.
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        [NonAction]
        public RedirectToRouteResult DoRedirectionToAction([AspMvcAction] string actionName,
            RouteValueDictionary routeValues)
        {
            return RedirectToAction(actionName, routeValues);
        }

        /// <summary>
        /// Gives ActionHelper access to RedirectToAction. Only use this in ActionHelper.
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        [NonAction]
        public RedirectToRouteResult DoRedirectionToAction([AspMvcAction] string actionName,
            [AspMvcController] string controllerName, object routeValues)
        {
            return RedirectToAction(actionName, controllerName, routeValues);
        }

        /// <summary>
        /// Gives ActionHelper access to RedirectToAction. Only use this in ActionHelper.
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        [NonAction]
        public RedirectToRouteResult DoRedirectionToAction([AspMvcAction] string actionName,
            [AspMvcController] string controllerName, RouteValueDictionary routeValues)
        {
            return RedirectToAction(actionName, controllerName, routeValues);
        }

        /// <summary>
        /// Gives ActionHelper access to View method. Only use this in ActionHelper.
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <param name="partial"></param>
        /// <returns></returns>
        [NonAction]
        public ActionResult DoView([AspMvcView] string viewName, object model, bool partial = false)
        {
            return partial ? (ActionResult)DoPartialView(viewName, model) : View(viewName, model);
        }

        /// <summary>
        /// Gives ActionHelper access to PartialView method. Only use this in ActionHelper.
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        public PartialViewResult DoPartialView([AspMvcView] string viewName, object model)
        {
            return PartialView(viewName, model);
        }

        /// <summary>
        /// Gives ActionHelper access to HttpNotFound method. Only use this in ActionHelper.
        /// </summary>
        /// <param name="notFound"></param>
        /// <returns></returns>
        [NonAction]
        public HttpNotFoundResult DoHttpNotFound(string notFound)
        {
            return HttpNotFound(notFound);
        }

        #endregion
    }

    public abstract class
        ControllerBaseWithPersistence<TEntity, TUser> : ControllerBaseWithPersistence<IRepository<TEntity>, TEntity,
            TUser>
        where TUser : IAdministratedUser
        where TEntity : class
    {
        #region Constructors

        protected ControllerBaseWithPersistence(
            ControllerBaseWithPersistenceArguments<IRepository<TEntity>, TEntity, TUser> args) : base(args) { }

        #endregion
    }

    public class
        ControllerBaseWithPersistenceArguments<TRepository, TEntity, TUser> : ControllerBaseWithAuthenticationArguments<
            TRepository, TEntity, TUser>
        where TRepository : class, IRepository<TEntity>
        where TUser : IAdministratedUser
        where TEntity : class
    {
        #region Constructors

        public ControllerBaseWithPersistenceArguments(TRepository repository, IContainer container,
            IViewModelFactory viewModelFactory, IAuthenticationService<TUser> authenticationService)
            : base(repository, container, viewModelFactory, authenticationService) { }

        #endregion
    }
}
