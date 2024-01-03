using System;

namespace MMSINC.Common
{
    public class EntityEventArgs<TEntity> : EventArgs where TEntity : class
    {
        #region Private Members

        readonly TEntity _entity;

        #endregion

        #region Properties

        public TEntity Entity
        {
            get { return _entity; }
        }

        #endregion

        #region Static Properties

        new public static EntityEventArgs<TEntity> Empty
        {
            get
            {
                return new EntityEventArgs<TEntity>(
                    Activator.CreateInstance<TEntity>());
            }
        }

        #endregion

        #region Constructors

        public EntityEventArgs(TEntity entity)
        {
            if (entity == null) // TODO: Need better exception message.
                throw new ArgumentNullException("entity", "Cannot pass an uninitialized object into this class.");
            _entity = entity;
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Entity.ToString();
        }

        #endregion
    }
}
