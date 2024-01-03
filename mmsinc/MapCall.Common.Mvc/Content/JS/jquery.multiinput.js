(function($) {
 /*** SHIMS ***/

  if (!Function.prototype.bind) {
    Function.prototype.bind = function (oThis) {
      if (typeof this !== "function") {
        // closest thing possible to the ECMAScript 5 internal IsCallable function
        throw new TypeError("Function.prototype.bind - what is trying to be bound is not callable");
      }

      var aArgs = Array.prototype.slice.call(arguments, 1),
      fToBind = this,
      fNOP = function () {},
      fBound = function () {
        return fToBind.apply(
          this instanceof fNOP && oThis ? this : oThis,
          aArgs.concat(Array.prototype.slice.call(arguments)));
      };

      fNOP.prototype = this.prototype;
      fBound.prototype = new fNOP();

      return fBound;
    };
  }

  var pluginName = 'multiinput';

  var events = {
    addBtnClick: function ($this, $target, e) {
      e.preventDefault();
      var $input = $this.data(pluginName).$input;
      $this[pluginName]('addValue', $input.val());
      $input.val('');
      return false;
    },

    removeLinkClick: function($this, $target, e) {
      e.preventDefault();

      $($target.parent()).remove();

      return false;
    }
  };

  var eventCurry = function(callback, e) {
    var $this = $(this);
    var $target = $(e.target);
    callback($this, $target, e);
  };

  var methods = {
    init: function(options) {
      var that = this;
      var $this = $(this);
      var attr = {
        name: $this.attr('name'),
        $input: $('<input id="' + $this.attr('id') + 'Input">').appendTo($this),
        $addBtn: $('<button class="add" title="Add item">+</button>').appendTo($this),
        $list: $('<ul></ul>').appendTo($this),
        onValidationError: options.onValidationError || $.error,
        refuseBlanks: options.refuseBlanks || true
      };

      $this.on('click', 'button.add', eventCurry.bind(that, events.addBtnClick));
      $this.on('click', 'li a', eventCurry.bind(that, events.removeLinkClick));
      $this.closest('form').on('fauxreset', function() {
        methods.reset($this, attr);
      });

      $this.data(pluginName, attr);
    },

    reset: function ($this, attr) {
      attr.$list.empty();
    },

    hasValue: function ($this, attr, value) {
      return $('input[value="' + value + '"]', attr.$list).length > 0;
    },

    addValue: function ($this, attr, value) {
      if ($this[pluginName]('hasValue', value)) {
        attr.onValidationError('Value "' + value + '" already added.');
      } else if (attr.refuseBlanks && /^\s*$/.test(value)) {
        attr.onValidationError('Cannot add blank value.');
      } else {
        attr.$list.append('<li>' + value + ' <input type="hidden" name="' + attr.name + '" value="' + value + '" /><a href="#" title="Remove item">-</a></li>');
      }
      return $this;
    },

    getCount: function($this, attr) {
      return $('input[type="hidden"]', attr.$list).length;
    }
  };

  $.fn[pluginName] = function(method) {
    if (methods[method]) {
      var $this = $(this);
      var attr = $this.data(pluginName);
      var args = [$this, attr].concat(Array.prototype.slice.call(arguments, 1));
      return methods[method].apply(this, ($this, attr, args));
    } else if (typeof method === 'object' || !method) {
      return methods.init.apply(this, arguments);
    } else if (typeof method === 'function') {
      return method();
    } else {
      $.error('Method ' + method + ' does not exist');
    }
  };

  $(document).ready(() => {
    $('div.multiinput').multiinput({ onValidationError: function (msg) { window.alert.call(window, msg); } });
  });
})(jQuery);
