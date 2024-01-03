using System;
using System.Web.Mvc;
using MMSINC.Authentication;
using MMSINC.Data.ChangeTracking;
using MMSINC.Utilities;
using MMSINC.Utilities.StructureMap;
using NHibernate.Type;
using StructureMap;

namespace MMSINC.Data.NHibernate
{
    /// <inheritdoc cref="IChangeTrackingInterceptor{TUser}"/>
    public class ChangeTrackingInterceptor<TUser>
        : StructureMapInterceptor, IChangeTrackingInterceptor<TUser>
        where TUser : class, IAdministratedUser
    {
        #region Properties

        protected IDateTimeProvider DateTimeProvider => _container.GetInstance<IDateTimeProvider>();

        protected virtual IAuthenticationService<TUser> AuthenticationService =>
            DependencyResolver.Current.GetService<IAuthenticationService<TUser>>();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for the <see cref="ChangeTrackingInterceptor{TUser}"/> class.
        /// </summary>
        public ChangeTrackingInterceptor(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        /// <summary>
        /// We can't just set the values on the entity, because that won't actually persist them.
        /// https://ayende.com/blog/3987/nhibernate-ipreupdateeventlistener-ipreinserteventlistener
        /// </summary>
        private static void Set(string[] propertyNames, object[] state, string propertyName, object value)
        {
            var index = Array.IndexOf(propertyNames, propertyName);
            if (index == -1)
            {
                return;
            }

            state[index] = value;
        }

        private TValue Reset<TValue>(
            string[] propertyNames,
            object[] state,
            object[] oldState,
            string propertyName)
        {
            var index = Array.IndexOf(propertyNames, propertyName);
            if (index == -1 || oldState == null)
            {
                return Activator.CreateInstance<TValue>();
            }

            return (TValue)(state[index] = oldState[index]);
        }

        private bool MaybeSetCreationFields(
            IEntity entity,
            object[] state,
            string[] propertyNames,
            DateTime now)
        {
            var changed = false;

            if (entity is IEntityWithCreationTimeTracking timeTracking &&
                timeTracking.CreatedAt == default)
            {
                timeTracking.CreatedAt = now;
                Set(propertyNames, state, nameof(timeTracking.CreatedAt), now);
                changed = true;
            }

            if (entity is IEntityWithCreationUserTracking<TUser> whomTracking &&
                whomTracking.CreatedBy == default)
            {
                if (!AuthenticationService.CurrentUserIsAuthenticated)
                {
                    throw new InvalidOperationException(
                        "No user is currently authenticated to set change tracking field for new " +
                        entity.GetType().FullName);
                }
                
                var whom = AuthenticationService.CurrentUser;
                whomTracking.CreatedBy = whom;
                Set(propertyNames, state, nameof(whomTracking.CreatedBy), whom);
                changed = true;
            }

            return changed;
        }

        private bool MaybeSetUpdateFields(
            IEntity entity,
            object[] state,
            string[] propertyNames,
            DateTime now)
        {
            var changed = false;

            if (entity is IEntityWithUpdateTimeTracking timeTracking)
            {
                timeTracking.UpdatedAt = now;
                Set(propertyNames, state, nameof(timeTracking.UpdatedAt), now);
                changed = true;
            }

            if (entity is IEntityWithUpdateUserTracking<TUser> whomTracking)
            {
                if (!AuthenticationService.CurrentUserIsAuthenticated)
                {
                    throw new InvalidOperationException(
                        "No user is currently authenticated to set change tracking field for " +
                        $"{entity.GetType().FullName} {entity.Id}.  Type of authentication service is " +
                        AuthenticationService.GetType().FullName);
                }
                var whom = AuthenticationService.CurrentUser;
                whomTracking.UpdatedBy = whom;
                Set(propertyNames, state, nameof(whomTracking.UpdatedBy), whom);
                changed = true;
            }

            return changed;
        }

        private void MaybeResetCreationFields(
            IEntity entity,
            object[] currentState,
            object[] previousState,
            string[] propertyNames)
        {
            if (entity is IEntityWithCreationTimeTracking timeTracking)
            {
                timeTracking.CreatedAt = Reset<DateTime>(
                    propertyNames, currentState, previousState, nameof(timeTracking.CreatedAt));
            }

            if (entity is IEntityWithCreationUserTracking<TUser> whomTracking)
            {
                whomTracking.CreatedBy = Reset<TUser>(
                    propertyNames, currentState, previousState, nameof(whomTracking.CreatedBy));
            }
        }

        #endregion

        #region Exposed Methods

        /// <inheritdoc />
        public override bool OnSave(
            object entity,
            object id,
            object[] state,
            string[] propertyNames,
            IType[] types)
        {
            if (!(entity is IEntity typedEntity))
            {
                return base.OnSave(entity, id, state, propertyNames, types);
            }

            var now = DateTimeProvider.GetCurrentDate();
            var creationTracked = false;
            var updationTracked = false;

            creationTracked = MaybeSetCreationFields(typedEntity, state, propertyNames, now);
            updationTracked = MaybeSetUpdateFields(typedEntity, state, propertyNames, now);

            return creationTracked || updationTracked;
        }
        
        /// <inheritdoc />
        public override bool OnFlushDirty(
            object entity,
            object id,
            object[] currentState,
            object[] previousState,
            string[] propertyNames,
            IType[] types)
        {
            if (!(entity is IEntity typedEntity))
            {
                return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
            }

            var now = DateTimeProvider.GetCurrentDate();
            var changed = false;

            changed = MaybeSetUpdateFields(typedEntity, currentState, propertyNames, now);
            MaybeResetCreationFields(typedEntity, currentState, previousState, propertyNames);

            return changed;
        }

        #endregion
    }
}
