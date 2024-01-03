using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;

// ReSharper disable CheckNamespace
namespace MMSINC.Helpers
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// A table column that automatically populates its column header and value
    /// based on a specific model property.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    internal class RazorTableExpressionColumn<TModel, TProperty> : RazorTableColumn<TModel>
    {
        #region Fields

        private static readonly bool _isPatched;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Expression object that represents this column model's property. This can be passed to
        /// HtmlHelpers to get model data stuff.
        /// </summary>
        public Expression<Func<TModel, TProperty>> Expression { get; private set; }

        /// <summary>
        /// Gets the name of the property this column represents.
        /// </summary>
        public string PropertyName { get; private set; }

        #endregion

        #region Constructor

        static RazorTableExpressionColumn()
        {
            _isPatched = typeof(ModelMetadata).GetProperty("HtmlEncode") != null;
        }

        public RazorTableExpressionColumn(Expression<Func<TModel, TProperty>> expression)
        {
            expression.ThrowIfNull("expression");
            var header = new RazorTableExpressionColumnHeader<TModel, TProperty>(expression);
            Expression = expression;
            PropertyName = header.PropertyName;
            Header = header;
        }

        #endregion

        #region Protected Methods

        protected override void RenderCell(HtmlHelper<TModel> helper, TagBuilder tagBuilder)
        {
            // This MVC patch changed how DisplayText/DisplayTextFor works:
            // https://github.com/ASP-NET-MVC/aspnetwebstack/commit/2b12791aee4ffc56c7928b623bb45ee425813021
            //
            // The values returned are now html escaped which potentially breaks things.
            // When all machines have the patched MVC, this code should be able to be removed.

            var val = helper.DisplayValueFor(Expression).ToString();

            if (_isPatched)
            {
                tagBuilder.InnerHtml = val;
            }
            else
            {
                tagBuilder.SetInnerText(val);
            }
        }

        #endregion
    }
}
