using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Models.ViewModels.Users
{
    public class CreateUser : BaseUserViewModel
    {
        #region Properties

        [DoesNotAutoMap("Needs to be trimmed before mapping.")]
        [Required, StringLength(User.StringLengths.USERNAME)]
        public string UserName { get; set; }

        [DoesNotAutoMap("Property does not exist on User.")]
        public string UserNameTrimmed => UserName?.Trim();

        #endregion

        #region Constructor

        public CreateUser(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void SetDefaults()
        {
            base.SetDefaults();

            HasAccess = true;
            IsUserAdmin = false;
        }

        public override User MapToEntity(User entity)
        {
            base.MapToEntity(entity);

            // The business likes to copy/paste the username and it ends up adding unseen 
            // whitespace characters to the end of the username. Then they get confused why
            // the user can't login.
            entity.UserName = UserNameTrimmed;

            return entity;
        }

        private IEnumerable<ValidationResult> ValidateUserNameIsNotTaken()
        {
            // First check the User table
            var existingUser = _container.GetInstance<IRepository<User>>().TryGetUserByUserName(UserNameTrimmed);
            if (existingUser != null)
            {
                yield return new ValidationResult("This username is already taken.", new[] { nameof(UserName) });
            }

#if DEBUG
            // Can't use Membership stuff during functional tests. See comments related to this flag
            // in other parts of the codebase.
            if (MMSINC.MvcApplication.IsInTestMode && MMSINC.MvcApplication.RegressionTestFlags.Contains("do not create membership user"))
            {
                yield break;
            }
#endif
            // Then check the Membership provider.
            var membershipHelper = _container.GetInstance<IMembershipHelper>();
            if (membershipHelper.UserExists(UserNameTrimmed))
            {
                yield return new ValidationResult("This username is already taken by Membership Provider.", new[] { nameof(UserName) });
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateUserNameIsNotTaken());
        }

        #endregion
    }
}