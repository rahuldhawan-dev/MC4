using System.ComponentModel;
using System.Web.Mvc;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Reports")]
    public class ReportsHomeController : MMSINC.Controllers.ControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ReportsHomeController(ControllerBaseArguments args) : base(args) { }
    }
}