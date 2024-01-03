using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;

namespace MapCallMVC.Controllers
{
    public class DataTableLayoutController : ControllerBaseWithPersistence<DataTableLayout, User>
    {
        #region Constructors

        public DataTableLayoutController(ControllerBaseWithPersistenceArguments<IRepository<DataTableLayout>, DataTableLayout, User> args) : base(args) { }

        #endregion

        // There is nothing to secure and this is posted via ajax without any sort of form tag.
        [HttpPost, RequiresSecureForm(false)]
        public ActionResult Create(CreateDataTableLayout model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => Json(new { success = true, id = model.Id }),
                OnError = () => Json(new
                {
                    success = false,
                    errors = ModelState.ToDictionaryOfErrors()
                })
            });
        }

        // There is nothing to secure and this is posted via ajax without any sort of form tag.
        [HttpPost, RequiresSecureForm(false)]
        public ActionResult Update(EditDataTableLayout model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs
            {
                OnSuccess = () => Json(new { success = true, id = model.Id }),
                OnError = () => Json(new
                {
                    success = false,
                    errors = ModelState.ToDictionaryOfErrors()
                })
            });
        }

        // There is nothing to secure and this is posted via ajax without any sort of form tag.
        [HttpDelete, RequiresSecureForm(false)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id, new MMSINC.Utilities.ActionHelperDoDestroyArgs {
                OnSuccess = () => Json(new { success = true, id = id }),
                OnError = () => Json(new {
                    success = false,
                    errors = ModelState.ToDictionaryOfErrors()
                })
            });
        }

    }
}