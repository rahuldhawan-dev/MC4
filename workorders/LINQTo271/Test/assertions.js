function AssertFailedException(message, expected, actual) {
  /// <summary>Base exception type which will cause a test being run in the
  /// testMyJs framework to fail.</summary>
  /// <param name="message">String to use as the failure message.</param>
  /// <param name="expected">Optional object value used as the control
  /// for a test that does some sort of comparison.</param>
  /// <param name="actual">Optional object value used as the target
  /// for a text, which may have been expected to match the value
  /// of the expected object.</param>
  this.message = message;
  this.expected = expected || 'undefined';
  this.actual = actual || 'undefined';

  return true;
}

AssertFailedException.prototype.toString = function() {
  return this.message.toString();
};

var Assert = {
  fail: function(message) {
    /// <summary>Causes a test to immediately fail, using either the
    /// optional message argument, or the default message.</summary>
    /// <param name="message">Optional string to use as the failure
    /// message.</param>

    this._doFail(message || this._defaultMessages.fail);
  },

  notImplemented: function() {
    /// <summary>Causes a test to immediately fail, stating that the
    /// given test or condition is not yet implemented.</summary>

    this._doFail(this._defaultMessages.notImplemented);
  },

  isNull: function(val, message) {
    /// <summary>Causes a test to fail if the given value does not
    /// evaluate to null.</summary>
    /// <param name="val">Reference which is expected to be null.</param>
    /// <param name="message">Optional string to use as the failure
    /// message.</param>

    if (val !== null) {
      this._doFail(message || this._defaultMessages.isNull,
              null, val);
    }
  },

  isNotNull: function(val, message) {
    /// <summary>Causes a test to fail if the given value
    /// evaluates to null.</summary>
    /// <param name="val">Reference which is expected to have a value.</param>
    /// <param name="message">Optional string to use as the failure
    /// message.</param>

    if (val === null) {
      this._doFail(message || this._defaultMessages.isNotNull,
              null, val);
    }
  },

  isTrue: function(val, message) {
    /// <summary>Causes a test to fail if the given value
    /// does not evaluate to true.</summary>
    /// <param name="val">Value which is expected to be true.</param>
    /// <param name="message">Optional string to use as the failure
    /// message.</param>

    if (val !== true) {
      this._doFail(message || this._defaultMessages.isTrue,
              true, val);
    }
  },

  isFalse: function(val, message) {
    /// <summary>Causes a test to fail if the given value
    /// does not evaluate to false.</summary>
    /// <param name="val">Value which is expected to be false</param>
    /// <param name="message">Optional string to use as the failure
    /// message.</param>

    if (val !== false) {
      this._doFail(message || this._defaultMessages.isFalse,
              false, val);
    }
  },

  areEqual: function(expected, actual, message) {
    /// <summary>Causes a test to fail if the actual value
    /// does not match the expected value.</summary>
    /// <param name="expected">Value to be used as the control
    /// for the test, which the actual value is expected to match.</param>
    /// <param name="actual">Actual value to be tested for equality
    /// against the expected value.</param>
    /// <param name="message">Optional string to use as the failure
    /// message.</param>

    if (expected != actual) {
      this._doFail(message || this._defaultMessages.areEqual,
              expected, actual);
    }
  },

  areNotEqual: function(notExpected, actual, message) {
    /// <summary>Causes a test to fail if the actual value
    /// matches the unexpected value.</summary>
    /// <param name="notExpected">Value to be used as the control
    /// for the test, which the actual value is expected to not match.</param>
    /// <param name="actual">Actual value to be tested for equality
    /// against the expected value.</param>
    /// <param name="message">Optional string to use as the failure
    /// message.</param>

    if (notExpected == actual) {
      this._doFail(message || this._defaultMessages.areNotEqual,
                   '(anything but) ' + notExpected, actual);
    }
  },

  areSame: function(expected, actual, message) {
    /// <summary>Causes a test to fail if the actual reference
    /// does not match the expected reference.</summary>
    /// <param name="expected">Reference to be used as the control
    /// for the test, which the actual reference is expected to
    /// match.</param>
    /// <param name="actual">Actual reference to be tested for
    /// equality against the expected reference.</param>
    /// <param name="message">Optional string to use as the failure
    /// message.</param>

    if (expected === actual) {
      return;
    }
    this._doFail(message || this._defaultMessages.areSame,
            expected, actual);
  },

  throwsException: function(fn, message) {
    /// <summary>Causes a test to fail if the function reference
    /// does not throw an exception when executed.</summary>
    /// <param name="fn">Function reference to be executed,
    /// which is expected to throw an exception.</param>
    /// <param name="message">Optional string to use as the failure
    /// message.</param>

    var thrown = false;
    try {
      fn();
    } catch(e) {
      thrown = true;
    }

    if (!thrown) {
      this._doFail(mesage || this._defaultMessages.throwsException);
    }
  },

  doesNotThrow: function(fn, message) {
    var thrown = false;
    try {
      fn();
    } catch(e) {
      thrown = true;
    }

    if (thrown) {
      this._doFail(message || this._defaultMessages.doesNotThrow);
    }
  },

  _doFail: function(message, expected, actual) {
    throw new AssertFailedException(message, expected, actual);
  },

  _defaultMessages: {
    fail: 'Assertion failed.',
    notImplemented: 'Test is not yet implemented.',
    isNull: 'Value was not null when expected.',
    isNotNull: 'Value evaluated to null when not expected.',
    isTrue: 'Value did not evaluate to true.',
    isFalse: 'Value did not evaluate to false.',
    areEqual: 'Actual value did not match expected value.',
    areNotEqual: 'Actual value matched unexpected value.',
    areSame: 'Arguments expected and actual were not found to be the same reference.',
    throwsException: 'Exception was not thrown when expected.',
    doesNotThrow: 'Exception thrown when not expected.'
  }
};
