using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Metadata;

namespace Contractors.Controllers
{
    public class AuthenticationController : ControllerBaseWithValidation<IAuthenticationRepository<ContractorUser>, ContractorUser>
    {
        #region Consts

        public const string AJAX_NOT_LOGGED_IN_ERROR = "You must be logged in to continue.";

        #endregion

        #region Private Methods
        
        /// <summary>
        /// This exists solely to support some controller unit tests that are doing view model tests.
        /// Those tests should be setup as proper view model tests and then this can be removed.
        /// </summary>
        /// <param name="model"></param>
        internal void RunModelValidation(object model)
        {
            TryValidateModel(model);
        }

        #endregion

        #region LogOn

        // **************************************
        // URL: /Authentication/LogOn
        // **************************************
        [AllowAnonymous]
        public ActionResult LogOn()
        {
            // If we're an ajax request, we wanna return a simple 404.
            // Otherwise, dialogs/otherthings will get an entire login
            // page rendered inside them and screw everything up.
            if (Request.IsAjaxRequest())
            {
                return HttpNotFound(AJAX_NOT_LOGGED_IN_ERROR);
            }

            var model = _container.GetInstance<ContractorUserLogOn>();
            model.ReturnUrl = (string)TempData[ControllerExtensions.TempDataKeys.REDIRECT_URL];
            return View(model);
        }

        [HttpPost, AllowAnonymous, RequiresSecureForm(false)]
        public ActionResult LogOn(ContractorUserLogOn model)
        {
            if (ModelState.IsValid)
            {
                var entity = Repository.GetUser(model.Email);
                AuthenticationService.SignIn(entity.Id, false);

                if (model.LoginAttemptResult == UserLoginAttemptStatus.SuccessRequiresPasswordChange)
                {
                    // Include a warning message thing here. Note that this can not be a validation error because it would
                    // prevent the user from being able to login and change their password.
                    this.DisplayErrorMessage("Your password does not meet current password security requirements. You must update your password now.");
                    return RedirectToAction("ChangePasswordPost", "User", new { id = entity.Id });
                }
                if (Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        #endregion

        #region LogOff

        [HttpGet, AllowAnonymous, NoCache, RequiresSecureForm(false)]
        public ActionResult LogOff()
        {
            AuthenticationService.SignOut();
            return RedirectToAction("LogOn", "Authentication");
        }

        #endregion

        public AuthenticationController(ControllerBaseWithPersistenceArguments<IAuthenticationRepository<ContractorUser>, ContractorUser, ContractorUser> args) : base(args) { }
    }
}