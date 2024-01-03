using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    public class WaterSampleComplianceFormController : ControllerBaseWithPersistence<WaterSampleComplianceForm, User>
    {
        #region Consts

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;
        
        #endregion

        #region Constructors

        public WaterSampleComplianceFormController(ControllerBaseWithPersistenceArguments<IRepository<WaterSampleComplianceForm>, WaterSampleComplianceForm, User> args) : base(args) { }

        #endregion

        #region Public Methods

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchWaterSampleComplianceForm>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchWaterSampleComplianceForm search)
        {
            return ActionHelper.DoIndex(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        /// <param name="id">The PWSID value. This parameter should really be called "pwsid" but regression tests get messed up.</param>
        /// <returns></returns>
        [HttpGet, RequiresRole(ROLE, RoleActions.Add), ActionBarVisible(false)]
        public ActionResult New(int id)
        {
            return ActionHelper.DoNew(new CreateWaterSampleComplianceForm(_container) {
                PublicWaterSupply = id
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateWaterSampleComplianceForm model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditWaterSampleComplianceForm>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditWaterSampleComplianceForm model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion
    }
}