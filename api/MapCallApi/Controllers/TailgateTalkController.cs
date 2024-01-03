using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallApi.Models;
using MMSINC.Controllers;
using MMSINC.Data;

namespace MapCallApi.Controllers
{
    public class TailgateTalkController : ControllerBaseWithPersistence<ITailgateTalkRepository, TailgateTalk, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsHealthAndSafety;

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchTailgateTalk search)
        {
            if (search.HeldOn == null || search.HeldOn.Operator != RangeOperator.Between ||
                search.HeldOn.End == null || search.HeldOn.End.Value
                    .Subtract(search.HeldOn.Start.Value).TotalDays > 30)
            {
                throw new InvalidOperationException(
                    "HeldOn must be a 'between' search of a month or less.");
            }
            search.EnablePaging = false;
            return Json(Repository.Search(search).Select(t => new {
                t.Id,
                OperatingCenter = t.OperatingCenter?.ToString(),
                t.HeldOn,
                t.TrainingTimeHours,
                Category = t.Category?.ToString(),
                Topic = t.Topic?.ToString(),
                PresentedBy = t.PresentedBy?.ToString(),
                t.OrmReferenceNumber
            }), JsonRequestBehavior.AllowGet);
        }

        #endregion

        public TailgateTalkController(ControllerBaseWithPersistenceArguments<ITailgateTalkRepository, TailgateTalk, User> args) : base(args)
        {
        }
    }
}