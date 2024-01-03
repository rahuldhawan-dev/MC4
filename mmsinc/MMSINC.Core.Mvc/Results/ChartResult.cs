using System.Web.Mvc;
using MMSINC.Helpers;

namespace MMSINC.Results
{
    /// <summary>
    /// Returns a rendered chart tag as a partial content result. If you
    /// need anything more than that you should probably create and return
    /// an actual partial view.
    /// </summary>
    public class ChartResult : ActionResult
    {
        #region Properties

        /// <summary>
        /// Gets the data used to generate the different chart serieses.
        /// </summary>
        public IChartBuilder Chart { get; set; }

        #endregion

        #region Constructors

        public ChartResult(IChartBuilder chart)
        {
            Chart = chart;
        }

        #endregion

        public override void ExecuteResult(ControllerContext context)
        {
            var cr = new ContentResult();
            cr.Content = Chart.ToHtmlString();
            cr.ContentType = "text/html";
            cr.ExecuteResult(context);
        }
    }
}
