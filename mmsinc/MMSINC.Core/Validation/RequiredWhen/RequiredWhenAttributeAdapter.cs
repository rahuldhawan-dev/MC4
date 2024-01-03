using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MMSINC.Utilities.StructureMap;
using StructureMap;

namespace MMSINC.Validation
{
    public class RequiredWhenAttributeAdapter : DataAnnotationsModelValidator<RequiredWhenAttribute>
    {
        #region Private Members

        private readonly IContainer _container;

        #endregion

        #region Constructors

        public RequiredWhenAttributeAdapter(ModelMetadata metadata, ControllerContext context,
            RequiredWhenAttribute attribute, IContainer container) : base(metadata, context, attribute)
        {
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var context = new ValidationContext(container ?? Metadata.Model,
                new StructureMapServiceProvider(_container), null) {
                DisplayName = Metadata.GetDisplayName()
            };

            ValidationResult result = Attribute.GetValidationResult(Metadata.Model, context);
            if (result != ValidationResult.Success)
            {
                yield return new ModelValidationResult {
                    Message = result.ErrorMessage
                };
            }
        }

        #endregion
    }
}
