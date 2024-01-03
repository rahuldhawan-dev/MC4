using MapCall.Common.Configuration;
using MapCall.Common.Helpers;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Views
{
    /// <summary>
    /// Abstract base class for all layouts/views/partial views/whathaveyous.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MvcViewBase<T> : MMSINC.Views.MvcViewBase<T>
    {
        #region Constants

        public const string ACTION_BAR_HELPER_VIEWDATA_KEY = "ActionBarHelper";

        #endregion

        #region Properties

        public ActionBarHelper ActionBarHelper
        {
            get
            {
                // TODO: Having this use this instance's HtmlHelper is kind of sloppy
                //       because there's no way of knowing which html helper will
                //       get used(ie from _Layout or a partial or somewhere in between).
                //       See about getting the top level ViewContext if possible.
                return GetSharedData<ActionBarHelper>(ACTION_BAR_HELPER_VIEWDATA_KEY,
                    () => _container.With(Html).GetInstance<ActionBarHelper<T>>());
            }
        }

        public ISecureFormTokenService SecureFormTokenService =>
            _container.GetInstance<ISecureFormTokenService>();

        public IAuthenticationService<User> AuthenticationService =>
            _container.GetInstance<IAuthenticationService<User>>();

        public IRoleService RoleService => _container.GetInstance<IRoleService>();

        public IDateTimeProvider DateTimeProvider => _container.GetInstance<IDateTimeProvider>();

        #endregion
    }
}
