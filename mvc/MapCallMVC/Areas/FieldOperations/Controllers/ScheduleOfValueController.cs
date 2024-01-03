using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class ScheduleOfValueController : ControllerBaseWithPersistence<IScheduleOfValueRepository, ScheduleOfValue, User>
    {
        #region Constructors

        public ScheduleOfValueController(
            ControllerBaseWithPersistenceArguments<IScheduleOfValueRepository, ScheduleOfValue, User> args) : base(args) {}

        #endregion
        
        #region ByScheduleOfValueCategoryId

        [HttpGet]
        public ActionResult ByScheduleOfValueCategoryId(int scheduleOfCategoryId)
        {
            return new CascadingActionResult(Repository.GetByScheduleOfValueCategoryId(scheduleOfCategoryId),
                "Description", "Id");
        }

        #endregion

    }
}