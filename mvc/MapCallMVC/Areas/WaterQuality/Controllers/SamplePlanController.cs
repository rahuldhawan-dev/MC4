using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    public class SamplePlanController : ControllerBaseWithPersistence<SamplePlan, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.WaterQualityGeneral;

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddDynamicDropDownData<PublicWaterSupply, PublicWaterSupplyDisplayItem>("PWSID");
            }
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchSamplePlan>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoShow(id));
                f.Pdf(() => ActionHelper.DoPdf(id));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchSamplePlan search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateSamplePlan(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateSamplePlan model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditSamplePlan>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditSamplePlan model)
        {
            return ActionHelper.DoUpdate(model);
        }
		
        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

		#region Constructors

        public SamplePlanController(ControllerBaseWithPersistenceArguments<IRepository<SamplePlan>, SamplePlan, User> args) : base(args) {}

		#endregion
    }
}