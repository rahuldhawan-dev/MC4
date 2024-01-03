(function($) {

  var DATA_KEY = 'collapsePanel';
  var ANIMATION_SPEED = 100;
  var methods = {
    init: function($panel) {
      if ($panel.data(DATA_KEY)) {
        return;
      }

      var data = {};

      // Add wrapper so we can detach the DOM elements.
      $panel.wrapInner('<div class="collapse-content"></div>');
      data.content = $panel.find('.collapse-content');

      var titleBar = $('<table class="collapse-title-bar"><tr>' +
          '<td class="collapse-title"><button type="button" class="collapse-trigger"></button></td>' +
          '<td class="collapse-title-buttons"><button type="button" class="open"></button></td>' +
          '<td class="collapse-title-buttons"><button type="button" class="close"></button></td>' +
          '</tr></table>');

      data.titleCloseButton = titleBar.find('.close');
      data.titleOpenButton = titleBar.find('.open');
      data.triggerButton = titleBar.find('.collapse-trigger');
      data.triggerButton.text($panel.attr('data-title'));
      data.triggerButton.click(function() { methods.toggle($panel); });
      $panel.prepend(titleBar);

      data.divider = $('<div class="collapse-title-divider"></div>');
      data.content.prepend(data.divider);

      $panel.on('click', '.close', function() { methods.close($panel); });
      $panel.on('click', '.cancel', function() { methods.close($panel); });
      $panel.on('click', '.open', function() { methods.open($panel); });

      $panel.data(DATA_KEY, data);
      methods.close($panel);
    },

    toggle: function($panel) {
      var data = $panel.data(DATA_KEY);
      if (data.isOpen) {
        methods.close($panel);
      } else {
        methods.open($panel);
      }
    },

    _animate: function($panel, endHeight, onComplete) {
      $panel.animate({
        height: endHeight
      }, {
        duration: ANIMATION_SPEED,
        queue: false,
        complete: function() {
          onComplete($panel);
        }
      });
    },

    _onClose: function($panel) {
      var data = $panel.data(DATA_KEY);
      data.content.hide();
      data.isOpen = false;
      $panel.trigger('close');
    },

    _onOpen: function($panel) {
      var data = $panel.data(DATA_KEY);
      $panel.height('auto');
      $panel.css('overflow', 'visible');
      data.isOpen = true;
      $panel.trigger('open');
    },

    close: function($panel) {
      var data = $panel.data(DATA_KEY);
      data.titleCloseButton.hide();
      data.titleOpenButton.show();
      $panel.removeClass('collapse-panel-open');
      methods._animate($panel, data.collapsedHeight, methods._onClose);
    },

    open: function($panel) {
      var data = $panel.data(DATA_KEY);
      data.titleCloseButton.show();
      data.titleOpenButton.hide();
      if (!data.fullHeight) {
        // since JS and CSS are awful, the full height
        // can't be obtained until the element is actually
        // visible.

        data.collapsedHeight = $panel.outerHeight();

        // so first, set height to auto
        $panel.height('auto');
        // then display the content
        data.content.show();
        // then actually get the full height
        data.fullHeight = $panel.outerHeight();
        // and then hide it again so we can animate.
        data.content.hide();
      }

      // When this animates, we need to set the height
      // to auto at the very end because validation errors
      // will cause it to stretch.

      $panel.height($panel.height());
      $panel.css('overflow', 'hidden');
      $panel.addClass('collapse-panel-open');
      data.content.show();

      methods._animate($panel, data.fullHeight, methods._onOpen);
    }
  };

  $.fn.collapsePanel = function() {
    this.each(function() {
      methods.init($(this));
    });
  };

  $(document).ready(function() {
    $('.collapse-panel').collapsePanel();
  });

})(jQuery);