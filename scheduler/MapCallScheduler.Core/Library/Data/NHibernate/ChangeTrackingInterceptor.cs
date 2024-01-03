using System.Web.Mvc;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.ChangeTracking;
using MMSINC.Data.NHibernate;
using StructureMap;

namespace MapCallScheduler.Library.Data.NHibernate
{
    /// <inheritdoc />
    /// <remarks>
    /// This implementation uses the <see cref="IContainer"/> instance that was provided to its constructor
    /// to fetch an <see cref="IAuthenticationService{User}"/> instance for determining the current user,
    /// instead of using <see cref="DependencyResolver.Current"/>.  This seems to fix issues with a nested
    /// <see cref="IContainer"/> being disposed when records are saved and we attempt to set
    /// <see cref="IEntityWithCreationUserTracking{TUser}.CreatedBy"/>/
    /// <see cref="IEntityWithUpdateUserTracking{TUser}.UpdatedBy"/>.
    /// </remarks>
    public class ChangeTrackingInterceptor : ChangeTrackingInterceptor<User>
    {
        #region Properties

        protected override IAuthenticationService<User> AuthenticationService =>
            _container.GetInstance<IAuthenticationService<User>>();

        #endregion

        #region Constructors

        public ChangeTrackingInterceptor(IContainer container) : base(container) { }

        #endregion
    }
}
