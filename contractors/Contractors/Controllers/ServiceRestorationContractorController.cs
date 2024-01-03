using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;

namespace Contractors.Controllers 
{
    public class ServiceRestorationContractorController : Data.DesignPatterns.Mvc.ControllerBase<IRepository<ServiceRestorationContractor>, ServiceRestorationContractor>
    {
        public ServiceRestorationContractorController(ControllerBaseWithAuthenticationArguments<IRepository<ServiceRestorationContractor>, ServiceRestorationContractor, ContractorUser> args) : base(args) { }

        [HttpGet, NoCache]
        public ActionResult ByOperatingCenterId(int id)
        {
            return new CascadingActionResult(Repository.Where(x => x.OperatingCenter.Id == id), "Contractor", "Id");
        }
    }
}
