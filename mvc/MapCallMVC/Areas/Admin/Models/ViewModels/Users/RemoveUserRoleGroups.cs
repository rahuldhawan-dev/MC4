using System;
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
    public class RemoveUserRoleGroups : ViewModel<User>
    {
        #region Properties

        [RequiredCollection, EntityMap(MapDirections.None), EntityMustExist(typeof(RoleGroup))]
        public int[] RoleGroupsToRemove { get; set; }

        #endregion

        #region Constructor

        public RemoveUserRoleGroups(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override User MapToEntity(User entity)
        {
            // NOTE: There's nothing for base.MapToEntity to do here.
            var toRemove = entity.RoleGroups.Where(x => RoleGroupsToRemove.Contains(x.Id)).ToList();
            foreach (var template in toRemove)
            {
                entity.RoleGroups.Remove(template);
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RoleGroupsToRemove == null)
            {
                // Handled by RequiredCollection validation.
                yield break;
            }

            var currentUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            var currentUserRoleAdminAccess = currentUser.GetUserAdministrativeRoleAccess();
            var userModified = _container.GetInstance<IRepository<User>>().Find(Id);

            foreach (var roleGroupId in RoleGroupsToRemove)
            {
                var roleGroup = userModified.RoleGroups.SingleOrDefault(x => x.Id == roleGroupId);
                if (roleGroup == null)
                {
                    yield return new ValidationResult($"Role group id {roleGroupId} does not exist for user {userModified.UserName}.");
                }
                else
                {
                    if (!currentUserRoleAdminAccess.CanAdministrate(roleGroup))
                    {
                        yield return new ValidationResult($"You do not have the ability to remove the \"{roleGroup.Name}\" role group as you do not have the user administrator access to all of the roles within the role group.");
                    }
                }
            }
        }

        #endregion
    }
}