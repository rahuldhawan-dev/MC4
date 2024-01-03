using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.Utilities;
using StructureMap;

namespace MMSINC.Data
{
    public class EntityMapValueConverter : DefaultValueConverter
    {
        #region Fields

        private static readonly Type _baseIRepositoryType = typeof(IBaseRepository),
                                     _intType = typeof(int),
                                     _nullableIntType = typeof(int?),
                                     _intArrayType = typeof(int[]);

        private readonly Type _repositoryType,
                              _entityPropertyType,
                              _viewModelPropertyType;

        private readonly Func<Type> _repositoryTypeAccessor;
        private readonly IContainer _container;

        #endregion

        #region Constructor

        public EntityMapValueConverter(IContainer container, Type repositoryType, Type entityPropertyType,
            Type viewModelPropertyType)
            : this(container, () => repositoryType, entityPropertyType, viewModelPropertyType) { }

        public EntityMapValueConverter(IContainer container, Func<Type> repositoryTypeAccessor, Type entityPropertyType,
            Type viewModelPropertyType)
        {
            _container = container;
            repositoryTypeAccessor.ThrowIfNull("repositoryType");
            entityPropertyType.ThrowIfNull("entityPropertyType");
            viewModelPropertyType.ThrowIfNull("viewModelPropertyType");

            _repositoryTypeAccessor = repositoryTypeAccessor;
            _entityPropertyType = entityPropertyType;
            _viewModelPropertyType = viewModelPropertyType;
            // TODO: Throw exception if the entity type and the repository's generic type do not match.
        }

        #endregion

        #region Private Methods

        private bool TypeIsSomeKindOfInteger(Type type)
        {
            return (type == _intType || type == _nullableIntType);
        }

        private bool TypeIsIntegerArray(Type type)
        {
            return (type == _intArrayType);
        }

        private bool TypesAreConvertable(Type typeOne, Type typeTwo)
        {
            return typeOne == _entityPropertyType &&
                   (TypeIsSomeKindOfInteger(typeTwo) || TypeIsIntegerArray(typeTwo));
        }

        private IBaseRepository GetRepository()
        {
            // For the sake of unit testing, this method needs to be called
            // as late as possible. 
            var repoType = _repositoryTypeAccessor();
            repoType.ThrowIfNull("repositoryType");

            if (!_baseIRepositoryType.IsAssignableFrom(repoType))
            {
                throw EntityMapperException.RepositoryMustImplementIRepository(_repositoryType.FullName);
            }

            return (IBaseRepository)_container.GetInstance(repoType);
        }

        private object MapToList(int[] values)
        {
            var ret = (IList)Activator.CreateInstance(
                typeof(List<>).MakeGenericType(_entityPropertyType.GetProperty("Item").PropertyType));

            if (values != null)
            {
                dynamic repo = GetRepository();

                foreach (var value in values)
                {
                    ret.Add(repo.Load(value));
                }
            }

            return ret;
        }

        private object MapToSet(int[] values)
        {
            var ret = (dynamic)Activator.CreateInstance(
                typeof(HashSet<>).MakeGenericType(_entityPropertyType.GetGenericArguments()));

            if (values != null)
            {
                dynamic repo = GetRepository();

                foreach (var value in values)
                {
                    ret.Add(repo.Load(value));
                }
            }

            return ret;
        }

        private object MapToIntArray(object values)
        {
            var list = new List<int>();
            var typed = values as IEnumerable;

            foreach (dynamic entity in typed)
            {
                if (entity != null)
                {
                    list.Add(entity.Id);
                }
            }

            return list.ToArray();
        }

        #endregion

        #region Public Methods

        public override bool CanConvert(Type typeToConvert, Type typeToConvertTo)
        {
            // Need to check that it can be converted in either direction.
            return TypesAreConvertable(typeToConvert, typeToConvertTo) ||
                   TypesAreConvertable(typeToConvertTo, typeToConvert);
        }

        public override object ToPrimary(object secondaryValue)
        {
            if (secondaryValue != null)
            {
                return EntityMapHelpers.IsPropertyASupportedGenericCollectionType(_entityPropertyType)
                    ? MapToIntArray(secondaryValue)
                    : secondaryValue.AsDynamic().Id;
            }

            return null;
        }

        public override object ToSecondary(object primaryValue)
        {
            int idVal = 0;
            if (_viewModelPropertyType == _nullableIntType)
            {
                // There's no point in calling the repository method if we've been
                // given a null value, so we can just return null early.
                var casted = (int?)primaryValue;
                if (!casted.HasValue)
                {
                    return null;
                }

                idVal = casted.GetValueOrDefault();
            }
            else if (_viewModelPropertyType == _intType)
            {
                idVal = (int)primaryValue;
            }
            else if (_viewModelPropertyType == _intArrayType)
            {
                return _entityPropertyType.IsSubclassOfRawGeneric(typeof(ISet<>))
                    ? MapToSet((int[])primaryValue)
                    : MapToList((int[])primaryValue);
            }

            // Also hackish because I don't wanna add the Find method
            // to IRepository.
            dynamic repo = GetRepository();
            return idVal == 0 ? null : repo.Load(idVal);
        }

        #endregion
    }
}
