using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

// ReSharper disable once CheckNamespace
namespace MMSINC.Helpers
{
    [Obsolete("Don't use this interface. It offers no benefit over reference RazorTable directly.")]
    public interface IRazorTableRowCollection<T> : IHtmlString
    {
        HtmlHelper HtmlHelper { get; set; }
        IEnumerable<T> Model { get; set; }
        Action<T, TagBuilder> RowBuilder { get; set; }

        RazorTable<T> ColumnFor<TProperty>(Expression<Func<T, TProperty>> expression);
        RazorTable<T> ColumnFor<TProperty>(Expression<Func<T, TProperty>> expression, string headerText);
        RazorTable<T> IsVisible(bool isVisible);

        RazorTable<T> SortableColumnFor<TProperty>(Expression<Func<T, TProperty>> expression,
            object htmlAttributes = null);

        RazorTable<T> SortableColumnFor<TProperty>(Expression<Func<T, TProperty>> expression, string sortBy,
            object htmlAttributes = null);

        RazorTable<T> SortableColumnFor<TProperty>(Expression<Func<T, TProperty>> expression, string headerText,
            string sortBy, object htmlAttributes = null);

        RazorTable<T> SortableTemplateColumnFor(string sortBy, Func<T, IHtmlString> template);
        RazorTable<T> SortableTemplateColumnFor(string sortBy, Func<T, Func<T, HelperResult>> template);
        RazorTable<T> SortableTemplateColumnFor(string headerText, string sortBy, Func<T, IHtmlString> template);

        RazorTable<T> SortableTemplateColumnFor(string headerText, string sortBy,
            Func<T, Func<T, HelperResult>> template);

        RazorTable<T> SortableTemplateColumnFor<TProperty>(Expression<Func<T, TProperty>> expression, string sortBy,
            Func<T, Func<T, HelperResult>> template);

        RazorTable<T> TemplateColumnFor(Func<T, IHtmlString> template);
        RazorTable<T> TemplateColumnFor(Func<T, Func<T, HelperResult>> template);
        RazorTable<T> TemplateColumnFor(string headerText, Func<T, IHtmlString> template);
        RazorTable<T> TemplateColumnFor(string headerText, Func<T, Func<T, HelperResult>> template);

        RazorTable<T> TemplateColumnFor<TProperty>(Expression<Func<T, TProperty>> expression,
            Func<T, Func<T, HelperResult>> template);

        string ToHtmlString();
        string ToString();
        RazorTable<T> WithRowBuilder(Action<T, TagBuilder> rowHelper);
    }
}
