using MMSINC.Authentication;
using StructureMap;

namespace MapCall.Common.Views
{
    public abstract class PdfViewBase<T> : MvcViewBase<T>
    {
        #region Enums

        public enum MarginWidth
        {
            Zero = 0,
            EigthInch = 9,
            QuarterInch = 18,
            HalfInch = 36,
            ThreeQuarterInch = 54,
            Inch = 72,
            InchAndQuarter = 90,
            InchAndHalf = 108,
        }

        #endregion

        #region Properties

        public bool IsLandscape
        {
            set { ViewContext.ViewBag.IsLandscape = value; }
        }

        public MarginWidth TopMargin
        {
            set { ViewContext.ViewBag.TopMargin = (int)value; }
        }

        public MarginWidth BottomMargin
        {
            set { ViewContext.ViewBag.BottomMargin = (int)value; }
        }

        public MarginWidth LeftMargin
        {
            set { ViewContext.ViewBag.LeftMargin = (int)value; }
        }

        public MarginWidth RightMargin
        {
            set { ViewContext.ViewBag.RightMargin = (int)value; }
        }

        public bool ShowHeader
        {
            set { ViewContext.ViewBag.ShowHeader = value; }
        }

        public int HeaderHeight
        {
            set { ViewContext.ViewBag.HeaderHeight = value; }
        }

        public int FooterHeight
        {
            set { ViewContext.ViewBag.FooterHeight = value; }
        }

        public string HeaderHtml
        {
            set { ViewContext.ViewBag.HeaderHtml = value; }
        }

        public string FooterHtml
        {
            set { ViewContext.ViewBag.FooterHtml = value; }
        }

        public bool ShowPageNumbersInHeader
        {
            set { ViewContext.ViewBag.ShowPageNumbersInHeader = value; }
        }

        public bool ShowPageNumbersInFooter
        {
            set { ViewContext.ViewBag.ShowPageNumbersInFooter = value; }
        }

        public string PageNumberFormat
        {
            set { ViewContext.ViewBag.PageNumberFormat = value; }
        }

        public bool SkipHeaderOnFirstPage
        {
            set { ViewContext.ViewBag.SkipHeaderOnFirstPage = value; }
        }

        #endregion
    }
}
