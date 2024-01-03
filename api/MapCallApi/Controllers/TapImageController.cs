using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility;
using MapCallApi.Models;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallApi.Controllers
{
    public class TapImageController : ControllerBaseWithPersistence<ITapImageRepository, TapImage, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.FieldServicesImages;

        #endregion

        #region Private Members

        private IUrlHelper _urlHelper;

        #endregion

        #region Constructors

        public TapImageController(ControllerBaseWithPersistenceArguments<ITapImageRepository, TapImage, User> args,
            IUrlHelper urlHelper) : base(args)
        {
            _urlHelper = urlHelper;
        }

        #endregion

        #region Private Methods
        
        private string GetPdfUrl(string action, string controller, object routeValues)
        {
            var url = _urlHelper.Action(action, controller, routeValues);
            return $"{Request.Url.Scheme}://{Request.Url.Authority}{url}";
        }

        #endregion

        #region Index

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult Index(SearchTapImage model)
        {
            return Json(
                Repository.Search(model).Select(ti => new {
                    URL = GetPdfUrl("Show", "TapImage", new { id = ti.Id, area = "FieldOperations", frag = "PDF" }),
                    DateCompleted = string.Format(CommonStringFormats.DATE, ti.DateCompleted),
                    ti.PremiseNumber,
                    ti.ApartmentNumber,
                    ti.StreetNumber,
                    ti.Street,
                    ti.StreetSuffix,
                    ti.FullStreetName,
                    Town = ti.Town.ToString(),
                    ti.TownSection,
                    State = ti.Town?.State?.Abbreviation,
                    ServiceMaterial = ti.ServiceMaterial?.Description,
                    ServiceSize=ti.ServiceSize?.Size,
                    ti.ServiceNumber
                }), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
