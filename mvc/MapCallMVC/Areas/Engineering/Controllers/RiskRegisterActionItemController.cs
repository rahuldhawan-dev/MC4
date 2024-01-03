using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Engineering.Models.ViewModels.RiskRegisterActionItems;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Engineering.Controllers
{
    public class RiskRegisterActionItemController : ControllerBaseWithPersistence<ActionItem<RiskRegisterAsset>, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EngineeringRiskRegister;

        #endregion

        #region Constructors

        public RiskRegisterActionItemController(ControllerBaseWithPersistenceArguments<IRepository<ActionItem<RiskRegisterAsset>>, ActionItem<RiskRegisterAsset>, User> args) : base(args) { }

        #endregion

        #region Exposed Methods

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchRiskRegisterActionItem search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchRiskRegisterActionItem search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));

                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(e => new {
                        e.Id,
                        RiskRegisterAssetId = e.LinkedId,
                        e.Entity.OperatingCenter.State,
                        e.Entity.OperatingCenter,
                        e.ActionItem.Type,
                        ResponsibleOwner = e.ActionItem.ResponsibleOwner?.FullName,
                        e.ActionItem.NotListedType,
                        e.ActionItem.TargetedCompletionDate,
                        e.ActionItem.DateCompleted
                    });
                    return this.Excel(results);
                });
            });
        }

        #endregion
    }
}