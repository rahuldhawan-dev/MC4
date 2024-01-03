using System;
using System.Linq;
using System.Web.Mvc;
using AuthorizeNet;
using AuthorizeNet.Utility.NotProvided;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using StructureMap;

namespace MMSINC.Authentication
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UserMustHaveProfileAttribute : Attribute { }

    public class UserMustHaveProfileAuthorizer : MvcAuthorizer
    {
        #region Constants

        public const string MUST_HAVE_PROFILE =
            "You must add a payment profile to your account before placing an order.";

        #endregion

        #region Properties

        protected IAuthenticationService<IUserWithProfile> AuthenticationService =>
            _container.GetInstance<IAuthenticationService<IUserWithProfile>>();

        protected IRepository<IUserWithProfile> UserRepository =>
            _container.GetInstance<IRepository<IUserWithProfile>>();

        protected IDateTimeProvider DateTimeProvider => _container.GetInstance<IDateTimeProvider>();

        protected IExtendedCustomerGateway CustomerGateway => _container.GetInstance<IExtendedCustomerGateway>();

        #endregion

        #region Private Methods

        protected virtual bool ProfileValid(PaymentProfile profile)
        {
            return !String.IsNullOrEmpty(profile.CardNumber) &&
                   !String.IsNullOrEmpty(profile.CardExpiration);
            // note: the following fields aren't set
            //                !String.IsNullOrEmpty(profile.CardCode) &&
            //                !String.IsNullOrEmpty(profile.CardType) &&
            //                (DateTime.Parse(profile.CardExpiration) >= DateTimeProvider.GetCurrentDate());
        }

        protected virtual Customer GetCustomer(IUserWithProfile currentUser)
        {
            return
                CustomerGateway.GetCustomer(
                    currentUser.CustomerProfileId.ToString());
        }

        protected virtual IUserWithProfile GetCurrentUser()
        {
            return AuthenticationService.CurrentUser;
        }

        protected void UpdateCurrentUserVerification(IUserWithProfile currentUser)
        {
            currentUser.ProfileLastVerified = DateTimeProvider.GetCurrentDate();
            UserRepository.Save(currentUser);
        }

        protected bool AuthorizeCore()
        {
            var currentUser = GetCurrentUser();

            //if (IsRegularUser(currentUser) && !PaymentInfoWasVerifiedRecently(currentUser))
            if (!PaymentInfoWasVerifiedRecently(currentUser))
            {
                if (VerifyCurrentUserProfile(currentUser))
                {
                    UpdateCurrentUserVerification(currentUser);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        //TODO: Bug 2642
        //private bool IsRegularUser(User user)
        //{
        //    return !user.Roles.HasFlag(UserRoles.Administrator) &&
        //           !user.Roles.HasFlag(UserRoles.FormDesigner);
        //}

        private bool PaymentInfoWasVerifiedRecently(IUserWithProfile user)
        {
            var diff = (DateTimeProvider.GetCurrentDate() -
                        user.ProfileLastVerified);
            return diff <=
                   new TimeSpan(1, 0, 0, 0);
        }

        private bool VerifyCurrentUserProfile(IUserWithProfile currentUser)
        {
            if (currentUser.CustomerProfileId == null)
                return false;
            var customer = GetCustomer(currentUser);
            return customer.PaymentProfiles.Count >= 1 &&
                   customer.PaymentProfiles.Any(ProfileValid);
        }

        #endregion

        #region Exposed Methods

        public override void Authorize(AuthorizationArgs authArgs)
        {
            if (!GetAttributes<UserMustHaveProfileAttribute>(authArgs.Context).Any())
            {
                // Nothing to do here.
                return;
            }

            if (!AuthorizeCore())
            {
                //authArgs.Context.Result = new RedirectResult(MMSINC.ClassExtensions.ControllerExtensions.Urls.FORBIDDEN);
                var result = new ViewResult();
                result.ViewName = "~/Views/Error/Forbidden.cshtml";
                result.ViewData["Message"] = MUST_HAVE_PROFILE;
                authArgs.Context.Result = result;
            }
        }

        #endregion

        public UserMustHaveProfileAuthorizer(IContainer container) : base(container) { }
    }
}
