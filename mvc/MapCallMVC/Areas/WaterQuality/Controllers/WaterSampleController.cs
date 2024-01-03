using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    public class WaterSampleController : ControllerBaseWithPersistence<WaterSample, User>
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
                    this.AddDropDownData<WaterConstituent>();
                    // bug 3987 asked for this specifically for searching.
                    var pwsids = _container.GetInstance<IPublicWaterSupplyRepository>().GetAllFilteredByWaterQualityGeneralRole();
                    this.AddDropDownData(pwsids, x => x.Id, x => x.Description);
                    break;
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownData(x => x.IsActive);
                    break;
                case ControllerAction.Edit:
                    this.AddDynamicDropDownData<SampleIdMatrix, SampleIdMatrixDisplayItem>("SampleIdMatrix");
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchWaterSample search = null)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchWaterSample search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(x => new {
                        WaterSampleID = x.Id,
                        OperatingCenter = x.SampleIdMatrix?.SampleSite?.OperatingCenter,
                        PWSID = x.SampleIdMatrix?.SampleSite?.PublicWaterSupply?.Identifier,
                        x.SampleIdMatrix?.SampleSite?.LeadCopperTierClassification,
                        x.SampleDate,
                        Description = x.SampleIdMatrix?.SampleSite?.LocationNameDescription,
                        WaterConstituent = x.SampleIdMatrix?.WaterConstituent,
                        Parameter = x.SampleIdMatrix?.Parameter, 
                        x.NonDetect,
                        x.IsInvalid,
                        x.SampleValue,
                        BactiSite = x.SampleIdMatrix?.SampleSite?.BactiSite,
                        LeadCopperSite = x.SampleIdMatrix?.SampleSite?.LeadCopperSite,
                        x.AnalysisPerformedBy,
                        Town = x.SampleIdMatrix?.SampleSite?.Town,
                        ZipCode = x.SampleIdMatrix?.SampleSite?.ZipCode
                    });
                    return this.Excel(results);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? id)
        {
            return ActionHelper.DoNew(new CreateWaterSample(_container) {SampleSite = id});
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateWaterSample model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditWaterSample>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditWaterSample model)
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

        public WaterSampleController(ControllerBaseWithPersistenceArguments<IRepository<WaterSample>, WaterSample, User> args) : base(args) {}

        #endregion
    }
}