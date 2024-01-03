using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Production.Views
{
    public class UtilityCompanyController : ControllerBaseWithPersistence<IRepository<UtilityCompany>, UtilityCompany, User>
    {
        #region ByStateId

        [HttpGet]
        public ActionResult ByStateId(int stateId)
        {
            return new CascadingActionResult(Repository.Where(x => x.State.Id == stateId), "Description", "Id");
        }

        #endregion

        #region Constructors

        public UtilityCompanyController(ControllerBaseWithPersistenceArguments<IRepository<UtilityCompany>, UtilityCompany, User> args) : base(args) { }

        #endregion
    }
}
