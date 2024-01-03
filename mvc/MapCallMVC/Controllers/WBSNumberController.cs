using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class WBSNumberController : ControllerBaseWithPersistence<IRepository<WBSNumber>, WBSNumber, User>
    {
        #region Exposed Methods

        [HttpGet]
        public ActionResult ByOperatingCenterId(int operatingCenterId)
        {
            var operatingCenter = _container.GetInstance<IOperatingCenterRepository>().Find(operatingCenterId);

            var result = new CascadingActionResult(Repository.GetAll(), "Description", "Id");

            if (operatingCenter != null && operatingCenter.DefaultServiceReplacementWBSNumber != null)
            {
                result.SelectedValue = operatingCenter.DefaultServiceReplacementWBSNumber.Id;
            }

            return result;
        }

        #endregion

        public WBSNumberController(ControllerBaseWithPersistenceArguments<IRepository<WBSNumber>, WBSNumber, User> args) : base(args) {}
    }
}