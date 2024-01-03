using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Exceptions;

namespace MapCallMVC.Controllers
{
    public class NoteController : ControllerBaseWithPersistence<Note, User>
    {
        #region Constants

        public const string FRAGMENT_IDENTIFIER = "#NotesTab";

        #endregion

        #region New/Create

        [HttpGet]
        public ActionResult New(NewNote model = null)
        {
            return View(model ?? new NewNote(_container));
        }

        [HttpPost]
        public ActionResult Create(NewNote model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER),
                // TODO: Look into why we're throwing an exception here rather than returning something useful to the client.
                OnError = () => throw new ModelValidationException(ModelState)
            });
        }

        #endregion

        #region Edit/Update

        [HttpPost]
        public ActionResult Update(DeleteNote model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER),
                // TODO: Look into why we're throwing an exception here rather than returning something useful to the client.
                OnError = () => throw new ModelValidationException(ModelState)
            });
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete]
        public ActionResult Destroy(DeleteNote model)
        {
            var note = Repository.Find(model.Id);

            if (note == null)
            {
                return HttpNotFound(string.Format("Note with id '{0} not found.", model.Id));
            }

            Repository.Delete(note);

            return RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER);
        }

        #endregion

        public NoteController(ControllerBaseWithPersistenceArguments<IRepository<Note>, Note, User> args) : base(args) {}
    }
}
