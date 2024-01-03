using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Controllers;

namespace Contractors.Controllers
{
    public class ContractorMeterCrewController : ControllerBaseWithValidation<IContractorMeterCrewRepository, ContractorMeterCrew>
    {
        #region Constructor

        public ContractorMeterCrewController(ControllerBaseWithPersistenceArguments<IContractorMeterCrewRepository, ContractorMeterCrew, ContractorUser> args) : base(args) { }

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchContractorMeterCrew());
        }

        [HttpGet]
        public ActionResult Index(SearchContractorMeterCrew search)
        {
            return ActionHelper.DoIndex(search);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet]
        public ActionResult New()
        {
            return ActionHelper.DoNew(_container.GetInstance<CreateContractorMeterCrew>());
        }

        [HttpPost]
        public ActionResult Create(CreateContractorMeterCrew model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditContractorMeterCrew>(id);
        }

        [HttpPost]
        public ActionResult Update(EditContractorMeterCrew model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

    }
}