using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MMSINC.Metadata
{
    /// <summary>
    /// ModelMetadataProvider that allows for registering different providers for 
    /// specific types.
    /// </summary>
    public class AdvancedModelMetadataProvider : ModelMetadataProvider
    {
        #region Fields

        // Using a ConcurrentDictionary to stick with how ModelValidatorProviders and ModelBindingProviders
        // work with registration.
        private readonly ConcurrentDictionary<Type, ModelMetadataProvider> _registeredProviders =
            new ConcurrentDictionary<Type, ModelMetadataProvider>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ModelMetadataProvider that should be used by default when there is not a 
        /// custom provider registered.
        /// </summary>
        public virtual ModelMetadataProvider Default { get; set; }

        /// <summary>
        /// Gets the currently registered providers. Recommended that you use Add/GetProvider instead.
        /// </summary>
        protected virtual IDictionary<Type, ModelMetadataProvider> RegisteredProviders
        {
            get { return _registeredProviders; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the ModelMetadataProvider for a given type. If there isn't a registered
        /// provider for the type, the currently set default is used instead.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual ModelMetadataProvider GetProvider(Type type)
        {
            ModelMetadataProvider provider;
            if (!RegisteredProviders.TryGetValue(type, out provider))
            {
                provider = Default;
            }

            return provider;
        }

        /// <summary>
        /// Adds a provider to the registered providers collection for a given type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="provider"></param>
        public virtual void Add(Type type, ModelMetadataProvider provider)
        {
            type.ThrowIfNull("type");
            provider.ThrowIfNull("provider", "Use remove if you're trying to remove a provider.");
            if (provider == this)
            {
                throw new InvalidOperationException("What, you wanna stack overflow?");
            }

            RegisteredProviders.Add(type, provider);
        }

        /// <summary>
        /// Removes a provider from the registered providers collection for a given type.
        /// </summary>
        /// <param name="type"></param>
        public virtual void Remove(Type type)
        {
            RegisteredProviders.Remove(type);
        }

        #endregion

        #region ModelMetadataProvider implementation

        public override IEnumerable<ModelMetadata> GetMetadataForProperties(object container, Type containerType)
        {
            return GetProvider(containerType).GetMetadataForProperties(container, containerType);
        }

        public override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType,
            string propertyName)
        {
            return GetProvider(containerType).GetMetadataForProperty(modelAccessor, containerType, propertyName);
        }

        public override ModelMetadata GetMetadataForType(Func<object> modelAccessor, Type modelType)
        {
            return GetProvider(modelType).GetMetadataForType(modelAccessor, modelType);
        }

        #endregion
    }
}
