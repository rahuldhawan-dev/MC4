using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class NearMissController : ControllerBaseWithPersistence<INearMissRepository, NearMiss, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsHealthAndSafety;

        #endregion
        
        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchNearMiss search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet]
        public ActionResult ReportedByEmployeePartialFirstOrLastName(string partialFirstOrLastName)
        {
            var results = Repository.Where(u => u.ReportedBy.Contains(partialFirstOrLastName))
                                    .Select(u => new { u.ReportedBy }).Distinct();
            return new AutoCompleteResult(results, "ReportedBy", "ReportedBy");
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchNearMiss search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditNearMiss>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditNearMiss model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Constructors

        public NearMissController(ControllerBaseWithPersistenceArguments<INearMissRepository, NearMiss, User> args) : base(args) { }

        #endregion
    }
}