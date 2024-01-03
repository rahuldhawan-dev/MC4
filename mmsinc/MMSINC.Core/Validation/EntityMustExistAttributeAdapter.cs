using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MMSINC.Utilities.StructureMap;
using StructureMap;

namespace MMSINC.Validation
{
    public class EntityMustExistAttributeAdapter : DataAnnotationsModelValidator<EntityMustExistAttribute>
    {
        #region Private Members

        private readonly IContainer _container;

        #endregion

        #region Constructors

        public EntityMustExistAttributeAdapter(ModelMetadata metadata, ControllerContext context,
            EntityMustExistAttribute attribute, IContainer container) : base(metadata, context, attribute)
        {
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            // Per the WCF RIA Services team, instance can never be null (if you have
            // no parent, you pass yourself for the "instance" parameter).
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
