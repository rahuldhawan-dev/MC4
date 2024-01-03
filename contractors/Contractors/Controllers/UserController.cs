using System.Web.Mvc;
using Contractors.Data.DesignPatterns.Mvc;
using Contractors.Data.Models.Repositories;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Controllers;

namespace Contractors.Controllers
{
    // NOTE: All users can only access their own user information. Creation of new users must be done from MapCall.

    public class UserController : ControllerBaseWithValidation<IContractorUserRepository, ContractorUser>
    {
        #region Contants

        public struct Messages
        {
            public const string PASSWORD_SUCCESSFULLY_CHANGED = "Password has been changed successfully!",
                                PASSWORD_QA_SUCCESSFULLY_CHANGED =
                                    "Password question and answer have been changed successfully!";
        }

        #endregion
      
        #region ChangePassword

        [HttpGet]
        public ActionResult ChangePassword()
        {
            var user = AuthenticationService.CurrentUser;
            var model = _container.GetInstance<ChangePasswordContractorUser>();
            model.Map(user);
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePasswordPost(ChangePasswordContractorUser model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    DisplaySuccessMessage(Messages.PASSWORD_SUCCESSFULLY_CHANGED);
                    return RedirectToAction("Show"); // Allow for ActionHelper default success handling.
                },
                OnError = () => View("ChangePassword", model)
            });
        }

        #endregion

        #region ChangePasswordQA

        [HttpGet]
        public ActionResult ChangePasswordQA()
        {
            var user = AuthenticationService.CurrentUser;
            var model = _container
               .GetInstance<ChangePasswordQuestionAndAnswerContractorUser>();
            model.Map(user);
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePasswordQAPost(ChangePasswordQuestionAndAnswerContractorUser model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs
            {
                OnSuccess = () => {
                    DisplaySuccessMessage(Messages.PASSWORD_QA_SUCCESSFULLY_CHANGED);
                    return RedirectToAction("Show");
                },
                OnError = () => View("ChangePasswordQA", model)
            });
        }

        #endregion

        #region Show

        [HttpGet]
        public ActionResult Show()
        {
            return ActionHelper.DoShow(0, new MMSINC.Utilities.ActionHelperDoShowArgs {
                GetEntityOverride = () => AuthenticationService.CurrentUser 
            });
        }

        #endregion

        public UserController(ControllerBaseWithPersistenceArguments<IContractorUserRepository, ContractorUser, ContractorUser> args) : base(args) {}
    }
}
