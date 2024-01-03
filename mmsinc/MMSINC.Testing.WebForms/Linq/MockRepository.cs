using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;

namespace MMSINC.Testing.Linq
{
    public class MockRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        #region Properties

        public virtual int CurrentIndex
        {
            get { throw new NotImplementedException(); }
        }

        public virtual object SelectedDataKey
        {
            get { throw new NotImplementedException(); }
        }

        public virtual TEntity CurrentEntity
        {
            get { return default(TEntity); }
        }

        public virtual IList<TEntity> Entities
        {
            get { throw new NotImplementedException(); }
        }

        public Expression<Func<TEntity, bool>> FilterExpression
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string SortExpression
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string SortDirection
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string SqlSortExpression
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        #region Events

        public event EventHandler<EntityEventArgs<TEntity>> CurrentEntityChanged,
                                                            EntityDeleted,
                                                            EntityInserted,
                                                            EntityUpdated;

        #endregion

        #region Exposed Methods

        public virtual void RestoreFromPersistedState(int selectedIndex)
        {
            if (CurrentEntityChanged != null)
            {
                CurrentEntityChanged(this, EntityEventArgs<TEntity>.Empty);
            }
        }

        public virtual void SetSelectedEntityIndex(int selectedObjectIndex)
        {
            throw new NotImplementedException();
        }

        public virtual void SetSelectedDataKey(object selectedDataKey)
        {
            throw new NotImplementedException();
        }

        public virtual void SetIndexFromDataKey(object selectedDataKey)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateCurrentEntity(TEntity entity)
        {
            var onValidate = typeof(TEntity).GetMethod("OnValidate",
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (onValidate != null)
            {
                onValidate.Invoke(entity, new object[] {
                    ChangeAction.Update
                });
            }
        }

        public virtual void UpdateCurrentEntityLiterally(TEntity entity)
        {
            var onValidate = typeof(TEntity).GetMethod("OnValidate",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (onValidate != null)
            {
                onValidate.Invoke(entity, new object[] {
                    ChangeAction.Update
                });
            }
        }

        public virtual void InsertNewEntity(TEntity entity)
        {
            var onValidate = typeof(TEntity).GetMethod("OnValidate",
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (onValidate != null)
            {
                onValidate.Invoke(entity, new object[] {
                    ChangeAction.Insert
                });
            }
        }

        public virtual void DeleteEntity(TEntity entity)
        {
            var onValidate = typeof(TEntity).GetMethod("OnValidate",
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (onValidate != null)
            {
                onValidate.Invoke(entity, new object[] {
                    ChangeAction.Delete
                });
            }
        }

        public virtual TEntity Get(object id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetFilteredSortedData(Expression<Func<TEntity, bool>> filterExpression,
            string sortExpression)
        {
            throw new NotImplementedException();
        }

        public int GetCountForExpression(Expression<Func<TEntity, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public void SetSelectedDataKeyForRPC(object selectedDataKey, Expression<Func<TEntity, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public virtual int Count()
        {
            throw new NotImplementedException();
        }

        public void SetSiteUserKey(IUser user)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
