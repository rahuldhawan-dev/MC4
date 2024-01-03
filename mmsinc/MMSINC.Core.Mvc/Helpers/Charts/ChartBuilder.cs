using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MMSINC.Utilities;

namespace MMSINC.Helpers
{
    // This interface exists so ChartResult doesn't need to care about the generic
    // aspect of ChartBuilder.
    public interface IChartBuilder : IHtmlString { }

    public class ChartBuilder<TXValue, TYValue> : IChartBuilder
    {
        #region Fields

        private readonly Dictionary<string, string> _htmlAttributes = new Dictionary<string, string>();

        private object _yMinVal,
                       _yMaxVal;

        #endregion

        #region Properties

        /// <summary>
        /// Dictionary of html attributes used when rendering the tag.
        /// </summary>
        public IDictionary<string, string> HtmlAttributes
        {
            get { return _htmlAttributes; }
        }

        /// <summary>
        /// Gets/sets the interval for values on the X axis. Use this when you have different
        /// various values but want the grid to have evenly spaced values. This depends on the
        /// type of chart being used and the data being displayed. This is also only used
        /// when TXValue == DateTime.
        /// </summary>
        public ChartIntervalType Interval { get; set; }

        /// <summary>
        /// Gets the position for the chart legend.
        /// </summary>
        public ChartLegendPosition LegendPosition { get; set; }

        /// <summary>
        /// Gets/sets how many decimal places should be displayed for numeric values.
        /// Default is null(same as leaving it null or setting it to -1 in AmCharts) which
        /// displays all decimal places.
        /// </summary>
        public int? NumberPrecision { get; set; }

        /// <summary>
        /// Gets the collection of ChartSeries instances that contain chart data.
        /// </summary>
        public List<ChartSeries<TXValue, TYValue>> Series { get; private set; }

        /// <summary>
        /// Gets/sets how values should be sorted for specific chart types. Default is None.
        /// </summary>
        public ChartSortType SortType { get; set; }

        /// <summary>
        /// Optional title for the chart.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets/sets the type of chart being created. Default is ChartType.Line.
        /// </summary>
        public ChartType Type { get; set; }

        /// <summary>
        /// Gets/sets the label used to describe the Y axis values.
        /// </summary>
        public string YAxisLabel { get; set; }

        /// <summary>
        /// The minimum value for the y axis of the chart.
        /// </summary>
        public object YMinValue
        {
            get { return _yMinVal; }
            set
            {
                if (value != null && !(value is TYValue))
                {
                    // No conversions can be done here(ie float to int and the like) because this class
                    // isn't meant to deal with rounding and the like.
                    throw new InvalidOperationException("YMinValue must be null or of type " + typeof(TYValue).Name);
                }

                _yMinVal = value;
            }
        }

        /// <summary>
        /// The maximum value for the y axis of the chart.
        /// </summary>
        public object YMaxValue
        {
            get { return _yMaxVal; }
            set
            {
                if (value != null && !(value is TYValue))
                {
                    // No conversions can be done here(ie float to int and the like) because this class
                    // isn't meant to deal with rounding and the like.
                    throw new InvalidOperationException("YMaxValue must be null or of type " + typeof(TYValue).Name);
                }

                _yMaxVal = value;
            }
        }

        #endregion

        #region Constructors

        public ChartBuilder()
        {
            Series = new List<ChartSeries<TXValue, TYValue>>();
        }

        #endregion

        #region Private Methods

        private static string GetDataKey(string key)
        {
            return "data-chart-" + key;
        }

        private IDictionary<TXValue, Dictionary<object, object>> GetPrePopulatedDictionaryForDataConfiguration()
        {
            // NOTE: This will probably need to switch to a List<KeyValuePair<TXValue, Dictionary<string, object>>> someday.
            //        And should that happen, make a custom type cause that's annoying to write.
            switch (SortType)
            {
                case ChartSortType.LowToHigh:
                    return new SortedDictionary<TXValue, Dictionary<object, object>>();
                default:
                    return new Dictionary<TXValue, Dictionary<object, object>>();
            }
        }

        private ChartConfig GenerateConfig()
        {
            /*  For Line and Bar charts, data needs to be serialized like this:
                config.dataProvider = [
             *  { 
             *      x: "first x value",
             *      y1: "first series y value for first x value",
             *      y2: "second series y value for first x value"
             *  },
             *  { 
             *      x: "second x value",
             *      y1: "first series y value for second x value",
             *      y2: "second series y value for second x value"
             *  },
             * ]
             * 
             * Duplicate X values will result in duplicates on the chart and mess everything up.
             * 
             * Something else will need to be done here if we use it for XY charts or
             * other things that work with duplicate X values.
             */

            // Distinct call will need to go away for charts that allow duplicate x values.
            var sortedKeys = Series.SelectMany(x => x.Keys).Distinct();
            var sortDict = GetPrePopulatedDictionaryForDataConfiguration();
            var sconfigs = new List<SeriesConfig>();

            foreach (var k in sortedKeys)
            {
                var valDict = new Dictionary<object, object>();
                valDict["x"] = k;
                sortDict.Add(k, valDict);
            }

            // Must start with 1, 0 gets ignored by AmCharts for some reason.
            var curSeriesNumber = 1;
            foreach (var s in Series)
            {
                var valueField = "y" + curSeriesNumber;
                sconfigs.Add(new SeriesConfig {
                    title = s.Name,
                    valueField = valueField
                });

                foreach (var kv in s)
                {
                    sortDict[kv.Key][valueField] = kv.Value;
                }

                curSeriesNumber++;
            }

            return new ChartConfig {
                Series = sconfigs.ToArray(),
                Data = sortDict.Values.ToArray()
            };
        }

        private static string GenerateHiddenField(string @class, object value)
        {
            var strValue = JavaScriptSerializerFactory.Build().Serialize(value);

            var tb = new TagBuilder("input");
            tb.AddCssClass(@class);
            tb.Attributes.Add("type", "hidden");
            tb.Attributes.Add("value", strValue);
            return tb.ToString(TagRenderMode.SelfClosing);
        }

        private string GetInterval()
        {
            switch (Interval)
            {
                case ChartIntervalType.Minute:
                    return "mm";
                case ChartIntervalType.Hourly:
                    return "hh";
                case ChartIntervalType.Daily:
                    return "DD";
                case ChartIntervalType.Monthly:
                    return "MM";
                case ChartIntervalType.Yearly:
                    return "YYYY";

                default:
                    return null;
            }
        }

        private void ValidateChart()
        {
            if (Type == ChartType.SingleSeriesBar && Series.Count > 1)
            {
                throw new InvalidOperationException("A single series bar chart can only have one series.");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds an x and y value to a series based on the series' name. If a series
        /// does not exist for the given name, one is created for you.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        public void AddSeriesValue(string name, TXValue xValue, TYValue yValue)
        {
            var s = Series.SingleOrDefault(x => x.Name == name);
            if (s == null)
            {
                s = new ChartSeries<TXValue, TYValue>();
                s.Name = name;
                Series.Add(s);
            }

            s.Add(xValue, yValue);
        }

        /// <summary>
        /// Adds multiple x and y value to a series based on the series' name. If a series
        /// does not exist for the given name, one is created for you.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="values"></param>
        public void AddSeriesValues(string name, IDictionary<TXValue, TYValue> values)
        {
            foreach (var kv in values)
            {
                AddSeriesValue(name, kv.Key, kv.Value);
            }
        }

        public string ToHtmlString()
        {
            ValidateChart();

            var tb = new TagBuilder("div");
            tb.AddCssClass("chart");

            Action<string, object> addUnobtrusiveData = (key, value) => {
                if (value != null)
                {
                    tb.MergeAttribute(GetDataKey(key), value.ToString());
                }
            };

            addUnobtrusiveData("title", Title);
            addUnobtrusiveData("type", Type.ToString());
            addUnobtrusiveData("xtype",
                ChartBuilderUtility.GetJavascriptTypeCode(System.Type.GetTypeCode(typeof(TXValue))));
            addUnobtrusiveData("yaxislabel", YAxisLabel);
            addUnobtrusiveData("yminval", YMinValue);
            addUnobtrusiveData("ymaxval", YMaxValue);
            addUnobtrusiveData("ytype",
                ChartBuilderUtility.GetJavascriptTypeCode(System.Type.GetTypeCode(typeof(TYValue))));

            if (typeof(TXValue) == typeof(DateTime))
            {
                addUnobtrusiveData("interval", GetInterval());
            }

            if (LegendPosition != ChartLegendPosition.Bottom)
            {
                addUnobtrusiveData("legend", LegendPosition.ToString().ToLower());
            }

            if (NumberPrecision.HasValue)
            {
                addUnobtrusiveData("precision", NumberPrecision.Value);
            }

            // Add these last, don't allow for overwriting of above attributes.
            tb.MergeAttributes(HtmlAttributes);

            // NOTE: The hidden inputs are removed from the DOM when AmCharts
            //       does its chart creation thing.
            var hiddens = new StringBuilder();
            var config = GenerateConfig();
            hiddens.Append(GenerateHiddenField("chart-series", config.Series));
            hiddens.Append(GenerateHiddenField("chart-data", config.Data));

            tb.InnerHtml = hiddens.ToString();
            return tb.ToString(TagRenderMode.Normal);
        }

        public override string ToString()
        {
            return ToHtmlString();
        }

        #endregion

        #region Serialization helper classes

        // These are for the json stuff.

        private sealed class ChartConfig
        {
            public SeriesConfig[] Series { get; set; }
            public Array Data { get; set; }
        }

        private sealed class SeriesConfig
        {
            public string title { get; set; }
            public string valueField { get; set; }
        }

        #endregion
    }
}
