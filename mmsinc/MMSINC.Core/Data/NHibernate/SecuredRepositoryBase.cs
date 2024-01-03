using System;
using MMSINC.Authentication;
using MMSINC.Exceptions;
using NHibernate;
using StructureMap;

namespace MMSINC.Data.NHibernate
{
    public class SecuredRepositoryBase<TEntity, TUser> : RepositoryBase<TEntity>
        where TEntity : class where TUser : IAdministratedUser
    {
        protected readonly IAuthenticationService<TUser> _authenticationSerice;

        #region Properties

        protected TUser CurrentUser
        {
            get { return _authenticationSerice.CurrentUser; }
        }

        #endregion

        #region Constructors

        public SecuredRepositoryBase(ISession session, IAuthenticationService<TUser> authenticationService,
            IContainer container) : base(session, container)
        {
            _authenticationSerice = authenticationService;
        }

        #endregion

        #region Private methods

        private static Exception EntityDoesNotExist(int id)
        {
            throw new DomainLogicException(
                string.Format("{0} does not exist with id {1}",
                    typeof(TEntity).Name, id));
        }

        #endregion

        #region Public methods

        public override TEntity Find(int id)
        {
            // Don't call base.
            // No reason to hit the database if id = 0, seeing as it can't exist.
            if (id <= 0)
            {
                return null;
            }

            // NOTE: This does not return from the session cache!
            var ret = Criteria.Add(GetIdEqCriterion(id)).UniqueResult<TEntity>();

            if (ret != null)
            {
                _container.BuildUp(ret);
            }

            return ret;
        }

        public override void Delete(TEntity entity)
        {
            var id = GetIdentifier(entity);
            var existing = Find(id);

            // We don't want to delete entities that the current user doesn't have access to.
            if (existing == null)
            {
                throw EntityDoesNotExist(id);
            }

            // We need to use the entity found from Find because Session.Delete
            // requires an entity that's already associated with the session.
            base.Delete(existing);
        }

        // TODO: Not that it's ever happened, but this wouldn't actually prevent a user
        // from saving something. Automatic change tracking would still flush these changes
        // *if* the session flushes for any reason.
        public override TEntity Save(TEntity entity)
        {
            // We don't want to allow entities to be saved if the user
            // wouldn't be able to query for them normally.
            var id = GetIdentifier(entity);

            // Assuming here that if the id > 0 then it's a pre-existing 
            // entity.
            if (id > 0 && Find(id) == null)
            {
                throw EntityDoesNotExist(id);
            }

            return base.Save(entity);
        }

        #endregion
    }
}
