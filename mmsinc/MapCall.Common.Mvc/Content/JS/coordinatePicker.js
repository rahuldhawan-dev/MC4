var CoordinatePicker = {
  SELECTORS: {
    pickerIcon: 'img.coordinate-picker-icon',
    displayIcon: 'img.coordinate-display-icon',
    pickerDiv: 'div#pickerDiv',
    pickerFrame: 'iframe#pickerFrame',
    manualEntryButton: '#coordinateManualEntryButton'
  },

  initialize: function () {
    $(CoordinatePicker.SELECTORS.pickerIcon).click(CoordinatePicker.coordinatePickerIcon_click);
    $(CoordinatePicker.SELECTORS.displayIcon).click(CoordinatePicker.coordinateDisplayIcon_click);
    $(CoordinatePicker.SELECTORS.manualEntryButton).click(CoordinatePicker.manualEntryButton_click);
    var validator = $('form').data('validator');
    if (validator) {
        validator.settings.ignore = '';
    }
  },

  close: function () {
    $('div.jqmOverlay').remove();
    // Make sure to call jqmHide because it needs to clean up
    // event handlers(like those that prevent focus on elements
    // outside of the modal dialog).
    $(CoordinatePicker.SELECTORS.pickerDiv).jqmHide();
    $(CoordinatePicker.SELECTORS.pickerDiv).remove();
    // "What's this for?" you might ask.
    // IE8 in some very weird setup that NJAW uses
    // will cause all textboxes on the page to become
    // disabled when the iframe a) navigates to a new page
    // and b) the iframe is closed. This has something
    // to do with focused elements. 
    //
    // http://stackoverflow.com/a/13476052/152168

    var tb = $('<input type="textbox" />');
    $('body').append(tb);
    tb.focus();
    tb.remove();
  },

  createPickerDiv: function (coordinateUrl, titleText) {
    var div = $('<div id="pickerDiv" class="jqmWindow">' +
      '<div class="jqmTitle">' +
      '<button class="jqmClose">Close X</button>' +
      '<span class="jqmTitleText">' + titleText + '</span>' +
      '</div>' +
      '<iframe id="pickerFrame" class="jqmContent" src="' + coordinateUrl + '"></iframe>' +
      '</div>');
    $(document.body).append(div);
    return div;
  },

  getPickerDiv: function (coordinateUrl, titleText) {
    var div = $(CoordinatePicker.SELECTORS.pickerDiv);
    return div.length ? div : CoordinatePicker.createPickerDiv(coordinateUrl, titleText);
  },

  coordinatePickerIcon_click: function (e) {
      var target = $(e.target);
      // TODO: This data-address-field stuff needs to be incorporated
      //       into the CoordinatePickerBuilder if it gets used more
      //       than once. I'll gladly do it when the time comes. -Ross
      var coordinateUrl = target.attr('coordinateUrl');
      if (coordinateUrl && coordinateUrl != '') {
          var addressFieldId = target.attr('data-address-field');
          var addressCallback = target.attr('data-address-callback');
          var address = null;

          if (addressFieldId) {
              address = $('#' + addressFieldId).val();
          }
          else if (addressCallback) {
              // This should only be a method name, so we need to invoke it as a function
              // after eval.
              address = eval(addressCallback)();
          }

          if (address) {
              coordinateUrl = coordinateUrl + '&Address=' + encodeURIComponent(address);
          }
      }
      CoordinatePicker.getPickerDiv(coordinateUrl, 'Select a location:')
      .jqm({ modal: true }).jqmShow();
    $(CoordinatePicker.SELECTORS.manualEntryButton).show();
  },

  coordinateDisplayIcon_click: function (e) {
    var coordinateUrl = $(e.target).attr('coordinateUrl');
    if (coordinateUrl && coordinateUrl != '') {
      CoordinatePicker.getPickerDiv(coordinateUrl, 'Current location:')
        .jqm({ modal: true }).jqmShow();
    }
  },

  manualEntryButton_click: function (e) {
    if (confirm('This should only be used when the map will not load!')) {
      var target = $(e.target);
      var coordinateUrl = target.attr('coordinateUrl');
      if ($(CoordinatePicker.SELECTORS.pickerFrame).attr('src') != coordinateUrl) {
        $(CoordinatePicker.SELECTORS.pickerDiv).remove();
      }
      CoordinatePicker.getPickerDiv(coordinateUrl, 'Enter a location:')
        .jqm({ modal: true })
        .jqmShow();
    }
    return false;
  }
};

$(document).ready(CoordinatePicker.initialize);
