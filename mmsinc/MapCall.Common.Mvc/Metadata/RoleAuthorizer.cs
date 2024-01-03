using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using Microsoft.CSharp.RuntimeBinder;
using MMSINC;
using MMSINC.Authentication;
using StructureMap;

namespace MapCall.Common.Metadata
{
    /// <summary>
    /// This is for use with MvcAuthorizationFilter.
    /// </summary>
    public class RoleAuthorizer : MvcAuthorizer
    {
        #region Constants

        public const string UNAUTHORIZED_ERROR = "You are not authorized to access this resource.";

        #endregion
        
        #region Private Members

        // Because this is running on multiple threads we want to make sure this is done in a threadsafe
        // manner. Also there's no ConcurrentHashSet so this is the best we can use that would also be
        // the most performant.
        private readonly ConcurrentDictionary<Type, object> _nonRepositoryControllers =
            new ConcurrentDictionary<Type, object>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the name/path of the view to be used when a user doesn't have proper access.
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Are we returning view errors (MVC) or httpResponse errors (API)
        /// </summary>
        public bool ReturnErrorsAsViews { get; set; } = true;

        // need to get this on the fly from the container rather than constructor
        // injecting so that other instances can be swapped in for testing
        protected IRoleService RoleService => _container.GetInstance<IRoleService>();

        #endregion

        #region Constructors

        public RoleAuthorizer(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns true if the user has access to at least one of the roles in the requiredRoles array or if
        /// there are no roles in the array.
        /// </summary>
        private bool UserMeetsRequirements(IEnumerable<RequiresRoleAttribute> requiredRoles, OperatingCenter opc)
        {
            foreach (var requiredRole in requiredRoles)
            {
                // RoleService does the site admin check, so no need to do it elsewhere.
                if (!RoleService.CanAccessRole(requiredRole.Module, requiredRole.Action, opc))
                {
                    return false;
                }
            }

            return true;
        }

        private static int? TryParseIdParameter(object possibleId)
        {
            int? id = null;
            // We'll get a UrlParameter if the route has an optional id value without an id set.
            if (possibleId == null || possibleId is UrlParameter)
            {
                // noop
            }
            else if (possibleId is string)
            {
                int testId;
                if (int.TryParse((string)possibleId, out testId))
                {
                    id = testId;
                }
            }
            else
            {
                throw new NotSupportedException(possibleId.GetType().FullName);
            }

            return id;
        }

        private object TryGetControllerRepository(AuthorizationArgs authArgs)
        {
            var controller = authArgs.Context.Controller;
            var type = controller.GetType();
            if (!_nonRepositoryControllers.ContainsKey(type))
            {
                try
                {
                    dynamic dynoller = controller;
                    return dynoller.Repository;
                }
                catch (RuntimeBinderException)
                {
                    _nonRepositoryControllers.TryAdd(type, null);
                }
            }

            return null;
        }

        /// <summary>
        /// Attempts to return an OperatingCenter object for the current request.
        /// </summary>
        /// <remarks>
        /// 
        /// This is a hack, but it's the only way to include a dynamic value check as part of
        /// the role authorization check.
        /// 
        /// AuthorizationFilters run *before* any form of model binding occurs and before any
        /// other ActionFilters. This is the only way to get at the OperatingCenter from within
        /// the scope of authorization. It can be done in an ActionFilter but then we'd have to 
        /// rewrite a lot of stuff that's used for checking if a user is authorized to do something.
        /// 
        /// </remarks>
        private OperatingCenter TryGetOperatingCenter(AuthorizationArgs authArgs)
        {
            if (!authArgs.Context.ActionDescriptor
                         .GetCustomAttributes(typeof(SkipRoleOperatingCenterCheckAttribute), false).Any())
            {
                // Attempt, somehow, in some fashion, to get an operating center.
                dynamic repository = TryGetControllerRepository(authArgs);

                if (repository != null)
                {
                    // NOTE: Casting junk. Don't do a thing if there's no id parameter. 
                    // Also sometimes the id is a System.Web.Mvc.UrlParameter.
                    // Sometimes it's a string.
                    var id = TryParseIdParameter(authArgs.Context.RouteData.Values["id"]);
                    if (id != null)
                    {
                        var fullModel = repository.Find(id.Value);
                        if (fullModel is IThingWithOperatingCenter)
                        {
                            return ((IThingWithOperatingCenter)fullModel).OperatingCenter;
                        }

                        if (fullModel != null)
                        {
                            // Since the model doesn't have an OperatingCenter then there's no reason to continue
                            // checking this controller type in the future. 
                            _nonRepositoryControllers.TryAdd(authArgs.Context.Controller.GetType(), null);
                        }
                    }
                }
            }

            return null;
        }

        #endregion

        #region Exposed Methods

        public override void Authorize(AuthorizationArgs authArgs)
        {
            var filterContext = authArgs.Context;
            var actionDesc = filterContext.ActionDescriptor;
            var actionRoles = actionDesc.GetFilterAttributes(true).OfType<RequiresRoleAttribute>().ToList();
            var dynamicRoles = actionDesc.GetFilterAttributes(true).OfType<DynamicRequiresRoleAttribute>().ToList();

            if (dynamicRoles.Any())
            {
                // Because of threading and caching of attributes and probably other reasons, we need to create a new RequiresRoleAttribute
                // instance rather than setting/initializing the DynamicRequiresRoleAttribute. The EntityLookupControllerBase would
                // start causing problems due to the base attribute being shared with inherited controllers.

                // This will throw an exception when the controller doesn't implement from IControllerOfRoles.
                // So if you're seeing this exception throw, you should implement IControllerOfRoles.
                var controller = (IControllerOfRoles)filterContext.Controller;
                actionRoles.AddRange(dynamicRoles.Select(role => new RequiresRoleAttribute(controller.GetDynamicRoleModuleForAction(actionDesc.ActionName), role.Action)));
            }

            // This filter shouldn't do anything if there aren't role requirements.
            if (!actionRoles.Any())
            {
                return;
            }

            var opc = TryGetOperatingCenter(authArgs);

            if (UserMeetsRequirements(actionRoles, opc))
            {
                return;
            }

            var model = new ForbiddenRoleAccessModel();
            model.RequiredRoles.AddRange(actionRoles);

            if (ReturnErrorsAsViews)
            {
                var result = filterContext.IsChildAction ? (ViewResultBase)new PartialViewResult() : new ViewResult();
                result.ViewName = ViewName;
                result.ViewData = new ViewDataDictionary<ForbiddenRoleAccessModel>(model);
                filterContext.Result = result;
            }
            else
            {
                filterContext.Result = new JsonHttpStatusCodeResult(HttpStatusCode.Unauthorized, UNAUTHORIZED_ERROR);
            }
        }

        #endregion
    }
}
