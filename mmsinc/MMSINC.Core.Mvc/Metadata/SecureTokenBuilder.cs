using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.MemberInfoExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.DesignPatterns;
using MMSINC.Utilities;

namespace MMSINC.Metadata
{
    public abstract class SecureTokenBuilder<TToken, TValues> : Builder<TToken>
        where TToken : class, ISecureFormToken<TToken, TValues>, new()
        where TValues : ISecureFormDynamicValue<TValues, TToken>, new()
    {
        #region Private Members

        #endregion

        #region Properties

        protected virtual RouteContext RouteContext { get; }

        protected virtual ITokenRepository<TToken, TValues> Repository { get; }

        protected virtual IAuthenticationService<IAdministratedUser> AuthServ { get; }

        #endregion

        #region Constructors

        protected SecureTokenBuilder(RouteContext routeContext, ITokenRepository<TToken, TValues> repository,
            IAuthenticationService<IAdministratedUser> authServ)
        {
            RouteContext = routeContext.ThrowIfNull("routeContext");
            Repository = repository.ThrowIfNull("repository");
            AuthServ = authServ.ThrowIfNull("authServ");
        }

        #endregion

        #region Private Methods

        protected virtual IEnumerable<PropertyInfo> GetSecuredProperties()
        {
            var parameters = RouteContext.ActionDescriptor.GetParameters();

            return
                from param in parameters.Map<ParameterDescriptor, Type>(p => p.ParameterType)
                from pi in
                    param.GetProperties(BindingFlags.Instance | BindingFlags.Public,
                        p => p.HasAttribute<SecuredAttribute>())
                let attr = pi.GetCustomAttribute<SecuredAttribute>()
                where !attr.UserCanEdit(AuthServ)
                select pi;
        }

        #endregion

        #region Abstract Methods

        protected abstract object GetValue(string key);

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Build a token using the supplied route values or view data.
        /// </summary>
        /// <returns></returns>
        public override TToken Build()
        {
            var token = new TToken {
                UserId = AuthServ.CurrentUserId,
                Action = RouteContext.ActionName,
                Controller = RouteContext.ControllerName,
                Area = RouteContext.AreaName
            };

            var secured = GetSecuredProperties();

            foreach (var prop in secured)
            {
                var key = prop.Name;
                var value = GetValue(key);

                if (prop.HasAttribute<RequiredAttribute>() && value == null)
                {
                    throw new InvalidOperationException(
                        string.Format("Required secured property '{0}' had no provided value.",
                            key));
                }

                token.DynamicValues.Add(new TValues {SecureFormToken = token, Key = key, Value = value});
            }

            return token;
        }

        /// <summary>
        /// Build a token (see #Build), save it to the repository, and return it.
        /// </summary>
        public virtual TToken Create()
        {
            return Repository.Save(Build());
        }

        #endregion
    }

    /// <summary>
    /// Builds (and saves) secure tokens from ModelMetadata.
    /// </summary>
    public class SecureModelMetadataTokenBuilder<TToken, TValues> : SecureTokenBuilder<TToken, TValues>
        where TToken : class, ISecureFormToken<TToken, TValues>, new()
        where TValues : ISecureFormDynamicValue<TValues, TToken>, new()
    {
        #region Private Members

        private readonly ModelMetadata _modelData;

        #endregion

        #region Properties

        protected virtual ModelMetadata ModelData
        {
            get { return _modelData; }
        }

        #endregion

        #region Constructors

        public SecureModelMetadataTokenBuilder(ModelMetadata modelData, RouteContext routeContext,
            ITokenRepository<TToken, TValues> repository,
            IAuthenticationService<IAdministratedUser> authServ)
            : base(routeContext, repository, authServ)
        {
            _modelData = modelData.ThrowIfNull("modelData");
        }

        #endregion

        #region Private Methods

        protected override object GetValue(string key)
        {
            return ModelData.Model.GetPropertyValueByName(key);
        }

        #endregion
    }

    /// <summary>
    /// Builds (and saves) secure form tokens from RouteValueDictionaries.
    /// </summary>
    public class SecureRouteValueTokenBuilder<TToken, TValues> : SecureTokenBuilder<TToken, TValues>
        where TToken : class, ISecureFormToken<TToken, TValues>, new()
        where TValues : ISecureFormDynamicValue<TValues, TToken>, new()
    {
        #region Private Members

        private readonly RouteValueDictionary _routeValues;

        #endregion

        #region Properties

        protected virtual RouteValueDictionary RouteValues
        {
            get { return _routeValues; }
        }

        #endregion

        #region Constructors

        public SecureRouteValueTokenBuilder(RouteValueDictionary routeValues, RouteContext routeContext,
            ITokenRepository<TToken, TValues> repository, IAuthenticationService<IAdministratedUser> authServ)
            : base(routeContext, repository, authServ)
        {
            _routeValues = routeValues.ThrowIfNull("routeValues");
        }

        #endregion

        #region Private Methods

        protected override object GetValue(string key)
        {
            return RouteValues[key];
        }

        #endregion
    }
}
