using System.Reflection;
using System.Web.Mvc;
using MMSINC.Utilities;

namespace MMSINC.Testing
{
    /// <summary>
    /// A simple controller with two methods, one that will always
    /// be authorized for a user and one that will never be authorized
    /// for a user.
    /// </summary>
    public class ActionAuthorizationController : Controller
    {
        #region Properties

        public static MemberInfo AuthorizedActionMemberInfo
        {
            get { return Expressions.GetMember((ActionAuthorizationController aac) => aac.AuthorizedAction()); }
        }

        public static MemberInfo UnauthorizedActionMemberInfo
        {
            get { return Expressions.GetMember((ActionAuthorizationController aac) => aac.UnauthorizedAction()); }
        }

        public static MemberInfo HttpGetAuthorizedActionMemberInfo
        {
            get { return Expressions.GetMember((ActionAuthorizationController aac) => aac.HttpGetAuthorizedAction()); }
        }

        public static MemberInfo HttpPostAuthorizedActionMemberInfo
        {
            get { return Expressions.GetMember((ActionAuthorizationController aac) => aac.HttpPostAuthorizedAction()); }
        }

        #endregion

        [FakeAuthorize(true)]
        public ActionResult AuthorizedAction()
        {
            return null;
        }

        [FakeAuthorize(false)]
        public ActionResult UnauthorizedAction()
        {
            return null;
        }

        [HttpGet]
        [FakeAuthorize(true)]
        public ActionResult HttpGetAuthorizedAction()
        {
            return null;
        }

        [HttpPost]
        [FakeAuthorize(true)]
        public ActionResult HttpPostAuthorizedAction()
        {
            return null;
        }
    }
}
