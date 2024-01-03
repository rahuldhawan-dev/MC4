using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;

namespace Contractors.Controllers
{
    public class ServiceMaterialController : Data.DesignPatterns.Mvc.ControllerBase<IRepository<ServiceMaterial>, ServiceMaterial>
    {

        public ServiceMaterialController(ControllerBaseWithAuthenticationArguments<IRepository<ServiceMaterial>, ServiceMaterial, ContractorUser> args) : base(args) { }

        #region Public Methods

        [HttpGet, NoCache]
        public ActionResult ByOperatingCenterId(int id)
        {
            var results = Repository.Where(x =>
                x.OperatingCentersServiceMaterials.Any(y =>
                    y.OperatingCenter.Id == id));
            return new CascadingActionResult(
                Repository.Where(x => x.OperatingCentersServiceMaterials.Any(y => y.OperatingCenter.Id == id)), "Description", "Id");
        }

        #endregion
    }
}
