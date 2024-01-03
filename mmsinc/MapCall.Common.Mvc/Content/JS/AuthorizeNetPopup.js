(function ($, global) {
  //////////////////////////////////CONSTANTS///////////////////////////////////
  var selectors = {
    popupScreen: '#divAuthorizeNetPopupScreen',
    popup: '#divAuthorizeNetPopup',
    frame: '#iframeAuthorizeNet',
    form: '#formAuthorizeNetPopup',
    paymentProfileId: 'input[name="PaymentProfileId"]',
    shippingAddressId: 'input[name="ShippingAddressId"]',
    token: '#Token'
  };

  var popupSizes = {
    addPayment: { width: '435px', height: '479px', eCheckEnabledHeight: '508px' },
    editPayment: { width: '435px', height: '479px' },
    addShipping: { width: '385px', height: '359px' },
    editShipping: { width: '385px', height: '359px' },
    manage: { width: '442px', height: '578px' }
  };

  var urls = {
    test: 'https://test.authorize.net/customer/',
		live: 'https://accept.authorize.net/customer/'
  };

  //////////////////////////////PRIVATE FUNCTIONS///////////////////////////////
  var parseQueryString = function (str) {
    var vars = [];
    var arr = str.split('&');
    var pair;
    for (var i = 0; i < arr.length; i++) {
      pair = arr[i].split('=');
      vars.push(pair[0]);
      vars[pair[0]] = unescape(pair[1]);
    }
    return vars;
  };

  var getHighestZIndex = function () {
    var max = 0;
    var a = document.getElementsByTagName('*');
    var z, style;
    for (var i = 0; i < a.length; i++) {
      z = 0;
      if (a[i].currentStyle) {
        style = a[i].currentStyle;
        if (style.display != "none") {
          z = parseFloat(style.zIndex);
        }
      } else if (window.getComputedStyle) {
        style = window.getComputedStyle(a[i], null);
        if (style.getPropertyValue("display") != "none") {
          z = parseFloat(style.getPropertyValue("z-index"));
        }
      }
      if (!isNaN(z) && z > max) max = z;
    }
    return Math.ceil(max);
  };

  var centerPopup = function () {
    var d = $(selectors.popup)
      .css({ left: '50%', top: '50%' });
    var left = -Math.floor(d[0].clientWidth / 2);
    var top = -Math.floor(d[0].clientHeight / 2);

    d.css({ marginLeft: left + 'px', marginTop: top + 'px' });

    if (d[0].offsetLeft < 16) {
      d.css({ left: '16px', marginLeft: '0px' });
    }
    if (d[0].offsetTop < 16) {
      d.css({ top: '16px', marginTop: '0px' });
    }
  };

  var openSpecificPopup = function (opt) {
    var $popup = $(selectors.popup);
    var $popupScreen = $(selectors.popupScreen);
    var $frame = $(selectors.frame);
    var $form = $(selectors.form);
    var $paymentProfileId = $(selectors.paymentProfileId);
    var $shippingAddressId = $(selectors.shippingAddressId);
    var size = popupSizes[opt.action];
    var url = urls[AuthorizeNetPopup.options.useTestEnvironment ? 'test' : 'live']
      + opt.action;

    $frame
      .width(size.width)
      .height(opt.action == 'addPayment' &&
              AuthorizeNetPopup.options.eCheckEnabled ?
              size.eCheckEnabledHeight : size.height);

    if (!AuthorizeNetPopup.options.skipZIndexCheck) {
      var zIndexHighest = getHighestZIndex();
      $popup.css('zIndex', zIndexHighest + 2);
      $popupScreen.css('zIndex', zIndexHighest + 1);
    }

    $form
      .attr('action', url)
      .submit();

    $paymentProfileId.val(opt.paymentProfileId || '');
    $shippingAddressId.val(opt.shippingAddressId || '');

    $popup.toggle(true);
    $popupScreen.toggle(true).click(onPopupScreenClick);
    centerPopup();
  };

  var onPopupScreenClick = function() {
    AuthorizeNetPopup.closePopup();
  };

  if (!global.AuthorizeNetPopup) global.AuthorizeNetPopup = {
    selectors: selectors,

    closePopup: function () {
      $(selectors.popupScreen + ',' + selectors.popup).toggle(false);
      $(selectors.frame).attr('src', '/Content/AuthorizeNet/empty.html');
      if (AuthorizeNetPopup.options.onPopupClosed) {
        AuthorizeNetPopup.options.onPopupClosed();
      }
    },

    openManagePopup: function () {
      openSpecificPopup({ action: 'manage' });
    },

    openAddPaymentPopup: function () {
      openSpecificPopup({ action: 'addPayment', paymentProfileId: 'new' });
    },

    openEditPaymentPopup: function (paymentProfileId) {
      openSpecificPopup({ action: "editPayment", paymentProfileId: paymentProfileId });
    },

    openAddShippingPopup: function () {
      openSpecificPopup({ action: "addShipping", shippingAddressId: "new" });
    },

    openEditShippingPopup: function (shippingAddressId) {
      openSpecificPopup({ action: "editShipping", shippingAddressId: shippingAddressId });
    },

    onReceiveCommunication: function (querystr) {
      var params = parseQueryString(querystr);
      switch (params["action"]) {
        case "successfulSave":
          AuthorizeNetPopup.closePopup();
          break;
        case "cancel":
          AuthorizeNetPopup.closePopup();
          break;
        case "resizeWindow":
          $(AuthorizeNetPopup.selectors.frame)
            .width(parseInt(params["width"]))
            .height(parseInt(params["height"]));
          centerPopup();
          break;
      }
    }
  };

  if (!AuthorizeNetPopup.options) AuthorizeNetPopup.options = {
    onPopupClosed: null,
    eCheckEnabled: true,
    skipZIndexCheck: false,
    useTestEnvironment: $('#AuthTest').val() == "True"
  };
})(jQuery, this);
