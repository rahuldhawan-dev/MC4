using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MMSINC.Utilities;

// ReSharper disable CheckNamespace
namespace MMSINC.Helpers
    // ReSharper restore CheckNamespace
{
    public abstract class GridBox : ViewContextWriterWrapper
    {
        #region Properties

        internal bool IsDisposed { get; private set; }

        public IDictionary<string, object> HtmlAttributes { get; set; }

        #endregion

        #region Constructor

        protected GridBox(Grid parentGrid) : base(parentGrid) { }

        #endregion

        #region Private Methods

        protected override void DisposeCore()
        {
            base.DisposeCore();
            IsDisposed = true;
        }

        protected abstract void Render(TagBuilder wrapperDiv);

        #endregion

        #region Public Methods

        public string Render()
        {
            var div = new TagBuilder("div");
            div.AddCssClass(Grid.Css.GRID_BOX_CLASS);

            if (HtmlAttributes != null)
            {
                div.MergeAttributes(HtmlAttributes);
            }

            div.InnerHtml = "<div>" + Writer.ToString() + "</div>";
            Render(div);
            return div.ToString();
        }

        #endregion
    }
}
