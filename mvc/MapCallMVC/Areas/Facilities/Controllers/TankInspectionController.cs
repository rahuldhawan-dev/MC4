using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Helpers;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Facilities.Controllers
{
    public class TankInspectionController : ControllerBaseWithPersistence<TankInspection, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionWorkManagement;

        #endregion

        #region Constructors

        public TankInspectionController(ControllerBaseWithPersistenceArguments<IRepository<TankInspection>, TankInspection, User> args) : base(args) { }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add);
                    this.AddDropDownData<TankInspectionType>(
                        filter: x =>
                            x.Id == TankInspectionType.Indices.INSPECTION_SITE_OBSERVATION ||
                            x.Id == TankInspectionType.Indices.INSPECTION_SITE_OBSERVATION_DRONE, x => x.Id,
                        x => x.Description);
                    break;
            }
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchTankInspection search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchTankInspection search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [ActionBarVisible(false)]
        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int productionWorkOrderId)
        {
            var order = _container.GetInstance<ProductionWorkOrderRepository>().Find(productionWorkOrderId);
            var model = ViewModelFactory.Build<CreateTankInspection>();
            model.SetValuesFromProductionWorkOrder(order);

            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateTankInspection model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditTankInspection>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditTankInspection model)
        {
            var modelQuestions = model.TankInspectionQuestions;
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnError = () => {
                    model.SetDefaults();
                    foreach (var question in modelQuestions)
                    {
                        var oldQuestion = model.TankInspectionQuestions.Single(x => x.TankInspectionQuestionType == question.TankInspectionQuestionType);
                        oldQuestion.ObservationAndComments = question.ObservationAndComments;
                        oldQuestion.CorrectiveWoDateCompleted = question.CorrectiveWoDateCompleted;
                        oldQuestion.CorrectiveWoDateCreated = question.CorrectiveWoDateCreated;
                        oldQuestion.RepairsNeeded = question.RepairsNeeded;
                        oldQuestion.TankInspectionAnswer = question.TankInspectionAnswer;
                    }
                    return null;
                }
            });
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #endregion
    }
}