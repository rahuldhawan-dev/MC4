using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MMSINC.Data
{
    public class DisplayItemService : IDisplayItemService
    {
        private static readonly ConcurrentDictionary<Type, Type> _types = new ConcurrentDictionary<Type, Type>();

        private static readonly ConcurrentDictionary<Assembly, IEnumerable<Type>> _allTypes =
            new ConcurrentDictionary<Assembly, IEnumerable<Type>>();

        #region Private Methods

        private static Func<Type, bool> ImplementsIDisplayItemForType(Type type)
        {
            return t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDisplayItem<>) &&
                i.GenericTypeArguments[0] == type);
        }

        private static IEnumerable<Type> LoadTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(t =>
                !t.IsAbstract && t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() ==
                    typeof(IDisplayItem<>)));
        }

        #endregion

        #region Exposed Methods

        public bool HasDisplayItem<TEntity>()
        {
            return HasDisplayItem(typeof(TEntity));
        }

        public bool HasDisplayItem(Type entityType)
        {
            return DisplayItemType(entityType) == null;
        }

        public Type DisplayItemType<TEntity>()
        {
            return DisplayItemType(typeof(TEntity));
        }

        public Type DisplayItemType(Type entityType)
        {
            if (_types.ContainsKey(entityType))
            {
                return _types[entityType];
            }

            var displayType = (_allTypes.ContainsKey(entityType.Assembly)
                    ? _allTypes[entityType.Assembly]
                    : (_allTypes[entityType.Assembly] = LoadTypes(entityType.Assembly)))
               .FirstOrDefault(ImplementsIDisplayItemForType(entityType));

            return _types[entityType] = displayType;
        }

        #endregion
    }

    public interface IDisplayItemService
    {
        #region Abstract Methods

        bool HasDisplayItem<TEntity>();
        bool HasDisplayItem(Type entityType);
        Type DisplayItemType<TEntity>();
        Type DisplayItemType(Type entityType);

        #endregion
    }
}
