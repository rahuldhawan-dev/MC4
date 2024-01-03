using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace MMSINC.Authentication
{
    /// <summary>
    /// Base class for creating an Authorizor that worsk with MvcAuthorizationFilter.
    /// 
    /// NOTE: MvcAuthorizers act like singletons, so do not store state in them!
    /// </summary>
    public abstract class MvcAuthorizer
    {
        #region Fields

        protected readonly IContainer _container;

        #endregion

        #region Properties

        // need to get this on the fly from the container rather than consuming
        // directly via constructor so that other instances can be injected by
        // tests
        protected IAuthenticationService AuthenticationService => _container.GetInstance<IAuthenticationService>();

        /// <summary>
        /// Gets/sets whether this authorizer should be ran. This is only
        /// meant for unit tests. 
        /// </summary>
        public bool IsEnabled { get; set; }

        #endregion

        #region Constructor

        public MvcAuthorizer(IContainer container)
        {
            _container = container;
            IsEnabled = true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the matching attributes of a type if any exist. They are returned in order by
        /// controller first and then action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        protected static IEnumerable<T> GetAttributes<T>(AuthorizationContext context) where T : Attribute
        {
            var aDesc = context.ActionDescriptor;
            var actionAttributes = aDesc.GetCustomAttributes(typeof(T), true);
            var controllerAttributes = aDesc.ControllerDescriptor.GetCustomAttributes(typeof(T), true);

            return controllerAttributes.Concat(actionAttributes).Cast<T>().ToList();
        }

        #endregion

        #region Public Abstract Methods

        /// <summary>
        /// Authorizes the current request.
        /// </summary>
        /// <param name="authArgs"></param>
        /// <remarks>
        /// 
        /// Inheritors must do the following:
        ///     - Set authArgs.ContinueAuthorizing to false if the rest of the authorizors
        ///       should not be ran. This defaults to true.
        ///     - Set authArgs.Context.Result to something if the authorization fails for
        ///       any reason.
        /// 
        /// </remarks>
        public abstract void Authorize(AuthorizationArgs authArgs);

        #endregion
    }
}
