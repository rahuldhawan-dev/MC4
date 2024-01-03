using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;

namespace MMSINC.Data.Linq
{
    internal class EntityMerger<TEntity> : EntityMerger
        where TEntity : class
    {
        #region Constants

        private struct NecessaryTypes
        {
            public static readonly Type Entity = typeof(TEntity),
                                        ColumnAttribute =
                                            typeof(ColumnAttribute),
                                        AssociationAttribute =
                                            typeof(AssociationAttribute),
                                        DateTime = typeof(DateTime),
                                        Int = typeof(int);
        }

        #endregion

        #region Private Members

        private readonly TEntity _source, _destination;
        private readonly bool _mergeNulls;
        private PropertyInfo[] _columns, _associations;

        #endregion

        #region Properties

        protected PropertyInfo[] Columns
        {
            get
            {
                if (_columns == null)
                    _columns = (from p in NecessaryTypes.Entity.GetProperties()
                                where p.GetIndexParameters().Length == 0 &&
                                      p.GetCustomAttributes(NecessaryTypes.ColumnAttribute, false).FirstOrDefault() !=
                                      null
                                select p).ToArray();
                return _columns;
            }
        }

        protected PropertyInfo[] Associations
        {
            get
            {
                if (_associations == null)
                    _associations =
                        (from p in NecessaryTypes.Entity.GetProperties()
                         where p.GetIndexParameters().Length == 0 && IsForeignKey(p)
                         select p).ToArray();
                return _associations;
            }
        }

        #endregion

        #region Constructors

        internal EntityMerger(TEntity destination, TEntity source)
        {
            _destination = destination;
            _source = source;
        }

        internal EntityMerger(TEntity destination, TEntity source, bool mergeNulls)
        {
            _destination = destination;
            _source = source;
            _mergeNulls = mergeNulls;
        }

        #endregion

        #region Private Methods

        private bool IsForeignKey(PropertyInfo propertyInfo)
        {
            var customAttributes =
                propertyInfo.GetCustomAttributes(typeof(AssociationAttribute),
                    false);

            return customAttributes.Length != 0 && ((AssociationAttribute)customAttributes[0]).IsForeignKey;
        }

        internal TEntity DoMerge()
        {
            foreach (var prop in Columns)
            {
                var oldValue = prop.GetValue(_destination, null);
                var newValue = prop.GetValue(_source, null);

                // if both values are null, goto the next one.
                // or
                // if new one is null and old one isn't and we aren't merging nulls, goto the next one
                if ((oldValue == null && newValue == null) || (oldValue != null && newValue == null && !_mergeNulls))
                    continue;

                var valueType = prop.PropertyType;
                if (valueType == NecessaryTypes.DateTime)
                {
                    if (((DateTime)newValue) == DateTime.MinValue &&
                        ((DateTime)oldValue) != DateTime.MinValue)
                        continue;
                }
                else if (valueType == NecessaryTypes.Int)
                {
                    if (((int)newValue) == default(int) &&
                        ((int)oldValue) != default(int))
                        continue;
                }

                prop.SetValue(_destination, newValue, null);
            }

            return _destination;
        }

        #endregion
    }

    /// <summary>
    /// Provides the functionality required to merge the values of two business
    /// entities of the same type together.
    /// </summary>
    public class EntityMerger
    {
        #region Exposed Static Methods

        /// <summary>
        /// Merges the values set on the properties of the _source entity
        /// to those of the destination entity, taking care to keep from
        /// overwriting previously set values with nulls.
        /// </summary>
        /// <typeparam name="TEntity">Type of entities to be merged.</typeparam>
        /// <param name="destination">Entity into which all values should be merged.</param>
        /// <param name="source">Entity from which non-null values should be gathered.</param>
        public static void Merge<TEntity>(ref TEntity destination, TEntity source)
            where TEntity : class
        {
            destination = (new EntityMerger<TEntity>(destination, source).DoMerge());
        }

        /// <summary>
        /// Merges the values set on the properties of the _source entity
        /// to those of the destination entity.
        /// </summary>
        /// <typeparam name="TEntity">Type of entities to be merged.</typeparam>
        /// <param name="destination">Entity into which all values should be merged.</param>
        /// <param name="source">Entity from which all values should be gathered.</param>
        /// <param name="mergeNulls">Whether or not to overwrite destination values with nulls 
        /// from the source. This will include associations.</param>
        public static void Merge<TEntity>(ref TEntity destination, TEntity source, bool mergeNulls)
            where TEntity : class
        {
            destination = (new EntityMerger<TEntity>(destination, source, mergeNulls).DoMerge());
        }

        #endregion
    }
}
