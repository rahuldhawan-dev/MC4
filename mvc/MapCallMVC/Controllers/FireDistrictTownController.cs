using System;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Exceptions;

namespace MapCallMVC.Controllers
{
    public class FireDistrictTownController : ControllerBaseWithPersistence<FireDistrictTown, User>
    {
        #region Constants

        public const string FRAGMENT_IDENTIFIER = "#FireDistrictsTab";

        #endregion

        #region New/Create

        [HttpPost, RequiresRole(RoleModules.FieldServicesDataLookups)] // Why does this not use the Add RoleAction?
        public ActionResult Create(CreateFireDistrictTown model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER),
                // TODO: Look into why we're throwing an exception here rather than returning something useful to the client.
                OnError = () => throw new ModelValidationException(ModelState)
            });
        }

        #endregion

        #region Edit/Update

        [HttpPost, RequiresRole(RoleModules.FieldServicesDataLookups)] // Why does this not use the Add RoleAction?
        public ActionResult Update(MakeDefaultFireDistrictTown model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER),
                // TODO: Look into why we're throwing an exception here rather than returning something useful to the client.
                OnError = () => throw new ModelValidationException(ModelState)
            });
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(RoleModules.FieldServicesDataLookups)] // Why does this not use the Delete RoleAction?
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id, new MMSINC.Utilities.ActionHelperDoDestroyArgs {
                OnSuccess = () => RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER),
                OnError = () => RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER),
            });
        }

        #endregion

        public FireDistrictTownController(ControllerBaseWithPersistenceArguments<IRepository<FireDistrictTown>, FireDistrictTown, User> args) : base(args) {}
    }
}