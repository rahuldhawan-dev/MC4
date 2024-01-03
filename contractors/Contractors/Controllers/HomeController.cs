using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using StructureMap;

namespace Contractors.Controllers
{
    public class HomeController : MMSINC.Controllers.ControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Layout()
        {
            this.AddDropDownData<ITownRepository, Town>("DropDown", s => s.Id, s => s.ShortName);
            this.AddDropDownData<ITownRepository, Town>("CheckBoxing", s => s.Id, s => s.ShortName);
            var model = new LayoutModel();
            model.TableRows.Add(new LayoutModel());
            model.TableRows.Add(new LayoutModel());
            model.TableRows.Add(new LayoutModel());
            model.TableRows.Add(new LayoutModel());

            model.CheckBoxing = new List<int>();
            model.CheckBoxing.Add(1);
            model.CheckBoxing.Add(3);

            model.MultilineText =
                @"This is a long incoherent string of text: ooaoe g ogoag ho goase gouasghasoueg haosg uhogaheo ga ohaesg ouasegouh ao ghaoeuhnagon aseona gonagogao oa

And here's a line break!
";
            return View(model);
        }

#if DEBUG
        /// <summary>
        /// Logs in as a user. Makes debugging locally a lot easier. Home/Impersonate/?username=username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Impersonate(string userName)
        {
            if (!Request.IsLocal || MvcApplication.IsProduction || MvcApplication.IsStaging)
            {
                return HttpNotFound("No access.");
            }
            var authRepo = _container.GetInstance<IAuthenticationRepository<ContractorUser>>();
            var user = authRepo.GetUser(userName);
            if (user == null)
            {
                return HttpNotFound("No such user.");
            }

            var authServ = _container.GetInstance<IAuthenticationService<ContractorUser>>();
            if (authServ.CurrentUserIsAuthenticated)
            {
                authServ.SignOut();
                Session.Abandon();
            }
            authServ.SignIn(user.Id, false);
            var redirectUrl = TempData["REDIRECT_URL"];

            return redirectUrl == null ? (ActionResult)RedirectToAction("Index") : Redirect(redirectUrl.ToString());
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

        public HomeController(ControllerBaseArguments args) : base(args) { }
    }
}
