using System;
using System.Net;
using System.Web.Mvc;
using MMSINC.Authentication;
using MMSINC.Helpers;
using MMSINC.Utilities;
using StructureMap;

namespace MMSINC.Metadata
{
    public class SecureTokenFilter<TToken, TValues> : IActionFilter
        where TToken : ISecureFormToken<TToken, TValues>
        where TValues : ISecureFormDynamicValue<TValues, TToken>
    {
        #region Constants

        internal const string FORM_KEY = FormBuilder.SECURE_FORM_HIDDEN_FIELD_NAME;

        internal struct ErrorMessages
        {
            #region Constants

            public const string USER_IS_NOT_AUTHENTICATED = "User is not authenticated.",
                                INVALID_SECURITY_TOKEN = "Security token not found or is invalid.",
                                TOKEN_IS_EXPIRED = "Security token has expired.",
                                TOKEN_DOES_NOT_MATCH_ROUTE = "Security token does not match route.",
                                TOKEN_DOES_NOT_MATCH_USER = "Security token is invalid for the current user.",
                                NOT_AUTHORIZED = "Not authorized.";

            #endregion
        }

        #endregion

        #region Fields

        private IContainer _container;

        #endregion

        #region Properties

        public string ViewName { get; set; }

        public ITokenRepository<TToken, TValues> TokenRepository =>
            _container.GetInstance<ITokenRepository<TToken, TValues>>();

        #endregion

        #region Constructor

        public SecureTokenFilter(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Private Methods

        private ActionResult GetAuthorizationResultForSecuredForm(ActionExecutingContext filterContext,
            out TToken token)
        {
            // TODO: STATUS CODE STATUS CODE STATUS CODE
            Func<string, ActionResult> createResult = (errMessage) => {
                var result = filterContext.IsChildAction ? (ViewResultBase)new PartialViewResult() : new ViewResult();
                result.ViewName = ViewName;
                result.ViewData = new ViewDataDictionary();
                result.ViewData["Message"] = errMessage;
                filterContext.Result = result;
                return new InvalidSecureFormTokenViewResult(result);
            };

            token = TryFindToken(filterContext, TokenRepository);

            if (token == null)
            {
                return createResult(ErrorMessages.INVALID_SECURITY_TOKEN);
            }

            var authServ = _container.GetInstance<IAuthenticationService<IAdministratedUser>>();
            if (token.UserId != authServ.CurrentUser.Id)
            {
                return createResult(ErrorMessages.TOKEN_DOES_NOT_MATCH_USER);
            }

            if (token.IsExpired(_container.GetInstance<IDateTimeProvider>()))
            {
                return createResult(ErrorMessages.TOKEN_IS_EXPIRED);
            }

            if (!TokenMatchesCurrentRoute(filterContext, token))
            {
                return createResult(ErrorMessages.TOKEN_DOES_NOT_MATCH_ROUTE);
            }

            // Don't return any result if everything should keep going as planned.
            return null;
        }

        /// <summary>
        /// Returns true if the token's route values match the route for this request.
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static bool TokenMatchesCurrentRoute(ActionExecutingContext filterContext, TToken token)
        {
            var rc = new RouteContext(filterContext.RequestContext);

            // RouteContext.AreaName will return null if a controller doesn't belong
            // to an area, but the token will have an empty string. 
            var rcArea = rc.AreaName ?? string.Empty;
            var tokenArea = token.Area ?? string.Empty;

            var ignoreCase = StringComparison.InvariantCultureIgnoreCase;
            return string.Equals(rc.ActionName, token.Action, ignoreCase) &&
                   string.Equals(rc.ControllerName, token.Controller, ignoreCase) &&
                   string.Equals(rcArea, tokenArea, ignoreCase);
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Returns the guid secure form token if one is found for the request.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Guid? TryFindTokenId(ControllerContext context)
        {
            var request = context.HttpContext.Request;
            var token = request.Form[FORM_KEY];
            token = string.IsNullOrWhiteSpace(token) ? request.QueryString[FORM_KEY] : token;
            Guid tokenGuid;
            if (!Guid.TryParse(token, out tokenGuid))
            {
                return null;
            }

            return tokenGuid;
        }

        /// <summary>
        /// Attempts to return a SecureFormToken if a valid token id is provided
        /// for the request and a match still exists in the database.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static TToken TryFindToken(ControllerContext context, ITokenRepository<TToken, TValues> tokenRepo)
        {
            var tokenId = TryFindTokenId(context);
            if (!tokenId.HasValue)
            {
                return default(TToken);
            }

            return tokenRepo.FindByToken(tokenId.Value);
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            TToken token;

            // another filter (like authorization filters) has already handled it
            if (filterContext.Result != null ||
                !FormBuilder.RequiresSecureForm(filterContext.ActionDescriptor) ||
                // non-null means the token was somehow invalid
                (filterContext.Result = GetAuthorizationResultForSecuredForm(filterContext, out token)) != null)
            {
                return;
            }

            // (over)write the posted values which should be secured to prevent fuckery
            foreach (var value in token.DynamicValues)
            {
                filterContext.ActionParameters[value.Key] = value.DeserializedValue;
            }

            TokenRepository.Delete(token);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // no need to do anything here
        }

        #endregion

        #region Private Classes

        internal class InvalidSecureFormTokenViewResult : ActionResult
        {
            public ViewResultBase ViewResult { get; set; }

            public InvalidSecureFormTokenViewResult(ViewResultBase viewResult)
            {
                ViewResult = viewResult;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                // Without this, QA/prod will override the custom errors and return plain text/statuscode-only errors.
                context.HttpContext.Response.TrySkipIisCustomErrors = true;
                ViewResult.ExecuteResult(context);
            }
        }

        #endregion
    }
}
