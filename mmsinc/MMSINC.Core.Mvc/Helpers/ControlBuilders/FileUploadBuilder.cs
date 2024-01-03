using MMSINC.ClassExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMSINC.Helpers
{
    /// <summary>
    /// Builds a control for use with the ajax fileupload script.
    /// </summary>
    public class FileUploadBuilder : ControlBuilder<FileUploadBuilder>
    {
        #region Consts

        private const string DATA_KEY_FORMAT = "data-file-{0}";

        #endregion

        #region Properties

        /// <summary>
        /// The file extensions that are allowed for uploads. Leave empty if any file type is allowed.
        /// </summary>
        public HashSet<string> AllowedExtensions { get; private set; }

        /// <summary>
        /// Gets/sets the text for the upload button. 
        /// </summary>
        public string ButtonText { get; set; }

        /// <summary>
        /// Gets/sets the callback function to call when the file
        /// has been uploaded. Not required. The method takes one argument which
        /// is the result of the upload.
        /// </summary>
        public string OnComplete { get; set; }

        /// <summary>
        /// Gets/sets the url the ajax file uploader uploads to.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets/sets the max upload size (most likely read from web.config).
        /// </summary>
        public int? MaxSize { get; set; }

        #endregion

        #region Constructor

        public FileUploadBuilder()
        {
            AllowedExtensions = new HashSet<string>();
        }

        #endregion

        #region Private Methods

        private string GetAllowedExtensions()
        {
            var exts = new HashSet<string>(AllowedExtensions.Select(x => x.ToLowerInvariant()));

            // TODO HACK: Fix this.
            if (exts.Contains("jpeg"))
            {
                exts.Add("jpg");
            }

            if (exts.Contains("tiff"))
            {
                exts.Add("tif");
            }

            // Make sure they're in alphabetical order to make testing more predictable.
            return string.Join(",", exts.OrderBy(x => x));
        }

        protected override string CreateHtmlString()
        {
            var hidden = new HiddenInputBuilder();
            hidden.Id = Id + "_Key";
            hidden.Name = Name + ".Key";

            // Need to move the unobtrusive attrusive values from
            // the div to the hidden input for things to work properly.
            hidden.With(GetUnobtrusiveHtmlAttributes());

            var div = CreateTagBuilder("div", includeUnobtrusiveValidation: false);
            div.Attributes.Remove("name"); // Not a form element so.
            div.AddCssClass("file-upload");

            Action<string, string> addAttr = (key, val) => {
                div.Attributes.Add(string.Format(DATA_KEY_FORMAT, key), val);
            };

            var ext = GetAllowedExtensions();
            if (!string.IsNullOrWhiteSpace(ext))
            {
                addAttr("ext", ext);
            }

            addAttr("url", Url);

            // inputname is hardcoded to "FileUpload" because all file uploads
            // go through FileController initially. FileController/Create expects
            // the uploaded file to always be "FileUpload". 
            addAttr("inputname", "FileUpload");
            addAttr("keyelement", hidden.Name);

            if (!string.IsNullOrWhiteSpace(OnComplete))
            {
                addAttr("oncomplete", OnComplete);
            }

            if (!string.IsNullOrWhiteSpace(ButtonText))
            {
                addAttr("buttontext", ButtonText);
            }

            return hidden.ToString() + div.ToString();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the allowed file extensions for uploads. Note that this does not validate
        /// that the files are actually of a valid file type.
        /// </summary>
        /// <param name="allowedExtensions"></param>
        /// <returns></returns>
        public FileUploadBuilder WithAllowedExtensions(params string[] allowedExtensions)
        {
            this.AllowedExtensions.AddRange(allowedExtensions);
            return this;
        }

        public FileUploadBuilder WithButtonText(string buttonText)
        {
            ButtonText = buttonText;
            return this;
        }

        public FileUploadBuilder WithOnComplete(string onComplete)
        {
            OnComplete = onComplete;
            return this;
        }

        public FileUploadBuilder WithUrl(string url)
        {
            Url = url;
            return this;
        }

        public FileUploadBuilder WithMaxUploadSize(int maxSize)
        {
            MaxSize = maxSize;
            return this;
        }

        #endregion
    }
}
