using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;

// ReSharper disable CheckNamespace
namespace MMSINC.Helpers
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// Represents a RazorTableColumnHeader that automatically populates a table cell's value
    /// based on a given expression.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    internal class RazorTableExpressionColumnHeader<TModel, TProperty> : RazorTableColumnHeader<TModel>
    {
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

        public RazorTableExpressionColumnHeader(Expression<Func<TModel, TProperty>> headerExpression)
        {
            headerExpression.ThrowIfNull("headerExpression");
            Expression = headerExpression;
            PropertyName = Utilities.Expressions.GetMember(Expression).Name;
        }

        #endregion

        #region Private Methods

        protected override string GetHtmlEscapedText(HtmlHelper<TModel> helper)
        {
            return helper.DisplayPrettyNameFor(Expression).ToHtmlString();
        }

        protected override void RenderAdditionalDataAttributes(TagBuilder thTag)
        {
            base.RenderAdditionalDataAttributes(thTag);
            // This is used for the mc-datatable element for toggling the display of columns.
            thTag.Attributes.Add("data-property", PropertyName);
        }

        #endregion
    }
}
