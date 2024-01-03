using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.TypeExtensions;

namespace MMSINC.Metadata
{
    /// <summary>
    /// ModelBinderProvider that allows us to not have to manually add an EnumModelBinder
    /// for every enum type we use.
    /// </summary>
    public class EnumModelBinderProvider : IModelBinderProvider
    {
        #region Fields

        // EnumModelBinders are thread safe as they don't have any instance fields/properties aside from Type,
        // so we can cache them. Also nulls are allowed here.
        private readonly ConcurrentDictionary<Type, EnumModelBinder> _bindersByType =
            new ConcurrentDictionary<Type, EnumModelBinder>();

        #endregion

        public IModelBinder GetBinder(Type modelType)
        {
            if (!_bindersByType.ContainsKey(modelType))
            {
                if (modelType.IsEnum)
                {
                    _bindersByType[modelType] = new EnumModelBinder(modelType);
                }
                else if (modelType.IsNullable())
                {
                    _bindersByType[modelType] = (EnumModelBinder)GetBinder(modelType.GetGenericArguments().First());
                }
                else
                {
                    _bindersByType[modelType] = null;
                }
            }

            return _bindersByType[modelType];
        }
    }
}
