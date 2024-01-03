using System.Configuration;
using System.Net;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Migrations;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallApi.Controllers
{
    public class HomeController : ControllerBaseWithPersistence<IRepository<ServiceInstallationPosition>, ServiceInstallationPosition, User>
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            //return new HttpStatusCodeResult(HttpStatusCode.OK);
            return new EmptyResult();
        }

#if DEBUG

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

        public HomeController(ControllerBaseWithPersistenceArguments<IRepository<ServiceInstallationPosition>, ServiceInstallationPosition, User> args) : base(args) { }
    }
}
