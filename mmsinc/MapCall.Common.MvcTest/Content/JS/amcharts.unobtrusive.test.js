/// <reference path="../../../MMSINC.Testing/Scripts/qunit.js" />
/// <reference path="../../../MMSINC.Testing/Scripts/qmock.js" />
/// <reference path="../../../MMSINC.Testing/Scripts/testHelpers.js" />
/// <reference path="../../../MapCall.Common.Mvc/Content/JS/jquery-1.10.2.js" />
/// <reference path="../../../MapCall.Common.Mvc/Content/JS/amcharts.js" />
/// <reference path="../../../MapCall.Common.Mvc/Content/JS/amcharts.serial.js" />
/// <reference path="../../../MapCall.Common.Mvc/Content/JS/amcharts.themes.light.js" />
/// <reference path="../../../MapCall.Common.Mvc/Content/JS/amcharts.unobtrusive.js" />

module('amcharts.unobtrusive.js');

preserve([],
function () {

  var chartHolder = $('<div id="chartHolder"></div>');
  var config = null;

  var run = function (chartSetup, testFunc) {
    // Resharper test runner obliterates everything in the
    // body tag, so we need to append it when the tests actually start.
    if (chartHolder.parents().length === 0) {
      $('body').append(chartHolder);
    }

    config = {
      chartEl: $('<div id="chart"></div>')
    };

    if (chartSetup.hasOwnProperty('chartId')) {
      config.chartEl.attr('id', chartSetup.chartId);
    }

    for (var key in chartSetup) {
      if (key === 'data') {
        config.chartEl.append($('<input type="hidden" class="chart-data" />').val(JSON.stringify(chartSetup[key])));
      }
      else if (key == 'series') {
        config.chartEl.append($('<input type="hidden" class="chart-series" />').val(JSON.stringify(chartSetup[key])));
      }
      else {
        config.chartEl.attr('data-chart-' + key, chartSetup[key]);
      }
    }

    config.chart = config.chartEl.unobtrusiveChart().data('amcharts').config;

    mock(function () {
      testFunc();
    });

    // Clear any charts that have been appended.
    chartHolder.empty();
  };


  test('[ALL] config.balloon.fixedPosition must be false', function () {
    run({ type: 'Line' }, function () {
      equal(config.chart.balloon.fixedPosition, false, 'Setting this to true makes for strange flickering of the balloon');
    });
  });

  test('[ALL] balloon animation should be disabled because they kill the already slow performance in IE8 and also they look bad', function () {
    run({ type: 'Line' }, function () {
      equal(config.chart.balloon.animationDuration, 0);
      equal(config.chart.balloon.fadeOutDuration, 0);
    });
  });

  test('[ALL] Configuration should have a default YYYY-MM-DD L:NN A as the default date formats', function () {
    run({ type: 'Line' }, function () {
      equal(config.chart.balloonDateFormat, 'YYYY-MM-DD L:NN A');
      equal(config.chart.dataDateFormat, 'YYYY-MM-DD L:NN A');
    });
  });

  test('[ALL] categoryField must equal "x"', function () {
    run({ type: 'Line' }, function () {
      equal(config.chart.categoryField, 'x');
    });
  });

  test('[ALL] config.legend sets position to bottom if legend attr value is not set', function () {
    run({ type: 'Line' }, function () {
      // Need to read this because different browsers have different values(IE)
      var color = config.chartEl.css('color');
      propEqual(config.chart.legend, { color: color, position: 'bottom' });
    });
  });

  test('[ALL] config.legend is not set if legend attr is set to none', function () {
    run({ type: 'Line', legend: 'none' }, function () {
      equal(config.chart.hasOwnProperty('legend'), false);
    });
  });

  test('[ALL] config.legend has position value set', function () {
    run({ type: 'Line', legend: 'right' }, function () {
      // Need to read this because different browsers have different values(IE)
      var color = config.chartEl.css('color');
      propEqual(config.chart.legend, { color: color, position: 'right' });
    });
  });

  test('[ALL] config.titles is set if title attr has value', function () {
    run({ type: 'Line', title: 'I am a title' }, function () {
      equal(config.chart.titles[0].text, 'I am a title');
    });
    run({ type: 'Line' }, function () {
      equal(config.chart.titles, null, 'No title object should be created.');
    });
  });

  test('[ALL] config.dateProvider is set to data deserialized from data attr', function () {
    var data = [{ x: '1', y1: '2' }];
    run({ type: 'Line', data: data }, function () {
      propEqual(config.chart.dataProvider, data);
    });
  });

  test('[LINE] config.type must equal "serial" when type attr is "Line"', function () {
    run({ type: 'Line' }, function () {
      equal(config.chart.type, 'serial');
    });
  });

  test('[BAR] config.type must equal "serial" when type attr is "Bar"', function () {
    run({ type: 'Bar' }, function () {
      equal(config.chart.type, 'serial');
    });
  });

  test('[SINGLESERIESBAR] config.type must equal "serial" when type attr is "Bar"', function () {
    run({ type: 'SingleSeriesBar' }, function () {
      equal(config.chart.type, 'serial');
    });
  });

  test('[ALL] valueAxis.title is set to yaxislabel attr', function () {
    run({ type: 'Bar', yaxislabel: 'some axis title' }, function () {
      equal(config.chart.valueAxes[0].title, 'some axis title');
    });
    run({ type: 'Bar' }, function () {
      equal(config.chart.valueAxes[0].hasOwnProperty('title'), false, 'The property needs to not exist.');
    });
  });

  test('[ALL] valueAxis.minimum is set to yminval attr', function () {
    run({ type: 'Bar', yminval: 15 }, function () {
      equal(config.chart.valueAxes[0].minimum, 15);
    });
    run({ type: 'Bar' }, function () {
      equal(config.chart.valueAxes[0].hasOwnProperty('minimum'), false, 'The property needs to not exist. Setting it to null or undefined messes up the chart.');
    });
  });

  test('[ALL] valueAxis.maximum is set to ymaxval attr', function () {
    run({ type: 'Bar', ymaxval: 15 }, function () {
      equal(config.chart.valueAxes[0].maximum, 15);
    });
    run({ type: 'Bar' }, function () {
      equal(config.chart.valueAxes[0].hasOwnProperty('maximum'), false, 'The property needs to not exist. Setting it to null or undefined messes up the chart.');
    });
  });

  test('[ALL - INT] valueAxis.integersOnly to true if the ytype is int', function () {
    run({ type: 'Bar', ytype: 'int' }, function () {
      equal(config.chart.valueAxes[0].integersOnly, true);
    });
    run({ type: 'Bar', ytype: 'notint' }, function () {
      equal(config.chart.valueAxes[0].integersOnly, false);
    });
  });

  test('[ALL - DATE] Configuration sets categoryAxis.parseDates to true if xtype is date', function () {
    run({ type: 'Bar', xtype: 'date' }, function () {
      equal(config.chart.categoryAxis.parseDates, true);
    });

    run({ type: 'Bar' }, function () {
      equal(config.chart.categoryAxis.parseDates, false);
    });
  });

  test('[ALL - DATE] Configuration sets categoryAxis.minPeriod value from interval attr if xtype is date', function () {
    run({ type: 'Bar', xtype: 'date', interval: 'HUEHUE' }, function () {
      equal(config.chart.categoryAxis.minPeriod, 'HUEHUE');
    });

    run({ type: 'Bar' }, function () {
      equal(config.chart.categoryAxis.minPeriod, null);
    });
  });

  test('[ALL] Configuration sets categoryAxis.autoRotateAngle to -80 by default', function() {
    run({ type: 'Bar' }, function() {
      equal(config.chart.categoryAxis.autoRotateAngle, -80);
    });
  });

  test('[ALL] Configuration sets categoryAxis.autoRotateCount to 2 by default', function() {
    run({ type: 'Bar' }, function() {
      equal(config.chart.categoryAxis.autoRotateCount, 2);
    });
  });

  test('[ALL] Configuration sets categoryAxis.minHorizontalGap to 0 by default', function() {
    run({ type: 'Bar' }, function() {
      equal(config.chart.categoryAxis.minHorizontalGap, 0);
    });
  });

  test('[DATE] Configuration does not set categoryAxis.minHorizontalGap', function() {
    run({ type: 'Bar', xtype: 'date' }, function() {
      equal(config.chart.categoryAxis.minHorizontalGap, null);
    });
  });

  test('[ALL] config.graphs contains graph for every series value', function () {
    var series = [{ title: 'title 1', valueField: 'y1' }, { title: 'title 2', valueField: 'y2' }];
    run({ type: 'Bar', series: series }, function () {
      equal(config.chart.graphs[0].title, 'title 1');
      equal(config.chart.graphs[0].valueField, 'y1');
      equal(config.chart.graphs[0].colorField, 'color');
      equal(config.chart.graphs[0].lineColorField, 'color');
      equal(config.chart.graphs[0].balloonText, '[[title]] - [[value]]');

      equal(config.chart.graphs[1].title, 'title 2');
      equal(config.chart.graphs[1].valueField, 'y2');
      equal(config.chart.graphs[1].colorField, 'color');
      equal(config.chart.graphs[1].lineColorField, 'color');
      equal(config.chart.graphs[1].balloonText, '[[title]] - [[value]]');
    });
  });


  test('[SINGLESERIESBAR] config.graphs.balloonText does not include title', function () {
    var series = [{ title: 'title 1', valueField: 'y1' }];
    run({ type: 'SingleSeriesBar', series: series }, function () {
      equal(config.chart.graphs[0].title, 'title 1');
      equal(config.chart.graphs[0].valueField, 'y1');
      equal(config.chart.graphs[0].colorField, 'color');
      equal(config.chart.graphs[0].lineColorField, 'color');
      equal(config.chart.graphs[0].balloonText, '[[category]] - [[value]]');
    });
  });

  test('[LINE] config.graphs have round bullet', function () {
    var series = [{ title: 'title 1', valueField: 'y1' }];
    run({ type: 'Line', series: series }, function () {
      equal(config.chart.graphs[0].bullet, 'round');
    });
  });

  test('[BAR] config.graphs have no bullet', function () {
    var series = [{ title: 'title 1', valueField: 'y1' }];
    run({ type: 'Bar', series: series }, function () {
      equal(config.chart.graphs[0].hasOwnProperty('bullet'), false);
    });
  });

  test('[SINGLESERIESBAR] config.graphs have no bullet', function () {
    var series = [{ title: 'title 1', valueField: 'y1' }];
    run({ type: 'Bar', series: series }, function () {
      equal(config.chart.graphs[0].hasOwnProperty('bullet'), false);
    });
  });

  test('[LINE] config.graphs do not have type set', function () {
    var series = [{ title: 'title 1', valueField: 'y1' }];
    run({ type: 'Line', series: series }, function () {
      equal(config.chart.graphs[0].hasOwnProperty('type'), false);
    });
  });

  test('[BAR] config.graphs have type set to column', function () {
    var series = [{ title: 'title 1', valueField: 'y1' }];
    run({ type: 'Bar', series: series }, function () {
      equal(config.chart.graphs[0].type, 'column');
    });
  });

  test('[SINGLESERIESBAR] config.graphs have type set to column', function () {
    var series = [{ title: 'title 1', valueField: 'y1' }];
    run({ type: 'SingleSeriesBar', series: series }, function () {
      equal(config.chart.graphs[0].type, 'column');
    });
  });

  test('[LINE] config.graphs do not have fillAlphas set', function () {
    var series = [{ title: 'title 1', valueField: 'y1' }];
    run({ type: 'Line', series: series }, function () {
      equal(config.chart.graphs[0].hasOwnProperty('fillAlphas'), false);
    });
  });

  test('[BAR] config.graphs have fillAlphas set to 1', function () {
    var series = [{ title: 'title 1', valueField: 'y1' }];
    run({ type: 'Bar', series: series }, function () {
      equal(config.chart.graphs[0].fillAlphas, 1);
    });
  });

  test('[SINGLESERIESBAR] config.graphs have fillAlphas set to 1', function () {
    var series = [{ title: 'title 1', valueField: 'y1' }];
    run({ type: 'SingleSeriesBar', series: series }, function () {
      equal(config.chart.graphs[0].fillAlphas, 1);
    });
  });

  test('[ALL] element id is set to automatic value if an id is not already set', function () {
    run({ type: 'Line', chartId: null }, function () {
      equal(config.chartEl.attr('id').indexOf('chart-'), 0);
    });
  });

  test('[LINE] config.chartCursor must disable animation if type attr is "Line"', function () {
    run({ type: 'Line' }, function () {
      equal(config.chart.chartCursor.animationDuration, 0);
    });
  });

  test('[LINE] config.chartCursor has categoryBalloonDateFormat set if type attr is "Line"', function () {
    run({ type: 'Line' }, function () {
      equal(config.chart.chartCursor.categoryBalloonDateFormat, 'YYYY-MM-DD L:NN A');
    });
  });

  test('[ALL] config.precision value is set if precision attribute has value', function () {
    run({ type: 'Line' }, function () {
      equal(-1, config.chart.precision);
    });
    run({ type: 'Line', precision: 42 }, function () {
      equal(42, config.chart.precision);
    });
  });
}
);