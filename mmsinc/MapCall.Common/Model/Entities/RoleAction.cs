using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Controllers;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    // NOTE: This isn't setup to be bitwise, and having one action doesn't give you access to
    //       any other actions(ie Edit doesn't give you Read access).
    public enum RoleActions
    {
        /// <summary>
        /// Can do all things(read, edit, add, delete) and misc things only role admins should be allowed to do.
        /// </summary>
        UserAdministrator = 1,
        Read = 2,
        Edit = 3,
        Add = 4,
        Delete = 5
    }

    [Serializable]
    public class RoleAction : IEntityLookup
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual string Description => Name;
        public virtual string Name { get; set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Description;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    public static class ControllerActionExtensions
    {
        public static RoleActions ToRoleAction(this ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                case ControllerAction.Show:
                    return RoleActions.Read;
                case ControllerAction.New:
                    return RoleActions.Add;
                case ControllerAction.Edit:
                    return RoleActions.Edit;
                default:
                    throw new InvalidOperationException(String.Format("ControllerAction {0} is not supported.",
                        action));
            }
        }
    }
}
