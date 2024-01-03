using System.Web.Mvc;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC.Controllers;

namespace Contractors.Controllers
{
    public class MaterialController : Data.DesignPatterns.Mvc.ControllerBase<IMaterialRepository, Material>
    {
        public MaterialController(ControllerBaseWithAuthenticationArguments<IMaterialRepository, Material, ContractorUser> args) : base(args) {}

        [HttpGet]
        public ActionResult MaterialSearchByMaterialUsedId(string search, int materialUsedId)
        {
            var materials =
                new SelectList(
                        Repository.GetBySearchAndMaterialUsedId(
                            search,
                            materialUsedId), 
                        "Id", 
                        "FullDescription");
            return Json(new {Result = "OK", Options = materials}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MaterialSearchByWorkOrderId(string search, int workOrderId)
        {
            var materials =
                new SelectList(
                        Repository.GetBySearchAndWorkOrderId(
                            search,
                            workOrderId),
                        "Id",
                        "FullDescription");
            return Json(new { Result = "OK", Options = materials }, JsonRequestBehavior.AllowGet);
        }
    }
}
