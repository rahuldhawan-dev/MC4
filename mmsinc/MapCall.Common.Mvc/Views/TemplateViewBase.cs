using System;
using System.Web;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using MMSINC.ClassExtensions;
using MMSINC.Helpers;
using StructureMap;

namespace MapCall.Common.Views
{
    /// <summary>
    /// Base view class used for shared editor/display templates. 
    /// If you wanna use this outside of this project, you need to use @inherits MapCall.Common.Views.TemplateViewBase(Of Whatever)
    /// in that project's view.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TemplateViewBase<T> : MvcViewBase<T>
    {
        #region Properties

        public ViewTemplateHelper ViewTemplateHelper { get; private set; }

        #endregion

        #region Private Methods

        protected IHtmlString RenderDisplayForTemplate(Func<object, HelperResult> fieldHtml)
        {
            return ViewTemplateHelper.RenderDisplayTemplate(Html.DisplayLabelFor(), Html.Description(), fieldHtml,
                includeWrapperHtml: ViewTemplateHelper.IncludeWrapperHtml);
        }

        protected IHtmlString RenderEditorForTemplate(Func<object, HelperResult> fieldHtml)
        {
            return ViewTemplateHelper.RenderEditorTemplate(Html.DisplayLabelFor(), Html.Description(), fieldHtml,
                includeWrapperHtml: ViewTemplateHelper.IncludeWrapperHtml);
        }

        /// <summary>
        /// Some editor templates just display a textbox with nothing special. So use this to save time.
        /// </summary>
        /// <returns></returns>
        protected IHtmlString RenderGenericSingleLineTextBoxEditorForTemplate(TextBoxType type = TextBoxType.Text)
        {
            Func<object, HelperResult> helperResult = (obj) => new HelperResult((writer) => {
                writer.Write(
                    Control.TextBox("")
                           .WithValue(ViewData.TemplateInfo.FormattedModelValue)
                           .AsType(type)
                           .With(ViewTemplateHelper.HtmlAttributes));
                writer.Write(
                    " "); // Need that extra gap between elements because HTML is just so great at spacing out elements.
                writer.Write(Html.ValidationMessage(""));
            });
            return RenderEditorForTemplate(helperResult);
        }

        protected IHtmlString RenderGenericDisplayForTemplate()
        {
            Func<object, HelperResult> helperResult = (obj) => new HelperResult((writer) => {
                // We need to html-encode the value ourselves since it's not gonna be done for us by razor.
                writer.Write(Html.Encode(ViewData.TemplateInfo.FormattedModelValue));
            });
            return RenderDisplayForTemplate(helperResult);
        }

        #endregion

        #region Exposed Methods

        #region Public Methods

        public override void InitHelpers()
        {
            base.InitHelpers();
            ViewTemplateHelper = new ViewTemplateHelper(this);
        }

        #endregion

        #endregion
    }

    // I'm just here so ReSharper doesn't get mad about the reference to this in web.config
    public abstract class TemplateViewBase : TemplateViewBase<object> { }
}
