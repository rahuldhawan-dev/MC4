(function(that) {
  // hidden functions

  var each = function(arr, fn) {
    for (var i = 0, len = arr.length; i < len; ++i) {
      fn(arr[i]);
    }
  };

var createExpectation = function (name, target, memberName, isMockedObject) {
    checkForMockingContext(true);
    var expectation = new Expectation(name, target, memberName, isMockedObject);
    mock.expectations.push(expectation);
    return expectation;
  };

  var verifyExpectations = function() {
    each(mock.expectations, function(ex) { ex.verifyCallCount(); });
  };

  var checkForMockingContext = function(shouldBe) {
    if (shouldBe != mock.inMockingContext) {
      throw shouldBe ? 'Not in mocking context' : 'Already in mocking context.'
    }
  }

  var clearMockedObjects = function() {
    mock.mockedObjects = {};
  };

  var clearExpectations = function() {
    // needs to happen in reverse!
    for (var i = mock.expectations.length - 1; i >= 0; --i) {
      Expectation.clear(mock.expectations[i]);
    }
    mock.expectations = [];
  };

  var dereferenceR = function(parts, scope) {
    var name = parts.shift();
    var ref = scope[name] || (scope[name] = {});
    return parts.length == 0 ? ref : dereferenceR(parts, ref);
  };

  var dereference = function(str) {
    return dereferenceR(str.split('.'), that);
  };

  var setGlobalValue = function(str, value) {
    var split = str.split('.');
    var last = split.pop();
    var ref = split.length == 0 ? that : dereferenceR(split, that);
    ref[last] = value;
  };

  var cleanUpMock = function(noVerify) {
    try {
      if (!noVerify) {
        verifyExpectations();
      }
    } finally {
      clearExpectations();
      clearMockedObjects();
    }
  }

  // classes

  var Expectation = function(name, target, memberName, isMockedObject) {
    var me = this;
    this.name = name;
    this.memberName = memberName;
    this.expectedCalls = -1;
    this.receivedCalls = 0;
    this.expectedArguments = null;
    this.originalFunction = target[memberName];
    this.target = target;
    this.isMockedObject = isMockedObject;
    target[memberName] = Expectation.generateOverrideFn(this);
  };

  Expectation.generateOverrideFn = function(that) {
    var ret = function() {
      return Expectation.overrideFn.apply(that, arguments);
    };

    if (that.originalFunction) {
      for (var x in that.originalFunction) {
        ret[x] = that.originalFunction[x];
      }
    }

    return ret;
  };

  Expectation.clear = function(that) {
    if (!that.isMockedObject) {
      that.target[that.memberName] = that.originalFunction;
    }
  };

  Expectation.compareArguments = function(that, args) {
    for (var i = 0, len = args.length; i < len; ++i) {
      if (!QUnit.equiv(that.expectedArguments[i], args[i])) {
        return false;
      }
    }
    return true;
  };

  Expectation.checkArguments = function(that, args) {
    switch (true) {
      case that.expectedArguments == null:
        return true;
      case args.length != that.expectedArguments.length:
        return false;
      default:
        return Expectation.compareArguments(that, args);
    }
  };

  Expectation.overrideFn = function() {
    if (!Expectation.checkArguments(this, arguments)) {
      return typeof(this.originalFunction) === 'function'
        ? this.originalFunction.apply(this.target, arguments)
        : null;
    }
    this.receivedCalls++;

    switch (true) {
      case this.hasSpecifiedReturn:
        return this.valueToReturn;
      case (typeof(this.mockFunction) !== 'function' &&
            typeof(this.originalFunction) === 'function'):
        return this.originalFunction.apply(this.target, arguments);
      case typeof(this.mockFunction) === 'function':
        return this.mockFunction.apply(this.target, arguments);
      case this.isMockedObject:
        return;
      default:
        throw 'Not sure what to do here';
    }
  };

  Expectation.prototype = {
    verifyCallCount: function() {
      switch (this.expectedCalls) {
        case -2:
          return;
        case -1:
          ok(this.receivedCalls > 0, this.name + ': ' + (this.expectedArguments == null ? 'Expected at least one call.' : 'Expected at least one call with the given arguments.'));
          return;
        case 0:
          ok(this.receivedCalls == 0, this.name + ': ' + 'Expected no calls, was called ' + this.receivedCalls + ' times.');
          return;
        case this.expectedCalls > 0 && this.expectedCalls:
          ok(this.receivedCalls == this.expectedCalls, this.name + ': ' + 'Expected ' + this.expectedCalls + ' calls, received ' + this.receivedCalls + '.');
          return;
        // TODO: negatives less than -1
      }
    },

    withArguments: function() {
      this.expectedArguments = arguments;
      return this;
    },

    returnValue: function(value) {
      this.hasSpecifiedReturn = true;
      this.valueToReturn = value;
      return this;
    },

    exactly: function(times) {
      this.expectedCalls = times;
      return this;
    },

    maybe: function() {
      this.expectedCalls = -2;
      return this;
    },

    mock: function(fn) {
      this.mockFunction = fn;
      return this;
    },

    never: function() {
      this.expectedCalls = 0;
      return this;
    }
  };

  // preserve

  var preserve = function(arr, fn) {
    var oldValues = {};
    each(arr, function(val) { oldValues[val] = dereference(val); });
    try {
      fn();
    } finally {
      each(arr, function(val) { setGlobalValue(val, oldValues[val]); });
    }
  };

  // mock

  var mock = function(fn) {
    var noVerify = false;
    checkForMockingContext(false);

    mock.inMockingContext = true;
    try {
      fn();
    } catch(e) {
      noVerify = true;
      throw e;
    } finally {
      mock.inMockingContext = false;
      cleanUpMock(noVerify);
    }
  };

  // mock.create

  mock.create = function(name, members) {
    members = members || {};
    checkForMockingContext(true);

    var obj = {};
    for (var x in members) {
      obj[x] = members[x];
    }

    mock.mockedObjects[name] = obj;
    return obj;
  };

  // mock.expect

  mock.expect = function(str) {
    var split = str.split('.');
    var isMockedObject = false;
    var obj, name, expectation;
    if (split.length == 2 && (obj = mock.mockedObjects[split[0]])) {
      name = split[1];
      isMockedObject = true;
    } else {
      name = split.pop();
      obj = split.length == 0 ? that : dereferenceR(split, that);
    }
    return createExpectation(str, obj, name, isMockedObject);
  };

  // mock members

  mock.inMockingContext = false;
  mock.mockedObjects = {};
  mock.expectations = [];

  // exposed functions

  that['mock'] = mock;
  that['preserve'] = preserve;
})(this);
