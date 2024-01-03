using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Admin.Models.ViewModels.Users
{
    public class RemoveUserRoles : ViewModel<User>
    {
        #region Properties

        [DoesNotAutoMap]
        [RequiredCollection, EntityMustExist(typeof(Role))]
        public int[] RolesToRemove { get; set; }

        #endregion

        #region Constructor

        public RemoveUserRoles(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override User MapToEntity(User entity)
        {
            // NOTE: There's nothing for base.MapToEntity to do here.
            // NOTE 2: We only want to check Roles, not AggregateRoles.
            var toRemove = entity.Roles.Where(x => RolesToRemove.Contains(x.Id)).ToList();
            foreach (var role in toRemove)
            {
                entity.Roles.Remove(role);
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var currentUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            var currentUserRoleAdminAccess = currentUser.GetUserAdministrativeRoleAccess();
            var userModified = _container.GetInstance<IRepository<User>>().Find(Id);

            if (RolesToRemove == null)
            {
                // Handled by RequiredCollection validation.
                yield break;
            }

            foreach (var roleId in RolesToRemove)
            {
                // NOTE: We're only checking against Roles for this, not AggregateRoles.
                var role = userModified.Roles.SingleOrDefault(x => x.Id == roleId);
                if (role == null)
                {
                    yield return new ValidationResult($"Role id {roleId} does not exist for user {userModified.UserName}.", new[] { nameof(RolesToRemove) });
                }
                else
                {
                    if (!currentUserRoleAdminAccess.CanAdministrate(role.Module.Id, role.OperatingCenter?.Id))
                    {
                        var opc = role.OperatingCenter != null ? role.OperatingCenter.ToString() : "ALL";
                        yield return new ValidationResult($"You do not have the ability to administate the role module {role.Module.Name} in operating center {opc}.", new[] { nameof(RolesToRemove) });
                    }
                }
            }
        }

        #endregion
    }
}