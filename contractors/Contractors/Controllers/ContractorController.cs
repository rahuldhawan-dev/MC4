using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MMSINC;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;

namespace Contractors.Controllers
{
    public class ContractorController : Data.DesignPatterns.Mvc.ControllerBase<IRepository<Contractor>, Contractor>
    {
        [HttpGet, NoCache]
        public ActionResult ByOperatingCenterId(int id)
        {
            return new CascadingActionResult(Repository.Where(x => 
                x.OperatingCenters.Any(z => z.Id == id)).OrderBy(x => x.Name), "Name", "Id");
        }

        public ContractorController(ControllerBaseWithAuthenticationArguments<IRepository<Contractor>, Contractor, ContractorUser> args) : base(args) { }
    }
}
