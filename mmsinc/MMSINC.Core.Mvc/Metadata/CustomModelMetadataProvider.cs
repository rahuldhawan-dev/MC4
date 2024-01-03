using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.Data;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Metadata provider needed when using any attribute that derives from ICustomModelMetadataAttribute
    /// as part of a ViewModel. 
    /// 
    /// To use, in the application's Application_Start method, set ModelMetadataProviders.Current to
    /// an instance of this provider.
    /// </summary>
    public class CustomModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        #region Fields

        private static readonly Type _viewModelType = typeof(ViewModel<>);

        private readonly ConcurrentDictionary<Type, Type> _entityTypesByViewModelType =
            new ConcurrentDictionary<Type, Type>();

        #endregion

        #region Methods

        protected override ModelMetadata CreateMetadata(
            IEnumerable<Attribute> attributes,
            Type containerType,
            Func<object> modelAccessor,
            Type modelType,
            string propertyName)
        {
            var newAttributes = attributes.ToList();

            if (containerType != null)
            {
                var entityType = GetEntityTypeIfTypeIsViewModel(containerType);
                if (entityType != null)
                {
                    // Do not cache the type descriptor, the property descriptor, or the attributes. 
                    // The TypeDescriptorProvider MVC uses for this does its own caching for when
                    // things need to be cached. Also attributes just shouldn't be cached.
                    var propDesc = GetTypeDescriptor(entityType).GetProperties().Find(propertyName, true);

                    if (propDesc != null)
                    {
                        var baseAttributes = propDesc.Attributes;
                        AddViewAttributeRelatedAttributesIfThereAreAny(newAttributes, baseAttributes);
                        AddEntityAttributeIfItExists<MultilineAttribute>(newAttributes, baseAttributes);

                        // TODO: This should probably be removed to force use to use the ViewAttribute
                        //       but a lot of stuff needs to be replaced. 
                        AddEntityAttributeIfItExists<DisplayNameAttribute>(newAttributes, baseAttributes);
                        AddEntityAttributeIfItExists<DisplayFormatAttribute>(newAttributes, baseAttributes);
                        AddEntityAttributeIfItExists<DisplayAttribute>(newAttributes, baseAttributes);
                        AddEntityAttributeIfItExists<DescriptionAttribute>(newAttributes, baseAttributes);
                    }
                    else
                    {
                        // Not all properties on a ViewModel<T> have a matching property on the original T entity, we need
                        // to process the ViewAttributes of those too.
                        AddViewAttributeRelatedAttributesIfThereAreAny(newAttributes, AttributeCollection.Empty);
                    }
                }
                else
                {
                    AddViewAttributeRelatedAttributesIfThereAreAny(newAttributes, AttributeCollection.Empty);
                }
            }

            var modelMetadata =
                base.CreateMetadata(newAttributes, containerType, modelAccessor, modelType, propertyName);
            newAttributes.OfType<ICustomModelMetadataAttribute>().Each(x => x.Process(modelMetadata));

            // TODO: Come back and make this work as a proper provider. Nothing setup for processing
            // non-ICustomModelMetadataAttributes that we need to process but don't have control of.
            //
            // TODO #2: Add a PreProcess method that takes the attributes *before* calling base.CreateMetadata.
            //          Would work nicely with ViewAttribute.
            var descriptionAttr = newAttributes.OfType<DescriptionAttribute>().SingleOrDefault();
            if (descriptionAttr != null)
            {
                modelMetadata.AdditionalValues.Add("DescriptionAttribute", descriptionAttr);
            }

            return modelMetadata;
        }

        /// <summary>
        /// Returns an attribute of the TAttr type if the entity has that attribute and the view model
        /// does not have it.
        /// </summary>
        /// <typeparam name="TAttr"></typeparam>
        /// <param name="viewModelAttributes"></param>
        /// <param name="entityAttributes"></param>
        /// <returns></returns>
        private static TAttr GetViewModelAndEntityAttribute<TAttr>(List<Attribute> viewModelAttributes,
            AttributeCollection entityAttributes) where TAttr : Attribute
        {
            var viewModelAttribute = viewModelAttributes.OfType<TAttr>().SingleOrDefault();
            var entityAttribute = entityAttributes.OfType<TAttr>().SingleOrDefault();

            return viewModelAttribute ?? entityAttribute;
        }

        private static void AddEntityAttributeIfItExists<TAttr>(List<Attribute> viewModelAttributes,
            AttributeCollection entityAttributes) where TAttr : Attribute
        {
            // ONLY add the entity attribute if the view model does not have an attribute of the same type. 
            // View models are allowed to override whatever's set on the entity.

            var attrResult = GetViewModelAndEntityAttribute<TAttr>(viewModelAttributes, entityAttributes);

            if (attrResult != null && !viewModelAttributes.Contains(attrResult))
            {
                viewModelAttributes.Add(attrResult);
            }
        }

        private static void AddViewAttributeRelatedAttributesIfThereAreAny(List<Attribute> viewModelAttributes,
            AttributeCollection entityAttributes)
        {
            var attrResult = GetViewModelAndEntityAttribute<ViewAttribute>(viewModelAttributes, entityAttributes);

            if (attrResult != null) // && (!viewModelAttributes.Contains(attrResult) || doItRight))
            {
                // Allow for empty strings in case we need to empty out a display format.
                if (attrResult.DisplayFormat != null)
                {
                    viewModelAttributes.Add(new DisplayFormatAttribute {
                        DataFormatString = attrResult.DisplayFormat,
                        ApplyFormatInEditMode = attrResult.ApplyFormatInEditMode
                    });
                }

                if (attrResult.DisplayName != null)
                {
                    viewModelAttributes.Add(new DisplayAttribute {
                        Name = attrResult.DisplayName
                    });
                }

                if (attrResult.Description != null)
                {
                    viewModelAttributes.Add(new DescriptionAttribute(attrResult.Description));
                }
            }
        }

        private Type GetEntityTypeIfTypeIsViewModel(Type type)
        {
            // All the reflection we're doing can be a bit costly, so caching it is important.
            return _entityTypesByViewModelType.GetOrAdd(type, (key) => {
                if (!type.IsSubclassOfRawGeneric(_viewModelType))
                {
                    return null;
                }

                var baseType = GetViewModelBaseType(type);
                return
                    baseType.GetGenericArguments()
                            .Single(); // There should only ever be one generic parameter on ViewModel<T> models.
            });
        }

        private static Type GetViewModelBaseType(Type type)
        {
            if (type.IsGenericType)
            {
                return type;
            }

            return GetViewModelBaseType(type.BaseType);
        }

        #endregion
    }
}
