using System.Collections.Generic;
using System.Web.Mvc;
using MMSINC.Validation;

namespace MMSINC.Metadata
{
    public class FileUploadValidatorProvider : ModelValidatorProvider
    {
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            if (!metadata.AdditionalValues.ContainsKey(FileUploadAttribute.METADATA_KEY))
            {
                yield break;
            }

            var attr = (FileUploadAttribute)metadata.AdditionalValues[FileUploadAttribute.METADATA_KEY];
            yield return new FileUploadValidator(metadata, context, attr);
        }
    }
}
