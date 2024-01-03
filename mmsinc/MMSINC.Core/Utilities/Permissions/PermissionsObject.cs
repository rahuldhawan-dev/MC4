using MMSINC.Interface;

namespace MMSINC.Utilities.Permissions
{
    public class PermissionsObject : IPermissionsObject
    {
        #region Fields

        #endregion

        #region Properties

        public IUser User { get; protected set; }
        public IModulePermissions SpecificPermissions { get; protected set; }
        public ModuleAction Action { get; protected set; }
        public IRoleManager RoleManager { get; }

        #endregion

        #region Constructors

        public PermissionsObject(IRoleManager roleManager, IUser user, IModulePermissions specificPermissions,
            ModuleAction action)
        {
            RoleManager = roleManager;
            User = user;
            SpecificPermissions = specificPermissions;
            Action = action;
        }

        #endregion

        #region Operators

        public static implicit operator bool(PermissionsObject perm)
        {
            return perm.InAny();
        }

        #endregion

        #region Exposed Methods

        public virtual bool In(string opCntr)
        {
            return RoleManager.UserIsInRoleWithOperatingCenter(this, opCntr);
        }

        public virtual bool InAny()
        {
            return RoleManager.UserIsInRole(this);
        }

        #endregion
    }

    // Sample Usage:
    //using MapCall.Permissions.Applications;

    //public class SomePageClassOrWhateverItDoesntMatterThisIsJustAnExampleGoshGetOffMyBack : MvpPage /* which should be renamed */
    //{
    //  protected void Page_PreInit(object sender, EventArgs e)
    //  {
    //    if (!IUser.CanRead(FieldServices.WorkManagement))
    //    {
    //      // special case in the error handler, maybe just redirects to
    //      // whatever valid thing they were doing before they tried to come
    //      // here and louse things up.  Or something.  Just don't let them
    //      // see the page and don't waste too many resources preventing them
    //      // from seeing the page.  Or crash the server and email their
    //      // supervisor like "Hey such and such keeps trying to do something
    //      // they shouldn't".
    //      throw new SecurityException(FieldServices.DataLookups);
    //    }

    //    // specific opCenter
    //    if (!User.CanRead(FieldServices.DataLookups).In(someOpCntrFromSomewhere))
    //    {
    //      // see above, i'm running low on silly juice
    //    }

    //    // some other specifics
    //    if (!User.CanRead(FieldServices.DataLookups).After(2PM))
    //    {
    //      // etc.
    //    }
    //  }
    //}
}
