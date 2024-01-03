using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MMSINC.Metadata
{
    /// <summary>
    /// Base view model for use with ajax file uploads. This model handles all
    /// the proper binding and what not.
    /// </summary>
    [ModelBinder(typeof(AjaxFileUploadModelBinder))]
    public class AjaxFileUpload : IValidatableObject
    {
        #region Consts

        public const string INVALID_UPLOAD_KEY = "Invalid upload key.";

        #endregion

        #region Properties

        public virtual byte[] BinaryData { get; set; }
        public virtual string FileName { get; set; }
        public virtual Guid Key { get; set; }

        public bool HasBinaryData
        {
            get { return BinaryData != null && BinaryData.Length > 0; }
        }

        #endregion

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!HasBinaryData)
            {
                // This is for validating when an AjaxFileUpload key is returned(after a file's
                // been uploaded to the FileController). If there's no BinaryData but a token is
                // returned, then that's indicating that the ModelBinder couldn't find a matching
                // file for the token.
                if (Key != Guid.Empty)
                {
                    yield return new ValidationResult("Invalid upload key.", new[] {"Key"});
                }
                // This is for validating when an AjaxFileUpload is actually uploading a file.
                // This will never have a key, but should have binary data.
                else
                {
                    yield return new ValidationResult("An empty file was uploaded.", new[] {"BinaryData"});
                }
            }
        }
    }
}
