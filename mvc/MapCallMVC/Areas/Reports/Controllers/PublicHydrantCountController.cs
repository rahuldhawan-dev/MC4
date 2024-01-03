using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Public Hydrants")]
    public class PublicHydrantCountController : ControllerBaseWithPersistence<IHydrantRepository, Hydrant, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.FieldServicesAssets;
        
        #endregion
        
        #region Constructor

        public PublicHydrantCountController(ControllerBaseWithPersistenceArguments<IHydrantRepository, Hydrant, User> args) : base(args) { }

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchPublicHydrantCountReport>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchPublicHydrantCountReport search)
        {
            var args = new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.GetPublicHydrantCounts(search)
            };
            return this.RespondTo((formatter) =>
            {
                formatter.View(() => ActionHelper.DoIndex(search, args));
                formatter.Excel(() => ActionHelper.DoExcel(search, args));
            });
        }
    }
}