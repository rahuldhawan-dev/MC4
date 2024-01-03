using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class FacilityFacilityAreaController : ControllerBaseWithPersistence<FacilityFacilityArea, User>
    {
        #region ByFacilityId

        [HttpGet]
        public ActionResult FacilityAreaByFacilityId(int facilityId)
        {
            var query = Repository.Where(x => x.Facility.Id == facilityId).Select(x => x.FacilityArea).Distinct().ToList();

            return new CascadingActionResult(query, "Description", "Id") {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult FacilitySubAreaByFacilityAreaId(int facilityId, int facilityAreaId)
        {
            var query = Repository.Where(x => x.Facility.Id == facilityId && x.FacilityArea.Id == facilityAreaId && x.FacilitySubArea != null).Select((x => x.FacilitySubArea)).ToList();

            return new CascadingActionResult(query, "Description", "Id") {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        
        [HttpGet]
        public ActionResult ByFacilityId(int facilityId)
        {
            return new CascadingActionResult<FacilityFacilityArea, FacilityFacilityAreaDisplayItem>(
                Repository.Where(x => x.Facility.Id == facilityId), "Display", "Id") {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        
        [HttpGet]
        public ActionResult ByFacilityIds(int[] facilityIds)
        {
            return new CascadingActionResult<FacilityFacilityArea, FacilityFacilityAreaDisplayItem>(
                Repository.Where(x => facilityIds.Contains(x.Facility.Id)), "Display", "Id") {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion

        #region Constructors

        public FacilityFacilityAreaController(ControllerBaseWithPersistenceArguments<IRepository<FacilityFacilityArea>, FacilityFacilityArea, User> args)
            : base(args) { }

        #endregion
    }
}
