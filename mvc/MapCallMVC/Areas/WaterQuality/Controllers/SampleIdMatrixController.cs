using System.ComponentModel;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    [DisplayName("Sample ID Matrix")]
    public class SampleIdMatrixController : ControllerBaseWithPersistence<SampleIdMatrix, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.WaterQualityGeneral;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.Search:
                   // this.AddOperatingCenterDropDownData();
                    this.AddDropDownData<WaterConstituent>();
                    // bug 3987 asked for this specifically for searching.
                    var pwsids = _container.GetInstance<IPublicWaterSupplyRepository>().GetAllFilteredByWaterQualityGeneralRole();
                    this.AddDropDownData(pwsids, x => x.Id, x => x.Description);
                    break;
                case ControllerAction.Edit:
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownData();
                    this.AddDropDownData<WaterConstituent>();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchSampleIdMatrix search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchSampleIdMatrix search)
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
            return ActionHelper.DoNew(new CreateSampleIdMatrix(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateSampleIdMatrix model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditSampleIdMatrix>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditSampleIdMatrix model)
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

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult BySampleSiteId(int sampleSiteId)
        {
            return new CascadingActionResult<SampleIdMatrix, SampleIdMatrixDisplayItem>(Repository.Where(x => x.SampleSite.Id == sampleSiteId), "Display", "Id");
        }

        #region Constructors

        public SampleIdMatrixController(ControllerBaseWithPersistenceArguments<IRepository<SampleIdMatrix>, SampleIdMatrix, User> args) : base(args) {}

		#endregion
    }
}