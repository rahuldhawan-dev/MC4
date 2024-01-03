// HUGE NOTE: I, Ross, loathe this entire thing with every fiber of my being.
// And I say this as the author. It is filled with hacks to deal with shortcomings
// in MVC's unobtrusive ajax library. -Ross 2/17/2020

// Not working as expected? This script has to load *before* the unobtrusive-ajax.js library! -Ross 6/12/2023

// NOTES:
// The cache methods are for holding on to references to rows/tables/dialogs
// instead of hiding it all in jQuery data and having to find everything through 
// element ids.

// TODO:
//    1. Disable elements when they're clicked?
//    2. Make this unobtrusive so we don't need to have external scripts that only run the initializer script.
//    3. Switch this to a jQuery plugin

var AjaxTable = {
  _clickHandlers: [],
  initializeAll: () => {
    // AjaxTable was setup to parse links lazily. This allowed them to work easily when 
    // links were loaded dynamically(ex: ajax tabs). However, allowing them to initialize
    // dynamically meant that their click handlers would be added after unobtrusive-ajax
    // added its click handlers. To bypass this, we need to get a general click handler
    // added before unobtrusive-ajax does. We then need to handle the individual click
    // handling in here rather than through normal click handlers.
    $(document).on('click', 'a[data-ajax="true"]', (e) => {
      for (let i = 0; i < AjaxTable._clickHandlers.length; i++) {
        if (AjaxTable._clickHandlers[i](e)) {
		  // stop processing handlers after the correct one is found
          return true;
        }
      }
      return true; // Need to let other click handlers continue 
    });
    },
  'initialize': function (selector) {
    AjaxTable._clickHandlers.push(function (e) {
	  const $el = $(e.target);
      if ($el.attr('data-ajax-table') === selector || $(selector + ' a[data-ajax="true"]').is($el)) {
	  	return AjaxTable.events.onLinkClick($el, selector);
	  }
	  return false; 
    });
  },
  'createUniqueId': function() { 
     // Why did I make this a random number? -Ross 11/11/2014
     return Math.floor(Math.random() * 1000000);
  },
  'destroy': function(cacheId) {
    var cache = AjaxTable.caching.get(cacheId);
    AjaxTable.dialogs.destroy(cache.dialog);
    AjaxTable.caching.destroy(cache.id);
  },
  'caching': {
    '_itemCache': {},
    'create': function($row, $table, $dialog) {
      var cache = {
        'id': AjaxTable.createUniqueId(),
        'row': $row,
        'table': $table,
        'dialog': $dialog
      };
      AjaxTable.caching._itemCache[cache.id] = cache;
      return cache;
    },
    'destroy': function (id) {
      delete AjaxTable.caching._itemCache[id];
    },
    'get': function (id) {
      return AjaxTable.caching._itemCache[id];
    }
  },

  'events': {
    'setOnSuccessHandler': function($el, cacheId, resultType) {
      var method = $el.attr('data-ajax-method');
      // These have to be strings due to the way the unobtrusive ajax library works.
      $el.attr('data-ajax-success', 'AjaxTable.events.onSuccess(data, "' + cacheId + '", "' + resultType + '","' + method + '")');
      $el.attr('data-ajax-failure', 'AjaxTable.events.onError(xhr, status, error, "' + cacheId + '", "' + resultType + '")');
    },
    // For action links inside the table
    'onLinkClick': function($link, selector) {
      var $row = $link.closest('tr', $(selector));
      var $table = $($link.attr('data-ajax-table'));
      var cache = AjaxTable.caching.create($row, $table, null);
      AjaxTable.events.setOnSuccessHandler($link, cache.id, 'dialog');
      // Let unobtrusive ajax continue doing its thing. 
      return true;
    },
    'onError': function(xhr, status, error, cacheId, resultType) {
      AjaxTable.dialogs.showError(xhr.responseText, cacheId);
    },
    'onSuccess': function(data, cacheId, resultType, method) {
      var cache = AjaxTable.caching.get(cacheId);
      var $row = cache.row;
      if (method === 'DELETE') {
        $row.remove();
      }
      else if (resultType === 'dialog') {
        AjaxTable.dialogs.show(data, cacheId);
      }
      else if (resultType === 'row') {
        // This means our model state was invalid, so we need
        // to show the dialog again.
        if (data.indexOf('<form') >= 0) {
          return AjaxTable.events.onSuccess(data, cacheId, 'dialog', method);
        }
        
        var $newRow = AjaxTable.rows.create(data, cacheId);
        
        if (cache.row && cache.row.length > 0) {
          AjaxTable.rows.replace(cache.row, $newRow);
        }
        else if (cache.table && cache.table.length > 0) {
          AjaxTable.rows.add(cache.table, $newRow);
        }
        AjaxTable.destroy(cacheId);
      }
    }
  },

  'rows': {
    'create': function(html, cacheId) {
      var $row = $(html);
      $row.attr('id', cacheId); // Give it the same id as the original row.
      return $row;
    },
    'add': function($table, $newRow) {
      $table.append($newRow);
    },
    'replace': function($oldRow, $newRow) {
      $oldRow.after($newRow);
      AjaxTable.rows.destroy($oldRow);
    },
    'destroy': function($row) {
      $row.remove();
    }
  },

  'dialogs': {
    'show': function(content, cacheId) {
      var $content = $(content);
      var $form = AjaxTable.dialogs.getFormFromContent($content);
      AjaxTable.events.setOnSuccessHandler($form, cacheId, 'row');
      var cache = AjaxTable.caching.get(cacheId);
      if (!cache.dialog) {
        cache.dialog = AjaxTable.dialogs.create($content, $form.attr('title'), cacheId);
        // Remove the title so it doesn't show up on hover.
        $form.attr('title', null);
      }
      else {
        AjaxTable.dialogs.reset($content, cache.dialog);
      }

      // We need to do this ourselves since it's not automated.
      $.validator.unobtrusive.parseDynamicContent($content);
    },
    'getFormFromContent': function($content) {
      if ($content.prop('tagName') === 'FORM') {
        return $content;
      }
      var fromSiblings = $content.siblings('form');
      if (fromSiblings.length > 0) {
        return fromSiblings;
      }
      // .find doesn't return what .siblings returns.
      return $content.find('form');
    },
    'showError': function(error, cacheId) {
      var content = '<div>' + error + '</div>';
      AjaxTable.dialogs.show(content, cacheId, 'Error');
    },
    'create': function($content, title, cacheId) {
      // We wrap the content in a single div so when we call the
      // dialog function it doesn't try to create a dialog
      // for each child element. The id's used for regression testing.
      var $d = $('<div id="dialog-content-wrapper" style="position:relative;"></div>');

      // This is set to true if the outermost element
      // for the form has a width explicitly set. We 
      // need this because of IE7.
      var widthSpecified = false;

      // When you do a $(<htmlcontentstuff>), jQuery returns
      // an object that has all the script tags removed and
      // appended to the end, making it easy for us to 
      // pull them out and append properly.
      var scripts = [];
      $content.each(function() {
                var el = $(this);
                // tagName is always all caps.
                if (el.prop('tagName') === 'SCRIPT') {
                    scripts.push(el);
                }
                else {
                    // We need to ignore nodeType 3(text nodes), because
                    // width() will throw an exception on it.
                    if (!widthSpecified && el[0].nodeType !== 3 && el.width()) {
                        widthSpecified = true;
                    }
                    $d.append(el);
                }
            });

      // this needs to be setup *after* the content is appended, otherwise
      // the dialog doesn't get positioned properly.
      $d.dialog({
        'autoOpen': false, // Needs to be false so it doesn't open until after scripts load
        'modal': true,
        'alsoResize': true,
        // Arbitrarily setting this to 400px if no width is specified from the view.
        'width': 'auto',
        'height': 'auto',
        'title': title,
        'maxHeight': '400px',
        // We need to destroy the dialog if they use the jQuery cancel button.
        'close': function () { AjaxTable.destroy(cacheId); },
        'closeText': '' // The css for this doesn't work with text. 
      });
      //    $d.dialog('option','width', '300px');
      // Scripts that were part of the content must be loaded AFTER
      // the rest of the content is attached to the document's DOM.
      // Otherwise if a script is dependent on elements in the
      // ajaxed content, the content won't exist and the script
      // will get null refs to elements.
      $(scripts).each(function(i, script) {
        $d.append(script);
      });
      // Open the dialog after scripts have loaded
      $d.dialog('open');
      AjaxTable.dialogs._fixDialogDimensions($d);
      $d.find('select').unobtrusiveDetergent();
      $d.on('click.ajaxtable', '.cancel', function() { AjaxTable.dialogs.destroy($d); });
      return $d;
    },
    '_fixDialogDimensions': function($d) {
      var dWrapper = $d.closest('.ui-dialog');
      var dTitle = dWrapper.find('.ui-dialog-titlebar');
      var dContent = dWrapper.find('.ui-dialog-content');

      // The max heights need to be set manually because jqueryui's dialog
      // doesn't do it correctly. 
      // 24 is an arbitrary number for the padding used by ui-dialog-content

      var pxToInt = function($el, cssProp) {
                var val = $el.css(cssProp);
                if (!val) { return 0; } // Don't wanna return NaN
                return parseInt(val.match(/\d+/g));
            };

      var contentPadding = pxToInt(dContent, 'padding-top') + pxToInt(dContent, 'padding-bottom');
      var winHeight = $(window).height();
      dWrapper.css('max-height', winHeight);
      dContent.css('max-height', winHeight - dTitle.height() - (contentPadding * 2));
      dContent.css('overflow-x', 'hidden'); // Fixes scrollbar coming up when width is specified
    },
    'reset': function($content, $existingDialog) {
      AjaxTable.dialogs.destroyUnobtrusive($existingDialog);
      $existingDialog.empty();
      $existingDialog.append($content);
      $content.find('select').unobtrusiveDetergent();
    },
    'destroy': function($dialog) {
      $dialog.off('.ajaxtable');
      $dialog.dialog('destroy');
      AjaxTable.dialogs.destroyUnobtrusive($dialog);
      // remove needs to be called because destroy doesn't actually remove
      // it from the DOM. Otherwise element id conflicts come up.
      $dialog.remove();
    },
    'destroyUnobtrusive': function($dialog) {
      $dialog.find('.date').datepicker('destroy');
      $dialog.find('select').unobtrusiveDetergent('destroy');
    }
  }
};

$(document).ready(AjaxTable.initializeAll);