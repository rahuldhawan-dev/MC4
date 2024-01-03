using System;
using System.Reflection;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MMSINC.Data
{
    /// <summary>
    /// Describes how a ViewModel and Entity map to each other when 
    /// the Entity property references another entity and the ViewModel
    /// property is that entity's Id.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EntityMapAttribute : AutoMapAttribute
    {
        #region Properties

        /// <summary>
        /// Gets/sets the repository type registered with ObjectFactory that can be used
        /// to retrieve an existing entity that matches the ViewModel's property. If this
        /// property is null, the mapper will attempt to figure out the repository
        /// based on the entity property's type.
        /// </summary>
        public Type RepositoryType { get; set; }

        #endregion

        #region Constructors

        public EntityMapAttribute(MapDirections direction = MapDirections.BothWays) : this(null, null, direction) { }

        public EntityMapAttribute(string entityPropName, MapDirections direction = MapDirections.BothWays) : this(
            entityPropName, null, direction) { }

        public EntityMapAttribute(Type repositoryType, MapDirections direction = MapDirections.BothWays) : this(null,
            repositoryType, direction) { }

        public EntityMapAttribute(string entityPropName, Type repositoryType, MapDirections direction)
            : base(entityPropName, direction)
        {
            RepositoryType = repositoryType;
        }

        #endregion

        #region Private Methods

        public override ObjectPropertyDescriptor CreatePropertyDescriptor(IContainer container,
            PropertyInfo primaryPropertyInfo, Type secondaryType)
        {
            return new EntityMapPropertyDescriptor(container, primaryPropertyInfo, secondaryType, RepositoryType,
                SecondaryPropertyName, Direction);
        }

        //protected internal override IViewModelMapperProperty CreatePropertyMapper()
        //{
        //    // TODO: It would be better if there was an EntityViewModelPropertyMapper class
        //    //       that did just this. It's gonna need to happen sooner or later.
        //    var baseMapper = base.CreatePropertyMapper();

        //    var baseEntityGetter = baseMapper.EntityGetter;
        //    baseMapper.EntityGetter = (entity) =>
        //    {
        //        dynamic val = baseEntityGetter(entity);
        //        if (val != null)
        //        {
        //            return val.Id;
        //        }
        //        return null;
        //    };

        //    var baseViewModelGetter = baseMapper.ViewModelGetter;

        //    if (ViewModelPropertyInfo.PropertyType == typeof(int))
        //    {
        //        baseMapper.ViewModelGetter = (model) =>
        //        {
        //            dynamic id = baseViewModelGetter(model);
        //            dynamic repo = GetRepository();
        //            return repo.Find(id);
        //        };
        //    }
        //    else if (ViewModelPropertyInfo.PropertyType == typeof(int?))
        //    {
        //        baseMapper.ViewModelGetter = (model) =>
        //        {
        //            dynamic nullableId = baseViewModelGetter(model);
        //            if (nullableId == null)
        //            {
        //                return null;
        //            }
        //            int id = (int)nullableId;
        //            dynamic repo = GetRepository();
        //            return repo.Find(id);
        //        };
        //    }

        //    return baseMapper;
        //}

        #endregion
    }
}
