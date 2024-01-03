/* TEST SCOPE: */

function TestScope(description, ref) {
  this.description = description;
  this.ref = ref;

  return true;
}

TestScope.prototype.toString = function() {
  return this.description;
};

/* TEST: */

function Test(scope) {
  this.scope = scope;
  this.classes = [];

  return true;
}

Test.prototype.addClass = function(cls) {
  this.classes[this.classes.length] = cls;
};

Test.prototype.getLength = function() {
  return this.classes.length;
};

Test.prototype.getClass = function(i) {
  return this.classes[i];
};

/* TEST CLASS: */

function TestClass(name, ref) {
  this.name = name;
  this.ref = ref;
  this.methods = [];

  return true;
}

TestClass.prototype.addMethod = function(method) {
  this.methods[this.methods.length] = method;
};

TestClass.prototype.getLength = function() {
  return this.methods.length;
};

TestClass.prototype.getMethod = function(i) {
  return this.methods[i];
};

TestClass.prototype.addSetup = function(method) {
  if (this.setup) {
    throw 'Test setup method already defined for class ' + this.name + '.';
  }
  this.setup = method;
};

TestClass.prototype.addTeardown = function(method) {
  if (this.teardown) {
    throw 'Test teardown method already defined for class ' + this.name + '.';
  }
  this.teardown = method;
};

/* TEST METHOD: */

function TestMethod(name, ref) {
  this.name = name;
  this.ref = ref;

  return true;
}

TestMethod.prototype.run = function(params) {
  this.ref(params);
};

/* TEST RESULT: */

function TestResult(test) {
  this.testName = test.scope.toString();
  this.classResults = [];
  this.tests = this.successes = this.failures = this.exceptions = 0;

  return true;
}

TestResult.prototype.addClassResult = function(result) {
  this.classResults[this.classResults.length] = result;
  this.tests += result.tests;
  this.successes += result.successes;
  this.failures += result.failures;
  this.exceptions += result.exceptions;
};

TestResult.prototype.getLength = function() {
  return this.classResults.length;
};

TestResult.prototype.getClassResult = function(i) {
  return this.classResults[i];
};

TestResult.prototype.toString = function() {
  return this.testName + ' test result.\n' +
    this.tests + ' tests in ' + this.classResults.length + ' classes.\n' +
    this.successes + ' successes, ' + this.failures + ' failures, ' +
    this.exceptions + ' exceptions.';
};

/* TEST CLASS RESULT:  */

function TestClassResult(cls) {
  this.className = cls.name;
  this.methodResults = [];
  this.tests = this.successes = this.failures = this.exceptions = 0;

  return true;
}

TestClassResult.prototype.addMethodResult = function(result) {
  this.methodResults[this.methodResults.length] = result;
  this.tests++;
  // i will keep doing this as long as javascript lets me. :P
  switch (true) {
    case result.failed:
      this.failures++;
      break;
    case result.exception:
      this.exceptions++;
      break;
    default:
      this.successes++;
      break;
  }
};

TestClassResult.prototype.getLength = function() {
  return this.tests;
};

TestClassResult.prototype.getMethodResult = function(i) {
  return this.methodResults[i];
};

/* TEST METHOD RESULT:  */

function TestMethodResult(method) {
  this.methodName = method.name;
  this.message = '';
  this.failed = this.exception = false;
  this.expected = this.actual = null;

  return true;
}

/* TEST RUNNER: */

function TestRunner(nameSpace) {
  /// <summary>Provides a means of running tests which have been written
  /// using the jsTest library.</summary>
  /// <param name="nameSpace">Optional string representing the root
  /// nameSpace, under which all of the test classes should be found.</param>
  /// <returns>A new TestRunner object.</returns>
  this.nameSpace = nameSpace;

  return true;
}

TestRunner.prototype.runAllTestsInNameSpace = function(nameSpace) {
  var test = TestRunner.buildTestFromScope(
    TestRunner.determineScope(nameSpace || this.nameSpace)
  );
  return TestRunner.runTest(test);
};

TestRunner.determineScope = function(nameSpace) {
  return nameSpace ? new TestScope(nameSpace, window[nameSpace]) :
    new TestScope('window', window);
};

TestRunner.runTest = function(test) {
  return TestRunner.runEachTestClass(test);
};

TestRunner.buildTestFromScope = function(scope) {
  var test = new Test(scope);
  scope = scope.ref;
  for (var x in scope) {
    if (TestRunner.regularExpressions.testClass.test(x)) {
      test.addClass(TestRunner.buildTestClassFromRef(x, scope[x]));
    }
  }
  return test;
};

TestRunner.buildTestClassFromRef = function(name, ref) {
  var cls = new TestClass(name, ref);
  for (var x in ref) {
    switch (true) {
      case TestRunner.regularExpressions.testMethod.test(x):
        cls.addMethod(new TestMethod(x, ref[x]));
        break;
      case TestRunner.regularExpressions.testSetup.test(x):
        cls.addSetup(ref[x]);
        break;
      case TestRunner.regularExpressions.testTeardown.test(x):
        cls.addTeardown(ref[x]);
        break;
    }
  }
  return cls;
};

TestRunner.runEachTestClass = function(test) {
  var result = new TestResult(test);
  for (var i = 0, len = test.getLength(); i < len; ++i) {
    result.addClassResult(TestRunner.runClassTestMethods(test.getClass(i)));
  }
  return result;
};

TestRunner.runClassTestMethods = function(cls) {
  var result = new TestClassResult(cls);
  for (var i = 0, len = cls.getLength(); i < len; ++i) {
    result.addMethodResult(TestRunner.runTestMethod(cls, cls.getMethod(i)));
  }
  return result;
};

TestRunner.runTestMethod = function(cls, method) {
  var result = new TestMethodResult(method);

  var opts = TestRunner.runTestSetup(cls);

  try {
    method.run(opts);
  } catch(e) {
    if (e instanceof AssertFailedException) {
      result.failed = true;
      result.message = e.toString();
      result.expected = e.expected;
      result.actual = e.actual;
    } else {
      result.exception = true;
      result.message = e.toString();
    }
  }

  TestRunner.runTestTeardown(cls);

  return result;
};

TestRunner.runTestSetup = function(cls) {
  return (cls.setup) ? cls.setup() : null;
};

TestRunner.runTestTeardown = function(cls) {
  return (cls.teardown) ? cls.teardown() : null;
};

TestRunner.regularExpressions = {
  testClass: /^(.+)Test$/,
  testMethod: /^test(.+)$/,
  testSetup: /^setup$/,
  testTeardown: /^teardown$/
};
