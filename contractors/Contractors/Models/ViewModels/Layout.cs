using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MMSINC.Data;
using MMSINC.Metadata;

namespace Contractors.Models.ViewModels
{
    public class LayoutModel
    {
        #region Properties

        public string TextGoesHere { get; set; }

        [Multiline]
        public string MultilineText { get; set; }

        public int WholeNumbersOnly { get; set; }
        public bool IsItTrue { get; set; }
        public decimal Pi { get; set; }
        [DropDown]
        public int DropDown { get; set; }
        public DateTime TodaysDate { get; set; }
        public DateRange DateRange { get; set; }
        public RequiredDateRange RequiredDateRange { get; set; }
        public NumericRange NumericRange { get; set; }

        public DateRange EqualsDateRange { get; set; }

        [DateTimePicker]
        public DateTime DateTimePicker { get; set; }

        public List<LayoutModel> TableRows { get; set; }

        [Select(SelectType.CheckBoxList)]
        public List<int> CheckBoxing { get; set; }

        [UIHint("StringArray")]
        public string[] MultiInput { get; set; }

        #endregion

        public LayoutModel()
        {
            TableRows = new List<LayoutModel>();
            TodaysDate = DateTime.Now;
            DateRange = new DateRange
            {
                Start = DateTime.Today,
                End = DateTime.Today.AddDays(7)
            };
            EqualsDateRange = new DateRange
            {
                Start = DateTime.Today,
                End = DateTime.Today.AddDays(22),
                Operator = RangeOperator.Equal
            };

            MultiInput = new[] { "Some value", "And another value" };

            // Don't add rows in the constructor, it'll cause a stackoverflow.
        }
    }
}