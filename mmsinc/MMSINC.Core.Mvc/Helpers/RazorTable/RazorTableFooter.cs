using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

// ReSharper disable CheckNamespace
namespace MMSINC.Helpers
    // ReSharper restore CheckNamespace
{
    public interface IRazorTableFooter
    {
        #region Methods

        TagBuilder Render(HtmlHelper htmlHelper, int colSpan);

        #endregion
    }

    internal abstract class RazorTableFooter : IRazorTableFooter
    {
        #region Private Methods

        protected abstract void Render(HtmlHelper htmlHelper, TagBuilder footerTag, int colSpan);

        #endregion

        #region Public Methods

        public TagBuilder Render(HtmlHelper htmlHelper, int colSpan)
        {
            // This is a div because we have too many tables that sprawl
            // way past the end of the window, making right-aligned text
            // "hidden" to the user. This div isn't part of the table, so
            // it shouldn't go off the end of the page like the table itself.
            var footy = new TagBuilder("div");
            footy.AddCssClass("table-footer");
            // Rows and Cells actually needed for the tfoot tag are
            // handled by inheritors.
            Render(htmlHelper, footy, colSpan);
            return footy;
        }

        #endregion
    }
}
