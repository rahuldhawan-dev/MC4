using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using MMSINC.Utilities;
using StructureMap;

namespace MMSINC.Metadata
{
    // NOTE: If you're wondering "Hey, where do SecureForms get deleted? They expire, right?"
    // then you need to look for the sql server job called "Clean up SecureFormTokens".
    // This will not exist in your local dev database.

    public class SecureFormTokenService<TToken, TValues> : ISecureFormTokenService
        where TToken : class, ISecureFormToken<TToken, TValues>, new()
        where TValues : ISecureFormDynamicValue<TValues, TToken>, new()
    {
        #region Private Members

        private readonly IContainer _container;

        #endregion

        #region Constructors

        public SecureFormTokenService(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public Guid CreateToken(ModelMetadata modelMetadata, RouteContext routeContext)
        {
            return _container
                  .With(modelMetadata)
                  .With(routeContext)
                  .GetInstance<SecureModelMetadataTokenBuilder<TToken, TValues>>().Create().Token;
        }

        public Guid CreateToken(RouteValueDictionary routeValues, RouteContext routeContext)
        {
            return _container
                  .With(routeValues)
                  .With(routeContext)
                  .GetInstance<SecureRouteValueTokenBuilder<TToken, TValues>>().Create().Token;
        }

        public Guid CreateTokenWithRouteValues(string actionName, string controllerName, int userId,
            IDictionary<string, object> routeValues)
        {
            var token = new TToken {
                Action = actionName,
                Controller = controllerName,
                Area = routeValues.ContainsKey("area") ? routeValues["area"].ToString() : null,
                UserId = userId
            };

            foreach (var value in routeValues)
            {
                if (value.Key != "area")
                {
                    token.DynamicValues.Add(new TValues
                        {SecureFormToken = token, Key = value.Key, Value = value.Value});
                }
            }

            return _container.GetInstance<ITokenRepository<TToken, TValues>>().Save(token).Token;
        }

        #endregion
    }

    public interface ISecureFormTokenService
    {
        #region Abstract Methods

        Guid CreateToken(ModelMetadata modelMetadata, RouteContext routeContext);
        Guid CreateToken(RouteValueDictionary routeValues, RouteContext routeContext);

        Guid CreateTokenWithRouteValues(string actionName, string controllerName, int userId,
            IDictionary<string, object> routeValues);

        #endregion
    }
}
