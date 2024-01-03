using System;
using System.Web;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ObjectExtensions;

// ReSharper disable CheckNamespace
namespace MMSINC.Helpers
    // ReSharper restore CheckNamespace
{
    internal class RazorTableTemplateColumn<TModel> : RazorTableColumn<TModel>
    {
        #region Properties

        /// <summary>
        /// Gets the function that renders out a template for a specific model.
        /// </summary>
        public Func<TModel, IHtmlString> Template { get; private set; }

        #endregion

        #region Constructor

        public RazorTableTemplateColumn(Func<TModel, IHtmlString> template)
        {
            template.ThrowIfNull("template");
            Template = template;
        }

        #endregion

        #region Private Methods

        protected override void RenderCell(HtmlHelper<TModel> helper, TagBuilder tagBuilder)
        {
            var tempRendered = Template(helper.ViewData.Model);
            tagBuilder.InnerHtml = tempRendered != null ? tempRendered.ToString() : null;
        }

        #endregion
    }
}
