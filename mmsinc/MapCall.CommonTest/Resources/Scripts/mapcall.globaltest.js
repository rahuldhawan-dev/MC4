/// <reference path="../../../MMSINC.Testing/Scripts/qunit.js" />
/// <reference path="../../../MMSINC.Testing/Scripts/qmock.js" />
/// <reference path="../../../MMSINC.Testing/Scripts/testHelpers.js" />
/// <reference path="testHelpers.js"/>
/// <reference path="../../../MapCall.Common.WebForms/Resources/scripts/mapcall.global.js" />

module('MapCall.Global Tests');

test('MapCall.initialize() should setup the global unload, fix webkit issues, initilize the menu, and initialize any tabs', function() {
  mock(function() {
    mock.expect('MapCall.fixWebkit')
      .mock(noop);
    mock.expect('MapCall.initializeMenu')
      .mock(noop);
    mock.expect('MapCall.initializeTabs')
      .mock(noop);
    mock.expect('MapCall.initializeEventCleanup')
      .mock(noop);
    mock.expect('MapCall.initEvents')
			.mock(noop);

    MapCall.initialize();
  });
});

test('MapCall.fixWebkit() fixes some .net AJAX functionality if userAgent denotes webkit', function() {
  var oldSys = typeof(Sys) == 'undefined' ? {} : Sys;
  var oldSysBrowser = typeof(oldSys.Browser) == 'undefined' ? {} : Sys.Browser;
  Sys = {Browser: {}};
  var name = 'WebKit';
  var version = 12.34;
  var userAgent = name + '/' + version.toString();

  mock(function() {
    mock.expect('MapCall.getUserAgent')
      .mock(noop)
      .returnValue(userAgent);

    MapCall.fixWebkit();

    deepEqual(Sys.Browser.agent, Sys.Browser.WebKit);
    equal(version, Sys.Browser.version);
    equal(name, Sys.Browser.name);
  });

  Sys = oldSys;
  Sys.Browser = oldSysBrowser;
});

test('MapCall.fixWebkit() does nothing if userAgent does not denote webkit', function() {
  var oldSys = typeof(Sys) == 'undefined' ? {} : Sys;
  var oldSysBrowser = typeof(oldSys.Browser) == 'undefined' ? {} : Sys.Browser;
  Sys = {Browser: {}};
  var name = 'KrebKit';

  mock(function() {
    mock.expect('MapCall.getUserAgent')
      .mock(noop)
      .returnValue(name);

    MapCall.fixWebkit();

    ok(typeof(Sys.Browser.agent) == 'undefined');
    ok(typeof(Sys.Browser.WebKit) == 'undefined');
    ok(typeof(Sys.Browser.version) == 'undefined');
    ok(typeof(Sys.Browser.name) == 'undefined');
  });

  Sys = oldSys;
  Sys.Browser = oldSysBrowser;
});

test('MapCall.initializeMenu() initializes the main menu accordion control', function () {
  var locHref = 'THIS IS THE LOCATION HREF';
  mock(function () {
    var mainMenu = mock.create('mainMenu', ['accordion', 'css']);
    var contentList = mock.create('contentList', ['treeview']);
    var linkOnlyHeaders = mock.create('linkOnlyHeaders', ['click']);
    var menuSections = mock.create('menuSections', ['filter']);
    var activeSection = mock.create('activeSection', ['index']);

    mock.expect('jQuery')
      .withArguments(MapCall.LOOKUP_STRINGS.mainMenu)
      .returnValue(mainMenu);
    mock.expect('mainMenu.accordion')
      .mock(function (opts) {
        if (arguments.length == 3) {
          equal(arguments[0], 'option');
          equal(arguments[1], 'active');
          equal(arguments[2], 42);
        } else {
          equal(MapCall.LOOKUP_STRINGS.titleDiv, opts.header);
          equal('content', opts.heightStyle);
          ok(opts.collapsible);
        }
      });
    mock.expect('mainMenu.find')
      .withArguments('> div')
      .returnValue(menuSections);
    mock.expect('menuSections.filter')
      .mock(function (filterFunc) {
        return activeSection;
      });
    mock.expect('activeSection.index')
      .returnValue(42);

    mock.expect('jQuery')
      .withArguments(MapCall.LOOKUP_STRINGS.contentList)
      .returnValue(contentList);
    mock.expect('contentList.treeview')
      .mock(function (opts) {
        ok(opts.collapsed);
        equal('location', opts.persist);
      });
    mock.expect('jQuery')
      .withArguments(MapCall.LOOKUP_STRINGS.linkOnlyHeaders)
      .returnValue(linkOnlyHeaders);
    mock.expect('linkOnlyHeaders.click')
      .withArguments(MapCall.linkOnlyHeader_Click);
    mock.expect('mainMenu.css')
      .withArguments('display', 'block');

    MapCall.initializeMenu();
  });
});

test('MapCall.initializeTabs() initializes the tabs container and shows the nav', function() {
  mock(function() {
    var tabsContainer = mock.create('tabsContainer', ['tabs']);
    var uiTabsNav = mock.create('uiTabsNav', ['css']);

    mock.expect('jQuery')
      .withArguments(MapCall.LOOKUP_STRINGS.tabsContainer)
      .returnValue(tabsContainer);
    mock.expect('tabsContainer.tabs');
    mock.expect('jQuery')
      .withArguments(MapCall.LOOKUP_STRINGS.uiTabsNav)
      .returnValue(uiTabsNav);
    mock.expect('uiTabsNav.css')
      .withArguments('visibility', 'visible');

    MapCall.initializeTabs();
  });
});

test('MapCall.initializeEventCleanup() sets up an unload event for the window', function() {
  mock(function() {
    var theWindow = mock.create('theWindow', ['unload']);

    mock.expect('MapCall.getWindowObj')
      .returnValue(theWindow);
    mock.expect('jQuery')
      .withArguments(theWindow)
      .returnValue(theWindow);
    mock.expect('theWindow.unload')
      .withArguments(MapCall.unload);

    MapCall.initializeEventCleanup();
  });
});

test('MapCall.linkOnlyHeader_Click(e) sets the window.location to the href from the event target', function() {
  var e = {
    currentTarget: {
      href: 'this is the href'
    }
  };
  mock(function() {
    var theWindow = mock.create('theWindow', ['attr']);

    mock.expect('MapCall.getWindowObj')
      .returnValue(theWindow);
    mock.expect('jQuery')
      .withArguments(theWindow)
      .returnValue(theWindow);
    mock.expect('theWindow.attr')
      .withArguments('location', e.currentTarget.href);

    MapCall.linkOnlyHeader_Click(e);
  });
});

test('MapCall.unload() unloads any set event handlers', function() {
  var oldJQueryCache = jQuery.cache;
  jQuery.cache = {
    foo: {
      handle: {
        elem: new Object()
      }
    },
    bar: {
      handle: {
        elem: new Object()
      }
    }
  };
  mock(function() {
    var theWindow = mock.create('theWindow', ['unbind']);

    mock.expect('MapCall.getWindowObj')
      .returnValue(theWindow);
    mock.expect('jQuery')
      .withArguments(theWindow)
      .returnValue(theWindow);
    mock.expect('theWindow.unbind')
      .withArguments('unload', MapCall.unload);
    mock.expect('jQuery.event.remove')
      .withArguments(jQuery.cache.foo.handle.elem);
    mock.expect('jQuery.event.remove')
      .withArguments(jQuery.cache.bar.handle.elem);

    MapCall.unload();
  });

  jQuery.cache = oldJQueryCache;
});

test('MapCall.getUserAgent() returns the userAgent string', function() {
  equal(navigator.userAgent, MapCall.getUserAgent());
});

test('MapCall.getWindowObj() returns the window', function() {
  deepEqual(window, MapCall.getWindowObj());
});

test('MapCall.getLocationHref() returns the href value from the location object', function() {
  equal(location.href, MapCall.getLocationHref());
});

test('MapCall.fixHref(str) strips any querystring or hash value from the given url', function() {
  var url = 'this is the url';
  var query = '?and this is the query string';
  var hash = '#and this is the hash value';

  equal(url, MapCall.fixHref(url));
  equal(url, MapCall.fixHref(url + query));
  equal(url, MapCall.fixHref(url + hash));
});

test('script setups an event for document.ready that initializes the MapCall class', function() {
  deepEqual(MapCall.initialize, $.documentReadyFn);
});
