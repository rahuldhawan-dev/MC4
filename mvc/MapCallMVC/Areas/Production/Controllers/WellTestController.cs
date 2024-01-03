using System;
using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Production.Models.ViewModels.WellTests;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Production.Controllers
{
    public class WellTestController : ControllerBaseWithPersistence<IRepository<WellTest>, WellTest, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionWorkManagement;

        #endregion

        #region Constructors

        public WellTestController(ControllerBaseWithPersistenceArguments<IRepository<WellTest>, WellTest, User> args) : base(args) { }

        #endregion

        #region Public Methods

        #region Search/Index/Show

        [HttpGet, 
         RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchWellTestsViewModel search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, 
         RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchWellTestsViewModel viewModel)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(viewModel));
                formatter.Excel(() => ActionHelper.DoExcel(viewModel));
            });
        }

        [HttpGet, 
         RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, null, DisplayErrorMessages);
        }

        #endregion

        #region New/Create

        [HttpGet, 
         ActionBarVisible(false),
         RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(NewWellTestViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return HttpNotFound($"One or more of the provided ids could not be found.");
            }

            var createViewModel = ViewModelFactory.BuildWithOverrides<CreateWellTestViewModel>(new {
                viewModel.ProductionWorkOrder,
                viewModel.OperatingCenter,
                viewModel.Equipment
            });

            return ActionHelper.DoNew(createViewModel);
        }

        [HttpPost, 
         RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateWellTestViewModel viewModel)
        {
            return ActionHelper.DoCreate(viewModel);
        }

        #endregion

        #region Edit/Update

        [HttpGet, 
         RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<WellTestViewModel>(id);
        }

        [HttpPost, 
         RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(WellTestViewModel viewModel)
        {
            return ActionHelper.DoUpdate(viewModel);
        }

        #endregion

        #endregion

        #region Private Methods

        private void DisplayErrorMessages(WellTest model)
        {
            decimal pumpDepth, wellDepth, wellCapacityRating;

            if (!decimal.TryParse(model.PumpDepth, out pumpDepth)) DisplayErrorMessage(WellTest.Validation.INVALID_PUMP_DEPTH_MESSAGE);
            if (!decimal.TryParse(model.WellDepth, out wellDepth)) DisplayErrorMessage(WellTest.Validation.INVALID_WELL_DEPTH_MESSAGE);
            if (!decimal.TryParse(model.WellCapacityRating, out wellCapacityRating)) DisplayErrorMessage(WellTest.Validation.INVALID_WELL_CAPACITY_RATING_MESSAGE);

            if (pumpDepth > 0 && model.StaticWaterLevel > pumpDepth) DisplayErrorMessage(WellTest.Validation.STATIC_WATER_LEVEL_GREATER_THAN_PUMP_DEPTH_MESSAGE);
            if (wellDepth > 0 && model.StaticWaterLevel > wellDepth) DisplayErrorMessage(WellTest.Validation.STATIC_WATER_LEVEL_GREATER_THAN_WELL_DEPTH_MESSAGE);
            if (pumpDepth > 0 && model.PumpingWaterLevel > pumpDepth) DisplayErrorMessage(WellTest.Validation.PUMPING_WATER_LEVEL_GREATER_THAN_PUMP_DEPTH_MESSAGE);
            if (wellDepth > 0 && model.PumpingWaterLevel > wellDepth) DisplayErrorMessage(WellTest.Validation.PUMPING_WATER_LEVEL_GREATER_THAN_WELL_DEPTH_MESSAGE);
            if (wellCapacityRating > 0 && model.PumpingRate > wellCapacityRating) DisplayErrorMessage(WellTest.Validation.PUMPING_RATE_GREATER_THAN_WELL_CAPACITY_RATING_MESSAGE);
        }

        #endregion
    }
}