using System.ComponentModel;
using System.Web.Mvc;
using MMSINC.Authentication;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Admin.Controllers
{
    [RequiresAdmin, DisplayName("Admin")]
    public class AdminHomeController : MMSINC.Controllers.ControllerBase
    {
        #region Actions

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        public AdminHomeController(ControllerBaseArguments args) : base(args) { }
    }
}