using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using MMSINC.Data;
using MMSINC.Helpers.RazorTable;

// ReSharper disable CheckNamespace
namespace MMSINC.Helpers
    // ReSharper restore CheckNamespace
{
    public interface IRazorTableColumn<TModel>
    {
        #region Properties

        IRazorTableColumnHeader<TModel> Header { get; set; }
        bool IsSortable { get; }
        string SortBy { get; }
        bool IsVisible { get; set; }
        Action<TModel, TagBuilder> CellBuilder { get; set; }

        /// <summary>
        /// Gets/sets the optional footer cell for this column.
        /// </summary>
        IRazorTableColumnFooterCell FooterCell { get; set; }

        #endregion

        #region Methods

        TagBuilder RenderColumnHeader(HtmlHelper<TModel> helper, ISearchSet sortedSet = null);
        TagBuilder RenderCell(HtmlHelper<TModel> helper);

        #endregion
    }

    /// <summary>
    /// Base class that represents a table column for a specific model type.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    internal abstract class RazorTableColumn<TModel> : IRazorTableColumn<TModel>
    {
        #region Properties

        public IRazorTableColumnHeader<TModel> Header { get; set; }

        public Action<TModel, TagBuilder> CellBuilder { get; set; }

        /// <inheritdoc />
        public IRazorTableColumnFooterCell FooterCell { get; set; }

        public bool IsSortable
        {
            get { return !string.IsNullOrWhiteSpace(SortBy); }
        }

        /// <summary>
        /// Gets/sets the value needed to sort a table by this column. 
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// If false, does not render. True by default.
        /// </summary>
        public bool IsVisible { get; set; }

        public object HtmlAttributes { get; set; }

        #endregion

        #region Constructor

        protected RazorTableColumn()
        {
            IsVisible = true; // DEFAULT!!!!!!
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Do any extra rendering stuff required for the td tag for this column.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="tagBuilder">A TagBuilder representing the TD tag that will be used to render this column.</param>
        protected abstract void RenderCell(HtmlHelper<TModel> helper, TagBuilder tagBuilder);

        #endregion

        #region Public Methods

        /// <summary>
        /// Renders the column header for a RazorTable. 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="searchSet">Passed in by the RazorTable instance if the RazorTable is sortable.</param>
        /// <returns></returns>
        public TagBuilder RenderColumnHeader(HtmlHelper<TModel> helper, ISearchSet searchSet = null)
        {
            // Do not override this method. Make an implementation of IRazorTableColumnHeader instead.
            return Header.Render(helper, this, searchSet);
        }

        public TagBuilder RenderCell(HtmlHelper<TModel> helper)
        {
            var td = new TagBuilder("td");
            td.MergeAttributes(new RouteValueDictionary(HtmlAttributes ?? new { }));
            RenderCell(helper, td);

            // Calling the CellBuilder after so views can have access to the rendered text value.
            // You probably don't want to actually mess with the values at that point.

            CellBuilder?.Invoke(helper.ViewData.Model, td);

            return td;
        }

        #endregion
    }
}
