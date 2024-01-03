using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Helpers;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MMSINC.Core.MvcTest.Helpers.Charts
{
    [TestClass]
    public class ChartBuilderTest
    {
        #region Consts

        /// <summary>
        /// This is what the opening div tag should look like when all properties are at their
        /// default values.
        /// </summary>
        private const string EXPECTED_RENDERING_WHEN_NOTHING_IS_SET =
            "<div class=\"chart\" data-chart-type=\"Line\" data-chart-xtype=\"string\" data-chart-ytype=\"int\">";

        #endregion

        #region Fields

        private ChartBuilder<DateTime, int> _dateTarget;
        private ChartBuilder<string, int> _stringTarget;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _dateTarget = new ChartBuilder<DateTime, int>();
            _stringTarget = new ChartBuilder<string, int>();
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestConstructorDefaultsSortTypeToNone()
        {
            _dateTarget = new ChartBuilder<DateTime, int>();
            Assert.AreEqual(ChartSortType.None, _dateTarget.SortType);
        }

        [TestMethod]
        public void TestConstructorDefaultsChartTypeToLine()
        {
            _dateTarget = new ChartBuilder<DateTime, int>();
            Assert.AreEqual(ChartType.Line, _dateTarget.Type);
        }

        [TestMethod]
        public void TestConstructorDefaultsIntervalToNone()
        {
            _dateTarget = new ChartBuilder<DateTime, int>();
            Assert.AreEqual(ChartIntervalType.None, _dateTarget.Interval);
        }

        [TestMethod]
        public void TestConstructorDefaultsLegendPositionToBottom()
        {
            _dateTarget = new ChartBuilder<DateTime, int>();
            Assert.AreEqual(ChartLegendPosition.Bottom, _dateTarget.LegendPosition);
        }

        #endregion

        #region Rendering

        [TestMethod]
        public void TestToHtmlStringThrowsInvalidOperationExceptionIfTypeIsSingleSeriesBarAndMoreThanOneSeriesIsAdded()
        {
            _stringTarget.Type = ChartType.SingleSeriesBar;
            _stringTarget.AddSeriesValue("one", "oh", 1);
            _stringTarget.AddSeriesValue("oops", "too many", 2);
            MyAssert.Throws<InvalidOperationException>(() => _stringTarget.ToHtmlString());
        }

        [TestMethod]
        public void TestToHtmlStringReturnsDivWithChartCssClass()
        {
            var result = _stringTarget.ToHtmlString();
            StringAssert.StartsWith(result, EXPECTED_RENDERING_WHEN_NOTHING_IS_SET);
        }

        [TestMethod]
        public void TestToHtmlStringRendersDataTitleAttributeIfTitleIsSet()
        {
            StringAssert.StartsWith(_stringTarget.ToHtmlString(), EXPECTED_RENDERING_WHEN_NOTHING_IS_SET);

            _stringTarget.Title = "Some Title";
            var expected =
                "<div class=\"chart\" data-chart-title=\"Some Title\" data-chart-type=\"Line\" data-chart-xtype=\"string\" data-chart-ytype=\"int\">";
            StringAssert.StartsWith(_stringTarget.ToHtmlString(), expected);
        }

        [TestMethod]
        public void TestToHtmlStringRendersYAxisTitleAttributeIfYAxisTitleIsSet()
        {
            StringAssert.StartsWith(_stringTarget.ToHtmlString(), EXPECTED_RENDERING_WHEN_NOTHING_IS_SET);

            _stringTarget.YAxisLabel = "Some Axis";
            var expected =
                "<div class=\"chart\" data-chart-type=\"Line\" data-chart-xtype=\"string\" data-chart-yaxislabel=\"Some Axis\" data-chart-ytype=\"int\">";
            StringAssert.StartsWith(_stringTarget.ToHtmlString(), expected);
        }

        [TestMethod]
        public void TestToHtmlStringRendersAdditionalHtmlAttributes()
        {
            var expected =
                "<div class=\"chart\" data-chart-type=\"Line\" data-chart-xtype=\"string\" data-chart-ytype=\"int\" some-attribute=\"value\">";
            _stringTarget.HtmlAttributes.Add("some-attribute", "value");
            StringAssert.StartsWith(_stringTarget.ToHtmlString(), expected);
        }

        [TestMethod]
        public void TestToHtmlStringDoesNotAllowHtmlAttributesToOverwriteRequiredAttributes()
        {
            _stringTarget.HtmlAttributes.Add("data-chart-type", "NOPE LOLZ");
            StringAssert.StartsWith(_stringTarget.ToHtmlString(), EXPECTED_RENDERING_WHEN_NOTHING_IS_SET);
        }

        [TestMethod]
        public void TestToHtmlStringAddsExpectedIntervalAttributeValuesForDateTime()
        {
            var expected =
                "<div class=\"chart\" data-chart-type=\"Line\" data-chart-xtype=\"date\" data-chart-ytype=\"int\">";
            _dateTarget.Interval = ChartIntervalType.None;
            StringAssert.StartsWith(_dateTarget.ToHtmlString(), expected);

            expected =
                "<div class=\"chart\" data-chart-interval=\"hh\" data-chart-type=\"Line\" data-chart-xtype=\"date\" data-chart-ytype=\"int\">";
            _dateTarget.Interval = ChartIntervalType.Hourly;
            StringAssert.StartsWith(_dateTarget.ToHtmlString(), expected);

            expected =
                "<div class=\"chart\" data-chart-interval=\"DD\" data-chart-type=\"Line\" data-chart-xtype=\"date\" data-chart-ytype=\"int\">";
            _dateTarget.Interval = ChartIntervalType.Daily;
            StringAssert.StartsWith(_dateTarget.ToHtmlString(), expected);

            expected =
                "<div class=\"chart\" data-chart-interval=\"MM\" data-chart-type=\"Line\" data-chart-xtype=\"date\" data-chart-ytype=\"int\">";
            _dateTarget.Interval = ChartIntervalType.Monthly;
            StringAssert.StartsWith(_dateTarget.ToHtmlString(), expected);

            expected =
                "<div class=\"chart\" data-chart-interval=\"YYYY\" data-chart-type=\"Line\" data-chart-xtype=\"date\" data-chart-ytype=\"int\">";
            _dateTarget.Interval = ChartIntervalType.Yearly;
            StringAssert.StartsWith(_dateTarget.ToHtmlString(), expected);
        }

        [TestMethod]
        public void TestToHtmlStringRendersLegendAttributeIfLegendPositionIsNotBottom()
        {
            _stringTarget.LegendPosition = ChartLegendPosition.Bottom;
            StringAssert.StartsWith(_stringTarget.ToHtmlString(), EXPECTED_RENDERING_WHEN_NOTHING_IS_SET);

            var expected =
                "<div class=\"chart\" data-chart-legend=\"none\" data-chart-type=\"Line\" data-chart-xtype=\"string\" data-chart-ytype=\"int\">";
            _stringTarget.LegendPosition = ChartLegendPosition.None;
            StringAssert.StartsWith(_stringTarget.ToHtmlString(), expected);
        }

        [TestMethod]
        public void TestToHtmlStringRendersWithPrecisionAttributeIfNumberPrecisionValueIsSet()
        {
            _stringTarget.NumberPrecision = null;
            StringAssert.StartsWith(_stringTarget.ToHtmlString(), EXPECTED_RENDERING_WHEN_NOTHING_IS_SET);

            var expected =
                "<div class=\"chart\" data-chart-precision=\"42\" data-chart-type=\"Line\" data-chart-xtype=\"string\" data-chart-ytype=\"int\">";
            _stringTarget.NumberPrecision = 42;
            StringAssert.StartsWith(_stringTarget.ToHtmlString(), expected);
        }

        #region Series Configuration Rendering

        [TestMethod]
        public void TestToHtmlStringRendersHiddenFieldForSeriesWithEmptyJSONArrayIfNoSeriesValuesExist()
        {
            var expected = "<input class=\"chart-series\" type=\"hidden\" value=\"[]\" />";
            _dateTarget = new ChartBuilder<DateTime, int>(); // Ensure we have no series.
            StringAssert.Contains(_dateTarget.ToHtmlString(), expected);
        }

        [TestMethod]
        public void TestToHtmlStringRendersExpectedSeriesConfigurationWithTitle()
        {
            var expected =
                "<input class=\"chart-series\" type=\"hidden\" value=\"[{&quot;title&quot;:&quot;Season 3&quot;,&quot;valueField&quot;:&quot;y1&quot;}]\" />";
            _dateTarget = new ChartBuilder<DateTime, int>(); // Ensure we have no series.
            var series = new ChartSeries<DateTime, int>();
            series.Name = "Season 3";
            _dateTarget.Series.Add(series);
            StringAssert.Contains(_dateTarget.ToHtmlString(), expected);
        }

        [TestMethod]
        public void TestToHtmlStringRendersSeriesConfigurationsWithValueFieldsSetToTheOrderTheyExistInTheSeriesList()
        {
            var expected =
                "<input class=\"chart-series\" type=\"hidden\" value=\"[{&quot;title&quot;:&quot;First&quot;,&quot;valueField&quot;:&quot;y1&quot;},{&quot;title&quot;:&quot;Second&quot;,&quot;valueField&quot;:&quot;y2&quot;}]\" />";
            _dateTarget = new ChartBuilder<DateTime, int>(); // Ensure we have no series.
            _dateTarget.Series.Add(new ChartSeries<DateTime, int> {
                Name = "First"
            });
            _dateTarget.Series.Add(new ChartSeries<DateTime, int> {
                Name = "Second"
            });
            StringAssert.Contains(_dateTarget.ToHtmlString(), expected);
        }

        #endregion

        #region Data Configuration Rendering

        [TestMethod]
        public void TestToHtmlStringRendersHiddenFieldForDataWithEmptyJSONArrayIfNoSeriesValuesExist()
        {
            var expected = "<input class=\"chart-data\" type=\"hidden\" value=\"[]\" />";
            _dateTarget = new ChartBuilder<DateTime, int>(); // Ensure we have no series.
            StringAssert.Contains(_dateTarget.ToHtmlString(), expected);
        }

        [TestMethod]
        public void TestToHtmlStringRendersHiddenFieldForDataWithEmptyJSONArrayIfSeriesValuesDoNotContainAnyData()
        {
            var expected = "<input class=\"chart-data\" type=\"hidden\" value=\"[]\" />";
            _dateTarget = new ChartBuilder<DateTime, int>(); // Ensure we have no series.
            _dateTarget.Series.Add(new ChartSeries<DateTime, int>());

            StringAssert.Contains(_dateTarget.ToHtmlString(), expected);
        }

        [TestMethod]
        public void TestToHtmlStringRendersFieldForDataWithSerializedDataForOneSeriesAndOneDataPoint()
        {
            // This should render with only a single data point for the x and y values.
            var expectedJson = HttpUtility.HtmlEncode("[{\"x\":\"\\/Date(1402027200000)\\/\",\"y1\":15}]");

            var expectedXKey = new DateTime(2014, 6, 6);
            var series = new ChartSeries<DateTime, int>();
            series.Add(expectedXKey, 15);

            _dateTarget.Series.Add(series);

            StringAssert.Contains(_dateTarget.ToHtmlString(), expectedJson);
        }

        [TestMethod]
        public void TestToHtmlStringRendersFieldForDataWithSerializedDataForOneSeriesAndTwoDataPoints()
        {
            // This should render with two separate data points.
            // Note that the date values are intentionally out of order.
            var expectedJson =
                HttpUtility.HtmlEncode(
                    "[{\"x\":\"\\/Date(1402027200000)\\/\",\"y1\":15},{\"x\":\"\\/Date(1401681600000)\\/\",\"y1\":10}]");

            var series = new ChartSeries<DateTime, int>();
            series.Add(new DateTime(2014, 6, 6), 15);
            series.Add(new DateTime(2014, 6, 2), 10);

            _dateTarget.Series.Add(series);

            StringAssert.Contains(_dateTarget.ToHtmlString(), expectedJson);
        }

        [TestMethod]
        public void
            TestToHtmlStringRendersFieldForDataWithSerializedDataForTwoSeriesEachWithOneDataPointForTheSameXValue()
        {
            // This should render with two separate data points.
            var expectedJson = HttpUtility.HtmlEncode("[{\"x\":\"\\/Date(1402027200000)\\/\",\"y1\":15,\"y2\":42}]");

            var series = new ChartSeries<DateTime, int>();
            series.Add(new DateTime(2014, 6, 6), 15);
            _dateTarget.Series.Add(series);

            var series2 = new ChartSeries<DateTime, int>();
            series2.Add(new DateTime(2014, 6, 6), 42);
            _dateTarget.Series.Add(series2);

            StringAssert.Contains(_dateTarget.ToHtmlString(), expectedJson);
        }

        [TestMethod]
        public void TestToHtmlStringRendersFieldForDataWithUnsortedDataIfSortTypeIsSetToNone()
        {
            var expectedJson =
                HttpUtility.HtmlEncode(
                    "[{\"x\":\"\\/Date(1402545600000)\\/\",\"y1\":42},{\"x\":\"\\/Date(1402027200000)\\/\",\"y1\":15}]");
            _dateTarget.SortType = ChartSortType.None;

            var series = new ChartSeries<DateTime, int>();
            series.Add(new DateTime(2014, 6, 12), 42);
            series.Add(new DateTime(2014, 6, 6), 15);
            _dateTarget.Series.Add(series);

            StringAssert.Contains(_dateTarget.ToHtmlString(), expectedJson);
        }

        [TestMethod]
        public void TestToHtmlStringRendersFieldForDataSortedLowToHigh()
        {
            var expectedJson =
                HttpUtility.HtmlEncode(
                    "[{\"x\":\"\\/Date(1401681600000)\\/\",\"y1\":42},{\"x\":\"\\/Date(1402027200000)\\/\",\"y1\":15}]");
            _dateTarget.SortType = ChartSortType.LowToHigh;

            var series = new ChartSeries<DateTime, int>();
            series.Add(new DateTime(2014, 6, 6), 15);
            series.Add(new DateTime(2014, 6, 2), 42);
            _dateTarget.Series.Add(series);

            StringAssert.Contains(_dateTarget.ToHtmlString(), expectedJson);
        }

        [TestMethod]
        public void TestToHtmlStringRendersFieldForDataWhenOneSeriesHasAnXValueThatAnotherSeriesDoesNot()
        {
            var expectedJson =
                HttpUtility.HtmlEncode(
                    "[{\"x\":\"\\/Date(1402027200000)\\/\",\"y1\":15},{\"x\":\"\\/Date(1401681600000)\\/\",\"y2\":42}]");

            var series = new ChartSeries<DateTime, int>();
            series.Add(new DateTime(2014, 6, 6), 15);
            _dateTarget.Series.Add(series);

            var series2 = new ChartSeries<DateTime, int>();
            series2.Add(new DateTime(2014, 6, 2), 42);
            _dateTarget.Series.Add(series2);

            StringAssert.Contains(_dateTarget.ToHtmlString(), expectedJson);
        }

        #endregion

        #endregion

        #endregion
    }
}
