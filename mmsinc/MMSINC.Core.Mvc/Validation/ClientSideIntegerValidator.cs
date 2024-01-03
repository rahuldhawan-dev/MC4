using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MMSINC.Validation
{
    public class ClientSideIntegerValidatorProvider : AssociatedValidatorProvider
    {
        #region Fields

        private static readonly Type _intType = typeof(int);
        private static readonly Type _nullableIntType = typeof(int?);

        #endregion

        #region Methods

        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context,
            IEnumerable<Attribute> attributes)
        {
            if (metadata.ModelType != _intType && metadata.ModelType != _nullableIntType)
            {
                return Enumerable.Empty<ModelValidator>();
            }

            return new[] {new ClientSideIntegerValidator(metadata, context)};
        }

        #endregion
    }

    public class ClientSideIntegerValidator : ModelValidator
    {
        #region Constructors

        public ClientSideIntegerValidator(ModelMetadata metadata, ControllerContext context) :
            base(metadata, context) { }

        #endregion

        #region Methods

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            yield return new ModelClientValidationRule {
                ErrorMessage = Metadata.GetDisplayName() + " must be a whole number.",
                ValidationType = "integer"
            };
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            // This is client-side only, the MVC model binder already validates this correctly.
            return Enumerable.Empty<ModelValidationResult>();
        }

        #endregion
    }
}
