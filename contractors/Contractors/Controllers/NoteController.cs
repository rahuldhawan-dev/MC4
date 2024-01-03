using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Exceptions;

namespace Contractors.Controllers
{
    public class NoteController : ControllerBaseWithValidation<Note>
    {
        #region Constants

        public const string FRAGMENT_IDENTIFIER = "#NotesTab";

        #endregion

        #region Constructor

        public NoteController(ControllerBaseWithPersistenceArguments<IRepository<Note>, Note, ContractorUser> args) : base(args) { }

        #endregion

        #region New/Create

        [HttpGet]
        public ActionResult New(NewNote model)
        {
            // Note/Index.cshtml links to New with extra route data
            // which is why we have a model for the parameter of this action.
            return ActionHelper.DoNew(model);
        }

        [HttpPost]
        public ActionResult Create(NewNote model)
        {
            return ActionHelper.DoCreate(model,
                new MMSINC.Utilities.ActionHelperDoCreateArgs {
                    OnSuccess = () => RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER),
                    // I don't know why this is throwing an exception instead of returning a view with errors.
                    OnError = () => throw new ModelValidationException(ModelState)
                });
        }

        #endregion

        #region Edit/Update

        [HttpPost]
        public ActionResult Update(EditNote model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER),
                // I don't know why this is throwing an exception instead of returning a view with errors.
                OnError = () => throw new ModelValidationException(ModelState),
                OnNotFound = () => HttpNotFound()
            });
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete]
        public ActionResult Destroy(DeleteNote model)
        {
            // The DeleteNote model has additional validation which is why this isn't just an int id param.
            return ActionHelper.DoDestroy(model.Id, new MMSINC.Utilities.ActionHelperDoDestroyArgs {
                NotFound = $"Note with id '{model.Id} not found.",
                OnSuccess = () => RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER)
            });
        }

        #endregion

    }
}
