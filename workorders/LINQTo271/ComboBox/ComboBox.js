/// <reference path="../../../MMSINC/MapCall.Common/Resources/scripts/jquery.js" />

var ComboBox = {
/// <summary>Wrapper object containing all necessary client functionality for
/// the ComboBox control.</summary>
  keyCodes: {
    TAB: 9,
    ESC: 27,
    UP: 38,
    DOWN: 40
  },

  cancelEvent: function(e) {
    e.stopPropagation();
    e.cancelBubble = true;
  },

  getKeyCode: function(event) {
    return (event.which === undefined || event.which === 0) ?
      event.keyCode : event.which;
  },

  Button: {
    imageUrls: {
      NORMAL: '/includes/ddlArrowNormal.png',
      PRESSED: '/includes/ddlArrowPressed.png'
    },

    mouseDown: function(img) {
      img[0].src = ComboBox.Button.imageUrls.PRESSED;
    },

    mouseUp: function(img) {
      img[0].src = ComboBox.Button.imageUrls.NORMAL;
    },

    click: function(select) {
      ComboBox.Options.toggle(select);
    }
  },

  Options: {
    toggle: function(elem) {
      elem.toggle();
      if (this.isVisible(elem)) {
        elem[0].focus();
      }
    },

    cancel: function(elem) {
      elem.hide();
    },

    change: function(elem, hidVal, hidText, txt) {
      // set the visible text box to the selected text
      txt.val($('#' + elem[0].id + ' option:selected').text());
      // set the hidden text field to the selected text
      hidText.val(txt.val());
      // set the hidden value field to the selected value
      hidVal.val(elem.val());
      // toggle the select
      this.toggle(elem);
    },

    moveUp: function(elem) {
      elem.show();
      elem[0].selectedIndex--;
      if (elem[0].selectedIndex == -1) {
        elem[0].selectedIndex = elem[0].options.length - 1;
      }
    },

    moveDown: function(elem) {
      elem.show();
      elem[0].selectedIndex++;
      if (elem[0].selectedIndex == elem[0].options.length) {
        elem[0].selectedIndex = 0;
      }
    },

    trySelectByText: function(elem, text) {
      var opt = null;
      for (var i = 0, len = elem[0].options.length; i < len; ++i) {
        opt = elem[0].options[i];
        if (opt.text.toLowerCase().indexOf(text.toLowerCase()) >= 0) {
          opt.selected = true;
          return;
        }
      }
    },

    parseText: function(elem, text) {
      elem.show();
      ComboBox.Options.trySelectByText(elem, text);
    },

    isVisible: function(elem) {
      return elem[0].style.display != 'none';
    }
  },

  TextBox: {
    change: function(txt, hidText) {
      hidText.val(txt.val());
    },

    keyUp: function(e, txt, select) {
      var keyCode = ComboBox.getKeyCode(e);
      switch (keyCode) {
        case ComboBox.keyCodes.ESC:
          ComboBox.Options.cancel(select);
          return false;
        case ComboBox.keyCodes.UP:
          ComboBox.Options.moveUp(select);
          return false;
        case ComboBox.keyCodes.DOWN:
          ComboBox.Options.moveDown(select);
          return false;
        default:
          ComboBox.Options.parseText(select, txt.val());
          return true;
      }
    },

    keyDown: function(e, select, hidVal, hidText, txt) {
      if (ComboBox.getKeyCode(e) === ComboBox.keyCodes.TAB &&
          ComboBox.Options.isVisible(select)) {
        ComboBox.Options.change(select, hidVal, hidText, txt);
      }

      return true;
    },

    blur: function(e, txt, select) {
      if (ComboBox.Options.isVisible(select)) {
        ComboBox.cancelEvent(e);
        return false;
      } else {
        return true;
      }
    }
  }
};
