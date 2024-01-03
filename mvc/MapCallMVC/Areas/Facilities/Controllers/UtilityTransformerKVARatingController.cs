using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class UtilityTransformerKVARatingController : ControllerBaseWithPersistence<UtilityTransformerKVARating, User>
    {
        #region Constructors

        public UtilityTransformerKVARatingController(ControllerBaseWithPersistenceArguments<IRepository<UtilityTransformerKVARating>, UtilityTransformerKVARating, User> args) : base(args) { }

        #endregion

        #region Public Methods

        [HttpGet]
        public ActionResult ByVoltage(int id)
        {
            var data = Repository.Where(x => x.Voltages.Any(y => y.Id == id));
            return new CascadingActionResult(data, "Description", "Id");
        }

        #endregion
    }
}