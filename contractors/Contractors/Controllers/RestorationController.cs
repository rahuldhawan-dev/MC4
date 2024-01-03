using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace Contractors.Controllers
{
    public class RestorationController : ControllerBaseWithValidation<Restoration>
    {
        #region Constants

        public const string NO_SUCH_RESTORATION = "No such restoration.";

        #endregion

        #region Actions

        #region Show/Index

        [HttpGet, NoCache]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchRestoration>();
        }

        [HttpGet]
        public ActionResult Index(SearchRestoration model)
        {
            return ActionHelper.DoIndex(model);
        }

        #endregion

        #region Create/New

        [HttpPost]
        public ActionResult Create(CreateRestoration model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet]
        public ActionResult New(int id)
        {
            var model = _viewModelFactory.Build<CreateRestoration>();
            var wo = GetWorkOrder(id);

            if (wo == null)
            {
                return DoHttpNotFound($"Work order for '#{id}' could not be found.");
            }

            model.WorkOrder = wo.Id;

            return ActionHelper.DoNew(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, NoCache]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditRestoration>(id, new ActionHelperDoEditArgs<Restoration, EditRestoration> {
                NotFound = NO_SUCH_RESTORATION,
                ViewName = "Edit"
            });
        }

        [HttpPost]
        public ActionResult Update(EditRestoration model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs
            {
                NotFound = NO_SUCH_RESTORATION
            });
        }

        #endregion

        #endregion

        public RestorationController(ControllerBaseWithPersistenceArguments<IRepository<Restoration>, Restoration, ContractorUser> args) : base(args) { }
    }
}