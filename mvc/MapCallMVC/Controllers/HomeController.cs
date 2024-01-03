using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data;
using ControllerBase = MMSINC.Controllers.ControllerBase;

namespace MapCallMVC.Controllers
{
    [DisplayName("Home")]
    public class HomeController : ControllerBase
    {
        #region Exposed Methods

        [RequiresAdmin]
        public ActionResult EntityLookups()
        {
            var types = typeof(AssetType).Assembly.GetTypes().Where(t => typeof(ReadOnlyEntityLookup).IsAssignableFrom(t)).OrderBy(t => t.Name);
            return View(types);
        }

        #endregion

        #region Search/Index/Show

        //
        // GET: /Home/
        // TODO: Dunno if this should die sometime or what.
        [HttpGet]
        [AllowAnonymous] // AllowAnonymous is necessary for us to view the index page while logged out.
        public ActionResult Index()
        {
            var controllers =
                GetType()
                   .Assembly.GetTypes()
                   .Where(t => !t.IsAbstract && typeof(Controller).IsAssignableFrom(t) && t != GetType())
                   .OrderBy(t => t.Namespace)
                   .ThenBy(t => t.Name)
                   .Select(t => new ControllerClass(t));

            return View(controllers);
        }

        #endregion

        #region Impersonation

        /// <summary>
        /// Impersonates a user. DO NOT allow this to be used in production ever.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private ActionResult DoImpersonation(string username)
        {
            // We allow for local requests and QA, but not production.
            if (MvcApplication.IsProduction)
            {
                return HttpNotFound("No access.");
            }

            var authRepo = _container.GetInstance<IAuthenticationRepository<User>>();
            var user = authRepo.GetUser(username);
            if (user == null)
            {
                return HttpNotFound("No such user.");
            }

            if (!user.HasAccess)
            {
                // This makes life easier for people admins using the impersonate button on the User/Show page.
                // Users on QA don't have access by default. Same for local dev db.
                user.HasAccess = true;
                authRepo.Save(user);
            }

            var authServ = _container.GetInstance<IAuthenticationService<User>>();
            if (authServ.CurrentUserIsAuthenticated)
            {
                authServ.SignOut();
                Session.Abandon();
            }
            authServ.SignIn(user.Id, false);
            var redirectUrl = TempData["REDIRECT_URL"];

            return redirectUrl == null ? (ActionResult)RedirectToAction("Index") : Redirect(redirectUrl.ToString());
        }

#if DEBUG

        /// <summary>
        /// Logs in as a user. Makes debugging locally a lot easier. Home/Impersonate/?username=username.
        /// NOTE: This action DOES NOT COMPILE for QA or Prod. This is for dev only. We don't want to expose
        /// an AllowAnonymous endpoint to QA as it would allow any user to give themselves access.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [AllowAnonymous] // AllowAnonymous necessary for us to actually login. Otherwise it'll get stuck in an authentication loop
        public ActionResult Impersonate(string username)
        {
            return DoImpersonation(username);
        }

#endif

        /// <summary>
        /// This acts the same as Impersonate, but requires an admin user to use it in the first place.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [RequiresAdmin]
        public ActionResult Pretend(string username)
        {
            return DoImpersonation(username);
        }

        #endregion

#if DEBUG

        [AllowAnonymous] // AllowAnonymous necessary so logging out doesn't cause a redirect loop if the user happens to not be authenticated.
        public ActionResult Logout()
        {
            var authServ = _container.GetInstance<IAuthenticationService<User>>();
            if (authServ.CurrentUserIsAuthenticated)
            {
                authServ.SignOut();
            }
            return Redirect(ConfigurationManager.AppSettings["loginUrl"]);
        }

        /// <summary>
        /// Only exists to allow the functional tests to check if there's a webservice running.  This should
        /// never be sent live.
        /// </summary>
        [AllowAnonymous]
        public ActionResult Blank()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
#endif

        public class ControllerClass
        {
            #region Private Members

            private readonly Type _type;
            private string _area, _name;

            #endregion

            #region Properties

            public string Name => _name ?? (_name = _type.Name.Replace("Controller", string.Empty));

            public string Area => _area ?? (_area = GetArea());

            public bool HasSearch => _type.GetMethod("Search") != null;

            #endregion

            #region Constructors

            public ControllerClass(Type type)
            {
                _type = type;
            }

            #endregion

            #region Private Methods

            private string GetArea()
            {
                return _type.Namespace.Split('.').Reverse().Skip(1).Take(1).Single().Replace("MapCallMVC", "General");
            }

            #endregion
        }

        public HomeController(ControllerBaseArguments args) : base(args) { }
    }
}
