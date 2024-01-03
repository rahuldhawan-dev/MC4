using System;
using System.Collections.Concurrent;
using System.Reflection;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using NHibernate;
using NHibernate.Type;
using StructureMap;
using StructureMap.Attributes;

namespace MMSINC.Utilities.StructureMap
{
    /// <summary>
    /// <see cref="IInterceptor"/> implementation which allows for setter-injection on entity properties.
    /// This is not ideal, but currently the codebase relies on being able to do so. 
    /// </summary>
    public class StructureMapInterceptor : EmptyInterceptor
    {
        #region Private Members

        protected IContainer _container;
        private static readonly ConcurrentDictionary<Type, bool> _typeDict = new ConcurrentDictionary<Type, bool>();

        #endregion

        #region Constructors

        public StructureMapInterceptor(IContainer container)
        {
            _container = container;
        }

        #endregion

        private bool HasInjectableProperties(object entity)
        {
            var type = entity.GetType();

            if (_typeDict.ContainsKey(type))
            {
                return _typeDict[type];
            }

            var properties = type.GetProperties();
            foreach (var prop in properties)
            {
                if (prop.GetCustomAttributes(typeof(SetterPropertyAttribute)).Any())
                {
                    return _typeDict[type] = true;
                }
            }

            return _typeDict[type] = false;
        }

        public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            if (HasInjectableProperties(entity))
            {
                _container.BuildUp(entity);
            }

            return false;
        }
    }
}
