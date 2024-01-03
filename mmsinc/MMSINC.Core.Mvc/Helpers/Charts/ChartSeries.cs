using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMSINC.Helpers
{
    // This is probably going to need to inherit from List instead of Dictionary at some point.
    // It mostly depends on the type of chart. Line and Bar chart can't deal with duplicate X values.
    // But an XY chart should be able to.
    public class ChartSeries<TXValue, TYValue> : Dictionary<TXValue, TYValue>
    {
        #region Properties

        /// <summary>
        /// Gets the name used for this data. This is used
        /// in the chart legend as well as in tooltips
        /// when hovering over data points.
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}
