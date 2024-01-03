using System.Collections.Generic;
using System.Web.Mvc;

namespace MMSINC.Validation
{
    /// <summary>
    /// Interface for models that need to manually set some of their own values prior
    /// to model binding validation occurring.
    /// </summary>
    /// <remarks>
    /// 
    /// NOTE: This is different from IValidableObject. The OnPreValidating method is supposed
    /// to be called so that the DynamicModelBinder and DynamicValidatorProvider classes
    /// can set the proper validators on a model. 
    /// 
    /// </remarks>
    public interface IDynamicModel
    {
        /// <summary>
        /// Method called prior to a DynamicModelValidator creating validators for this instance.
        /// </summary>
        /// <remarks>
        /// 
        /// DynamicModelBinder will call this method prior to validation starting.
        /// This allows us to set values on a model before DynamicModelValidator
        /// creates its validators.
        /// 
        /// </remarks>
        void OnPreValidating();

        /// <summary>
        /// Method called by DynamicModelValidator when creating validators for a property. 
        /// </summary>
        /// <returns>
        /// 
        /// Note that this gets called for *every* property on a model, so we need to check
        /// during the call that it's the right property we're getting validators for.
        /// 
        /// </returns>
        IEnumerable<ModelValidator> GetValidators(DynamicValidationContext validationContext);
    }

    /// <summary>
    /// Typical values needed by an IDynamicModel to create custom validators.
    /// </summary>
    public class DynamicValidationContext
    {
        #region Properties

        public ControllerContext ControllerContext { get; set; }
        public ModelMetadata ModelMetadata { get; set; }
        public string Property { get; set; }

        #endregion
    }
}
