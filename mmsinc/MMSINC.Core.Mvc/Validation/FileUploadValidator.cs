using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MMSINC.Validation
{
    public class FileUploadValidator : ModelValidator
    {
        #region Consts

        public const string INVALID_FILE_TYPE = "The uploaded file is not in an accepted format.",
                            BINARY_DATA_NOT_FOUND = "A file upload is required.";

        #endregion

        #region Fields

        private readonly FileUploadAttribute _attr;

        #endregion

        #region Constructor

        public FileUploadValidator(ModelMetadata metadata, ControllerContext controllerContext,
            FileUploadAttribute attr)
            : base(metadata, controllerContext)
        {
            _attr = attr;
        }

        #endregion

        #region Public Methods

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            // container returns the parent model, Metadata.Model returns the
            // actual property value to validate.
            var model = (AjaxFileUpload)Metadata.Model;

            if (model == null || model.BinaryData == null)
            {
                if (Metadata.IsRequired)
                {
                    yield return new ModelValidationResult {MemberName = string.Empty, Message = BINARY_DATA_NOT_FOUND};
                }
            }

            // Handling empty BinaryData separately because that's indicative of a bad upload.
            else if (!model.BinaryData.Any())
            {
                yield return new ModelValidationResult {MemberName = string.Empty, Message = BINARY_DATA_NOT_FOUND};
            }

            else if (_attr.AllowedFileTypes.Any())
            {
                var uploadedFileType = FileTypeAnalyzer.GetFileType(model.BinaryData);
                if (!_attr.AllowedFileTypes.Contains(uploadedFileType))
                {
                    yield return new ModelValidationResult {MemberName = string.Empty, Message = INVALID_FILE_TYPE};
                }
            }
        }

        #endregion
    }
}
