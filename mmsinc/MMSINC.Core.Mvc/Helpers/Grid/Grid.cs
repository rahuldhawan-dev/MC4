using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMSINC.ClassExtensions;
using MMSINC.Utilities;

// ReSharper disable CheckNamespace
namespace MMSINC.Helpers
    // ReSharper restore CheckNamespace
{
    public class Grid : ViewContextWriterWrapper, IHtmlString
    {
        #region Consts

        public struct Css
        {
            public const string GRID_CLASS = "grid",
                                COL_COUNT_CLASS_FORMAT = "grid-{0}col",
                                GRID_BOX_CLASS = "grid-box",
                                FIXED_GRID_BOX_CLASS = "grid-box-fixed",
                                FLEX_BOX_CLASS = "grid-box-flex";
        }

        #endregion

        #region Fields

        private readonly List<GridBox> _boxes = new List<GridBox>();

        #endregion

        #region Properties

        public IDictionary<string, object> HtmlAttributes { get; set; }

        #endregion

        #region Constructor

        public Grid(ViewContext viewContext, IViewDataContainer viewDataContainer) : base(viewContext,
            viewDataContainer) { }

        #endregion

        #region Private Methods

        protected override void DisposeCore()
        {
            if (_boxes.Any(x => !x.IsDisposed))
            {
                throw new Exception(
                    "One or more GridBox instances were left undisposed. Dispose them or else rendering is gonna get all weird.");
            }

            var output = new StringWriter();
            Render(output);
            OriginalWriter.Write(output.ToString());
            base.DisposeCore();
        }

        private TagBuilder CreateGridTag()
        {
            var grid = new TagBuilder("div");
            grid.AddCssClass(Css.GRID_CLASS);
            grid.AddCssClass(string.Format(Css.COL_COUNT_CLASS_FORMAT, _boxes.Count));

            if (HtmlAttributes != null)
            {
                grid.MergeAttributes(HtmlAttributes);
            }

            return grid;
        }

        private void Render(TextWriter outputWriter)
        {
            var grid = CreateGridTag();

            outputWriter.Write(grid.ToString(TagRenderMode.StartTag));
            foreach (var box in _boxes)
            {
                outputWriter.Write(box.Render());
            }

            outputWriter.Write(grid.ToString(TagRenderMode.EndTag));
        }

        private T AddBox<T>(T box) where T : GridBox
        {
            _boxes.Add(box);
            return box;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new box that resizes based on the other box sizes and the size of the grid container.
        /// </summary>
        /// <returns></returns>
        public FlexBox FlexBox(object htmlAttributes = null)
        {
            var html = HtmlHelperExtensions.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return AddBox(new FlexBox(this) {HtmlAttributes = html});
        }

        public FixedBox FixedBox(int width, object htmlAttributes = null)
        {
            var html = HtmlHelperExtensions.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return AddBox(new FixedBox(this) {
                HtmlAttributes = html,
                Width = width
            });
        }

        public string ToHtmlString()
        {
            using (var writer = new StringWriter())
            {
                Render(writer);
                return writer.ToString();
            }
        }

        #endregion
    }
}
