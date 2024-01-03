using System;
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
    public class UnionContractProposalController : ControllerBaseWithPersistence<UnionContractProposal, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.HumanResourcesUnion;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            Action allActions = () => {
                this.AddDynamicDropDownData<UnionContract, UnionContractDisplayItem>("Contract");
                this.AddDropDownData<UnionContractProposalStatus>("Status");
                this.AddDropDownData<UnionContractProposalNegotiationTiming>("NegotiationTiming");
                this.AddDropDownData<UnionContractProposalGrouping>("Grouping");
            };

            Action newEditActions = () => {
                this.AddDropDownData<UnionContractProposalPrioritization>("Prioritization");
                this.AddDropDownData<UnionContractProposalPrintingSequence>("PrintingSequence");
                this.AddDropDownData<UnionContractProposalAffectedDepartment>("AffectedDepartment");
                this.AddDropDownData<ManagementOrUnion>();
            };

            switch (action)
            {
                case ControllerAction.Search:
                    allActions();
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);//, key: "OperatingCenterList");
                    this.AddDropDownData<Local>(l => l.Id, l => l.Name);
                    this.AddDropDownData<Union>(x => x.Id, x => x.BargainingUnit);
                    this.AddDropDownData<ManagementOrUnion>();
                    this.AddDropDownData<PrimaryDriverForProposal>();
                    break;
                case ControllerAction.Edit:
                    allActions();
                    newEditActions();
                    break;
                case ControllerAction.New:
                    allActions();
                    newEditActions();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchUnionContractProposal search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchUnionContractProposal search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
                formatter.Pdf(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search);
                    return new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf", results);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateUnionContractProposal(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateUnionContractProposal model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditUnionContractProposal>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditUnionContractProposal model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public UnionContractProposalController(ControllerBaseWithPersistenceArguments<IRepository<UnionContractProposal>, UnionContractProposal, User> args) : base(args) {}
    }
}
