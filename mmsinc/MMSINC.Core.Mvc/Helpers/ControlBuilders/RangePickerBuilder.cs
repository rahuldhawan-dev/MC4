using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.ClassExtensions.EnumExtensions;
using MMSINC.Data;

namespace MMSINC.Helpers
{
    public class RangePickerBuilder : ControlBuilder<RangePickerBuilder>
    {
        #region Fields

        private static readonly IEnumerable<RangeOperator> _operators =
            (RangeOperator[])Enum.GetValues(typeof(RangeOperator));

        #endregion

        #region Properties

        /// <summary>
        /// The control builder for the start value of a range picker. Must be set.
        /// </summary>
        public ControlBuilder StartBuilder { get; set; }

        /// <summary>
        /// The control builder for the end value of a range picker. Must be set.
        /// </summary>
        public ControlBuilder EndBuilder { get; set; }

        /// <summary>
        /// The dropdown control builder for the range operator. 
        /// </summary>
        public SelectListBuilder OperatorBuilder { get; private set; }

        #endregion

        #region Constructor

        public RangePickerBuilder()
        {
            OperatorBuilder = new SelectListBuilder();
            OperatorBuilder.Type = SelectListType.DropDown;
            OperatorBuilder.WithItems(_operators.Select(x => new SelectListItem {
                Text = x.DescriptionAttr(),
                Value = ((int)x).ToString()
            }));
        }

        #endregion

        #region Private Methods

        protected override string CreateHtmlString()
        {
            if (StartBuilder == null)
            {
                throw new InvalidOperationException("StartBuilder must be set.");
            }

            if (EndBuilder == null)
            {
                throw new InvalidOperationException("EndBuilder must be set.");
            }

            StartBuilder.AddCssClass("range-start");
            OperatorBuilder.AddCssClass("range-operator");
            EndBuilder.AddCssClass("range-end");

            var wrapper = CreateTagBuilder("div");
            wrapper.AddCssClass("range");
            // It's a div so the name attribute is useless here.
            wrapper.Attributes.Remove("name");

            // The spaces are needed so that the inputs aren't squashed right next to each other.
            // I don't wanna have to use CSS for that because it'd be a pain.
            wrapper.InnerHtml = StartBuilder + " " + OperatorBuilder + " " + EndBuilder;

            return wrapper.ToString();
        }

        #endregion
    }
}
