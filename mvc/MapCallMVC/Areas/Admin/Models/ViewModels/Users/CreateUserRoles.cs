using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Admin.Models.ViewModels.Users
{
    public class CreateUserRoles : ViewModel<User>
    {
        #region Properties

        // IMPORTANT: All mapping must be done manually as we're creating multiple roles from these properties.
        // Also some properties(State) aren't part of the entity and are only needed by the view.

        [CheckBoxList, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        public int[] States { get; set; }

        [CheckBox, DoesNotAutoMap]
        public bool IsForAllOperatingCenters { get; set; }

        // This exists solely for use in the view to show/hide the IsForAllOperatingCenters field
        // for some users.
        [DoesNotAutoMap]
        public bool UserCanAdministrateAllOperatingCenters { get; set; }

        // This needs to be added at Controller.SetLookupData since it'll be based on user access.
        // This *does not cascade* off State like you'd expect. State only exists for the client-side to
        // autoselect operating centers.
        [RequiredWhen(nameof(IsForAllOperatingCenters), false)]
        [CheckBoxList, EntityMap(MapDirections.None), EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenters { get; set; }

        // This needs to be added at Controller.SetLookupData since it'll be based on user access.
        [Required]
        [CheckBoxList, EntityMap(MapDirections.None), EntityMustExist(typeof(Module))]
        public int[] Modules { get; set; }

        [Required]
        [CheckBoxList, EntityMap(MapDirections.None), EntityMustExist(typeof(RoleAction))]
        public int[] Actions { get; set; }

        #endregion

        #region Constructors

        public CreateUserRoles(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override User MapToEntity(User entity)
        {
            // NOTE: No call to base.MapToEntity as none of this maps to the User object directly.

            var newRoles = new List<Role>();
            var opcRepo = _container.GetInstance<IRepository<OperatingCenter>>();
            var moduleRepo = _container.GetInstance<IRepository<Module>>();
            var actionRepo = _container.GetInstance<IRepository<RoleAction>>();

            var operatingCenters = OperatingCenters == null ? new Dictionary<int, OperatingCenter>() : opcRepo.FindManyByIds(OperatingCenters);
            var modules = moduleRepo.FindManyByIds(Modules);
            var actions = actionRepo.FindManyByIds(Actions);

            Action<Module, RoleAction, OperatingCenter> tryCreateRole = (module, action, opc) => {
                if (!entity.Roles.Any(x => x.Module == module &&
                                           x.Action == action &&
                                           x.OperatingCenter == opc))
                {
                    var role = new Role();
                    role.User = entity;
                    role.Module = module;
                    role.Application = module.Application;
                    role.Action = action;
                    role.OperatingCenter = opc;
                    newRoles.Add(role);
                }
            };

            foreach (var module in modules)
            {
                foreach (var action in actions)
                {
                    if (IsForAllOperatingCenters == true)
                    {
                        tryCreateRole(module.Value, action.Value, null);
                    }
                    else
                    {
                        foreach (var opc in operatingCenters)
                        {
                            tryCreateRole(module.Value, action.Value, opc.Value);
                        }
                    }
                }
            }

            entity.Roles.AddRange(newRoles);

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var currentUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            var currentUserRoleAdminAccess = currentUser.GetUserAdministrativeRoleAccess();

            Func<int, int?, ValidationResult> canAdministrate = (moduleId, operatingCenterId) => {
                if (!currentUserRoleAdminAccess.CanAdministrate(moduleId, operatingCenterId))
                {
                    var opcName = !operatingCenterId.HasValue ? "ALL" : _container.GetInstance<IRepository<OperatingCenter>>().Find(operatingCenterId.Value).ToString();
                    return new ValidationResult(
                        $"You do not have the ability to administate the role module {(RoleModules)moduleId} in operating center {opcName}.",
                        new[] { nameof(Modules) });
                }

                return ValidationResult.Success;
            };

            if (Modules == null)
            {
                // Handled by Required.
                yield break;
            }

            foreach (var module in Modules)
            {
                if (IsForAllOperatingCenters == true)
                {
                    yield return canAdministrate(module, null);
                }
                else
                {
                    foreach (var opc in OperatingCenters)
                    {
                        yield return canAdministrate(module, opc);
                    }
                }
            }
        }

        #endregion
    }
}