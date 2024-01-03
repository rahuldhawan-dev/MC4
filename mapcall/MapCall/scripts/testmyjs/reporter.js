/* STRING BUILDER: */

function StringBuilder() {
  this.ar = [];

  return true;
}

// TODO: heart was in the right place.
// these are ass-backwards, the static methods
// should and instantiate and use a new StringBuilder,
// instead of having to pass in this.  it'll amount
// to less code that way.
StringBuilder.objectToHtmlAttributes = function(obj, sb) {
  var existing = true;

  if (!sb) {
    sb = new StringBuilder();
    existing = false;
  }

  for (var x in obj) {
    sb.append(' ' + x + '="' + obj[x].toString() + '"');
  }

  if (!existing) {
    return sb.toString();
  }
};

StringBuilder.makeOpenTag = function(tag, attr, sb) {
  var existing = true;

  if (!sb) {
    sb = new StringBuilder();
    existing = false;
  }

  sb.append('<' + tag);
  sb.appendHTMLAttributes(attr);
  sb.append('>');

  if (!existing) {
    return sb.toString();
  }
};

StringBuilder.makeCloseTag = function(tag, sb) {
  var existing = true;

  if (!sb) {
    sb = new StringBuilder();
    existing = false;
  }

  sb.append('</' + tag + '>');

  if (!existing) {
    return sb.toString();
  }
};

StringBuilder.makeElement = function(tag, content, attr, sb) {
  var existing = true;

  if (!sb) {
    sb = new StringBuilder();
    existing = false;
  }

  sb.append('<' + tag);
  sb.appendHTMLAttributes(attr);
  sb.append('>' + content + '</' + tag + '>');

  if (!existing) {
    return sb.toString();
  }
};

StringBuilder.makeSpan = function(content, attr, sb) {
  var existing = true;

  if (!sb) {
    sb = new StringBuilder();
    existing = false;
  }

  sb.appendElement('span', content, attr);

  if (!existing) {
    return sb.toString();
  }
};

StringBuilder.makeTableCell = function(content, attr, sb) {
  var existing = true;

  if (!sb) {
    sb = new StringBuilder();
    existing = false;
  }

  sb.appendElement('td', content, attr);

  if (!existing) {
    return sb.toString();
  }
};

StringBuilder.makeHeaderCell = function(content, attr, sb) {
  var existing = true;

  if (!sb) {
    sb = new StringBuilder();
    existing = false;
  }

  sb.appendElement('th', content, attr);

  if (!existing) {
    return sb.toString();
  }
};

StringBuilder.appendBreak = function(str) {
  return str + '<br />';
};

StringBuilder.prototype.append = function(obj) {
  this.ar[this.ar.length] = obj.toString();
};

StringBuilder.prototype.appendOpenTag = function(tag, attr) {
  StringBuilder.makeOpenTag(tag, attr, this);
};

StringBuilder.prototype.appendCloseTag = function(tag) {
  StringBuilder.makeCloseTag(tag, this);
};

StringBuilder.prototype.appendElement = function(tag, content, attr) {
  StringBuilder.makeElement(tag, content, attr, this);
};

StringBuilder.prototype.appendSpan = function(content, attr) {
  StringBuilder.makeSpan(content, attr, this);
};

StringBuilder.prototype.appendTableCell = function(content, attr) {
  StringBuilder.makeTableCell(content, attr, this);
};

StringBuilder.prototype.appendHeaderCell = function(content, attr) {
  StringBuilder.makeHeaderCell(content, attr, this);
};

StringBuilder.prototype.appendWithBreak = function(str) {
  this.append(StringBuilder.appendBreak(str));
};

StringBuilder.prototype.appendHTMLAttributes = function(attr) {
  StringBuilder.objectToHtmlAttributes(attr, this);
};

StringBuilder.prototype.toString = function(delimiter) {
  return this.ar.join(delimiter || '');
};

/* TEST REPORT: */

var TestReport = {
  toggleRows: function(rowAr) {
    if (this.anyRowIsVisible(rowAr)) {
      this.hideAllRows(rowAr);
    } else {
      this.showAllRows(rowAr);
    }
  },

  toggleAllResultsForClass: function(testClass) {
    var rowAr = document.getElementsByName(testClass);
    this.toggleRows(rowAr);
  },

  toggleByTestAndCssClass: function(testClass, cssClass) {
    var rowAr = document.getElementsByName(testClass);
    rowAr = this.filterRowsByCssClass(rowAr, cssClass);
    this.toggleRows(rowAr);
  },

  rowIsVisible: function(row) {
    return row.style.display != 'none';
  },

  rowIsInCssClass: function(row, cssClass) {
    return row.className.indexOf(cssClass) >= 0;
  },

  anyRowIsVisible: function(rowAr) {
    for (var i = rowAr.length - 1; i >= 0; --i) {
      if (this.rowIsVisible(rowAr[i])) {
        return true;
      }
    }
    return false;
  },

  showRow: function(row) {
    row.style.display = '';
  },

  showAllRows: function(rowAr) {
    for (var i = rowAr.length - 1; i >= 0; --i) {
      this.showRow(rowAr[i]);
    }
  },

  hideRow: function(row) {
    row.style.display = 'none';
  },

  hideAllRows: function(rowAr) {
    for (var i = rowAr.length - 1; i >= 0; --i) {
      this.hideRow(rowAr[i]);
    }
  },

  filterRowsByCssClass: function(rowAr, cssClass) {
    var retAr = [];
    var row;
    for (var i = rowAr.length - 1; i >= 0; --i) {
      row = rowAr[i];
      if (this.rowIsInCssClass(row, cssClass)) {
        retAr[retAr.length] = row;
      }
    }
    return retAr;
  },

  classNames: {
    resultHeader: 'result-header',
    classResult: 'class-result',
    methodResult: 'method-result',
    successfulResult: 'successful-result',
    failedResult: 'failed-result',
    exceptionResult: 'exception-result'
  }
};

/* TEST REPORTER: */

function TestReporter(result) {
  this.result = result;

  return true;
}

TestReporter.prototype.buildResultTable = function(div) {
  var sb = new StringBuilder();
  sb.appendOpenTag('table', {border: 1});
  sb.append(TestReporter.buildHeader(this.result));
  sb.append(TestReporter.buildResult(this.result));
  sb.append(TestReporter.buildFooter(this.result));
  sb.appendCloseTag('table');
  div.innerHTML = sb.toString();
};

TestReporter.buildHeader = function(testResult) {
  var sb = new StringBuilder();
  sb.appendOpenTag('tr');
  sb.appendHeaderCell('Results For: ' + testResult.testName,
    {'class': TestReport.classNames.resultHeader, 'colspan': 5});
  sb.appendCloseTag('tr');
  sb.appendOpenTag('tr');
  sb.appendHeaderCell('Test Class');
  sb.appendHeaderCell('Tests');
  sb.appendHeaderCell('Successful');
  sb.appendHeaderCell('Failed');
  sb.appendHeaderCell('Exceptions');
  sb.appendCloseTag('tr');
  return sb.toString();
};

TestReporter.buildResult = function(result) {
  var sb = new StringBuilder();
  for (var i = 0, len = result.getLength(); i < len; ++i) {
    sb.append(TestReporter.buildClassResult(result.getClassResult(i)));
  }
  return sb.toString();
};

TestReporter.buildClassResult = function(result) {
  var sb = new StringBuilder();
  sb.appendOpenTag('tr', {'class': TestReport.classNames.classResult});
  sb.appendTableCell(result.className,
    {'onclick': 'TestReport.toggleAllResultsForClass(\'' + result.className + '\')'});
  sb.appendTableCell(result.getLength(),
    {'onclick': 'TestReport.toggleAllResultsForClass(\'' + result.className + '\')'});
  sb.appendTableCell(result.successes,
    {'onclick': 'TestReport.toggleByTestAndCssClass(\'' + result.className + '\', ' +
      '\'' + TestReport.classNames.successfulResult + '\')'});
  sb.appendTableCell(result.failures,
    {'onclick': 'TestReport.toggleByTestAndCssClass(\'' + result.className + '\', ' +
      '\'' + TestReport.classNames.failedResult + '\')'});
  sb.appendTableCell(result.exceptions,
    {'onclick': 'TestReport.toggleByTestAndCssClass(\'' + result.className + '\', ' +
      '\'' + TestReport.classNames.exceptionResult + '\')'});
  sb.appendCloseTag('tr');

  for (var i = 0, len = result.getLength(); i < len; ++i) {
    sb.append(
      TestReporter.buildMethodResult(
        result.getMethodResult(i), result.className));
  }

  return sb.toString();
};

TestReporter.buildMethodResult = function(result, className) {
  var message = '',
      cls = TestReport.classNames.methodResult,
      rowspan = 0;

  // i will keep doing this as long as javascript lets me. :P
  switch (true) {
    case result.failed:
      message = TestReporter.buildFailureMessage(result);
      cls += ' ' + TestReport.classNames.failedResult;
      rowspan = 4;
      break;
    case result.exception:
      message = TestReporter.buildExceptionMessage(result);
      cls += ' ' + TestReport.classNames.exceptionResult;
      rowspan = 2;
      break;
    default:
      message = TestReporter.buildSuccessMessage(result);
      cls += ' ' + TestReport.classNames.successfulResult;
      break;
  }
  return TestReporter.buildMethodResultMessageRow(className, message, cls, rowspan);
};

TestReporter.buildFooter = function(result) {
  var sb = new StringBuilder();
  sb.appendOpenTag('tr');
  sb.appendHeaderCell('Totals:');
  sb.appendTableCell(result.tests);
  sb.appendTableCell(result.successes);
  sb.appendTableCell(result.failures);
  sb.appendTableCell(result.exceptions);
  sb.appendCloseTag('tr');
  return sb.toString();
};

TestReporter.buildFailureMessage = function(result) {
  var sb = new StringBuilder();
  sb.appendSpan('Failure: &nbsp;&nbsp;',
    {'class': TestReport.classNames.methodResult});
  sb.appendWithBreak(result.methodName);
  sb.appendWithBreak('Expected: &nbsp;' + result.expected.toString());
  sb.appendWithBreak('Actual: &nbsp;&nbsp;&nbsp;' + result.actual.toString());
  sb.appendSpan(result.message, {'class': 'message'});
  return sb.toString();
};

TestReporter.buildExceptionMessage = function(result) {
  var sb = new StringBuilder();
  sb.appendSpan('Exception: ',
    {'class': TestReport.classNames.methodResult});
  sb.appendWithBreak(result.methodName);
  sb.appendSpan(result.message, {'class': 'message'});
  return sb.toString();
};

TestReporter.buildSuccessMessage = function(result) {
  return StringBuilder.makeSpan('Success: &nbsp;&nbsp;',
    {'class': TestReport.classNames.methodResult}) +
      result.methodName;
};

TestReporter.buildMethodResultMessageRow = function(className, message, cls, rowspan) {
  var rowAttr = {
    'class': cls,
    'style': 'display: none;',
    'name': className
  };
  var cellAttr = {'colspan': 5};
  if (rowspan) {
    rowAttr.rowspan = rowspan;
  }
  var sb = new StringBuilder();
  sb.appendElement('tr', StringBuilder.makeTableCell(message, cellAttr), rowAttr);
  return sb.toString();
};
