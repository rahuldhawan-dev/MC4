using System.Web.Mvc;

namespace MMSINC.Testing
{
    /// <summary>
    /// A simple authorization attribute for testing things that authorize and stuff.
    /// </summary>
    public class FakeAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// NOTE: If you set this value and you're testing this attribute on a class/method(as opposed to
        /// adding it to GlobalFilters) then this value will never be reported correctly. You must use the
        /// constructor call in those cases.
        /// </summary>
        public bool IsAuthorized { get; set; }

        /// <param name="isAuthorized">Set to true if the current user is always authorized, false otherwise.</param>
        public FakeAuthorizeAttribute(bool isAuthorized)
        {
            IsAuthorized = isAuthorized;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!IsAuthorized)
            {
                filterContext.Result = new EmptyResult();
            }
        }
    }
}
