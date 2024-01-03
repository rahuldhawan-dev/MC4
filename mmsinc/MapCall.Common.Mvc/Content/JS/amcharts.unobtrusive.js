(function ($) {

  // NOTE: Themes require additional js files and junk.

  // TODO: UNIT TEST ALL OF THIS
  // TODO: Default dimensions. Height needs to be explicitly set.

  var DATA_KEY = 'amcharts';

  var TYPES = {
    Line: 'serial',
    Bar: 'serial',
    SingleSeriesBar: 'serial'
  };

  var VALUE_TYPES = {
    DATE: 'date',
    INT: 'int',
    FLOAT: 'float',
    STRING: 'string'
  };

  // Needed for auto-generated element id.
  var chartCount = 0;

  var parseJson = function (jsonString) {
    // The Reviver function is to fix ASP's weird JavaScriptSerializer date serialization.
    return JSON.parse(jsonString, function (key, val) {
      if (typeof val !== 'string' || val.indexOf('/Date(') !== 0) {
        return val;
      }
      return eval('new ' + val.replace(/\//g, ''));
    });
  }

  var methods = {
    init: function ($el) {
      if ($el.attr('data-chart-initialized')) {
        return;
      }

      var config = methods.initConfig($el);

      // Need to randomly generate an id because it's the only
      // thing AmCharts works with.
      if (!$el.attr('id')) {
        $el.attr('id', 'chart-' + chartCount);
        chartCount = chartCount + 1;
      }

      // May need to look into disposing this when the chart is refreshed
      // through Ajax Forms.
      var chart = AmCharts.makeChart($el.attr('id'), config);
      $el.data(DATA_KEY, {
        chart: chart,
        config: config
      });
      $el.attr('data-chart-initialized', true);
    },

    initConfig: function ($el) {
      var getAttr = function (key) {
        return $el.attr('data-chart-' + key);
      }

      var options = {
        data: parseJson($el.find('.chart-data').val() || '[]'),
        // L = am/pm 1-12 hours
        dateFormat: 'YYYY-MM-DD L:NN A',
        interval: getAttr('interval'),
        legend: getAttr('legend'),
        precision: getAttr('precision'),
        series: JSON.parse($el.find('.chart-series').val() || '[]'),
        subType: getAttr('type'),
        title: getAttr('title'),
        type: TYPES[getAttr('type')],
        xType: getAttr('xtype'),
        yAxisLabel: getAttr('yaxislabel'),
        yMinVal: getAttr('yminval'),
        yMaxVal: getAttr('ymaxval'),
        yType: getAttr('ytype'),

        fontColor: $el.css('color'),
        fontFamily: $el.css('fontFamily')
      };

      var config = {
        theme: 'light',
        fontSize: 10,
        color: options.fontColor,
        fontFamily: options.fontFamily,
        precision: (options.precision || -1), // -1 displays all decimal places
        type: options.type,
        pathToImages: '//cdn.amcharts.com/lib/3/images/',
        categoryField: 'x',
        dataDateFormat: options.dateFormat,
        balloonDateFormat: options.dateFormat,
        balloon: {
          // Disable animations
          animationDuration: 0,
          fadeOutDuration: 0,

          // fixedPosition causes weird flickering when the mouse is over the point in just the right spot.
          fixedPosition: false
        },
        // sequenceAnimation: false, // The animations are really slow and pretty useless.
        // startDuration: 0,
        dataProvider: options.data
      };

      if (options.title) {
        config.titles = [{ size: 15, text: options.title }];
      }

      methods.initLegend(config, options);
      methods.initCategoryAxis(config, options);
      methods.initValueAxis(config, options);
      methods.initGraphs(config, options);

      if (options.subType === 'Line') {
        // This is the line and popup bubble that follows
        // your mouse regardless of where it is in the chart. 
        config.chartCursor = {
          categoryBalloonDateFormat: options.dateFormat,
          animationDuration: 0 // This animation is incredibly slow in IE8 when there are a lot of data points.
        };
      }
      else if (options.subType === 'SingleSeriesBar') {
        methods.colorizeBarChartData(config, options);
      }

      return config;
    },

    initLegend: function (config, options) {
      // This object just needs to exist in order for
      // the legend to display. AmCharts will fill in
      // any default values that are needed.
      if (options.legend !== 'none') {
        config.legend = {
          color: options.fontColor,
          position: options.legend || 'bottom'
        }
      }
    },
    initCategoryAxis: function (config, options) {
      config.categoryAxis = {
        // False tells the chart to include interval values that may not have any data.
        equalSpacing: false,

        gridPosition: 'start',
        minorGridEnabled: true,

        // Tells AmCharts to parse Date objects. Which seems backwards since you'd
        // think it means string representations of dates.
        parseDates: (options.xType === VALUE_TYPES.DATE),

        // makes the labels rotate instead of being horizontally flat.
        // NOTE: There's an issue where the last label won't appear if AmCharts
        // decides that the text would be cut off too much. This will happen if the
        // angles are less than 90 degrees.
        // NOTE 2: This is apparently ignored entirely when the category axis is date.
        autoRotateAngle: -80,
        autoRotateCount: 2
      };

      if (options.xType === VALUE_TYPES.DATE && options.interval) {
        // minPeriod/Interval can only work for date objects in AmCharts.
        // minPeriod can't be set to null/undefined, AmCharts blows up. But
        // if the property never even exists it works fine.
        config.categoryAxis.minPeriod = options.interval;
      }

      if (options.xType !== VALUE_TYPES.DATE) {
        // Prevents labels from disappearing when there isn't enough room to display
        // all of them clearly. 
        config.categoryAxis.minHorizontalGap = 0;
      }
    },

    initValueAxis: function (config, options) {
      var yAxis = {};

      if (options.yAxisLabel) {
        yAxis.title = options.yAxisLabel;
      }

      if (options.yMinVal) {
        yAxis.minimum = options.yMinVal;
      }
      if (options.yMaxVal) {
        yAxis.maximum = options.yMaxVal;
      }

      // Setting this to true will stop the Y axis from having decimal
      // values.
      yAxis.integersOnly = (options.yType === VALUE_TYPES.INT);
      config.valueAxes = [yAxis];
    },

    initGraphs: function (config, options) {
      config.graphs = [];

      var isBarChart = (options.subType === 'Bar' || options.subType === 'SingleSeriesBar');

      for (var i = 0; i < options.series.length; i++) {
        var cur = options.series[i];
        var g = {
          // Keep the title out since it will always display the same text.
          // Also keep the category out for line charts, the chartCursor will display it.
          balloonText: (options.subType === 'SingleSeriesBar' ? '[[category]] - [[value]]' : '[[title]] - [[value]]'),
          colorField: 'color',
          lineColorField: 'color',
          title: cur.title,
          valueField: cur.valueField
        };

        if (options.subType === 'Line') {
          g.bullet = 'round';
        }
        else if (isBarChart) {
          g.type = 'column';
          g.fillAlphas = 1; // Tells the value bar to fill with color instead of just being outlined.
        }
        config.graphs.push(g);
      }
    },

    colorizeBarChartData: function (config, options) {
      // AmChart themes don't work when you want a single
      // series bar chart where each value has a different color.
      var colorEnumerator = (function () {
        var i = 0;
        var colors = AmCharts.themes.colors;

        return function () {
          if (i >= colors.length) {
            i = 0;
          }
          var c = colors[i];
          i = i + 1;
          return c;
        };
      })();

      for (var cur = 0; cur < config.dataProvider.length; cur++) {
        config.dataProvider[cur].color = colorEnumerator();
      }
    },

    refresh: function ($el) {
      // AmChart.invalidateSize() forces the chart to
      // refresh based on its container's size.
      $el.data(DATA_KEY).chart.invalidateSize();
    }
  };

  $.fn.unobtrusiveChart = function (options) {
    $(this).each(function () {
      var $el = $(this);
      if (!options) {
        methods.init($el);
      }
      else if (options === 'refresh') {
        methods.refresh($el);
      }
    });

    return this;
  };

  $('.chart').unobtrusiveChart();

  // This forces the charts in a tab to resize correctly everytime
  // a tab is opened. Otherwise the chart won't display or
  // it might be the wrong size entirely.
  //
  // We need to add this event listener *after* the tab remembering
  // script runs. Otherwise we end up trying to refresh on a chart
  // that isn't fully initialized and some error about renderFix
  // being null gets thrown by the AmCharts lib.
  $(document).ready(function () {
    $(document).on('tabsactivate', function (event, ui) {
      if (ui.newPanel) {
        ui.newPanel.find('.chart').unobtrusiveChart('refresh');
      }
    });
  });

})(jQuery);