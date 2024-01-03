using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Admin.Models.ViewModels.Users
{
    public class AddUserRoleGroup : ViewModel<User>
    {
        #region Properties

        [Required, DropDown]
        [EntityMap(MapDirections.None), EntityMustExist(typeof(RoleGroup))]
        public int? RoleGroup { get; set; }

        #endregion

        #region Constructors

        public AddUserRoleGroup(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override User MapToEntity(User entity)
        {
            // NOTE: No call to base.MapToEntity as this doesn't map to the User object directly.
            var roleTempRepo = _container.GetInstance<IRepository<RoleGroup>>();
            var roleGroupObj = roleTempRepo.Find(RoleGroup.Value);

            if (!entity.RoleGroups.Contains(roleGroupObj))
            {
                entity.RoleGroups.Add(roleGroupObj);
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!RoleGroup.HasValue)
            {
                // Already handled by Required validation.
                yield break;
            }

            var currentUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            var currentUserRoleAdminAccess = currentUser.GetUserAdministrativeRoleAccess();
            var roleGroup = _container.GetInstance<IRepository<RoleGroup>>().Find(RoleGroup.Value);
            
            // NOTE: There's no point in returning every single role the user can't administrate at this
            // time. The ability to remove role groups is hidden from the frontend if they can't administrate
            // the entire role group, so this is only here for redundancy. 

            if (!currentUserRoleAdminAccess.CanAdministrate(roleGroup))
            {
                yield return new ValidationResult("You do not have the ability to add this user role group as you do not have the user administrator access to all of the roles within the role group.");
            }
        }

        #endregion
    }
}