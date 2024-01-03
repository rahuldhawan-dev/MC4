using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace Contractors.Controllers
{
    public class SmallMeterLocationController : Data.DesignPatterns.Mvc.ControllerBase<IRepository<SmallMeterLocation>, SmallMeterLocation>
    {
        #region Constructors

        public SmallMeterLocationController(ControllerBaseWithAuthenticationArguments<IRepository<SmallMeterLocation>, SmallMeterLocation, ContractorUser> args) : base(args) { }
            
        #endregion

        #region Public Methods

        [HttpGet, AllowAnonymous]
        public ActionResult ByMeterSupplementalLocationId(int id)
        {
            return new CascadingActionResult(
                Repository.Where(x => x.MeterSupplementalLocations.Any(z => z.Id == id)), "Description", "Id");
        }
        
        #endregion
    }
}