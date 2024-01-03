using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Metadata;

namespace MMSINC.Validation
{
    /// <summary>
    /// Provides validators for properties on models that implement IDynamicModel
    /// </summary>
    /// <remarks>
    /// 
    /// We wanna inherit from just the normal ModelValidatorProvider class. If we inherit
    /// from, say, DataAnnotationsValidatorProvider, we run into duplicate validator
    /// errors.
    /// 
    /// </remarks>
    public class DynamicModelValidatorProvider : ModelValidatorProvider
    {
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            //  Debug.Print("SomeValidatorProvider.GetValidators Name: " + metadata.PropertyName + ", ModelType: " + metadata.ModelType.Name + ", ContainerType: " + (metadata.ContainerType != null ? metadata.ContainerType.Name : "Null"));

            // We're gonna be passed all the Metadata ever created, but
            // we can only work with LinkedModelMetadata since we need 
            // to know the container model.
            var meta = metadata as LinkedModelMetadata;
            if (meta == null)
            {
                return Enumerable.Empty<ModelValidator>();
            }

            // There could be a long chain of metadata using LinkedModelMetadata, but
            // might not necessarily be IDynamicModel. Return empty if it's not.
            var container = meta.ContainerModel as IDynamicModel;
            if (container == null)
            {
                return Enumerable.Empty<ModelValidator>();
            }

            var validationContext = new DynamicValidationContext {
                ControllerContext = context,
                ModelMetadata = metadata,
                Property = metadata.PropertyName
            };

            return GetDynamicValidators(container, validationContext);
        }

        protected virtual IEnumerable<ModelValidator> GetDynamicValidators(IDynamicModel model,
            DynamicValidationContext validationContext)
        {
            return model.GetValidators(validationContext);
        }
    }
}
