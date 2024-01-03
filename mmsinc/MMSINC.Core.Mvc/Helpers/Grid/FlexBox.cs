using System;
using System.Web.Mvc;

// ReSharper disable CheckNamespace
namespace MMSINC.Helpers
    // ReSharper restore CheckNamespace
{
    /// <summary>
    /// A GridBox that resizes based on the size of its parent container.
    /// </summary>
    public class FlexBox : GridBox
    {
        #region Constructor

        public FlexBox(Grid parentGrid) : base(parentGrid) { }

        #endregion

        protected override void Render(TagBuilder wrapperDiv)
        {
            wrapperDiv.AddCssClass(Grid.Css.FLEX_BOX_CLASS);
        }
    }
}
