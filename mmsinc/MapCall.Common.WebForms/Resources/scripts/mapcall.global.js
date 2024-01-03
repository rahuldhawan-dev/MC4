// Make sure to use the jQuery caller here to prevent any weird conflicts that could arise.

// Not sure if there's a namespace conflict here
var MapCall = {
  LOOKUP_STRINGS: {
    mainMenu: '#mainMenu',
    titleDiv: 'div.title',
    contentList: '#mainMenu div.content > ul',
    linkOnlyHeaders: '#mainMenu div.linkOnly a',
    tabsContainer: '.tabsContainer',
    noDoubleSubmitForm: 'form', // all the forms
    submitButtons: ':submit',
    resetButtons: '.reset',
    uiTabsNav: '.ui-tabs-nav'
  },
  initialize: function () {
    MapCall.fixWebkit();
    MapCall.initializeMenu();
    MapCall.initializeTabs();
    MapCall.initializeEventCleanup();
	  MapCall.initEvents();
  },
  fixWebkit: function () {
    // This is fixed in ASP 4 I think, but at the moment MS's
    // AJAX library gets upset about validators and crap 
    // that are inside UpdatePanels when the browser runs Webkit.
    var userAgent = MapCall.getUserAgent();
    if (userAgent.indexOf('WebKit/') > -1) {
      var b = Sys.Browser;
      b.WebKit = {}; //Safari 3 is considered WebKit
      Sys.Browser.agent = b.WebKit;
      Sys.Browser.version = parseFloat(userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
      Sys.Browser.name = 'WebKit';
    }
  },
  initEvents: function () {
	  if ($('#content_cphMain_cphMain_pnlSearch').length === 0) {
		  $(document).on('submit', MapCall.LOOKUP_STRINGS.noDoubleSubmitForm, MapCall.onNoDoubleSubmitFormSubmit);
	  }
	  $(document).on('click', MapCall.LOOKUP_STRINGS.submitButtons, MapCall.onSubmitButtonClick);
		$(document).on('click', MapCall.LOOKUP_STRINGS.resetButtons, MapCall.onResetButtonClicked);
	},
  onNoDoubleSubmitFormSubmit: function (e) {
  	// also want to skip if we're in a search form

  	// don't skip if the button was a non-destructive action lke view or show
	  if ($("input[type=submit][clicked=true]")[0] !== undefined && $("input[type=submit][clicked=true]")[0].value.toUpperCase() === "VIEW")
		  return true;

  	if (e.isImmediatePropagationStopped()) {
			return false;
  	}
		
		//disable all the submit buttons
		$(':submit', this).attr('disabled', 'disabled');
		//get the clicked submit button
		var button = $(':submit[clicked=true]');
		$(this).submit(function () { return false; });

		if (button.val()) {
			button.after('<input type="hidden" value="' + button.val() + '" name="' + button.attr('name') + '" />');
		}

		return true;
	},
	onSubmitButtonClick: function () {
		// clear all the submit buttons in case one was already clicked
		$(MapCall.LOOKUP_STRINGS.submitButtons).removeAttr('clicked');
		// add clicked to this one.
		$(this).attr('clicked', 'true');
	},
	onResetButtonClicked: function (ev) {
		var element = $(this)[0];
		var form = element.form;
		$(':input', form).filter(':visible').val('').removeAttr('checked').removeAttr('selected').trigger('change');
		$('.range', form).unobtrusiveRangePicker('reset');
		$('.multilist', form).multilist('clear');
		// Some dropdowns do not have an empty value so form
		// resetting causes the dropdown to not display any
		// selected value. To get past this, we select the first
		// value in the list. This is only done for selects that
		// request it, because otherwise we want the form reset
		// to just select the empty string value item by default.
		$('select[data-select-on-reset]').each(function() {
			var $s = $(this);
			var resetValue = $s.attr('data-select-on-reset');

			// Specifically check "" instead of !resetValue because we want 0's included.
			if (resetValue == "") {
				resetValue = $(this).find('option:first').val();
			}
			$(this).val(resetValue);
		});

		// TODO: Make unobtrusiveRangePicker listen for this event.
		// We can't trigger the onreset event because it messes things up in IE.
		// It won't mark the selected dropdown item as selected.
		$(form).trigger('fauxreset');
	},
  initializeMenu: function () {

    // When initializing alot of these jQuery controls, they should be set to display:none until
    // they've been fully initialized. This allows for faster rendering since the browser
    // won't have to actually draw the controls until they've finished creating secondary
    // styling elements.  This may cause issues with slow connections or huge table loads.

    var mainMenu = jQuery(MapCall.LOOKUP_STRINGS.mainMenu);

    // Initializes the site-wide nav menu.
    // navigation: true tells the accordion to automatically open
    // to the right url. How it does that, I don't know. But yay magic!
    // navigationFilter: Overrides the default so it handles querystrings properly.
    mainMenu.accordion({
      header: MapCall.LOOKUP_STRINGS.titleDiv,
      heightStyle: 'content',
      collapsible: true
    });

    var menuSections = mainMenu.find('> div');
    var activeSection = menuSections.filter(function () {
      var locHref = MapCall.fixHref(MapCall.getLocationHref().toLowerCase());

      var activeLink = $(this).find('a').filter(function () {
        return this.href.toLowerCase() == locHref;
      });

      return (activeLink.length > 0);
    });
    mainMenu.accordion('option', 'active', activeSection.index());

    // Initializes the treeview part of the site-wide menu.
    // There's custom code added in jquery.treeview.js that were needed
    // so that it worked properly when urls contain a querystring or a #.
    jQuery(MapCall.LOOKUP_STRINGS.contentList).treeview({ collapsed: true, persist: 'location' });

    // This is needed to make link-only accordion headers function properly.
    jQuery(MapCall.LOOKUP_STRINGS.linkOnlyHeaders).click(MapCall.linkOnlyHeader_Click);

    mainMenu.css('display', 'block');
  },
  initializeTabs: function () {
    // do the tab thing.
    jQuery(MapCall.LOOKUP_STRINGS.tabsContainer).tabs();
    jQuery(MapCall.LOOKUP_STRINGS.uiTabsNav).css('visibility', 'visible');

  },
  initializeEventCleanup: function () {
    jQuery(MapCall.getWindowObj()).unload(MapCall.unload);
  },
  linkOnlyHeader_Click: function (e) {
    jQuery(MapCall.getWindowObj()).attr('location', e.currentTarget.href);
  },
  unload: function () {
    jQuery(MapCall.getWindowObj()).unbind('unload', MapCall.unload);
    var cache = jQuery.cache;
    for (var id in cache) {
      try {
        var cur = cache[id];
        if (cur && cur.handle) {
          jQuery.event.remove(cur.handle.elem);
        }
      } catch (e) {
        //alert(id); //noop
      }
    }
  },
  getUserAgent: function () {
    return navigator.userAgent;
  },
  getWindowObj: function () {
    return window;
  },
  getLocationHref: function () {
    return location.href;
  },
  fixHref: function (str) {
    if (str.indexOf('?') > -1) {
      return str.split('?')[0];
    }
    else if (str.indexOf('#') > -1) {
      return str.split('#')[0];
    }
    return str;
  }
};

jQuery(document).ready(MapCall.initialize);


// HelpBox jQuery plugin
// By Ross Dickinson: 9/14/11
// So if it screws up let him know.
// This could very well be fixed in a later version of jQuery or jQuery UI.
(function ($) {
  $.fn.helpBox = function (opts) {
    var clone = this.find('.content').clone();
    clone.dialog({
      'title': this.attr("title"),
      'width': (opts.width ? opts.width : 'auto'),
      'height': (opts.height ? opts.height : 'auto'),
      'alsoResize': true
    });
    // Hack to make the titlebar resize properly in IE7. 
    // The titlebar won't stretch to fill out until someone manually
    // resizes the dialog otherwise.
    if ($.browser.msie && parseInt($.browser.version) <= 7) {
      var dialog = clone.closest('.ui-dialog');
      var dialogWidth = dialog.innerWidth();
      dialog.css({ 'width': dialogWidth });
    }
  };
})(jQuery);


(function ($) {
  // This is needed because jQuery 1.9+ does not include jQuery.browser anymore
  // and we need a decent way to detect just IE.
  if (!$.browser) {

    $.browser = {}; (function () {
      $.browser.msie = false;
      $.browser.version = 0;
      if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        $.browser.msie = true;
        $.browser.version = RegExp.$1;
      }
    })();
  }
})(jQuery);

/*
json.js
2010-12-08

Public Domain

No warranty expressed or implied. Use at your own risk.

See http://www.JSON.org/js.html
*/

if (!this.JSON) { this.JSON = {}; }
(function () {
  "use strict"; function f(n) { return n < 10 ? '0' + n : n; }
  if (typeof Date.prototype.toJSON !== 'function') {
    Date.prototype.toJSON = function (key) {
      return isFinite(this.valueOf()) ? this.getUTCFullYear() + '-' +
  f(this.getUTCMonth() + 1) + '-' +
  f(this.getUTCDate()) + 'T' +
  f(this.getUTCHours()) + ':' +
  f(this.getUTCMinutes()) + ':' +
  f(this.getUTCSeconds()) + 'Z' : null;
    }; String.prototype.toJSON = Number.prototype.toJSON = Boolean.prototype.toJSON = function (key) { return this.valueOf(); };
  }
  var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, gap, indent, meta = { '\b': '\\b', '\t': '\\t', '\n': '\\n', '\f': '\\f', '\r': '\\r', '"': '\\"', '\\': '\\\\' }, rep; function quote(string) { escapable.lastIndex = 0; return escapable.test(string) ? '"' + string.replace(escapable, function (a) { var c = meta[a]; return typeof c === 'string' ? c : '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4); }) + '"' : '"' + string + '"'; }
  function str(key, holder) {
    var i, k, v, length, mind = gap, partial, value = holder[key]; if (value && typeof value === 'object' && typeof value.toJSON === 'function') { value = value.toJSON(key); }
    if (typeof rep === 'function') { value = rep.call(holder, key, value); }
    switch (typeof value) {
      case 'string': return quote(value); case 'number': return isFinite(value) ? String(value) : 'null'; case 'boolean': case 'null': return String(value); case 'object': if (!value) { return 'null'; }
        gap += indent; partial = []; if (Object.prototype.toString.apply(value) === '[object Array]') {
          length = value.length; for (i = 0; i < length; i += 1) { partial[i] = str(i, value) || 'null'; }
          v = partial.length === 0 ? '[]' : gap ? '[\n' + gap +
partial.join(',\n' + gap) + '\n' +
mind + ']' : '[' + partial.join(',') + ']'; gap = mind; return v;
        }
        if (rep && typeof rep === 'object') { length = rep.length; for (i = 0; i < length; i += 1) { k = rep[i]; if (typeof k === 'string') { v = str(k, value); if (v) { partial.push(quote(k) + (gap ? ': ' : ':') + v); } } } } else { for (k in value) { if (Object.hasOwnProperty.call(value, k)) { v = str(k, value); if (v) { partial.push(quote(k) + (gap ? ': ' : ':') + v); } } } }
        v = partial.length === 0 ? '{}' : gap ? '{\n' + gap + partial.join(',\n' + gap) + '\n' +
mind + '}' : '{' + partial.join(',') + '}'; gap = mind; return v;
    }
  }
  if (typeof JSON.stringify !== 'function') {
    JSON.stringify = function (value, replacer, space) {
      var i; gap = ''; indent = ''; if (typeof space === 'number') { for (i = 0; i < space; i += 1) { indent += ' '; } } else if (typeof space === 'string') { indent = space; }
      rep = replacer; if (replacer && typeof replacer !== 'function' && (typeof replacer !== 'object' || typeof replacer.length !== 'number')) { throw new Error('JSON.stringify'); }
      return str('', { '': value });
    };
  }
  if (typeof JSON.parse !== 'function') {
    JSON.parse = function (text, reviver) {
      var j; function walk(holder, key) {
        var k, v, value = holder[key]; if (value && typeof value === 'object') { for (k in value) { if (Object.hasOwnProperty.call(value, k)) { v = walk(value, k); if (v !== undefined) { value[k] = v; } else { delete value[k]; } } } }
        return reviver.call(holder, key, value);
      }
      text = String(text); cx.lastIndex = 0; if (cx.test(text)) {
        text = text.replace(cx, function (a) {
          return '\\u' +
  ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
        });
      }
      if (/^[\],:{}\s]*$/.test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, '@').replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']').replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) { j = eval('(' + text + ')'); return typeof reviver === 'function' ? walk({ '': j }, '') : j; }
      throw new SyntaxError('JSON.parse');
    };
  }
})();
