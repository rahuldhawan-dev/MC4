using System.Web.Mvc;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Environmental.Models.ViewModels.PublicWaterSupplyFirmCapacities;
using System.ComponentModel;
using System.Linq;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    [DisplayName("PWSID Capacities")]
    public class PublicWaterSupplyFirmCapacityController : ControllerBaseWithPersistence<PublicWaterSupplyFirmCapacity, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EngineeringPWSIDCapacity;

        #endregion

        #region Constructors
        
        public PublicWaterSupplyFirmCapacityController(ControllerBaseWithPersistenceArguments<IRepository<PublicWaterSupplyFirmCapacity>, PublicWaterSupplyFirmCapacity, User> args) : base(args) { }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action) =>
            this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchPublicWaterSupplyFirmCapacityViewModel viewModel)
        {
            return ActionHelper.DoSearch(viewModel);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchPublicWaterSupplyFirmCapacityViewModel viewModel)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(viewModel));
                formatter.Excel(() => {
                    var results = Repository.Search(viewModel).Select(x => new {
                        x.PublicWaterSupply.State,
                        x.PublicWaterSupply,
                        x.CurrentSystemPeakDailyDemandMGD,
                        x.CurrentSystemPeakDailyDemandYearMonth,
                        x.TotalSystemSourceCapacityMGD,
                        x.TotalCapacityFacilitySumMGD,
                        x.FirmCapacityMultiplier,
                        x.FirmSystemSourceCapacityMGD,
                        DateUpdated = x.UpdatedAt
                    });
                    return this.Excel(results);
                });
            });
        }
        
        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<CreatePublicWaterSupplyFirmCapacityViewModel>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreatePublicWaterSupplyFirmCapacityViewModel viewModel)
        {
            return ActionHelper.DoCreate(viewModel);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<PublicWaterSupplyFirmCapacityViewModel>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(PublicWaterSupplyFirmCapacityViewModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion
    }
}