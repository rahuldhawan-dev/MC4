using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using MMSINC.Exceptions;

namespace MMSINC.Common
{
    public class EntityComparer<TEntity> : IComparer<TEntity>
    {
        #region Private Members

        private string _propertyName;
        private bool _sortAscending = true;

        #endregion

        #region Constructors

        public EntityComparer(string sortExpression)
        {
            if (sortExpression == null)
                throw new DomainLogicException("sortExpression set to null.");
            Initialize(sortExpression);
        }

        #endregion

        #region Private Methods

        private void Initialize(string propertyName)
        {
            if (propertyName.EndsWith(" DESC"))
            {
                _propertyName = propertyName.Remove(propertyName.Length - 5);
                _sortAscending = false;
            }
            else
            {
                _propertyName = propertyName;
                _sortAscending = true;
            }
        }

        private int CompareMethod(TEntity x, TEntity y)
        {
            // TODO: This is unclean, I just did it to satisfy a test
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;

            var a = GetObjectToCompare(x, _propertyName);
            var b = GetObjectToCompare(y, _propertyName);

            if (a == null && b == null)
                return 0;
            if (a == null)
                return -1;
            if (b == null)
                return 1;

            if (a is IComparable && b is IComparable)
                return Comparer.DefaultInvariant.Compare(a, b);

            return Comparer.DefaultInvariant.Compare(a.ToString(), b.ToString());
        }

        private static object GetObjectToCompare(object x, string propertyName)
        {
            PropertyInfo propertyInfo;
            if (propertyName.Contains("."))
            {
                var propname = propertyName.Substring(0, propertyName.IndexOf('.'));
                propertyInfo = x.GetType().GetProperty(propname);
                var a = propertyInfo.GetValue(x, null);
                return a == null ? null : GetObjectToCompare(a, propertyName.Substring(propertyName.IndexOf(".") + 1));
            }

            propertyInfo = x.GetType().GetProperty(propertyName);
            return (propertyInfo.GetValue(x, null));
        }

        #endregion

        #region Exposed Methods

        public int Compare(TEntity x, TEntity y)
        {
            if (_sortAscending)
                return CompareMethod(x, y);

            return -CompareMethod(x, y);
        }

        #endregion
    }
}
