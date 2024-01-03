using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace MMSINC.Helpers
{
    // TODO: Get cascades working in here too. Or get it working with ListBuilder instead so it can
    //       potentially be expanded to other controls.

    public enum SelectListType
    {
        /// <summary>
        /// A regular single select dropdown list.
        /// </summary>
        DropDown = 0, // Default value

        /// <summary>
        /// A regular listbox.
        /// </summary>
        ListBox,
    }

    /// <summary>
    /// Control builder for creating select and multiselect lists.
    /// </summary>
    public class SelectListBuilder : ListBuilder<SelectListBuilder>
    {
        #region Properties

        /// <summary>
        /// Gets/sets the type of SelectList to render.
        /// </summary>
        public SelectListType Type { get; set; }

        #endregion

        #region Private Methods

        protected override string CreateHtmlString()
        {
            var select = CreateTagBuilder("select");

            if (Type == SelectListType.ListBox)
            {
                select.Attributes["multiple"] = "multiple";
            }

            select.InnerHtml = GetOptionTags();

            return select.ToString(TagRenderMode.Normal);
        }

        private string GetOptionTags()
        {
            var sb = new StringBuilder();

            // Reuse the same TagBuilder to reduce mem usage.
            var tb = new TagBuilder("option");

            foreach (var val in GetItems())
            {
                // Clear the attributes out to make sure the TagBuilder
                // is properly being reset.
                tb.Attributes.Clear();
                tb.Attributes["value"] = val.Value;
                if (val.Selected)
                {
                    tb.Attributes["selected"] = "selected";
                }

                tb.SetInnerText(val.Text);
                sb.Append(tb);
            }

            return sb.ToString();
        }

        #endregion

        #region Public Methods

        public SelectListBuilder AsType(SelectListType type)
        {
            Type = type;
            return this;
        }

        #endregion
    }
}
