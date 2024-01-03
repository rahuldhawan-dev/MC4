using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Utilities;

namespace MMSINC.Metadata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FileUploadAttribute : Attribute, ICustomModelMetadataAttribute
    {
        #region Consts

        public const string METADATA_KEY = "FileUploadAttribute";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the file types that are allowed to be uploaded. Leave empty if
        /// any file type can be uploaded.
        /// </summary>
        public HashSet<FileTypes> AllowedFileTypes { get; private set; }

        /// <summary>
        /// Gets/sets the action the file uploader should upload to
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets/sets the controller the action property is on.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Gets/sets the area, if any, that the controller is on. Leave null
        /// if the area for the current request should be used, set to String.Empty
        /// if the controller is not in an area.
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Gets/sets the text for the upload button. Defaults to "Upload".
        /// </summary>
        public string ButtonText { get; set; }

        /// <summary>
        /// Javascript method called when the file upload is complete.
        /// </summary>
        public string OnComplete { get; set; }

        #endregion

        #region Constructor

        public FileUploadAttribute(params FileTypes[] allowedFileTypes)
        {
            if (allowedFileTypes.Contains(FileTypes.Unknown))
            {
                throw new InvalidOperationException(
                    "Don't add FileTypes.Unknown. If you want to allow any files, don't set any FileTypes.");
            }

            AllowedFileTypes = new HashSet<FileTypes>(allowedFileTypes);

            // Defaults for Action/Controller since we're using FileController with this.
            Action = "Create";
            Controller = "File";
            Area = "";
        }

        #endregion

        #region Public Methods

        public void Process(ModelMetadata modelMetaData)
        {
            modelMetaData.AdditionalValues.Add(METADATA_KEY, this);
            // Don't know if we need the template hint or not
            modelMetaData.TemplateHint = "AjaxFileUpload";
        }

        /// <summary>
        /// Returns the FileUploadAttribute for the given ModelMetadata. An exception is thrown if no attribute is found.
        ///  </summary>
        public static FileUploadAttribute GetAttributeForModel(ModelMetadata meta)
        {
            meta.ThrowIfNull("metadata");

            if (meta.AdditionalValues.ContainsKey(METADATA_KEY))
            {
                return (FileUploadAttribute)meta.AdditionalValues[METADATA_KEY];
            }

            if (meta.ModelType.IsAssignableFrom(typeof(AjaxFileUpload)))
            {
                var attr = new FileUploadAttribute();
                attr.Process(meta);
                return attr;
            }

            throw new InvalidOperationException("Unable to find FileUploadAttribute for metadata");
        }

        #endregion
    }
}
