using System;
using System.Web.Mvc;

// ReSharper disable CheckNamespace
namespace MMSINC.Helpers
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// A GridBox that is fixed in size.
    /// </summary>
    public class FixedBox : GridBox
    {
        #region Properties

        public int Width { get; set; }

        #endregion

        #region Constructor

        public FixedBox(Grid parentGrid) : base(parentGrid) { }

        #endregion

        #region Private Methods

        protected override void Render(TagBuilder wrapperDiv)
        {
            wrapperDiv.AddCssClass(Grid.Css.FIXED_GRID_BOX_CLASS);

            // TODO: Test that this doesn't overwrite extra style stuff
            //       added in HtmlAttributes.
            wrapperDiv.MergeAttribute("style", "width:" + Width + "px;");
        }

        #endregion
    }
}
