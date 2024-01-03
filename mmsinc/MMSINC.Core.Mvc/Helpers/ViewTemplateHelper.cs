using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.DictionaryExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Metadata;

namespace MMSINC.Helpers
{
    /// <summary>
    /// Base helper class for working with the Editor and Display templates.
    /// </summary>
    /// <remarks>
    /// Getting this stuff to work with EditorFor or DisplayFor:
    /// 
    /// Basically this:
    /// @Html.EditorFor(m => m.ModelProp, new { Prop = value });
    /// 
    /// Or in the case of passing extra html attributes along:
    /// @Html.EditorFor(m => m.ModelProp, new { html = new { attribute = value }});
    /// </remarks>
    public class ViewTemplateHelper
    {
        #region Constants

        public const string FIELD_PAIR_CLASS = "field-pair",
                            FIELD_PAIR_AND_NO_FLOAT_CLASS = FIELD_PAIR_CLASS + " no-float";

        public struct ViewDataKeys
        {
            #region Constants

            public const string DISPLAY_NAME = "DisplayName",
                                HTML_ATTRIBUTES = "html", // Intentionally lower case as to not conflict with @Html. 
                                CUSTOM_CSS_CLASS = "class",
                                INCLUDE_WRAPPER_HTML = "IncludeWrapperHtml";

            #endregion
        }

        private static readonly IHtmlString _emptyHtmlString = new HtmlString(string.Empty);

        #endregion

        #region Private Members

        private readonly ViewDataDictionary _viewData;

        #endregion

        #region Properties

        public WebViewPage View { get; private set; }

        public ViewDataDictionary ViewData
        {
            get { return _viewData; }
        }

        /// <summary>
        /// Gets the custom Display name passed in through ViewData if one exists.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return
                    ViewData.GetValueOrDefault<String>(
                        ViewDataKeys.DISPLAY_NAME, null);
            }
        }

        public string FieldPairWrapperClass
        {
            get
            {
                var result = FIELD_PAIR_CLASS;
                var customClass = ViewData.GetValueOrDefault<string>(ViewDataKeys.CUSTOM_CSS_CLASS, null);
                if (!string.IsNullOrWhiteSpace(customClass))
                {
                    result += " " + customClass;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the additional html attributes that may have been supplied to the template.
        /// </summary>
        public ViewDataDictionary HtmlAttributes
        {
            get { return ViewData.GetValueOrDefault<ViewDataDictionary>(ViewDataKeys.HTML_ATTRIBUTES, null); }
        }

        public bool IncludeWrapperHtml
        {
            get { return ViewData.GetValueOrDefault(ViewDataKeys.INCLUDE_WRAPPER_HTML, true); }
        }

        /// <summary>
        /// Gets the ViewData's ModelMetadata. NOTE: It can be null!
        /// </summary>
        private ModelMetadata ModelMetadata
        {
            get { return ViewData.ModelMetadata; }
        }

        #endregion

        #region Constructors

        public ViewTemplateHelper()
        {
            // Setting this to a generic(and not the non-generic ViewDataDictionary)
            // so that it autopopulates itself with a ModelMetadata object, even though
            // we don't techniaclly need it.
            _viewData = new ViewDataDictionary();
            Init();
        }

        public ViewTemplateHelper(WebViewPage view)
        {
            view.ThrowIfNull("view");
            View = view;
            _viewData = view.ViewData;
            Init();
        }

        #endregion

        #region Private Methods

        private void Init()
        {
            var dispName = DisplayName;
            //if (!string.IsNullOrWhiteSpace(dispName))
            if (dispName != null && ModelMetadata != null) // null here because we allow empty displaynames
            {
                ModelMetadata.DisplayName = dispName;
            }

            EnsureHtmlAttributes();

            if (View != null)
            {
                SetStringMaxLength();
            }
        }

        private void EnsureHtmlAttributes()
        {
            // In order to merge passed in HtmlAttributes with ones added by the view template,
            // we need to create a new ViewDataDictionary object. In order to take an anonymous
            // type and get a dictionary out of it, we need to create a new RouteValueDictionary.
            // Then we can add those keys manually to the ViewDataDictionary.
            var d = new ViewDataDictionary();

            var htmlAttr = ViewData.GetValueOrDefault<object>(ViewDataKeys.HTML_ATTRIBUTES, null);
            if (htmlAttr != null)
            {
                var htmlDict = HtmlHelperExtensions.AnonymousObjectToHtmlAttributes(htmlAttr);
                d.MergeIn(htmlDict);
            }

            if (ModelMetadata != null)
            {
                d.MergeIn(HtmlAttribute.GetHtmlAttributesFromMetadata(ModelMetadata));
            }

            ViewData[ViewDataKeys.HTML_ATTRIBUTES] = d;
        }

        private void SetStringMaxLength()
        {
            var stringLengthValidator = ModelValidatorProviders.Providers.GetValidators(ModelMetadata, View.ViewContext)
                                                               .SingleOrDefault(s => s is StringLengthAttributeAdapter);
            if (stringLengthValidator != null)
            {
                var rule = stringLengthValidator.GetClientValidationRules()
                                                .SingleOrDefault(r => r.ValidationType == "length");
                if (rule != null && rule.ValidationParameters.ContainsKey("max"))
                {
                    HtmlAttributes["maxlength"] = rule.ValidationParameters["max"].ToString();
                }
            }
        }

        private IHtmlString RenderTemplate(IHtmlString displayLabel, IHtmlString descriptionText,
            Func<object, HelperResult> fieldHtml, string[] wrapperCssClass, bool includeWrapperHtml, bool isRequired)
        {
            var field = RenderFieldTag(fieldHtml, includeWrapperHtml);
            if (!includeWrapperHtml)
            {
                return new HtmlString(field);
            }

            var label = RenderLabelTag(displayLabel, descriptionText, isRequired);
            var wrapper = new TagBuilder("div");
            var cssJoined = string.Join(" ", wrapperCssClass.Where(x => !string.IsNullOrWhiteSpace(x)));

            wrapper.AddCssClass(cssJoined);

            wrapper.InnerHtml = label + field;

            return new HtmlString(wrapper.ToString());
        }

        private string RenderDescriptionTag(IHtmlString descriptionText)
        {
            if (descriptionText == null)
            {
                return null;
            }

            var descript = new TagBuilder("span");
            descript.AddCssClass("fp-description");
            descript.InnerHtml = descriptionText.ToHtmlString();
            return descript.ToString();
        }

        private string RenderLabelTag(IHtmlString displayLabel, IHtmlString descriptionText, bool isRequired)
        {
            const string REQUIRED = "<span class=\"required-label\"> *</span>";

            // It's nice to have less html if the label junk isn't there.
            if (displayLabel == null)
            {
                return string.Empty;
            }

            var label = new TagBuilder("div");
            label.AddCssClass("label");
            label.InnerHtml = displayLabel.ToHtmlString() + (isRequired ? REQUIRED : string.Empty) +
                              RenderDescriptionTag(descriptionText);

            return label.ToString();
        }

        private string RenderFieldTag(Func<object, HelperResult> fieldHtml, bool includeWrapperHtml)
        {
            var fieldAllByItsLonesome = fieldHtml(null).ToHtmlString();
            if (!includeWrapperHtml)
            {
                return fieldAllByItsLonesome;
            }

            var outer = new TagBuilder("div");
            outer.AddCssClass("field");
            var inner = new TagBuilder("div"); // Wrapper is for stupid IE7 inherited margin bug on input/textarea tags.
            inner.InnerHtml = fieldAllByItsLonesome;
            outer.InnerHtml = inner.ToString();

            return outer.ToString();
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Renders a label and some field html in the usual DisplayFor template format.
        /// </summary>
        /// <param name="displayLabel">The html for the label side. Can be null.</param>
        /// <param name="descriptionText">Hint text displayed by the label.</param>
        /// <param name="fieldHtml">The html for the field side. Can not be null. Make an empty HtmlString if you need to.</param>
        /// <param name="cssClasses">Additional css classes to be added to the wrapper div.</param>
        /// <param name="includeWrapperHtml">If true, all the wrapper html is included. If false, only the actual editor and validation are included.</param>
        public IHtmlString RenderDisplayTemplate(IHtmlString displayLabel, IHtmlString descriptionText,
            IHtmlString fieldHtml, string cssClasses = null, bool includeWrapperHtml = true)
        {
            return RenderDisplayTemplate(displayLabel, descriptionText, fieldHtml.ToHelperResult(), cssClasses,
                includeWrapperHtml);
        }

        /// <summary>
        /// Renders a label and some field html in the usual DisplayFor template format.
        /// </summary>
        /// <param name="displayLabel">The html for the label side. Can be null.</param>
        /// <param name="descriptionText">Hint text displayed by the label.</param>
        /// <param name="fieldHtml">The html for the field side. Can not be null. Make an empty HtmlString if you need to.</param>
        /// <param name="cssClasses">Additional css classes to be added to the wrapper div.</param>
        /// <param name="includeWrapperHtml">If true, all the wrapper html is included. If false, only the actual editor and validation are included.</param>
        public IHtmlString RenderDisplayTemplate(IHtmlString displayLabel, IHtmlString descriptionText,
            Func<object, HelperResult> fieldHtml, string cssClasses = null, bool includeWrapperHtml = true)
        {
            return RenderTemplate(displayLabel, descriptionText, fieldHtml,
                new[] {FieldPairWrapperClass, "fp-display", cssClasses}, includeWrapperHtml, isRequired: false);
        }

        /// <summary>
        /// Renders a label and some field html in the usual EditorFor template format.
        /// </summary>
        /// <param name="displayLabel">The html for the label side. Can be null.</param>
        /// <param name="descriptionText">Hint text displayed by the label.</param>
        /// <param name="fieldHtml">The html for the field side. Can not be null. Make an empty HtmlString if you need to.</param>
        /// <param name="cssClasses">Additional css classes to be added to the wrapper div.</param>
        /// <param name="includeWrapperHtml">If true, all the wrapper html is included. If false, only the actual editor and validation are included.</param>
        public IHtmlString RenderEditorTemplate(IHtmlString displayLabel, IHtmlString descriptionText,
            IHtmlString fieldHtml, string cssClasses = null, bool includeWrapperHtml = true)
        {
            return RenderEditorTemplate(displayLabel, descriptionText, fieldHtml.ToHelperResult(), cssClasses,
                includeWrapperHtml);
        }

        /// <summary>
        /// Renders a label and some field html in the usual EditorFor template format.
        /// </summary>
        /// <param name="displayLabel">The html for the label side. Can be null.</param>
        /// <param name="descriptionText">Hint text displayed by the label.</param>
        /// <param name="fieldHtml">The html for the field side. Can not be null. Make an empty HtmlString if you need to.</param>
        /// <param name="cssClasses">Additional css classes to be added to the wrapper div.</param>
        /// <param name="includeWrapperHtml">If true, all the wrapper html is included. If false, only the actual editor and validation are included.</param>
        public IHtmlString RenderEditorTemplate(IHtmlString displayLabel, IHtmlString descriptionText,
            Func<object, HelperResult> fieldHtml, string cssClasses = null, bool includeWrapperHtml = true)
        {
            var isRequired = ModelMetadata != null && ModelMetadata.IsRequired;
            return RenderTemplate(displayLabel, descriptionText, fieldHtml,
                new[] {FieldPairWrapperClass, "fp-edit", cssClasses}, includeWrapperHtml, isRequired);
        }

        #endregion
    }
}
