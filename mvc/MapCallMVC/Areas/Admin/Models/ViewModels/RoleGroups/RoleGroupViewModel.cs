using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Admin.Models.ViewModels.RoleGroups
{
    public class RoleGroupViewModel : ViewModel<RoleGroup>
    {
        #region Fields

        private Collection<RoleGroupRole> _existingRoles;

        #endregion

        #region Properties

        [Required, StringLength(RoleGroup.StringLengths.NAME)]
        public string Name { get; set; }

        #region DELICATE PROPERTIES

        // HUGE NOTE: These properties are hardcoded in the Form.js for this page.
        // If you rename them here, then you have a lot of other renaming to do.

        /// <summary>
        /// Gets a cached copy of the existing roles for the role group.
        /// If this group is new, then the enumerable will be empty.
        /// </summary>
        [DoesNotAutoMap]
        public IEnumerable<RoleGroupRole> ExistingRoles
        {
            get
            {
                if (Id != 0)
                {
                    // This property's used by the view, so there's no reason *not* to pull them
                    // all in since they're going to be displayed. This is also used during mapping.
                    _existingRoles = new Collection<RoleGroupRole>(_container.GetInstance<IRepository<RoleGroup>>().Find(Id).Roles);
                    return _existingRoles;
                }

                return Enumerable.Empty<RoleGroupRole>();
            }
        }

        [DoesNotAutoMap]
        public List<CreateRoleGroupRole> NewRoles { get; set; }

        [EntityMustExist(typeof(RoleGroupRole))]
        [EntityMap(MapDirections.None)]
        public int[] RolesToRemove { get; set; }

        #endregion 

        #endregion

        #region Constructor

        public RoleGroupViewModel(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override RoleGroup MapToEntity(RoleGroup entity)
        {
            MapRolesToRemove(entity);
            MapRolesToAdd(entity);
            return base.MapToEntity(entity);
        }

        private void MapRolesToAdd(RoleGroup entity)
        {
            // Go through each NewRole and only add roles they don't already have. This should be the same
            // check seen in CreateNewRoles.
            if (NewRoles == null)
            {
                return;
            }

            var opcRepo = _container.GetInstance<IRepository<OperatingCenter>>();
            var moduleRepo = _container.GetInstance<IRepository<Module>>();
            var actionRepo = _container.GetInstance<IRepository<RoleAction>>();

            var operatingCenters = opcRepo.GetAll().ToDictionary(x => x.Id, x => x);
            var modules = moduleRepo.GetAll().ToDictionary(x => x.Id, x => x);
            var actions = actionRepo.GetAll().ToDictionary(x => x.Id, x => x);

            foreach (var newRole in NewRoles)
            {
                var roleGroupItem = new RoleGroupRole {
                    // Using Value checks is fine here since validation
                    // will catch any nulls.
                    // ReSharper disable once PossibleInvalidOperationException
                    Module = modules[newRole.Module.Value],
                    // ReSharper disable once PossibleInvalidOperationException
                    Action = actions[newRole.Action.Value],
                    OperatingCenter = newRole.OperatingCenter.HasValue ? operatingCenters[newRole.OperatingCenter.Value] : null,
                    RoleGroup = entity
                };

                entity.Roles.Add(roleGroupItem);
            }
        }

        private void MapRolesToRemove(RoleGroup entity)
        {
            if (RolesToRemove == null)
            {
                return;
            }

            foreach (var roleGroupRoleId in RolesToRemove)
            {
                var roleGroupRole = ExistingRoles.Single(x => x.Id == roleGroupRoleId);
                entity.Roles.Remove(roleGroupRole);
            }
        }

        private IEnumerable<ValidationResult> ValidateNameIsUnique()
        {
            var repo = _container.GetInstance<IRepository<RoleGroup>>();
            var match = repo.Where(x => x.Name == Name).SingleOrDefault();
            if (match != null && match.Id != Id)
            {
                yield return new ValidationResult("This group name is already in used.", new[] { nameof(Name) });
            }
        }

        private IEnumerable<ValidationResult> ValidateRolesToRemoveExistForGroup()
        {
            if (RolesToRemove != null)
            {
                var hasInvalidRoles = RolesToRemove.Except(ExistingRoles.Select(x => x.Id)).Any();
                if (hasInvalidRoles)
                {
                    yield return new ValidationResult("Unable to remove one or more roles because they do not belong to this role group.", new[] { nameof(RolesToRemove) });
                }
            }
        }

        private IEnumerable<ValidationResult> ValidateNewRolesAreUnique()
        {
            if (NewRoles == null)
            {
                yield break;
            }

            var distinctRoles = NewRoles.DistinctBy(x => new {
                x.Module,
                x.Action,
                x.OperatingCenter
            });
            if (distinctRoles.Count() != NewRoles.Count)
            {
                yield return new ValidationResult("Duplicate new roles were found.", new[] { nameof(NewRoles) });
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // NOTE: *Only* site admins are allowed to create/edit a RoleGroup so there's
            // no additional need to validate if they're able to access a specific operating center or module.

            return base.Validate(validationContext)
                       .Concat(ValidateNameIsUnique())
                       .Concat(ValidateNewRolesAreUnique())
                       .Concat(ValidateRolesToRemoveExistForGroup());
        }

        #endregion
    }
}