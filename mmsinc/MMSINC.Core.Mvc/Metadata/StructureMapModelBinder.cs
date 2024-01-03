using System;
using System.Collections.Generic;
using System.Web.Mvc;
using StructureMap;

namespace MMSINC.Metadata
{
    public class StructureMapModelBinder : DefaultModelBinder
    {
        #region Properties

        /// <summary>
        /// Returns the constructor injected IContainer instance.
        /// </summary>
        protected IContainer Container { get; }

        #endregion

        #region Constructors

        public StructureMapModelBinder(IContainer container)
        {
            Container = container;
        }

        #endregion

        #region Private Methods

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext,
            Type modelType)
        {
            var type = modelType;

            if (modelType.IsGenericType)
            {
                var genericTypeDefinition = modelType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(IDictionary<,>))
                {
                    type = typeof(Dictionary<,>).MakeGenericType(modelType.GetGenericArguments());
                }
                else
                {
                    if (genericTypeDefinition != typeof(IEnumerable<>) &&
                        genericTypeDefinition != typeof(ICollection<>))
                    {
                        if (genericTypeDefinition == typeof(IList<>))
                        {
                            type = typeof(List<>).MakeGenericType(modelType.GetGenericArguments());
                        }
                    }
                }
            }

            return Container.GetInstance(type);
        }

        #endregion
    }
}
