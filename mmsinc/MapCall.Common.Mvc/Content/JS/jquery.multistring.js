// Multistring plugin.
// NOTE: This plugin is for search pages. It has not been developed to work with normal validation.
(function ($) {
  var DATA_KEY = "MultiString";

  var methods = {
    initialize: function ($mainEl) {
      var data = $mainEl.data(DATA_KEY);

      // This means it's the first time this has initialized.
      if (data == null) {
        data = {
          items: [],
          formState: $mainEl.find('.multistring-formstate'),
          itemWrapper: $('<div class="multistring-item-wrapper"></div>')
        };

        $mainEl.data(DATA_KEY, data);

        var form = $mainEl.closest('form');
        form.on('fauxreset', function () {
          methods.reset($mainEl);
        });

        $mainEl.append(data.itemWrapper);
      }

      $mainEl.find('input[type=hidden]').each(function () {
        var $this = $(this);
        if (!$this.hasClass("multistring-formstate")) {
          $(this).remove(); // We don't want the hidden fields.
          methods.createAndAddTextBox($mainEl, $(this).val());
        }
      });

      // If none, try to load these from form-state.
      if (data.items.length === 0) {
        if (data.formState.val()) {
          var deserialized = JSON.parse(data.formState.val());
          for (var i = 0; i < deserialized.length; i++) {
            methods.createAndAddTextBox($mainEl, deserialized[i]);
          }
        }
      }

      // If none, initiate an empty textbox
      if (data.items.length === 0) {
        methods.createAndAddTextBox($mainEl);
      }
      methods.reinitTextBoxes($mainEl);

    },

    reset: function ($mainEl) {
      var mainElData = $mainEl.data(DATA_KEY);
      mainElData.itemWrapper.empty();
      mainElData.items = [];
      // Need to clear the state before reinitializing everything.
      methods.serializeFormState($mainEl);
      methods.initialize($mainEl);
    },

    serializeFormState: function ($mainEl) {
      var mainElData = $mainEl.data(DATA_KEY);
      var values = [];
      for (var i = 0; i < mainElData.items.length; i++) {
        var val = mainElData.items[i].textBox.val();
        if (val) {
          values.push(val);
        }
      }

      if (values.length > 0) {
        mainElData.formState.val(JSON.stringify(values));
      } else {
        mainElData.formState.val("");
      }
    },

    createAndAddTextBox: function ($mainEl, value) {
      var mainElData = $mainEl.data(DATA_KEY);
      var data = {};

      data.element = $('<div><input type="text" class="multistring-textbox" /><button type="button" class="multistring-add">+</button><button type="button" class="multistring-remove">-</button></div>');
      data.textBox = data.element.find('.multistring-textbox');
      data.addButton = data.element.find('.multistring-add');
      data.removeButton = data.element.find('.multistring-remove');

      data.textBox.val(value);

      mainElData.items.push(data);
      mainElData.itemWrapper.append(data.element);

      data.addButton.click(function () {
        methods.addItem($mainEl);
      });

      data.removeButton.click(function () {
        methods.removeItem($mainEl, data);
      });

      data.textBox.change(function () {
        methods.serializeFormState($mainEl);
      });

      return data;
    },

    reinitTextBoxes: function ($mainEl) {
      // In order for MVC to correctly model bind to an array, we need to
      // include array accessors in the element names. Numbers can not be skipped
      // as the model binder will ignore anything after a skipped number. 
      // ex: 1,2,4,5 will only bind 1 and 2 because 3 is missing.

      var mainElData = $mainEl.data(DATA_KEY);
      var nameRoot = $mainEl.attr('id');

      var maxI = mainElData.items.length;
      var lastI = maxI - 1;
      for (var i = 0; i < maxI; i++) {
        var data = mainElData.items[i];
        data.textBox.attr('name', nameRoot + '[' + i + ']');

        // Settings ids on these for regression testing.
        // There's no internal need for these for the plugin.
        // Also, using _ instead of [] here because [] aren't valid for id attributes.
        data.addButton.attr('id', nameRoot + '_' + i + '_Add');
        data.removeButton.attr('id', nameRoot + '_' + i + '_Remove');

        if (i !== lastI) {
          data.addButton.hide();
          data.removeButton.show();
        } else {
          data.addButton.show();
          data.removeButton.hide();
        }
      }
    },

    addItem: function ($mainEl) {
      var newItem = methods.createAndAddTextBox($mainEl);
      methods.reinitTextBoxes($mainEl);
      // NOTE: Focusing should only occur when the add button has been clicked, not
      // when prepopulating values.
      newItem.textBox.focus();
    },

    removeItem: function ($mainEl, item) {
      var mainElData = $mainEl.data(DATA_KEY);
      // Need to kill this thing from the array.
      var removedItemIndex = mainElData.items.indexOf(item);
      mainElData.items.splice(removedItemIndex, 1);
      item.element.remove();
      methods.reinitTextBoxes($mainEl);
    }
  };

  var multiString = function () {
    this.each(function () {
      methods.initialize($(this));
    });
  };

  $.fn.multiString = multiString;

  $(document).ready(function () {
    $('.multistring').multiString();
  });
})(jQuery);
