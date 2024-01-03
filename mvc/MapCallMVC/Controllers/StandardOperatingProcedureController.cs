using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Controllers
{
    public class StandardOperatingProcedureController : ControllerBaseWithPersistence<StandardOperatingProcedure, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ManagementGeneral;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            Action allActions = () =>
            {
                this.AddDropDownData<SOPSection>("Section");
                this.AddDropDownData<SOPSubSection>("SubSection");
                this.AddOperatingCenterDropDownData();
                this.AddDropDownData<FunctionalArea>();
                this.AddDropDownData<SOPStatus>("Status");
                this.AddDropDownData<SOPCategory>("Category");
                this.AddDropDownData<SOPSystem>("System");
            };

            switch (action)
            {
                case ControllerAction.Show:
                    this.AddDropDownData<RecurringFrequencyUnit>("FrequencyUnit");
                    this.AddDropDownData<PositionGroupCommonName>("PositionGroupCommonName");
                    this.AddDropDownData<TrainingModule>(x => x.GetAllSorted(y => y.Title), x => x.Id, x => x.Title);
                    break;

                case ControllerAction.Search:
                    allActions();
                    break;
                case ControllerAction.Edit:
                    allActions();
                    this.AddDynamicDropDownData<Facility, FacilityDisplayItem>();
                    this.AddDropDownData<PolicyPractice>();

                    break;
                case ControllerAction.New:
                    allActions();
                    this.AddDynamicDropDownData<Facility, FacilityDisplayItem>();
                    this.AddDropDownData<PolicyPractice>();

                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchStandardOperatingProcedure search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchStandardOperatingProcedure search)
        {
            Func<IEnumerable<StandardOperatingProcedure>> resultsFn = () => {
                search.EnablePaging = false;
                return Repository.Search(search).ToList();
            };

            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(e => new {
                        e.Id,
                        e.FunctionalArea,
                        e.Section,
                        e.SubSection,
                        e.Status,
                        e.Category,
                        e.OperatingCenter,
                        e.Facility,
                        e.DateApproved,
                        e.DateIssued,
                        e.Revision,
                        e.Description,
                        e.PsmTcpa,
                        e.Dpcc
                    });
                    return this.Excel(results);
                });
                formatter.Pdf(() => new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "PdfIndex", resultsFn()));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateStandardOperatingProcedure(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateStandardOperatingProcedure model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditStandardOperatingProcedure>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditStandardOperatingProcedure model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Add/Remove questions

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult AddStandardOperatingProcedureQuestion(AddStandardOperatingProcedureQuestion model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult RemoveStandardOperatingProcedureQuestion(RemoveStandardOperatingProcedureQuestion model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Add/Remove PGCN requirements

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddStandardOperatingProcedurePositionGroupCommonNameRequirement(AddSOPPGCN model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveStandardOperatingProcedurePositionGroupCommonNameRequirement(RemoveSOPPGCN model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Add/Remove training modules

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddTrainingModule(AddSOPTrainingModule model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveTrainingModule(RemoveSOPTrainingModule model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        public StandardOperatingProcedureController(ControllerBaseWithPersistenceArguments<IRepository<StandardOperatingProcedure>, StandardOperatingProcedure, User> args) : base(args) { }
    }
}
