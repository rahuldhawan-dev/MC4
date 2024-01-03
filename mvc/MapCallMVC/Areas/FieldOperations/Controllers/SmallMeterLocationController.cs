using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class SmallMeterLocationController : ControllerBaseWithPersistence<IRepository<SmallMeterLocation>, SmallMeterLocation, User>
    {
        #region Constructors

        public SmallMeterLocationController(ControllerBaseWithPersistenceArguments<IRepository<SmallMeterLocation>, SmallMeterLocation, User> args) : base(args) {}

        #endregion

        #region Public Methods

        [HttpGet]
        public ActionResult ByMeterSupplementalLocationId(int id)
        {
            return new CascadingActionResult(
                Repository.Where(x => x.MeterSupplementalLocations.Any(z => z.Id == id)), "Description", "Id");
        }

        #endregion
    }
}