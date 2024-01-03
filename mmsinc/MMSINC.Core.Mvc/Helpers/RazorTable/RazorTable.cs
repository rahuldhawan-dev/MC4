using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using MMSINC.ClassExtensions;
using MMSINC.Data;
using MMSINC.Helpers.RazorTable;
using MMSINC.Utilities;

// ReSharper disable CheckNamespace
namespace MMSINC.Helpers
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// Fluent html helper for rendering tables.
    /// </summary>
    public class RazorTable<T> : IRazorTableRowCollection<T>
    {
        #region Fields

        private readonly ISearchSet<T> _searchSetModelParent;

        #endregion

        #region Properties

        public IEnumerable<T> Model { get; set; }
        public string Caption { get; set; }

        /// <summary>
        /// Optional display text that can be used in place of the Caption if the table
        /// does not have any data.
        /// </summary>
        public string EmptyResultCaption { get; set; }

        public IRazorTableFooter Footer { get; set; }
        public HtmlHelper HtmlHelper { get; set; }
        public IDictionary<string, object> HtmlAttributes { get; set; }

        public Action<T, TagBuilder> RowBuilder { get; set; }

        /// <summary>
        /// List of columns that have been added to the table. Don't use this directly, use the
        /// various ColumnFor methods instead.
        /// </summary>
        internal List<IRazorTableColumn<T>> Columns { get; private set; }

        /// <summary>
        /// Returns the last column that was added to the Columns list.
        /// </summary>
        /// <remarks>
        /// 
        /// This used to be a private field, but because the Columns property
        /// can be Clear'ed we don't want a private field to be stuck with
        /// a reference to something that no longer exists.
        /// 
        /// </remarks>
        private IRazorTableColumn<T> LastAddedColumn
        {
            get { return Columns.Last(); }
        }

        /// <summary>
        /// Returns true if this RazorTable instance was created with an ISearchSet and the ISearchSet.EnablePaging == true.
        /// </summary>
        public bool IsPagable { get; private set; }

        /// <summary>
        /// Returns true if this RazorTable instance was created with an ISearchSet.
        /// </summary>
        public bool IsSortable { get; protected set; }

        /// <summary>
        /// Determines if the table headers should still render if there are now rows. Default is TRUE for 
        /// backwards compatibility.
        /// </summary>
        public bool IncludeHeadersForEmptyTable { get; set; }

        private bool CanRenderEmptyResultCaption
        {
            get { return (!GetModelItems().Any() && EmptyResultCaption != null); }
        }

        #endregion

        #region Constructors

        private RazorTable()
        {
            // ReSharper disable once MustUseReturnValue
            IncludeHeadersWhenTableIsEmpty(true);
        }

        public RazorTable(IEnumerable<T> model, object htmlAttributes) : this()
        {
            Columns = new List<IRazorTableColumn<T>>();
            Model = model;
            HtmlAttributes = new RouteValueDictionary(htmlAttributes);
        }

        public RazorTable(ISearchSet<T> model, object htmlAttributes)
            : this(model?.Results, htmlAttributes)
        {
            _searchSetModelParent = model;
            IsSortable = true;
            IsPagable = model != null && model.EnablePaging;
        }

        #endregion

        #region Private Methods

        private void AddColumn(IRazorTableColumn<T> column)
        {
            Columns.Add(column);
        }

        private void EnsureSortable(string sortBy)
        {
            if (!IsSortable && !string.IsNullOrWhiteSpace(sortBy))
            {
                throw new InvalidOperationException("Can not add a sortable column on an unsortable table.");
            }
        }

        private void EnsurePagable()
        {
            if (!IsPagable)
            {
                throw new InvalidOperationException(
                    "Can not add a paginated footer when the table's model does not implement ISearchSet or if ISearchSet.EnablePaging == false.");
            }
        }

        /// <summary>
        /// Returns a Func that converts a HelperResult to an IHtmlString. This also wraps the actual HelperResult
        /// so that it can properly render form tags.
        /// </summary>
        private Func<T, IHtmlString> WrapTemplateForHtmlHelper(Func<T, Func<T, HelperResult>> template)
        {
            return (model) => {
                var helperResultFunc = template(model);
                var wrapped = HtmlHelperExtensions.WrapHelperResult(HtmlHelper, helperResultFunc);
                return wrapped(default(T));
            };
        }

        #region Fluenting

        private RazorTable<T> AddColumnFor<TProperty>(Expression<Func<T, TProperty>> expression, string sortBy,
            object htmlAttributes = null)
        {
            EnsureSortable(sortBy);
            AddColumn(new RazorTableExpressionColumn<T, TProperty>(expression) {
                SortBy = sortBy,
                HtmlAttributes = htmlAttributes
            });
            return this;
        }

        private RazorTable<T> AddColumnFor<TProperty>(Expression<Func<T, TProperty>> expression, string headerText,
            string sortBy, object htmlAttributes = null)
        {
            EnsureSortable(sortBy);
            AddColumn(new RazorTableExpressionColumn<T, TProperty>(expression) {
                Header = new RazorTableColumnHeader<T> {Text = headerText},
                SortBy = sortBy,
                HtmlAttributes = htmlAttributes
            });
            return this;
        }

        private RazorTable<T> AddTemplateColumnFor(string headerText, string sortBy, Func<T, IHtmlString> template)
        {
            EnsureSortable(sortBy);
            AddColumn(new RazorTableTemplateColumn<T>(template) {
                Header = new RazorTableColumnHeader<T> {Text = headerText},
                SortBy = sortBy
            });
            return this;
        }

        private RazorTable<T> AddTemplateColumnFor(string headerText, string sortBy,
            Func<T, Func<T, HelperResult>> template)
        {
            EnsureSortable(sortBy);
            var templateWrapper = WrapTemplateForHtmlHelper(template);
            return AddTemplateColumnFor(headerText, sortBy, templateWrapper);
        }

        private RazorTable<T> AddTemplateColumnFor<TProperty>(Expression<Func<T, TProperty>> expression, string sortBy,
            Func<T, Func<T, HelperResult>> template)
        {
            EnsureSortable(sortBy);
            var templateWrapper = WrapTemplateForHtmlHelper(template);
            AddColumn(new RazorTableTemplateColumn<T>(templateWrapper) {
                Header = new RazorTableExpressionColumnHeader<T, TProperty>(expression),
                SortBy = sortBy
            });
            return this;
        }

        #endregion

        #region Rendering

        protected TagBuilder RenderRow(T modelItem)
        {
            var row = new TagBuilder("tr");
            var rowHelper = HtmlHelper.HtmlHelperFor(modelItem);

            using (var writer = new StringWriter())
            {
                foreach (var col in Columns)
                {
                    if (col.IsVisible)
                    {
                        writer.Write(col.RenderCell(rowHelper));
                    }
                }

                row.InnerHtml = writer.ToString();
            }

            RowBuilder?.Invoke(modelItem, row);

            return row;
        }

        private TagBuilder RenderCaption()
        {
            var captionText = CanRenderEmptyResultCaption ? EmptyResultCaption : Caption;

            if (captionText == null)
            {
                return null;
            }

            var cap = new TagBuilder("caption");
            cap.SetInnerText(captionText);
            return cap;
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets whether or not the last added column will be rendered or not. If false, no html is sent to
        /// the client for that column.
        /// </summary>
        /// <param name="isVisible"></param>
        /// <returns></returns>
        public RazorTable<T> IsVisible(bool isVisible)
        {
            LastAddedColumn.IsVisible = isVisible;
            return this;
        }

        #region SortableTemplateColumnFor

        /// <summary>
        /// Creates a new column using the given template. 
        /// Lets you do this: SortableTemplateColumnFor("SortBy.Something", x => Html.DisplayStuffFor(x => x.Property));
        /// </summary>
        public RazorTable<T> SortableTemplateColumnFor(string sortBy, Func<T, IHtmlString> template)
        {
            return SortableTemplateColumnFor(headerText: null, sortBy: sortBy, template: template);
        }

        /// <summary>
        /// Creates a new column using the given template. 
        /// Lets you do this: SortableTemplateColumnFor("SortBy.Something", "Some Header", x => Html.DisplayStuffFor(x => x.Property));
        /// </summary>
        public RazorTable<T> SortableTemplateColumnFor(string headerText, string sortBy, Func<T, IHtmlString> template)
        {
            return AddTemplateColumnFor(headerText: headerText, sortBy: sortBy, template: template);
        }

        /// <summary>
        /// Lets you do this: SortableTemplateColumnFor("SortBy.Something", model => @&lt;div&gt;@model.SomeProperty&lt;/div&gt;) where model is passed in for each row
        /// and the expression is used to generate the header.
        /// </summary>
        public RazorTable<T> SortableTemplateColumnFor(string sortBy, Func<T, Func<T, HelperResult>> template)
        {
            return SortableTemplateColumnFor(headerText: null, sortBy: sortBy, template: template);
        }

        /// <summary>
        /// Lets you do this: SortableTemplateColumnFor("SortBy.Something", "Some Header", model => @&lt;div&gt;@model.SomeProperty&lt;/div&gt;) where model is passed in for each row
        /// and the expression is used to generate the header.
        /// </summary>
        public RazorTable<T> SortableTemplateColumnFor(string headerText, string sortBy,
            Func<T, Func<T, HelperResult>> template)
        {
            return AddTemplateColumnFor(headerText: headerText, sortBy: sortBy, template: template);
        }

        /// <summary>
        /// Lets you do this: SortableTemplateColumnFor("SortBy.Something", x => x.Property, model => @&lt;div&gt;@model.SomeProperty&lt;/div&gt;) where model is passed in for each row
        /// and the expression is used to generate the header.
        /// </summary>
        public RazorTable<T> SortableTemplateColumnFor<TProperty>(Expression<Func<T, TProperty>> expression,
            string sortBy,
            Func<T, Func<T, HelperResult>> template)
        {
            return AddTemplateColumnFor(expression, sortBy, template);
        }

        #endregion

        #region TemplateColumnFor

        /// <summary>
        /// Creates a new column using the given template. 
        /// Lets you do this: TemplateColumnFor(x => Html.DisplayStuffFor(x => x.Property));
        /// </summary>
        public RazorTable<T> TemplateColumnFor(Func<T, IHtmlString> template)
        {
            return AddTemplateColumnFor(headerText: null, sortBy: null, template: template);
        }

        /// <summary>
        /// Creates a new column using the given template. 
        /// Lets you do this: TemplateColumnFor("Some Header", x => Html.DisplayStuffFor(x => x.Property));
        /// </summary>
        public RazorTable<T> TemplateColumnFor(string headerText, Func<T, IHtmlString> template)
        {
            return AddTemplateColumnFor(headerText: headerText, sortBy: null, template: template);
        }

        public RazorTable<T> TemplateColumnFor(string headerText, string sortBy, Func<T, IHtmlString> template)
        {
            return AddTemplateColumnFor(headerText: headerText, sortBy: sortBy, template: template);
        }

        /// <summary>
        /// Lets you do this: TemplateColumnFor(model => @&lt;div&gt;@model.SomeProperty&lt;/div&gt;) where model is passed in for each row
        /// and the expression is used to generate the header.
        /// </summary>
        public RazorTable<T> TemplateColumnFor(Func<T, Func<T, HelperResult>> template)
        {
            return TemplateColumnFor(null, template);
        }

        /// <summary>
        /// Lets you do this: TemplateColumnFor("Some Header", model => @&lt;div&gt;@model.SomeProperty&lt;/div&gt;) where model is passed in for each row
        /// and the expression is used to generate the header.
        /// </summary>
        public RazorTable<T> TemplateColumnFor(string headerText, Func<T, Func<T, HelperResult>> template)
        {
            return AddTemplateColumnFor(headerText, null, template);
        }

        /// <summary>
        /// Lets you do this: TemplateColumnFor(x => x.Property, model => @&lt;div&gt;@model.SomeProperty&lt;/div&gt;) where model is passed in for each row
        /// and the expression is used to generate the header.
        /// </summary>
        public RazorTable<T> TemplateColumnFor<TProperty>(Expression<Func<T, TProperty>> expression,
            Func<T, Func<T, HelperResult>> template)
        {
            return AddTemplateColumnFor(expression, null, template);
        }

        #endregion

        #region ColumnFor

        /// <summary>
        /// Adds a column to the table.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression">The expression for the property getter that this column represents.</param>
        /// <returns></returns>
        public RazorTable<T> ColumnFor<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            return AddColumnFor(expression, null);
        }

        /// <summary>
        /// Adds a column to the table.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression">The expression for the property getter that this column represents.</param>
        /// <param name="headerText">Custom header text for this column.</param>
        /// <returns></returns>
        public RazorTable<T> ColumnFor<TProperty>(Expression<Func<T, TProperty>> expression, string headerText)
        {
            return AddColumnFor(expression, headerText, null);
        }

        #endregion

        #region SortableColumnFor

        /// <summary>
        /// Adds a sortable column to the table. The SortBy string is dictated by the expression property.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression">The expression for the property getter that this column represents.</param>
        /// <returns></returns>
        public RazorTable<T> SortableColumnFor<TProperty>(Expression<Func<T, TProperty>> expression,
            object htmlAttributes = null)
        {
            var sortBy = Expressions.GetMember(expression).Name;
            return AddColumnFor(expression, sortBy, htmlAttributes);
        }

        /// <summary>
        /// Adds a sortable column to the table.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression">The expression for the property getter that this column represents.</param>
        /// <returns></returns>
        public RazorTable<T> SortableColumnFor<TProperty>(Expression<Func<T, TProperty>> expression, string sortBy,
            object htmlAttributes = null)
        {
            return AddColumnFor(expression, sortBy, htmlAttributes);
        }

        /// <summary>
        /// Adds a sortable column to the table.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression">The expression for the property getter that this column represents.</param>
        /// <param name="headerText">Custom header text for this column.</param>
        /// <returns></returns>
        public RazorTable<T> SortableColumnFor<TProperty>(Expression<Func<T, TProperty>> expression, string headerText,
            string sortBy, object htmlAttributes = null)
        {
            return AddColumnFor(expression, headerText, sortBy, htmlAttributes);
        }

        #endregion

        #region WithCaption

        public RazorTable<T> WithCaption(string caption)
        {
            Caption = caption;
            return this;
        }

        #endregion

        #region WithEmptyResultCaption

        public RazorTable<T> WithEmptyResultCaption(string text)
        {
            EmptyResultCaption = text;
            return this;
        }

        #endregion

        #region IncludeHeadersForEmptyTable

        public RazorTable<T> IncludeHeadersWhenTableIsEmpty(bool yesNo)
        {
            IncludeHeadersForEmptyTable = yesNo;
            return this;
        }

        #endregion

        #region WithPaginatedFooter

        /// <summary>
        /// Adds a footer to the table using PaginatedHelper.
        /// </summary>
        public RazorTable<T> WithPaginatedFooter(bool showPageSizeOptions = true)
        {
            EnsurePagable();
            Footer = new RazorTablePaginatedFooter(_searchSetModelParent) {
                ShowPageSizeOptions = showPageSizeOptions
            };
            return this;
        }

        #endregion

        #region WithRowBuilder

        /// <summary>
        /// Allows for modifying the TagBuilder instance being used when rendering a row 
        /// for a specific model item. This is for the entire row, not individual cells. The
        /// RowBuilder is called after all of the cells have been rendered and added to the TagBuilder.
        /// </summary>
        /// <param name="rowHelper"></param>
        /// <returns></returns>
        public RazorTable<T> WithRowBuilder(Action<T, TagBuilder> rowHelper)
        {
            RowBuilder = rowHelper;
            return this;
        }

        #endregion

        #region WithCellBuilder

        /// <summary>
        /// Allows for modifying each cell's TagBuilder as the table renders. The cell builder
        /// is attached to the last created column. 
        /// </summary>
        /// <param name="cellHelper"></param>
        /// <returns></returns>
        public RazorTable<T> WithCellBuilder(Action<T, TagBuilder> cellHelper)
        {
            var lastAdded = LastAddedColumn;
            if (lastAdded == null)
            {
                throw new InvalidOperationException("You can not call WithCellBuilder before a column exists.");
            }

            lastAdded.CellBuilder = cellHelper;
            return this;
        }

        #endregion

        #region WithFooterCell

        /// <summary>
        /// Adds a footer cell to the last added column. The given value is displayed as-is, so you must include
        /// any additional formatting if necessary.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public RazorTable<T> WithFooterCell(object value)
        {
            var lastAdded = LastAddedColumn;
            if (lastAdded == null)
            {
                throw new InvalidOperationException("You can not call WithFooterCell before a column has been added.");
            }

            lastAdded.FooterCell = new RazorTableColumnFooterCell {
                Value = value
            };

            return this;
        }

        #endregion

        protected IEnumerable<T> GetModelItems()
        {
            if (Model != null)
            {
                foreach (var item in Model)
                {
                    yield return item;
                }
            }
        }

        private bool CanRenderHeaders
        {
            get { return GetModelItems().Any() || IncludeHeadersForEmptyTable; }
        }

        public override string ToString()
        {
            var modelItems = GetModelItems();
            var sb = new StringBuilder();
            sb.Append("<div class=\"table-wrapper\">");

            using (var writer = new StringWriter())
            {
                writer.Write(RenderCaption());

                if (CanRenderHeaders)
                {
                    writer.Write("<thead><tr>");

                    // This can be reused since it's a null model.
                    var headerHelper = HtmlHelper.HtmlHelperFor<T>();

                    foreach (var col in Columns)
                    {
                        if (!col.IsVisible)
                        {
                            continue;
                        }

                        writer.Write(col.RenderColumnHeader(headerHelper, _searchSetModelParent));
                    }

                    writer.Write("</tr></thead>");
                }

                writer.Write("<tbody>");

                foreach (var item in modelItems)
                {
                    writer.Write(RenderRow(item));
                }

                writer.Write("</tbody>");

                var needsFooterRow = Columns.Any(x => x.FooterCell != null);
                if (needsFooterRow)
                {
                    writer.Write("<tfoot><tr>");

                    foreach (var column in Columns)
                    {
                        if (column.FooterCell != null)
                        {
                            writer.Write(column.FooterCell.RenderCell());
                        }
                        else
                        {
                            // Need to render empty cells if there's not a FooterCell.
                            // Could come back at some point to make this do a colspan or something.
                            writer.Write(new TagBuilder("td"));
                        }
                    }

                    writer.Write("</tr></tfoot>");
                }

                var tb = new TagBuilder("table");
                tb.MergeAttributes(HtmlAttributes);
                tb.InnerHtml = writer.ToString();

                sb.Append(tb);

                if (Footer != null)
                {
                    var renderedFooter = Footer.Render(HtmlHelper.HtmlHelperFor<object>(), Columns.Count);
                    // Don't render an empty footer if nothing's been added to the tag.
                    if (!string.IsNullOrWhiteSpace(renderedFooter.InnerHtml))
                    {
                        sb.Append(renderedFooter.ToString());
                    }
                }
            }

            sb.Append("</div>");
            return sb.ToString();
        }

        public string ToHtmlString()
        {
            return ToString();
        }

        #endregion
    }
}
